using System;
using Assimp;
using OpenTK.Mathematics;
using Quaternion = OpenTK.Mathematics.Quaternion;

namespace ArchEngine.Core.Utils
{
    public static class Extensions
    {
        public static uint ToUint(this int i)
        {
            return Convert.ToUInt32(i);
        }
        public static int ToInt32(this uint i)
        {
            return Convert.ToInt32(i);
        }

        public static Vector3 ToVector3TK(this Vector3D vec)
        {
            return new Vector3(vec.X, vec.Y, vec.Z);
        }
        public static Vector3[] ToVector3TK(this Vector3D[] vec)
        {
            Vector3[] arr = new Vector3[vec.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = vec[i].ToVector3TK();
            }
            return arr;
        }
        public static Vector2 ToVector2TK(this Vector3D vec)
        {
            return new Vector2(vec.X, vec.Y);
        }

        public static void Translate(this ref Matrix4 matrix, Vector3 translation)
        {
            matrix *= Matrix4.CreateTranslation(translation);
        }

        public static void Scale(this ref Matrix4 matrix, Vector3 scale)
        {
            matrix *= Matrix4.CreateScale(scale);
        }

        public static void RotateX(this ref Matrix4 matrix, float angleInDegrees)
        {
            Matrix4 old = matrix;
            float angleInRadians = MathHelper.DegreesToRadians(angleInDegrees);
            matrix = Matrix4.CreateScale(old.ExtractScale());
            matrix *= Matrix4.CreateFromQuaternion(old.ExtractRotation() * Quaternion.FromAxisAngle(Vector3.UnitX, angleInRadians));
            matrix *= Matrix4.CreateTranslation(old.ExtractTranslation());
        }

        public static void RotateY(this ref Matrix4 matrix, float angleInDegrees)
        {
            Matrix4 old = matrix;
            float angleInRadians = MathHelper.DegreesToRadians(angleInDegrees);
            matrix = Matrix4.CreateScale(old.ExtractScale());
            matrix *= Matrix4.CreateFromQuaternion(old.ExtractRotation() * Quaternion.FromAxisAngle(Vector3.UnitY, angleInRadians));
            matrix *= Matrix4.CreateTranslation(old.ExtractTranslation());
            
        }
        

        public static void RotateZ(this ref Matrix4 matrix, float angleInDegrees)
        {
            Matrix4 old = matrix;
            float angleInRadians = MathHelper.DegreesToRadians(angleInDegrees);
            matrix = Matrix4.CreateScale(old.ExtractScale());
            matrix *= Matrix4.CreateFromQuaternion(old.ExtractRotation() * Quaternion.FromAxisAngle(Vector3.UnitZ, angleInRadians));
            matrix *= Matrix4.CreateTranslation(old.ExtractTranslation());
        }
        
    }
}