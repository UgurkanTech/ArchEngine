using System;
using System.Linq;
using System.Reflection;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.GUI.Editor.Windows;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using static ArchEngine.GUI.Editor.Attributes;
using GL = OpenTK.Graphics.ES11.GL;
using Scene = ArchEngine.GUI.Editor.Windows.Scene;
using Vector2 = System.Numerics.Vector2;

namespace ArchEngine.GUI.Editor
{
    public class Editor
    {
        
        public static GameObject selectedGameobject;

        public Editor()
        {
            new ConsoleWindow();
        }
        
        public static void DrawEditor()
        {
            DockSpace.Draw();

            Scene.Draw();
            
            Hierarchy.Draw();
            Inspector.Draw();
            
            ConsoleWindow.Draw();

            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("File"))
            {
                if (ImGui.MenuItem("Save Scene"))
                {
                    AssetManager.SaveScene();
                }
                if (ImGui.MenuItem("Load Scene"))
                {
                    AssetManager.LoadScene();
                }
                
                
                ImGui.EndMenu();
            }
            
            ImGui.EndMainMenuBar();
        }
        

        [InspectorAttribute] public static int a = 5;



    }
}