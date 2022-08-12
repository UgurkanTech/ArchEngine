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
using Scene = ArchEngine.GUI.Editor.Windows.Scene;
using Vector2 = System.Numerics.Vector2;

namespace ArchEngine.GUI.Editor
{
    public class Editor
    {
        
        public static GameObject selectedGameobject;
        
        public static void DrawEditor()
        {
            DockSpace.Draw();

            Scene.Draw();
            
            Hierarchy.Draw();
            Inspector.Draw();
            
            ConsoleWindow.Draw();
        }
        

        [InspectorAttribute] public static int a = 5;



    }
}