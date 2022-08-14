using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Utils.Serialization
{
    public class SerializedMatrix4
    {
        [JsonProperty] private SerializedVector4 col0 { get; set; }
        [JsonProperty] private SerializedVector4 col1 { get; set; }
        [JsonProperty] private SerializedVector4 col2 { get; set; }
        [JsonProperty] private SerializedVector4 col3 { get; set; }


        public SerializedMatrix4(){}

        public SerializedMatrix4(Matrix4 matrix4)
        {
            col0 = new SerializedVector4(matrix4.Column0);
            col1 = new SerializedVector4(matrix4.Column1);
            col2 = new SerializedVector4(matrix4.Column2);
            col3 = new SerializedVector4(matrix4.Column3);
            
        }
        
        public SerializedMatrix4(SerializedVector4 col0, SerializedVector4 col1, SerializedVector4 col2, SerializedVector4 col3)
        {
            this.col0 = col0;
            this.col1 = col1;
            this.col2 = col2;
            this.col3 = col3;
        }
        
        public void SetMatrix(Matrix4 matrix4)
        {
            col0 = new SerializedVector4(matrix4.Column0);
            col1 = new SerializedVector4(matrix4.Column1);
            col2 = new SerializedVector4(matrix4.Column2);
            col3 = new SerializedVector4(matrix4.Column3);
        }

        public Matrix4 GetMatrix()
        {
            Matrix4 matrix4 = new Matrix4();
            matrix4.Column0 = col0.GetVector();
            matrix4.Column1 = col1.GetVector();
            matrix4.Column2 = col2.GetVector();
            matrix4.Column3 = col3.GetVector();
            return matrix4;
        }

    }
}