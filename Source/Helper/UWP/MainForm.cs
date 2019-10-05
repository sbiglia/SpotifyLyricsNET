using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Spotify_Lyrics.NET_Helper_UWP
{
    public partial class MainForm : Form
    {
        const string VERSION = "v1.0.0";
        const string BUILD = "06.10.2019"; // DD.MM.YYYY
        const string appAuthor = "Jakub Stęplowski";
        const string appAuthorWebsite = "https://jakubsteplowski.com";
        const string appHelperName = "Spotify_Lyrics.NET_Helper_UWP";
        const string appName = "Spotify_Lyrics.NET";

        const string appLink = @"shell:appsFolder\2238JakubSteplowski.SpotifyLyrics.NET_76k8dzmm3mrer!App";

        private bool isSpotifyOpened = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Opacity = 0;
            this.Visible = false;

            checkRegistry();
            timer.Start();
        }

        private void checkRegistry()
        {
            try
            {
                RegistryKey rkApp = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                if (rkApp.GetValue(appHelperName) == null)
                    rkApp.SetValue(appHelperName, System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            catch (Exception ex)
            {
            }
        }

        private bool isFlagPresent()
        {
            try
            {
                return File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Spotify Lyrics .NET\launch_flag");
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private bool isProcessPresent(string processName)
        {
            try
            {
                return (Process.GetProcessesByName(processName).Count() > 0);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!isSpotifyOpened)
            {
                if (isFlagPresent() && isProcessPresent("Spotify") && !isProcessPresent(appName))
                {
                    launchSpotifyLyricsNET();
                    isSpotifyOpened = true;
                }
            }
            else if (!isProcessPresent("Spotify"))
            {
                isSpotifyOpened = false;
            }
        }

        private void launchSpotifyLyricsNET()
        {
            try
            {
                Process p = new Process();
                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.UseShellExecute = true;
                startInfo.FileName = appLink;
                p.StartInfo = startInfo;
                p.Start();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
