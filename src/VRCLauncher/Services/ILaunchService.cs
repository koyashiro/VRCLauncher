namespace VRCLauncher.Services
{
    public interface ILaunchService
    {
        void LaunchVR(string arguments);
        void LaunchDesktop(string arguments);
    }
}
