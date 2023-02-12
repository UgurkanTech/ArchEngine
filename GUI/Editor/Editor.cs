using System;
using System.Threading;
using ArchEngine.Core;
using ArchEngine.Core.ECS;
using ArchEngine.GUI.Editor.Windows;
using ImGuiNET;
using OpenTK.Mathematics;
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
            Playing
        }

        public static EditorState state = EditorState.NeedsCompiling;

        private static bool filesChanged = false;
        public static bool windowFocussedNew = false;
        public static RuntimeCompiler<Script> compiler;

        public static string projectDir = @"C:\Users\saw\Desktop\Scripts";
        public Editor()
        {
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
                state = EditorState.NeedsCompiling;
            }

            if (state == EditorState.NeedsCompiling)
            {
                state = EditorState.Compiling;
                new Thread(() =>
                {
                    try
                    {
                        compiler.UnLoad();
                        compiler.Compile(projectDir);
                        compiler.Load();
                        Console.WriteLine("Scripts are compiled!");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }  
                }).Start();

                state = EditorState.Idle;
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
            
            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("File"))
            {
               
                ImGui.Image(Icons.Texture, new Vector2(20, 20),
                    Icons.GetUV0FromID(249), Icons.GetUV1FromID(249));
                ImGui.SameLine();

                if (ImGui.MenuItem("Save Scene"))
                {
                    AssetManager.SaveScene();
                    
                }
                ImGui.Image(Icons.Texture, new Vector2(20, 20),
                    Icons.GetUV0FromID(37), Icons.GetUV1FromID(37));
                ImGui.SameLine();
                if (ImGui.MenuItem("Load Scene"))
                {
                    AssetManager.LoadScene();
                }
                ImGui.Image(Icons.Texture, new Vector2(20, 20),
                    Icons.GetUV0FromID(102), Icons.GetUV1FromID(102));
                ImGui.SameLine();
                if (ImGui.MenuItem("Exit"))
                {
                    Window.instance.Close();
                }
                
                ImGui.EndMenu();
            }
            
            ImGui.EndMainMenuBar();
        }
        

        [InspectorAttribute] public static int a = 5;



    }
}