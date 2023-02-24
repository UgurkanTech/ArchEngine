using OpenTK.Mathematics;

namespace ArchEngine.Core.Rendering.Geometry
{
    public class Quad : Mesh
    {
        public Quad()
        {
            // positions
        Vector3 pos1 = new Vector3(-1.0f,  1.0f, 0.0f);
        Vector3 pos2 = new Vector3(-1.0f, -1.0f, 0.0f);
        Vector3 pos3 = new Vector3( 1.0f, -1.0f, 0.0f);
        Vector3 pos4 = new Vector3( 1.0f,  1.0f, 0.0f);
        // texture coordinates
        Vector2 uv1 = new Vector2(0.0f, 1.0f);
        Vector2 uv2 = new Vector2(0.0f, 0.0f);
        Vector2 uv3 = new Vector2(1.0f, 0.0f);
        Vector2 uv4 = new Vector2(1.0f, 1.0f);
        // normal vector
        Vector3 nm = new Vector3(0.0f, 0.0f, 1.0f);

        // calculate tangent/bitangent vectors of both triangles
        Vector3 tangent1, bitangent1;
        Vector3 tangent2, bitangent2;
        // triangle 1
        // ----------
        Vector3 edge1 = pos2 - pos1;
        Vector3 edge2 = pos3 - pos1;
        Vector2 deltaUV1 = uv2 - uv1;
        Vector2 deltaUV2 = uv3 - uv1;

        float f = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV2.X * deltaUV1.Y);

        tangent1.X = f * (deltaUV2.Y * edge1.X - deltaUV1.Y * edge2.X);
        tangent1.Y = f * (deltaUV2.Y * edge1.Y - deltaUV1.Y * edge2.Y);
        tangent1.Z = f * (deltaUV2.Y * edge1.Z - deltaUV1.Y * edge2.Z);
        tangent1 = Vector3.Normalize(tangent1);

        bitangent1.X = f * (-deltaUV2.X * edge1.X + deltaUV1.X * edge2.X);
        bitangent1.Y = f * (-deltaUV2.X * edge1.Y + deltaUV1.X * edge2.Y);
        bitangent1.Z = f * (-deltaUV2.X * edge1.Z + deltaUV1.X * edge2.Z);
        bitangent1 = Vector3.Normalize(bitangent1);

        // triangle 2
        // ----------
        edge1 = pos3 - pos1;
        edge2 = pos4 - pos1;
        deltaUV1 = uv3 - uv1;
        deltaUV2 = uv4 - uv1;

        f = 1.0f / (deltaUV1.X * deltaUV2.Y - deltaUV2.X * deltaUV1.Y);

        tangent2.X = f * (deltaUV2.Y * edge1.X - deltaUV1.Y * edge2.X);
        tangent2.Y = f * (deltaUV2.Y * edge1.Y - deltaUV1.Y * edge2.Y);
        tangent2.Z = f * (deltaUV2.Y * edge1.Z - deltaUV1.Y * edge2.Z);
        tangent2 = Vector3.Normalize(tangent2);


        bitangent2.X = f * (-deltaUV2.X * edge1.X + deltaUV1.X * edge2.X);
        bitangent2.Y = f * (-deltaUV2.X * edge1.Y + deltaUV1.X * edge2.Y);
        bitangent2.Z = f * (-deltaUV2.X * edge1.Z + deltaUV1.X * edge2.Z);
        bitangent2 = Vector3.Normalize(bitangent2);


        float[] quadVertices = {
            // positions            // normal         // teXcoords  // tangent                          // bitangent
            pos1.X, pos1.Y, pos1.Z,uv1.X, uv1.Y, nm.X, nm.Y, nm.Z, tangent1.X, tangent1.Y, tangent1.Z, bitangent1.X, bitangent1.Y, bitangent1.Z,
            pos2.X, pos2.Y, pos2.Z,uv2.X, uv2.Y, nm.X, nm.Y, nm.Z, tangent1.X, tangent1.Y, tangent1.Z, bitangent1.X, bitangent1.Y, bitangent1.Z,
            pos3.X, pos3.Y, pos3.Z,uv3.X, uv3.Y, nm.X, nm.Y, nm.Z, tangent1.X, tangent1.Y, tangent1.Z, bitangent1.X, bitangent1.Y, bitangent1.Z,

            pos1.X, pos1.Y, pos1.Z,uv1.X, uv1.Y, nm.X, nm.Y, nm.Z, tangent2.X, tangent2.Y, tangent2.Z, bitangent2.X, bitangent2.Y, bitangent2.Z,
            pos3.X, pos3.Y, pos3.Z,uv3.X, uv3.Y, nm.X, nm.Y, nm.Z, tangent2.X, tangent2.Y, tangent2.Z, bitangent2.X, bitangent2.Y, bitangent2.Z,
            pos4.X, pos4.Y, pos4.Z,uv4.X, uv4.Y, nm.X, nm.Y, nm.Z, tangent2.X, tangent2.Y, tangent2.Z, bitangent2.X, bitangent2.Y, bitangent2.Z
        };
        Vertices = quadVertices;

        }
    }
}