using System;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Text.Json;
using System.Threading;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace SharpRPC
{
    public class Client
    {
        public string CONFIG_PATH { get; private set; }

        public static Client Instance { get; private set; }
        public Logger Logger { get; private set; }

        public Config Config { get; private set; }
        public PresenceManager PresenceManager { get; }
        public RemoteAssetManager RemoteAssetManager { get; set; }

        public bool LoadedPresence { get; set; } = false;

        private Client(string[] args)
        {
            Instance = this;

            KillExistingInstances();

            Logger = new Logger("Client");

            CONFIG_PATH = FindConfigPath();

            while(true)
            {
                if (ReloadConfig()) break;
                Logger.exception("Failed to initialize config, sleeping for 10 seconds before trying again . . .");
                Thread.Sleep(10000);
            }

            if(Config.applicationID == null || Config.applicationSecret == null)
            {
                Logger.exception("Missing applicationID or applicationSecret . . .");
                Environment.Exit(0);
            } else if(Config.applicationID.Length < 6 || Config.applicationSecret.Length < 4)
            {
                Logger.exception("Invalid applicationID or applicationSecret . . .");
                Environment.Exit(0);
            }

            RemoteAssetManager = new RemoteAssetManager(this);

            string maskedSecret = MaskString(Config.applicationSecret);
            Logger.debug("Loading assets for applicationID: " + Config.applicationID + " and secret: " + maskedSecret);

            RemoteAsset[] remoteAssets = RemoteAssetManager.GetAssets(Config.applicationSecret);
            Logger.debug("Found " + remoteAssets.Length + " assets");
            foreach (RemoteAsset asset in remoteAssets)
            {
                Console.WriteLine("");
                Console.WriteLine("Remote Asset with ID: " + MaskString(asset.Id));
                Console.WriteLine(" - Type: " + RemoteAssetManager.AssetTypeFromValue(asset.Type));
                Console.WriteLine(" - Name: " + asset.Name);
                Console.WriteLine("");
            }

            PresenceManager = new PresenceManager(this);
            PresenceManager.StartPresence();
        }

        public string MaskString(string secret)
        {
            string maskedString = Regex.Replace(secret, ".", "*", RegexOptions.ECMAScript);
            maskedString = maskedString.Substring(0, maskedString.Length - 4);
            int len = secret.Length;
            string maskedFinal = secret.Substring(secret.Length - 4);
            return maskedString + maskedFinal;
        }

        public bool ReloadConfig()
        {
            try
            {
                LoadConfig();
                return true;
            } catch(Exception e)
            {
                Console.Error.WriteLine("Encountered error while reloading config:\n\t" + e.Message);
                return false;
            }
        }

        static void Main(string[] args)
        {
            new Client(args);
        }

        private void KillExistingInstances()
        {
            string filePath = Assembly.GetEntryAssembly().Location;
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            Process[] existing = Process.GetProcessesByName(fileName);

            foreach(Process instance in existing)
            {
                if (instance.Id == Process.GetCurrentProcess().Id) continue;

                Logger.debug("Killing existing instance with Id: " + instance.Id);
                instance.Kill();
            }
        }

        private string FindConfigPath()
        {
            string installPath = Assembly.GetEntryAssembly().Location;
            string configInInstallPath = installPath + "/config.json";

            if(!File.Exists(configInInstallPath))
            {
                configInInstallPath = "config.json";
            }

            if(RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Logger.debug("Attempting to use config in AppData - we're on windows");

                string appDataFolder = Environment.GetEnvironmentVariable("APPDATA");
                string presenceAppData = appDataFolder + "\\SharpRPC";
                Directory.CreateDirectory(presenceAppData);

                if(!File.Exists(presenceAppData + "\\config.json"))
                {
                    Logger.debug("Copying program files config to app data . . .");
                    File.Copy(configInInstallPath, presenceAppData + "\\config.json");
                }

                return presenceAppData + "\\config.json";
            } else
            // TODO: macOS ~/Application Support config
            {
                return configInInstallPath;
            }
        }

        public void LoadConfig()
        {
            Logger.debug("Loading config . . .");
            string fileContents = File.ReadAllText(CONFIG_PATH);
            Config = JsonSerializer.Deserialize<Config>(fileContents);
        }
    }
}
