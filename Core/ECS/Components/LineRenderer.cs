using System.ComponentModel.DataAnnotations;
using System.Drawing;
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
        
        private Color4 color;

        private int width;

        public LineRenderer()
        {
            Material mat = new Material();
            mat.Shader = ShaderManager.LineShader;
            Material = mat;
            color = Color4.Red;
            line = Primitives.Line;
            StartPos = Vector3.Zero;
            EndPos = Vector3.One;
            width = 3;

        }
        
        [Inspector][Range(1, 32)] public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
                line.width = value;
            }
        }
        
        [Inspector] public Color4 Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                line.color = new Vector4(color.R, color.G, color.B, color.A);
            }
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
                line.color = new Vector4(color.R, color.G, color.B, color.A);
                line.width = width;
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