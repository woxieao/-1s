using Newtonsoft.Json;
using System.IO;

namespace IAddOneSecondForElder.Models
{
    public class Config
    {
        public const string ConfigPath = "config.json";
        private Config()
        {
        }
        public const int DefaultIntervalSeconds = 60 * 5;
        public static readonly Config Instance = new Config().LoadConfig();
        private int _intervalSeconds = DefaultIntervalSeconds;



        public Config LoadConfig()
        {
            try
            {
                var config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(ConfigPath));
                WriteConfig(config);
            }
            catch
            {
                //配置异常用默认配置
                WriteConfig(this);
            }
            return this;
        }

        public void WriteConfig(Config config)
        {
            this.ColorTheme = config.ColorTheme;
            this.AudioPath = config.AudioPath;
            this.IntervalSeconds = config.IntervalSeconds;
            File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(config));
        }

        public ColorTheme ColorTheme { get; set; } = ColorTheme.Auto;

        public string AudioPath { get; set; } = @"Audio\slient.wav";

        public int IntervalSeconds
        {
            get => _intervalSeconds < 1 ? DefaultIntervalSeconds : _intervalSeconds;

            set => _intervalSeconds = value;
        }
    }
}
