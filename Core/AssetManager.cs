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
                PostProcessSteps.JoinIdenticalVertices | PostProcessSteps.ValidateDataStructure | PostProcessSteps.CalculateTangentSpace | PostProcessSteps.GenerateNormals |
                PostProcessSteps.Triangulate | PostProcessSteps.FixInFacingNormals   | PostProcessSteps.FlipUVs 
                  );

            //if (assimpScene.MeshCount > 1) throw new NotSupportedException("single meshes supported.");
            var assimpMesh = assimpScene.Meshes.First();

            var verts = assimpMesh.Vertices.ToArray();
            var faces = assimpMesh.Faces.ToArray();
            var uvs = assimpMesh.TextureCoordinateChannels[0].ToArray();
            var normals = assimpMesh.Normals.ToArray();
            var tangents = assimpMesh.Tangents.ToArray();
            var bitangents = assimpMesh.BiTangents.ToArray();

            List<float> vertList = new List<float>();
            List<int> indicesList = new List<int>();
            for (int i = 0; i < verts.Length; i++)
            {
                vertList.Add(verts[i].X);
                vertList.Add(verts[i].Y);
                vertList.Add(verts[i].Z);
                
                vertList.Add(uvs[i].X);
                vertList.Add(uvs[i].Y);
                
                vertList.Add(normals[i].X);
                vertList.Add(normals[i].Y);
                vertList.Add(normals[i].Z);
                
                vertList.Add(tangents[i].X);
                vertList.Add(tangents[i].Y);
                vertList.Add(tangents[i].Z);
                
                vertList.Add(bitangents[i].X);
                vertList.Add(bitangents[i].Y);
                vertList.Add(bitangents[i].Z);
                
            }
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
            Console.WriteLine("imported model verts: " + vertList.Count + " - indices: " + indicesList.Count);
            vertList.Clear();
            indicesList.Clear();
            return mesh;
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