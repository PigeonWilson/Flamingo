using System;
using System.Collections.Generic;
using System.Text;

namespace ZTALauncher.Mode
{
    public interface IMode
    {
        /// <summary>
        /// Detect if the current mode is detected
        /// </summary>
        /// <returns></returns>
        public bool IsMode();

        public string[]? ListMods();
        public string GetName();
        public string? GetAppId();
        public string GetWorkShopPath();
        public string? GetWorkShopPath(string executablePath);
        public string? GetExecutablePath();
    }
}
