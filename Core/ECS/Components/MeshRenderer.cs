using System;

namespace ArchEngine.Core.ECS.Components
{
    public class MeshRenderer : Component
    {
        public override void Init()
        {
            Console.WriteLine("Inited mesh renderer");
        }

        public override void Start()
        {
            Console.WriteLine("started mesh renderer");
        }

        public override void Update()
        {
            Console.WriteLine("update mesh renderer");
        }
    }
}