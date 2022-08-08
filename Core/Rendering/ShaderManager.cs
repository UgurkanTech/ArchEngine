using ArchEngine.Core.Rendering.Camera;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering
{
    public class ShaderManager
    {
        public static Shader EquirectAngularToCubemapShader;
        public static Shader IrradianceShader;
        public static Shader BackgroundShader;
        public static Shader PrefilterShader;
        public static Shader DefaultShader;
        public static Shader TextShader;
        public static Shader PbrShader;
        public static Shader FullscreenShader;
        
        public static void LoadShaders()
        {
            //DefaultShader = new Shader("Resources/Shaders/shader.vert", "Resources/Shaders/shader.frag");
            PbrShader = new Shader("Resources/Shaders/pbr.vert", "Resources/Shaders/pbr.frag");
            
            //EquirectAngularToCubemapShader = new Shader("Resources/Shaders/cubemap.vert", "Resources/Shaders/equirectangular.frag");
            //IrradianceShader = new Shader("Resources/Shaders/cubemap.vert", "Resources/Shaders/irradiance.frag");
            //BackgroundShader = new Shader("Resources/Shaders/background.vert", "Resources/Shaders/background.frag");
            //PrefilterShader = new Shader("Resources/Shaders/cubemap.vert", "Resources/Shaders/prefilter.frag");
            
            TextShader = new Shader("Resources/Shaders/text.vert", "Resources/Shaders/text.frag");
            FullscreenShader = new Shader("Resources/Shaders/fullscreen.vert", "Resources/Shaders/fullscreen.frag");
        }

        public static void StartShaders()
        {
            //BackgroundShader.Use();
            //BackgroundShader.SetInt("environmentMap", 0);
            
            //DefaultShader.SetMatrix4("projection", CameraManager.activeCamera.GetProjectionMatrix());
            //DefaultShader.SetInt("texture0", 0);
            
            
            
            PbrShader.SetInt("material.diffuse", 0);
            PbrShader.SetInt("material.specular", 1);
            PbrShader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
            PbrShader.SetFloat("material.shininess", 32.0f);

            // Directional light needs a direction, in this example we just use (-0.2, -1.0, -0.3f) as the lights direction
            PbrShader.SetVector3("light.direction", new Vector3(-0.2f, -1.0f, -0.3f));
            PbrShader.SetVector3("light.ambient", new Vector3(0.7f));
            PbrShader.SetVector3("light.diffuse", new Vector3(1f));
            PbrShader.SetVector3("light.specular", new Vector3(0.1f));
            
            
            Matrix4 ortho = Matrix4.CreateOrthographic(800, 600, 0, 100);
            TextShader.SetMatrix4("projection", ortho);
            
            
        }

        public static void UpdateShaders(int witdh, int height)
        {
            Camera.Camera camera = CameraManager.activeCamera;
            Matrix4 ortho = Matrix4.CreateOrthographic(witdh, height, 0, 100);
            TextShader.SetMatrix4("projection", ortho);
            //DefaultShader.SetMatrix4("view", camera.GetViewMatrix());
            //DefaultShader.SetMatrix4("projection", camera.GetProjectionMatrix());
            //_shader.SetVector3("camPos", _camera.Position);
            
           // BackgroundShader.SetMatrix4("view", camera.GetViewMatrix());
            
            PbrShader.SetMatrix4("view", camera.GetViewMatrix());
            PbrShader.SetMatrix4("projection", camera.GetProjectionMatrix());
            PbrShader.SetVector3("camPos", camera.Position);
            
            //PbrShader.SetVector3("lightPositions[0]", new Vector3(1.2f ,1.2f,1.2f));
            //PbrShader.SetVector3("lightColors[0]", new Vector3(10,10,10));

            //PbrShader.SetInt("lightCount", 1);
            //BackgroundShader.Use();
            //BackgroundShader.SetMatrix4("view", camera.GetViewMatrix());
        }
    }
}