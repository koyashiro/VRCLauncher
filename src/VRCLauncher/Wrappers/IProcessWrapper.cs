using System.Diagnostics;

namespace VRCLauncher.Wrappers
{
    public interface IProcessWrapper
    {
        void Start(ProcessStartInfo startInfo);
    }
}
