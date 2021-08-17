using Newtonsoft.Json;
using System.IO;

namespace IAddOneSecondForElder.Models
{
    public class Config
    {
        public const string ConfigPath = "config.json";
        private readonly static Config _config = new Config().LoadConfig();
        public readonly static Config Instance = _config;
        private Config()
        {
        }

        public Config LoadConfig()
        {
            try
            {
                var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
                WriteConfig(config);
            }
            catch
            {
                WriteConfig(this);
            }
            return this;
        }

        public void WriteConfig(Config config)
        {
            this.ColorTheme = config.ColorTheme;
            this.AudioPath = config.AudioPath;
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(config));
        }

        public ColorTheme ColorTheme { get; set; } = ColorTheme.Auto;
        public string AudioPath { get; set; } = @"Audio\slient.wav";

    }
}
