using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering
{
    public interface IRenderable
    {

        public Shader Shader { get; set; }
        
        public Texture Texture { get; set; }
        
        public int Vao { get; set; }
        public int Vbo { get; set; }
        
        public int Ibo  { get; set; }
        
        public float[] Vertices { get; set; }
        
        public uint[] Indices { get; set; }

        public Matrix4 Model { get; set; }

        public void Init()
        {
            if (Indices != null)
            {
                Ibo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ibo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            }

            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            Vao = GL.GenVertexArray();
            GL.BindVertexArray(Vao);

            var vertexLocation = Shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false , 8 * sizeof(float), 0);
            
            var texCoordLocation = Shader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false , 8 * sizeof(float), 3 * sizeof(float));
            
            var normalLocation = Shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false , 8 * sizeof(float), 5 * sizeof(float));

            
        }

        public void Render(bool raw = false)
        {
            
            
            if (!raw)
            {
                Texture.Use();
                Shader.Use();
                Shader.SetMatrix4("model", Model);
            }

            
            
            GL.BindVertexArray(Vao);

            if (Indices != null)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ibo);
                GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, Ibo);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles,0,Vertices.Length * 3 / 8);
            }
        }

        public void Destroy()
        {
            GL.DeleteBuffer(Vbo);
            GL.DeleteVertexArray(Vao);
        }
    }
}