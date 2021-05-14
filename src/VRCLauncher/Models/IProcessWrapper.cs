using System.Diagnostics;

namespace VRCLauncher.Models
{
    public interface IProcessWrapper
    {
        void Start(ProcessStartInfo startInfo);
    }
}
