using System;
using ArchEngine.Core;
using ArchEngine.Core.Audio;
using ImGuiNET;

namespace ArchEngine.GUI.Editor.Windows
{
    public class Menubar
    {
        public static void Draw()
        {
            
            ImGui.BeginMainMenuBar();
            if (ImGui.BeginMenu("File"))
            {
                
                if (ImGui.MenuItem("Save Scene"))
                {
                    AssetManager.SaveScene();
                    
                }
                
                if (ImGui.MenuItem("Load Scene"))
                {
                    AssetManager.LoadScene();
                }
                
                if (ImGui.MenuItem("Save Project"))
                {
                    AssetManager.SaveScene();
                }
                if (ImGui.MenuItem("Load Project"))
                {
                    AssetManager.LoadScene();
                }
                if (ImGui.MenuItem("Build"))
                {
                    Console.WriteLine("This feature will be supported soon..");
                }
                if (ImGui.MenuItem("Exit"))
                {
                    Window.instance.Close();
                }
                
                ImGui.EndMenu();
            }

            if (ImGui.BeginMenu("Edit"))
            {
                if (ImGui.MenuItem("Play"))
                {
                    Console.WriteLine("Scene starting!");
                    Window.started = true;
                }
                if (ImGui.MenuItem("Pause"))
                {
                    Window.started = false;
                    AudioEngine.engines.ForEach(engine => { engine.SetLooping(false);});
                    Console.WriteLine("Scene stopped!");
                }
                if (ImGui.MenuItem("Project Settings"))
                {
                    Console.WriteLine("This feature will be supported soon..");
                }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Assets"))
            {
                if (ImGui.MenuItem("Create Script"))
                {
                    Console.WriteLine("This feature will be supported soon..");
                }
                if (ImGui.MenuItem("Show in Explorer"))
                {
                    Console.WriteLine("This feature will be supported soon..");
                }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Window"))
            {
                if (ImGui.MenuItem("Reset Layout"))
                {
                    Console.WriteLine("This feature will be supported soon..");
                }
                ImGui.EndMenu();
            }
            if (ImGui.BeginMenu("Help"))
            {
                if (ImGui.MenuItem("About Arch Engine"))
                {
                    Console.WriteLine("Arch Engine is created by -Uğurkan Hoşgör");
                }
                ImGui.EndMenu();
            }
            
            ImGui.EndMainMenuBar();
        }
    }
}