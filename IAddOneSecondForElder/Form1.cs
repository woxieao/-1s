using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using IAddOneSecondForElder.Models;

namespace IAddOneSecondForElder
{
    public partial class Form1 : Form
    {
        private static int _currentIndex = 0;

        public Form1()
        {
            InitializeComponent();
            Core.AppShortcutToStartUp();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x80;
                return cp;
            }
        }
        private void BeepCaller()
        {
            _currentIndex += timer1.Interval;
            if (_currentIndex >= 1000 * 60)
            {
                _currentIndex = 0;
                Core.Beep();
            }
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern bool DestroyIcon(IntPtr handle);
        public void ShowTemperatureInfo(TemperatureInfo temperatureInfo)
        {
            using (var fontToUse = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Pixel))
            {
                using (var brushToUse = new SolidBrush(Core.GetDisplayColor(temperatureInfo.Average)))
                {
                    using (var bitmapText = new Bitmap(16, 16))
                    {
                        using (var g = Graphics.FromImage(bitmapText))
                        {
                            g.Clear(Color.Transparent);
                            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                            g.DrawString(temperatureInfo.Average.ToString("F0"), fontToUse, brushToUse, 0, 0);
                        }
                        var hIcon = (bitmapText.GetHicon());
                        notifyIcon1.Icon = Icon.FromHandle(hIcon);
                        notifyIcon1.Text = $"CPU\n温度Temperature\n最低Min:{temperatureInfo.Min:F0}℃\n最高Max:{temperatureInfo.Max:F0}℃\n平均Average:{temperatureInfo.Average:F0}℃";
                        DestroyIcon(Icon.FromHandle(hIcon).Handle);
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            BeepCaller();
            ShowTemperatureInfo(Core.GetCpuTemperature());
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo("https://github.com/woxieao/-1s"));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Core.Beep();
            MessageBox.Show("Hello There", ":)");
            return;
            for (var i = 0; i < 10000; i++)
            {
                // ShowTemperatureInfo(new TemperatureInfo(new List<double>() { i % 100 }));
                button1.Text = i.ToString();
            }
        }

        private void testToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.Instance.LoadConfig();
            Core.Beep();
        }

        private void autoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.Instance.ColorTheme = ColorTheme.Auto;
            Config.Instance.WriteConfig(Config.Instance);
        }

        private void lightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.Instance.ColorTheme = ColorTheme.Light;
            Config.Instance.WriteConfig(Config.Instance);
        }

        private void darkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config.Instance.ColorTheme = ColorTheme.Dark;
            Config.Instance.WriteConfig(Config.Instance);
        }
    }
}
