using System;

namespace VRCLauncher.Wrappers
{
    public class EnvironmentWrapper : IEnvironmentWrapper
    {
        public string NewLine => Environment.NewLine;
        public string GetLocalApplicationDataDirectoryPath()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        }
    }
}
