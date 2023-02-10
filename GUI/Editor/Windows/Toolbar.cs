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
            ImGui.SetNextWindowSize(new Vector2(370, 35), ImGuiCond.FirstUseEver);
            if (ImGui.Begin("Tools", ImGuiWindowFlags.NoDecoration | ImGuiWindowFlags.NoScrollbar |ImGuiWindowFlags.NoTitleBar))
            {
                ImGui.Columns(3);
               
                
                if( ImGui.ImageButtonEx(ImGui.GetID("Start"), Icons.Texture, new Vector2(20, 20), Icons.GetUV0FromID(122), Icons.GetUV1FromID(122),
                       Vector2.One, Vector4.Zero, Vector4.One))
                {
                    Window.started = true;
                    Console.WriteLine("Game started!");
                }
                ImGui.SameLine();
                
                if( ImGui.ImageButtonEx(ImGui.GetID("Stop"), Icons.Texture, new Vector2(20, 20), Icons.GetUV0FromID(114), Icons.GetUV1FromID(114),
                       Vector2.One, Vector4.Zero, Vector4.One))
                {
                    Window.started = false;
                    Console.WriteLine("Game stopped!");
                }
                ImGui.SameLine();
                if( ImGui.ImageButtonEx(ImGui.GetID("Save"), Icons.Texture, new Vector2(20, 20), Icons.GetUV0FromID(249), Icons.GetUV1FromID(249),
                       Vector2.One, Vector4.Zero, Vector4.One))
                {
                    AssetManager.SaveScene();
                }
                ImGui.SameLine();
                if( ImGui.ImageButtonEx(ImGui.GetID("Load"), Icons.Texture, new Vector2(20, 20), Icons.GetUV0FromID(37), Icons.GetUV1FromID(37),
                       Vector2.One, Vector4.Zero, Vector4.One))
                {
                    AssetManager.LoadScene();
                }
                

                ImGui.NextColumn();
                if( ImGui.ImageButtonEx(ImGui.GetID("Textured"), Icons.Texture, new Vector2(20, 20), Icons.GetUV0FromID(193), Icons.GetUV1FromID(193),
                       Vector2.One, Vector4.Zero, Vector4.One))
                {
                    Window._renderer.mode = PolygonMode.Fill;
                }
                ImGui.SameLine();
                if( ImGui.ImageButtonEx(ImGui.GetID("Wireframe"), Icons.Texture, new Vector2(20, 20), Icons.GetUV0FromID(250), Icons.GetUV1FromID(250),
                       Vector2.One, Vector4.Zero, Vector4.One))
                {
                    Window._renderer.mode = PolygonMode.Line;
                }
                
                ImGui.NextColumn();
                if( ImGui.ImageButtonEx(ImGui.GetID("Translate"), Icons.Texture, new Vector2(20, 20), Icons.GetUV0FromID(144), Icons.GetUV1FromID(144),
                       Vector2.One, Vector4.Zero, Vector4.One))
                {
                    Gizmo.op = TransformOperation.Translate;
                }
                ImGui.SameLine();
                if( ImGui.ImageButtonEx(ImGui.GetID("Rotate"), Icons.Texture, new Vector2(20, 20), Icons.GetUV0FromID(105), Icons.GetUV1FromID(105),
                       Vector2.One, Vector4.Zero, Vector4.One))
                {
                    Gizmo.op = TransformOperation.Rotate;
                }
                ImGui.SameLine();
                if( ImGui.ImageButtonEx(ImGui.GetID("Scale"), Icons.Texture, new Vector2(20, 20), Icons.GetUV0FromID(107), Icons.GetUV1FromID(107),
                       Vector2.One, Vector4.Zero, Vector4.One))
                {
                    Gizmo.op = TransformOperation.Scale;
                }
            }
        }
    }
}