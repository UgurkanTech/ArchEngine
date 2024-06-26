﻿using ImGuiNET;
using OpenTK.Mathematics;

namespace ArchEngine.GUI.ImGUI
{
    public class Theme
    {
        public static void Use()
        {
	        //Use2();
	        //return;
            
            var style = ImGui.GetStyle();

	        var colors = style.Colors;
	        
            style.WindowPadding = new Vector2(5, 15); //15
			style.WindowRounding = 5.0f;
			style.FramePadding = new Vector2(5, 5);
			style.FrameRounding = 4.0f;
			style.ItemSpacing = new Vector2(12, 8);
			style.ItemInnerSpacing = new Vector2(8, 6);
			style.IndentSpacing = 5.0f;
			style.ScrollbarSize = 15.0f;
			style.ScrollbarRounding = 9.0f;
			style.GrabMinSize = 5.0f;
			style.GrabRounding = 3.0f;
			style.ScaleAllSizes(0.6f);
		 
			colors[(int) ImGuiCol.Text]                  = new Vector4(1.00f, 1.00f, 1.00f, 1.00f);
			colors[(int) ImGuiCol.TextDisabled]          = new Vector4(0.50f, 0.50f, 0.50f, 1.00f);
			colors[(int) ImGuiCol.WindowBg]              = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);
			colors[(int) ImGuiCol.ChildBg]               = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);
			colors[(int) ImGuiCol.PopupBg]               = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);
			colors[(int) ImGuiCol.Border]                = new Vector4(0.43f, 0.43f, 0.50f, 0.50f);
			colors[(int) ImGuiCol.BorderShadow]          = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
			colors[(int) ImGuiCol.FrameBg]               = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
			colors[(int) ImGuiCol.FrameBgHovered]        = new Vector4(0.38f, 0.38f, 0.38f, 1.00f);
			colors[(int) ImGuiCol.FrameBgActive]         = new Vector4(0.67f, 0.67f, 0.67f, 0.39f);
			colors[(int) ImGuiCol.TitleBg]               = new Vector4(0.08f, 0.08f, 0.09f, 1.00f);
			colors[(int) ImGuiCol.TitleBgActive]         = new Vector4(0.08f, 0.08f, 0.09f, 1.00f);
			colors[(int) ImGuiCol.TitleBgCollapsed]      = new Vector4(0.00f, 0.00f, 0.00f, 0.51f);
			colors[(int) ImGuiCol.MenuBarBg]             = new Vector4(0.1f, 0.1f, 0.1f, 1.00f);
			colors[(int) ImGuiCol.ScrollbarBg]           = new Vector4(0.02f, 0.02f, 0.02f, 0.53f);
			colors[(int) ImGuiCol.ScrollbarGrab]         = new Vector4(0.31f, 0.31f, 0.31f, 1.00f);
			colors[(int) ImGuiCol.ScrollbarGrabHovered]  = new Vector4(0.41f, 0.41f, 0.41f, 1.00f);
			colors[(int) ImGuiCol.ScrollbarGrabActive]   = new Vector4(0.51f, 0.51f, 0.51f, 1.00f);
			colors[(int) ImGuiCol.CheckMark]             = new Vector4(0.11f, 0.64f, 0.92f, 1.00f);
			colors[(int) ImGuiCol.SliderGrab]            = new Vector4(0.11f, 0.64f, 0.92f, 1.00f);
			colors[(int) ImGuiCol.SliderGrabActive]      = new Vector4(0.08f, 0.50f, 0.72f, 1.00f);
			colors[(int) ImGuiCol.Button]                = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
			colors[(int) ImGuiCol.ButtonHovered]         = new Vector4(0.38f, 0.38f, 0.38f, 1.00f);
			colors[(int) ImGuiCol.ButtonActive]          = new Vector4(0.67f, 0.67f, 0.67f, 0.39f);
			colors[(int) ImGuiCol.Header]                = new Vector4(0.22f, 0.22f, 0.22f, 1.00f);
			colors[(int) ImGuiCol.HeaderHovered]         = new Vector4(0.25f, 0.25f, 0.25f, 1.00f);
			colors[(int) ImGuiCol.HeaderActive]          = new Vector4(0.67f, 0.67f, 0.67f, 0.39f);
			colors[(int) ImGuiCol.Separator]             = colors[(int) ImGuiCol.Border];
			colors[(int) ImGuiCol.SeparatorHovered]      = new Vector4(0.41f, 0.42f, 0.44f, 1.00f);
			colors[(int) ImGuiCol.SeparatorActive]       = new Vector4(0.26f, 0.59f, 0.98f, 0.95f);
			colors[(int) ImGuiCol.ResizeGrip]            = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
			colors[(int) ImGuiCol.ResizeGripHovered]     = new Vector4(0.29f, 0.30f, 0.31f, 0.67f);
			colors[(int) ImGuiCol.ResizeGripActive]      = new Vector4(0.26f, 0.59f, 0.98f, 0.95f);
			colors[(int) ImGuiCol.Tab]                   = new Vector4(0.08f, 0.08f, 0.09f, 0.83f);
			colors[(int) ImGuiCol.TabHovered]            = new Vector4(0.33f, 0.34f, 0.36f, 0.83f);
			colors[(int) ImGuiCol.TabActive]             = new Vector4(0.23f, 0.23f, 0.24f, 1.00f);
			colors[(int) ImGuiCol.TabUnfocused]          = new Vector4(0.08f, 0.08f, 0.09f, 1.00f);
			colors[(int) ImGuiCol.TabUnfocusedActive]    = new Vector4(0.13f, 0.14f, 0.15f, 1.00f);
			colors[(int) ImGuiCol.DockingPreview]        = new Vector4(0.26f, 0.59f, 0.98f, 0.70f);
			colors[(int) ImGuiCol.DockingEmptyBg]        = new Vector4(0.20f, 0.20f, 0.20f, 1.00f);
			colors[(int) ImGuiCol.PlotLines]             = new Vector4(0.61f, 0.61f, 0.61f, 1.00f);
			colors[(int) ImGuiCol.PlotLinesHovered]      = new Vector4(1.00f, 0.43f, 0.35f, 1.00f);
			colors[(int) ImGuiCol.PlotHistogram]         = new Vector4(0.90f, 0.70f, 0.00f, 1.00f);
			colors[(int) ImGuiCol.PlotHistogramHovered]  = new Vector4(1.00f, 0.60f, 0.00f, 1.00f);
			colors[(int) ImGuiCol.TextSelectedBg]        = new Vector4(0.26f, 0.59f, 0.98f, 0.35f);
			colors[(int) ImGuiCol.DragDropTarget]        = new Vector4(0.11f, 0.64f, 0.92f, 1.00f);
			colors[(int) ImGuiCol.NavHighlight]          = new Vector4(0.26f, 0.59f, 0.98f, 1.00f);
			colors[(int) ImGuiCol.NavWindowingHighlight] = new Vector4(1.00f, 1.00f, 1.00f, 0.70f);
			colors[(int) ImGuiCol.NavWindowingDimBg]     = new Vector4(0.80f, 0.80f, 0.80f, 0.20f);
			colors[(int) ImGuiCol.ModalWindowDimBg]      = new Vector4(0.80f, 0.80f, 0.80f, 0.35f);
			style.GrabRounding                           = style.FrameRounding = 2.3f;

			style.WindowMenuButtonPosition = ImGuiDir.None;
            
        }

        public static void Use2()
        {
	        
	        var style = ImGui.GetStyle();

	        var colors = style.Colors;
	        
            style.WindowPadding = new Vector2(5, 15); //15
			style.WindowRounding = 5.0f;
			style.FramePadding = new Vector2(5, 5);
			style.FrameRounding = 4.0f;
			style.ItemSpacing = new Vector2(12, 8);
			style.ItemInnerSpacing = new Vector2(8, 6);
			style.IndentSpacing = 5.0f;
			style.ScrollbarSize = 15.0f;
			style.ScrollbarRounding = 9.0f;
			style.GrabMinSize = 5.0f;
			style.GrabRounding = 3.0f;
			style.ScaleAllSizes(0.6f);
		 
			colors[(int) ImGuiCol.Text] = new Vector4(0.80f, 0.80f, 0.83f, 1.00f);
			colors[(int) ImGuiCol.TextDisabled] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
			colors[(int) ImGuiCol.WindowBg] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
			colors[(int) ImGuiCol.ChildBg] = new Vector4(0.07f, 0.07f, 0.09f, 1.00f);
			colors[(int) ImGuiCol.PopupBg] = new Vector4(0.07f, 0.07f, 0.09f, 1.00f);
			colors[(int) ImGuiCol.Border] = new Vector4(0.80f, 0.80f, 0.83f, 0.88f);
			colors[(int) ImGuiCol.BorderShadow] = new Vector4(0.92f, 0.91f, 0.88f, 0.00f);
			colors[(int) ImGuiCol.FrameBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
			colors[(int) ImGuiCol.FrameBgHovered] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
			colors[(int) ImGuiCol.FrameBgActive] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
			colors[(int) ImGuiCol.TitleBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
			colors[(int) ImGuiCol.TitleBgCollapsed] = new Vector4(1.00f, 0.98f, 0.95f, 0.75f);
			colors[(int) ImGuiCol.TitleBgActive] = new Vector4(0.07f, 0.07f, 0.09f, 1.00f);
			colors[(int) ImGuiCol.MenuBarBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
			colors[(int) ImGuiCol.ScrollbarBg] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
			colors[(int) ImGuiCol.ScrollbarGrab] = new Vector4(0.80f, 0.80f, 0.83f, 0.31f);
			colors[(int) ImGuiCol.ScrollbarGrabHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
			colors[(int) ImGuiCol.ScrollbarGrabActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
			//colors[(int) ImGuiCol.ComboBg] = new Vector4(0.19f, 0.18f, 0.21f, 1.00f);
			colors[(int) ImGuiCol.CheckMark] = new Vector4(0.80f, 0.80f, 0.83f, 0.31f);
			colors[(int) ImGuiCol.SliderGrab] = new Vector4(0.80f, 0.80f, 0.83f, 0.31f);
			colors[(int) ImGuiCol.SliderGrabActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
			colors[(int) ImGuiCol.Button] = new Vector4(0.10f, 0.09f, 0.12f, 1.00f);
			colors[(int) ImGuiCol.ButtonHovered] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
			colors[(int) ImGuiCol.ButtonActive] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
			
			colors[(int) ImGuiCol.Header] = new Vector4(0.4f, 0.6f, 0.38f, 1.00f);
			colors[(int) ImGuiCol.HeaderHovered] = new Vector4(0.6f, 0.5f, 0.3f, 1.00f);
			
			colors[(int) ImGuiCol.HeaderActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
			
			
			colors[(int) ImGuiCol.TabUnfocusedActive] = new Vector4(0.40f, 0.10f, 0.10f, 1.00f);
			colors[(int) ImGuiCol.TabActive] = new Vector4(0.5f, 0.2f, 0.2f, 1.00f);
			colors[(int) ImGuiCol.TabHovered] = new Vector4(0.7f, 0.4f, 0.4f, 1.00f);
			
			//colors[(int) ImGuiCol.Column] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
			//colors[(int) ImGuiCol.ColumnHovered] = new Vector4(0.24f, 0.23f, 0.29f, 1.00f);
			//colors[(int) ImGuiCol.ColumnActive] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
			colors[(int) ImGuiCol.ResizeGrip] = new Vector4(0.00f, 0.00f, 0.00f, 0.00f);
			colors[(int) ImGuiCol.ResizeGripHovered] = new Vector4(0.56f, 0.56f, 0.58f, 1.00f);
			colors[(int) ImGuiCol.ResizeGripActive] = new Vector4(0.06f, 0.05f, 0.07f, 1.00f);
			//colors[(int) ImGuiCol.CloseButton] = new Vector4(0.40f, 0.39f, 0.38f, 0.16f);
			//colors[(int) ImGuiCol.CloseButtonHovered] = new Vector4(0.40f, 0.39f, 0.38f, 0.39f);
			//colors[(int) ImGuiCol.CloseButtonActive] = new Vector4(0.40f, 0.39f, 0.38f, 1.00f);
			colors[(int) ImGuiCol.PlotLines] = new Vector4(0.40f, 0.39f, 0.38f, 0.63f);
			colors[(int) ImGuiCol.PlotLinesHovered] = new Vector4(0.25f, 1.00f, 0.00f, 1.00f);
			colors[(int) ImGuiCol.PlotHistogram] = new Vector4(0.40f, 0.39f, 0.38f, 0.63f);
			colors[(int) ImGuiCol.PlotHistogramHovered] = new Vector4(0.25f, 1.00f, 0.00f, 1.00f);
			colors[(int) ImGuiCol.TextSelectedBg] = new Vector4(0.25f, 1.00f, 0.00f, 0.43f);
			//colors[(int) ImGuiCol.ModalWindowDarkening] = new Vector4(1.00f, 0.98f, 0.95f, 0.73f);
            
        }
    }
}