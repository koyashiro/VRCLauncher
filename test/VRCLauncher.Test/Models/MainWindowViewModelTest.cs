using Moq;
using VRCLauncher.Models;
using VRCLauncher.ViewModels;
using Xunit;
using Xunit.Sdk;

namespace VRCLauncher.Test.Models
{
    public class MainWindowViewModelTest
    {
        private const string WORLD_ID = "wrld_00000000-0000-0000-0000-000000000000";
        private const string INSTANCE_ID = "00000";

        [Fact]
        public void UriToLaunchParameterTest_Public()
        {
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}"; ;

            var mockLauncher = new Mock<ILauncher>();
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
            var instanceOwnerId = "usr_00000000-0000-0000-0000-000000000000";
            var nonce = "0000000000000000000000000000000000000000000000000000000000000000";
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~hidden({instanceOwnerId})~nonce({nonce})";

            var mockLauncher = new Mock<ILauncher>();
            var mockWindowWrapper = new Mock<IWindowWrapper>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(WORLD_ID, mainWindowViewModel.WorldId.Value);
            Assert.Equal(INSTANCE_ID, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(instanceOwnerId, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(nonce, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_FriendOnly()
        {
            var instanceType = InstanceType.FriendOnly;
            var instanceOwnerId = "usr_00000000-0000-0000-0000-000000000000";
            var nonce = "0000000000000000000000000000000000000000000000000000000000000000";
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~friends({instanceOwnerId})~nonce({nonce})";

            var mockLauncher = new Mock<ILauncher>();
            var mockWindowWrapper = new Mock<IWindowWrapper>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(WORLD_ID, mainWindowViewModel.WorldId.Value);
            Assert.Equal(INSTANCE_ID, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(instanceOwnerId, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(nonce, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_InvitePlus()
        {
            var instanceType = InstanceType.InvitePlus;
            var instanceOwnerId = "usr_00000000-0000-0000-0000-000000000000";
            var nonce = "0000000000000000000000000000000000000000000000000000000000000000";
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~private({instanceOwnerId})~nonce({nonce})~canRequestInvite";
            var mockLauncher = new Mock<ILauncher>();
            var mockWindowWrapper = new Mock<IWindowWrapper>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(WORLD_ID, mainWindowViewModel.WorldId.Value);
            Assert.Equal(INSTANCE_ID, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(instanceOwnerId, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(nonce, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void UriToLaunchParameterTest_InviteOnly()
        {
            var instanceType = InstanceType.InviteOnly;
            var instanceOwnerId = "usr_00000000-0000-0000-0000-000000000000";
            var nonce = "0000000000000000000000000000000000000000000000000000000000000000";
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~private({instanceOwnerId})~nonce({nonce})";

            var mockLauncher = new Mock<ILauncher>();
            var mockWindowWrapper = new Mock<IWindowWrapper>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);
            mainWindowViewModel.Uri.Value = uri;

            Assert.Equal(WORLD_ID, mainWindowViewModel.WorldId.Value);
            Assert.Equal(INSTANCE_ID, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(instanceOwnerId, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(nonce, mainWindowViewModel.Nonce.Value);
        }

        [Fact]
        public void LaunchVRCommandTest()
        {
            var uri = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}";

            var mockLauncher = new Mock<ILauncher>();
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

            var mockLauncher = new Mock<ILauncher>();
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
