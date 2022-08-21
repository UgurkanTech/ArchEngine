using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor.Windows;
using ImGuiNET;
using ImGuizmoNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using Quaternion = OpenTK.Mathematics.Quaternion;

namespace ArchEngine.GUI.Editor.Windows
{
    public class Scene
    {
        public static Vector2 MouseScenePos;
        public static Vector2i SceneSize;

        private static int frameCounter = 0;
        
        public static void Draw()
        {
            ImGui.SetNextWindowPos(new Vector2(50,50), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new Vector2(300, 300), ImGuiCond.FirstUseEver);
    

            ImGui.Begin("Scene");
            
            //pass the texture of the FBO
            //window.getRenderTexture() is the texture of the FBO
            //the next parameter is the upper left corner for the uvs to be applied at
            //the third parameter is the lower right corner
            //the last two parameters are the UVs
            //they have to be flipped (normally they would be (0,0);(1,1) 
            
            Vector2 vMin = ImGui.GetWindowPos() + new Vector2(0, 20);
            
            Vector2 size = ImGui.GetWindowPos() + ImGui.GetWindowSize();
            
            //0-800 0-600    
            Vector2 mp = ImGui.GetMousePos();
            
            mp -= vMin;
            
            mp /= ImGui.GetWindowSize() - new Vector2(0, 20);
            mp *= size;

            MouseScenePos = mp;
            
            SceneSize = new Vector2i((int) (size.X), (int) (size.Y));
            if (frameCounter == 15)
            {
                if (Window._renderer.RenderSize.X != SceneSize.X || Window._renderer.RenderSize.Y != SceneSize.Y)
                {
                    Window._renderer.Resize((int) SceneSize.X, (int) SceneSize.Y);
                }

                frameCounter = 0;
            }
            
            ImGui.Image((IntPtr) 0, ImGui.GetContentRegionAvail(), new Vector2(0, 1), new Vector2(1, 0)); //for guizmo
            ImGui.GetWindowDrawList().AddImage((IntPtr) Window._renderer.frameBuffer.frameBufferTexture, vMin, size, new Vector2(0, 1), new Vector2(1, 0));
           
            //Resizes framebuffer:
            //GL.BindTexture(TextureTarget.Texture2D, Window._renderer.frameBuffer.frameBufferTexture);
            //GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgb,(int)size.X,(int)size.Y,0,PixelFormat.Rgb,PixelType.UnsignedByte,IntPtr.Zero);
            if (ImGui.IsWindowHovered())
            {
                ObjectSelecter.SelectObject();
            }
            
            Gizmo.Draw();
            
            ImGui.End();
            
            //ImGui.PopStyleVar(1);
            frameCounter++;
        }


    }
}