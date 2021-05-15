using System.Collections.Generic;
using VRCLauncher.Models;
using Xunit;
using Xunit.Sdk;

namespace VRCLauncher.Test.Models
{
    public class LaunchParameterTest
    {
        public static IEnumerable<object?[]> LaunchParameterToUri_MemberData()
        {
            // when Public
            yield return new object?[] { InstanceType.Public, TestConstantValue.URI_PUBLIC };

            // when Friend Plus
            yield return new object?[] { InstanceType.FriendPlus, TestConstantValue.URI_FRIEND_PLUS };

            // when Friend Only
            yield return new object?[] { InstanceType.FriendOnly, TestConstantValue.URI_FRIEND_ONLY };

            // when Invite Plus
            yield return new object?[] { InstanceType.InvitePlus, TestConstantValue.URI_INVITE_PLUS };

            // when Invite Only
            yield return new object?[] { InstanceType.InviteOnly, TestConstantValue.URI_INVITE_ONLY };
        }

        [Theory]
        [MemberData(nameof(LaunchParameterToUri_MemberData))]
        public static void LaunchParameterToUri(InstanceType instanceType, string uri)
        {
            var launchParameter = new LaunchParameter
            {
                InstanceType = instanceType,
            };
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            // when WorldId is invalid
            launchParameter.WorldId = string.Empty;
            launchParameter.InstanceId = TestConstantValue.INSTANCE_ID;
            launchParameter.InstanceOwnerId = TestConstantValue.INSTANCE_OWNER_ID;
            launchParameter.Nonce = TestConstantValue.NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            // when InstanceId is invalid
            launchParameter.WorldId = TestConstantValue.WORLD_ID;
            launchParameter.InstanceId = string.Empty;
            launchParameter.InstanceOwnerId = TestConstantValue.INSTANCE_OWNER_ID;
            launchParameter.Nonce = TestConstantValue.NONCE;
            Assert.False(launchParameter.IsValid());
            Assert.Equal(string.Empty, launchParameter.ToString());

            if (instanceType != InstanceType.Public)
            {
                // when InstanceOwnerId is invalid
                launchParameter.WorldId = TestConstantValue.WORLD_ID;
                launchParameter.InstanceId = TestConstantValue.INSTANCE_ID;
                launchParameter.InstanceOwnerId = string.Empty;
                launchParameter.Nonce = TestConstantValue.NONCE;
                Assert.False(launchParameter.IsValid());
                Assert.Equal(string.Empty, launchParameter.ToString());

                // when Nonce is invalid
                launchParameter.WorldId = TestConstantValue.WORLD_ID;
                launchParameter.InstanceId = TestConstantValue.INSTANCE_ID;
                launchParameter.InstanceOwnerId = TestConstantValue.INSTANCE_OWNER_ID;
                launchParameter.Nonce = string.Empty;
                Assert.False(launchParameter.IsValid());
                Assert.Equal(string.Empty, launchParameter.ToString());
            }

            // when parameters are valid
            launchParameter.WorldId = TestConstantValue.WORLD_ID;
            launchParameter.InstanceId = TestConstantValue.INSTANCE_ID;
            launchParameter.InstanceOwnerId = TestConstantValue.INSTANCE_OWNER_ID;
            launchParameter.Nonce = TestConstantValue.NONCE;
            Assert.True(launchParameter.IsValid());
            Assert.Equal(uri, launchParameter.ToString());
        }

