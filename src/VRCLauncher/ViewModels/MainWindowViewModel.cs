using System;
using System.IO;
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

            const string VRCHAT_BIN = "VRChat.exe";
            var vrchatPath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, VRCHAT_BIN);
            LaunchVRCommand = new LaunchCommand(vrchatPath, uri, LaunchMode.VR);
            LaunchDesktopCommand = new LaunchCommand(vrchatPath, uri, LaunchMode.Desktop);
        }

        public string Uri { get; }
        public LaunchParameter LaunchParameter { get; }

        public ICommand LaunchVRCommand { get; }
        public ICommand LaunchDesktopCommand { get; }
    }
}
