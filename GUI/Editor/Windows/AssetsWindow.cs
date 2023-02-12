using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using ImGuiNET;
using OpenTK.Mathematics;

namespace ArchEngine.GUI.Editor.Windows
{
    public class AssetsWindow
    {
        private static string _selectedDirectory;

        private static string[] _filesAndDirectories;
        private static int _selectedFileOrDirectoryIndex = -1;
        private static Stack<string> _directoryStack = new Stack<string>();
        private static string selected = "";
        private static FileInfo fileInfo;
        
        
        private static string[] folders;
        private static string[] files;
        public AssetsWindow(string path)
        {
            _selectedDirectory = path;
            UpdateFolder();
        }

        public static void UpdateFolder(object sender, EventArgs e)
        {
            UpdateFolder();
        }

        public static void UpdateFolder()
        {
            _filesAndDirectories = Directory.GetFileSystemEntries(_selectedDirectory);
            Array.Sort(_filesAndDirectories); //sort the entries
            
            folders = _filesAndDirectories.Where(x => Directory.Exists(x)).ToArray();
            files = _filesAndDirectories.Where(x => !Directory.Exists(x)).ToArray();
            _selectedFileOrDirectoryIndex = -1;
            
            Console.WriteLine("Folder updated");
        }
        
        private static bool isFolder = false;
        private static bool _isRenaming = false;
        private static bool _isDeleting = false;
        private static string _renameInput = "";
        private static bool _isCreatingFile = false;
        private static bool _isCreatingFolder = false;
        
        public static void Draw()
        {
            ImGui.SetNextWindowPos(new Vector2(255,200), ImGuiCond.FirstUseEver);
            ImGui.SetNextWindowSize(new Vector2(300, 300), ImGuiCond.FirstUseEver);
            ImGui.Begin("Assets");
            
            
            ImGui.BeginChild("Files", new Vector2( ImGui.GetContentRegionAvail().X ,  ImGui.GetContentRegionAvail().Y - 70), false);

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
                if (_isCreatingFile || _isCreatingFolder)
                {
                    ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar;
                    ImGui.BeginChild("##InputTextHeight", new Vector2(0, 13), false, flags);
                    ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(0,0));
                      
                    if (ImGui.ImageButtonEx(ImGui.GetID( "assetsWindowButtonCreate"), Icons.Texture, new Vector2(12, 12), Icons.GetUV0FromID(_isCreatingFolder ? 216 : 240),
                            Icons.GetUV1FromID(_isCreatingFolder ? 216 : 240),
                            Vector2.Zero, Vector4.Zero, Vector4.One))
                    {}
                    ImGui.SameLine();
                    
                    ImGui.BeginChild("##InputTextHeight2", new Vector2(ImGui.GetContentRegionAvail().X - 100, 0), false, flags);
                            
                    ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                    ImGui.InputText("##RenameInput", ref _renameInput, 256,
                        ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.AutoSelectAll);

                    ImGui.EndChild();
                    ImGui.SameLine();
                    ImGui.BeginChild("##InputTextHeight3", new Vector2( ImGui.GetContentRegionAvail().X, 0), false, flags);

                    if (ImGui.Button("Cancel"))
                    {
                        _isCreatingFile = false;
                        _isCreatingFolder = false;
                        _renameInput = "";
                    }
                    ImGui.SameLine();
                    if (ImGui.Button("Create"))
                    {
                        if (_isCreatingFile)
                        {
                            
                            CreateFile(_selectedDirectory, _renameInput);
                        }
                        else
                        {
                            CreateDirectory(_selectedDirectory, _renameInput);
                        }
                        _isCreatingFile = false;
                        _isCreatingFolder = false;
                    }
                    ImGui.EndChild();
                    ImGui.PopStyleVar();
                    ImGui.EndChild();
                    
                }
                
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
                    
                        
                        if (_isRenaming && _selectedFileOrDirectoryIndex == i)
                        {
                            ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar;
                            ImGui.BeginChild("##InputTextHeight", new Vector2(0, 13), false, flags);
                            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(0,0));
                          
