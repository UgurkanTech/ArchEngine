﻿using ImGuiNET;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Runtime.CompilerServices;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Vector2 = System.Numerics.Vector2;
using Window = ArchEngine.Core.Window;

namespace ArchEngine.GUI.ImGUI
{
    /// <summary>
    /// A modified version of Veldrid.ImGui's ImGuiRenderer.
    /// Manages input for ImGui and handles rendering ImGui's DrawLists with Veldrid.
    /// </summary>
    public class ImGuiController : IDisposable
    {
        private bool _frameBegun;

        // Veldrid objects
        private int _vertexArray;
        private int _vertexBuffer;
        private int _vertexBufferSize;
        private int _indexBuffer;
        private int _indexBufferSize;

        private Texture _fontTexture;
        private GUIShader _guiShader;

        private int _windowWidth;
        private int _windowHeight;

        private System.Numerics.Vector2 _scaleFactor = System.Numerics.Vector2.One;

        /// <summary>
        /// Constructs a new ImGuiController.
        /// </summary>
        public ImGuiController(int width, int height)
        {
            _windowWidth = width;
            _windowHeight = height;

            IntPtr context = ImGui.CreateContext();
            ImGui.SetCurrentContext(context);
            var io = ImGui.GetIO();
            io.Fonts.AddFontDefault();
            io.ConfigFlags |= ImGuiConfigFlags.NavEnableKeyboard; // Enable Keyboard Controls
            io.ConfigFlags |= ImGuiConfigFlags.DockingEnable; // Enable Docking
            io.ConfigFlags |= ImGuiConfigFlags.ViewportsEnable; // Enable Multi-Viewport / Platform Windows

            io.BackendFlags |= ImGuiBackendFlags.RendererHasVtxOffset;
            io.BackendFlags |= ImGuiBackendFlags.RendererHasViewports;
            io.BackendFlags |= ImGuiBackendFlags.HasMouseCursors;
            io.BackendFlags |= ImGuiBackendFlags.HasSetMousePos;

            
            
            CreateDeviceResources();
            SetKeyMappings();

            SetPerFrameImGuiData(1f / 60f);
            
            ImGui.NewFrame();
            _frameBegun = true;
        }
        
        private void setupDockspace(GameWindow wnd) {
            ImGuiWindowFlags windowFlags =  ImGuiWindowFlags.NoDocking;

            
            
            ImGui.SetNextWindowPos(new Vector2(0,0), ImGuiCond.Always);
            ImGui.SetNextWindowSize(new Vector2(wnd.Size.X, wnd.Size.Y));
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
 
            windowFlags |= ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoCollapse |
                           ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove |
                           ImGuiWindowFlags.NoBringToFrontOnFocus | ImGuiWindowFlags.NoNavFocus;
            bool pOpen = true;
            ImGui.Begin("Dockspace Demo", ref pOpen, windowFlags);
            ImGui.PopStyleVar(2);

            // Dockspace
            ImGui.DockSpace(ImGui.GetID("Dockspace"));
            ImGui.End();
        }

        public void WindowResized(int width, int height)
        {
            _windowWidth = width;
            _windowHeight = height;
        }

        public void DestroyDeviceObjects()
        {
            Dispose();
        }

