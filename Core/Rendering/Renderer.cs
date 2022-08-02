using System.Collections.Generic;
using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering.Geometry;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering
{
    public class Renderer
    {
        public Framebuffer frameBuffer;

        private IRenderable fsq;

        public Renderer()
        {
            frameBuffer = new Framebuffer();
            frameBuffer.Init();

            fsq = new FullScreenQuad();
            fsq.InitBuffers(true);
        }


        public void Use()
        {
            frameBuffer.Use();
        }

        public void Render(IRenderable objects, Matrix4 model)
        {

            objects.Render(model);

        }

        public void RenderRecursively(GameObject obj)
        {
            obj._components.ForEach(component =>
            {
                if (component.GetType() == typeof(GameObject))
                {
                    RenderRecursively(component as GameObject);
                }
                else if (component.GetType() == typeof(MeshRenderer))
                {
                    MeshRenderer mr = component as MeshRenderer;
                    Render(mr.mesh, obj.Transform);
                }   
            });
        }
        

        public void RenderAllChildObjects(List<GameObject> objs)
        {
            foreach (var obj in objs)
            {
                RenderRecursively(obj);
            }
        }


        public void DisplayFullScreen(Shader shader)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0); // unbind your FBO to set the default framebuffer
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            shader.Use(); // shader program for rendering the quad  

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, frameBuffer.frameBufferTexture); // color attachment texture
            
            fsq.RawRender();
        }
    }
}