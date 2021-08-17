using IAddOneSecondForElder.Models;
using OpenHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

namespace IAddOneSecondForElder
{
    public class Core
    {

        private static readonly Computer Computer = new Computer
        {
            CPUEnabled = true,
        };

        static Core()
        {
            Computer.Open();
        }
        public static void Beep()
        {
            try
            {
                new System.Media.SoundPlayer(Config.Instance.AudioPath).Play();
            }
            catch
            {
                Console.Beep(40, 500);
            }
        }
        public static Color GetDisplayColor(double temperature)
        {
            var autoChangeColorArr = new[] { Color.Green, Color.Blue, Color.OrangeRed, Color.Red };
            var lightColorArr = new[] { Color.White, Color.White, Color.White, Color.White };
            var darkColorArr = new[] { Color.Black, Color.Black, Color.Black, Color.Black };
            Color[] colorArr;
            switch (Config.Instance.ColorTheme)
            {
                case ColorTheme.Light:
                    {
                        colorArr = lightColorArr;
                    }
                    break;
                case ColorTheme.Dark:
                    {
                        colorArr = darkColorArr;
                    }
                    break;
                case ColorTheme.Auto:
                default:
                    {
                        colorArr = autoChangeColorArr;
                        break;
                    }
            }
            if (temperature < 40)
            {
                return colorArr[0];
            }
            if (temperature < 60)
            {
                return colorArr[1];
            }
            if (temperature < 80)
            {
                return colorArr[2];
            }
            return colorArr[3];
        }

        public static void AppShortcutToStartUp()
        {
            using (var writer = new StreamWriter($"{Environment.GetFolderPath(Environment.SpecialFolder.Startup)}\\+1s.url"))
            {
                var app = System.Reflection.Assembly.GetExecutingAssembly().Location;
                writer.WriteLine("[InternetShortcut]");
                writer.WriteLine($"URL=file:///{app}");
                writer.WriteLine("IconIndex=0");
                var icon = app.Replace('\\', '/');
                writer.WriteLine($"IconFile={icon}");
            }
        }

        public static TemperatureInfo GetCpuTemperature()
        {
            var temperatureList = new List<double>();
            foreach (var hardwareItem in Computer.Hardware.Where(i => i.HardwareType == HardwareType.CPU))
            {
                hardwareItem.Update();
                foreach (var sensor in hardwareItem.Sensors.Where(i => i.SensorType == SensorType.Temperature && i.Value.HasValue))
                {
                    temperatureList.Add(sensor.Value ?? 0);
                }
            }
            return new TemperatureInfo(temperatureList);
        }

    }
}
