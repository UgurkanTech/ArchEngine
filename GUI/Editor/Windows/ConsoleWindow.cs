using System.Collections.Generic;
using ImGuiNET;

namespace ArchEngine.GUI.Editor.Windows
{
    public class ConsoleWindow
    {

        static private ImGuiTextBuffer Buf;
        static private ImGuiTextFilter Filter;
        static private List<int> LineOffsets;
        static private bool AutoScroll;

        public ConsoleWindow()
        {
            AutoScroll = true;
            Clear();
        }
        
        public static void Clear()
        {
            
        }

        public static void AddLog()
        {
            int oldSize = Buf.Buf.Size;
            

        }

        public static void Draw()
        {
            
            
            
        }


    }
}