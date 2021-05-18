using System.IO;

namespace VRCLauncher.Wrappers
{
    public class DirectoryWrapper : IDirectoryWrapper
    {
        public bool Exists(string? path)
        {
            return Directory.Exists(path);
        }

        public void CreateDirectory(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}
