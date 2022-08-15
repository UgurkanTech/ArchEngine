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
        }

        public Scene AddDemo()
        {
            Material mat = new Material();
            mat.LoadTextures("Resources/Textures/wall");
            mat.Shader = ShaderManager.PbrShader;
            
            Material mat2 = new Material();
            mat2.Shader = ShaderManager.ColorShader;
            
            MeshRenderer mr = new MeshRenderer();
            mr.mesh = new Cube();
            mr.Material = mat;
            
            
            MeshRenderer mr2 = new MeshRenderer();
            mr2.mesh = new Cube();
            mr2.Material = mat;
            
            MeshRenderer mr3 = new MeshRenderer();
            mr3.mesh = new Cube();
            mr3.Material = mat;
            
            MeshRenderer mr4 = new MeshRenderer();
            mr4.mesh = new Cube();
            mr4.Material = mat;
            
            MeshRenderer mr5 = new MeshRenderer();
            mr5.mesh = new Cube();
            mr5.Material = mat;
            
            LineRenderer mr7 = new LineRenderer();
            mr7.line = new Line(Vector3.Zero, Vector3.One);
            mr7.Material = mat2;
            
            GameObject gm = new GameObject("Cube");
            GameObject gm2 = new GameObject("Cube2");
            GameObject gm3 = new GameObject("Cube3");
            GameObject gm4 = new GameObject("Cube-1");
            GameObject gm5 = new GameObject("Cube-2");
            
            
            
            
            GameObject gm8 = new GameObject("Line");
            
            
            
            gm2.Transform = Matrix4.CreateTranslation(new Vector3(2f, 0f, 0)) * Matrix4.CreateScale(.7f);
            
            gm.AddComponent(mr);
            gm2.AddComponent(mr2);
            gm3.AddComponent(mr3);
            gm4.AddComponent(mr4);
            gm5.AddComponent(mr5);

            AddGameObject(gm);
            AddGameObject(gm2);
            AddGameObject(gm3);
            
            gm.AddChild(gm4);
            gm4.AddChild(gm5);
            
            
            gm8.AddComponent(new ACustomScript());
            
            
            AddGameObject(gm8);
            gm8.Transform = Matrix4.CreateTranslation(-3, 0, 0);
            gm8.AddComponent(mr7);
            return this;
        }
    }
}