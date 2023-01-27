using System.Collections;
using System.Collections.Generic;
using ArchEngine.Core.Physics;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.GUI.Editor;

namespace ArchEngine.Core.ECS.Components
{
    public class MeshCollider : Component
    {
        public void Dispose()
        {
            
        }

        public GameObject gameObject { get; set; }
        public bool initialized { get; set; }

        public AABB Aabb;

        [Inspector] public bool collided = false;
        
        public void Init()
        {


        }
        public void Start()
        {
            Aabb = new AABB();
            Aabb.gameobject = gameObject;
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                Window._log.Warn("No mesh renderer found for mesh collider!");
                return;
            }
            Mesh mesh = renderer.mesh;
            if (mesh == null)
            {
                Window._log.Warn("No mesh found in renderer for mesh collider!");
                return;
            }
            if (mesh.Vertices == null)
            {
                Window._log.Warn("No mesh vertices found in renderer for mesh collider!");
                return;
            }
            Aabb.CalculateFromPbrVertices(renderer.mesh.Vertices);
            PhysicsCore.AABBs.Add(Aabb);
        }

        public bool CheckCollide()
        {
            collided = AABB.IsCollidingBounds(Aabb, PhysicsCore.AABBs);
            return collided;
        }

        public void FixedUpdate()
        {

        }

        public void Update()
        {

        }
        
        
    }
}