using System;
using System.IO;
using System.Text.Json;
using VRCLauncher.Models;
using VRCLauncher.Wrappers;

namespace VRCLauncher.Services
{
    public class ConfigService : IConfigService
    {
        private const string DEFAULT_VRCHAT_PATH = @"C:\Program Files (x86)\Steam\steamapps\common\VRChat\VRChat.exe";
        private static readonly string CONFIG_FILE_PATH = $"{Path.Join(AppDomain.CurrentDomain.BaseDirectory, "VRCLauncher.json")}";

        private readonly IFileWrapper _fileWrapper;

        public ConfigService(IFileWrapper fileWrapper)
        {
            _fileWrapper = fileWrapper;
        }

        public bool ExistConfigFile()
        {
            return _fileWrapper.Exists(CONFIG_FILE_PATH);
        }

        public Config Load()
        {
            if (!ExistConfigFile())
            {
                Initialize();
            }

            var configJson = _fileWrapper.ReadAllText(CONFIG_FILE_PATH);
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
            try
            {
                var option = new JsonSerializerOptions
                {
                    WriteIndented = true,

                };
                var configJson = JsonSerializer.Serialize(config, option);
                _fileWrapper.WriteAllText(CONFIG_FILE_PATH, configJson);
            }
            catch (PathTooLongException)
            {
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }
        }

        private void Initialize()
        {
            var config = new Config
            {
                VRChatPath = DEFAULT_VRCHAT_PATH
            };
            Save(config);
        }
    }
}
