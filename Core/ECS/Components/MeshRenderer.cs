using System;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Numerics;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.GUI.Editor;

namespace ArchEngine.Core.ECS.Components
{
    public class MeshRenderer : Component
    {
        public GameObject gameObject { get; set; }

        public IRenderable mesh;
        
        public bool initialized = false;

        [Inspector()] public int StencilID  { get; set; }

        public MeshRenderer()
        {
            
        }

        public void Init()
        {
            if (!initialized)
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
            //mesh?.Destroy();
        }
    }
}