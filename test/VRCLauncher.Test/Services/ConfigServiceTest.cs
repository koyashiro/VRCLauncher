using Moq;
using System.Collections.Generic;
using VRCLauncher.Services;
using VRCLauncher.Wrappers;
using Xunit;
using Xunit.Sdk;

namespace VRCLauncher.Test.Services
{
    public class ConfigServiceTest
    {
        public static IEnumerable<object?[]> Initialize_MemberData()
        {
            Mock<IDirectoryWrapper> mockDirectoryWrapper;
            Mock<IFileWrapper> mockFileWrapper;

            // when config directory and file does not exists
            mockDirectoryWrapper = CreateMockDirectoryWrapper(false);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Verifiable();
            mockFileWrapper = CreateMockFileWrapper(false);
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.DEFAULT_CONFIG_JSON))
                .Verifiable();
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper };

            // when config directory exists and config file does not exists
            mockDirectoryWrapper = CreateMockDirectoryWrapper(true);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                .Throws<XunitException>();
            mockFileWrapper = CreateMockFileWrapper(false);
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.DEFAULT_CONFIG_JSON))
                .Verifiable();
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper };

            // when config directory and config file exists
            mockDirectoryWrapper = CreateMockDirectoryWrapper(true);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                .Throws<XunitException>();
            mockFileWrapper = CreateMockFileWrapper(true);
            mockFileWrapper.Setup(fw => fw.WriteAllText(It.IsAny<string>(), It.IsAny<string?>()))
                .Throws<XunitException>();
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper };
        }

        [Theory]
        [MemberData(nameof(Initialize_MemberData))]
        public void Initialize(Mock<IDirectoryWrapper> mockDirectoryWrapper, Mock<IFileWrapper> mockFileWrapper)
        {
            var mockEnvironmentWrapper = CreateMockEnvironmentWrapper();
            var configService = new ConfigService(mockDirectoryWrapper.Object, mockFileWrapper.Object, mockEnvironmentWrapper.Object);
            configService.Initialize();

            mockDirectoryWrapper.Verify();
            mockFileWrapper.Verify();
        }

        [Fact]
        public void GetConfigDirectoryPath()
        {
            var mockEnvironmentWrapper = CreateMockEnvironmentWrapper();
            var mockDirectoryWrapper = new Mock<IDirectoryWrapper>();
            var mockFileWrapper = new Mock<IFileWrapper>();
            var configService = new ConfigService(mockDirectoryWrapper.Object, mockFileWrapper.Object, mockEnvironmentWrapper.Object);
            Assert.Equal(TestConstantValue.CONFIG_DIRECTORY_PATH, configService.GetConfigDirectoryPath());
        }

        [Fact]
        public void GetConfigFilePath()
        {
            var mockEnvironmentWrapper = CreateMockEnvironmentWrapper();
            var mockDirectoryWrapper = new Mock<IDirectoryWrapper>();
            var mockFileWrapper = new Mock<IFileWrapper>();
            var configService = new ConfigService(mockDirectoryWrapper.Object, mockFileWrapper.Object, mockEnvironmentWrapper.Object);
            Assert.Equal(TestConstantValue.CONFIG_FILE_PATH, configService.GetConfigFilePath());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ExistsConfigDirectory(bool exists)
        {
            var mockEnvironmentWrapper = CreateMockEnvironmentWrapper();
            var mockDirectoryWrapper = CreateMockDirectoryWrapper(exists);
            var mockFileWrapper = CreateMockFileWrapper(false);
            var configService = new ConfigService(mockDirectoryWrapper.Object, mockFileWrapper.Object, mockEnvironmentWrapper.Object);
            Assert.Equal(exists, configService.ExistsConfigDirectory());
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void ExistsConfigFile(bool exists)
        {
            var mockEnvironmentWrapper = CreateMockEnvironmentWrapper();
            var mockDirectoryWrapper = CreateMockDirectoryWrapper(exists);
            var mockFileWrapper = CreateMockFileWrapper(exists);
            var configService = new ConfigService(mockDirectoryWrapper.Object, mockFileWrapper.Object, mockEnvironmentWrapper.Object);
            Assert.Equal(exists, configService.ExistsConfigFile());
        }

        public static IEnumerable<object?[]> Load_MemberData()
        {
            Mock<IDirectoryWrapper> mockDirectoryWrapper;
            Mock<IFileWrapper> mockFileWrapper;
            string vrchatPath;

            // when config directory and file does not exists
            mockDirectoryWrapper = CreateMockDirectoryWrapper(false);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Verifiable();
            mockFileWrapper = CreateMockFileWrapper(false);
            mockFileWrapper.Setup(fw => fw.ReadAllText(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(TestConstantValue.DEFAULT_CONFIG_JSON)
                .Verifiable();
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.DEFAULT_CONFIG_JSON))
                .Verifiable();
            vrchatPath = TestConstantValue.DEFAULT_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper, vrchatPath };

            // when config directory exists and config file does not exists
            mockDirectoryWrapper = CreateMockDirectoryWrapper(true);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                .Throws<XunitException>();
            mockFileWrapper = CreateMockFileWrapper(false);
            mockFileWrapper.Setup(fw => fw.ReadAllText(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(TestConstantValue.DEFAULT_CONFIG_JSON)
                .Verifiable();
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.DEFAULT_CONFIG_JSON))
                .Verifiable();
            vrchatPath = TestConstantValue.DEFAULT_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper, vrchatPath };

            // when config directory and config file exists
            mockDirectoryWrapper = CreateMockDirectoryWrapper(true);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                .Throws<XunitException>();
            mockFileWrapper = CreateMockFileWrapper(true);
            mockFileWrapper.Setup(fw => fw.ReadAllText(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(TestConstantValue.TEST_CONFIG_JSON)
                .Verifiable();
            mockFileWrapper.Setup(fw => fw.WriteAllText(It.IsAny<string>(), It.IsAny<string?>()))
                .Throws<XunitException>();
            vrchatPath = TestConstantValue.TEST_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper, vrchatPath };
        }

        [Theory]
        [MemberData(nameof(Load_MemberData))]
        public void Load(Mock<IDirectoryWrapper> mockDirectoryWrapper, Mock<IFileWrapper> mockFileWrapper, string vrchatPath)
        {
            var mockEnvironmentWrapper = CreateMockEnvironmentWrapper();
            var configService = new ConfigService(mockDirectoryWrapper.Object, mockFileWrapper.Object, mockEnvironmentWrapper.Object);
            var config = configService.Load();

            Assert.Equal(vrchatPath, config.VRChatPath);
            mockDirectoryWrapper.Verify();
            mockFileWrapper.Verify();
        }

        public static IEnumerable<object?[]> Save_MemberData()
        {
            Mock<IDirectoryWrapper> mockDirectoryWrapper;
            Mock<IFileWrapper> mockFileWrapper;
            string vrchatPath;

            // when config directory and file does not exists
            mockDirectoryWrapper = CreateMockDirectoryWrapper(false);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Verifiable();
            mockFileWrapper = CreateMockFileWrapper(false);
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.TEST_CONFIG_JSON))
                .Verifiable();
            vrchatPath = TestConstantValue.DEFAULT_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper };

            // when config directory exists and config file does not exists
            mockDirectoryWrapper = CreateMockDirectoryWrapper(true);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                .Throws<XunitException>();
            mockFileWrapper = CreateMockFileWrapper(false);
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.TEST_CONFIG_JSON))
                .Verifiable();
            vrchatPath = TestConstantValue.DEFAULT_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper };

            // when config directory and config file exists
            mockDirectoryWrapper = CreateMockDirectoryWrapper(true);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                .Throws<XunitException>();
            mockFileWrapper = CreateMockFileWrapper(true);
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.TEST_CONFIG_JSON))
                .Verifiable();
            vrchatPath = TestConstantValue.TEST_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper };
        }

        [Theory]
        [MemberData(nameof(Save_MemberData))]
        public void Save(Mock<IDirectoryWrapper> mockDirectoryWrapper, Mock<IFileWrapper> mockFileWrapper)
        {
            var mockEnvironmentWrapper = CreateMockEnvironmentWrapper();
            var configService = new ConfigService(mockDirectoryWrapper.Object, mockFileWrapper.Object, mockEnvironmentWrapper.Object);
            configService.Save(TestConstantValue.TEST_CONFIG);

            mockDirectoryWrapper.Verify();
            mockFileWrapper.Verify();
        }

        private static Mock<IEnvironmentWrapper> CreateMockEnvironmentWrapper()
        {
            var mockEnvironmentWrapper = new Mock<IEnvironmentWrapper>();
            mockEnvironmentWrapper.SetupGet(ew => ew.NewLine)
                .Returns(TestConstantValue.NEW_LINE);
            mockEnvironmentWrapper.Setup(ew => ew.GetLocalApplicationDataDirectoryPath())
                .Returns(TestConstantValue.LOCAL_APPLICATION_DATA);
            return mockEnvironmentWrapper;
        }

        private static Mock<IDirectoryWrapper> CreateMockDirectoryWrapper(bool exists)
        {
            var mockDirectoryWrapper = new Mock<IDirectoryWrapper>();
            mockDirectoryWrapper.Setup(dw => dw.Exists(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Returns(exists);
            return mockDirectoryWrapper;
        }

        private static Mock<IFileWrapper> CreateMockFileWrapper(bool exists)
        {
            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(dw => dw.Exists(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(exists);
            return mockFileWrapper;
        }
    }
}
