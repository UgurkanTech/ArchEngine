using System;
using ArchEngine.Core;
using ImGuiNET;
using ImGuizmoNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchEngine.GUI.Editor.Windows
{
    public class Toolbar
    {
        public static void Draw()
        {
            if (ImGui.Begin("Tools", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoScrollbar |ImGuiWindowFlags.NoTitleBar))
            {
                ImGui.Columns(3);
                
                if (ImGui.Button("Start", new Vector2(40,20)))
                {
                    Window.started = true;
                    Console.WriteLine("Game started!");
                }
                ImGui.SameLine();
                if (ImGui.Button("Stop", new Vector2(40,20)))
                {
                    Window.started = false;
                    Console.WriteLine("Game stopped!");
                }
                ImGui.SameLine();
                if (ImGui.Button("Save", new Vector2(40,20)))
                {
                    AssetManager.SaveScene();
                }
                ImGui.SameLine();
                if (ImGui.Button("Load", new Vector2(40,20)))
                {
                    AssetManager.LoadScene();
                }
                

                ImGui.NextColumn();
                if (ImGui.Button("Textured", new Vector2(60,20)))
                {
                    Window._renderer.mode = PolygonMode.Fill;
                }
                ImGui.SameLine();
                if (ImGui.Button("Wireframe", new Vector2(60, 20)))
                {
                    Window._renderer.mode = PolygonMode.Line;
                }
                
                ImGui.NextColumn();
                if (ImGui.Button("Translate", new Vector2(50,20)))
                {
                    Gizmo.op = TransformOperation.Translate;
                }
                ImGui.SameLine();
                if (ImGui.Button("Rotate", new Vector2(50, 20)))
                {
                    Gizmo.op = TransformOperation.Rotate;
                }
                ImGui.SameLine();
                if (ImGui.Button("Scale", new Vector2(50, 20)))
                {
                    Gizmo.op = TransformOperation.Scale;
                }
            }
        }
    }
}