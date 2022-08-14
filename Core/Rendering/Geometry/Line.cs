using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Geometry
{
    public class Line  : IRenderable
    {
        public Material Material { get; set; }
        public int Vao { get; set; }
        public int Vbo { get; set; }
        public int Ibo { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public PrimitiveType type { get; set; }

        public Line()
        {
            Vertices = new float[]
            {
                0, 0, 0,
                2, 2, 2,
            };
            type = PrimitiveType.Lines;
        }
        
        public Line(Vector3 start, Vector3 end)
        {
            Vertices = new float[]
            {
                start.X, start.Y, start.Z,
                end.X, end.Y, end.Z,
            };
            type = PrimitiveType.Lines;

        }
    }
}