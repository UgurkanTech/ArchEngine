using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImGuiNET;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ArchEngine.GUI.Editor.Windows
{
    public class ConsoleDecorator : TextWriter
    {
        private TextWriter consoleOriginal;

        public ConsoleDecorator(TextWriter consoleTextWriter)
        {
            consoleOriginal = consoleTextWriter;
        }
        public override void WriteLine(string value)
        {
            consoleOriginal.WriteLine(value);
            ConsoleWindow._stringWriter.WriteLine(value);
            // Fire event here with value
        }
        
        public override void WriteLine(int value)
        {
            consoleOriginal.WriteLine(value);
            ConsoleWindow._stringWriter.WriteLine(value);
            // Fire event here with value
        }
        public override void WriteLine(object value)
        {
            consoleOriginal.WriteLine(value);
            ConsoleWindow._stringWriter.WriteLine(value);
            // Fire event here with value
        }
        public override void WriteLine(long value)
        {
            consoleOriginal.WriteLine(value);
            ConsoleWindow._stringWriter.WriteLine(value);
            // Fire event here with value
        }
        public override void Write(string value)
        {
            consoleOriginal.Write(value);
            ConsoleWindow._stringWriter.Write(value);
            // Fire event here with value
        }
        public override void Write(object value)
        {
            consoleOriginal.Write(value);
            ConsoleWindow._stringWriter.Write(value);
            // Fire event here with value
        }
        public override Encoding Encoding { get; }
        
    }
    
    public class ConsoleWindow
    {
        private static List<string> _lines = new List<string>();
        public static StringWriter _stringWriter = new StringWriter();
        
        private static bool autoscroll = true;
        public ConsoleWindow()
        {
            Clear();
            Console.SetOut(new ConsoleDecorator(Console.Out));
        }
        
        public static void Clear()
        {


            _stringWriter.GetStringBuilder().Clear();
           

        }

        private static int oldLength = 0;

        public static void Draw()
        {
            
            
            ImGui.SetNextWindowPos(new Vector2(55,100), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new Vector2(350, 100), ImGuiCond.FirstUseEver);
            //ImGui.Begin("Arch Console");
            Icons.ImguiBeginIcon("Arch Console", 46);

            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.2f, 0,0,1));
            
                
            if (ImGui.ImageButtonEx(ImGui.GetID("ClearConsole"), Icons.Texture, new Vector2(20, 20), Icons.GetUV0FromID(21),
                    Icons.GetUV1FromID(21),
                    Vector2.Zero, Vector4.Zero, Vector4.One))
            {
                Clear();
            }
            ImGui.PopStyleColor();
            
 
            ImGui.SameLine();

            if (ImGui.ImageButtonEx(ImGui.GetID("SettingsConsole"), Icons.Texture, new Vector2(20, 20),
                    Icons.GetUV0FromID(302),
                    Icons.GetUV1FromID(302),
                    Vector2.Zero, Vector4.Zero, Vector4.One))
            {
                ImGui.OpenPopup("Options");
            }


            
            ImGui.SameLine();

            ImGui.BeginChild("ErrorChild", new Vector2(0, 20));
            ImGui.SetCursorPosY(3);
            
            
            ImGui.Text("Status: " + Editor.state);
            
            ImGui.SameLine();
            int errors = Editor.compiler.errorsCount;
            ImGui.TextColored(errors == 0 ? new Vector4(0,1,0,1) : new Vector4(1,0,0,1), "Compiler Errors: " + errors);

            ImGui.EndChild();
            
            //ImGui.TextWrapped("Console window");
            
            ImGui.Separator();
            
            float footerHeightToReserve = ImGui.GetStyle().ItemSpacing.Y + ImGui.GetFrameHeightWithSpacing();
            if (ImGui.BeginChild("ScrollingRegion", new Vector2(0, -footerHeightToReserve), false,
                    ImGuiWindowFlags.HorizontalScrollbar |ImGuiWindowFlags.AlwaysVerticalScrollbar))
            {
                for (int i = 0; i < _lines.Count; i++)
                {
                    //ImGui.TextWrapped(_lines[i]);
                }
                if (_stringWriter.GetStringBuilder().Length > 3000)
                {
                    string s = _stringWriter.GetStringBuilder().ToString().Substring(1000, _stringWriter.GetStringBuilder().Length - 1001);
                    _stringWriter.GetStringBuilder().Clear();
                    _stringWriter.WriteLine(s);
                }
                ImGui.TextWrapped(_stringWriter.ToString());
                ImGui.TextColored(Vector4.UnitX, "Broken native wrapping");
                

                if (autoscroll && oldLength != _stringWriter.GetStringBuilder().Length)
                {
                    oldLength = _stringWriter.GetStringBuilder().Length;
                    ImGui.SetScrollHereY(1.0f);
                }

                
                ImGui.EndChild();
            }

            
            if (ImGui.BeginPopup("Options"))
            {
                ImGui.Checkbox("Auto-scroll", ref autoscroll);

                ImGui.EndPopup();
            }
            
            if (ImGui.BeginPopupContextWindow())
            {
                if (ImGui.Selectable("Clear")) Clear();
                ImGui.EndPopup();
            }

            byte[] buff = new byte[128];
            
            ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
            bool reclaim_focus = false;
            unsafe
            {
                
                if (ImGui.InputText("", buff, 128,
                        ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.CallbackAlways, Callback))
                {

                    reclaim_focus = true;
                }
            }
            if (reclaim_focus)
            {
                ImGui.SetKeyboardFocusHere(-1);
            }
            
            ImGui.End();


        }

        private static unsafe int Callback(ImGuiInputTextCallbackData* data)
        {
            
            if (ImGui.IsKeyPressed((int)Keys.Enter))
            {
                //_lines.Add(Encoding.ASCII.GetString(data->Buf, data->BufTextLen));
                Console.WriteLine("Invalid command: " + Encoding.ASCII.GetString(data->Buf, data->BufTextLen));
            }
            return -1;
        }
    }
}