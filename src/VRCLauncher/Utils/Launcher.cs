using System.ComponentModel;
using System.Diagnostics;

namespace VRCLauncher.Utils
{
    public static class Launcher
    {
        public static void LaunchVR(string path, string arguments)
        {
            Launch(LaunchMode.VR, path, arguments);
        }

        public static void LaunchDesktop(string path, string arguments)
        {
            Launch(LaunchMode.Desktop, path, arguments);
        }

        private static void Launch(LaunchMode launchMode, string path, string uri)
        {
            var arguments = launchMode switch
            {
                LaunchMode.VR => $"\"{uri}\"",
                LaunchMode.Desktop => $"--no-vr \"{uri}\"",
                _ => throw new InvalidEnumArgumentException(nameof(launchMode), (int)launchMode, typeof(LaunchMode)),
            };

            var processStartInfo = new ProcessStartInfo(path, arguments);
            Process.Start(processStartInfo);
        }
    }

    public enum LaunchMode
    {
        VR,
        Desktop
    }
}
