using System.IO;

namespace IAddOneSecondForElder
{
    public class Config
    {
        private const char Flag = '-';
        public const string ConfigPath = "config.txt";
        public Config()
        {
            LoadConfig();
        }
        public void LoadConfig()
        {
            Frequency = 100;
            Interval = 59 * 1000 * 5 - 1;
            if (File.Exists(ConfigPath))
            {
                var lines = File.ReadAllLines(ConfigPath);
                if (lines.Length >= 2)
                {
                    int.TryParse(lines[0].Split(Flag)[0], out var frequency);
                    int.TryParse(lines[1].Split(Flag)[0], out var interval);
                    Frequency = (frequency > 32767 || frequency < 37) ? 1000 : frequency;
                    Interval = interval < 0 ? 1000 : interval;
                }
            }

            File.WriteAllLines(ConfigPath, new[] { $"{Frequency}{Flag}声音频率(HZ),越高越尖锐", $"{Interval}{Flag}运行时间间隔(ms)", "重启应用程序后配置文件生效" });

        }


        public int Frequency { get; private set; }
        public int Interval { get; private set; }
    }
}
