using System;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering
{
    public class Skybox : IRenderable
    {
        public void Dispose()
        {
            GL.DeleteBuffer(Vbo);
            GL.DeleteVertexArray(Vao);
            GL.DeleteBuffer(Ibo);
        }

        public int Vao { get; set; }
        public int Vbo { get; set; }
        public int Ibo { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        private bool initialized { get; set; }
        
        private int IndicesCount { get; set; }
        private int VerticesCount { get; set; }
        
        public void InitBuffers(CubemapMaterial mat)
        {
            if (initialized)
            {
                return;
            }

            
            VerticesCount = Vertices.Length;
            
            if (Indices != null)
            {
                IndicesCount = Indices.Length;
                Ibo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ibo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, IndicesCount * sizeof(uint), Indices, BufferUsageHint.StaticDraw);

            }

            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            
            GL.BufferData(BufferTarget.ArrayBuffer, VerticesCount * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            Vao = GL.GenVertexArray();
            GL.BindVertexArray(Vao);

            if (mat.Shader == null)
            {
                throw new Exception("No shader!!");
            }
            
            var vertexLocation = mat.Shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false , 3 * sizeof(float), 0);
            
            
            Vertices = null;
            Indices = null;
            initialized = true;
        }

        public void InitBuffers(Material mat)
        {
            throw new NotImplementedException();
        }

        public void Render(Matrix4 Model, Material mat)
        {
            throw new NotImplementedException();
        }

        public void RenderSkybox(Matrix4 Model, CubemapMaterial mat)
        {
            GL.DepthFunc(DepthFunction.Lequal);
            mat.Use(Model);
            GL.BindVertexArray(Vao);
            GL.DepthMask(false);
       
            
            if (Indices != null)
            {
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ibo);
                GL.DrawElements(BeginMode.Triangles, IndicesCount, DrawElementsType.UnsignedInt, Ibo);
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles,0,VerticesCount * 3 / 8);
            }
            GL.DepthMask(true);
        }

        public void RenderOutline(Matrix4 Model)
        {
            throw new System.NotImplementedException();
        }
    }
}