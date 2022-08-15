using System;
using System.Reflection;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.GUI.Editor.Windows;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using Scene = ArchEngine.GUI.Editor.Windows.Scene;
using Vector2 = System.Numerics.Vector2;

namespace ArchEngine.GUI.Editor.Windows
{
    public class DockSpace
    {
        public static void Draw()
        {


            ImGuiDockNodeFlags dockmodeFlags = ImGuiDockNodeFlags.None;
            
            ImGuiWindowFlags windowFlags =  ImGuiWindowFlags.NoDocking;
            
            ImGui.SetNextWindowPos(new Vector2(0,10), ImGuiCond.Always);
            ImGui.SetNextWindowSize(new Vector2(Window.WindowSize.X, Window.WindowSize.Y ));
            
 
            windowFlags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse | ImGuiWindowFlags.NoMove |
                           ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
            
            bool pOpen = true;
            
            ImGui.Begin("Dockspace Demo", ref pOpen, windowFlags);

            
            

           

            // Dockspace
            ImGui.DockSpace(ImGui.GetID("Dockspace"));
            ImGui.End();
            

            //ImGui.SetNextWindowDockID(ImGui.GetID("Dockspace"));
        }
    }
}