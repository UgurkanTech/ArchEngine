using ArchEngine.GUI.Editor.Windows;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ArchEngine.Core.Rendering.Camera
{
    public class CameraManager
    { 
        public static Camera EditorCamera;

        public static void Init(float aspectRatio)
        {
            EditorCamera = new EditorCamera(Vector3.UnitZ * 4, aspectRatio);
        }

        private static bool _firstMove = true;

        private static Vector2 _lastPos;
        
        public static void UpdateCamera(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            
            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;
            
            EditorCamera.Update();

            if (Scene.isFocussed)
            {
                if (input.IsKeyDown(Keys.W))
                {
                    EditorCamera.Position += EditorCamera.Front * cameraSpeed * (float)e.Time; // Forward
                }

                if (input.IsKeyDown(Keys.S))
                {
                    EditorCamera.Position -= EditorCamera.Front * cameraSpeed * (float)e.Time; // Backwards
                }
                if (input.IsKeyDown(Keys.A))
                {
                    EditorCamera.Position -= EditorCamera.Right * cameraSpeed * (float)e.Time; // Left
                }
                if (input.IsKeyDown(Keys.D))
                {
                    EditorCamera.Position += EditorCamera.Right * cameraSpeed * (float)e.Time; // Right
                }
                if (input.IsKeyDown(Keys.LeftShift))
                {
                    EditorCamera.Position += EditorCamera.Up * cameraSpeed * (float)e.Time; // Up
                }
                if (input.IsKeyDown(Keys.LeftControl))
                {
                    EditorCamera.Position -= EditorCamera.Up * cameraSpeed * (float)e.Time; // Down
                }
                
            }

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
                var deltaX = mouse.X - _lastPos.X;
                var deltaY = mouse.Y - _lastPos.Y;
                _lastPos = new Vector2(mouse.X, mouse.Y);

                if (mouse.IsButtonDown(MouseButton.Button2))
                {
                    if (Scene.isFocussed)
                    {
                        // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                        EditorCamera.Yaw += deltaX * sensitivity;
                        EditorCamera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
                    }


                }
            }
            
        }

        public static void UpdateMouseWheelInput(MouseWheelEventArgs e)
        {
            if (Scene.isFocussed)
            {
                EditorCamera.Fov -= e.OffsetY;
            }
            
        }

    }
}