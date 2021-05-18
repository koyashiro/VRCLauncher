using System.Diagnostics;

namespace VRCLauncher.Wrappers
{
    public class ProcessWrapper : IProcessWrapper
    {
        public void Start(ProcessStartInfo startInfo)
        {
            Process.Start(startInfo);
        }
    }
}
