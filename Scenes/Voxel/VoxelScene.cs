using System;
using System.Collections.Generic;
using System.Diagnostics;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchEngine.Scenes.Voxel
{
    public class VoxelScene : Scene
    {
        public VoxelScene()
        {
            Material mat = new Material();
            mat.Shader = ShaderManager.PbrShader;
            mat.LoadAlbedo("Resources/Textures/tiles", TextureMagFilter.Nearest, TextureMinFilter.Nearest);

            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    AddGameObject(GenerateWorldChunk(mat, new Vector3(i * 1.6f, -9f, j * 1.6f)));
                }
            }
            sw.Stop();
            Console.WriteLine("World gen:" + sw.ElapsedMilliseconds);
            
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
            Block[] blocks = BlockGenerator.generateBlocks(pos.X, pos.Y, pos.Z);
            Vector3[] verts = null;
            Vector2[] uvs = null;
            int[] indices = null;
            MeshGenerator.generateMesh(ref blocks, ref verts, ref indices, ref uvs);

            float[] vertList = new float[verts.Length * 8];
            int c = 0;
            for (int i = 0; i < verts.Length; i++)
            {
                vertList[c] = (verts[i].X);
                vertList[c+1] = (verts[i].Y);
                vertList[c+2] = (verts[i].Z);
                
                vertList[c+3] = (uvs[i].X);
                vertList[c+4] = (uvs[i].Y);
                
                vertList[c+5] = (verts[i].X);
                vertList[c+6] = (verts[i].Y);
                vertList[c+7] = (verts[i].Z);
                c += 8;
            }

            Mesh mesh = new Mesh();
            mesh.Vertices = vertList;
            mesh.Indices = indices;
            vertList = null;
            indices = null;
            uvs = null;
            return mesh;
        }
    }
}