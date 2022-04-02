using OpenTK.Graphics.OpenGL4;

namespace ArchEngine.Core.Rendering.Textures
{
    public class PbrTexture : Texture
    {
        public UniqueTexture albedoMap;
        public UniqueTexture normalMap;
        public UniqueTexture metallicMap;
        public UniqueTexture roughnessMap;
        public UniqueTexture aoMap;
        public override void Use()
        {
            albedoMap.Use();
            normalMap.Use();
            metallicMap.Use();
            roughnessMap.Use();
            aoMap.Use();
        }
    }
}