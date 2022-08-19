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
                //SerializedRenderable sr = new SerializedRenderable();
                //sr.Type = value.GetType();
                //serializer.Serialize(writer, sr);

                serializer.Serialize(writer, value);
            }

            public override IRenderable ReadJson(JsonReader reader, Type objectType, IRenderable existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                if (false)
                {
                    Assembly asm = typeof(Cube).Assembly;
                    SerializedRenderable sr = serializer.Deserialize<SerializedRenderable>(reader);
                    Type type = asm.GetType(sr.Type.FullName);

                    if (type == typeof(Cube))
                    {
                        return Primitives.Cube;
                    }
                    else if (type == typeof(Line))
                    {
                        return Primitives.Line;
                    }
                }
                return serializer.Deserialize<IRenderable>(reader);
            }
        }
        public class MeshConverter : JsonConverter<Mesh>
        {
            public override void WriteJson(JsonWriter writer, Mesh value, JsonSerializer serializer)
            {
                //SerializedRenderable sr = new SerializedRenderable();
                //sr.Type = value.GetType();
                //serializer.Serialize(writer, sr);

                serializer.Serialize(writer, value.MeshHash);
            }

            public override Mesh ReadJson(JsonReader reader, Type objectType, Mesh existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                var meshHash = serializer.Deserialize<string>(reader);
                
                return AssetManager.GetMeshByFilePath(meshHash);
            }
        }
        
        public class MaterialConverter : JsonConverter<Material>
        {
            public override void WriteJson(JsonWriter writer, Material value, JsonSerializer serializer)
            {
       
                serializer.Serialize(writer, value.Shader.hash + "@@@" + value.MaterialHash);
            }

            public override Material ReadJson(JsonReader reader, Type objectType, Material existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                string hash = serializer.Deserialize<string>(reader);
                var shader = hash.Split("@@@")[0];
                var matHash = hash.Split("@@@")[1];

                Material mat = new Material();
                mat.MaterialHash = matHash;
                
                
                foreach (var sha in ShaderManager.shaders)
                {
                    if (sha.hash.Equals(shader))
                    {
                        mat.Shader = sha;
                        mat.LoadTextures(mat.MaterialHash);
                        return mat;
                    }
                }

                return null;
            }
        }
        
        public class ShaderConverter : JsonConverter<Shader>
        {
            public override void WriteJson(JsonWriter writer, Shader value, JsonSerializer serializer)
            {
       
                serializer.Serialize(writer, value.hash);
            }

            public override Shader ReadJson(JsonReader reader, Type objectType, Shader existingValue, bool hasExistingValue,
                JsonSerializer serializer)
            {
                string hash = serializer.Deserialize<string>(reader);
                foreach (var sha in ShaderManager.shaders)
                {
                    if (sha.hash.Equals(hash))
                    {
                        Console.WriteLine("shader found!!!");
                        return sha;
                    }
                }
                Console.WriteLine("No shader found!!!");
                
                return null;
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