using System;
using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Utils;
using ArchEngine.GUI.Editor;
using BulletSharp;
using BulletSharp.Math;
using Newtonsoft.Json;
using OpenTK.Mathematics;
using Vector3 = OpenTK.Mathematics.Vector3;

namespace ArchEngine.Core.Physics
{
    public class RigidObject : Component
    {
        public void Dispose()
        {
            initialized = false;
            PhysicsCore.world.RemoveRigidBody(_rb);
        }

        public GameObject gameObject { get; set; }
        public bool initialized { get; set; }


        [JsonIgnore] public RigidBody _rb;

        public RigidObject()
        {
            
        }
        public RigidObject(bool isKinematic, bool useGravity, float mass, bool isStatic)
        {
            initMass = mass;
            initIsKinematic = isKinematic;
            initUseGravity = useGravity;
            initStatic = isStatic;
        }


        public float initMass = 1f;
        public bool initIsKinematic = false;
        public bool initUseGravity = true;
        private bool initStatic = false;
        
        
        public void Init()
        {
            MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
            if (renderer == null)
            {
                Console.WriteLine("No mesh renderer found on object: " + gameObject.name + " creating new one.");
                renderer = new MeshRenderer();
                renderer.gameObject = gameObject;
                renderer.Init();
                gameObject.AddComponent(renderer);
            }

            if (renderer.mesh == null)
            {
                Console.WriteLine("No Mesh found in mesh renderer on object: " + gameObject.name);
                return;
            }

            if (renderer.mesh.AssimpScene == null )
            {
                Console.WriteLine("No assimpscene found on mesh on object: " + gameObject.name);
                return;
            }
            Matrix mat = Matrix4ToMatrix(gameObject.Transform);
            _rb = PhysicsCore.AddMesh(renderer.mesh.AssimpScene, initIsKinematic, mat);
            Matrix currentTransform = _rb.WorldTransform;

            // Modify the transform's position to the random position
            currentTransform.Origin = mat.Origin;

            // Set the RigidBody's new transform
            _rb.WorldTransform = currentTransform;
            
            IsKinematic = initIsKinematic;
            UseGravity = initUseGravity;
            Mass = initMass;
        }

        public void Start()
        {
            _rb.LinearVelocity = BulletSharp.Math.Vector3.Zero;
            _rb.AngularVelocity = BulletSharp.Math.Vector3.Zero;
            
            
            var old = Matrix4.CreateScale(1);
            old *= Matrix4.CreateFromQuaternion(gameObject.Transform.ExtractRotation());
            old *= Matrix4.CreateTranslation(gameObject.Transform.ExtractTranslation());
            
            Matrix mat = Matrix4ToMatrix(old);
            //mat.ScaleVector = BulletSharp.Math.Vector3.One;
            
            _rb.MotionState = new DefaultMotionState(mat);
            _rb.WorldTransform = mat;
            _rb.MotionState.WorldTransform = mat;
            
            
            _rb.CollisionShape.LocalScaling = gameObject.Transform.ExtractScale().Vector3ToBullet3();

            PhysicsCore.world.RemoveRigidBody(_rb);
            PhysicsCore.world.AddRigidBody(_rb);
            _rb.ForceActivationState(ActivationState.DisableDeactivation);
            _rb.Activate(true);

            UseGravity = _UseGravity;
            IsKinematic = _isKinematic;
            Mass = _mass;

        }

        private Matrix Matrix4ToMatrix(Matrix4 sourceMatrix)
        {
            Matrix newMatrix = new Matrix(
                sourceMatrix.M11, sourceMatrix.M12, sourceMatrix.M13, sourceMatrix.M14,
                sourceMatrix.M21, sourceMatrix.M22, sourceMatrix.M23, sourceMatrix.M24,
                sourceMatrix.M31, sourceMatrix.M32, sourceMatrix.M33, sourceMatrix.M34,
                sourceMatrix.M41, sourceMatrix.M42, sourceMatrix.M43, sourceMatrix.M44
            );
            return newMatrix;
        }
        
        private Matrix4 MatrixToMatrix4(Matrix sourceMatrix)
        {
            Matrix4 newMatrix = new Matrix4(
                sourceMatrix.M11, sourceMatrix.M12, sourceMatrix.M13, sourceMatrix.M14,
                sourceMatrix.M21, sourceMatrix.M22, sourceMatrix.M23, sourceMatrix.M24,
                sourceMatrix.M31, sourceMatrix.M32, sourceMatrix.M33, sourceMatrix.M34,
                sourceMatrix.M41, sourceMatrix.M42, sourceMatrix.M43, sourceMatrix.M44
            );
            return newMatrix;
        }

        public void Update()
        {
            Velocity = new Vector3(_rb.LinearVelocity.X, _rb.LinearVelocity.Y, _rb.LinearVelocity.Z);
            
        }

        public void FixedUpdate()
        {
            Move();
        }


        private float _mass;
        [JsonIgnore][Inspector] public float Mass
        {
            get
            {
                return _mass;
            }
            set
            {
                if (value <= 0)
                {
                    value = 0.0001f;
                }
                _mass = value;
                _rb.SetMassProps(value, BulletSharp.Math.Vector3.One);
                initMass = value;
                IsKinematic = _isKinematic;
            }
        }
        
        
        
        private bool _UseGravity;
        [JsonIgnore][Inspector] public bool UseGravity
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
                    _rb.Gravity = new BulletSharp.Math.Vector3(0, -9.81f, 0);
                }
                else
                {
                    _rb.Gravity = BulletSharp.Math.Vector3.Zero;
                    _rb.LinearVelocity = BulletSharp.Math.Vector3.Zero;
                    _rb.AngularVelocity = BulletSharp.Math.Vector3.Zero;
                }
                initUseGravity = value;
            }
        }
        private bool _isKinematic;
        [JsonIgnore][Inspector] public bool IsKinematic
        {
            get
            {
                return _isKinematic;
            }
            set
            {
                _isKinematic = value;
                if (value)
                {
                    _rb.SetMassProps(0, BulletSharp.Math.Vector3.Zero);
                }
                else
                {
                    _rb.SetMassProps(1, BulletSharp.Math.Vector3.One);

                }
                initIsKinematic = value;
            }
        }
        
        
        [JsonIgnore][Inspector] public Vector3 Velocity = Vector3.Zero;
        
        private void Move()
        {
            Vector3 scale = gameObject.Transform.ExtractScale();
            var temp = MatrixToMatrix4(_rb.MotionState.WorldTransform);
            gameObject.Transform = Matrix4.CreateScale(scale);
            gameObject.Transform *= Matrix4.CreateFromQuaternion(temp.ExtractRotation());
            gameObject.Transform *= Matrix4.CreateTranslation(temp.ExtractTranslation());
        }
    }
}