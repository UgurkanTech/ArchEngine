using System.Runtime.InteropServices;
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

        [Inspector] public Vector3 StartPos
        {
            get
            {
                return line.StartPos;
            }
            set
            {
                line.StartPos = value;
            }
        }

        [Inspector] public Vector3 EndPos {             
            get
            {
                return line.EndPos;
            }
            set
            {
                line.EndPos = value;
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
            line?.Dispose();
            initialized = false;
        }
    }
}