using System;
using System.Runtime.InteropServices;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Camera
{

    public class EditorCamera : Camera
    {

        public override Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + _front, _up);
        }
        public EditorCamera(Vector3 position, float aspectRatio)
        {
            Position = position;
            AspectRatio = aspectRatio;
        }
       
    }
}