using System;
using OpenTK.Graphics.OpenGL;

namespace ArchEngine.Core
{
    public class Framebuffer
    {
        private int _framebuffer;
        public int FrameBufferTexture;
        private int _renderBuffer;
        
        public void Init()
        {
            
            _framebuffer = GL.GenFramebuffer();
            
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, _framebuffer);

            FrameBufferTexture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, FrameBufferTexture);
            GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgb,800,600,0,PixelFormat.Rgb,PixelType.UnsignedByte,IntPtr.Zero);
            GL.TextureParameter(FrameBufferTexture, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TextureParameter(FrameBufferTexture, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
            
            
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,FramebufferAttachment.ColorAttachment0,TextureTarget.Texture2D,FrameBufferTexture,0);
            
            
            
            
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