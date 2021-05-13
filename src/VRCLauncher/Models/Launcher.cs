using System.ComponentModel;
using System.Diagnostics;

namespace VRCLauncher.Models
{
    public class Launcher : ILauncher
    {
        private readonly IConfigService _configService;

        public Launcher(IConfigService configService)
        {
            _configService = configService;
        }

        public void LaunchVR(string arguments)
        {
            Launch(LaunchMode.VR, arguments);
        }

        public void LaunchDesktop(string arguments)
        {
            Launch(LaunchMode.Desktop, arguments);
        }

        private void Launch(LaunchMode launchMode, string arguments)
        {
            arguments = launchMode switch
            {
                LaunchMode.VR => $"\"{arguments}\"",
                LaunchMode.Desktop => $"--no-vr \"{arguments}\"",
                _ => throw new InvalidEnumArgumentException(nameof(launchMode), (int)launchMode, typeof(LaunchMode)),
            };

            var config = _configService.Load();
            var processStartInfo = new ProcessStartInfo(config.VRChatPath, arguments);
            Process.Start(processStartInfo);
        }

        private enum LaunchMode
        {
            VR,
            Desktop
        }
    }
}