        public void CreateDeviceResources()
        {
            Util.CreateVertexArray("ImGui", out _vertexArray);

            _vertexBufferSize = 10000;
            _indexBufferSize = 2000;

            Util.CreateVertexBuffer("ImGui", out _vertexBuffer);
            Util.CreateElementBuffer("ImGui", out _indexBuffer);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _indexBuffer);
            GL.BufferData(BufferTarget.ArrayBuffer, _indexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // If using opengl 4.5 this could be a better way of doing it so that we are not modifying the bound buffers
            // GL.NamedBufferData(_vertexBuffer, _vertexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
            // GL.NamedBufferData(_indexBuffer, _indexBufferSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);

            RecreateFontDeviceTexture();

            string VertexSource = @"#version 330 core
uniform mat4 projection_matrix;
layout(location = 0) in vec2 in_position;
layout(location = 1) in vec2 in_texCoord;
layout(location = 2) in vec4 in_color;
out vec4 color;
out vec2 texCoord;
void main()
{
    gl_Position = projection_matrix * vec4(in_position, 0, 1);
    color = in_color;
    texCoord = in_texCoord;
}";
            string FragmentSource = @"#version 330 core
uniform sampler2D in_fontTexture;
in vec4 color;
in vec2 texCoord;
out vec4 outputColor;
void main()
{
    outputColor = color * texture(in_fontTexture, texCoord);
}";
            _guiShader = new GUIShader("ImGui", VertexSource, FragmentSource);

            GL.BindVertexArray(_vertexArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _indexBuffer);

            int stride = Unsafe.SizeOf<ImDrawVert>();

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, stride, 0);
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 2, VertexAttribPointerType.Float, false, stride, 8);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.UnsignedByte, true, stride, 16);

            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            // We don't need to unbind the element buffer as that is connected to the vertex array
            // And you should not touch the element buffer when there is no vertex array bound.

            Util.CheckGLError("End of ImGui setup");
        }

        /// <summary>
        /// Recreates the device texture used to render text.
        /// </summary>
        public void RecreateFontDeviceTexture()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.Fonts.GetTexDataAsRGBA32(out IntPtr pixels, out int width, out int height, out int bytesPerPixel);

            _fontTexture = new Texture("ImGui Text Atlas", width, height, pixels);
            _fontTexture.SetMagFilter(TextureMagFilter.Linear);
            _fontTexture.SetMinFilter(TextureMinFilter.Linear);

            io.Fonts.SetTexID((IntPtr)_fontTexture.GLTexture);

            io.Fonts.ClearTexData();
        }

        /// <summary>
        /// Renders the ImGui draw list data.
        /// This method requires a <see cref="GraphicsDevice"/> because it may create new DeviceBuffers if the size of vertex
        /// or index data has increased beyond the capacity of the existing buffers.
        /// A <see cref="CommandList"/> is needed to submit drawing and resource update commands.
        /// </summary>
        public void Render()
        {
            if (_frameBegun)
            {
                _frameBegun = false;
                
                ImGui.Render();
                
                RenderImDrawData(ImGui.GetDrawData());

                Util.CheckGLError("Imgui Controller");
            }
        }

        /// <summary>
        /// Updates ImGui input and IO configuration state.
        /// </summary>
        public void Update(GameWindow wnd, float deltaSeconds)
        {
            if (_frameBegun)
            {
                ImGui.Render();
            }

            SetPerFrameImGuiData(deltaSeconds);
            UpdateImGuiInput(wnd);
            
            _frameBegun = true;
            ImGui.NewFrame();
            setupDockspace(wnd);
            ImGui.Begin("Scene", ImGuiWindowFlags.Modal);
            
            //pass the texture of the FBO
            //window.getRenderTexture() is the texture of the FBO
            //the next parameter is the upper left corner for the uvs to be applied at
            //the third parameter is the lower right corner
            //the last two parameters are the UVs
            //they have to be flipped (normally they would be (0,0);(1,1) 
            
            Vector2 vMin = ImGui.GetWindowContentRegionMin();
            Vector2 vMax = ImGui.GetWindowContentRegionMax();

            vMin.X += ImGui.GetWindowPos().X;
            vMin.Y += ImGui.GetWindowPos().Y;
            vMax.X += ImGui.GetWindowPos().X;
            vMax.Y += ImGui.GetWindowPos().Y;

            Vector2 size = (vMax - vMin);
            
            
            ImGui.GetWindowDrawList().AddImage(
                new IntPtr(Window.framebuffer.frameBufferTexture), //use real pointer
                ImGui.GetCursorScreenPos(),
                new Vector2(ImGui.GetCursorScreenPos().X + size.X, 
                    ImGui.GetCursorScreenPos().Y + size.Y), new Vector2(0, 1), new Vector2(1, 0));


            //Resizes framebuffer:
            //GL.BindTexture(TextureTarget.Texture2D, 2);
            //GL.TexImage2D(TextureTarget.Texture2D,0,PixelInternalFormat.Rgb,(int)size.X,(int)size.Y,0,PixelFormat.Rgb,PixelType.UnsignedByte,IntPtr.Zero);


           
            ImGui.End();

            
        }

        /// <summary>
        /// Sets per-frame data based on the associated window.
        /// This is called by Update(float).
        /// </summary>
        private void SetPerFrameImGuiData(float deltaSeconds)
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.DisplaySize = new System.Numerics.Vector2(
                _windowWidth / _scaleFactor.X,
                _windowHeight / _scaleFactor.Y);
            io.DisplayFramebufferScale = _scaleFactor;
            io.DeltaTime = deltaSeconds; // DeltaTime is in seconds.
        }

        MouseState PrevMouseState;
        KeyboardState PrevKeyboardState;
        readonly List<char> PressedChars = new List<char>();

        private void UpdateImGuiInput(GameWindow wnd)
        {
            
                ImGuiIOPtr io = ImGui.GetIO();

                MouseState MouseState = wnd.MouseState;
                KeyboardState KeyboardState = wnd.KeyboardState;

                io.MouseDown[0] = MouseState[MouseButton.Left];
                io.MouseDown[1] = MouseState[MouseButton.Right];
                io.MouseDown[2] = MouseState[MouseButton.Middle];

                io.MouseWheel = wnd.MouseState.ScrollDelta.Y;

                var screenPoint = new Vector2i((int)MouseState.X, (int)MouseState.Y);
                var point = screenPoint;//wnd.PointToClient(screenPoint);
                io.MousePos = new System.Numerics.Vector2(point.X, point.Y);
            
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                {
                    if (key == Keys.Unknown)
                    {
                        continue;
                    }
                    io.KeysDown[(int)key] = KeyboardState.IsKeyDown(key);
                }

                foreach (var c in PressedChars)
                {
                    io.AddInputCharacter(c);
                }
                PressedChars.Clear();

                io.KeyCtrl = KeyboardState.IsKeyDown(Keys.LeftControl) || KeyboardState.IsKeyDown(Keys.RightControl);
                io.KeyAlt = KeyboardState.IsKeyDown(Keys.LeftAlt) || KeyboardState.IsKeyDown(Keys.RightAlt);
                io.KeyShift = KeyboardState.IsKeyDown(Keys.LeftShift) || KeyboardState.IsKeyDown(Keys.RightShift);
                io.KeySuper = KeyboardState.IsKeyDown(Keys.LeftSuper) || KeyboardState.IsKeyDown(Keys.RightSuper);

                
                
        }


        internal void PressChar(char keyChar)
        {
            PressedChars.Add(keyChar);
        }

        private static void SetKeyMappings()
        {
            ImGuiIOPtr io = ImGui.GetIO();
            io.KeyMap[(int)ImGuiKey.Tab] = (int)Keys.Tab;
            io.KeyMap[(int)ImGuiKey.LeftArrow] = (int)Keys.Left;
            io.KeyMap[(int)ImGuiKey.RightArrow] = (int)Keys.Right;
            io.KeyMap[(int)ImGuiKey.UpArrow] = (int)Keys.Up;
            io.KeyMap[(int)ImGuiKey.DownArrow] = (int)Keys.Down;
            io.KeyMap[(int)ImGuiKey.PageUp] = (int)Keys.PageUp;
            io.KeyMap[(int)ImGuiKey.PageDown] = (int)Keys.PageDown;
            io.KeyMap[(int)ImGuiKey.Home] = (int)Keys.Home;
            io.KeyMap[(int)ImGuiKey.End] = (int)Keys.End;
            io.KeyMap[(int)ImGuiKey.Delete] = (int)Keys.Delete;
            io.KeyMap[(int)ImGuiKey.Backspace] = (int)Keys.Backspace;
            io.KeyMap[(int)ImGuiKey.Space] = (int)Keys.Space;
            io.KeyMap[(int)ImGuiKey.Enter] = (int)Keys.Enter;
            io.KeyMap[(int)ImGuiKey.Escape] = (int)Keys.Escape;
            io.KeyMap[(int)ImGuiKey.A] = (int)Keys.A;
            io.KeyMap[(int)ImGuiKey.C] = (int)Keys.C;
            io.KeyMap[(int)ImGuiKey.V] = (int)Keys.V;
            io.KeyMap[(int)ImGuiKey.X] = (int)Keys.X;
            io.KeyMap[(int)ImGuiKey.Y] = (int)Keys.Y;
            io.KeyMap[(int)ImGuiKey.Z] = (int)Keys.Z;
        }

        private void RenderImDrawData(ImDrawDataPtr draw_data)
        {
            uint vertexOffsetInVertices = 0;
            uint indexOffsetInElements = 0;

            if (draw_data.CmdListsCount == 0)
            {
                return;
            }

            uint totalVBSize = (uint)(draw_data.TotalVtxCount * Unsafe.SizeOf<ImDrawVert>());
            if (totalVBSize > _vertexBufferSize)
            {
                int newSize = (int)Math.Max(_vertexBufferSize * 1.5f, totalVBSize);

                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                _vertexBufferSize = newSize;

                Console.WriteLine($"Resized vertex buffer to new size {_vertexBufferSize}");
            }

            uint totalIBSize = (uint)(draw_data.TotalIdxCount * sizeof(ushort));
            if (totalIBSize > _indexBufferSize)
            {
                int newSize = (int)Math.Max(_indexBufferSize * 1.5f, totalIBSize);

                GL.BindBuffer(BufferTarget.ArrayBuffer, _indexBuffer);
                GL.BufferData(BufferTarget.ArrayBuffer, newSize, IntPtr.Zero, BufferUsageHint.DynamicDraw);
                GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

                _indexBufferSize = newSize;

                Console.WriteLine($"Resized index buffer to new size {_indexBufferSize}");
            }


            for (int i = 0; i < draw_data.CmdListsCount; i++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdListsRange[i];

                GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBuffer);
                GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(vertexOffsetInVertices * Unsafe.SizeOf<ImDrawVert>()), cmd_list.VtxBuffer.Size * Unsafe.SizeOf<ImDrawVert>(), cmd_list.VtxBuffer.Data);

                Util.CheckGLError($"Data Vert {i}");

                GL.BindBuffer(BufferTarget.ArrayBuffer, _indexBuffer);
                GL.BufferSubData(BufferTarget.ArrayBuffer, (IntPtr)(indexOffsetInElements * sizeof(ushort)), cmd_list.IdxBuffer.Size * sizeof(ushort), cmd_list.IdxBuffer.Data);

                Util.CheckGLError($"Data Idx {i}");

                vertexOffsetInVertices += (uint)cmd_list.VtxBuffer.Size;
                indexOffsetInElements += (uint)cmd_list.IdxBuffer.Size;
            }
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

            // Setup orthographic projection matrix into our constant buffer
            ImGuiIOPtr io = ImGui.GetIO();
            Matrix4 mvp = Matrix4.CreateOrthographicOffCenter(
                0.0f,
                io.DisplaySize.X,
                io.DisplaySize.Y,
                0.0f,
                -1.0f,
                1.0f);

            _guiShader.UseShader();
            GL.UniformMatrix4(_guiShader.GetUniformLocation("projection_matrix"), false, ref mvp);
            GL.Uniform1(_guiShader.GetUniformLocation("in_fontTexture"), 0);
            Util.CheckGLError("Projection");

            GL.BindVertexArray(_vertexArray);
            Util.CheckGLError("VAO");

            draw_data.ScaleClipRects(io.DisplayFramebufferScale);

            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.ScissorTest);
            GL.BlendEquation(BlendEquationMode.FuncAdd);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Disable(EnableCap.CullFace);
            GL.Disable(EnableCap.DepthTest);

            // Render command lists
            int vtx_offset = 0;
            int idx_offset = 0;
            for (int n = 0; n < draw_data.CmdListsCount; n++)
            {
                ImDrawListPtr cmd_list = draw_data.CmdListsRange[n];
                for (int cmd_i = 0; cmd_i < cmd_list.CmdBuffer.Size; cmd_i++)
                {
                    ImDrawCmdPtr pcmd = cmd_list.CmdBuffer[cmd_i];
                    if (pcmd.UserCallback != IntPtr.Zero)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        GL.ActiveTexture(TextureUnit.Texture0);
                        GL.BindTexture(TextureTarget.Texture2D, (int)pcmd.TextureId);
                        Util.CheckGLError("Texture");

                        // We do _windowHeight - (int)clip.W instead of (int)clip.Y because gl has flipped Y when it comes to these coordinates
                        var clip = pcmd.ClipRect;
                        GL.Scissor((int)clip.X, _windowHeight - (int)clip.W, (int)(clip.Z - clip.X), (int)(clip.W - clip.Y));
                        Util.CheckGLError("Scissor");

                        GL.DrawElementsBaseVertex(PrimitiveType.Triangles, (int)pcmd.ElemCount, DrawElementsType.UnsignedShort, (IntPtr)(idx_offset * sizeof(ushort)), vtx_offset);
                        Util.CheckGLError("Draw");
                    }

                    idx_offset += (int)pcmd.ElemCount;
                }
                vtx_offset += cmd_list.VtxBuffer.Size;
            }

            GL.Disable(EnableCap.Blend);
            GL.Disable(EnableCap.ScissorTest);
        }

        /// <summary>
        /// Frees all graphics resources used by the renderer.
        /// </summary>
        public void Dispose()
        {
            _fontTexture.Dispose();
            _guiShader.Dispose();
        }
    }
}