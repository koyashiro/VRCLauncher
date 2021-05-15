using VRCLauncher.Models;
using Xunit;

namespace VRCLauncher.Test.Models
{
    public class LaunchParameterTest
    {
        private const string WORLD_ID = "wrld_00000000-0000-0000-0000-000000000000";
        private const string INSTANCE_ID = "00000";
        private const string INSTANCE_OWNER_ID = "usr_00000000-0000-0000-0000-000000000000";
        private const string NONCE = "0000000000000000000000000000000000000000000000000000000000000000";

        [Fact]
        public void IsValidTest_Public()
        {
            var launchParameter = new LaunchParameter
            {
                InstanceType = InstanceType.Public
            };
            Assert.False(launchParameter.IsValid());

            launchParameter.WorldId = string.Empty;
            launchParameter.InstanceId = INSTANCE_ID;
            Assert.False(launchParameter.IsValid());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = string.Empty;
            Assert.False(launchParameter.IsValid());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            Assert.True(launchParameter.IsValid());
        }

        [Fact]
        public void IsValidTest_FriendPlus()
        {
            var launchParameter = new LaunchParameter
            {
                InstanceType = InstanceType.FriendPlus
            };
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = string.Empty;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = string.Empty;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = string.Empty;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = string.Empty;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.True(launchParameter.IsValid());
            Assert.Equal($"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~hidden({INSTANCE_OWNER_ID})~nonce({NONCE})", launchParameter.ToString());
        }

        [Fact]
        public void IsValidTest_FriendOnly()
        {
            var launchParameter = new LaunchParameter
            {
                InstanceType = InstanceType.FriendOnly
            };
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = string.Empty;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = string.Empty;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = string.Empty;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = string.Empty;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.True(launchParameter.IsValid());
            Assert.Equal($"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~friends({INSTANCE_OWNER_ID})~nonce({NONCE})", launchParameter.ToString());
        }

        [Fact]
        public void IsValidTest_InvitePlus()
        {
            var launchParameter = new LaunchParameter
            {
                InstanceType = InstanceType.InvitePlus
            };
            Assert.False(launchParameter.IsValid());

            launchParameter.WorldId = string.Empty;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = string.Empty;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = string.Empty;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = string.Empty;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.True(launchParameter.IsValid());
            Assert.Equal($"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~private({INSTANCE_OWNER_ID})~canRequestInvite~nonce({NONCE})", launchParameter.ToString());
        }

        [Fact]
        public void IsValidTest_InviteOnly()
        {
            var launchParameter = new LaunchParameter
            {
                InstanceType = InstanceType.InviteOnly
            };
            Assert.False(launchParameter.IsValid());

            launchParameter.WorldId = string.Empty;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = string.Empty;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = string.Empty;
            launchParameter.Nonce = NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = string.Empty;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            launchParameter.WorldId = WORLD_ID;
            launchParameter.InstanceId = INSTANCE_ID;
            launchParameter.InstanceOwnerId = INSTANCE_OWNER_ID;
            launchParameter.Nonce = NONCE;
            Assert.True(launchParameter.IsValid());
            Assert.Equal($"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}~private({INSTANCE_OWNER_ID})~nonce({NONCE})", launchParameter.ToString());
        }
    }
}
