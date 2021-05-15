using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using VRCLauncher.Services;
using VRCLauncher.Wrappers;
using Xunit;
using Xunit.Sdk;

namespace VRCLauncher.Test.Services
{
    public class ConfigServiceTest
    {
        private readonly IEnvironmentWrapper _environmentWrapper;

        public ConfigServiceTest()
        {
            var mockEnvironmentWrapper = new Mock<IEnvironmentWrapper>();
            mockEnvironmentWrapper.Setup(ew => ew.GetLocalApplicationDataDirectoryPath())
                .Returns(TestConstantValue.LOCAL_APPLICATION_DATA);
            _environmentWrapper = mockEnvironmentWrapper.Object;
        }

        [Theory]
        [InlineData(false, false)]
        [InlineData(false, true)]
        [InlineData(true, true)]
        public void Initialize(bool existsConfigFile, bool existsConfigDirectory)
        {
            var mockDirectoryWrapper = new Mock<IDirectoryWrapper>();
            mockDirectoryWrapper.Setup(dw => dw.Exists(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Returns(existsConfigDirectory);

            var mockFileWrapper = new Mock<IFileWrapper>();
            mockFileWrapper.Setup(fw => fw.Exists(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(existsConfigFile);

            if (existsConfigFile)
            {
                mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                    .Throws<XunitException>();
                mockFileWrapper.Setup(fw => fw.WriteAllText(It.IsAny<string>(), It.IsAny<string?>()))
                    .Throws<XunitException>();
            }
            else if (existsConfigDirectory)
            {
                mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                    .Throws<XunitException>();
                mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.DEFAULT_CONFIG_JSON))
                    .Verifiable();
            }
            else
            {
                mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(TestConstantValue.CONFIG_DIRECTORY_PATH))
                    .Verifiable();
                mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.DEFAULT_CONFIG_JSON))
                    .Verifiable();
            }

            var configService = new ConfigService(mockDirectoryWrapper.Object, mockFileWrapper.Object, _environmentWrapper);
            configService.Initialize();

            mockFileWrapper.Verify();
        }

        public static IEnumerable<object?[]> Load_MemberData()
        {
            Mock<IDirectoryWrapper> mockDirectoryWrapper;
            Mock<IFileWrapper> mockFileWrapper;
            string vrchatPath;

            // when config directory and file does not exists
            mockDirectoryWrapper = new();
            mockDirectoryWrapper.Setup(dw => dw.Exists(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Returns(false);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Verifiable();
            mockFileWrapper = new();
            mockFileWrapper.Setup(fw => fw.Exists(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(false);
            mockFileWrapper.Setup(fw => fw.ReadAllText(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(TestConstantValue.DEFAULT_CONFIG_JSON)
                .Verifiable();
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.DEFAULT_CONFIG_JSON))
                .Verifiable();
            vrchatPath = TestConstantValue.DEFAULT_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper, vrchatPath };

            // when config directory exists and config file does not exists
            mockDirectoryWrapper = new();
            mockDirectoryWrapper.Setup(dw => dw.Exists(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Returns(true);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                .Throws<XunitException>();
            mockFileWrapper = new();
            mockFileWrapper.Setup(fw => fw.Exists(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(false);
            mockFileWrapper.Setup(fw => fw.ReadAllText(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(TestConstantValue.DEFAULT_CONFIG_JSON)
                .Verifiable();
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.DEFAULT_CONFIG_JSON))
                .Verifiable();
            vrchatPath = TestConstantValue.DEFAULT_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper, vrchatPath };

            // when config directory and config file exists
            mockDirectoryWrapper = new();
            mockDirectoryWrapper.Setup(dw => dw.Exists(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Returns(true);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                .Throws<XunitException>();
            mockFileWrapper = new();
            mockFileWrapper.Setup(fw => fw.Exists(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(true);
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
            var configService = new ConfigService(mockDirectoryWrapper.Object, mockFileWrapper.Object, _environmentWrapper);
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
            mockDirectoryWrapper = new();
            mockDirectoryWrapper.Setup(dw => dw.Exists(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Returns(false);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Verifiable();
            mockFileWrapper = new();
            mockFileWrapper.Setup(fw => fw.Exists(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(false);
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.TEST_CONFIG_JSON))
                .Verifiable();
            vrchatPath = TestConstantValue.DEFAULT_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper };

            // when config directory exists and config file does not exists
            mockDirectoryWrapper = new();
            mockDirectoryWrapper.Setup(dw => dw.Exists(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Returns(true);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                .Throws<XunitException>();
            mockFileWrapper = new();
            mockFileWrapper.Setup(fw => fw.Exists(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(false);
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.TEST_CONFIG_JSON))
                .Verifiable();
            vrchatPath = TestConstantValue.DEFAULT_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper };

            // when config directory and config file exists
            mockDirectoryWrapper = new();
            mockDirectoryWrapper.Setup(dw => dw.Exists(TestConstantValue.CONFIG_DIRECTORY_PATH))
                .Returns(true);
            mockDirectoryWrapper.Setup(dw => dw.CreateDirectory(It.IsAny<string>()))
                .Throws<XunitException>();
            mockFileWrapper = new();
            mockFileWrapper.Setup(fw => fw.Exists(TestConstantValue.CONFIG_FILE_PATH))
                .Returns(true);
            mockFileWrapper.Setup(fw => fw.WriteAllText(TestConstantValue.CONFIG_FILE_PATH, TestConstantValue.TEST_CONFIG_JSON))
                .Verifiable();
            vrchatPath = TestConstantValue.TEST_VRCHAT_PATH;
            yield return new object?[] { mockDirectoryWrapper, mockFileWrapper };
        }

        [Theory]
        [MemberData(nameof(Save_MemberData))]
        public void Save(Mock<IDirectoryWrapper> mockDirectoryWrapper, Mock<IFileWrapper> mockFileWrapper)
        {
            var configService = new ConfigService(mockDirectoryWrapper.Object, mockFileWrapper.Object, _environmentWrapper);
            configService.Save(TestConstantValue.TEST_CONFIG);

            mockDirectoryWrapper.Verify();
            mockFileWrapper.Verify();
        }
    }
}
