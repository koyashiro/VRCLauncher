using System.Windows;

namespace VRCLauncher.Models
{
    public class WindowWrapper : IWindowWrapper
    {
        private readonly Window _window;

        public WindowWrapper(Window window)
        {
            _window = window;
        }

        public void Close()
        {
            _window.Close();
        }
    }
}
