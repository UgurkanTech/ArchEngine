using System;
using OpenTK.Graphics.OpenGL;

namespace ArchEngine.Core
{
    public class Framebuffer
    {
        private int _framebuffer;
        public int frameBufferTexture;
        private int _renderBuffer;
        
        public void Init()
        {
            
            _framebuffer = GL.GenFramebuffer();
            
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer);

            frameBufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, frameBufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgb,800,600,0,PixelFormat.Rgb,PixelType.UnsignedByte,IntPtr.Zero);
            GL.TextureParameter(frameBufferTexture, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameter(frameBufferTexture, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,FramebufferAttachment.ColorAttachment0,TextureTarget.Texture2D,frameBufferTexture,0);
            
            
            
            
            _renderBuffer = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _renderBuffer); 
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.Depth24Stencil8, 800, 600);  
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, 0);
            
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthStencilAttachment, RenderbufferTarget.Renderbuffer, _renderBuffer);

        }

        public void Use()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Viewport(0, 0, 800, 600);
        }
    }
}