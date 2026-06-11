namespace ZTALauncher.Mode.Vanilla
{
    public sealed class Mode : IMode
    {
        public static readonly string ModeName = "vanilla";

        public string? GetAppId()
        {
            return null;
        }

        public string? GetWorkShopPath(string executablePath)
        {
            return null;
        }

        public string ExecutablePath { get; private set; }

        public string? GetExecutablePath()
        {
            return this.ExecutablePath;
        }

        public string GetWorkShopPath()
        {
            return string.Empty;
        }

        public string GetName()
        {
            return ModeName;
        }

        public bool IsMode()
        {
            return true;
        }

        public string[]? ListMods()
        {
            return null;
        }

        public Mode(string executablePath)
        {
            this.ExecutablePath = executablePath;  
        }
    }
}
