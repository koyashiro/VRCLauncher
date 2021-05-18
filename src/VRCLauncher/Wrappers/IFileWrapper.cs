namespace VRCLauncher.Wrappers
{
    public interface IFileWrapper
    {
        bool Exists(string? path);
        string ReadAllText(string path);
        void WriteAllText(string path, string? contents);
    }
}
