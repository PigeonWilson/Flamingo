using System;
using System.Collections.Generic;
using System.Text;

namespace ZTALauncher.Mode.Steam
{
    public class Mode : IMode
    {
        public static readonly string ModeName = "Steam";
        public static readonly string WorkShopPath = @"C:\Program Files (x86)\Steam\steamapps\workshop\content\";
        public static readonly string SteamAppIdFile = "steam_appid.txt";
        public string? GameWorkshop { get; private set; }
        public string? ApplicationId { get; private set; }
        public string ExecutablePath {  get; private set; }

        public string GetName()
        {
            return ModeName;
        }

        public string? GetExecutablePath()
        {
            return this.ExecutablePath;
        }

        public string GetWorkShopPath()
        {
            return GetWorkShopPath(this.ApplicationId);
        }

        public string? GetWorkShopPath(string applicationId)
        {
            
            return Path.Combine(WorkShopPath, applicationId);
        }
        

        public bool IsMode()
        {
            string workingDirectory = Path.GetDirectoryName(this.ExecutablePath);
            return File.Exists(Path.Combine(workingDirectory, Mode.SteamAppIdFile));
        }

        public bool IsMode(string executablePath)
        {
            string workingDirectory = Path.GetDirectoryName(executablePath);
            string filePath = Path.Combine(workingDirectory, Mode.SteamAppIdFile);
            bool val = File.Exists(filePath);
            return val;
        }

        public Mode(string executablePath)
        {
            this.ApplicationId = null;
            if (IsMode(executablePath))
            {
                if (!string.IsNullOrEmpty(this.ExecutablePath))
                {
                    string? workingDirectory = Path.GetDirectoryName(this.ExecutablePath);
                    if (!string.IsNullOrEmpty(workingDirectory))
                    {
                        this.ApplicationId = File.ReadAllText(Path.Combine(workingDirectory, Mode.SteamAppIdFile));
                        this.GameWorkshop = Path.Combine(WorkShopPath, this.ApplicationId);
                    }
                }

                this.ExecutablePath = executablePath;
            }
        }

        public string? GetAppId()
        {
            return this.GetAppId(this.ExecutablePath);
        }

        public string? GetAppId(string executablePath)
        {
            string workingDirectory = Path.GetDirectoryName(executablePath);
            string filePath = Path.Combine(workingDirectory, Mode.SteamAppIdFile);
            return File.ReadAllText(filePath);
        }

        public string[]? ListMods()
        {
            if (!IsMode()) return null;
            var dir = GetWorkShopPath(this.GetAppId());
            string[]? val = [.. Directory.GetDirectories(dir)];
            return val;
        }
    }
}
