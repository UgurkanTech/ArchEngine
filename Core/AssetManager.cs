using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor;
using Assimp;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using Mesh = ArchEngine.Core.Rendering.Geometry.Mesh;
using Scene = ArchEngine.Core.ECS.Scene;

namespace ArchEngine.Core
{
    public class AssetManager
    {
        public static Texture cube;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType.Name);

        public static void LoadEditor()
        {
            cube = TextureManager.LoadFromFile("Resources/Textures/Editor/cube2.png", TextureUnit.Texture0, false);


        }
        
        public static WindowIcon LoadWindowIcon(Stream stream)
        {
            using var image = new Bitmap(stream);
            var data = image.LockBits(
                new Rectangle(0, 0, image.Width, image.Height), 
                ImageLockMode.ReadOnly, 
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            
            byte[] data2 = new byte[Math.Abs(data.Stride * data.Height)];
            Marshal.Copy(data.Scan0, data2, 0, data2.Length);
            
            OpenTK.Windowing.Common.Input.Image[] images = new OpenTK.Windowing.Common.Input.Image[1];
            images[0] = new OpenTK.Windowing.Common.Input.Image(image.Width, image.Height, data2);
            
            return new WindowIcon(images);

        }

        public static Mesh GetMeshByFilePath(string filePath)
        {
            Stream stream;
            if (filePath.Contains(":"))
            {
                stream = new ResourceStream(filePath).GetStream();
            }
            else
            {
                stream = new ResourceStream(filePath, null).GetStream();
            }

            if (stream == null)
            {
                Console.WriteLine("Mesh not found: " + filePath);
                return null;
            }
            
            var assimpContext = new Assimp.AssimpContext();
            
            var assimpScene = assimpContext.ImportFileFromStream(stream,
  PostProcessSteps.GenerateSmoothNormals | PostProcessSteps.SortByPrimitiveType | PostProcessSteps.OptimizeGraph | PostProcessSteps.OptimizeMeshes | 
                 PostProcessSteps.ValidateDataStructure | PostProcessSteps.CalculateTangentSpace  | PostProcessSteps.GenerateUVCoords |
                PostProcessSteps.Triangulate    | PostProcessSteps.FlipUVs 
                  );

            if (assimpScene == null)
            {
                Console.WriteLine("Invalid Model. No meshes found! - " + filePath);
                return null;
            }

           
            //if (assimpScene.MeshCount > 1) throw new NotSupportedException("single meshes supported.");
            var assimpMesh = assimpScene.Meshes.First();

            var verts = assimpMesh.Vertices.ToArray();
            var faces = assimpMesh.Faces.ToArray();
            var uvs = assimpMesh.TextureCoordinateChannels[0].ToArray();
            var normals = assimpMesh.Normals.ToArray();

            if (uvs.Length == 0)
            {
                uvs = new Vector3D[verts.Length * 3 / 2];
                for (int u = 0; u < uvs.Length; u++)
                {
                    uvs[u] = new Vector3D(0, 0, 0);
                }
            }

            
            List<float> vertList = new List<float>();
            for (int i = 0; i < verts.Length; i += 3)
            {

                       // Get vertex positions of the polygon
                    Vector3 v0 = verts[i].ToVector3TK();
                    Vector3 v1 = verts[i + 1].ToVector3TK();
                    Vector3 v2 = verts[i + 2].ToVector3TK();

                    // Get texture coordinates of the polygon
                    Vector2 uv0 = uvs[i].ToVector2TK();
                    Vector2 uv1 = uvs[i + 1].ToVector2TK();
                    Vector2 uv2 = uvs[i + 2].ToVector2TK();

                    // Get normals of the polygon
                    Vector3 n0 = normals[i].ToVector3TK();
                    Vector3 n1 = normals[i + 1].ToVector3TK();
                    Vector3 n2 = normals[i + 2].ToVector3TK();

                    // Calculate position difference
                    Vector3 deltaPos1 = v1 - v0;
                    Vector3 deltaPos2 = v2 - v0;

                    // Calculate texture coordinate difference
                    Vector2 deltaUV1 = uv1 - uv0;
                    Vector2 deltaUV2 = uv2 - uv0;

                    // Calculate tangent and bitangent
                    float r = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV1.Y * deltaUV2.X);
                    Vector3 tangent = (deltaPos1 * deltaUV2.Y - deltaPos2 * deltaUV1.Y) * r;
                    Vector3 bitangent = (deltaPos2 * deltaUV1.X - deltaPos1 * deltaUV2.X) * r;

                    // Orthogonalize using Gram–Schmidt process, to make tangents and bitangents smooth based on normal
                    Vector3 t0 = tangent - n0 * Vector3.Dot(n0, tangent);
                    Vector3 t1 = tangent - n1 * Vector3.Dot(n1, tangent);
                    Vector3 t2 = tangent - n2 * Vector3.Dot(n2, tangent);

                    t0 = Vector3.Normalize(t0);
                    t1 = Vector3.Normalize(t1);
                    t2 = Vector3.Normalize(t2);
                    

                    Vector3 b0 = bitangent - n0 * Vector3.Dot(n0, bitangent);
                    Vector3 b1 = bitangent - n1 * Vector3.Dot(n1, bitangent);
                    Vector3 b2 = bitangent - n2 * Vector3.Dot(n2, bitangent);

                    b0 = Vector3.Normalize(b0);
                    b1 = Vector3.Normalize(b1);
                    b2 = Vector3.Normalize(b2);


                        vertList.Add(v0.X);
                        vertList.Add(v0.Z);
                        vertList.Add(v0.Y);
                        vertList.Add(uv0.X);
                        vertList.Add(uv0.Y);
                        vertList.Add(n0.X);
                        vertList.Add(n0.Z);
                        vertList.Add(n0.Y);
                        vertList.Add(t0.X);
                        vertList.Add(t0.Z);
                        vertList.Add(t0.Y);
                        vertList.Add(b0.X);
                        vertList.Add(b0.Z);
                        vertList.Add(b0.Y);

                        vertList.Add(v1.X);
                        vertList.Add(v1.Z);
                        vertList.Add(v1.Y);
                        vertList.Add(uv1.X);
                        vertList.Add(uv1.Y);
                        vertList.Add(n1.X);
                        vertList.Add(n1.Z);
                        vertList.Add(n1.Y);
                        vertList.Add(t1.X);
                        vertList.Add(t1.Z);
                        vertList.Add(t1.Y);
                        vertList.Add(b1.X);
                        vertList.Add(b1.Z);
                        vertList.Add(b1.Y);
                    
                        vertList.Add(v2.X);
                        vertList.Add(v2.Z);
                        vertList.Add(v2.Y);
                        vertList.Add(uv2.X);
                        vertList.Add(uv2.Y);
                        vertList.Add(n2.X);
                        vertList.Add(n2.Z);
                        vertList.Add(n2.Y);
                        vertList.Add(t2.X);
                        vertList.Add(t2.Z);
                        vertList.Add(t2.Y);
                        vertList.Add(b2.X);
                        vertList.Add(b2.Z);
                        vertList.Add(b2.Y);
                    

                
            }

            List<int> indicesList = new List<int>();
            for (int i = 0; i < faces.Length; i++)
            {
                for (int j = 0; j < faces[i].Indices.Count; j++)
                {
                    indicesList.Add(faces[i].Indices[j]);
                }
            }

            Mesh mesh = new Mesh();
            mesh.Vertices = vertList.ToArray();
            mesh.Indices = indicesList.ToArray();
            mesh.MeshHash = filePath;
            Console.WriteLine("imported model verts: " + vertList.Count + " - indices: " + indicesList.Count + " - Has tangents:" + assimpMesh.HasTangentBasis + " - Has Normals:" + assimpMesh.HasNormals);
            vertList.Clear();
            indicesList.Clear();
            return mesh;
        }

