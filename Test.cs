using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using ArchEngine.Core;
using ArchEngine.Core.Utils;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace ArchEngine
{
    public class Test
    {
        [JsonConverter(typeof(JsonConverters.Matrix4Converter))]
        public Matrix4 matrix = Matrix4.Identity;
        [JsonConverter(typeof(JsonConverters.Vector4Converter))]
        public Vector4 matrix2 = new Vector4(6, 5, 7, 3);
        public Test()
        {}

        public static void Run()
        {
            Test t = new Test();
            
            Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();
            serializer.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            serializer.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto;
            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            serializer.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(@"D:\path.json"))
            using (Newtonsoft.Json.JsonWriter writer = new Newtonsoft.Json.JsonTextWriter(sw))
            {
                serializer.Serialize(writer, t, typeof(Test));
            }

            
            Console.WriteLine(t.matrix2);
            
            
            
            t = Newtonsoft.Json.JsonConvert.DeserializeObject<Test>(File.ReadAllText(@"D:\path.json"),
                new Newtonsoft.Json.JsonSerializerSettings
                {
                    NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore,
                    TypeNameHandling = Newtonsoft.Json.TypeNameHandling.Auto,
                    Formatting = Newtonsoft.Json.Formatting.Indented,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Error = (sender, eventArgs) => {
                        Console.WriteLine(eventArgs.ErrorContext.Error.Message);  // or write to a log
                        eventArgs.ErrorContext.Handled = true;
                    }
                });
            
            Console.WriteLine(t.matrix2);
        }


    }
}