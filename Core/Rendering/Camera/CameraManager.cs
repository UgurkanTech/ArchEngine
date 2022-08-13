using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ArchEngine.Core.Rendering.Camera
{
    public class CameraManager
    { 
        public static Camera activeCamera;

        public static void Init(float aspectRatio)
        {
            activeCamera = new Camera(Vector3.UnitZ * 4, aspectRatio);
        }

        private static bool _firstMove = true;

        private static Vector2 _lastPos;
        
        public static void UpdateCamera(KeyboardState input, MouseState mouse, FrameEventArgs e)
        {
            
            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;
            
            activeCamera.Update();

            if (input.IsKeyDown(Keys.W))
            {
                activeCamera.Position += activeCamera.Front * cameraSpeed * (float)e.Time; // Forward
                
            }

            if (input.IsKeyDown(Keys.S))
            {
                activeCamera.Position -= activeCamera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                activeCamera.Position -= activeCamera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                activeCamera.Position += activeCamera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                activeCamera.Position += activeCamera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftControl))
            {
                activeCamera.Position -= activeCamera.Up * cameraSpeed * (float)e.Time; // Down
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
                    // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
                    activeCamera.Yaw += deltaX * sensitivity;
                    activeCamera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
                }
            }
            
        }

        public static void UpdateMouseWheelInput(MouseWheelEventArgs e)
        {
            activeCamera.Fov -= e.OffsetY;
        }

    }
}