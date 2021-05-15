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
        private const string WORLD_ID = "wrld_00000000-0000-0000-0000-000000000000";
        private const string INSTANCE_ID = "00000";
        private const string INSTANCE_OWNER_ID = "usr_00000000-0000-0000-0000-000000000000";
        private const string NONCE = "0000000000000000000000000000000000000000000000000000000000000000";

        private static readonly string URI_PUBLIC = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}";
        private static readonly string URI_FRIEND_PLUS = $"{URI_PUBLIC}~hidden({INSTANCE_OWNER_ID})~nonce({NONCE})";
        private static readonly string URI_FRIEND_ONLY = $"{URI_PUBLIC}~friends({INSTANCE_OWNER_ID})~nonce({NONCE})";
        private static readonly string URI_INVITE_PLUS = $"{URI_PUBLIC}~private({INSTANCE_OWNER_ID})~canRequestInvite~nonce({NONCE})";
        private static readonly string URI_INVITE_ONLY = $"{URI_PUBLIC}~private({INSTANCE_OWNER_ID})~nonce({NONCE})";

        public static IEnumerable<object?[]> UriChanged_MemberData()
        {
            // when invalid uri
            yield return new object?[] { "invaliduri", string.Empty, string.Empty, InstanceType.Public, null, null, false };

            // when Public
            yield return new object?[] { URI_PUBLIC, WORLD_ID, INSTANCE_ID, InstanceType.Public, null, null, true };

            // when Friend Plus
            yield return new object?[] { URI_FRIEND_PLUS, WORLD_ID, INSTANCE_ID, InstanceType.FriendPlus, INSTANCE_OWNER_ID, NONCE, true };

            // when Friend Only
            yield return new object?[] { URI_FRIEND_ONLY, WORLD_ID, INSTANCE_ID, InstanceType.FriendOnly, INSTANCE_OWNER_ID, NONCE, true };

            // when Invite Plus
            yield return new object?[] { URI_INVITE_PLUS, WORLD_ID, INSTANCE_ID, InstanceType.InvitePlus, INSTANCE_OWNER_ID, NONCE, true };

            // when Invite Only
            yield return new object?[] { URI_INVITE_ONLY, WORLD_ID, INSTANCE_ID, InstanceType.InviteOnly, INSTANCE_OWNER_ID, NONCE, true };
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
