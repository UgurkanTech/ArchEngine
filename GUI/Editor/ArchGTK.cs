using System;
using System.IO;

namespace ArchEngine.GUI.Editor
{
    using Gdk;
    using Gtk;
    using Window = Gtk.Window;

    public class ArchGTK
    {

        private static Pixbuf icon;
        
        public ArchGTK(Stream iconPath)
        {
            Application.Init();
            Environment.SetEnvironmentVariable("GTK_THEME", "Adwaita:dark");
            icon = new Gdk.Pixbuf(iconPath);
        }
    
    

        public void ShowMessage(string message, string title, Window? parent)
        {
            MessageDialog md = new MessageDialog(parent, DialogFlags.Modal, MessageType.Info, ButtonsType.Ok, message);
            md.Icon = icon;
            md.Title = title;
            md.Run();
            md.Destroy();
        }

        public string SelectFolder()
        {
            FileChooserDialog dialog = new FileChooserDialog("Choose the project directory", null, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);
            dialog.Icon = icon;
            dialog.CreateFolders = true;
            dialog.SetCurrentFolder(Environment.GetFolderPath(Environment.SpecialFolder.Desktop));

            int result = dialog.Run();
            if (result == (int)ResponseType.Accept)
            {
                String path = dialog.Filename;
                dialog.Destroy();
                return path;
            }
            else
            {
                ShowMessage("Cannot start without a project dir!", "Arch Engine" , dialog);
                dialog.Destroy();
                return "";
            }

        }

    }
}