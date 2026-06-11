
namespace ZTALauncher.Common.Database.Table
{
    public sealed class DLLMod
    {
        public static readonly string TableName = "DLLMod";
        public long Id {  get; set; }
        public long GameId {  get; set; }
        public long ModId {  get; set; }
        public string? PackageName {  get; set; }
        public string? DLLPath { get; set; }
    }
}
