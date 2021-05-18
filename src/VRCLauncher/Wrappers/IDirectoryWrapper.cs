namespace VRCLauncher.Wrappers
{
    public interface IDirectoryWrapper
    {
        bool Exists(string? path);
        void CreateDirectory(string path);
    }
}
