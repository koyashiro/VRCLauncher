using System.Diagnostics;

namespace VRCLauncher.Models
{
    public class ProcessWrapper : IProcessWrapper
    {
        public void Start(ProcessStartInfo startInfo)
        {
            Process.Start(startInfo);
        }
    }
}
