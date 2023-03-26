using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ArchEngine.Core.ECS;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.GUI.Editor;
using Newtonsoft.Json;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Lighting
{
    public class PointLight :  Component 
    {
        [JsonIgnore] public static List<PointLight> PointLights = new List<PointLight>();

        public Vector3 color; //For rendering with range 0-255

         public float intensity = 0.1f;

         [JsonIgnore]private Color4 colorTemp;

         public static Mesh lightMesh;
         public static Material mat;
         
         
         [JsonIgnore][Inspector] [Range(0, 1)] public float Intensity
        {
            get
            {
                return intensity;
            }
            set
            {
                intensity = value;
                color = new Vector3(colorTemp.R * intensity * 255, colorTemp.G * intensity * 255, colorTemp.B * intensity * 255);
                
            }
        }
        [Inspector] public Color4 Color
        {
            get
            {
                return new Color4(colorTemp.R, colorTemp.G, colorTemp.B, 1);
            }
            set
            {
                colorTemp = value;
            }
        }
        
        public PointLight()
        {
            colorTemp = Color4.Green;

        }
        
        public void Dispose()
        {
            initialized = false;
            PointLights.Remove(this);
        }

        public GameObject gameObject { get; set; }
        
        [JsonIgnore]public bool initialized { get; set; }
        [JsonIgnore]
        public int StencilID  { get; set; }
        public void Init()
        {
            PointLights.Add(this);
            if (!initialized)
            {
                if (lightMesh == null)
                {
                    lightMesh = AssetManager.GetMeshByFilePath("Resources/Models/light.fbx");
                    mat = new Material();
                
                    mat.Shader = ShaderManager.PbrShader;
                    
                
                    lightMesh.InitBuffers(mat);
                }
                
                initialized = true;
            }
        }

        public void Start()
        {
            
        }

        public void Update()
        {

        }
    }
}