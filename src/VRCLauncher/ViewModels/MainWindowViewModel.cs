using System.Windows.Input;
using VRCLauncher.Commands;

namespace VRCLauncher.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(string uri, LaunchParameter launchParameter)
        {
            Uri = uri;
            LaunchParameter = launchParameter;
            LaunchVRCommand = new LaunchCommand(uri, true);
            LaunchDesktopCommand = new LaunchCommand(uri, false);
        }

        public string Uri { get; }
        public LaunchParameter LaunchParameter { get; }

        public ICommand LaunchVRCommand { get; }
        public ICommand LaunchDesktopCommand { get; }
    }
}
