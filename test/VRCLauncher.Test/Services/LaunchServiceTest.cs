using Moq;
using System.Diagnostics;
using VRCLauncher.Models;
using VRCLauncher.Services;
using VRCLauncher.Wrappers;
using Xunit;

namespace VRCLauncher.Test.Services
{
    public class LaunchServiceTest
    {
        [Fact]
        public void LaunchVRTest()
        {
            var expectedFileName = TestConstantValue.TEST_VRCHAT_PATH;
            var expectedArguments = TestConstantValue.URI_PUBLIC;
            var actualFileName = string.Empty;
            var actualArguments = string.Empty;

            var config = new Config
            {
                VRChatPath = TestConstantValue.TEST_VRCHAT_PATH,
            };
            var mockConfigService = new Mock<IConfigService>();
            mockConfigService.Setup(cs => cs.Load()).Returns(config);
            var mockProcessWrapper = new Mock<IProcessWrapper>();
            mockProcessWrapper.Setup(pw => pw.Start(It.IsAny<ProcessStartInfo>()))
                .Callback<ProcessStartInfo>(psi =>
                {
                    actualFileName = psi.FileName;
                    actualArguments = psi.Arguments;
                });

            var launchService = new LaunchService(mockConfigService.Object, mockProcessWrapper.Object);
            launchService.LaunchVR(expectedArguments);

            Assert.Equal(expectedFileName, actualFileName);
            Assert.Equal(expectedArguments, expectedArguments);
        }

        [Fact]
        public void LaunchDesktopTest()
        {
            var expectedFileName = TestConstantValue.TEST_VRCHAT_PATH;
            var expectedArguments = $"{TestConstantValue.URI_PUBLIC} --no-vr";
            var actualFileName = string.Empty;
            var actualArguments = string.Empty;

            var config = new Config
            {
                VRChatPath = TestConstantValue.TEST_VRCHAT_PATH,
            };
            var mockConfigService = new Mock<IConfigService>();
            mockConfigService.Setup(cs => cs.Load()).Returns(config);
            var mockProcessWrapper = new Mock<IProcessWrapper>();
            mockProcessWrapper.Setup(pw => pw.Start(It.IsAny<ProcessStartInfo>()))
                .Callback<ProcessStartInfo>(psi =>
                {
                    actualFileName = psi.FileName;
                    actualArguments = psi.Arguments;
                });

            var launchService = new LaunchService(mockConfigService.Object, mockProcessWrapper.Object);
            launchService.LaunchVR(expectedArguments);

            Assert.Equal(expectedFileName, actualFileName);
            Assert.Equal(expectedArguments, expectedArguments);
        }
    }
}
