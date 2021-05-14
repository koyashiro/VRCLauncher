using Moq;
using System;
using System.IO;
using System.Text.Json;
using VRCLauncher.Models;
using Xunit;
using Xunit.Sdk;

namespace VRCLauncher.Test.Models
{
    public class ConfigServiceTest
    {
        private const string VRCHAT_PATH = @"C:\tmp\VRChat.exe";
        private const string DEFAULT_VRCHAT_PATH = @"C:\Program Files (x86)\Steam\steamapps\common\VRChat\VRChat.exe";
        private static readonly string CONFIG_FILE_PATH = $"{Path.Join(AppDomain.CurrentDomain.BaseDirectory, "VRCLauncher.json")}";

        [Fact]
        public void LoadTest_ExistsConfigFile()
        {
            var expectedConfig = new Config
            {
                VRChatPath = VRCHAT_PATH,
            };

            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(fw => fw.Exists(CONFIG_FILE_PATH))
                .Returns(true)
                .Verifiable();
            mockFileWrapper.Setup(fw => fw.ReadAllText(CONFIG_FILE_PATH))
                .Returns(Serialize(expectedConfig))
                .Verifiable();

            var configService = new ConfigService(mockFileWrapper.Object);
            var actualConfig = configService.Load();

            Assert.Equal(expectedConfig.VRChatPath, actualConfig.VRChatPath);
            mockFileWrapper.Verify();
        }

        [Fact]
        public void LoadTest_NotExistsConfigFile()
        {
            var expectedConfig = new Config
            {
                VRChatPath = DEFAULT_VRCHAT_PATH,
            };

            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(fw => fw.Exists(CONFIG_FILE_PATH))
                .Returns(false)
                .Verifiable();
            mockFileWrapper.Setup(fw => fw.WriteAllText(CONFIG_FILE_PATH, Serialize(expectedConfig)))
                .Verifiable();

            var configService = new ConfigService(mockFileWrapper.Object);
            var actualConfig = configService.Load();

            Assert.Equal(DEFAULT_VRCHAT_PATH, actualConfig.VRChatPath);
            mockFileWrapper.Verify();
        }

        [Fact]
        public void SaveTest()
        {
            var expectedConfig = new Config
            {
                VRChatPath = VRCHAT_PATH,
            };

            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(fw => fw.Exists(VRCHAT_PATH))
                .Throws<XunitException>();
            mockFileWrapper.Setup(fw => fw.WriteAllText(CONFIG_FILE_PATH, Serialize(expectedConfig)))
                .Verifiable();

            var configService = new ConfigService(mockFileWrapper.Object);
            configService.Save(expectedConfig);

            mockFileWrapper.Verify();
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
