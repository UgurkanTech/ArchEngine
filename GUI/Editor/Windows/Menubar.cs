using ArchEngine.Core;
using ImGuiNET;
using OpenTK.Mathematics;

namespace ArchEngine.GUI.Editor.Windows
{
    public class Menubar
    {
        public static void Draw()
        {
            
            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("File"))
            {
               
                ImGui.Image(Icons.Texture, new Vector2(20, 20),
                    Icons.GetUV0FromID(249), Icons.GetUV1FromID(249));
                ImGui.SameLine();

                if (ImGui.MenuItem("Save Scene"))
                {
                    AssetManager.SaveScene();
                    
                }
                ImGui.Image(Icons.Texture, new Vector2(20, 20),
                    Icons.GetUV0FromID(37), Icons.GetUV1FromID(37));
                ImGui.SameLine();
                if (ImGui.MenuItem("Load Scene"))
                {
                    AssetManager.LoadScene();
                }
                
                if (ImGui.MenuItem("Save Project"))
                {
                }
                if (ImGui.MenuItem("Load Project"))
                {
                }
                if (ImGui.MenuItem("Build"))
                {
                }
                ImGui.Image(Icons.Texture, new Vector2(20, 20),
                    Icons.GetUV0FromID(102), Icons.GetUV1FromID(102));
                ImGui.SameLine();
                if (ImGui.MenuItem("Exit"))
                {
                    Window.instance.Close();
                }
                
                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Edit"))
            {
                if (ImGui.MenuItem("Play"))
                {
                }
                if (ImGui.MenuItem("Pause"))
                {
                }
                if (ImGui.MenuItem("Project Settings"))
                {
                }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Assets"))
            {
                if (ImGui.MenuItem("Create Script"))
                {
                }
                if (ImGui.MenuItem("Show in Explorer"))
                {
                }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Window"))
            {
                if (ImGui.MenuItem("Reset Layout"))
                {
                }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Help"))
            {
                if (ImGui.MenuItem("About Arch Engine"))
                {
                }
                ImGui.EndMenu();
            }
            
            ImGui.EndMainMenuBar();
        }
    }
}