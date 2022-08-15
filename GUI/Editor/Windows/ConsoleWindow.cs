using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using ArchEngine.Core;
using ImGuiNET;

namespace ArchEngine.GUI.Editor.Windows
{
    public class ConsoleWindow
    {
        private static List<string> _lines = new List<string>();
        private static StringWriter _stringWriter = new StringWriter();
        public ConsoleWindow()
        {
            Clear();
            //Console.SetOut(_stringWriter);
        }
        
        public static void Clear()
        {
            for (int i = 0; i < 5; i++)
            {
                //_lines.Add("test sds messsssssssssssssage  sds ddd sdsds sdssd sdsds adsda");
            }

            _stringWriter.GetStringBuilder().Clear();
           

        }

        private static int oldLength = 0;

        public static void Draw()
        {
            
            
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(25,100), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(150, 300), ImGuiCond.FirstUseEver);
            ImGui.Begin("Arch Console");
            

            ImGui.PushStyleColor(ImGuiCol.Button, new Vector4(0.2f, 0,0,1));
            
            if (ImGui.SmallButton("Clear"))
                Clear();
            
            ImGui.PopStyleColor();
            ImGui.SameLine();
            if (ImGui.SmallButton("Options"))
                ImGui.OpenPopup("Options");

           
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
                ImGui.TextWrapped(_stringWriter.ToString());

                if (oldLength != _stringWriter.GetStringBuilder().Length)
                {
                    oldLength = _stringWriter.GetStringBuilder().Length;
                    ImGui.SetScrollHereY(1.0f);
                }
                
                ImGui.EndChild();
            }

            bool asd = false;
            if (ImGui.BeginPopup("Options"))
            {
                ImGui.Checkbox("Auto-scroll", ref asd);
                if (ImGui.Button("Save scene"))
                {
                    AssetManager.SaveScene();
                }
                if (ImGui.Button("Load scene"))
                {
                    AssetManager.LoadScene();
                }
                
                ImGui.EndPopup();
            }
            
            if (ImGui.BeginPopupContextWindow())
            {
                if (ImGui.Selectable("Clear")) Clear();
                ImGui.EndPopup();
            }

            byte[] buff = new byte[32];
            
            ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
            bool reclaim_focus = false;
            unsafe
            {
                
                if (ImGui.InputText("", buff, 32,
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
            if (ImGui.IsKeyPressed(ImGuiKey.Enter))
            {
                //_lines.Add(Encoding.ASCII.GetString(data->Buf, data->BufTextLen));
                Console.WriteLine("Invalid command: " + Encoding.ASCII.GetString(data->Buf, data->BufTextLen));
            }
            return -1;
        }
    }
}