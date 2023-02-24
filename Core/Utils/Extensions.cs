using System;
using Assimp;
using OpenTK.Mathematics;

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
        
    }
}