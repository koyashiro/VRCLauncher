using Xunit;
using Moq;
using VRCLauncher.Models;
using System.Text.Json;
using System.Diagnostics;

namespace VRCLauncher.Test.Models
{
    public class LauncherTest
    {
        private const string ARGUMENTS = "vrchat://launch";
        private const string VRCHAT_PATH = @"C:\Program Files (x85)\Steam\steamapps\common\VRChat\VRChat.exe";

        [Fact]
        public void LaunchVRTest()
        {
            var expectedFileName = VRCHAT_PATH;
            var expectedArguments = ARGUMENTS;
            var actualFileName = string.Empty;
            var actualArguments = string.Empty;

            var config = new Config
            {
                VRChatPath = VRCHAT_PATH,
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

            var launcher = new Launcher(mockConfigService.Object, mockProcessWrapper.Object);
            launcher.LaunchVR(expectedArguments);

            Assert.Equal(expectedFileName, actualFileName);
            Assert.Equal(expectedArguments, expectedArguments);
        }

        [Fact]
        public void LaunchDesktopTest()
        {
            var expectedFileName = VRCHAT_PATH;
            var expectedArguments = $"{ARGUMENTS} --no-vr";
            var actualFileName = string.Empty;
            var actualArguments = string.Empty;

            var config = new Config
            {
                VRChatPath = VRCHAT_PATH,
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

            var launcher = new Launcher(mockConfigService.Object, mockProcessWrapper.Object);
            launcher.LaunchVR(expectedArguments);

            Assert.Equal(expectedFileName, actualFileName);
            Assert.Equal(expectedArguments, expectedArguments);
        }
    }
}