        private static (Vector3 tangent, Vector3 bitangent) CalculateTangentAndBitangent(
            Vector3 v1, Vector3 v2, Vector3 v3,
            Vector2 uv1, Vector2 uv2, Vector2 uv3,
            Vector3 n1, Vector3 n2, Vector3 n3)
        {
        // Calculate the tangent and bitangent for a single triangle
            var deltaPos1 = v2 - v1;
            var deltaPos2 = v3 - v1;
            var deltaUV1 = uv2 - uv1;
            var deltaUV2 = uv3 - uv1;
            float r = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV1.Y * deltaUV2.X);

            var tangent = new Vector3(
                (deltaPos1.X * deltaUV2.Y - deltaPos2.X * deltaUV1.Y) * r,
                (deltaPos1.Y * deltaUV2.Y - deltaPos2.Y * deltaUV1.Y) * r,
                (deltaPos1.Z * deltaUV2.Y - deltaPos2.Z * deltaUV1.Y) * r);

            var bitangent = new Vector3(
                (deltaPos2.X * deltaUV1.X - deltaPos1.X * deltaUV2.X) * r,
                (deltaPos2.Y * deltaUV1.X - deltaPos1.Y * deltaUV2.X) * r,
                (deltaPos2.Z * deltaUV1.X - deltaPos1.Z * deltaUV2.X) * r);

// Use vertex normal to calculate handedness of tangent and bitangent
            var n = Vector3.Normalize(n1 + n2 + n3);
            var hand = Vector3.Dot(Vector3.Cross(n, tangent), bitangent) < 0.0f ? -1.0f : 1.0f;

