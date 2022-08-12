using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType;

namespace ArchEngine.Core.Rendering.Geometry
{
    public class Cube : IRenderable
    {


	    public Material Material { get; set; }
        public int Vao { get; set; }
        public int Vbo { get; set; }
        public int Ibo { get; set; }
        public float[] Vertices { get; set; }
        public uint[] Indices { get; set; }
        public PrimitiveType type { get; set; }


        public Matrix4 Model { get; set; }

       

        public Cube()
        {
	        Vertices = _vertices;
	        type = PrimitiveType.Triangles;
	        
        }

        private readonly float[] _vertices =
        {
			//Positions          //UVS         //Normals

			//Front
		   -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f, -1.0f,  1.0f,  //bottom left
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
			1.0f, -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  //bottom right
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
		   -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f, -1.0f,  1.0f,  //bottom left
		   -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  1.0f,  //top left

			//Back
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
			1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  //top right
			1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  1.0f, -1.0f, -1.0f,  //bottom right
			1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  //top right
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
		   -1.0f,  1.0f, -1.0f,  0.0f,  1.0f, -1.0f,  1.0f, -1.0f,  //top left

			//Right
			1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  1.0f, -1.0f, -1.0f,  //bottom left
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
			1.0f, -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  //bottom right
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
			1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  1.0f, -1.0f, -1.0f,  //bottom left
			1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  1.0f,  1.0f, -1.0f,  //top left

			//Left
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
		   -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  //top right
		   -1.0f, -1.0f,  1.0f,  1.0f,  0.0f, -1.0f, -1.0f,  1.0f,  //bottom right
		   -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  //top right
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
		   -1.0f,  1.0f, -1.0f,  0.0f,  1.0f, -1.0f,  1.0f, -1.0f,  //top left

			//Top
		   -1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f,  1.0f, -1.0f,  //bottom left
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
			1.0f,  1.0f, -1.0f,  1.0f,  0.0f,  1.0f,  1.0f, -1.0f,  //bottom right
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
		   -1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f,  1.0f, -1.0f,  //bottom left
		   -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  1.0f,  //top left

			//Bottom
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
			1.0f, -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  //top right
			1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  1.0f, -1.0f, -1.0f,  //bottom right
			1.0f, -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  //top right
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
		   -1.0f, -1.0f,  1.0f,  0.0f,  1.0f, -1.0f, -1.0f,  1.0f,  //top left
        };
        
        
        private readonly float[] _verticesPlane =
        {
	        //Positions          //UVS         //Normals

	        //Front
	        -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f, -1.0f,  1.0f,  //bottom left
	        1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
	        1.0f, -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  //bottom right
	        1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
	        -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f, -1.0f,  1.0f,  //bottom left
	        -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  1.0f,  //top left
	    };
        
        private readonly uint[] _indicesPlane =
        {
	        //front
	        0, 7, 3,
	        0, 4, 7
        };
        

        private readonly uint[] _indices =
        {
	        //front
	        0, 7, 3,
	        0, 4, 7,
	        //back
	        1, 2, 6,
	        6, 5, 1,
	        //left
	        0, 2, 1,
	        0, 3, 2,
	        //right
	        4, 5, 6,
	        6, 7, 4,
	        //top
	        2, 3, 6,
	        6, 3, 7,
	        //bottom
	        0, 1, 5,
	        0, 5, 4
        };
        
    }
}