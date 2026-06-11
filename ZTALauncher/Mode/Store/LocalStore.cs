using System.Text.Json;
using System.Text.Json.Serialization;

namespace ZTALauncher.Mode.Store
{
    public class LocalStore
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Configuration? Configuration { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Game? Game { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Mod[]? Mods {  get; set; }

        public LocalStore()
        {
            this.Configuration = new Configuration();
            this.Game = new Game();
            this.Mods = null;
        }

        public static string ToJson(LocalStore store)
        {
            return LocalStore.ToJson(store, true);
        }

        public static string ToJson(LocalStore store, bool writeIdented)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = writeIdented
            };

            return JsonSerializer.Serialize(store, options);
        }
    }
}
