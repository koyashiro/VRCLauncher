using VRCLauncher.Models;

namespace VRCLauncher.Services
{
    public interface IConfigService
    {
        void Initialize();
        bool Exists();
        Config Load();
        void Save(Config config);
    }
}
