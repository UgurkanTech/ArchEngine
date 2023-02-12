using System;
using System.IO;

namespace ArchEngine.GUI.Editor
{
    public class FileSystemWatcherClass
    {
        private readonly FileSystemWatcher _watcher;

        public event EventHandler onChangeEvent;
        public event EventHandler onDataChangeEvent;
        
        public FileSystemWatcherClass(string path)
        {
            _watcher = new FileSystemWatcher(path)
            {
                IncludeSubdirectories = true,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite
            };

            _watcher.Created += OnChange;
            _watcher.Deleted += OnChange;
            _watcher.Renamed += OnRename;
            _watcher.Changed += OnChange;
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
        }

        private void OnChange(object sender, FileSystemEventArgs e)
        {
            onChangeEvent?.Invoke(this, e);
        }

        private void OnRename(object sender, RenamedEventArgs e)
        {
            onChangeEvent?.Invoke(this, e);
        }


    }
}