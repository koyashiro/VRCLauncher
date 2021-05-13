using System;
using Xunit;
using Moq;
using VRCLauncher.Models;
using System.IO;
using System.Text.Json;

namespace VRCLauncher.Test.Models
{
    public class ConfigServiceTest
    {
        private static readonly string _vrchatPath = @"C:\Program Files (x86)\Steam\steamapps\common\VRChat\VRChat.exe";
        private static readonly string _configFilePath = Path.Join(AppDomain.CurrentDomain.BaseDirectory, "VRCLauncher.json");

        [Fact]
        public void LoadTest_ExistsConfigFile()
        {
            var expectedConfig = new Config
            {
                VRChatPath = _vrchatPath,
            };
            var expectedConfigJson = Serialize(expectedConfig);

            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(fw => fw.Exists(_configFilePath)).Returns(true);
            mockFileWrapper.Setup(fw => fw.ReadAllText(_configFilePath)).Returns(expectedConfigJson);

            var configService = new ConfigService(mockFileWrapper.Object);
            var actualConfig = configService.Load();

            Assert.Equal(expectedConfig.VRChatPath, actualConfig.VRChatPath);
        }

        [Fact]
        public void LoadTest_NotExistsConfigFile()
        {
            var expectedConfigFilePath = _configFilePath;
            var expectedConfig = new Config
            {
                VRChatPath = _vrchatPath,
            };
            var expectedConfigJson = Serialize(expectedConfig);

            string actualConfigFilePath = string.Empty;
            string? actualConfigJson = default;

            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(fw => fw.Exists(_configFilePath)).Returns(false);
            mockFileWrapper.Setup(fw => fw.WriteAllText(It.IsAny<string>(), It.IsAny<string?>()))
                .Callback<string, string?>((path, contexts) =>
                {
                    actualConfigFilePath = path;
                    actualConfigJson = contexts;
                });

            var configService = new ConfigService(mockFileWrapper.Object);
            var actualConfig = configService.Load();

            Assert.Equal(expectedConfigFilePath, actualConfigFilePath);
            Assert.Equal(expectedConfigJson, actualConfigJson);
            Assert.Equal(expectedConfig.VRChatPath, actualConfig.VRChatPath);
        }

        [Fact]
        public void SaveTest()
        {
            var expectedConfigFilePath = _configFilePath;
            var expectedConfig = new Config
            {
                VRChatPath = _vrchatPath,
            };
            var expectedConfigJson = Serialize(expectedConfig);

            string actualConfigFilePath = string.Empty;
            string? actualConfigJson = default;

            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(fw => fw.Exists(_configFilePath)).Returns(true);
            mockFileWrapper.Setup(fw => fw.WriteAllText(It.IsAny<string>(), It.IsAny<string?>()))
                .Callback<string, string?>((path, contexts) =>
                {
                    actualConfigFilePath = path;
                    actualConfigJson = contexts;
                });

            var configService = new ConfigService(mockFileWrapper.Object);
            configService.Save(expectedConfig);

            Assert.Equal(expectedConfigFilePath, actualConfigFilePath);
            Assert.Equal(expectedConfigJson, actualConfigJson);
        }

        private string Serialize(Config config)
        {
            var option = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            return JsonSerializer.Serialize(config, option);
        }
    }
}
