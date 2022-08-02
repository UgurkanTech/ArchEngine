using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Geometry
{
    public class FullScreenQuad  : IRenderable
    {
        public Material Material { get; set; }
        public int Vao { get; set; }
        public int Vbo { get; set; }
        public int Ibo { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public Matrix4 Model { get; set; }


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

        public GameObject gameObject { get; set; }
        public void Init()
        {
            
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            
        }
    }
}