using System.Text.Json.Serialization;

namespace ZTALauncher.Common.Store
{
    public sealed class Mod
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ModName {  get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? PackageName {  get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string[]? DLLPath { get; set; }
    }
}
