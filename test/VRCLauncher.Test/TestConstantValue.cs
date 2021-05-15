using System;
using System.IO;
using System.Text.Json;
using VRCLauncher.Models;

namespace VRCLauncher.Test
{
    public static class TestConstantValue
    {
        public static readonly string LOCAL_APPLICATION_DATA = @"C:\Users\VRCLauncher\AppData\Local";

        public static readonly string VRCHAT_BIN_NAME = "VRChat.exe";
        public static readonly string CONFIG_DIRECTORY_BASE_NAME = "VRCLauncher";
        public static readonly string CONFIG_FILE_NAME = "config.json";

        public static readonly string CONFIG_DIRECTORY_PATH = Path.Join(LOCAL_APPLICATION_DATA, CONFIG_DIRECTORY_BASE_NAME);
        public static readonly string CONFIG_FILE_PATH = Path.Join(CONFIG_DIRECTORY_PATH, CONFIG_FILE_NAME);

        public static readonly string DEFAULT_VRCHAT_PATH = $@"C:\Program Files (x86)\Steam\steamapps\common\VRChat\{VRCHAT_BIN_NAME}";
        public static readonly Config DEFAULT_CONFIG = new() { VRChatPath = DEFAULT_VRCHAT_PATH };
        public static readonly string DEFAULT_CONFIG_JSON = JsonSerializer.Serialize(DEFAULT_CONFIG, new JsonSerializerOptions { WriteIndented = true });

        public static readonly string TEST_VRCHAT_PATH = @"C:\tmp\VRChat.exe";
        public static readonly Config TEST_CONFIG = new() { VRChatPath = TEST_VRCHAT_PATH };
        public static readonly string TEST_CONFIG_JSON = JsonSerializer.Serialize(TEST_CONFIG, new JsonSerializerOptions { WriteIndented = true });

        public static readonly string WORLD_ID = "wrld_00000000-0000-0000-0000-000000000000";
        public static readonly string INSTANCE_ID = "00000";
        public static readonly string INSTANCE_OWNER_ID = "usr_00000000-0000-0000-0000-000000000000";
        public static readonly string NONCE = "0000000000000000000000000000000000000000000000000000000000000000";

        public static readonly string URI_PUBLIC = $"vrchat://launch/?ref=vrchat.com&id={WORLD_ID}:{INSTANCE_ID}";
        public static readonly string URI_FRIEND_PLUS = $"{URI_PUBLIC}~hidden({INSTANCE_OWNER_ID})~nonce({NONCE})";
        public static readonly string URI_FRIEND_ONLY = $"{URI_PUBLIC}~friends({INSTANCE_OWNER_ID})~nonce({NONCE})";
        public static readonly string URI_INVITE_PLUS = $"{URI_PUBLIC}~private({INSTANCE_OWNER_ID})~canRequestInvite~nonce({NONCE})";
        public static readonly string URI_INVITE_ONLY = $"{URI_PUBLIC}~private({INSTANCE_OWNER_ID})~nonce({NONCE})";
    }
}
