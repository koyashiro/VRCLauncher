using Xunit;
using Moq;
using VRCLauncher.ViewModels;
using VRCLauncher.Models;
using Xunit.Sdk;

namespace VRCLauncher.Test.Models
{
    public class MainWindowViewModelTest
    {
        [Fact]
        public void UriToLaunchParameterTest_Public()
        {
            var worldId = "wrld_00000000-0000-0000-0000-000000000000";
            var instanceId = "00000";
            var instanceType = InstanceType.Public;
            string? instanceOwnerId = default;
            string? nonce = default;
            var uri = $"vrchat://launch/?ref=vrchat.com&id={worldId}:{instanceId}"; ;

            var mockLauncher = new Mock<ILauncher>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(worldId , mainWindowViewModel.WorldId.Value);
            Assert.Equal(instanceId, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(instanceOwnerId, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(nonce, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_FriendPlus()
        {
            var worldId = "wrld_00000000-0000-0000-0000-000000000000";
            var instanceId = "00000";
            var instanceType = InstanceType.FriendPlus;
            var instanceOwnerId = "usr_00000000-0000-0000-0000-000000000000";
            var nonce = "0000000000000000000000000000000000000000000000000000000000000000";
            var uri = $"vrchat://launch/?ref=vrchat.com&id={worldId}:{instanceId}~hidden({instanceOwnerId})~nonce({nonce})";

            var mockLauncher = new Mock<ILauncher>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(worldId , mainWindowViewModel.WorldId.Value);
            Assert.Equal(instanceId, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(instanceOwnerId, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(nonce, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_FriendOnly()
        {
            var worldId = "wrld_00000000-0000-0000-0000-000000000000";
            var instanceId = "00000";
            var instanceType = InstanceType.FriendOnly;
            var instanceOwnerId = "usr_00000000-0000-0000-0000-000000000000";
            var nonce = "0000000000000000000000000000000000000000000000000000000000000000";
            var uri = $"vrchat://launch/?ref=vrchat.com&id={worldId}:{instanceId}~friends({instanceOwnerId})~nonce({nonce})";

            var mockLauncher = new Mock<ILauncher>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(worldId , mainWindowViewModel.WorldId.Value);
            Assert.Equal(instanceId, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(instanceOwnerId, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(nonce, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_InvitePlus()
        {
            var worldId = "wrld_00000000-0000-0000-0000-000000000000";
            var instanceId = "00000";
            var instanceType = InstanceType.InvitePlus;
            var instanceOwnerId = "usr_00000000-0000-0000-0000-000000000000";
            var nonce = "0000000000000000000000000000000000000000000000000000000000000000";
            var uri = $"vrchat://launch/?ref=vrchat.com&id={worldId}:{instanceId}~private({instanceOwnerId})~nonce({nonce})~canRequestInvite";

            var mockLauncher = new Mock<ILauncher>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(worldId , mainWindowViewModel.WorldId.Value);
            Assert.Equal(instanceId, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(instanceOwnerId, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(nonce, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_InviteOnly()
        {
            var worldId = "wrld_00000000-0000-0000-0000-000000000000";
            var instanceId = "00000";
            var instanceType = InstanceType.InviteOnly;
            var instanceOwnerId = "usr_00000000-0000-0000-0000-000000000000";
            var nonce = "0000000000000000000000000000000000000000000000000000000000000000";
            var uri = $"vrchat://launch/?ref=vrchat.com&id={worldId}:{instanceId}~private({instanceOwnerId})~nonce({nonce})";

            var mockLauncher = new Mock<ILauncher>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(worldId , mainWindowViewModel.WorldId.Value);
            Assert.Equal(instanceId, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(instanceOwnerId, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(nonce, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void LaunchVRCommandTest()
        {
            var worldId = "wrld_00000000-0000-0000-0000-000000000000";
            var instanceId = "00000";
            var uri = $"vrchat://launch/?ref=vrchat.com&id={worldId}:{instanceId}";

            var expectedArguments = uri;
            var actualArguments = string.Empty;

            var mockLauncher = new Mock<ILauncher>();
            mockLauncher.Setup(ml => ml.LaunchVR(It.IsAny<string>()))
                .Callback<string>(arguments => actualArguments = arguments);
            mockLauncher.Setup(ml => ml.LaunchDesktop(It.IsAny<string>()))
                .Callback<string>(_ => throw new XunitException());

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object);
            Assert.False(mainWindowViewModel.LaunchVRCommand.CanExecute());
            Assert.False(mainWindowViewModel.LaunchDesktopCommand.CanExecute());

            mainWindowViewModel.Uri.Value = uri;
            Assert.True(mainWindowViewModel.LaunchVRCommand.CanExecute());
            Assert.True(mainWindowViewModel.LaunchDesktopCommand.CanExecute());

            // TODO: fix Window close action
            // mainWindowViewModel.LaunchVRCommand.Execute();
            // Assert.Equal(expectedArguments, actualArguments);
        }

        [Fact]
        public void LaunchDesktopCommandTest()
        {
            var worldId = "wrld_00000000-0000-0000-0000-000000000000";
            var instanceId = "00000";
            var uri = $"vrchat://launch/?ref=vrchat.com&id={worldId}:{instanceId}";

            var expectedArguments = uri;
            var actualArguments = string.Empty;

            var mockLauncher = new Mock<ILauncher>();
            mockLauncher.Setup(ml => ml.LaunchVR(It.IsAny<string>()))
                .Callback<string>(_ => throw new XunitException());
            mockLauncher.Setup(ml => ml.LaunchDesktop(It.IsAny<string>()))
                .Callback<string>(arguments => actualArguments = arguments);

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object);
            Assert.False(mainWindowViewModel.LaunchVRCommand.CanExecute());
            Assert.False(mainWindowViewModel.LaunchDesktopCommand.CanExecute());

            mainWindowViewModel.Uri.Value = uri;
            Assert.True(mainWindowViewModel.LaunchVRCommand.CanExecute());
            Assert.True(mainWindowViewModel.LaunchDesktopCommand.CanExecute());

            // TODO: fix Window close action
            // mainWindowViewModel.LaunchDesktopCommand.Execute();
            // Assert.Equal(expectedArguments, actualArguments);
        }
    }
}
