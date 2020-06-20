using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using OpenHardwareMonitor.Hardware;

namespace IAddOneSecondForElder
{
    public partial class Form1 : Form
    {
        private static int _currentIndex = 0;
        private const int BeepCoolDownMillisecond = 59 * 1000;
        private const string ExeName = "+1s.exe";
        private static readonly Computer Computer = new Computer
        {
            CPUEnabled = true,
        };
        public Form1()
        {
            InitializeComponent();
            SetStartup();
            Computer.Open();
        }

        public void ShowTemperatureInfo(TemperatureInfo temperatureInfo)
        {
            Color GetColorByTemperature(double temperature)
            {
                if (temperature < 40)
                {
                    return Color.Green;
                }
                if (temperature < 60)
                {
                    return Color.Blue;
                }
                if (temperature < 80)
                {
                    return Color.OrangeRed;
                }
                return Color.Red;
            }
            var fontToUse = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel);
            var brushToUse = new SolidBrush(GetColorByTemperature(temperatureInfo.Average));
            var bitmapText = new Bitmap(16, 16);
            var g = Graphics.FromImage(bitmapText);

            g.Clear(Color.Transparent);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            g.DrawString(temperatureInfo.Average.ToString("F0"), fontToUse, brushToUse, 0, 0);
            var hIcon = (bitmapText.GetHicon());
            notifyIcon1.Icon = Icon.FromHandle(hIcon);
            notifyIcon1.Text = $"CPU Temperature\nMin:{temperatureInfo.Min:F1}℃\nMax:{temperatureInfo.Max:F1}℃\nAverage:{temperatureInfo.Average:F1}℃";

        }

        private void BeepCaller()
        {
            _currentIndex += timer1.Interval;
            if (_currentIndex >= BeepCoolDownMillisecond)
            {
                Console.Beep(50, 500);
                _currentIndex = 0;
            }
        }

        private TemperatureInfo GetCpuTemperature()
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

        private void timer1_Tick(object sender, EventArgs e)
        {
            BeepCaller();
            ShowTemperatureInfo(GetCpuTemperature());
        }

        private void SetStartup()
        {
            var registryKey = Registry.CurrentUser.OpenSubKey
                ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            registryKey?.SetValue(ExeName, Application.ExecutablePath);

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/woxieao/-1s"));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
