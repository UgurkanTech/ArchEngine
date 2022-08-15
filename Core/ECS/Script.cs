namespace ArchEngine.Core.ECS
{
    public abstract class Script : Component
    {
        public GameObject gameObject { get; set; }
        public bool initialized { get; set; }
        public virtual void Init() {}

        public virtual void Start() {}

        public virtual void Update(){}
        public virtual void FixedUpdate(){}
        


        public void Dispose()
        {
            
        }
    }
}