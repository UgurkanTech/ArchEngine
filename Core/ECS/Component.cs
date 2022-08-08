namespace ArchEngine.Core.ECS
{
    public interface Component
    {
        public GameObject gameObject  { get; set; }
        
        public void Init();
        public void Start();
        public void Update();
    }
    
    
}