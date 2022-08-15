using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Geometry
{
    public class FullScreenQuad  : IRenderable
    {
        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
        public Material Material { get; set; }
        public int Vao { get; set; }
        public int Vbo { get; set; }
        public int Ibo { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public void InitBuffers(Material mat)
        {
            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            Vao = GL.GenVertexArray();
            GL.BindVertexArray(Vao);
            
            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false , 4 * sizeof(float), 0);

            var texCoordLocation = 1;
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false , 4 * sizeof(float), 2 * sizeof(float));

        }

        public void Render(Matrix4 Model, Material mat)
        {
            GL.BindVertexArray(Vao);

            if (Indices != null)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ibo);
                GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, Ibo);
            }
            else
            {
  
                GL.DrawArrays(PrimitiveType.Triangles,0,6);
                
            }
        }

        public void RenderOutline(Matrix4 Model)
        {
            throw new System.NotImplementedException();
        }
        
        public FullScreenQuad()
        {
            Vertices = vertices;

        }

        private readonly float[] vertices = {
            //   positions     texture coordinates
            -1.0f,  1.0f,  0.0f, 1.0f,
            -1.0f, -1.0f,  0.0f, 0.0f,
            1.0f, -1.0f,  1.0f, 0.0f,

            -1.0f,  1.0f,  0.0f, 1.0f,
            1.0f, -1.0f,  1.0f, 0.0f,
            1.0f,  1.0f,  1.0f, 1.0f
        };
    }
}