        public static IEnumerable<object?[]> UriToLaunchParameter_MemberData()
        {
            // when uri is null
            yield return new object?[] { null, null };

            // when uri is string.Empty
            yield return new object?[] { string.Empty, null };

            // when urhiddeni is invalid
            yield return new object?[] { "wrong uri", null };

            // when WorldId is invalid
            yield return new object?[]
            {
                $"vrchat://launch/?ref=vrchat.com&id=invalid-world-id:{TestConstantValue.INSTANCE_ID}",
                null,
            };

            // when Friend Plus and Nonce does not exists
            yield return new object?[]
            {
                $"{TestConstantValue.URI_PUBLIC}~hidden({TestConstantValue.INSTANCE_OWNER_ID})",
                null,
            };

            // when Friend Only and Nonce does not exists
            yield return new object?[]
            {
                $"{TestConstantValue.URI_PUBLIC}~friends({TestConstantValue.INSTANCE_OWNER_ID})",
                null,
            };

            // when Invite Plus and Nonce does not exists
            yield return new object?[]
            {
                $"{TestConstantValue.URI_PUBLIC}~private({TestConstantValue.INSTANCE_OWNER_ID})",
                null,
            };

            // when Invite Only and Nonce does not exists
            yield return new object?[]
            {
                $"{TestConstantValue.URI_PUBLIC}~private({TestConstantValue.INSTANCE_OWNER_ID})~canRequestInvite",
                null,
            };

            // when Public
            yield return new object?[]
            {
                TestConstantValue.URI_PUBLIC,
                new LaunchParameter
                {
                    WorldId = TestConstantValue.WORLD_ID,
                    InstanceId = TestConstantValue.INSTANCE_ID,
                    InstanceType = InstanceType.Public,
                },
            };

            // when Friend Plus
            yield return new object?[]
            {
                TestConstantValue.URI_FRIEND_PLUS,
                new LaunchParameter
                {
                    WorldId = TestConstantValue.WORLD_ID,
                    InstanceId = TestConstantValue.INSTANCE_ID,
                    InstanceType = InstanceType.FriendPlus,
                    InstanceOwnerId = TestConstantValue.INSTANCE_OWNER_ID,
                    Nonce = TestConstantValue.NONCE,
                },
            };

            // when Friend Only
            yield return new object?[]
            {
                TestConstantValue.URI_FRIEND_ONLY,
                new LaunchParameter
                {
                    WorldId = TestConstantValue.WORLD_ID,
                    InstanceId = TestConstantValue.INSTANCE_ID,
                    InstanceType = InstanceType.FriendOnly,
                    InstanceOwnerId = TestConstantValue.INSTANCE_OWNER_ID,
                    Nonce = TestConstantValue.NONCE,
                },
            };

            // when Invite Plus
            yield return new object?[]
            {
                TestConstantValue.URI_INVITE_PLUS,
                new LaunchParameter
                {
                    WorldId = TestConstantValue.WORLD_ID,
                    InstanceId = TestConstantValue.INSTANCE_ID,
                    InstanceType = InstanceType.InvitePlus,
                    InstanceOwnerId = TestConstantValue.INSTANCE_OWNER_ID,
                    Nonce = TestConstantValue.NONCE,
                },
            };

            // when Invite Only
            yield return new object?[]
            {
                TestConstantValue.URI_INVITE_ONLY,
                new LaunchParameter
                {
                    WorldId = TestConstantValue.WORLD_ID,
                    InstanceId = TestConstantValue.INSTANCE_ID,
                    InstanceType = InstanceType.InviteOnly,
                    InstanceOwnerId = TestConstantValue.INSTANCE_OWNER_ID,
                    Nonce = TestConstantValue.NONCE,
                },
            };
        }

        [Theory]
        [MemberData(nameof(UriToLaunchParameter_MemberData))]
        public static void UriToLaunchParameter(string uri, LaunchParameter? launchParameter)
        {
            Assert.Equal(launchParameter is not null, LaunchParameter.TryParse(uri, out var parsedLaunchParameter));

            if (launchParameter is null)
            {
                Assert.Null(parsedLaunchParameter);
            }
            else
            {
                if (parsedLaunchParameter is null)
                {
                    throw new NotNullException();
                }

                Assert.Equal(launchParameter.WorldId, parsedLaunchParameter.WorldId);
                Assert.Equal(launchParameter.InstanceId, parsedLaunchParameter.InstanceId);
                Assert.Equal(launchParameter.InstanceType, parsedLaunchParameter.InstanceType);
                Assert.Equal(launchParameter.InstanceOwnerId, parsedLaunchParameter.InstanceOwnerId);
                Assert.Equal(launchParameter.Nonce, parsedLaunchParameter.Nonce);
            }
        }
    }
}
