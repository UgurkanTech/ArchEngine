using System;

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
    }
}