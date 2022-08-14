using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Camera;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.Core.Utils;
using Assimp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using Polenter.Serialization;
using Camera = ArchEngine.Core.Rendering.Camera.Camera;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;
using Scene = ArchEngine.Core.ECS.Scene;

namespace ArchEngine.Core
{
    public class AssetManager
    {
        public static Texture cube;
        
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
        
        public void GetMeshByFilePath(string filePath)
        {
            //if (_meshCache.ContainsKey(filePath)) return _meshCache[filePath];

            var assimpContext = new Assimp.AssimpContext();
            var assimpScene = assimpContext.ImportFile(filePath, PostProcessSteps.GenerateNormals | PostProcessSteps.GenerateUVCoords | PostProcessSteps.Triangulate);

            if (assimpScene.MeshCount > 1) throw new NotSupportedException("single meshes supported.");

            var assimpMesh = assimpScene.Meshes.First();

        
        
            //assimpMesh.MaterialIndex
            // _meshCache.Add(filePath, modelMesh);
            //return modelMesh;
        }

        public static void SaveScene()
        {
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(@"D:\path.json"))
            using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
            {
                serializer.Serialize(writer, Window.activeScene, typeof(Scene));
            }

            //File.WriteAllText(@"D:\path.json", json);
            //Console.WriteLine(Serializer.Serialize(json));

        }

        public static void LoadScene()
        {
            Window.activeScene.Dispose();
            
            Window.activeScene  = Newtonsoft.Json.JsonConvert.DeserializeObject<Scene>(File.ReadAllText(@"D:\path.json"),
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Error = (sender, eventArgs) => {
                        Console.WriteLine(eventArgs.ErrorContext.Error.Message);
                        eventArgs.ErrorContext.Handled = true;
                    }
                });
            RestoreScene();
            Window.activeScene.Init();
            Window.activeScene.Start();
        }

        public static void RestoreScene()
        {
            Window.activeScene.gameObjects.ForEach(o =>
            {
                RestoreParents(null, o);
            });
            Window.activeScene.GameObjectFind("Camera").RemoveComponent<Camera>();
            
            CameraManager.Init(Window.WindowSize.X / (float)Window.WindowSize.Y);
            
            Window.activeScene.GameObjectFind("Camera").AddComponent(CameraManager.activeCamera);

        }

        public static void RestoreParents(GameObject parent, GameObject child)
        {
            //child.Transform = Matrix4.Identity;
            child.initialized = false;
            child.parent = parent;
            child._components.ForEach(component =>
            {
                component.gameObject = child;
                component.initialized = false;
            });
            child._childs.ForEach(childs =>
            {
                childs.initialized = false;
                //childs.Transform = Matrix4.Identity;
                RestoreParents(child, childs);
            });
        }
        //[JsonConverter(typeof(TimeSpanConverter))]
        

    }
}