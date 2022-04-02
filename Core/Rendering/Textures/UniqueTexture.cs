using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace ArchEngine.Core.Rendering.Textures
{
    // A helper class, much like Shader, meant to simplify loading textures.
    public class UniqueTexture : Texture
    {
        public readonly int handle;
        public TextureUnit unit;

        public UniqueTexture(int glHandle)
        {
            handle = glHandle;
        }

        public override void Use()
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(TextureTarget.Texture2D, handle);
        }

        public UniqueTexture SetUnit(TextureUnit unit)
        {
            this.unit = unit;
            return this;
        }
    }
}