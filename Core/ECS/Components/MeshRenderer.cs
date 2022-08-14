using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Numerics;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor;
using Newtonsoft.Json;

namespace ArchEngine.Core.ECS.Components
{
    public class MeshRenderer : Component
    {
        public GameObject gameObject { get; set; }
        public bool initialized { get; set; }

        public IRenderable mesh;
        
        [JsonIgnore]
        public int StencilID  { get; set; }

        public MeshRenderer()
        {
            
        }

        public void Init()
        {
            
            if (!initialized && mesh != null)
            {
                mesh.InitBuffers();
                initialized = true;
            }
            //mesh.Material = new Material();
            
            
        }

        public void Start()
        {
            
        }

        public void Update()
        {
            
        }

        

        public void Dispose()
        {
            mesh?.Dispose();
            initialized = false;
        }
    }
}