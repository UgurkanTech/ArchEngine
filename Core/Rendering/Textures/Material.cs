using Newtonsoft.Json;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Textures
{
    public class Material
    {
        [JsonIgnore] public Texture albedoMap;
        [JsonIgnore] public Texture normalMap;
        [JsonIgnore] public Texture metallicMap;
        [JsonIgnore] public Texture roughnessMap;
        [JsonIgnore] public Texture aoMap;

        public Shader Shader { get; set; }
        public string MaterialHash  { get; set; }
        public Material()
        {
            LoadTextures("Resources/Textures/Default");
        }
        
        
        public virtual void Use(Matrix4 model)
        {
            albedoMap?.Use();
            normalMap?.Use();
            metallicMap?.Use();
            roughnessMap?.Use();
            aoMap?.Use();
            
            Shader.SetMatrix4("model", model);
            Shader.Use();
        }

        public void LoadTextures(string folderPath)
        {
            MaterialHash = folderPath;

            folderPath += folderPath.Contains(@"\") ?  @"\" : @"/";
            albedoMap = TextureManager.LoadFromFile(folderPath + "albedo.png", TextureUnit.Texture0);
            normalMap = TextureManager.LoadFromFile(folderPath + "normal.png", TextureUnit.Texture1);
            metallicMap = TextureManager.LoadFromFile(folderPath + "metallic.png", TextureUnit.Texture2);
            roughnessMap = TextureManager.LoadFromFile(folderPath + "roughness.png", TextureUnit.Texture3);
            aoMap = TextureManager.LoadFromFile(folderPath + "ao.png", TextureUnit.Texture4);

        }

        public void LoadAlbedo(string folderPath, TextureMagFilter mag = TextureMagFilter.Linear, TextureMinFilter min = TextureMinFilter.Linear)
        {
            MaterialHash = folderPath;
            albedoMap = TextureManager.LoadFromFile(folderPath + "/albedo.png", TextureUnit.Texture0, true, mag, min);
            roughnessMap = TextureManager.LoadFromFile(folderPath + "/roughness.png", TextureUnit.Texture3,true, mag, min);
        }


    }
}