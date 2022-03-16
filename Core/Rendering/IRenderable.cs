namespace ArchEngine.Core.Rendering
{
    public interface IRenderable
    {
        public int Vao { get; set; }

        public int Vbo { get; set; }

        public Shader Shader { get; set; }
        
        
    }
}