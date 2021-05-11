using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace VRCLauncher.Models
{
    public class LaunchParameter
    {
        private static readonly string REGEX_UUID = "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";
        private static readonly string REGEX_WORLD_ID = $"wrld_{REGEX_UUID}";
        private static readonly string REGEX_USER_ID = $"usr_{REGEX_UUID}";
        private static readonly string REGEX_NONCE = "[0-9A-z]+";

        public LaunchParameter(
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

        public bool IsValid()
        {
            return IsValidWorldId(WorldId)
                && IsValidInstanceType(InstanceType)
                && IsValidUserId(InstanceOwnerId)
                && IsValidNonce(Nonce);
        }

        private static bool IsValidWorldId(string worldId)
        {
            return Regex.IsMatch(worldId, REGEX_WORLD_ID);
        }

        private static bool IsValidInstanceType(InstanceType instanceType)
        {
            return Enum.IsDefined(instanceType);
        }

        private static bool IsValidUserId(string? userId)
        {
            if (userId is null) return false;
            return Regex.IsMatch(userId, REGEX_USER_ID);
        }

        private static bool IsValidNonce(string nonce)
        {
            return Regex.IsMatch(nonce, REGEX_NONCE);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append($"vrchat://launch/?ref=vrchat.com&id={WorldId}:{InstanceId}");

            switch (InstanceType)
            {
                case InstanceType.FriendPlus:
                    sb.Append($"~hidden({InstanceOwnerId})");
                    break;
                case InstanceType.FriendOnly:
                    sb.Append($"~friends({InstanceOwnerId})");
                    break;
                case InstanceType.InvitePlus:
                case InstanceType.InviteOnly:
                    sb.Append($"~private({InstanceOwnerId})");
                    break;
            }

            if (InstanceType != InstanceType.Public)
            {
                sb.Append($"~nonce({Nonce})");
            }

            if (InstanceType == InstanceType.InvitePlus)
            {
                sb.Append("~canRequestInvite");
            }

            return sb.ToString();
        }

        public static bool TryParse(string? arg, [MaybeNullWhen(false)] out LaunchParameter launchParameter)
        {
            if (string.IsNullOrEmpty(arg))
            {
                launchParameter = default;
                return false;
            }

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

            var match = Regex.Match(arg, $@"[&?]id=(?<{GROUP_NAME_WORLD_ID}>{REGEX_WORLD_ID}):(?<{GROUP_NAME_INSTANCE_ID}>[0-9A-z]+)");
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
            var privateMatch = Regex.Match(arg, $@"~private\((?<{GROUP_NAME_USER_ID}>{REGEX_USER_ID})\)(~|$)");
            if (privateMatch.Success)
            {
                // Invite Plus
                if (Regex.IsMatch(arg, "~canRequestInvite(~|$)"))
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
            var hiddenMatch = Regex.Match(arg, $@"~hidden\((?<{GROUP_NAME_USER_ID}>{REGEX_USER_ID})\)(~|$)");
            if (hiddenMatch.Success)
            {
                return (InstanceType.FriendPlus, hiddenMatch.Groups[GROUP_NAME_USER_ID].Value);
            }

            // Friend Only
            var friendsMatch = Regex.Match(arg, $@"~friends\((?<{GROUP_NAME_USER_ID}>{REGEX_USER_ID})\)(~|$)");
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

            var match = Regex.Match(arg, $@"~nonce\((?<{GROUP_NAME_NONCE}>{REGEX_NONCE})\)(~|$)");
            if (match.Success)
            {
                nonce = match.Groups[GROUP_NAME_NONCE].Value;
                return true;
            }

            match = Regex.Match(arg, $@"~nonce\((?<{GROUP_NAME_NONCE}>{REGEX_UUID})\)(~|$)");
            if (match.Success)
            {
                nonce = match.Groups[GROUP_NAME_NONCE].Value;
                return true;
            }

            nonce = null;
            return false;
        }
    }

    public enum InstanceType
    {
        Public,
        FriendPlus,
        FriendOnly,
        InvitePlus,
        InviteOnly,
    }
}
