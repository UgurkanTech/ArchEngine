using System;
using ArchEngine.Core.Rendering.Textures;
using Newtonsoft.Json;

namespace ArchEngine.Core.Utils.Serialization
{
    public class SerializedRenderable
    {
        [JsonProperty] public Material Material;
        [JsonProperty] public Type Type;
    }
}