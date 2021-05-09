using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace VRCLauncher
{
    public class LaunchParameter
    {
        private const string REGEX_UUID = "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";
        private const string REGEX_NONCE = "[0-9A-F]{64}";

        private LaunchParameter(
            string worldId,
            string instanceId,
            InstanceType instanceType,
            string? instanceOwnerId,
            string nonce
        )
        {
            WorldId = worldId;
            InstanceId = instanceId;
            InstanceType = instanceType;
            InstanceOwnerId = instanceOwnerId;
            Nonce = nonce;
        }

        public string WorldId { get; set; }
        public string InstanceId { get; set; }
        public InstanceType InstanceType { get; set; }
        public string? InstanceOwnerId { get; set; }
        public string Nonce { get; set; }

        public static bool TryParse(string arg, [MaybeNullWhen(false)] out LaunchParameter launchParameter)
        {
            if (!TryParseWorldIdAndInstanceId(arg, out var worldId, out var instanceId))
            {
                launchParameter = default;
                return false;
            }

            var (instanceType, instanceOwnerId) = ParseInstanceTypeAndInstanceOwnerId(arg);

            if (!TryParseNonce(arg, out var nonce))
            {
                launchParameter = default;
                return false;
            }

            launchParameter = new LaunchParameter(worldId, instanceId, instanceType, instanceOwnerId, nonce);
            return true;
        }

        private static bool TryParseWorldIdAndInstanceId(string arg, [MaybeNullWhen(false)] out string worldId, [MaybeNullWhen(false)] out string instanceId)
        {
            const string GROUP_NAME_WORLD_ID = "worldId";
            const string GROUP_NAME_INSTANCE_ID = "instanceId";

            var match = Regex.Match(arg, $@"[&?]id=(?<{GROUP_NAME_WORLD_ID}>wrld_{REGEX_UUID}):(?<{GROUP_NAME_INSTANCE_ID}>[0-9A-z]+)");
            if (!match.Success)
            {
                worldId = default;
                instanceId = default;
                return false;
            }

            worldId = match.Groups[GROUP_NAME_WORLD_ID].Value;
            instanceId = match.Groups[GROUP_NAME_INSTANCE_ID].Value;
            return true;
        }

        private static (InstanceType instanceType, string? instanceOwnerId) ParseInstanceTypeAndInstanceOwnerId(string arg)
        {
            var GROUP_NAME_USER_ID = "userId";

            // Invite Only, Invite Plus
            var privateMatch = Regex.Match(arg, $@"~private\((?<{GROUP_NAME_USER_ID}>usr_{REGEX_UUID})\)");
            if (privateMatch.Success)
            {
                // Invite Plus
                var invitePlusMatch = Regex.Match(arg, "~canRequestInvite");
                if (invitePlusMatch.Success)
                {
                    return (InstanceType.InvitePlus, privateMatch.Groups[GROUP_NAME_USER_ID].Value);
                }
                // Invite Only
                else
                {
                    return (InstanceType.InviteOnly, privateMatch.Groups[GROUP_NAME_USER_ID].Value);
                }
            }

            // Friend Plus
            var hiddenMatch = Regex.Match(arg, $@"~hidden\((?<{GROUP_NAME_USER_ID}>usr_{REGEX_UUID})\)");
            if (hiddenMatch.Success)
            {
                return (InstanceType.FriendPlus, hiddenMatch.Groups[GROUP_NAME_USER_ID].Value);
            }

            // Friend Only
            var friendsMatch = Regex.Match(arg, $@"~friends\((?<{GROUP_NAME_USER_ID}>usr_{REGEX_UUID})\)");
            if (friendsMatch.Success)
            {
                return (InstanceType.FriendOnly, friendsMatch.Groups[GROUP_NAME_USER_ID].Value);
            }

            // Public
            return (InstanceType.Public, null);
        }

        private static bool TryParseNonce(string arg, [MaybeNullWhen(false)] out string nonce)
        {
            var GROUP_NAME_NONCE = "userId";

            var match = Regex.Match(arg, $@"~nonce\((?<{GROUP_NAME_NONCE}>{REGEX_NONCE})\)");
            if (!match.Success)
            {
                nonce = null;
                return false;
            }

            nonce = match.Groups[GROUP_NAME_NONCE].Value;
            return true;
        }
    }
}
