using OpenTK.Mathematics;

namespace ArchEngine.Core.Utils
{
    public static class VectorUtils
    {
        public static System.Numerics.Vector3 ToSystemVector3(this Vector3 other)
        {
            return new System.Numerics.Vector3(other.X, other.Y, other.Z);
        }
        public static Vector3 ToOpenTkVector3(this System.Numerics.Vector3 other)
        {
            return new Vector3(other.X, other.Y, other.Z);
        }
        
        public static Vector3 RadiansToAngles(this Vector3 other)
        {
            return new Vector3(MathHelper.RadiansToDegrees(other.X), MathHelper.RadiansToDegrees(other.Y), MathHelper.RadiansToDegrees(other.Z));
        }
        
        public static Vector3 DegreesToRadians(this Vector3 other)
        {
            return new Vector3(MathHelper.DegreesToRadians(other.X), MathHelper.DegreesToRadians(other.Y), MathHelper.DegreesToRadians(other.Z));
        }

        public static Vector3 GetNonZero(this Vector3 other)
        {
            if (other.X == 0 || other.Y == 0 || other.Z == 0)
            {
                return other + new Vector3(0.0000001f, 0.0000001f, 0.0000001f);
            }

            return other;
        }
    }
}