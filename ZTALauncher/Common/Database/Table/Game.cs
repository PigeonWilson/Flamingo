namespace ZTALauncher.Common.Database.Table
{
    public sealed class Game
    {
        public static readonly string TableName = "Game";
        public long Id { get; set; }
        public string? ExecutableName { get; set; }
        public string? ExecutableSignature {  get; set; }
        public string? ApplicationId { get; set; }  
    }
}
