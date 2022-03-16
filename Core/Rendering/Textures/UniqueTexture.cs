using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace ArchEngine.Core.Rendering.Textures
{
    // A helper class, much like Shader, meant to simplify loading textures.
    public class UniqueTexture : Texture
    {
        public readonly int Handle;
        public TextureUnit Unit;

        public UniqueTexture(int glHandle)
        {
            Handle = glHandle;
        }

        public override void Use()
        {
            GL.ActiveTexture(Unit);
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        public UniqueTexture SetUnit(TextureUnit unit)
        {
            this.Unit = unit;
            return this;
        }
    }
}