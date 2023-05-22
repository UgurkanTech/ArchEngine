using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL;

namespace ArchEngine.Core.Rendering.Textures
{
    // A helper class, much like Shader, meant to simplify loading textures.
    public class Texture
    {
        [JsonIgnore]public readonly int handle;
        [JsonIgnore]public string name;
        [JsonIgnore] public TextureUnit unit;

        public string hash;
        public Texture(int glHandle)
        {
            handle = glHandle;
        }

        public void Use()
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, handle);
        }

        public Texture SetUnit(TextureUnit unit)
        {
            this.unit = unit;
            return this;
        }
    }
}