using System;
using System.IO;
using System.Text.Json;

namespace VRCLauncher.Utils
{
    public class Config
    {
        private const string CONFIG_FILE_NAME = "VRCLauncher.json";
        private const string DEFAULT_VRCHAT_PATH = @"C:\Program Files (x86)\Steam\steamapps\common\VRChat\VRChat.exe";
        private static readonly string CONFIG_FILE_PATH = $"{Path.Join(AppDomain.CurrentDomain.BaseDirectory, CONFIG_FILE_NAME)}";

        public string VRChatPath { get; } = DEFAULT_VRCHAT_PATH;

        public static Config Load()
        {
            if (!File.Exists(CONFIG_FILE_PATH))
            {
                return Initialize();
            }

            var configJson = File.ReadAllText(CONFIG_FILE_PATH);
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
                return Initialize();
            }
            catch (IOException)
            {
                return Initialize();
            }
            catch (UnauthorizedAccessException)
            {
                return Initialize();
            }
            catch (JsonException)
            {
                return Initialize();
            }
        }

        public static void Save(Config config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            try
            {
                var option = new JsonSerializerOptions
                {
                    WriteIndented = true
                };
                var configJson = JsonSerializer.Serialize(config, option);
                File.WriteAllText(CONFIG_FILE_PATH, configJson);
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

        public static Config Initialize()
        {
            var config = new Config();
            Save(config);
            return config;
        }
    }
}
