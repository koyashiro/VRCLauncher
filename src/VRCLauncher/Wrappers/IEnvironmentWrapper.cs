namespace VRCLauncher.Wrappers
{
    public interface IEnvironmentWrapper
    {
        string NewLine { get; }
        string GetLocalApplicationDataDirectoryPath();
    }
}
