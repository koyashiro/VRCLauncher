using VRCLauncher.Models;

namespace VRCLauncher.Services
{
    public interface IConfigService
    {
        bool ExistConfigFile();
        Config Load();
        void Save(Config config);
    }
}
