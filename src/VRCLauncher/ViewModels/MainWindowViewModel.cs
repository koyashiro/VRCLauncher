using System.ComponentModel;

namespace VRCLauncher.ViewModels
{
    public class MainWindowViewModel
    {
        public MainWindowViewModel(string uri, LaunchParameter launchParameter)
        {
            Uri = uri;
            LaunchParameter = launchParameter;
        }

        public string Uri { get; }
        public LaunchParameter LaunchParameter { get; }
    }
}
