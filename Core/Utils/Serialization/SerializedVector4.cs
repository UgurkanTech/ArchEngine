using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Utils.Serialization
{
    public class SerializedVector4
    {
        [JsonProperty] private float f0 { get; set; }
        [JsonProperty] private float f1 { get; set; }
        [JsonProperty] private float f2 { get; set; }
        [JsonProperty] private float f3 { get; set; }
        
        public SerializedVector4(){}

        public SerializedVector4(Vector4 vector4)
        {
            f0 = vector4.X;
            f1 = vector4.Y;
            f2 = vector4.Z;
            f3 = vector4.W;
        }
        
        public SerializedVector4(float f0, float f1, float f2, float f3)
        {
            this.f0 = f0;
            this.f1 = f1;
            this.f2 = f2;
            this.f3 = f3;
        }
        
        public void SetVector(Vector4 vector4)
        {
            f0 = vector4.X;
            f1 = vector4.Y;
            f2 = vector4.Z;
            f3 = vector4.W;
        }

        public Vector4 GetVector()
        {
            return new Vector4(f0,f1,f2,f3);
        }
    }
}