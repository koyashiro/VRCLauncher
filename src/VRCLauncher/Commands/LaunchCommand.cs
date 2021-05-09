using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace VRCLauncher.Commands
{
    public class LaunchCommand : ICommand
    {
        private const string VRCHAT_BIN = "VRChat.exe";
        private readonly string _uri;
        private readonly bool _isVrMode;

#pragma warning disable CS8612
        public event EventHandler CanExecuteChanged;
#pragma warning restore CS8612

#pragma warning disable CS8618
        public LaunchCommand(string uri, bool isVrMode)
#pragma warning restore CS8618
        {
            _uri = uri;
            _isVrMode = isVrMode;
        }

        private static string VRChatPath => Path.Join(AppDomain.CurrentDomain.BaseDirectory, VRCHAT_BIN);

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

            if (!File.Exists(VRChatPath))
            {
                MessageBox.Show($"`{VRCHAT_BIN}` is not found");
                return;
            }

            string arguments = _isVrMode ? $"\"_uri\"" : $"--no-vr \"{_uri}\"";

            var processStartInfo = new ProcessStartInfo(VRChatPath, arguments);
            Process.Start(processStartInfo);

            Window.GetWindow(window).Close();
        }
    }
}
