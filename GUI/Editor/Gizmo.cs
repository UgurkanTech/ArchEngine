using System;
using System.Linq;
using System.Numerics;
using Assimp;
using ImGuiNET;

namespace ArchEngine.GUI.Editor
{
    public class Gizmo
    {
        public static void Draw()
        {
            uint col = ImGui.ColorConvertFloat4ToU32(new Vector4(1.0f, 1.0f, 0.4f, 1.0f));


            Vector2 pos = ImGui.GetWindowPos() + ImGui.GetWindowContentRegionMin();
            Vector2 pos2 = ImGui.GetWindowPos() + ImGui.GetWindowContentRegionMax();
            Vector2 middle = (pos2 + pos) / 2;
            
            
            //ImGui.GetWindowDrawList().AddCircle(pos, 20, col, 15, 2);
            //ImGui.GetWindowDrawList().AddCircle(pos2, 20, col, 15, 2);
            //ImGui.GetWindowDrawList().AddCircle(middle, 20, col, 15, 2);
            
            //ImGui.GetWindowDrawList().AddText(middle, col, "Test");
            var assimpNetimporter = new Assimp.AssimpContext();

           
        }

    }
    
}