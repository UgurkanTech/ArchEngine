using System;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Textures
{
    public class CubemapMaterial : Material
    {
        public Texture albedoMap { get; set; }

        public Shader Shader { get; set; }
        
        public CubemapMaterial()
        {
            LoadCubeTexture("Resources/Textures/Skybox");
        }
        public override void Use(Matrix4 model)
        {
            
            albedoMap?.Use();
            Shader.SetInt("skybox", albedoMap.handle);
            Shader.Use();
        }

        public void LoadCubeTexture(string folderPath)
        {
            albedoMap = TextureManager.LoadCubemapFromFile(folderPath, false);
        }
    }
}