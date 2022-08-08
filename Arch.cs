using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using ArchEngine.Core;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using Image = OpenTK.Windowing.Common.Input.Image;

namespace ArchEngine
{
    public static class Arch
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        static void Main(string[] args)
        {

            _log.Info("Arch Engine initializing...");
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(1600, 900),
                Title = "Arch Engine",
                // This is needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
                NumberOfSamples = 8,
                APIVersion = new Version(3,3),
                Profile = ContextProfile.Core,
                Icon = AssetManager.LoadWindowIconFromFile("arch.png")
            };
            
            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.VSync = VSyncMode.Adaptive;
                window.Location = new Vector2i(1920 / 2 - 800, 1080 / 2 - 450);
                _log.Info("Creating window...");
                window.Run();
            }
        }

    }
}