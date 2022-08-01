using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Textures
{
    public class Material
    {
        public Texture albedoMap;
        public Texture normalMap;
        public Texture metallicMap;
        public Texture roughnessMap;
        public Texture aoMap;

        public Shader Shader;
        
        public void Use(Matrix4 model)
        {
            albedoMap.Use();
            normalMap.Use();
            metallicMap.Use();
            roughnessMap.Use();
            aoMap.Use();
            
            Shader.SetMatrix4("model", model);
            Shader.Use();
        }

        public void LoadTextures(string folderPath)
        {
            albedoMap = TextureManager.LoadFromFile(folderPath + "/albedo.png", TextureUnit.Texture0);
            normalMap = TextureManager.LoadFromFile(folderPath + "/normal.png", TextureUnit.Texture1);
            metallicMap = TextureManager.LoadFromFile(folderPath + "/metallic.png", TextureUnit.Texture2);
            roughnessMap = TextureManager.LoadFromFile(folderPath + "/roughness.png", TextureUnit.Texture3);
            aoMap = TextureManager.LoadFromFile(folderPath + "/ao.png", TextureUnit.Texture4);

        }


    }
}