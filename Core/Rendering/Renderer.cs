using System;
using System.Collections.Generic;
using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.GUI.Editor;
using ArchEngine.GUI.Editor.Windows;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering
{
    public class Renderer
    {
        public Framebuffer frameBuffer;

        private FullScreenQuad fsq;

        public Vector2i RenderSize = new Vector2i(800, 600);

        public PolygonMode mode = PolygonMode.Fill;

        public long TotalVertices;
        private long vertCount;
        public Renderer()
        {
            frameBuffer = new Framebuffer();
            frameBuffer.Init(RenderSize.X, RenderSize.Y);

            fsq = new FullScreenQuad();
            fsq.Material = new Material();
            fsq.Material.Shader = ShaderManager.FullscreenShader;
            fsq.InitBuffers(fsq.Material);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Resize(int witdh, int height)
        {
            RenderSize = new Vector2i(witdh, height);
            frameBuffer.Dispose();
            frameBuffer = new Framebuffer();
            frameBuffer.Init(witdh, height);
        }


        public void Use()
        {
            
            frameBuffer.Use();
            GL.Viewport(0, 0, RenderSize.X, RenderSize.Y);
            GL.ClearColor(0.1f, 0.2f, 0.25f, 1.0f);
            GL.ClearStencil(0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            

            GL.StencilOp(StencilOp.Keep, StencilOp.Keep, StencilOp.Replace);
        }
        
        private int count;
        private Matrix4 selectedRoot;
        private bool selectedRootActive = true;

        public void RenderRecursively(GameObject parent, Matrix4 parentMatrix, bool selected = false, bool rootActive = true)
        {
            
            if (Editor.selectedGameobject == parent && !selected)
            {
                selectedRoot = parentMatrix;
                selectedRootActive = rootActive;
                return;
            }
            parent._components.ForEach(component =>
            {
                if (!parent.isActive || !rootActive)
                    return;
                if (component.GetType() == typeof(MeshRenderer))
                {
                    MeshRenderer mr = component as MeshRenderer;
                    if (!mr.initialized)
                        return;
                    GL.StencilFunc(StencilFunction.Always, count, -1);
                    mr.mesh.Render(parent.Transform * parentMatrix, mr.Material);
                    vertCount += mr.mesh.VerticesCount;
                    mr.StencilID = count;
                    count++;
                }
                else if (component.GetType() == typeof(LineRenderer))
                {
                    LineRenderer mr = component as LineRenderer;
                    if (!mr.initialized)
                        return;
                    GL.StencilFunc(StencilFunction.Always, count, -1);
                    mr.line.UpdatePositions(mr.StartPos, mr.EndPos);
                    mr.line.Render(parent.Transform * parentMatrix, mr.Material);
                    mr.StencilID = count;
                    count++;
                }

            });
            parent._childs.ForEach(child =>
            {
                RenderRecursively(child, child.Transform * parent.Transform *  parentMatrix, rootActive:child.isActive && rootActive);
                
            });
            
        }
        public void RenderOutlineRecursively(GameObject parent, Matrix4 parentMatrix)
        {
            parent._components.ForEach(component =>
            {
                if (component.GetType() == typeof(MeshRenderer))
                {
                    MeshRenderer mr = component as MeshRenderer;
                    if (!mr.initialized)
                        return;
                    mr.mesh.RenderOutline(parent.Transform * parentMatrix);

                }   
                else if (component.GetType() == typeof(LineRenderer))
                {
                    LineRenderer mr = component as LineRenderer;
                    if (!mr.initialized)
                        return;
                    mr.line.UpdatePositions(mr.StartPos, mr.EndPos);
                    mr.line.RenderOutline(parent.Transform * parentMatrix);
                }
            });
            parent._childs.ForEach(child =>
            {
                RenderOutlineRecursively(child, child.Transform * parent.Transform *  parentMatrix);
                
            });
            
        }
        

        public void RenderAllChildObjects(List<GameObject> objs)
        {
            GL.PolygonMode(MaterialFace.FrontAndBack, mode);
            count = 1;
            GL.Enable(EnableCap.StencilTest);
            foreach (var obj in objs)
            {
                RenderRecursively(obj, Matrix4.Identity, rootActive:obj.isActive);
            }
            GL.Disable(EnableCap.StencilTest);

            if (Editor.selectedGameobject != null)
            {
                RenderOutlineRecursively(Editor.selectedGameobject, selectedRoot);
                GL.Enable(EnableCap.StencilTest);
                RenderRecursively(Editor.selectedGameobject, selectedRoot, true, selectedRootActive);
                GL.Disable(EnableCap.StencilTest);
            }
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            TotalVertices = vertCount;
            vertCount = 0;
        }


        public void DisplayFullScreen(Shader shader)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0); // unbind your FBO to set the default framebuffer
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.ClearStencil(0);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);

            shader.Use(); // shader program for rendering the quad  

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, frameBuffer.frameBufferTexture); // color attachment texture
            
            fsq.Render(Matrix4.Zero, fsq.Material);
        }
    }
}