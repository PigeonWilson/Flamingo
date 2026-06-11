using ZTALauncher.Mode;
using ZTALauncher.Mode.Steam.Model;
using ZTALauncher.Mode.Store;
using static ZTALauncher.Enums;

namespace ZTALauncher
{
    public sealed class ZTAContext
    {
        public bool IsSet {  get; private set; }
        public Localization Localization { get; private set; }
        public SupportedModes SelectedMode { get; private set; }
        public IMode? Mode {  get; private set; }
        public LocalStore LocalStore { get; private set; }
        public string? ExecutablePath {  get; private set; }

        private static readonly Lazy<ZTAContext> lazy =
            new Lazy<ZTAContext>(() => new ZTAContext());

        public static ZTAContext Instance { get { return lazy.Value; } }

        private ZTAContext() 
        {
            this.IsSet = false;
            this.SelectedMode = SupportedModes.Vanilla;
            this.LocalStore = new LocalStore();
            this.Localization = new Localization();
        }

        public void Set(SupportedModes mode, string executablePath)
        {
            this.SelectedMode = mode;
            this.ExecutablePath = executablePath;
            this.IsSet = true;
        }

        private List<string> _filesListing = new List<string>();
        private void ListFiles(string path)
        {

            foreach (string file in Directory.GetFiles(path))
            {
                if (file.ToLower().EndsWith("dll"))
                {
                    this._filesListing.Add(file);
                }
            }

            foreach (string directory in Directory.GetDirectories(path))
            {
                ListFiles(directory);
            }
        }

        public void LoadMode()
        {
            if (!this.IsSet) return;
            if (!ArgumentValidator.CheckExecutableAccessibility(this.ExecutablePath)) return;

            this.Mode = this.SelectedMode switch
            {
                SupportedModes.Vanilla => new ZTALauncher.Mode.Vanilla.Mode(this.ExecutablePath),
                SupportedModes.Steam => new ZTALauncher.Mode.Steam.Mode(this.ExecutablePath),
                _ => new ZTALauncher.Mode.Vanilla.Mode(this.ExecutablePath),
            };
            this.LocalStore.Configuration = new Configuration() 
            { 
                WorkShopPath = this.Mode.GetWorkShopPath(this.Mode.GetAppId())
            };

            byte[]? signature = ArgumentValidator.SignFile(this.ExecutablePath);

            this.LocalStore.Game = new Game()
            {
                ExecutableName = this.ExecutablePath,
                ApplicationId = this.Mode.GetAppId()
            };

            if (signature != null) 
            {
                this.LocalStore.Game.ExecutableSignature = Convert.ToBase64String(signature);
            }

            string[]? modsList = this.Mode.ListMods();
            List<Mod> tmpList = new List<Mod>();
            if (modsList != null)
            {
                foreach (string mod in modsList)
                {
                    string aboutFilePath = Path.Combine(mod, "About", "About.xml");
                    string xml = File.ReadAllText(aboutFilePath);
                    dynamic result = DynamicXml.Parse(xml);

                    this._filesListing.Clear();
                    this.ListFiles(mod); ;
                    Mod _mod = new Mod()
                    {
                        ModName = result.name,
                        PackageName = result.packageId,
                        DLLPath = this._filesListing.ToArray()
                    };
                    tmpList.Add(_mod);
                }

                this.LocalStore.Mods = tmpList.ToArray();
            }
            
        }
    }
}
