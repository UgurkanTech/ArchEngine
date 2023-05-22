using System.Text.Json.Serialization;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Textures
{
    public class CubemapMaterial : Material
    {
        [JsonIgnore] public Texture albedoMap { get; set; }

        [JsonIgnore] public Shader Shader { get; set; }
        
        public CubemapMaterial()
        {
            LoadCubeTexture("Resources/Textures/Skybox");
        }
        public override void Use(Matrix4 model)
        {
            
            //albedoMap?.Use();
            GL.ActiveTexture(albedoMap.unit);
            GL.BindTexture(TextureTarget.TextureCubeMap, albedoMap.handle);
            GL.BindTexture(TextureTarget.Texture2D, albedoMap.handle);
            Shader.SetInt("skybox", 0);
            Shader.Use();
        }

        public void LoadCubeTexture(string folderPath)
        {
            albedoMap = TextureManager.LoadCubemapFromFile(folderPath, false);
        }
    }
}