using System;
using System.IO;
using System.Text.Json;
using VRCLauncher.Models;
using VRCLauncher.Wrappers;

namespace VRCLauncher.Services
{
    public class ConfigService : IConfigService
    {
        private const string CONFIG_DIRECTORY_BASE_NAME = "VRCLauncher";
        private const string CONFIG_FILE_NAME = "config.json";

        private const string DEFAULT_VRCHAT_PATH = @"C:\Program Files (x86)\Steam\steamapps\common\VRChat\VRChat.exe";

        private readonly IDirectoryWrapper _directoryWrapper;
        private readonly IFileWrapper _fileWrapper;
        private readonly IEnvironmentWrapper _environmentWrapper;

        public ConfigService(IDirectoryWrapper directoryWrapper, IFileWrapper fileWrapper, IEnvironmentWrapper environmentWrapper)
        {
            _directoryWrapper = directoryWrapper;
            _fileWrapper = fileWrapper;
            _environmentWrapper = environmentWrapper;
        }

        public void Initialize()
        {
            if (ExistsConfigFile())
            {
                return;
            }

            var config = new Config
            {
                VRChatPath = DEFAULT_VRCHAT_PATH
            };

            Save(config);
        }

        public string GetConfigDirectoryPath()
        {
            return Path.Join(_environmentWrapper.GetLocalApplicationDataDirectoryPath(), CONFIG_DIRECTORY_BASE_NAME);
        }

        public string GetConfigFilePath()
        {
            return Path.Join(GetConfigDirectoryPath(), CONFIG_FILE_NAME);
        }

        public bool ExistsConfigFile()
        {
            return _fileWrapper.Exists(GetConfigFilePath());
        }

        public bool ExistsConfigDirectory()
        {
            return _directoryWrapper.Exists(GetConfigDirectoryPath());
        }

        public Config Load()
        {
            if (!ExistsConfigFile())
            {
                Initialize();
            }

            var configJson = _fileWrapper.ReadAllText(GetConfigFilePath());
            try
            {
                var config = JsonSerializer.Deserialize<Config>(configJson);
                if (config is null)
                {
                    return new Config();
                }
                return config;
            }
            catch (PathTooLongException)
            {
                return new Config();
            }
            catch (IOException)
            {
                return new Config();
            }
            catch (UnauthorizedAccessException)
            {
                return new Config();
            }
            catch (JsonException)
            {
                return new Config();
            }
        }

        public void Save(Config config)
        {
            if (!ExistsConfigDirectory())
            {
                _directoryWrapper.CreateDirectory(GetConfigDirectoryPath());
            }

            var option = new JsonSerializerOptions
            {
                WriteIndented = true,

            };
            var configJson = JsonSerializer.Serialize(config, option);

            _fileWrapper.WriteAllText(GetConfigFilePath(), configJson);
        }
    }
}