                            ImGui.BeginChild("##InputTextHeight2", new Vector2(ImGui.GetContentRegionAvail().X - 100, 0), false, flags);
                            
                            ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                            ImGui.InputText("##RenameInput", ref _renameInput, 256,
                                ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.AutoSelectAll);

                            ImGui.EndChild();
                            ImGui.SameLine();
                            ImGui.BeginChild("##InputTextHeight3", new Vector2( ImGui.GetContentRegionAvail().X, 0), false, flags);

                            if (ImGui.Button("Cancel"))
                            {
                                _isRenaming = false;
                                _renameInput = "";
                            }
                            ImGui.SameLine();
                            if (ImGui.Button("Rename"))
                            {
                                
                                Rename(selected, _renameInput);
                                //_entries[i] = _renameInput;
                                _isRenaming = false;
                                _renameInput = "";
                                _selectedFileOrDirectoryIndex = -1;
                                
                            }
                            ImGui.EndChild();
                            ImGui.PopStyleVar();
                            ImGui.EndChild();
                        }
                        else if (_isDeleting && _selectedFileOrDirectoryIndex == i)
                        {
                            ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar;
                            ImGui.BeginChild("##InputTextHeight", new Vector2(0, 13), false, flags);
                            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(0,0));
                          
                            ImGui.BeginChild("##InputTextHeight2", new Vector2(ImGui.GetContentRegionAvail().X - 100, 0), false, flags);
                            ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                            ImGui.TextColored(new Vector4(1,0,0,1), name);
                            ImGui.EndChild();
                            ImGui.SameLine();
                            ImGui.BeginChild("##InputTextHeight3", new Vector2( ImGui.GetContentRegionAvail().X, 0), false, flags);

