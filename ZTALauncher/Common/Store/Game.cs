using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ZTALauncher.Common.Store
{
    public sealed class Game
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExecutableName {  get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ExecutableSignature { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ApplicationId {  get; set; }
    }
}