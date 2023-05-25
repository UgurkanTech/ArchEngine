using System;
using Assimp;
using BulletSharp.Math;
using OpenTK.Mathematics;
using Quaternion = OpenTK.Mathematics.Quaternion;
using Vector3 = OpenTK.Mathematics.Vector3;

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
        public static  Matrix Matrix4ToMatrix(this Matrix4 sourceMatrix)
        {
            Matrix newMatrix = new Matrix(
                sourceMatrix.M11, sourceMatrix.M12, sourceMatrix.M13, sourceMatrix.M14,
                sourceMatrix.M21, sourceMatrix.M22, sourceMatrix.M23, sourceMatrix.M24,
                sourceMatrix.M31, sourceMatrix.M32, sourceMatrix.M33, sourceMatrix.M34,
                sourceMatrix.M41, sourceMatrix.M42, sourceMatrix.M43, sourceMatrix.M44
            );
            return newMatrix;
        }
        
        public static  Matrix4 MatrixToMatrix4(this Matrix sourceMatrix)
        {
            Matrix4 newMatrix = new Matrix4(
                sourceMatrix.M11, sourceMatrix.M12, sourceMatrix.M13, sourceMatrix.M14,
                sourceMatrix.M21, sourceMatrix.M22, sourceMatrix.M23, sourceMatrix.M24,
                sourceMatrix.M31, sourceMatrix.M32, sourceMatrix.M33, sourceMatrix.M34,
                sourceMatrix.M41, sourceMatrix.M42, sourceMatrix.M43, sourceMatrix.M44
            );
            return newMatrix;
        }

        public static  BulletSharp.Math.Vector3 Vector3ToBullet3(this Vector3 source)
        {
            BulletSharp.Math.Vector3 vec = new BulletSharp.Math.Vector3(source.X, source.Y, source.Z);
            return vec;
        }
        public static  Vector3 Bullet3ToOpenTK3(this BulletSharp.Math.Vector3 source)
        {
            Vector3 vec = new Vector3(source.X, source.Y, source.Z);
            return vec;
        }
    }
}