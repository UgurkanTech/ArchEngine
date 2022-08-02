using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using ImGuiNET;

namespace ArchEngine.GUI.ImGUI
{
    public class Theme
    {
        public static void Use()
        {
            var io = ImGui.GetIO();
            
            //var font = ImGui.ImFontAtlasAddFontFromFileTTF(io.Fonts, "Assets/Fonts/Ruda-Bold.ttf", 15.0f, null, ref Unsafe.AsRef<ushort>((void*)0x0));
            //if (font != null) 
             //   io.FontDefault = font;

            var style = ImGui.GetStyle();
            style.FrameRounding = 4.0f;
            style.WindowBorderSize = 0.0f;
            style.PopupBorderSize = 0.0f;
            style.GrabRounding = 4.0f;
            var colors = style.Colors;
            colors[(int) ImGuiCol.Text] = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
            colors[(int) ImGuiCol.TextDisabled] = new Vector4(0.73f, 0.75f, 0.74f, 1.00f);
            colors[(int) ImGuiCol.WindowBg] = new Vector4(0.09f, 0.09f, 0.09f, 0.94f);
            colors[(int) ImGuiCol.ChildBg] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int) ImGuiCol.PopupBg] = new Vector4(0.08f, 0.08f, 0.08f, 0.94f);
            colors[(int) ImGuiCol.Border] = new Vector4(0.20f, 0.20f, 0.20f, 0.50f);
            colors[(int) ImGuiCol.BorderShadow] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
            colors[(int) ImGuiCol.FrameBg] = new Vector4(0.71f, 0.39f, 0.39f, 0.54f);
            colors[(int) ImGuiCol.FrameBgHovered] = new Vector4(0.84f, 0.66f, 0.66f, 0.40f);
            colors[(int) ImGuiCol.FrameBgActive] = new Vector4(0.84f, 0.66f, 0.66f, 0.67f);
            colors[(int) ImGuiCol.TitleBg] = new Vector4(0.47f, 0.22f, 0.22f, 0.67f);
            colors[(int) ImGuiCol.TitleBgActive] = new Vector4(0.47f, 0.22f, 0.22f, 1.00f);
            colors[(int) ImGuiCol.TitleBgCollapsed] = new Vector4(0.47f, 0.22f, 0.22f, 0.67f);
            colors[(int) ImGuiCol.MenuBarBg] = new Vector4(0.34f, 0.16f, 0.16f, 1.00f);
            colors[(int) ImGuiCol.ScrollbarBg] = new Vector4(0.02f, 0.02f, 0.02f, 0.53f);
            colors[(int) ImGuiCol.ScrollbarGrab] = new Vector4(0.31f, 0.31f, 0.31f, 1.00f);
            colors[(int) ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.41f, 0.41f, 0.41f, 1.00f);
            colors[(int) ImGuiCol.ScrollbarGrabActive] = new Vector4(0.51f, 0.51f, 0.51f, 1.00f);
            colors[(int) ImGuiCol.CheckMark] = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
            colors[(int) ImGuiCol.SliderGrab] = new Vector4(0.71f, 0.39f, 0.39f, 1.00f);
            colors[(int) ImGuiCol.SliderGrabActive] = new Vector4(0.84f, 0.66f, 0.66f, 1.00f);
            colors[(int) ImGuiCol.Button] = new Vector4(0.47f, 0.22f, 0.22f, 0.65f);
            colors[(int) ImGuiCol.ButtonHovered] = new Vector4(0.71f, 0.39f, 0.39f, 0.65f);
            colors[(int) ImGuiCol.ButtonActive] = new Vector4(0.20f, 0.20f, 0.20f, 0.50f);
            colors[(int) ImGuiCol.Header] = new Vector4(0.71f, 0.39f, 0.39f, 0.54f);
            colors[(int) ImGuiCol.HeaderHovered] = new Vector4(0.84f, 0.66f, 0.66f, 0.65f);
            colors[(int) ImGuiCol.HeaderActive] = new Vector4(0.84f, 0.66f, 0.66f, 0.00f);
            colors[(int) ImGuiCol.Separator] = new Vector4(0.43f, 0.43f, 0.50f, 0.50f);
            colors[(int) ImGuiCol.SeparatorHovered] = new Vector4(0.71f, 0.39f, 0.39f, 0.54f);
            colors[(int) ImGuiCol.SeparatorActive] = new Vector4(0.71f, 0.39f, 0.39f, 0.54f);
            colors[(int) ImGuiCol.ResizeGrip] = new Vector4(0.71f, 0.39f, 0.39f, 0.54f);
            colors[(int) ImGuiCol.ResizeGripHovered] = new Vector4(0.84f, 0.66f, 0.66f, 0.66f);
            colors[(int) ImGuiCol.ResizeGripActive] = new Vector4(0.84f, 0.66f, 0.66f, 0.66f);
            colors[(int) ImGuiCol.Tab] = new Vector4(0.71f, 0.39f, 0.39f, 0.54f);
            colors[(int) ImGuiCol.TabHovered] = new Vector4(0.84f, 0.66f, 0.66f, 0.66f);
            colors[(int) ImGuiCol.TabActive] = new Vector4(0.84f, 0.66f, 0.66f, 0.66f);
            colors[(int) ImGuiCol.TabUnfocused] = new Vector4(0.07f, 0.10f, 0.15f, 0.97f);
            colors[(int) ImGuiCol.TabUnfocusedActive] = new Vector4(0.14f, 0.26f, 0.42f, 1.00f);
            colors[(int) ImGuiCol.PlotLines] = new Vector4(0.61f, 0.61f, 0.61f, 1.00f);
            colors[(int) ImGuiCol.PlotLinesHovered] = new Vector4(1.00f, 0.43f, 0.35f, 1.00f);
            colors[(int) ImGuiCol.PlotHistogram] = new Vector4(0.90f, 0.70f, 0.00f, 1.00f);
            colors[(int) ImGuiCol.PlotHistogramHovered] = new Vector4(1.00f, 0.60f, 0.00f, 1.00f);
            colors[(int) ImGuiCol.TextSelectedBg] = new Vector4(0.26f, 0.59f, 0.98f, 0.35f);
            colors[(int) ImGuiCol.DragDropTarget] = new Vector4(1.00f, 1.00f, 0.00f, 0.90f);
            colors[(int) ImGuiCol.NavHighlight] = new Vector4(0.41f, 0.41f, 0.41f, 1.00f);
            colors[(int) ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
            colors[(int) ImGuiCol.NavWindowingDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);
            colors[(int) ImGuiCol.ModalWindowDimBg] = new Vector4(0.80f, 0.80f, 0.80f, 0.35f);
            
            
        }
    }
}