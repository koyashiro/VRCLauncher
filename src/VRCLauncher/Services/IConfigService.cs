using VRCLauncher.Models;

namespace VRCLauncher.Services
{
    public interface IConfigService
    {
        void Initialize();
        string GetConfigDirectoryPath();
        string GetConfigFilePath();
        bool ExistsConfigDirectory();
        bool ExistsConfigFile();
        Config Load();
        void Save(Config config);
    }
}
