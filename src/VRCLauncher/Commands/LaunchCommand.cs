using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace VRCLauncher.Commands
{
    public class LaunchCommand : ICommand
    {
        private readonly string _vrchatPath;
        private readonly string _uri;
        private readonly LaunchMode _launchMode;

#pragma warning disable CS8612
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS8612

#pragma warning disable CS8618
        public LaunchCommand(string vrchatPath, string uri, LaunchMode launchMode)
#pragma warning restore CS8618
        {
            _vrchatPath = vrchatPath;
            _uri = uri;
            _launchMode = launchMode;
        }

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (parameter is not Window window)
            {
                throw new ArgumentException($"{nameof(parameter)} is not Window", nameof(parameter));
            }

            if (!File.Exists(_vrchatPath))
            {
                MessageBox.Show($"`{_vrchatPath}` is not found");
                return;
            }

            string arguments = _launchMode switch
            {
                LaunchMode.VR => $"\"_uri\"",
                LaunchMode.Desktop => $"--no-vr \"{_uri}\"",
                _ => throw new InvalidEnumArgumentException(nameof(_launchMode), (int)_launchMode, typeof(LaunchMode)),
            };

            var processStartInfo = new ProcessStartInfo(_vrchatPath, arguments);
            Process.Start(processStartInfo);

            Window.GetWindow(window).Close();
        }
    }

    public enum LaunchMode
    {
        VR,
        Desktop
    }
}
