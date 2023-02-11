using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.GUI.Editor;
using Newtonsoft.Json;

namespace ArchEngine.Core.ECS.Components
{
    public class MeshRenderer : Component
    {
        
        [Inspector()] public Material Material { get; set; }
        public GameObject gameObject { get; set; }
        public bool initialized { get; set; }

        [Inspector()] public Mesh mesh;
        
        [JsonIgnore]
        public int StencilID  { get; set; }

        public MeshRenderer()
        {
            mesh = Primitives.Cube;
            Material mat = new Material();
            mat.Shader = ShaderManager.PbrShader;
            mat.MaterialHash = "";
            //mat.LoadTextures("Resources/Textures/wall");
            Material = mat;
        }

        public void Init()
        {
            
            if (!initialized && mesh != null)
            {
                mesh.InitBuffers(Material);
                initialized = true;
            }
            //mesh.Material = new Material();
            
            
        }

        public void Render()
        {
            
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            
        }

        

        public void Dispose()
        {
            mesh?.Dispose();
            initialized = false;
        }
    }
}