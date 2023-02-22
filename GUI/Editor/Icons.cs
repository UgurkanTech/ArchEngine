using System;
using System.Drawing;
using ArchEngine.Core.Rendering.Textures;
using ImGuiNET;
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
            _texture = TextureManager.LoadFromFile("Resources/Textures/Editor/editoricons.png");
        }

        public static Vector2 GetUV1FromID(int id)
        {
             
            return new Vector2(id % (int)tileSize.X, id / (int)tileSize.Y) / tileSize + new Vector2(0.052f,0) + new Vector2(-0.007f, 0.007f);
        }
        public static Vector2 GetUV0FromID(int id)
        {

            return new Vector2(id % (int) tileSize.X, id / (int) tileSize.Y) / tileSize + new Vector2(0, 0.052f) + new Vector2(0.007f, -0.007f);
        }

        public static void DrawIcon(int id, int size)
        {
            ImGui.Image(Icons.Texture, new Vector2(size, size), Icons.GetUV0FromID(id), Icons.GetUV1FromID(id));
        }

        public static void ImguiBeginIcon(string name, int id)
        {
            ImGui.Begin("     " + name);
            
            ImGuiWindowPtr window = ImGui.GetCurrentWindow();
            ImRect titleBarRect = window.TitleBarRect();
            var old = ImGui.GetCursorPos();
            ImGui.PushClipRect( titleBarRect.Min , titleBarRect.Max, false );
            ImGui.SetCursorPos(new Vector2(6.0f, 5.0f));
            Icons.DrawIcon(id, 10);
            ImGui.SetCursorPos(old);
            ImGui.PopClipRect();
        }
    }
}