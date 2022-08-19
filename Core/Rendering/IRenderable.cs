using System;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering
{
    public interface IRenderable : IDisposable
    {
      
        public int Vao { get; set; }
        public int Vbo { get; set; }
        public int Ibo  { get; set; }
        public float[] Vertices { get; set; }
        public int[] Indices { get; set; }
        abstract void InitBuffers(Material mat);
        abstract void Render(Matrix4 Model, Material mat);
        abstract void RenderOutline(Matrix4 Model);
       
    }
}