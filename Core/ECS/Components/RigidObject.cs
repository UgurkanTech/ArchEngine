using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Physics
{
    public class RigidObject : Component
    {
        public void Dispose()
        {
            initialized = false;
        }

        public GameObject gameObject { get; set; }
        public bool initialized { get; set; }

        

        private MeshCollider _collider;
        
        public void Init()
        {
        }

        public void Start()
        {
            _collider = gameObject.GetComponent<MeshCollider>();
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
            Velocity += Acceleration.Divide(Window.FixedFps);
            Move();
        }


        private bool _UseGravity = false;
        [Inspector] public bool UseGravity
        {
            get
            {
                return _UseGravity;
            }
            set
            {
                _UseGravity = value;
                if (value)
                {
                    Acceleration = new Vector3(Acceleration.X, -9.81f, Acceleration.Z);
                }
                else
                {
                    Acceleration = new Vector3(Acceleration.X, 0, Acceleration.Z);
                    Velocity = Vector3.Zero;
                }
            }
        }
        
        [Inspector] public Vector3 Velocity = Vector3.Zero;
        [Inspector] public Vector3 Acceleration = Vector3.Zero;

        private void Move()
        {
            Matrix4 oldPos = gameObject.Transform;
            //move
            Matrix4 mat = Matrix4.Identity;
            mat = Matrix4.CreateScale( gameObject.Transform.ExtractScale());
            mat *= Matrix4.CreateRotationX(gameObject.Transform.ExtractRotation().X);
            mat *= Matrix4.CreateRotationY(gameObject.Transform.ExtractRotation().Y);
            mat *= Matrix4.CreateRotationZ(gameObject.Transform.ExtractRotation().Z);
            mat *= Matrix4.CreateTranslation(gameObject.Transform.ExtractTranslation() + Velocity.Divide(Window.FixedFps));
            gameObject.Transform = mat;
            //check coll if has
            bool collided = false;
            if (_collider != null)
                collided = _collider.CheckCollide();

            //cancel move if collided
            if (collided)
            {
                gameObject.Transform = oldPos;
                Velocity = Vector3.Zero;
            }
                
        }
    }
}