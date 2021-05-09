using System.ComponentModel;
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
            LaunchCommand = new LaunchCommand(uri);
        }

        public string Uri { get; }
        public LaunchParameter LaunchParameter { get; }

        public ICommand LaunchCommand { get; }
    }
}
