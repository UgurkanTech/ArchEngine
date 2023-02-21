using System;
using ArchEngine.Core.Rendering.Camera;
using ImGuiNET;
using ImGuizmoNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ArchEngine.GUI.Editor
{
    public class Gizmo
    {
        public static bool usingGizmo = false;
        public static TransformOperation op = TransformOperation.Translate;
        public static void Draw()
        {
            var cameraView = CameraManager.EditorCamera.GetViewMatrix();
            var cameraProj = CameraManager.EditorCamera.GetProjectionMatrix();
            if (Editor.selectedGameobject != null)
            {
                ImGuizmo.SetDrawlist();
                ImGuizmo.SetOrthographic(true);
                if (ImGui.IsWindowHovered())
                {
                    if (ImGui.IsKeyDown((int)Keys.Q)) op = TransformOperation.Translate;
                    if (ImGui.IsKeyDown((int)Keys.E)) op = TransformOperation.Rotate;
                    if (ImGui.IsKeyDown((int)Keys.R)) op = TransformOperation.Scale;
                }
                ImGuizmo.SetGizmoSizeClipSpace(0.3f);
                ImGuizmo.SetRect(ImGui.GetWindowPos().X, ImGui.GetWindowPos().Y, ImGui.GetItemRectSize().X, ImGui.GetItemRectSize().Y);
                
                var transform = Editor.selectedGameobject.Transform;

               
                ImGuizmo.Manipulate(ref cameraView.Row0.X, ref cameraProj.Row0.X, op, TransformMode.Local, ref transform.Row0.X);
                
                if (ImGuizmo.IsUsing())
                {
                    Editor.selectedGameobject.Transform = transform;
                    usingGizmo = true;
                }
                else
                {
                    usingGizmo = false;
                }
   
            }

            //ImGuizmo.DrawGrid(ref cameraView.Row0.X, ref cameraProj.Row0.X, ref grid.Row0.X, 50);
        }

    }
    
}