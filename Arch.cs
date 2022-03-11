using System;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
namespace ArchEngine
{
    public static class Arch
    {
        static void Main(string[] args)
        {
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Arch Engine",
                // This is needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
                NumberOfSamples = 4
            };



            using (var window = new Window(GameWindowSettings.Default, nativeWindowSettings))
            {
                window.Run();
            }
        }

    }
}