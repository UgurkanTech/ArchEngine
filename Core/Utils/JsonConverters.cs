using System;
using System.Reflection;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.Core.Utils.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Utils
{
    public class JsonConverters
    {
        public class IRenderableConverter : JsonConverter<IRenderable>
        {
            public override void WriteJson(JsonWriter writer, IRenderable value, JsonSerializer serializer)
            {
                SerializedRenderable sr = new SerializedRenderable();
                sr.Type = value.GetType();
                sr.Material = value.Material;

                serializer.Serialize(writer, sr);

            }

            public override IRenderable ReadJson(JsonReader reader, Type objectType, IRenderable existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                Assembly asm = typeof(Cube).Assembly;
                SerializedRenderable sr = serializer.Deserialize<SerializedRenderable>(reader);
                Type type = asm.GetType(sr.Type.FullName);
                IRenderable o = Activator.CreateInstance(type) as IRenderable;
                o.Material = sr.Material;
                return o;
            }
        }
        
        public class Vector2Converter : JsonConverter<Vector2>
        {
            public override void WriteJson(JsonWriter writer, Vector2 value, JsonSerializer serializer)
            {
                SerializedVector2 sm = new SerializedVector2();
                sm.SetVector(value);
                serializer.Serialize(writer, sm);
            }

            public override Vector2 ReadJson(JsonReader reader, Type objectType, Vector2 existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                
                return serializer.Deserialize<SerializedVector2>(reader).GetVector();
            }
        }
        
        public class Vector3Converter : JsonConverter<Vector3>
        {
            public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
            {
                SerializedVector3 sm = new SerializedVector3();
                sm.SetVector(value);
                serializer.Serialize(writer, sm);
            }

            public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                
                return serializer.Deserialize<SerializedVector3>(reader).GetVector();
            }
        }
        
        public class Vector4Converter : JsonConverter<Vector4>
        {
            public override void WriteJson(JsonWriter writer, Vector4 value, JsonSerializer serializer)
            {
                SerializedVector4 sm = new SerializedVector4();
                sm.SetVector(value);
                serializer.Serialize(writer, sm);
            }

            public override Vector4 ReadJson(JsonReader reader, Type objectType, Vector4 existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                
                return serializer.Deserialize<SerializedVector4>(reader).GetVector();
            }
        }
        
        public class Matrix4Converter : JsonConverter<Matrix4>
        {

            public override void WriteJson(JsonWriter writer, Matrix4 value, JsonSerializer serializer)
            {
                serializer.Serialize(writer, new SerializedMatrix4(value));
            }

            public override Matrix4 ReadJson(JsonReader reader, Type objectType, Matrix4 existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                return serializer.Deserialize<SerializedMatrix4>(reader).GetMatrix();
            }
        }
    }
}