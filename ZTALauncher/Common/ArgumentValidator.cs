using CommonLibrary;
using System.Security.Cryptography;

namespace ZTALauncher.Common
{
    internal class ArgumentValidator : BaseObject
    {
        public static readonly string ExecutableExtension = "exe";
        public static readonly int FaultyExitCode = 1;
        public static readonly int SuccessExitCode = 2;

        private string? _executablePath;
        public string? ExecutablePath
        {
            get { return _executablePath; }
            set
            {
                if (_executablePath != value)
                {
                    _executablePath = value;
                    OnPropertyChanged(nameof(ExecutablePath));
                }
            }
        }

        public ArgumentValidator() : base()
        {
            
        }

        public static byte[]? SignFile(string path)
        {
            if (!ArgumentValidator.CheckExecutableAccessibility(path)) return null;

            byte[]? file = null;
            byte[]? signature = null;

            try
            {
                file = File.ReadAllBytes(path);
                signature = SHA512.HashData(file);
            }
            catch (Exception ex) { }
            finally
            {
                file = null;
            }

            return signature; 
        }

        public static bool CheckExecutableAccessibility(string path)
        {
            if (string.IsNullOrEmpty(path)) return false;
            var fileInfo = new FileInfo(path);
            if (!fileInfo.Exists) return false;
            if (!fileInfo.Extension.ToLower().EndsWith(ArgumentValidator.ExecutableExtension.ToLower()))
            {
                return false;
            }

            return true;
        }
    }
}
