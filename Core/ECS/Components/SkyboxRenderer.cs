using System;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Mathematics;

namespace ArchEngine.Core.ECS.Components
{
    public class SkyboxRenderer : Component
    {
        public void Dispose()
        {
            Skybox?.Dispose();
            initialized = false;
        }

        public SkyboxRenderer()
        {
            Skybox = new Skybox();
            CubemapMaterial mat = new CubemapMaterial();
            Material = mat;
            Material.Shader = ShaderManager.SkyboxShader;
  
            
            Skybox.Vertices = _vertices;
        }
        public CubemapMaterial Material { get; set; }
        
        public Skybox Skybox;
        public GameObject gameObject { get; set; }
        public bool initialized { get; set; }
        public void Init()
        {
	        if (!initialized && Skybox != null)
            {
	            Skybox.InitBuffers(Material);
                initialized = true;
            }
        }

        public void Render()
        {
            Skybox.RenderSkybox(Matrix4.Identity, Material);
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            
        }
        
        private readonly float[] _vertices =
        {
	        // positions          
	        -1.0f,  1.0f, -1.0f,
	        -1.0f, -1.0f, -1.0f,
	        1.0f, -1.0f, -1.0f,
	        1.0f, -1.0f, -1.0f,
	        1.0f,  1.0f, -1.0f,
	        -1.0f,  1.0f, -1.0f,

	        -1.0f, -1.0f,  1.0f,
	        -1.0f, -1.0f, -1.0f,
	        -1.0f,  1.0f, -1.0f,
	        -1.0f,  1.0f, -1.0f,
	        -1.0f,  1.0f,  1.0f,
	        -1.0f, -1.0f,  1.0f,

	        1.0f, -1.0f, -1.0f,
	        1.0f, -1.0f,  1.0f,
	        1.0f,  1.0f,  1.0f,
	        1.0f,  1.0f,  1.0f,
	        1.0f,  1.0f, -1.0f,
	        1.0f, -1.0f, -1.0f,

	        -1.0f, -1.0f,  1.0f,
	        -1.0f,  1.0f,  1.0f,
	        1.0f,  1.0f,  1.0f,
	        1.0f,  1.0f,  1.0f,
	        1.0f, -1.0f,  1.0f,
	        -1.0f, -1.0f,  1.0f,

	        -1.0f,  1.0f, -1.0f,
	        1.0f,  1.0f, -1.0f,
	        1.0f,  1.0f,  1.0f,
	        1.0f,  1.0f,  1.0f,
	        -1.0f,  1.0f,  1.0f,
	        -1.0f,  1.0f, -1.0f,

	        -1.0f, -1.0f, -1.0f,
	        -1.0f, -1.0f,  1.0f,
	        1.0f, -1.0f, -1.0f,
	        1.0f, -1.0f, -1.0f,
	        -1.0f, -1.0f,  1.0f,
	        1.0f, -1.0f,  1.0f
        };
        
    }
}