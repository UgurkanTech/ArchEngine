using System;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Geometry
{
    public class Mesh : IRenderable
    {
        public int Vao { get; set; }
        public int Vbo { get; set; }
        public int Ibo { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }

        public void InitBuffers(Material mat)
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
            
            var vertexLocation = mat.Shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false , 8 * sizeof(float), 0);
            
            var texCoordLocation = mat.Shader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false , 8 * sizeof(float), 3 * sizeof(float));
            
            var normalLocation = mat.Shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false , 8 * sizeof(float), 5 * sizeof(float));

        }

        public void Render(Matrix4 Model, Material mat)
        {
            mat.Use(Model);
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

        public void RenderOutline(Matrix4 Model)
        {
            GL.Disable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Never);
            //GL.DepthMask(true);
            ShaderManager.ColorShader.SetMatrix4("model", Matrix4.CreateScale(1.04f) * Model);
            ShaderManager.ColorShader.Use();
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
            //GL.DepthMask(false);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
        }


        public void Dispose()
        {
            GL.DeleteBuffer(Vbo);
            GL.DeleteVertexArray(Vao);
            GL.DeleteBuffer(Ibo);
        }
    }
}