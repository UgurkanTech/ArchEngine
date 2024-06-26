﻿using System;
using System.Threading;
using ArchEngine.Core;
using ArchEngine.GUI.Editor;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System.Windows.Forms;
using OpenTK.Mathematics;

namespace ArchEngine
{
    /*
     * Arch Engine
     * Version: 1.5.0
     * Written by -Uğurkan Hoşgör
     */
    public static class Arch
    {
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static String path = "";
       
        [STAThread]
        static void Main(string[] args)
        {
            ResourceStream icon = new ResourceStream("arch.png", null);
            //ArchGTK gtk = new ArchGTK(icon.GetStream());


            
            var thread =  new System.Threading.Thread(() =>
            {
                Console.WriteLine("Starting project selector..");
                //path = gtk.SelectFolder();
                var dialog = new FolderBrowserDialog();
                dialog.Description = "Select the project directory";
                dialog.UseDescriptionForTitle = true;
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string selectedFolder = dialog.SelectedPath;
                    // Do something with the selected folder
                    path = selectedFolder;
                }
            });
            thread.Priority = ThreadPriority.Highest;
            thread.SetApartmentState(ApartmentState.STA);
#if DEBUG
            path = @"C:\Users\ugurkan\Desktop\Project Directory";
#else
            thread.Start();
#endif
            
            
            
            
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream file = assembly.GetManifestResourceStream("ArchEngine.log4net.config");
            log4net.Config.XmlConfigurator.Configure(file);
            _log.Info("Arch Engine initializing...");
            var nativeWindowSettings = new NativeWindowSettings()
            {
                Size = new Vector2i(800, 600),
                Title = "Arch Engine",
                // This is needed to run on macos
                //AutoLoadBindings = true,
                Flags = ContextFlags.ForwardCompatible,
                NumberOfSamples = 0,
                APIVersion = new Version(3,3),
                Profile = ContextProfile.Core,
                Icon = AssetManager.LoadWindowIcon(icon.GetStream()),
                StartVisible = false,
                StartFocused = false
            };
            
            //Test.Run();
            //return;

            using Window window = new Window(GameWindowSettings.Default, nativeWindowSettings);
            window.VSync = VSyncMode.Off;
            //window.Location = new Vector2i(1920 / 2 - 400, 1080 / 2 - 300);
            _log.Info("Waiting project selection.");
            
#if DEBUG
#else
            thread.Join();
#endif

            if (path == "")
            {
                _log.Info("Project is not selected. Aborted.");
                return;
            }
                
            
            window.IsVisible = true;
            _log.Info("Creating window...");
            window.Focus();
            window.Run();
            
        }



    }
}