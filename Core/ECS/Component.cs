namespace ArchEngine.Core.ECS
{
    public interface Component
    {
        public GameObject gameObject  { get; set; }
        public abstract void Init();
        public abstract void Start();
        public abstract void Update();
    }
}