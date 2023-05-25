using System;
using System.Collections.Generic;
using ArchEngine.Core.Utils;
using Assimp;
using BulletSharp;
using BulletSharp.Math;
using Newtonsoft.Json;

namespace ArchEngine.Core.Physics
{
    public class PhysicsCore 
    {
        //public static List<AABB> AABBs = new List<AABB>();
        [JsonIgnore] public static DiscreteDynamicsWorld world;
        public PhysicsCore()
        {
            // Create a collision configuration
            var collisionConf = new DefaultCollisionConfiguration();

            // Create a collision dispatcher
            var dispatcher = new CollisionDispatcher(collisionConf);

            // Create a broadphase interface
            var broadphase = new DbvtBroadphase();

            // Create a constraint solver
            var solver = new SequentialImpulseConstraintSolver();

            // Create a dynamics world
            world = new DiscreteDynamicsWorld(dispatcher, broadphase, solver, collisionConf);
            
            // Set gravity
            world.Gravity = new Vector3(0, -9.81f, 0);   
        }

        public static ConvexHullShape SetupFromMesh(Mesh mesh, Vector3D scale)
        {
            var triMesh = new TriangleMesh();

            int i = 0;

            var vertices = mesh.Vertices;

            while (i < vertices.Count)
            {
                var vertex0 = FromVector3D(vertices[i] * 1);
                var vertex1 = FromVector3D(vertices[i + 1] * 1);
                var vertex2 = FromVector3D(vertices[i + 2] * 1);

                triMesh.AddTriangle(vertex0, vertex1, vertex2);

                i += 3;
            }

            var tempShape = new ConvexTriangleMeshShape(triMesh);
            tempShape.LocalScaling = new Vector3(scale.X,scale.Y,scale.Z);
            using var tempHull = new ShapeHull(tempShape);

            tempHull.BuildHull(tempShape.Margin);

            return new ConvexHullShape(tempHull.Vertices);
        }
        
        public static RigidBody AddMesh(Scene assimpScene, bool isStatic, Matrix mat)
        {
            
            // Create a mesh collider for the second object
            var triangleMesh = new TriangleMesh();
            var mesh = assimpScene.Meshes[0];

            // Create the collision shape
            // var convexHullShape = new ConvexHullShape();
            // foreach (var vertex in mesh.Vertices)
            // {
            //     var position = new Vector3(vertex.X, vertex.Y, vertex.Z);
            //     convexHullShape.AddPoint(position);
            // }
            var convexHullShape = SetupFromMesh(mesh, new Vector3D(mat.ScaleVector.X,mat.ScaleVector.Y,mat.ScaleVector.Z));
            
            //Console.WriteLine("scale:" + mat.ScaleVector);
            convexHullShape.LocalScaling = mat.ScaleVector; // Set local scaling
            convexHullShape.RecalcLocalAabb();


            var obj2MotionState = new DefaultMotionState(mat);
            var obj2RigidBodyCi = new RigidBodyConstructionInfo(1, obj2MotionState, convexHullShape);
            
            obj2RigidBodyCi.Friction = 0.5f;

            var obj2RigidBody = new RigidBody(obj2RigidBodyCi);
            obj2RigidBody.MotionState = obj2MotionState;
            world.AddRigidBody(obj2RigidBody);
            return obj2RigidBody;
        }
        private static Vector3 FromVector3D(Assimp.Vector3D vector)
        {
            return new Vector3(vector.X, vector.Z, vector.Y);
        }
      
        public static void Step()
        {
            world.StepSimulation(1 / 60f);
        }

        public static void Dispose()
        {
            world.Dispose();
        }
    }
    
}