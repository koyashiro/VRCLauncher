using Moq;
using VRCLauncher.Services;
using VRCLauncher.Wrappers;
using Xunit;
using Xunit.Sdk;

namespace VRCLauncher.Test.Services
{
    public class ConfigServiceTest
    {
        [Fact]
        public void LoadTest_NotExistsConfigFile()
        {
            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(fw => fw.Exists(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(false)
                .Verifiable();
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.DEFAULT_CONFIG_JSON))
                .Verifiable();

            var configService = new ConfigService(mockFileWrapper.Object);
            var config = configService.Load();

            Assert.Equal(TestConstantValue.DEFAULT_VRCHAT_PATH, config.VRChatPath);
            mockFileWrapper.Verify();
        }

        [Fact]
        public void LoadTest_ExistsConfigFile()
        {
            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(fw => fw.Exists(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(true)
                .Verifiable();
            mockFileWrapper.Setup(fw => fw.ReadAllText(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(TestConstantValue.TEST_CONFIG_JSON)
                .Verifiable();

            var configService = new ConfigService(mockFileWrapper.Object);
            var config = configService.Load();

            Assert.Equal(TestConstantValue.TEST_VRCHAT_PATH, config.VRChatPath);
            mockFileWrapper.Verify();
        }

        [Fact]
        public void SaveTest()
        {
            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(fw => fw.Exists(TestConstantValue.TEST_VRCHAT_PATH))
                .Throws<XunitException>();
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.TEST_CONFIG_JSON))
                .Verifiable();

            var configService = new ConfigService(mockFileWrapper.Object);
            configService.Save(TestConstantValue.TEST_CONFIG);

            mockFileWrapper.Verify();
        }
    }
}
