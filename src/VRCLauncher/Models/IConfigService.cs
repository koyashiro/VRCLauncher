namespace VRCLauncher.Models
{
    public interface IConfigService
    {
        bool ExistConfigFile();
        Config Load();
        void Save(Config config);
    }
}
