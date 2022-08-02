using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using ArchEngine.Core.ECS;
using ArchEngine.Core.ECS.Components;
using ArchEngine.Core.Rendering;
using ArchEngine.Core.Rendering.Camera;
using ArchEngine.Core.Rendering.Geometry;
using ArchEngine.Core.Rendering.Textures;
using ArchEngine.GUI;
using ArchEngine.GUI.ImGUI;
using ImGuiNET;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
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
        
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void OnLoad()
        {
            base.OnLoad();
            _log.Info("Window loading...");
            
            
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);

            _log.Info("Loading Renderer...");
            _renderer = new Renderer();
            
            
			CameraManager.Init(Size.X / (float)Size.Y);
			_log.Info("Loading shaders...");
            ShaderManager.LoadShaders();
            
            _log.Info("Starting shaders...");
            ShaderManager.StartShaders();

            _log.Info("Loading interface...");
            _controller = new ImGuiController(Size.X, Size.Y);
            Theme.Use();
            
            
            
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            _log.Info("Loading fonts...");
            _font = new FreeTypeFont(64);
            
            //stopWatch.Stop();
            //Console.WriteLine("RunTime " + stopWatch.Elapsed.Milliseconds);
            
            // Setup is now complete! Now we move to the OnRenderFrame function to finally draw the triangle.

            
            _log.Info("Loading scene objects...");
            
            
            Material mat = new Material();
			mat.LoadTextures("Resources/Textures/wall");
            mat.Shader = ShaderManager.PbrShader;
            
            
            //framebuffer.Init();

            activeScene = new Scene();

            MeshRenderer mr = new MeshRenderer();
            mr.mesh = new Cube();
            mr.mesh.Material = mat;
            
            GameObject gm = new GameObject("Cube");
            GameObject gm2 = new GameObject("Cube2");
            GameObject gm3 = new GameObject("Cube3");
            GameObject gm4 = new GameObject("Cube-1");
            GameObject gm5 = new GameObject("Cube-2");
            
            
            gm2.Transform = Matrix4.CreateTranslation(new Vector3(2f, 0f, 0)) * Matrix4.CreateScale(.7f);
            
            gm.AddComponent(mr);
            gm2.AddComponent(mr);
            gm3.AddComponent(mr);
            gm4.AddComponent(mr);
            gm5.AddComponent(mr);
            
            
            
  
            activeScene.AddGameObject(gm);
            activeScene.AddGameObject(gm4);
            activeScene.AddGameObject(gm2);
            activeScene.AddGameObject(gm3);
            gm.AddComponent(gm4);
            gm4.AddComponent(gm5);
            gm3.AddComponent(gm5);
            
           
            
            _log.Info("Initializing scene...");
            activeScene.Init();
            
            _log.Info("Arch Engine started!");
        }


        public static float f = 0.5f;

        // Now that initialization is done, let's create our render loop.
        protected override void OnRenderFrame(FrameEventArgs e)
        {
	        base.OnRenderFrame(e);

	        GL.Enable(EnableCap.DepthTest);
	        GL.DepthFunc(DepthFunction.Less);


	        _controller.Update(this, (float) e.Time);
	        
	        ShaderManager.UpdateShaders();

	        activeScene.GameObjectFind("Cube").Transform = Matrix4.CreateScale(f);
	
	        
	        _renderer.Use();
	        _renderer.RenderAllChildObjects(activeScene.gameObjects);
	        _renderer.DisplayFullScreen(ShaderManager.FullscreenShader);
            
            
            
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            
            
            _font.RenderText(ShaderManager.TextShader,"FPS: " + _fps + ", TPS: " + _ticks, 0.0f - 800 / 2, 0.0f + 600 / 2 - 50, 1f);

            //ImGui.ShowDemoWindow();
            
            Editor.DrawEditor();
            ImGui.ShowFontSelector("ad");
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
            
            
            //_camera.AspectRatio = Size.X / (float)Size.Y;
            _controller.WindowResized(Size.X, Size.Y);
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
