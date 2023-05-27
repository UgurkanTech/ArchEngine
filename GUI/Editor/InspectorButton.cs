using System;

namespace ArchEngine.GUI.Editor
{
    public class InspectorButton    
    {
        public event EventHandler ButtonClicked;

        public void Click()
        {
            ButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}