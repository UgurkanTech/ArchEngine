using System;
using ArchEngine.Core.Rendering.Textures;
using OpenTK.Mathematics;

namespace ArchEngine.GUI.Editor
{
    public class Icons
    {

        public static IntPtr Texture
        {
            get { return (IntPtr) _texture.handle;}
            private set { } }
        private static Texture _texture;
        private static Vector2 tileSize = new Vector2(19, 19);
        public static void LoadIcons()
        {
            _texture = TextureManager.LoadFromFile("Resources/Textures/editoricons.png");
        }

        public static Vector2 GetUV1FromID(int id)
        {
             
            return new Vector2(id % (int)tileSize.X, id / (int)tileSize.Y) / tileSize + new Vector2(0.052f,0) + new Vector2(-0.007f, 0.007f);
        }
        public static Vector2 GetUV0FromID(int id)
        {

            return new Vector2(id % (int) tileSize.X, id / (int) tileSize.Y) / tileSize + new Vector2(0, 0.052f) + new Vector2(0.007f, -0.007f);
        }
    }
}