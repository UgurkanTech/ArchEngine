using System;
using System.Collections.Generic;
using ArchEngine.Core.ECS;
using ArchEngine.Scenes.Voxel;
using OpenTK.Mathematics;

namespace ArchEngine.Core.Physics
{
    public class AABB
    {
        public Vector3 Min;
        public Vector3 Max;
        public float[] verts;
        public GameObject gameobject;

        public AABB()
        {
            Min = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Max = new Vector3(float.MinValue, float.MinValue, float.MinValue);
        }

        public void CalculateFromPbrVertices(float[] vertices)
        {
            for (int i = 0; i < vertices.Length; i += 8)
            {
                Vector3 vertex = new Vector3(vertices[i], vertices[i + 1], vertices[i + 2]);
                
                vertex += gameobject.Transform.ExtractTranslation();
                
                Min.X = Math.Min(Min.X, vertex.X);
                Min.Y = Math.Min(Min.Y, vertex.Y);
                Min.Z = Math.Min(Min.Z, vertex.Z);

                Max.X = Math.Max(Max.X, vertex.X);
                Max.Y = Math.Max(Max.Y, vertex.Y);
                Max.Z = Math.Max(Max.Z, vertex.Z);
            }

            this.verts = vertices;
        }

        //brute-force approach to broad-phase collision detection with AABBs
        public static bool IsCollidingBounds(AABB aabb, List<AABB> aabbList)
        {
            foreach (AABB aabb2 in aabbList)
            {
                if (aabb.gameobject.name == aabb2.gameobject.name) continue; //self-collision

                if (aabb.Min.X > aabb2.Max.X || aabb.Max.X < aabb2.Min.X) continue;
                if (aabb.Min.Y > aabb2.Max.Y || aabb.Max.Y < aabb2.Min.Y) continue;
                if (aabb.Min.Z > aabb2.Max.Z || aabb.Max.Z < aabb2.Min.Z) continue;

                
                Window._log.Warn("colliding!");
                return isCollidingSAT(aabb, aabb2);
            }
            Window._log.Warn("not colliding!");
            return false;
        }

        //The Separating Axis Theorem (SAT) 
        private static bool isCollidingSAT(AABB aabb1, AABB aabb2)
        {
            float[] vertices1 = aabb1.verts;
            float[] vertices2 = aabb2.verts;
            
            foreach (Vector3 axis in axes)
            {
                // Calculate the projection of each AABB onto the axis
                float min1 = float.MaxValue;
                float max1 = float.MinValue;
                float min2 = float.MaxValue;
                float max2 = float.MinValue;
                for (int i = 0; i < vertices1.Length; i += 8)
                {
                    Vector3 vertex = new Vector3(vertices1[i], vertices1[i + 1], vertices1[i + 2]);
                    vertex += aabb1.gameobject.Transform.ExtractTranslation();
                    
                    float projection = Vector3.Dot(vertex, axis);
                    min1 = Math.Min(min1, projection);
                    max1 = Math.Max(max1, projection);
                }
                for (int i = 0; i < vertices2.Length; i += 8)
                {
                    Vector3 vertex = new Vector3(vertices2[i], vertices2[i + 1], vertices2[i + 2]);
                    vertex += aabb2.gameobject.Transform.ExtractTranslation();
                    
                    float projection = Vector3.Dot(vertex, axis);
                    min2 = Math.Min(min2, projection);
                    max2 = Math.Max(max2, projection);
                }
                // Check if the projections are overlapping
                if (max1 < min2 || max2 < min1)
                {
                    // The projections are not overlapping
                    return false;
                }
            }
            //AABBs are colliding
            return true;
        }
        
        private static Vector3[] axes = new Vector3[3]
        {
            new Vector3(1, 0, 0), // x-axis
            new Vector3(0, 1, 0), // y-axis
            new Vector3(0, 0, 1)  // z-axis
        };
        
        
        Vector3[] TransformVertices(Vector3[] vertices, Matrix4 transform)
        {
            // Transform the vertices using the transformation matrix
            Vector3[] transformedVertices = new Vector3[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector3 transformedVertex;
                Vector3 vertex = vertices[i];
                transformedVertex.X = vertex.X * transform.M11 + vertex.Y * transform.M12 + vertex.Z * transform.M13 + transform.M14;
                transformedVertex.Y = vertex.X * transform.M21 + vertex.Y * transform.M22 + vertex.Z * transform.M23 + transform.M24;
                transformedVertex.Z = vertex.X * transform.M31 + vertex.Y * transform.M32 + vertex.Z * transform.M33 + transform.M34;
                transformedVertices[i] = transformedVertex;
                
            }
            return transformedVertices;
        }
        Vector3 TransformVertex(Vector3 vertex, Matrix4 transform)
        {
            // Transform the vertex using the transformation matrix
            Vector3 transformedVertex;
            transformedVertex.X = vertex.X * transform.M11 + vertex.Y * transform.M12 + vertex.Z * transform.M13 + transform.M14;
            transformedVertex.Y = vertex.X * transform.M21 + vertex.Y * transform.M22 + vertex.Z * transform.M23 + transform.M24;
            transformedVertex.Z = vertex.X * transform.M31 + vertex.Y * transform.M32 + vertex.Z * transform.M33 + transform.M34;
            return transformedVertex;
        }
        
        Vector3[] ScaleVertices(Vector3[] vertices, float scale)
        {
            // Create a transformation matrix for scaling
            Matrix4 transform = Matrix4.CreateScale(scale);

            // Transform the vertices using the scaling matrix
            Vector3[] scaledVertices = TransformVertices(vertices, transform);
            return scaledVertices;
        }

        Vector3[] RotateVerticesX(Vector3[] vertices, float angle)
        {
            // Create a transformation matrix for rotation around the x-axis
            Matrix4 transform = Matrix4.CreateRotationX(angle);

            // Transform the vertices using the rotation matrix
            Vector3[] rotatedVertices = TransformVertices(vertices, transform);
            return rotatedVertices;
        }

        Vector3[] RotateVerticesY(Vector3[] vertices, float angle)
        {
            // Create a transformation matrix for rotation around the y-axis
            Matrix4 transform = Matrix4.CreateRotationY(angle);

            // Transform the vertices using the rotation matrix
            Vector3[] rotatedVertices = TransformVertices(vertices, transform);
            return rotatedVertices;
        }

        Vector3[] RotateVerticesZ(Vector3[] vertices, float angle)
        {
            // Create a transformation matrix for rotation around the z-axis
            Matrix4 transform = Matrix4.CreateRotationZ(angle);

            // Transform the vertices using the rotation matrix
            Vector3[] rotatedVertices = TransformVertices(vertices, transform);
            return rotatedVertices;
        }
    }
}