                            if (ImGui.Button("Cancel"))
                            {
                                _isDeleting = false;
                            }
                            ImGui.SameLine();
                            if (ImGui.Button("Delete"))
                            {
                                DeleteFile(selected);
                                //_entries[i] = _renameInput;
                                _isDeleting = false;
                                _selectedFileOrDirectoryIndex = -1;
                                
                            }
                            ImGui.EndChild();
                            ImGui.PopStyleVar();
                            ImGui.EndChild();
                        }
                        else if (ImGui.Selectable(name, i == _selectedFileOrDirectoryIndex, ImGuiSelectableFlags.AllowDoubleClick))
                        {
                            if (ImGui.IsMouseDoubleClicked(0))
                            {
                                _directoryStack.Push(_selectedDirectory);
                                _selectedDirectory = folders[i];
                                UpdateFolder();
                                _selectedFileOrDirectoryIndex = -1;
                            }
                            else
                            {
                                _selectedFileOrDirectoryIndex = i;
                                selected = folders[i];
                                fileInfo = new FileInfo(selected);
                            }
                            isFolder = true;
                            _isRenaming = false;
                            _isDeleting = false;
                            _isCreatingFile = false;
                            _isCreatingFolder = false;
                        }
                        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                        {
                            _selectedFileOrDirectoryIndex = i;
                            selected = folders[i];
                            fileInfo = new FileInfo(selected);
                            ImGui.OpenPopup("ContextMenu");
                            isFolder = true;
                            _isRenaming = false;
                            _isDeleting = false;
                            _isCreatingFile = false;
                            _isCreatingFolder = false;
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

                        if (_isRenaming && _selectedFileOrDirectoryIndex == i)
                        {
                            ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar;
                            ImGui.BeginChild("##InputTextHeight", new Vector2(0, 13), false, flags);
                            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(0,0));
                          
                            ImGui.BeginChild("##InputTextHeight2", new Vector2(ImGui.GetContentRegionAvail().X - 100, 0), false, flags);
                            ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                            ImGui.InputText("##RenameInput", ref _renameInput, 256,
                                ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.AutoSelectAll);
                            
                            
                            ImGui.EndChild();
                            
                            ImGui.SameLine();
                            ImGui.BeginChild("##InputTextHeight3", new Vector2( ImGui.GetContentRegionAvail().X, 0), false, flags);
                            
                            if (ImGui.Button("Cancel##asd"))
                            {
                                _isRenaming = false;
                                _renameInput = "";
                            }
                            ImGui.SameLine();
                            if (ImGui.Button("Rename##asdw"))
                            {
                                Rename(selected, _renameInput);
                                //_entries[i] = _renameInput;
                                _isRenaming = false;
                                _renameInput = "";
                                _selectedFileOrDirectoryIndex = -1;
                                
                            }
                            ImGui.EndChild();
                            ImGui.PopStyleVar();
                            ImGui.EndChild();
                        }
                        else if (_isDeleting && _selectedFileOrDirectoryIndex == i)
                        {
                            ImGuiWindowFlags flags = ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize | ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoScrollbar;
                            ImGui.BeginChild("##InputTextHeight", new Vector2(0, 13), false, flags);
                            ImGui.PushStyleVar(ImGuiStyleVar.FramePadding, new Vector2(0,0));
                          
                            ImGui.BeginChild("##InputTextHeight2", new Vector2(ImGui.GetContentRegionAvail().X - 100, 0), false, flags);
                            ImGui.SetNextItemWidth(ImGui.GetContentRegionAvail().X);
                            ImGui.TextColored(new Vector4(1,0,0,1), name);
                            ImGui.EndChild();
                            ImGui.SameLine();
                            ImGui.BeginChild("##InputTextHeight3", new Vector2( ImGui.GetContentRegionAvail().X, 0), false, flags);

                            if (ImGui.Button("Cancel"))
                            {
                                _isDeleting = false;
                            }
                            ImGui.SameLine();
                            if (ImGui.Button("Delete"))
                            {
                                
                                DeleteFile(selected);
                                //_entries[i] = _renameInput;
                                _isDeleting = false;
                                _selectedFileOrDirectoryIndex = -1;
                                
                            }
                            ImGui.EndChild();
                            ImGui.PopStyleVar();
                            ImGui.EndChild();
                        }
                        else if (ImGui.Selectable(name, i == _selectedFileOrDirectoryIndex, ImGuiSelectableFlags.AllowDoubleClick))
                        {
                            if (ImGui.IsMouseDoubleClicked(0))
                            {
                                OpenFile(selected);
                            }
                            else
                            {
                                _selectedFileOrDirectoryIndex = i;
                                selected = files[i - folders.Length];
                                fileInfo = new FileInfo(selected);
                            }
                            
                            isFolder = false;
                            _isRenaming = false;
                            _isDeleting = false;
                            _isCreatingFile = false;
                            _isCreatingFolder = false;
                        }
                        if (ImGui.IsItemClicked(ImGuiMouseButton.Right))
                        {
                            _selectedFileOrDirectoryIndex = i;
                            selected = files[i - folders.Length];
                            fileInfo = new FileInfo(selected);
                            isFolder = false;
                            _isRenaming = false;
                            _isDeleting = false;
                            _isCreatingFile = false;
                            _isCreatingFolder = false;
                            ImGui.OpenPopup("ContextMenu");

                        }
                    }

                }
            }

            if (ImGui.BeginPopup("ContextMenu"))
            {
                if (ImGui.MenuItem("Open"))
                {
                    OpenFile(selected);
                }
                if (ImGui.MenuItem("Rename"))
                {
                    _isRenaming = true;
                    _renameInput = Path.GetFileName(selected);
                }
                if (ImGui.MenuItem("Delete"))
                {
                    _isDeleting = true;

                }

                ImGui.EndPopup();
            }
            
            ImGui.EndChild();
            if (ImGui.IsItemClicked(0))
            {
                _isRenaming = false;
                _isDeleting = false;
                _isCreatingFile = false;
                _isCreatingFolder = false;
                _selectedFileOrDirectoryIndex = -1;
            }
            
            if(ImGui.IsItemClicked(ImGuiMouseButton.Right) && !ImGui.IsAnyItemHovered())
            {
                ImGui.OpenPopup("NewMenu");
                _isRenaming = false;
                _isDeleting = false;
                _isCreatingFile = false;
                _isCreatingFolder = false;
                _selectedFileOrDirectoryIndex = -1;
            }
            
            if (ImGui.BeginPopup("NewMenu"))
            {
                if (ImGui.MenuItem("Create Folder"))
                {
                    _isCreatingFolder = true;
                    _selectedFileOrDirectoryIndex = -1;
                    _renameInput = "New Folder";
                }
                if (ImGui.MenuItem("Create File"))
                {
                    _isCreatingFile = true;
                    _selectedFileOrDirectoryIndex = -1;
                    _renameInput = "New File.cs";
                }
                ImGui.EndPopup();
            }
            ImGui.PushStyleColor(ImGuiCol.ChildBg, new Vector4(0.102f, 0.090f, 0.122f, 1.0f));
            
            ImGui.BeginChild("FileInfo", new Vector2(ImGui.GetContentRegionAvail().X ,  ImGui.GetContentRegionAvail().Y), false);
            if (_selectedFileOrDirectoryIndex != -1)
            {
                ImGui.Text(selected);

                string type = "Folder";
                
                long size = 0;
                string resultSize = "-";

                if (!isFolder)
                {
                    size = fileInfo.Length;
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
                    type = fileInfo.Extension;
                    if (type == "") type = "Raw";
                }
                
                DateTime creationTime = fileInfo.CreationTime;
                ImGui.Text("Size: " + resultSize);
                ImGui.Text("Type: " + type);
                ImGui.Text("Created at: " + creationTime);
                
            }
            ImGui.EndChild();
            if (ImGui.IsItemClicked(0))
            {
                _isRenaming = false;
                _isDeleting = false;
                _isCreatingFile = false;
                _isCreatingFolder = false;
                _selectedFileOrDirectoryIndex = -1;
            }
            if(ImGui.IsItemClicked(ImGuiMouseButton.Right) && !ImGui.IsAnyItemHovered())
            {
                ImGui.OpenPopup("NewMenu");
                _isRenaming = false;
                _isDeleting = false;
                _isCreatingFile = false;
                _isCreatingFolder = false;
                _selectedFileOrDirectoryIndex = -1;
            }
            ImGui.PopStyleColor();
            ImGui.End();
            
        }

        private static void OpenFile(string file)
        {
            if (File.Exists(file))
            {
                ProcessStartInfo psi = new ProcessStartInfo(selected);
                psi.Verb = "open";
                psi.UseShellExecute = true;
                Process.Start(psi);
                Console.WriteLine("File opened!");
            }
        }
        private static void DeleteFile(string file)
        {
            if (File.Exists(file))
            {
                File.Delete(file);
                Console.WriteLine("File deleted!");
            }
            else if (Directory.Exists(file))
            {
                Directory.Delete(file, true);
                Console.WriteLine("Directory deleted!");
            }
            UpdateFolder();
        }
        private static void CreateDirectory(string path, string name)
        {
            if (Directory.Exists(path))
            {
                string newPath = Path.Combine(path, name);
                if (!Directory.Exists(newPath))
                {
                    Directory.CreateDirectory(newPath);
                    Console.WriteLine("Directory created at: " + newPath);
                }
                else
                {
                    Console.WriteLine("Directory already exists!");
                }
            }
            UpdateFolder();
        }

        private static void CreateFile(string path, string name)
        {
            if (Directory.Exists(path))
            {
                string newPath = Path.Combine(path, name);
                if (!File.Exists(newPath))
                {
                    File.Create(newPath);
                    Console.WriteLine("File created at: " + newPath);
                }
                else
                {
                    Console.WriteLine("File already exists!");
                }
            }
            UpdateFolder();
        }

        private static void Rename(string file, string nameNew)
        {
            if (File.Exists(file))

            {
                string path = Path.GetDirectoryName(file);
                string destPath = Path.Combine(path, nameNew);

                if (!File.Exists(destPath))
                {
                    File.Move(file, destPath);
                    Console.WriteLine("File moved to: " + destPath);
                }
                else
                {
                    Console.WriteLine("A file already exists with same name!");
                }
                
                
            }
            else if (Directory.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                string newPath = Path.Combine(path, nameNew);
                
                
                if (!Directory.Exists(newPath))
                {
                    Directory.Move(file, newPath);
                    Console.WriteLine("Directory moved to: " + newPath);
                }
                else
                {
                    Console.WriteLine("A folder already exists with same name!");
                }
            }
            UpdateFolder();
        }
    }
}