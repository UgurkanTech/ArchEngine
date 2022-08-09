using System;

namespace ArchEngine.Core.ECS
{
    public interface Component : IDisposable
    {
        public GameObject gameObject  { get; set; }
        
        public void Init();
        public void Start();
        public void Update();
    }
    
    
}