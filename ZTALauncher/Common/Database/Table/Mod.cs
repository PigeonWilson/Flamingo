namespace ZTALauncher.Common.Database.Table
{
    public sealed class Mod
    {
        public static readonly string TableName = "Mod";
        public long Id { get; set; }
        public long GameId {  get; set; }
        public string? ModName { get; set; }
        public string? PackageName {  get; set; }
    }
}
