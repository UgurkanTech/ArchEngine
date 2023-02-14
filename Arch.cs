using System;
using ArchEngine.Core;
using ArchEngine.GUI.Editor;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace ArchEngine
{
    public static class Arch
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static String path = "";
        static void Main(string[] args)
        {
            ResourceStream icon = new ResourceStream("arch.png", null);
            ArchGTK gtk = new ArchGTK(icon.GetStream());

            path = gtk.SelectFolder();

            if (path == "")
                return;

            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream file = assembly.GetManifestResourceStream("ArchEngine.log4net.config");
            log4net.Config.XmlConfigurator.Configure(file);
            _log.Info("Arch Engine initializing...");
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Arch Engine",
                // This is needed to run on macos
                Flags = ContextFlags.Default,
                NumberOfSamples = 8,
                APIVersion = new Version(3,3),
                Profile = ContextProfile.Core,
                Icon = AssetManager.LoadWindowIcon(icon.GetStream())
            };
            
            //Test.Run();
            //return;

            using var window = new Window(GameWindowSettings.Default, nativeWindowSettings);
            window.VSync = VSyncMode.Adaptive;
            //window.Location = new Vector2i(1920 / 2 - 400, 1080 / 2 - 300);
            _log.Info("Creating window...");
            window.Run();
        }

    }
}