using System;
using System.Diagnostics;
using System.Threading;
using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Camera;
using ArchEngine.GUI;
using ArchEngine.GUI.Editor;
using ArchEngine.GUI.ImGUI;
using ArchEngine.Scenes.Voxel;
using ImGuiNET;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace ArchEngine.Core
{

    public class Window : GameWindow
    {

        private FreeTypeFont _font;


        public static Scene activeScene;
        
        ImGuiController _controller;

        public static bool started = false;

        public static Window instance;

        public static bool isWindowFocussed = true;
        
        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
	        Window.instance = this;

        }

        public static Renderer _renderer;
        
        public static readonly log4net.ILog _log = log4net.LogManager.GetLogger("Arch");

        public static Vector2i WindowSize;

        protected override void OnLoad()
        {
            base.OnLoad();
            WindowSize = new Vector2i(Size.X, Size.Y);
            
            _log.Info("Window loading...");
            
            _log.Info("OpenGL " + APIVersion);
            
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
            
      
            _controller.DrawLoadingBarAndSwapBuffers(this, 40, "Loading renderer..");
            
            
            _log.Info("Loading Renderer...");
            _renderer = new Renderer();
          
            
            _controller.DrawLoadingBarAndSwapBuffers(this, 60, "Loading shaders...");
            
			CameraManager.Init(Size.X / (float)Size.Y);
			_log.Info("Loading shaders...");
            ShaderManager.LoadShaders();
       
            _log.Info("Starting shaders...");
            ShaderManager.StartShaders();

            
            
            
            
            
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            
            
            //stopWatch.Stop();
            //Console.WriteLine("RunTime " + stopWatch.Elapsed.Milliseconds);
            
            // Setup is now complete! Now we move to the OnRenderFrame function to finally draw the triangle.

            _controller.DrawLoadingBarAndSwapBuffers(this, 80, "Loading scene objects...");
            
            _log.Info("Loading scene objects...");

            _controller.DrawLoadingBarAndSwapBuffers(this, 90, "Initializing scene...");
            _log.Info("Initializing scene...");
            
            //activeScene = new EditorScene().AddDemo2();
            activeScene = new VoxelScene();
            
            //throw new Exception();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            activeScene.Init();
            sw.Stop();
     
            _log.Info("Scene initialization took: " + sw.ElapsedMilliseconds + "ms");
            
            
            new Editor();
            _log.Info("Arch Engine started!");
            
            //Attributes.ScanAttiributes(this);
            _skyboxRenderer = new SkyboxRenderer();
            _skyboxRenderer.Init();

           
            
        }

        private SkyboxRenderer _skyboxRenderer;

        public static bool LockCursor = false;

        private bool firstStart = true;
        
        // Now that initialization is done, let's create our render loop.
        protected override void OnRenderFrame(FrameEventArgs e)
        {
	        base.OnRenderFrame(e);
	        if (!isWindowFocussed)
	        {
		        Thread.Sleep(50);
		        return;
	        }
				


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

	        if (firstStart && started)
	        {
		        Stopwatch sw = new Stopwatch();
		        sw.Start();
		        activeScene.Start();
		        sw.Stop();
		        _log.Info("Scene start took " + sw.ElapsedMilliseconds + "ms");
		        firstStart = false;
	        }
	        if (started)
		        activeScene.Update();
	        else
		        firstStart = true;
	        


	        //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
	        
	        _controller.Update(this, (float) e.Time);
	        
	        ShaderManager.UpdateShaders(_renderer.RenderSize.X, _renderer.RenderSize.Y);

	        //activeScene.GameObjectFind("Cube").Transform = Matrix4.CreateScale(f);
	        //GL.Enable(EnableCap.CullFace);
	        //GL.FrontFace(FrontFaceDirection.Ccw);
	        //GL.CullFace(CullFaceMode.Back);
	        
	        _renderer.Use();
	        _skyboxRenderer.Render();
	        //GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Line);
	        _renderer.RenderAllChildObjects(activeScene.gameObjects);
	        //GL.PolygonMode(MaterialFace.FrontAndBack,PolygonMode.Fill);
	        //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
	        
	        //GL.CullFace(CullFaceMode.Back);

	        _font.RenderText(ShaderManager.TextShader,"FPS: " + _fps + ", TPS: " + _ticks + ", Verts: " + ((_renderer.TotalVertices > 1000) ? (_renderer.TotalVertices / 1000 + "K") : _renderer.TotalVertices) , 0.0f - (_renderer.RenderSize.X / 2f), 0.0f + (_renderer.RenderSize.Y  / 2f) - 25, 0.5f);

	        GL.Viewport(0, 0, Size.X, Size.Y);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit);
            
           
            
            //ImGui.ShowDemoWindow();
            ImGui.PushStyleVar(ImGuiStyleVar.WindowRounding, 0.0f);
            ImGui.PushStyleVar(ImGuiStyleVar.WindowBorderSize, 0.0f);
            

            
            Editor.DrawEditor();
            
            ImGui.PopStyleVar(2);

            _controller.Render();
            
            //_renderer.DisplayFullScreen(ShaderManager.FullscreenShader);
            
            SwapBuffers();
        }

        
        public static double FixedFps = 50.0; //Physics fps
        static double _limitFps = 1.0 / FixedFps;

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
			Editor.EditorUpdate();
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
	            if (started)
	            {
		            activeScene.FixedUpdate();
	            }
	            
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
            
            //_renderer.Resize(Size.X, Size.Y);
            
            //_camera.AspectRatio = Size.X / (float)Size.Y;
           _controller.WindowResized(Size.X, Size.Y); //add this back
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            base.OnMouseWheel(e);

            CameraManager.UpdateMouseWheelInput(e);
        }
        

        protected override void OnFocusedChanged(FocusedChangedEventArgs e)
        {
	        base.OnFocusedChanged(e);
	        isWindowFocussed = e.IsFocused;
	        Editor.windowFocussedNew = e.IsFocused;

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
