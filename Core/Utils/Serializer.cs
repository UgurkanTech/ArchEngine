using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace ArchEngine.Core.Utils
{
    public static class Serializer
    {
        public static void Save(object obj, string path)
        {
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            serializer.Converters.Add(new JsonConverters.Matrix4Converter());
            serializer.Converters.Add(new JsonConverters.Vector4Converter());
            serializer.Converters.Add(new JsonConverters.Vector3Converter());
            serializer.Converters.Add(new JsonConverters.Vector2Converter());
            serializer.Converters.Add(new JsonConverters.ShaderConverter());
            serializer.Converters.Add(new JsonConverters.MeshConverter());
            serializer.Converters.Add(new JsonConverters.MaterialConverter());
            //serializer.Converters.Add(new JsonConverters.IRenderableConverter());

            using (StreamWriter sw = new StreamWriter(path))
            using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
            {
                serializer.Serialize(writer, Window.activeScene, obj.GetType());
            }
        }

        public static T Load<T>(string path)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(File.ReadAllText(path),
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Error = (sender, eventArgs) => {
                        Console.WriteLine(eventArgs.ErrorContext.Error.Message);
                        eventArgs.ErrorContext.Handled = true;
                    },
                    Converters = new List<JsonConverter>()
                    {
                        new JsonConverters.Matrix4Converter(),
                        new JsonConverters.Vector4Converter(),
                        new JsonConverters.Vector3Converter(),
                        new JsonConverters.Vector2Converter(),
                        new JsonConverters.ShaderConverter(),
                        new JsonConverters.MeshConverter(),
                        new JsonConverters.MaterialConverter(),
                        //new JsonConverters.IRenderableConverter()

                    }
                });
        }
    }
}