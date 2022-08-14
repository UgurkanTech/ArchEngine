using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Utils.Serialization
{
    public class SerializedVector3
    {
        [JsonProperty] private float f0 { get; set; }
        [JsonProperty] private float f1 { get; set; }
        [JsonProperty] private float f2 { get; set; }

        public SerializedVector3(){}

        public SerializedVector3(Vector3 vector3)
        {
            f0 = vector3.X;
            f1 = vector3.Y;
            f2 = vector3.Z;
        }
        
        public SerializedVector3(float f0, float f1, float f2)
        {
            this.f0 = f0;
            this.f1 = f1;
            this.f2 = f2;
        }
        
        public void SetVector(Vector3 vector3)
        {
            f0 = vector3.X;
            f1 = vector3.Y;
            f2 = vector3.Z;
        }

        public Vector3 GetVector()
        {
            return new Vector3(f0,f1,f2);
        }
    }
}