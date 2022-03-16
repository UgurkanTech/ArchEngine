using OpenTK.Graphics.OpenGL4;

namespace ArchEngine.Core.Rendering.Textures
{
    public class PBRTexture : Texture
    {
        public UniqueTexture AlbedoMap;
        public UniqueTexture NormalMap;
        public UniqueTexture MetallicMap;
        public UniqueTexture RoughnessMap;
        public UniqueTexture AoMap;
        public override void Use()
        {
            AlbedoMap.Use();
            NormalMap.Use();
            MetallicMap.Use();
            RoughnessMap.Use();
            AoMap.Use();
        }
    }
}