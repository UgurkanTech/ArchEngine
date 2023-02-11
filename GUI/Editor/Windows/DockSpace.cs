using ArchEngine.Core;
using ImGuiNET;
using OpenTK.Mathematics;

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