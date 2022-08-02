using System;
using System.Configuration;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Textures;

namespace ArchEngine.Core.ECS.Components
{
    public class MeshRenderer : Component
    {
        public GameObject gameObject { get; set; }

        public IRenderable mesh;
        
        private bool initialized = false;

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
            Console.WriteLine("started mesh renderer");
        }

        public void Update()
        {
            Console.WriteLine("update mesh renderer");
        }
    }
}