using System;
using ArchEngine.Core.Rendering.Textures;
using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Geometry
{
    public class Mesh : IRenderable
    {
        [JsonIgnore] public string MeshHash  { get; set; }
        [JsonIgnore] public int Vao { get; set; }
        [JsonIgnore] public int Vbo { get; set; }
        [JsonIgnore] public int Ibo { get; set; }
        [JsonIgnore] public float[] Vertices { get; set; }
        
        [JsonIgnore] public int[] Indices { get; set; }
        
        private bool initialized { get; set; }
        
        private int IndicesCount { get; set; }
        [JsonIgnore] public int VerticesCount { get; set; }

        public void InitBuffers(Material mat)
        {
            if (initialized)
            {
                return;
            }

            
            VerticesCount = Vertices.Length;
            


            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            
            GL.BufferData(BufferTarget.ArrayBuffer, VerticesCount * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            Vao = GL.GenVertexArray();
            GL.BindVertexArray(Vao);

            if (mat.Shader == null)
            {
                throw new Exception("No shader found!");
            }
            
            var vertexLocation = mat.Shader.GetAttribLocation("aPos");
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false , 8 * sizeof(float), 0);
            
            var texCoordLocation = mat.Shader.GetAttribLocation("aTexCoords");
            GL.EnableVertexAttribArray(texCoordLocation);
            GL.VertexAttribPointer(texCoordLocation, 2, VertexAttribPointerType.Float, false , 8 * sizeof(float), 3 * sizeof(float));
            
            var normalLocation = mat.Shader.GetAttribLocation("aNormal");
            GL.EnableVertexAttribArray(normalLocation);
            GL.VertexAttribPointer(normalLocation, 3, VertexAttribPointerType.Float, false , 8 * sizeof(float), 5 * sizeof(float));


            if (Indices != null) //do this after vao
            {
                IndicesCount = Indices.Length;
                Ibo = GL.GenBuffer();
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ibo);
                GL.BufferData(BufferTarget.ElementArrayBuffer, Indices.Length * sizeof(uint), Indices, BufferUsageHint.StaticDraw);
            }
            
            //Vertices = null; //required for physics
            Indices = null;
            initialized = true;
        }

        public void Render(Matrix4 Model, Material mat)
        {
            mat.Use(Model);
            GL.BindVertexArray(Vao);
            
            if (IndicesCount != 0)
            {
              
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ibo);
                GL.DrawElements(BeginMode.Triangles, IndicesCount, DrawElementsType.UnsignedInt, 0);
                
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles,0,VerticesCount * 3 / 8);
            }
        }

        public void RenderOutline(Matrix4 Model)
        {
            GL.Disable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Never);
            //GL.DepthMask(true);
            ShaderManager.ColorShader.SetMatrix4("model", Matrix4.CreateScale(Model.ExtractScale() + (new Vector3(0.05f)) * Model.ExtractScale()) * Model.ClearScale());
            ShaderManager.ColorShader.Use();
            GL.BindVertexArray(Vao);
            
            if (IndicesCount != 0)
            {
              
                GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ibo);
                GL.DrawElements(BeginMode.Triangles, IndicesCount, DrawElementsType.UnsignedInt, 0);
                
            }
            else
            {
                GL.DrawArrays(PrimitiveType.Triangles,0,VerticesCount * 3 / 8);

            }
            //GL.DepthMask(false);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
        }


        public virtual void Dispose()
        {
            GL.DeleteBuffer(Vbo);
            GL.DeleteVertexArray(Vao);
            GL.DeleteBuffer(Ibo);
        }
    }
}