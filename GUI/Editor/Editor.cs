using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ArchEngine.Core.ECS;
using ArchEngine.GUI.Editor.Windows;
using Scene = ArchEngine.GUI.Editor.Windows.Scene;
using Window = ArchEngine.Core.Window;

namespace ArchEngine.GUI.Editor
{
    public class Editor
    {

        public static GameObject selectedGameobject;

        public static FileSystemWatcherClass fileSystemWatcherClass;

        public enum EditorState
        {
            NeedsCompiling,
            Compiling,
            Idle,
            Loading,
            Playing
        }

        public static EditorState state = EditorState.NeedsCompiling;

        private static bool filesChanged = false;
        public static bool windowFocussedNew = false;
        public static RuntimeCompiler<Script> compiler;

        public static string projectDir = "";
        public Editor()
        {
            projectDir = Arch.path;
            if (!Directory.Exists(projectDir))
            {
                projectDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                state = EditorState.Compiling; //do not compile
            }
            
            fileSystemWatcherClass = new FileSystemWatcherClass(projectDir);
            fileSystemWatcherClass.Start();
            new ConsoleWindow();
            new AssetsWindow(projectDir);
            fileSystemWatcherClass.onChangeEvent += AssetsWindow.UpdateFolder;
            fileSystemWatcherClass.onChangeEvent += UpdateScripts;
            
            compiler = new RuntimeCompiler<Script>();

            

            Icons.LoadIcons();
        }

        public static void UpdateScripts(object sender, EventArgs e)
        {
            filesChanged = true;
        }

        public static void EditorUpdate()
        {
            if (filesChanged && windowFocussedNew)
            {
                windowFocussedNew = false;
                filesChanged = false;
                if (state == EditorState.Idle)
                {
                    state = EditorState.NeedsCompiling;
                }
                
            }

            if (state == EditorState.NeedsCompiling && Window.activeScene != null)
            {
                state = EditorState.Compiling;
                new Thread(() =>
                {
                    try
                    {
                        Dictionary<GameObject, string> typeCache = new Dictionary<GameObject, string>();
                        foreach (var script in compiler.GetObjects())
                        {
                            foreach (var activeSceneGameObject in Window.activeScene.gameObjects)
                            {
                                foreach (var component in activeSceneGameObject._components)
                                {
                                    if (component.GetType() == script.GetType())
                                    {
                                        activeSceneGameObject.RemoveComponent(component); //should be added back
                                        typeCache.Add(activeSceneGameObject, component.GetType().ToString());
                                        component.Dispose();
                                        break;
                                    }
                                }
                            }
                        }
                        
                        
                        compiler.UnLoad();
                        compiler.Compile(projectDir);
                        compiler.Load();
                        
                        compiler.types.ForEach(script =>
                        {
                            foreach (var typeCacheKey in typeCache.Keys)
                            {
                                 
                                if (typeCache[typeCacheKey].Equals(script.ToString()))
                                {
                                    Type type = script;
                                    Component o = Activator.CreateInstance(type) as Component;
                                    o.Init();
                                    typeCacheKey.AddComponent(o);

                                }
                                else
                                {
                                    Console.WriteLine(script.ToString() + ": and other not same :" + typeCache[typeCacheKey]);
                                }
                            }
                        });
                        typeCache.Clear();
                        //Console.WriteLine("Scripts are compiled!");
                        Core.Window._log.Info("Scripts are compiled!");
                        
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }  
                    state = EditorState.Idle;
                }).Start();

                
            }
        }

        public static void DrawEditor()
        {
            
            DockSpace.Draw();
            Toolbar.Draw();
            Scene.Draw();
            
            Hierarchy.Draw();
            Inspector.Draw();
            
            ConsoleWindow.Draw();

            AssetsWindow.Draw();

            Menubar.Draw();
        }
        

        [InspectorAttribute] public static int a = 5;



    }
}