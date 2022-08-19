using System;
using System.Collections.Generic;
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

            for (int i = 0; i < 25; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    AddGameObject(GenerateWorldChunk(mat, new Vector3(i * 1.6f, -9f, j * 1.6f)));
                }
            }
            
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
            
            List<float> vertList = new List<float>();
            for (int i = 0; i < verts.Length; i++)
            {
                vertList.Add(verts[i].X);
                vertList.Add(verts[i].Y);
                vertList.Add(verts[i].Z);
                
                vertList.Add(uvs[i].X);
                vertList.Add(uvs[i].Y);
                
                vertList.Add(verts[i].X);
                vertList.Add(verts[i].Y);
                vertList.Add(verts[i].Z);
            }

            Mesh mesh = new Mesh();
            mesh.Vertices = vertList.ToArray();
            mesh.Indices = indices;
            vertList.Clear();
            indices = null;
            uvs = null;
            return mesh;
        }
    }
}