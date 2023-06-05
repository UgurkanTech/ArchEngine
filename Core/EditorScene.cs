using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Physics;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Lighting;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Mathematics;

namespace ArchEngine.Core
{
    
    public class EditorScene : Scene
    {
        public EditorScene()
        {
            //BackpackDemo();
            //return;
            Material m = new Material();
            m.Shader = ShaderManager.PbrShader;
            
            
            MeshRenderer mr2 = new MeshRenderer();
            mr2.Material = m;

           // mr2.mesh = AssetManager.GetMeshByFilePath("Resources/Models/cube.fbx");

            
            GameObject gm = new GameObject("Ground");
            gm.Transform = Matrix4.CreateScale(15,0.1f,15);
            gm.Transform *= Matrix4.CreateTranslation(0, -6, 0);
            
            RigidObject ro = new RigidObject(true,false,0, true);
            
            gm.AddComponent(mr2);

            m.LoadTextures("grass");
            gm.AddComponent(ro);
            AddGameObject(gm);
            
            GameObject light = new GameObject("Orange Light");
            light.Transform = Matrix4.CreateTranslation(-8,-2,-4);
            PointLight pl = new PointLight();
            pl.Color = Color4.OrangeRed;
            pl.Intensity = 1f;
            light.AddComponent(pl);
            AddGameObject(light);
            
            GameObject light2 = new GameObject("Cyan Light");
            light2.Transform = Matrix4.CreateTranslation(4,-2,8);
            PointLight pl2 = new PointLight();
            pl2.Color = Color4.Cyan;
            pl2.Intensity = 1f;
            light2.AddComponent(pl2);
            AddGameObject(light2);

            GameObject backpack = new GameObject("Backpack");
            Material mat = new Material();
            mat.Shader = ShaderManager.PbrShader;
            mat.LoadTextures("backpack");
            MeshRenderer mr = new MeshRenderer();
            mr.mesh = AssetManager.GetMeshByFilePath("Resources/Models/backpack.fbx");
            mr.Material = mat;
            backpack.AddComponent(mr);
            backpack.Transform = Matrix4.CreateTranslation(-4, 4, 0);
            RigidObject rigid = new RigidObject(false,true,10, false);
            backpack.AddComponent(rigid);
            AddGameObject(backpack);
            
            
            Material m2 = new Material();
            m2.Shader = ShaderManager.PbrShader;
            m2.LoadTextures("space");

            int id = 0;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        gm = new GameObject("Cube - " + id++);
                        gm.Transform = Matrix4.CreateScale(0.8f,0.8f,0.8f);
                        gm.Transform *= Matrix4.CreateTranslation(i*2, 2 + k * 2, j *2);
            
                        mr2 = new MeshRenderer();
                        mr2.Material = m2;
                        gm.AddComponent(mr2);
                    
                        ro = new RigidObject(false,true,1, false);
                        gm.AddComponent(ro);
                    
                    
                        AddGameObject(gm);
                    }
                }
            }
            
        }

        public void BackpackDemo()
        {
            Material m = new Material();
            m.Shader = ShaderManager.PbrShader;
            m.LoadTextures("backpack");
            MeshRenderer mr2 = new MeshRenderer();
            mr2.Material = m;

            mr2.mesh = AssetManager.GetMeshByFilePath("Resources/Models/backpack.fbx");

            GameObject gm = new GameObject("Backpack");
            
            RigidObject ro = new RigidObject(true,false,0, true);
            
            gm.AddComponent(ro);

            m.LoadTextures("backpack");
            gm.AddComponent(mr2);
            AddGameObject(gm);
            
        }

        public Scene AddDemo2()
        {
            Material m = new Material();
            m.Shader = ShaderManager.PbrShader;
            m.LoadTextures("Resources/Textures/plastic");
            MeshRenderer mr2 = new MeshRenderer();
            mr2.Material = m;
            //mr2.mesh = AssetManager.GetMeshByFilePath("Resources/Models/f16.fbx");
            mr2.mesh = AssetManager.GetMeshByFilePath("Resources/Models/f16.fbx");
            
            GameObject gm = new GameObject("Backpack");
            gm.AddComponent(mr2);
            AddGameObject(gm);
            
            Matrix4 mat = Matrix4.Identity;
            
            mat *= Matrix4.CreateScale( 0.5f);
            mat *= Matrix4.CreateRotationX(0f);
            mat *= Matrix4.CreateTranslation(new Vector3(0,0,0));

            gm.Transform = mat;
            
            return this;
        }
        
        public Scene AddDemo()
        {
            
            MeshRenderer mr = new MeshRenderer();

            MeshRenderer mr2 = new MeshRenderer();
    
            MeshRenderer mr3 = new MeshRenderer();
     
            
            MeshRenderer mr4 = new MeshRenderer();
     
            MeshRenderer mr5 = new MeshRenderer();
         
            
            LineRenderer mr7 = new LineRenderer();

            
            
            
            LineRenderer mr8 = new LineRenderer();
            mr8.StartPos = Vector3.One;
            mr8.EndPos = Vector3.One + Vector3.One;

            
            GameObject gm = new GameObject("Cube");
            GameObject gm2 = new GameObject("Cube2");
            GameObject gm3 = new GameObject("Cube3");
            GameObject gm4 = new GameObject("Cube-1");
            GameObject gm5 = new GameObject("Cube-2");
            
            
            GameObject gm8 = new GameObject("Line");
            GameObject gm9 = new GameObject("Line2");
            
            
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
            
            
            //gm2.AddComponent(new ACustomScript());
            
            
            AddGameObject(gm8);
            gm8.Transform = Matrix4.CreateTranslation(-3, 0, 0);
            gm8.AddComponent(mr7);
            
            
            AddGameObject(gm9);
            gm9.Transform = Matrix4.CreateTranslation(-4, 0,0);
            gm9.AddComponent(mr8);
            
            
            return this;
        }
    }
}