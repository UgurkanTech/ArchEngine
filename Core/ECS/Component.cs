namespace ArchEngine.Core.ECS
{
    public abstract class Component
    {
        public GameObject gameObject;
        public abstract void Init();
        public abstract void Start();
        public abstract void Update();
    }
}