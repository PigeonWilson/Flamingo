using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace ZTALauncher.Mode.Store
{
    public sealed class Configuration
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? WorkShopPath {  get; set; }
    }
}
