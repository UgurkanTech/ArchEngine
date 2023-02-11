using OpenTK.Mathematics;

namespace ArchEngine.Core.Utils
{
    public class MathUtils
    {
        public static Vector3 UnProject(Vector3 screen, Matrix4 modelView, Matrix4 projection, Vector4 view)
        {

            Vector4 pos = new();

            pos.X = (screen.X - view.X) / view.Z * 2.0f - 1.0f;
            pos.Y = 1 - (screen.Y - view.Y) / view.W * 2.0f;
            pos.Z = screen.Z * 2.0f - 1.0f;
            pos.W = 1.0f;
            Matrix4 proj = Matrix4.Invert(projection) * Matrix4.Invert(modelView);
            Vector4 pos2 = Vector4.TransformRow(new Vector4(pos.X, pos.Y, pos.Z, pos.W), proj);
            Vector3 pos_out = new(pos2.X, pos2.Y, pos2.Z);

            return pos_out / pos2.W;
        }
    }
}