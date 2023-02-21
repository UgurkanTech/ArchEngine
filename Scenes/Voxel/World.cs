using ArchEngine.Core.ECS;
using System;
using System.Diagnostics;
using ArchEngine.Core;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
namespace ArchEngine.Scenes.Voxel
{
    public class World : Script
    {
        public override void Start()
        {
            Material mat = new Material();
            mat.Shader = ShaderManager.PbrShader;
            mat.LoadAlbedo("Resources/Textures/tiles", TextureMagFilter.Nearest, TextureMinFilter.Nearest);
            
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Window.activeScene.AddGameObject(GenerateWorldChunk(mat, new Vector3(11 * 1.6f, -9f, 11 * 1.6f)));
            sw.Stop();
            Console.WriteLine("\n\n");
            Console.WriteLine("First run: " + sw.ElapsedMilliseconds);
            sw.Reset();
            
            sw.Start();
            for (int i = -5; i < 10; i++)
            {
                for (int j = -5; j < 10; j++)
                {
                    Window.activeScene.AddGameObject(GenerateWorldChunk(mat, new Vector3(i * 1.6f, -9f, j * 1.6f)));
                }
            }
            sw.Stop();
            long time = sw.ElapsedMilliseconds;
            Console.WriteLine("World gen :" +  time + "ms (" + time/100f + " each)");
            
            
        }
        
        public static GameObject GenerateWorldChunk(Material mat, Vector3 pos)
        {
            MeshRenderer mesh = new MeshRenderer();
            mesh.Material = mat;
            mesh.mesh = GenerateChunkMesh(pos * 10);
            
            GameObject go = new GameObject("Chunk" + (pos.Xz * 10));
            go.Transform = Matrix4.CreateScale(.1f);
            go.Transform *= Matrix4.CreateTranslation(pos);
            go.AddComponent(mesh);
            return go;
        }

        public static Mesh GenerateChunkMesh(Vector3 pos)
        {
            Block[] blocks = null;
            BlockGenerator.generateBlocks(ref blocks, pos.X, pos.Y, pos.Z);
            float[] verts = null;
            int[] indices = null;

            MeshGenerator.generatePBRMesh(ref blocks, ref verts, ref indices);
            
            Mesh mesh = new Mesh();
            mesh.Vertices = verts;
            mesh.Indices = indices;

            return mesh;
        }
        
        
        
    }
}