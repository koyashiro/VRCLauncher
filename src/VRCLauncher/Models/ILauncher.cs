namespace VRCLauncher.Models
{
    public interface ILauncher
    {
        void LaunchVR(string arguments);
        void LaunchDesktop(string arguments);
    }
}
