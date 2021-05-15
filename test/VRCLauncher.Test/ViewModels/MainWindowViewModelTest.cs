using Moq;
using VRCLauncher.Models;
using VRCLauncher.Services;
using VRCLauncher.ViewModels;
using VRCLauncher.Wrappers;
using Xunit;
using Xunit.Sdk;

namespace VRCLauncher.Test.ViewModels
{
    public class MainWindowViewModelTest
    {
        private const string WORLD_ID = "wrld_00000000-0000-0000-0000-000000000000";
        private const string INSTANCE_ID = "00000";
        private const string INSTANCE_OWNER_ID = "usr_00000000-0000-0000-0000-000000000000";
        private const string NONCE = "0000000000000000000000000000000000000000000000000000000000000000";

        [Fact]
        public void UriToLaunchParameterTest_Public()
        {
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}"; ;

            var mockLauncher = new Mock<ILaunchService>();
            var mockWindowWrapper = new Mock<IWindowWrapper>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(WORLD_ID, mainWindowViewModel.WorldId.Value);
            Assert.Equal(INSTANCE_ID, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(InstanceType.Public, mainWindowViewModel.InstanceType.Value);
            Assert.Null(mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Null(mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_FriendPlus()
        {
            var instanceType = InstanceType.FriendPlus;
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~hidden({INSTANCE_OWNER_ID})~nonce({NONCE})";

            var mockLauncher = new Mock<ILaunchService>();
            var mockWindowWrapper = new Mock<IWindowWrapper>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(WORLD_ID, mainWindowViewModel.WorldId.Value);
            Assert.Equal(INSTANCE_ID, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(INSTANCE_OWNER_ID, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(NONCE, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_FriendOnly()
        {
            var instanceType = InstanceType.FriendOnly;
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~friends({INSTANCE_OWNER_ID})~nonce({NONCE})";

            var mockLauncher = new Mock<ILaunchService>();
            var mockWindowWrapper = new Mock<IWindowWrapper>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(WORLD_ID, mainWindowViewModel.WorldId.Value);
            Assert.Equal(INSTANCE_ID, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(INSTANCE_OWNER_ID, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(NONCE, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_InvitePlus()
        {
            var instanceType = InstanceType.InvitePlus;
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~private({INSTANCE_OWNER_ID})~nonce({NONCE})~canRequestInvite";
            var mockLauncher = new Mock<ILaunchService>();
            var mockWindowWrapper = new Mock<IWindowWrapper>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(WORLD_ID, mainWindowViewModel.WorldId.Value);
            Assert.Equal(INSTANCE_ID, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(INSTANCE_OWNER_ID, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(NONCE, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_InviteOnly()
        {
            var instanceType = InstanceType.InviteOnly;
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~private({INSTANCE_OWNER_ID})~nonce({NONCE})";

            var mockLauncher = new Mock<ILaunchService>();
            var mockWindowWrapper = new Mock<IWindowWrapper>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(WORLD_ID, mainWindowViewModel.WorldId.Value);
            Assert.Equal(INSTANCE_ID, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(INSTANCE_OWNER_ID, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(NONCE, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void LaunchVRCommandTest()
        {
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}";

            var mockLauncher = new Mock<ILaunchService>();
            mockLauncher.Setup(l => l.LaunchVR(uri)).Verifiable();
            mockLauncher.Setup(l => l.LaunchDesktop(It.IsAny<string>())).Throws<XunitException>();

            var mockWindowWrapper = new Mock<IWindowWrapper>();
            mockWindowWrapper.Setup(wr => wr.Close()).Verifiable();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);

            Assert.False(mainWindowViewModel.LaunchVRCommand.CanExecute());
            Assert.False(mainWindowViewModel.LaunchDesktopCommand.CanExecute());

            mainWindowViewModel.Uri.Value = uri;
            Assert.True(mainWindowViewModel.LaunchVRCommand.CanExecute());
            Assert.True(mainWindowViewModel.LaunchDesktopCommand.CanExecute());

            mainWindowViewModel.LaunchVRCommand.Execute();
            mockLauncher.Verify();
            mockWindowWrapper.Verify();
        }

        [Fact]
        public void LaunchDesktopCommandTest()
        {
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}";

            var mockLauncher = new Mock<ILaunchService>();
            mockLauncher.Setup(l => l.LaunchVR(It.IsAny<string>())).Throws<XunitException>();
            mockLauncher.Setup(l => l.LaunchDesktop(uri)).Verifiable();

            var mockWindowWrapper = new Mock<IWindowWrapper>();
            mockWindowWrapper.Setup(wr => wr.Close()).Verifiable();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);
            Assert.False(mainWindowViewModel.LaunchVRCommand.CanExecute());
            Assert.False(mainWindowViewModel.LaunchDesktopCommand.CanExecute());

            mainWindowViewModel.Uri.Value = uri;
            Assert.True(mainWindowViewModel.LaunchVRCommand.CanExecute());
            Assert.True(mainWindowViewModel.LaunchDesktopCommand.CanExecute());

            mainWindowViewModel.LaunchDesktopCommand.Execute();
            mockLauncher.Verify();
            mockWindowWrapper.Verify();
        }
    }
}
