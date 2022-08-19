﻿using System.Collections.Generic;
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
        public static Shader ColorShader;
        public static Shader FullscreenShader;
        public static Shader SkyboxShader;
        public static Shader CubemapShader;

        public static List<Shader> shaders = new List<Shader>();

        public static void LoadShaders()
        {
            //DefaultShader = new Shader("Resources/Shaders/shader.vert", "Resources/Shaders/shader.frag");
            PbrShader = new Shader("Resources/Shaders/pbr.vert", "Resources/Shaders/pbr.frag");
            ColorShader = new Shader("Resources/Shaders/shader.vert", "Resources/Shaders/color.frag");
            //EquirectAngularToCubemapShader = new Shader("Resources/Shaders/cubemap.vert", "Resources/Shaders/equirectangular.frag");
            //IrradianceShader = new Shader("Resources/Shaders/cubemap.vert", "Resources/Shaders/irradiance.frag");
            //BackgroundShader = new Shader("Resources/Shaders/background.vert", "Resources/Shaders/background.frag");
            //PrefilterShader = new Shader("Resources/Shaders/cubemap.vert", "Resources/Shaders/prefilter.frag");
            
            TextShader = new Shader("Resources/Shaders/text.vert", "Resources/Shaders/text.frag");
            FullscreenShader = new Shader("Resources/Shaders/fullscreen.vert", "Resources/Shaders/fullscreen.frag");
            SkyboxShader = new Shader("Resources/Shaders/skybox.vert", "Resources/Shaders/skybox.frag");
            CubemapShader = new Shader("Resources/Shaders/cubemap.vert", "Resources/Shaders/cubemap.frag");
        }

        public static void StartShaders()
        {
            //BackgroundShader.Use();
            //BackgroundShader.SetInt("environmentMap", 0);
            
            //DefaultShader.SetMatrix4("projection", CameraManager.activeCamera.GetProjectionMatrix());
            //DefaultShader.SetInt("texture0", 0);
            
            
            
            PbrShader.SetInt("material.diffuse", 0);
            PbrShader.SetInt("material.specular", 3);
            
            //PbrShader.SetVector3("material.specular", new Vector3(1f, 0f, 0f));
            PbrShader.SetFloat("material.shininess", 24.0f);

            // Directional light needs a direction, in this example we just use (-0.2, -1.0, -0.3f) as the lights direction
            PbrShader.SetVector3("light.direction", new Vector3(-0.2f, -1.0f, -0.3f));
            PbrShader.SetVector3("light.ambient", new Vector3(0.6f));
            PbrShader.SetVector3("light.diffuse", new Vector3(1f));
            PbrShader.SetVector3("light.specular", new Vector3(0.6f));
            
            
            
            //Matrix4 ortho = Matrix4.CreateOrthographic(800, 600, 0, 100);
            //TextShader.SetMatrix4("projection", CameraManager.EditorCamera.GetProjectionMatrix());
            
            
        }

        public static void UpdateShaders(int witdh, int height)
        {
            var camera = CameraManager.EditorCamera;
            Matrix4 ortho = Matrix4.CreateOrthographic(witdh, height, 0, 100);
            TextShader.SetMatrix4("projection", ortho);
            //DefaultShader.SetMatrix4("view", camera.GetViewMatrix());
            //DefaultShader.SetMatrix4("projection", camera.GetProjectionMatrix());
            //_shader.SetVector3("camPos", _camera.Position);
            
           // BackgroundShader.SetMatrix4("view", camera.GetViewMatrix());
            
            PbrShader.SetMatrix4("view", camera.GetViewMatrix());
            PbrShader.SetMatrix4("projection", camera.GetProjectionMatrix());
            PbrShader.SetVector3("camPos", camera.Position);
            
            ColorShader.SetMatrix4("view", camera.GetViewMatrix());
            ColorShader.SetMatrix4("projection", camera.GetProjectionMatrix());

            Matrix3 mat3 = new Matrix3(camera.GetViewMatrix());
            
            SkyboxShader.SetMatrix4("view", new Matrix4(mat3));
            SkyboxShader.SetMatrix4("projection", camera.GetProjectionMatrix());
            
            
            //PbrShader.SetVector3("lightPositions[0]", new Vector3(1.2f ,1.2f,1.2f));
            //PbrShader.SetVector3("lightColors[0]", new Vector3(10,10,10));

            //PbrShader.SetInt("lightCount", 1);
            //BackgroundShader.Use();
            //BackgroundShader.SetMatrix4("view", camera.GetViewMatrix());
        }
    }
}