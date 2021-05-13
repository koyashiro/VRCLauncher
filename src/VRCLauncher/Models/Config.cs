using System.Text.Json.Serialization;

namespace VRCLauncher.Models
{
    public class Config
    {
        [JsonPropertyName("vrchatPath")]
        public string VRChatPath { get; set; } = string.Empty;
    }
}
