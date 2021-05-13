using System;
using System.IO;
using System.Text.Json;

namespace VRCLauncher.Models
{
    public class ConfigService : IConfigService
    {
        private const string DEFAULT_VRCHAT_PATH = @"C:\Program Files (x86)\Steam\steamapps\common\VRChat\VRChat.exe";
        private readonly string _configFilePath;

        public ConfigService(string configFilePath)
        {
            _configFilePath = configFilePath;
        }

        public bool ExistConfigFile()
        {
            return File.Exists(_configFilePath);
        }

        public Config Load()
        {
            if (!ExistConfigFile())
            {
                return Initialize();
            }

            var configJson = File.ReadAllText(_configFilePath);
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
                    WriteIndented = true
                };
                var configJson = JsonSerializer.Serialize(this, option);
                File.WriteAllText(_configFilePath, configJson);
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

        private Config Initialize()
        {
            var config = new Config
            {
                VRChatPath = DEFAULT_VRCHAT_PATH
            };
            Save(config);
            return config;
        }
    }
}