            tangent *= hand;
            bitangent *= hand;

            return (tangent, bitangent);
        }

        public static void SaveScene()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Scene File|*.arch";
            saveFileDialog1.Title = "Save the scene";
            saveFileDialog1.ShowDialog();

            string path = "";
            if (saveFileDialog1.FileName != "")
            {
                path = saveFileDialog1.FileName;
            }
            else
            {
                return;
            }

            try
            {
                
                Serializer.Save(Window.activeScene, path);
                _log.Info("Scene saved! (" + path + ")");
            }
            catch (UnauthorizedAccessException e)
            {
                Window._log.Error("Scene saving failed. File access is denied.");
            }
            catch (Exception e)
            {
                Window._log.Error(e.ToString());
                Window._log.Error("Scene saving failed.");
            }

        }

        public static void LoadScene()
        {
            OpenFileDialog file = new OpenFileDialog();  
            file.Filter = "Scene File|*.arch";
            file.RestoreDirectory = true;  
            file.CheckFileExists = false;  
            file.Title = "Load the scene";  
            file.ShowDialog();

            string path = file.FileName;
            if (path == "")
            {
                return;
            }
            try
            {
                Editor.state = Editor.EditorState.Loading;
                _log.Info("Closing current scene..");
                Editor.selectedGameobject = null;
                Window.activeScene.Dispose();
                _log.Info("Loading new scene..");
                
                Window.activeScene = Serializer.Load<Scene>(path);
                _log.Info("Initializing new scene..");
                RestoreScene();
                Window.activeScene.Init();
                _log.Info("Ready!");
                Editor.state = Editor.EditorState.Idle;
                //Window.activeScene.Start();

                
            }
            catch (Exception e)
            {
                Window._log.Error("Scene loading failed.");
            }

        }

        public static void RestoreScene()
        {
            Window.activeScene.gameObjects.ForEach(o =>
            {
                RestoreParents(null, o);
            });
            //Window.activeScene.GameObjectFind("Camera").RemoveComponent<Camera>();
            
            //CameraManager.Init(Window.WindowSize.X / (float)Window.WindowSize.Y);
            
           // Window.activeScene.GameObjectFind("Camera").AddComponent(CameraManager.EditorCamera);

        }

        public static void RestoreParents(GameObject parent, GameObject child)
        {
            //child.Transform = Matrix4.Identity;
            //child.initialized = false;
            child.parent = parent;
            child._components.ForEach(component =>
            {
                component.gameObject = child;
                //component.initialized = false;
            });
            child._childs.ForEach(childs =>
            {
                //childs.initialized = false;
                //childs.Transform = Matrix4.Identity;
                RestoreParents(child, childs);
            });
        }
        //[JsonConverter(typeof(TimeSpanConverter))]
        

    }
}