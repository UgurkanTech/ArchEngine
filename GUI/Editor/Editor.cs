using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.GUI.Editor.Windows;
using ImGuiNET;
using OpenTK.Mathematics;
using Scene = ArchEngine.GUI.Editor.Windows.Scene;
using Window = ArchEngine.Core.Window;

namespace ArchEngine.GUI.Editor
{
    public class Editor
    {

        public static GameObject selectedGameobject;

        public Editor()
        {
            new ConsoleWindow();
            new AssetsWindow();
            Icons.LoadIcons();
        }
        
        public static void DrawEditor()
        {
            DockSpace.Draw();
            Toolbar.Draw();
            Scene.Draw();
            
            Hierarchy.Draw();
            Inspector.Draw();
            
            ConsoleWindow.Draw();

            AssetsWindow.Draw();
            
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
                ImGui.Image(Icons.Texture, new Vector2(20, 20),
                    Icons.GetUV0FromID(102), Icons.GetUV1FromID(102));
                ImGui.SameLine();
                if (ImGui.MenuItem("Exit"))
                {
                    Window.instance.Close();
                }
                
                ImGui.EndMenu();
            }
            
            ImGui.EndMainMenuBar();
        }
        

        [InspectorAttribute] public static int a = 5;



    }
}