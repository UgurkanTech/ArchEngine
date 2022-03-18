using ArchEngine.Core.Rendering.Textures;

namespace ArchEngine.Core.Rendering
{
    public interface IRenderable
    {
        public VertexObject Vo { get; set; }
        
        public Shader Shader { get; set; }
        
        public Texture Texture { get; set; }
    }
}