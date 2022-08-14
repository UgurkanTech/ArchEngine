using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Utils.Serialization
{
    public class SerializedVector2
    {
        [JsonProperty] private float f0 { get; set; }
        [JsonProperty] private float f1 { get; set; }

        public SerializedVector2(){}

        public SerializedVector2(Vector2 vector2)
        {
            f0 = vector2.X;
            f1 = vector2.Y;
        }
        
        public SerializedVector2(float f0, float f1)
        {
            this.f0 = f0;
            this.f1 = f1;
        }
        
        public void SetVector(Vector2 vector2)
        {
            f0 = vector2.X;
            f1 = vector2.Y;
        }

        public Vector2 GetVector()
        {
            return new Vector2(f0,f1);
        }
    }
}