using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using ArchEngine.GUI.Editor;
using OpenTK.Graphics.OpenGL;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace ArchEngine.Core.Rendering.Textures
{
    public class TextureManager
    {
        public static List<Texture> Textures = new List<Texture>();

        public static Texture LoadFromFile(string path, TextureUnit textureUnit = TextureUnit.Texture0, bool flip = true, TextureMagFilter mag = TextureMagFilter.Linear, TextureMinFilter min = TextureMinFilter.Linear)
        {
            foreach (var texture in Textures)
            {
                if (texture.hash.Equals(path))
                    return texture;
            }
            
            // Generate handle
            int handle = GL.GenTexture();

            // Bind the handle
            GL.ActiveTexture(textureUnit);
            GL.BindTexture(TextureTarget.Texture2D, handle);




            try
            {
                Stream stream;
                if (path.Contains(":"))
                {
                    stream = new ResourceStream(path).GetStream();
                }
                else
                {
                    stream = new ResourceStream(path, null).GetStream();
                }
                
                using var image = new Bitmap(stream);
                // Our Bitmap loads from the top-left pixel, whereas OpenGL loads from the bottom-left, causing the texture to be flipped vertically.
                // This will correct that, making the texture display properly.
                if (flip)
                {
                    image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                }

                // First, we get our pixels from the bitmap we loaded.
                // Arguments:
                //   The pixel area we want. Typically, you want to leave it as (0,0) to (width,height), but you can
                //   use other rectangles to get segments of textures, useful for things such as spritesheets.
                //   The locking mode. Basically, how you want to use the pixels. Since we're passing them to OpenGL,
                //   we only need ReadOnly.
                //   Next is the pixel format we want our pixels to be in. In this case, ARGB will suffice.
                //   We have to fully qualify the name because OpenTK also has an enum named PixelFormat.
                var data = image.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    ImageLockMode.ReadOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppArgb);

                // Now that our pixels are prepared, it's time to generate a texture. We do this with GL.TexImage2D.
                // Arguments:
                //   The type of texture we're generating. There are various different types of textures, but the only one we need right now is Texture2D.
                //   Level of detail. We can use this to start from a smaller mipmap (if we want), but we don't need to do that, so leave it at 0.
                //   Target format of the pixels. This is the format OpenGL will store our image with.
                //   Width of the image
                //   Height of the image.
                //   Border of the image. This must always be 0; it's a legacy parameter that Khronos never got rid of.
                //   The format of the pixels, explained above. Since we loaded the pixels as ARGB earlier, we need to use BGRA.
                //   Data type of the pixels.
                //   And finally, the actual pixels.
                GL.TexImage2D(TextureTarget.Texture2D,
                    0,
                    PixelInternalFormat.Rgba,
                    image.Width,
                    image.Height,
                    0,
                    PixelFormat.Bgra,
                    PixelType.UnsignedByte,
                    data.Scan0);
            }
            catch (Exception e)
            {
                //Console.WriteLine("Texture not found: " + path);
#if DEBUG
                Window._log.Debug("Texture not found: " + path);
#endif
                
                //Console.WriteLine(e);
                return new Texture(0).SetUnit(textureUnit);
            }

            // Now that our texture is loaded, we can set a few settings to affect how the image appears on rendering.

            // First, we set the min and mag filter. These are used for when the texture is scaled down and up, respectively.
            // Here, we use Linear for both. This means that OpenGL will try to blend pixels, meaning that textures scaled too far will look blurred.
            // You could also use (amongst other options) Nearest, which just grabs the nearest pixel, which makes the texture look pixelated if scaled too far.
            // NOTE: The default settings for both of these are LinearMipmap. If you leave these as default but don't generate mipmaps,
            // your image will fail to render at all (usually resulting in pure black instead).
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)min);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)mag);

            // Now, set the wrapping mode. S is for the X axis, and T is for the Y axis.
            // We set this to Repeat so that textures will repeat when wrapped. Not demonstrated here since the texture coordinates exactly match
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

            // Next, generate mipmaps.
            // Mipmaps are smaller copies of the texture, scaled down. Each mipmap level is half the size of the previous one
            // Generated mipmaps go all the way down to just one pixel.
            // OpenGL will automatically switch between mipmaps when an object gets sufficiently far away.
            // This prevents moiré effects, as well as saving on texture bandwidth.
            // Here you can see and read about the morié effect https://en.wikipedia.org/wiki/Moir%C3%A9_pattern
            // Here is an example of mips in action https://en.wikipedia.org/wiki/File:Mipmap_Aliasing_Comparison.png
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

            Texture t = new Texture(handle).SetUnit(textureUnit);
            t.hash = path;
            Textures.Add(t);
            return t;
        }

        public static Texture LoadCubemapFromFile(string FolderPath, bool flip = true)
        {
            List<string> faces = new List<string>();
            faces.Add(FolderPath + "/right.png");
            faces.Add(FolderPath + "/left.png");
            faces.Add(FolderPath + "/top.png");
            faces.Add(FolderPath + "/bottom.png");
            faces.Add(FolderPath + "/front.png");
            faces.Add(FolderPath + "/back.png");
            
            // Generate handle
            int handle = GL.GenTexture();
            GL.BindTexture(TextureTarget.TextureCubeMap, handle); //check this

            for(int i = 0; i < faces.Count; i++)
            {
                try
                {
                    Stream stream;
                    if (faces[i].Contains(":"))
                    {
                        stream = new ResourceStream(faces[i]).GetStream();
                    }
                    else
                    {
                        stream = new ResourceStream(faces[i], null).GetStream();
                    }
                    
                    using var image = new Bitmap(stream);
                    if (flip)
                    {
                        image.RotateFlip(RotateFlipType.RotateNoneFlipY);
                    }
                    var data = image.LockBits(
                        new Rectangle(0, 0, image.Width, image.Height),
                        ImageLockMode.ReadOnly,
                        System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    GL.TexImage2D(TextureTarget.TextureCubeMapPositiveX + i,
                        0,
                        PixelInternalFormat.Rgba,
                        image.Width,
                        image.Height,
                        0,
                        PixelFormat.Bgra,
                        PixelType.UnsignedByte,
                        data.Scan0);
                }
                catch (Exception e)
                {
                    //Console.WriteLine("Texture not found: " + FolderPath);
#if DEBUG
                    Window._log.Debug("Texture not found: " + FolderPath);
#endif
                    //Console.WriteLine(e);
                    return new Texture(0).SetUnit(TextureUnit.Texture0);
                }
                
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);


                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToEdge);
                GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureWrapR, (int)TextureWrapMode.ClampToEdge);
                
            }
            Texture t = new Texture(handle).SetUnit(TextureUnit.Texture0);
            t.hash = FolderPath;
            Window._log.Info("Skybox id:" + handle);
            Textures.Add(t);
            return t;
        }
    }
}