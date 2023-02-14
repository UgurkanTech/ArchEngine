using System;
using Gtk;

namespace ArchEngine.GUI.Editor
{
    public class ProjectSelector
    {
        public static String SelectFolder()
        {
            Application.Init();

            FileChooserDialog dialog = new FileChooserDialog("Choose a folder", null, FileChooserAction.SelectFolder, "Cancel", ResponseType.Cancel, "Open", ResponseType.Accept);

            if (dialog.Run() == (int)ResponseType.Accept)
            {
                Console.WriteLine("Selected folder: " + dialog.Filename);

            dialog.Destroy();
            Application.Run();
            return "";
        }
    }
}