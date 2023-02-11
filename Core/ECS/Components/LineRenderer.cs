using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.GUI.Editor;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace ArchEngine.Core.ECS.Components
{
    public class LineRenderer : Component
    {
        public Material Material { get; set; }
        public GameObject gameObject { get; set; }
        public bool initialized { get; set; }
        
        public Line line;
        
        [JsonIgnore]
        public int StencilID  { get; set; }
        
        
        private Vector3 _StartPos { get; set; }
        private Vector3 _EndPos { get; set; }

        public LineRenderer()
        {
            Material mat = new Material();
            mat.Shader = ShaderManager.ColorShader;
            Material = mat;
            line = Primitives.Line;
            StartPos = Vector3.Zero;
            EndPos = Vector3.One;
        }
        
        [Inspector] public Vector3 StartPos
        {
            get
            {
                return _StartPos;
            }
            set
            {
                _StartPos = value;
                line?.UpdatePositions(_StartPos, _EndPos);
            }
        }

        [Inspector] public Vector3 EndPos {             
            get
            {
                return _EndPos;
            }
            set
            {
                _EndPos = value;
                line?.UpdatePositions(_StartPos, _EndPos);
            } 
        }
        
        public void Init()
        {
            if (!initialized && line != null)
            {
                line.InitBuffers(Material);
                initialized = true;
            }
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            
        }
        
        public void Dispose()
        {
            //line?.Dispose();
            initialized = false;
        }
    }
}