using CommonLibrary;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Text;
using static ZTALauncher.Enums;

namespace ZTALauncher
{
    internal class Localization : BaseObject
    {
        #region localization keys
        public static class TruthSource
        {
            public static readonly string ResxResource = "LocalizationManager.resx";
            public static readonly string errorFileAccessibility = "errorFileAccessibility";
            public static readonly string exitCodeInfoMsg = "exitCodeInfoMsg";
            public static readonly string launching = "launching";
            public static readonly string missingArgument = "missingArgument";
            public static readonly string tip = "tip";
        }
        
        #endregion

        public static readonly LocalizationSupportedLanguages DefaultLocalizationLanguageCode = LocalizationSupportedLanguages.en;
        private readonly CultureInfo _cultureInfo;
        public Localization()
        {
            
            this._cultureInfo = Thread.CurrentThread.CurrentUICulture;
        }

        public LocalizationSupportedLanguages GetSupportLocalizationLanguageCode()
        {
            var cultureCode = this._cultureInfo.TwoLetterISOLanguageName.ToLower();
            LocalizationSupportedLanguages result = Localization.DefaultLocalizationLanguageCode;
            foreach (LocalizationSupportedLanguages code in Enum.GetValues(typeof(LocalizationSupportedLanguages)))
            {
                if (cultureCode == code.ToString()) result = code;
            }

            return result;
        }

        public string? GetString(string key)
        {
            var modifier = $"{GetSupportLocalizationLanguageCode().ToString()}_{key}";
            var val = RLocalizationManager.ResourceManager.GetString(modifier);
            return val;
        }

        public void ConsoleWriteLine(string[] keys, string extra)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string key in keys)
            {
               
                string val = $"{string.Empty}{key}";
                sb.Append(GetString(val));
            }

            Console.WriteLine($"{sb.ToString()} {extra}");
        }
    }
}
