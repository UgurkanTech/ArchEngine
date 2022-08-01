using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
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
        
        
        ImGuiController _controller;
        
        public static Framebuffer framebuffer;

        private IRenderable _renderable;
        private IRenderable _renderable2;

        public Window(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {

        }

        protected override void OnLoad()
        {
            base.OnLoad();
            
            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);

            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);



			CameraManager.Init(Size.X / (float)Size.Y);
            ShaderManager.LoadShaders();
            
            ShaderManager.StartShaders();

            _controller = new ImGuiController(Size.X, Size.Y);
            
            
            //Stopwatch stopWatch = new Stopwatch();
            //stopWatch.Start();
            
            _font = new FreeTypeFont(64);
            
            //stopWatch.Stop();
            //Console.WriteLine("RunTime " + stopWatch.Elapsed.Milliseconds);
            
            // Setup is now complete! Now we move to the OnRenderFrame function to finally draw the triangle.

            Material mat = new Material();
			mat.LoadTextures("Resources/Textures/wall");
            mat.Shader = ShaderManager.PbrShader;
            
            
            _renderable = new Cube();
            
            //_renderable.Indices = _indices;
            _renderable.Material = mat;
            _renderable.Init();


            _renderable2 = new Cube();
            _renderable2.Material = mat;
            _renderable2.Init();
            _renderable2.Model = Matrix4.CreateTranslation(new Vector3(2.25f,0f,0));
          
            
            framebuffer = new Framebuffer();
            //framebuffer.Init();

        }


        float f = 0;

        // Now that initialization is done, let's create our render loop.
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            
            _controller.Update(this, (float)e.Time);
            
            //framebuffer.Use();
            
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            
            GL.Viewport(0, 0, Size.X, Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            
			ShaderManager.UpdateShaders();

			_renderable.Model = Matrix4.CreateScale(f);
			_renderable.Render();

			_renderable2.Render();
			
			
			//glBindTexture(GL_TEXTURE_CUBE_MAP, irradianceMap); // display irradiance map
			
			
			
			
			
            //_shaderText.Use();

            //_shaderText.SetMatrix4("projection", ortho);

            _font.RenderText(ShaderManager.TextShader,"FPS: " + _fps, 0.0f - 800 / 2, 0.0f + 600 / 2 - 50, 1f);
            
            ImGui.SetNextWindowPos(new System.Numerics.Vector2(25,25), ImGuiCond.Once);
            ImGui.SetNextWindowSize(new System.Numerics.Vector2(300, 300));
            ImGui.Begin("Tool",  ImGuiWindowFlags.UnsavedDocument | ImGuiWindowFlags.NoResize);
            
            
            ImGui.SliderFloat("Scale", ref f, 0.0f, 1.0f);
           
            ImGui.End();
			
            _controller.Render();
            
            SwapBuffers();
        }

        
        
        static double _limitFps = 1.0 / 50.0; //Physics fps

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
