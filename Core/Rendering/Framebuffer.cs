using System;
using ArchEngine.Core.Rendering.Geometry;
using OpenTK.Graphics.OpenGL;

namespace ArchEngine.Core.Rendering
{
    public class Framebuffer : IDisposable
    {
        private int _framebuffer;
        public int frameBufferTexture;
        private int _renderBuffer;


        private int _width, _height;
        
        public void Init(int width, int height)
        {
            _width = width;
            _height = height;
            _framebuffer = GL.GenFramebuffer();

            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer);

            frameBufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, frameBufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb,
                PixelType.UnsignedByte, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int) TextureMagFilter.Linear);


            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0,
                TextureTarget.Texture2D, frameBufferTexture, 0);
            
            _renderBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _renderBuffer);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, width, height);
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);

            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment,
                RenderbufferTarget.Renderbuffer, _renderBuffer);

        }

        public void Use()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer);

            //GL.Viewport(0, 0, _width, _height);
        }


        public void Dispose()
        {
            GL.DeleteFramebuffer(_framebuffer);
            GL.DeleteTexture(frameBufferTexture);
            GL.DeleteRenderbuffer(_renderBuffer);
        }
    }
}