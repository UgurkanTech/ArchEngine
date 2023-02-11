using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering.Camera;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor.Windows;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Scene = ArchEngine.GUI.Editor.Windows.Scene;

namespace ArchEngine.GUI.Editor
{
    public class ObjectSelecter
    {
        public static void SelectObject()
        {
            
            if (ImGui.IsMouseReleased(ImGuiMouseButton.Left) && !Gizmo.usingGizmo)
            {
                int window_width = Window._renderer.RenderSize.X;
                int window_height = Window._renderer.RenderSize.Y;

                int x = (int)Scene.MouseScenePos.X;
                int y = (int)Scene.MouseScenePos.Y;
                

                byte[] color = new byte[4];
                float[] depth = new float[1];
                int[] index = new int[1];
                
                Window._renderer.frameBuffer.Use();
                
                GL.ReadPixels(x, window_height - y - 1, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, color);
                GL.ReadPixels(x, window_height - y - 1, 1, 1, PixelFormat.DepthComponent, PixelType.Float, depth);
                GL.ReadPixels(x, window_height - y - 1, 1, 1, PixelFormat.StencilIndex, PixelType.UnsignedInt, index);
                
                
                GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
                
                //Console.WriteLine("Clicked on pixel {0}, {1}, color {2} {3} {4} {5}, depth {6}, stencil index {7}\n", x, y, color[0], color[1], color[2], color[3], depth[0], index[0]);
                
                
                Vector4 viewport = new Vector4(0, 0, window_width, window_height);
                Vector3 wincoord = new Vector3(x, window_height - y - 1, depth[0]);
                Vector3 objcoord = MathUtils.UnProject(wincoord, CameraManager.EditorCamera.GetViewMatrix(), CameraManager.EditorCamera.GetProjectionMatrix(), viewport);

                //Console.WriteLine("Coordinates in object space: {0}, {1}, {2}\n", objcoord.X, objcoord.Y, objcoord.Z);
                
                SelectObjectWithStencilID(index[0]);
            }


            
        }
        private static void SelectObjectWithStencilID(int id)
        {
            GameObject go;

            scanRes = null;
            foreach (var obj in Window.activeScene.gameObjects)
            {
                ScanSceneObjectsRecursively(obj, id);
                
            }

            Editor.selectedGameobject = scanRes;
            Hierarchy.needsSelectUpdate = true;
            
        }

        private static GameObject scanRes;
        private static void ScanSceneObjectsRecursively(GameObject gameObject, int id)
        {
            
            gameObject._components.ForEach(component =>
            {
                if (component.GetType() == typeof(MeshRenderer))
                {
                    MeshRenderer mr = component as MeshRenderer;
                    if (mr.StencilID == id)
                    {
                        scanRes = gameObject;
                    }
                }  
                else if (component.GetType() == typeof(LineRenderer))
                {
                    LineRenderer mr = component as LineRenderer;
                    if (mr.StencilID == id)
                    {
                        scanRes = gameObject;
                    }
                }  

            });
            gameObject._childs.ForEach(child =>
            {
                ScanSceneObjectsRecursively(child, id);
                
            });
        }
    }
    

}