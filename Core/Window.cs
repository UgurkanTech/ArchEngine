using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Camera;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.GUI;
using ArchEngine.GUI.Editor;
using ArchEngine.GUI.ImGUI;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using Texture = ArchEngine.Core.Rendering.Textures.Texture;

namespace ArchEngine.Core
{

    public class Window : GameWindow
    {

        private FreeTypeFont _font;


        public static Scene activeScene;
        
        ImGuiController _controller;
        

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        public static Renderer _renderer;
        
        public static readonly log4net.ILog _log = log4net.LogManager.GetLogger("Arch");

        public static Vector2i WindowSize;

        protected override void OnLoad()
        {
            base.OnLoad();
            WindowSize = new Vector2i(Size.X, Size.Y);
            
            _log.Info("Window loading...");
            
            
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            

           // GL.StencilMask(0x00);
            
            _log.Info("Loading interface...");
            _controller = new ImGuiController(Size.X, Size.Y);
            Theme.Use();
            
            _controller.DrawLoadingBarAndSwapBuffers(this, 20, "Loading fonts...");
            
            AssetManager.LoadEditor();
            
            _log.Info("Loading fonts...");
            _font = new FreeTypeFont(64);
            
            Thread.Sleep(100);
            _controller.DrawLoadingBarAndSwapBuffers(this, 40, "Loading renderer..");
            
            
            _log.Info("Loading Renderer...");
            _renderer = new Renderer();
            Thread.Sleep(100);
            
            _controller.DrawLoadingBarAndSwapBuffers(this, 60, "Loading shaders...");
            
			CameraManager.Init(Size.X / (float)Size.Y);
			_log.Info("Loading shaders...");
            ShaderManager.LoadShaders();
            Thread.Sleep(100);
            _log.Info("Starting shaders...");
            ShaderManager.StartShaders();

            
            
            
            
            
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            
            
            //stopWatch.Stop();
            //Console.WriteLine("RunTime " + stopWatch.Elapsed.Milliseconds);
            
            // Setup is now complete! Now we move to the OnRenderFrame function to finally draw the triangle.

            _controller.DrawLoadingBarAndSwapBuffers(this, 80, "Loading scene objects...");
            
            _log.Info("Loading scene objects...");
            Thread.Sleep(100);
           
            _controller.DrawLoadingBarAndSwapBuffers(this, 90, "Initializing scene...");
            _log.Info("Initializing scene...");
            
            activeScene = new EditorScene();
            activeScene.Init();
            
            
            new Editor();
            activeScene.Start();
            _log.Info("Arch Engine started!");
            
            //Attributes.ScanAttiributes(this);
            
        }
        

        public static bool LockCursor = false;

        // Now that initialization is done, let's create our render loop.
        protected override void OnRenderFrame(FrameEventArgs e)
        {
	        base.OnRenderFrame(e);

	        GL.Enable(EnableCap.DepthTest);
	        GL.DepthFunc(DepthFunction.Less);
	        
	        
	        
	        
	        if (LockCursor)
	        {
		        CursorGrabbed = true;
	        }
	        else
	        {
		        CursorGrabbed = false;
		        CursorVisible = true;
	        }
	        
	        activeScene.Update();
	        
	        
	        _controller.Update(this, (float) e.Time);
	        
	        ShaderManager.UpdateShaders(_renderer.RenderSize.X, _renderer.RenderSize.Y);

	        //activeScene.GameObjectFind("Cube").Transform = Matrix4.CreateScale(f);
	
	        
	        _renderer.Use();
	        
	        _renderer.RenderAllChildObjects(activeScene.gameObjects);
	        
	        
	        //_renderer.DisplayFullScreen(ShaderManager.FullscreenShader);
            
	        _font.RenderText(ShaderManager.TextShader,"FPS: " + _fps + ", TPS: " + _ticks, 0.0f - (_renderer.RenderSize.X / 2f), 0.0f + (_renderer.RenderSize.Y  / 2f) - 50, 1f);
            
	        GL.Viewport(0, 0, Size.X, Size.Y);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            
           
            
            //ImGui.ShowDemoWindow();
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
            
            
            Editor.DrawEditor();
            ImGui.PopStyleVar(2);
            //ImGui.ShowFontSelector("ad");
            _controller.Render();
            

            
            SwapBuffers();
        }

        
        
        static double _limitFps = 1.0 / 30.0; //Physics fps

        double _lastTime = GLFW.GetTime(), _nowTime = 0, _timer = 0, _delta = 0;

        int _frames = 0, _fixedUpdates = 0;
        int[] _averageFps = new int[10];
        double _deltaTime = 0;
        int _fps, _ticks;
        
        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            WindowSize = new Vector2i(Size.X, Size.Y);

            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;
            var mouse = MouseState;
            
            CameraManager.UpdateCamera(input, mouse, e);
            

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            
            
            
            //Time for frames
            _nowTime = GLFW.GetTime();
            _delta += (_nowTime - _lastTime) / _limitFps;
            _deltaTime = _nowTime - _lastTime;
            _lastTime = _nowTime;

            // - Only update at 60 frames / s
            while (_delta >= 1.0) {
	            //fixedGameLoop();   // - Update function
	            
	            
	            _fixedUpdates++;
	            _delta--;
            }
            _frames++;
            if (GLFW.GetTime() - _timer > 1.0) {
	            _timer++;
	            _averageFps[(int)GLFW.GetTime() % 10] = _frames;
	            double avg = 0;
	            for (int i = 0; i < 10; i++)
	            {
		            avg += _averageFps[i];
	            }
	            avg /= 10;

	            //std::cout << "Render FPS: " << frames << " Fixed Updates:" << fixedUpdates << " Avg:" << avg << std::endl;
	            _fps = _frames;
	            _ticks = _fixedUpdates;
	            _fixedUpdates = 0;
	            _frames = 0;
            }
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, Size.X, Size.Y);
            //_renderer.Resize(Size.X, Size.Y);
            
            //_camera.AspectRatio = Size.X / (float)Size.Y;
           _controller.WindowResized(Size.X, Size.Y); //add this back
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            CameraManager.UpdateMouseWheelInput(e);
        }


        protected override void OnUnload()
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.


            //GL.DeleteProgram(_shader.handle);

            base.OnUnload();
        }
    }
}
