using ArchEngine.Core.Rendering.Textures;
using ArchEngine.GUI.Editor;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Geometry
{
    public class Line  : IRenderable
    {
        
        public int Vao { get; set; }
        public int Vbo { get; set; }
        public int Ibo { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }

        private Vector3 _StartPos;
        private Vector3 _EndPos;
        
        public Vector3 StartPos
        {
            get
            {
                return _StartPos;
            }
            set
            {
                _StartPos = value;
                UpdatePositions();
            }
        }

        public Vector3 EndPos
        {
            get
            {
                return _EndPos;
            }
            set
            {
                _EndPos = value;
                UpdatePositions();
            }
        }

        public void InitBuffers(Material mat)
        {
            Vbo = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);

            Vao = GL.GenVertexArray();
            GL.BindVertexArray(Vao);
            
            var vertexLocation = 0;
            GL.EnableVertexAttribArray(vertexLocation);
            GL.VertexAttribPointer(vertexLocation, 3, VertexAttribPointerType.Float, false , 0, 0);
        }

        public void UpdatePositions()
        {
            Vertices = new float[]
            {
                StartPos.X, StartPos.Y, StartPos.Z,
                EndPos.X, EndPos.Y, EndPos.Z,
            };
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, Vertices.Length * sizeof(float), Vertices, BufferUsageHint.StaticDraw);
        }
        
        public void Render(Matrix4 Model, Material mat)
        {
            mat.Use(Model);

            GL.BindVertexArray(Vao);
            
            GL.PointSize(5);
            GL.DrawArrays(PrimitiveType.Points, 0, 2);
            GL.LineWidth(3);
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
        }

        public void RenderOutline(Matrix4 Model)
        {
            GL.Disable(EnableCap.DepthTest);
            //GL.DepthFunc(DepthFunction.Never);
            //GL.DepthMask(true);
            ShaderManager.ColorShader.SetMatrix4("model",  Model);
            ShaderManager.ColorShader.Use();
            GL.BindVertexArray(Vao);
            
            GL.PointSize(6);
            GL.DrawArrays(PrimitiveType.Points, 0, 2);
            GL.LineWidth(4);
            GL.DrawArrays(PrimitiveType.Lines, 0, 2);
            //GL.DepthMask(false);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
        }
        
        public Line()
        {
            Vertices = new float[]
            {
                0, 0, 0,
                2, 2, 2,
            };
        }
        
        public Line(Vector3 start, Vector3 end)
        {
            StartPos = start;
            EndPos = end;
            Vertices = new float[]
            {
                StartPos.X, StartPos.Y, StartPos.Z,
                EndPos.X, EndPos.Y, EndPos.Z,
            };
        }

        public void Dispose()
        {
            GL.DeleteBuffer(Vbo);
            GL.DeleteVertexArray(Vao);
            GL.DeleteBuffer(Ibo);
        }
    }
}