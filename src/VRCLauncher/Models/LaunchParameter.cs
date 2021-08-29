using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace VRCLauncher.Models
{
    public class LaunchParameter
    {
        private const string REGEX_UUID = "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";
        private const string REGEX_WORLD_ID = "wrld_[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";
        private const string REGEX_USER_ID = "usr_[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}";
        private const string REGEX_REGION = "[a-z]+";
        private const string REGEX_NONCE = "[0-9A-z\\-]+";

        private static Dictionary<string, Region> RegionDictionary { get; } = new()
        {
            { "us", Region.US },
            { "eu", Region.EU },
            { "jp", Region.JP },
        };

        public string WorldId { get; set; } = string.Empty;
        public string InstanceId { get; set; } = string.Empty;
        public InstanceType InstanceType { get; set; } = InstanceType.Public;
        public string? InstanceOwnerId { get; set; }
        public Region Region { get; set; }
        public string? Nonce { get; set; }

        public bool IsValid()
        {
            if (InstanceType == InstanceType.Public)
            {
                return IsValidWorldId(WorldId)
                    && IsValidInstanceType(InstanceType)
                    && IsValidInstanceId(InstanceId)
                    && IsValidRegion(Region);
            }
            else
            {
                return IsValidWorldId(WorldId)
                    && IsValidInstanceId(InstanceId)
                    && IsValidInstanceType(InstanceType)
                    && IsValidUserId(InstanceOwnerId)
                    && IsValidNonce(Nonce)
                    && IsValidRegion(Region);
            }
        }

        private static bool IsValidWorldId(string worldId)
        {
            if (worldId.Length is 0)
            {
                return false;
            }

            return Regex.IsMatch(worldId, $"^{REGEX_WORLD_ID}$");
        }

        private static bool IsValidInstanceId(string instanceId)
        {
            return instanceId.Length is not 0;
        }

        private static bool IsValidInstanceType(InstanceType instanceType)
        {
            return Enum.IsDefined(instanceType);
        }

        private static bool IsValidUserId(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return false;
            }

            return Regex.IsMatch(userId, $"^{REGEX_USER_ID}$");
        }

        private static bool IsValidNonce(string? nonce)
        {
            if (string.IsNullOrEmpty(nonce))
            {
                return false;
            }

            return Regex.IsMatch(nonce, $"^{REGEX_NONCE}$");
        }

        private static bool IsValidRegion(Region region)
        {
            return Enum.IsDefined(region);
        }

        public override string ToString()
        {
            if (!IsValid())
            {
                return string.Empty;
            }

            var URI_PUBLIC = $"vrchat://launch/?ref=vrchat.com&id={WorldId}:{InstanceId}";

            var region = Region == Region.None ? string.Empty : $"~region{Region.ToString().ToLower()}";

            return InstanceType switch
            {
                InstanceType.Public => $"{URI_PUBLIC}{region}",
                InstanceType.FriendPlus => $"{URI_PUBLIC}~hidden({InstanceOwnerId}){region}~nonce({Nonce})",
                InstanceType.FriendOnly => $"{URI_PUBLIC}~friends({InstanceOwnerId}){region}~nonce({Nonce})",
                InstanceType.InvitePlus => $"{URI_PUBLIC}~private({InstanceOwnerId})~canRequestInvite{region}~nonce({Nonce})",
                InstanceType.InviteOnly => $"{URI_PUBLIC}~private({InstanceOwnerId}){region}~nonce({Nonce})",
                _ => string.Empty,
            };
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

            var region = ParseRegion(arg);

            if (!TryParseNonce(arg, out var nonce))
            {
                if (instanceType != InstanceType.Public)
                {
                    launchParameter = default;
                    return false;
                }
            }

            launchParameter = new LaunchParameter
            {
                WorldId = worldId,
                InstanceId = instanceId,
                InstanceType = instanceType,
                InstanceOwnerId = instanceOwnerId,
                Region = region,
                Nonce = nonce,
            };
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
            const string GROUP_NAME_USER_ID = "userId";

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

        private static Region ParseRegion(string arg)
        {
            const string GROUP_NAME_REGION = "region";

            var match = Regex.Match(arg, $@"~region\((?<{GROUP_NAME_REGION}>{REGEX_REGION})\)(~|$)");
            if (!match.Success)
            {
                return Region.None;
            }

            if (!RegionDictionary.TryGetValue(match.Groups[GROUP_NAME_REGION].Value, out var region))
            {
                return Region.None;
            }

            return region;
        }

        private static bool TryParseNonce(string arg, [MaybeNullWhen(false)] out string nonce)
        {
            const string GROUP_NAME_NONCE = "userId";

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

    public enum Region
    {
        None,
        US,
        EU,
        JP
    }
}
