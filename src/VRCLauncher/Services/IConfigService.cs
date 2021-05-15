using VRCLauncher.Models;

namespace VRCLauncher.Services
{
    public interface IConfigService
    {
        bool Exists();
        Config Load();
        void Save(Config config);
    }
}
