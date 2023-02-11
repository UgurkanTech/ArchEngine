namespace ArchEngine.Core
{
#if false
    public class TestWindow: GameWindow
    {


        public TestWindow(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
            
            
            Font font = new Font("Times New Roman", 12.0f);
            Brush brush = new SolidBrush(Color.Black);
            Bitmap tempBmp = new Bitmap(30, 30);
            Graphics tempGfx = Graphics.FromImage(tempBmp);
            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            
            string tempString = "Hello World";
            SizeF sizeF = tempGfx.MeasureString(tempString, font, 1000, StringFormat.GenericTypographic);
            Bitmap textBmp = new Bitmap((int)(sizeF.Width + 1), (int)(sizeF.Height + 1));
            Graphics gfx = Graphics.FromImage(textBmp);
            gfx.Clear(Color.Transparent);
            gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            gfx.DrawString(tempString, font, brush, 0, 0, StringFormat.GenericTypographic);

            
            stopWatch.Stop();
            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;

            // Format and display the TimeSpan value.
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds);
            Console.WriteLine("RunTime " + elapsedTime);
            
            textBmp.Save("image.png");
        }

        private Shader _shaderText;
        private Camera _camera;
        private FreeTypeFont _font;
        
        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.2f, 0.3f, 0.3f, 1.0f);
            
            _camera = new Camera(Vector3.UnitZ, Size.X / (float)Size.Y);

            _shaderText = new Shader("Shaders/text.vert", "Shaders/text.frag");
            _shaderText.Use();

            _shaderText.SetMatrix4("projection",_camera.GetProjectionMatrix());

            _font = new FreeTypeFont(32);
            
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
             
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            _font.RenderText(ref _shaderText,"Test", 50.0f, 200.0f, 1f);

           
            
            SwapBuffers();
        }
        

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (!IsFocused)
                return;
            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();
        }

        


    }
#endif
}