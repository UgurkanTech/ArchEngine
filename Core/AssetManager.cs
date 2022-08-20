using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Camera;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor;
using Assimp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using Camera = ArchEngine.Core.Rendering.Camera.Camera;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
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

        public static WindowIcon LoadWindowIconFromFile(string path)
        {
            using var image = new Bitmap(path);
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
            var assimpContext = new Assimp.AssimpContext();
            var assimpScene = assimpContext.ImportFile(filePath,
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
            string path = @"D:\save.json";
            Serializer.Save(Window.activeScene, path);
            _log.Info("Scene saved! (" + path + ")");
        }

        public static void LoadScene()
        {
            _log.Info("Closing current scene..");
            Window.activeScene.Dispose();

            _log.Info("Loading new scene..");
            Window.activeScene = Serializer.Load<Scene>("");
            _log.Info("Initializing new scene..");
            RestoreScene();
            Window.activeScene.Init();
            _log.Info("Starting new scene..");
            Window.activeScene.Start();
            Editor.selectedGameobject = null;
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