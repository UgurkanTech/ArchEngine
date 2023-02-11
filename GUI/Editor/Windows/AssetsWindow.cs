using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ImGuiNET;
using OpenTK.Mathematics;

namespace ArchEngine.GUI.Editor.Windows
{
    public class AssetsWindow
    {
        private static string _selectedDirectory = @"C:\Riot Games\Riot Client";

        private static string[] _filesAndDirectories;
        private static int _selectedFileOrDirectoryIndex = -1;
        private static Stack<string> _directoryStack = new Stack<string>();
        private static string selected = "";
        private static FileInfo fileInfo;
        
        
        private static string[] folders;
        private static string[] files;
        public AssetsWindow()
        {
            UpdateFolder();
        }

        public static void UpdateFolder()
        {
            _filesAndDirectories = Directory.GetFileSystemEntries(_selectedDirectory);
            Array.Sort(_filesAndDirectories); //sort the entries
            
            folders = _filesAndDirectories.Where(x => Directory.Exists(x)).ToArray();
            files = _filesAndDirectories.Where(x => !Directory.Exists(x)).ToArray();

        }
        
        public static void Draw()
        {
            ImGui.SetNextWindowPos(new Vector2(255,200), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new Vector2(300, 300), ImGuiCond.FirstUseEver);
            ImGui.Begin("Assets");
            
            
            ImGui.BeginChild("Files", new Vector2( ImGui.GetContentRegionAvail().X , 0.7f * ImGui.GetContentRegionAvail().Y), false);

            ImGui.PushItemFlag(ImGuiItemFlags.Disabled, _directoryStack.Count == 0);

            if (ImGui.Button("Back"))
            {
                _selectedDirectory = _directoryStack.Pop();
                UpdateFolder();
                _selectedFileOrDirectoryIndex = -1;
            }
            
            ImGui.PopItemFlag();
 
            
            ImGui.SameLine();
            ImGui.Text(_selectedDirectory);

            ImGui.Separator();

            if (_filesAndDirectories != null)
            {
                for (int i = 0; i < _filesAndDirectories.Length; i++)
                {
                    string name;
                    if (folders.Length > i)
                    {
                        name =  Path.GetFileName(folders[i]);
                        if (ImGui.ImageButtonEx(ImGui.GetID(i + "assetsWindowButton"), Icons.Texture, new Vector2(12, 12), Icons.GetUV0FromID(216),
                                Icons.GetUV1FromID(216),
                                Vector2.Zero, Vector4.Zero, Vector4.One))
                        {}
                        ImGui.SameLine();
                    
                        if (ImGui.Selectable(name, i == _selectedFileOrDirectoryIndex))
                        {
                            _directoryStack.Push(_selectedDirectory);
                            _selectedDirectory = folders[i];
                            UpdateFolder();
                            _selectedFileOrDirectoryIndex = -1;
                            
                        }
                    }
                    else
                    {
                        name =  Path.GetFileName(files[i - folders.Length]);
                        if (ImGui.ImageButtonEx(ImGui.GetID(i + "assetsWindowButton"), Icons.Texture, new Vector2(12, 12), Icons.GetUV0FromID(240),
                                Icons.GetUV1FromID(240),
                                Vector2.Zero, Vector4.Zero, Vector4.One))
                        {}
                        ImGui.SameLine();
                    
                        if (ImGui.Selectable(name, i == _selectedFileOrDirectoryIndex))
                        {
                            _selectedFileOrDirectoryIndex = i;
                            selected = files[i - folders.Length];
                            fileInfo = new FileInfo(selected);
                            
                        }
                    }

                }
            }
            ImGui.EndChild();
            ImGui.PushStyleColor(ImGuiCol.ChildBg, new Vector4(0.102f, 0.090f, 0.122f, 1.0f));
            
            ImGui.BeginChild("FileInfo", new Vector2(ImGui.GetContentRegionAvail().X ,  ImGui.GetContentRegionAvail().Y), false);
            if (_selectedFileOrDirectoryIndex != -1)
            {
                ImGui.Text(selected);
                
                long size = fileInfo.Length;
                string resultSize;

                if (size >= 1073741824)
                {
                    resultSize = string.Format("{0:0.00} GB", (float)size / 1073741824);
                }
                else if (size >= 1048576)
                {
                    resultSize = string.Format("{0:0.00} MB", (float)size / 1048576);
                }
                else if (size >= 1024)
                {
                    resultSize = string.Format("{0:0.00} KB", (float)size / 1024);
                }
                else
                {
                    resultSize = string.Format("{0} B", size);
                }
                string type = fileInfo.Extension;
                DateTime creationTime = fileInfo.CreationTime;
                ImGui.Text("Size: " + resultSize);
                ImGui.Text("Type: " + type);
                ImGui.Text("Created at: " + creationTime);
                
            }
            
            ImGui.EndChild();
            ImGui.PopStyleColor();
            ImGui.End();

        }
    }
}