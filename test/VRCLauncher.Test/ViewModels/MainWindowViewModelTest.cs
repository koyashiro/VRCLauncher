using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        public static IEnumerable<object?[]> UriChanged_MemberData()
        {
            // when invalid uri
            yield return new object?[] { "invaliduri", string.Empty, string.Empty, InstanceType.Public, null, null, false };

            // when Public
            yield return new object?[] { TestConstantValue.URI_PUBLIC, TestConstantValue.WORLD_ID, TestConstantValue.INSTANCE_ID, InstanceType.Public, null, null, true };

            // when Friend Plus
            yield return new object?[] { TestConstantValue.URI_FRIEND_PLUS, TestConstantValue.WORLD_ID, TestConstantValue.INSTANCE_ID, InstanceType.FriendPlus, TestConstantValue.INSTANCE_OWNER_ID, TestConstantValue.NONCE, true };

            // when Friend Only
            yield return new object?[] { TestConstantValue.URI_FRIEND_ONLY, TestConstantValue.WORLD_ID, TestConstantValue.INSTANCE_ID, InstanceType.FriendOnly, TestConstantValue.INSTANCE_OWNER_ID, TestConstantValue.NONCE, true };

            // when Invite Plus
            yield return new object?[] { TestConstantValue.URI_INVITE_PLUS, TestConstantValue.WORLD_ID, TestConstantValue.INSTANCE_ID, InstanceType.InvitePlus, TestConstantValue.INSTANCE_OWNER_ID, TestConstantValue.NONCE, true };

            // when Invite Only
            yield return new object?[] { TestConstantValue.URI_INVITE_ONLY, TestConstantValue.WORLD_ID, TestConstantValue.INSTANCE_ID, InstanceType.InviteOnly, TestConstantValue.INSTANCE_OWNER_ID, TestConstantValue.NONCE, true };
        }

        [Theory]
        [MemberData(nameof(UriChanged_MemberData))]
        public void UriChanged(string uri, string worldId, string instanceId, InstanceType instanceType, string? instanceOwnerId, string? nonce, bool canExecute)
        {
            var mockLauncher = new Mock<ILaunchService>();
            var mockWindowWrapper = new Mock<IWindowWrapper>();

            var mainWindowViewModel = new MainWindowViewModel(mockLauncher.Object, mockWindowWrapper.Object);

            mainWindowViewModel.Uri.Value = uri;
            Task.Delay(10).Wait();

            // check LaunchParameter
            Assert.Equal(worldId, mainWindowViewModel.WorldId.Value);
            Assert.Equal(instanceId, mainWindowViewModel.InstanceId.Value);
            Assert.Equal(instanceType, mainWindowViewModel.InstanceType.Value);
            Assert.Equal(instanceOwnerId, mainWindowViewModel.InstanceOwnerId.Value);
            Assert.Equal(nonce, mainWindowViewModel.Nonce.Value);

            // check Launch commands
            Assert.Equal(canExecute, mainWindowViewModel.LaunchVRCommand.CanExecute());
            Assert.Equal(canExecute, mainWindowViewModel.LaunchDesktopCommand.CanExecute());

            // execute LaunchVR command
            mockLauncher.Setup(l => l.LaunchVR(uri)).Verifiable();
            mockLauncher.Setup(l => l.LaunchDesktop(It.IsAny<string>())).Throws<XunitException>();
            mockWindowWrapper.Setup(wr => wr.Close()).Verifiable();
            mainWindowViewModel.LaunchVRCommand.Execute();
            mockLauncher.Verify();
            mockWindowWrapper.Verify();

            // execute LaunchDesktop Command
            mockLauncher.Setup(l => l.LaunchVR(It.IsAny<string>())).Throws<XunitException>();
            mockLauncher.Setup(l => l.LaunchDesktop(uri)).Verifiable();
            mockWindowWrapper.Setup(wr => wr.Close()).Verifiable();
            mainWindowViewModel.LaunchDesktopCommand.Execute();
            mockLauncher.Verify();
            mockWindowWrapper.Verify();
        }
    }
}
