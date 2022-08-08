using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Camera;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Mathematics;

namespace ArchEngine.Core
{
    public class EditorScene : Scene
    {
        public EditorScene()
        {
            Material mat = new Material();
            mat.LoadTextures("Resources/Textures/wall");
            mat.Shader = ShaderManager.PbrShader;
            
            MeshRenderer mr = new MeshRenderer();
            mr.mesh = new Cube();
            mr.mesh.Material = mat;
            
            
            MeshRenderer mr2 = new MeshRenderer();
            mr2.mesh = new Cube();
            mr2.mesh.Material = mat;
            
            GameObject gm = new GameObject("Cube");
            GameObject gm2 = new GameObject("Cube2");
            GameObject gm3 = new GameObject("Cube3");
            GameObject gm4 = new GameObject("Cube-1");
            GameObject gm5 = new GameObject("Cube-2");
            
            GameObject gm7 = new GameObject("Camera");
            
            
            gm2.Transform = Matrix4.CreateTranslation(new Vector3(2f, 0f, 0)) * Matrix4.CreateScale(.7f);
            
            gm.AddComponent(mr);
            gm2.AddComponent(mr);
            gm3.AddComponent(mr);
            gm4.AddComponent(mr2);
            gm5.AddComponent(mr2);

            AddGameObject(gm);
            AddGameObject(gm2);
            AddGameObject(gm3);
            gm.AddComponent(gm4);
            gm4.AddComponent(gm5);
            
            gm7.AddComponent(CameraManager.activeCamera);
            gm7.Transform = Matrix4.CreateTranslation(0, 0, 5);
            
            AddGameObject(gm7);
            gm7.AddComponent(new ACustomScript());
        }
    }
}