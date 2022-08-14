using System;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering
{
    public interface IRenderable : IDisposable
    {
        
        public Material Material { get; set; }
        
        public int Vao { get; set; }
        public int Vbo { get; set; }
        
        public int Ibo  { get; set; }
        
        public float[] Vertices { get; set; }
        
        public uint[] Indices { get; set; }

        public PrimitiveType type { get; set; }
        
        public void InitBuffers(bool raw = false)
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

            if (!raw)
            {
                if (type == PrimitiveType.Lines)
                {
                    var vertexLocation = 0;
                    GL.EnableVertexAttribArray(vertexLocation);
                    GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false , 0, 0);
                }
                else
                {
                    var vertexLocation = Material.Shader.GetAttribLocation("aPos");
                    GL.EnableVertexAttribArray(vertexLocation);
                    GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false , 8 * sizeof(float), 0);
            
                    var texCoordLocation = Material.Shader.GetAttribLocation("aTexCoords");
                    GL.EnableVertexAttribArray(texCoordLocation);
                    GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false , 8 * sizeof(float), 3 * sizeof(float));
            
                    var normalLocation = Material.Shader.GetAttribLocation("aNormal");
                    GL.EnableVertexAttribArray(normalLocation);
                    GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false , 8 * sizeof(float), 5 * sizeof(float));

                }
                
                Material = new Material();
            }
            else
            {
                var vertexLocation = 0;
                GL.EnableVertexAttribArray(vertexLocation);
                GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false , 4 * sizeof(float), 0);

                var texCoordLocation = 1;
                GL.EnableVertexAttribArray(texCoordLocation);
                GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false , 4 * sizeof(float), 2 * sizeof(float));

            }

        }

        public void Render(Matrix4 Model, PrimitiveType mode = PrimitiveType.Triangles)
        {

            Material.Use(Model);
            
            GL.BindVertexArray(Vao);

            

                if (Indices != null)
                {
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ibo);
                    GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, Ibo);
                }
                else
                {
                    switch (mode)
                    {
                        case PrimitiveType.Triangles:
                            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                            GL.DrawArrays(PrimitiveType.Triangles,0,Vertices.Length * 3 / 8);
                            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                            break;
                        case PrimitiveType.Lines:
                            GL.PointSize(5);
                            GL.DrawArrays(PrimitiveType.Points, 0, 2);
                            GL.LineWidth(3);
                            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
                            
                            break;
                    }
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
        
        public void RawRender()
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

        void IDisposable.Dispose()
        {
            GL.DeleteBuffer(Vbo);
            GL.DeleteVertexArray(Vao);
            GL.DeleteBuffer(Ibo);
        }


    }
}