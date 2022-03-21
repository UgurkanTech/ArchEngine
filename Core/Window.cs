using System;
using System.Diagnostics;
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

        private readonly float[] _vertices =
        {
			//Positions          //UVS         //Normals

			//Front
		   -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f, -1.0f,  1.0f,  //bottom left
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
			1.0f, -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  //bottom right
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
		   -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f, -1.0f,  1.0f,  //bottom left
		   -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  1.0f,  //top left

			//Back
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
			1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  //top right
			1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  1.0f, -1.0f, -1.0f,  //bottom right
			1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  //top right
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
		   -1.0f,  1.0f, -1.0f,  0.0f,  1.0f, -1.0f,  1.0f, -1.0f,  //top left

			//Right
			1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  1.0f, -1.0f, -1.0f,  //bottom left
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
			1.0f, -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  //bottom right
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
			1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  1.0f, -1.0f, -1.0f,  //bottom left
			1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  1.0f,  1.0f, -1.0f,  //top left

			//Left
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
		   -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  //top right
		   -1.0f, -1.0f,  1.0f,  1.0f,  0.0f, -1.0f, -1.0f,  1.0f,  //bottom right
		   -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  1.0f,  //top right
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
		   -1.0f,  1.0f, -1.0f,  0.0f,  1.0f, -1.0f,  1.0f, -1.0f,  //top left

			//Top
		   -1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f,  1.0f, -1.0f,  //bottom left
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
			1.0f,  1.0f, -1.0f,  1.0f,  0.0f,  1.0f,  1.0f, -1.0f,  //bottom right
			1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
		   -1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f,  1.0f, -1.0f,  //bottom left
		   -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  1.0f,  //top left

			//Bottom
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
			1.0f, -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  //top right
			1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  1.0f, -1.0f, -1.0f,  //bottom right
			1.0f, -1.0f,  1.0f,  1.0f,  1.0f,  1.0f, -1.0f,  1.0f,  //top right
		   -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f, -1.0f, -1.0f,  //bottom left
		   -1.0f, -1.0f,  1.0f,  0.0f,  1.0f, -1.0f, -1.0f,  1.0f,  //top left
        };
        
        
        private readonly float[] _verticesPlane =
        {
	        //Positions          //UVS         //Normals

	        //Front
	        -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f, -1.0f,  1.0f,  //bottom left
	        1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
	        1.0f, -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  //bottom right
	        1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  1.0f,  //top right
	        -1.0f, -1.0f,  1.0f,  0.0f,  0.0f, -1.0f, -1.0f,  1.0f,  //bottom left
	        -1.0f,  1.0f,  1.0f,  0.0f,  1.0f, -1.0f,  1.0f,  1.0f,  //top left
	    };

        private readonly uint[] _indicesPlane =
        {
	        //front
	        0, 7, 3,
	        0, 4, 7
        };
        

        private readonly uint[] _indices =
        {
				//front
				0, 7, 3,
				0, 4, 7,
				//back
				1, 2, 6,
				6, 5, 1,
				//left
				0, 2, 1,
				0, 3, 2,
				//right
				4, 5, 6,
				6, 7, 4,
				//top
				2, 3, 6,
				6, 3, 7,
				//bottom
				0, 1, 5,
				0, 5, 4
        };
        





        
        
        private UniqueTexture _texture;
        private Texture _texturePbr;

        private FreeTypeFont _font;
        private Shader _shader;

        private Shader _shaderText;
        
        private Shader _shaderPbr;

        
        ImGuiController _controller;
        
        private Camera _camera;

        public static Framebuffer _framebuffer;

        private IRenderable _renderable;

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
            
            _shader = new Shader("Resources/Shaders/shader.vert", "Resources/Shaders/shader.frag");
            _shader.Use();
            
            _shaderPbr = new Shader("Resources/Shaders/pbr.vert", "Resources/Shaders/pbr.frag");
            _shaderPbr.Use();
            
            
            _shaderPbr.SetInt("irradianceMap", 0);
            _shaderPbr.SetInt("prefilterMap", 1);
            _shaderPbr.SetInt("brdfLUT", 2);
            _shaderPbr.SetInt("albedoMap", 3);
            _shaderPbr.SetInt("normalMap", 4);
            _shaderPbr.SetInt("metallicMap", 5);
            _shaderPbr.SetInt("roughnessMap", 6);
            _shaderPbr.SetInt("aoMap", 7);
            

            _controller = new ImGuiController(Size.X, Size.Y);
            
            _camera = new Camera(Vector3.UnitZ * 1, Size.X / (float)Size.Y);

            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            
            _texture = Texture.LoadFromFile("Resources/Textures/wall/albedo.png");
            _texturePbr = Texture.LoadPbrFromFile("Resources/Textures/wall");

            _shaderText = new Shader("Resources/Shaders/text.vert", "Resources/Shaders/text.frag");
            Matrix4 ortho = Matrix4.CreateOrthographic(this.Size.X, this.Size.Y, 0, 100);
            _shaderText.SetMatrix4("projection", ortho);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            _font = new FreeTypeFont(64);

            
            stopWatch.Stop();
            Console.WriteLine("RunTime " + stopWatch.Elapsed.Milliseconds);
            
            // Setup is now complete! Now we move to the OnRenderFrame function to finally draw the triangle.

            _renderable = new Cube();
            //_renderable.Indices = _indices;
            _renderable.Vertices = _vertices;
            _renderable.Shader = _shaderPbr;
            _renderable.Texture = _texturePbr;
            _renderable.Model = Matrix4.Identity * Matrix4.CreateScale(0.25f);
            _renderable.Init();

            _framebuffer = new Framebuffer();

            _framebuffer.Init();
            
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.TextureCubeMap, _texture.Handle);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.TextureCubeMap, _texture.Handle);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, _texture.Handle);
            
        }
        // Now that initialization is done, let's create our render loop.
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            
            _controller.Update(this, (float)e.Time);
            
            _framebuffer.Use();
            

            Matrix4 ortho = Matrix4.CreateOrthographic(800, 600, 0, 100);

            _shader.SetMatrix4("view", _camera.GetViewMatrix());
            _shader.SetMatrix4("projection", _camera.GetProjectionMatrix());
            //_shader.SetVector3("camPos", _camera.Position);
            
            
            _shaderPbr.SetMatrix4("view", _camera.GetViewMatrix());
            _shaderPbr.SetMatrix4("projection", _camera.GetProjectionMatrix());
            _shaderPbr.SetVector3("camPos", _camera.Position);
            
			_shaderPbr.SetVector3("lightPositions[0]", Vector3.One);
			_shaderPbr.SetVector3("lightColors[0]", new Vector3(10,10,10));
			_shaderPbr.SetInt("lightCount", 1);
            
			
            
			_renderable.Render();
			

            _shaderText.Use();

            _shaderText.SetMatrix4("projection", ortho);

            _font.RenderText(ref _shaderText,"FPS: " + fps, 0.0f - 800 / 2, 0.0f + 600 / 2 - 50, 1f);
            
            
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, Size.X, Size.Y);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            
            ImGui.ShowDemoWindow();

			
            _controller.Render();
            
            SwapBuffers();
        }
        private bool _firstMove = true;

        private Vector2 _lastPos;
        
        
        static double limitFPS = 1.0 / 30.0; //Physics fps

        double lastTime = GLFW.GetTime(), nowTime = 0, timer = 0, delta = 0;

        int frames = 0, fixedUpdates = 0;
        int[] averageFPS = new int[10];
        double deltaTime = 0;
        int fps, ticks;
        
        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);


            if (!IsFocused) // Check to see if the window is focused
            {
                return;
            }

            var input = KeyboardState;

            if (input.IsKeyDown(Keys.Escape))
            {
                Close();
            }

            const float cameraSpeed = 1.5f;
            const float sensitivity = 0.2f;

            if (input.IsKeyDown(Keys.W))
            {
                _camera.Position += _camera.Front * cameraSpeed * (float)e.Time; // Forward
            }

            if (input.IsKeyDown(Keys.S))
            {
                _camera.Position -= _camera.Front * cameraSpeed * (float)e.Time; // Backwards
            }
            if (input.IsKeyDown(Keys.A))
            {
                _camera.Position -= _camera.Right * cameraSpeed * (float)e.Time; // Left
            }
            if (input.IsKeyDown(Keys.D))
            {
                _camera.Position += _camera.Right * cameraSpeed * (float)e.Time; // Right
            }
            if (input.IsKeyDown(Keys.LeftShift))
            {
                _camera.Position += _camera.Up * cameraSpeed * (float)e.Time; // Up
            }
            if (input.IsKeyDown(Keys.LeftControl))
            {
                _camera.Position -= _camera.Up * cameraSpeed * (float)e.Time; // Down
            }


            var mouse = MouseState;

            if (_firstMove)
            {
                _lastPos = new Vector2(mouse.X, mouse.Y);
                _firstMove = false;
            }
            else
            {
	            var deltaX = mouse.X - _lastPos.X;
	            var deltaY = mouse.Y - _lastPos.Y;
	            _lastPos = new Vector2(mouse.X, mouse.Y);

	            if (MouseState.IsButtonDown(MouseButton.Button2))
	            {
		            // Apply the camera pitch and yaw (we clamp the pitch in the camera class)
		            _camera.Yaw += deltaX * sensitivity;
		            _camera.Pitch -= deltaY * sensitivity; // Reversed since y-coordinates range from bottom to top
	            }
            }
            
            
            //Time for frames
            nowTime = GLFW.GetTime();
            delta += (nowTime - lastTime) / limitFPS;
            deltaTime = nowTime - lastTime;
            lastTime = nowTime;

            // - Only update at 60 frames / s
            while (delta >= 1.0) {
	            //fixedGameLoop();   // - Update function
	            fixedUpdates++;
	            delta--;
            }
            frames++;
            if (GLFW.GetTime() - timer > 1.0) {
	            timer++;
	            averageFPS[(int)GLFW.GetTime() % 10] = frames;
	            double avg = 0;
	            for (int i = 0; i < 10; i++)
	            {
		            avg += averageFPS[i];
	            }
	            avg /= 10;

	            //std::cout << "Render FPS: " << frames << " Fixed Updates:" << fixedUpdates << " Avg:" << avg << std::endl;
	            fps = frames;
	            ticks = fixedUpdates;
	            fixedUpdates = 0;
	            frames = 0;
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

            _camera.Fov -= e.OffsetY;
        }


        protected override void OnUnload()
        {
            // Unbind all the resources by binding the targets to 0/null.
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            // Delete all the resources.


            GL.DeleteProgram(_shader.Handle);

            base.OnUnload();
        }
    }
}
