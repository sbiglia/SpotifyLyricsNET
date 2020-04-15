using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Spotify_Lyrics.NET
{
    public partial class Settings : Window
    {
        public Settings()
        {
            InitializeComponent();

            loadCheckboxes();
            loadTheme(Properties.Settings.Default.theme);
            this.ShowDialog();
        }

        private SolidColorBrush bgColor = new SolidColorBrush();
        private SolidColorBrush bgColor2 = new SolidColorBrush();
        private SolidColorBrush textColor = new SolidColorBrush();
        private SolidColorBrush textColor2 = new SolidColorBrush();
        private SolidColorBrush spotifyGreen = new SolidColorBrush(Color.FromRgb(29, 185, 84));

        // Themes
        private void loadTheme(int themeID)
        {
            switch (themeID)
            {
                case 0: // Light
                {
                    bgColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    bgColor2 = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    textColor = new SolidColorBrush(Color.FromRgb(24, 24, 24));
                    textColor2 = new SolidColorBrush(Color.FromRgb(10, 10, 10));
                    break;
                }
                case 1: // Dark
                {
                    bgColor = new SolidColorBrush(Color.FromRgb(24, 24, 24));
                    bgColor2 = new SolidColorBrush(Color.FromRgb(61, 61, 61));
                    textColor = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    textColor2 = new SolidColorBrush(Color.FromRgb(179, 179, 179));
                    break;
                }
            }

            // Set colors
            this.settingsGrid.Background = bgColor;
            this.settingsTitle.Foreground = textColor;
            this.settingsText.Foreground = textColor;
            this.musixmatchCheck.Foreground = textColor;
            this.geniusCheck.Foreground = textColor;
            this.tekstowoCheck.Foreground = textColor;

            this.musixmatchCheckBtn.Foreground = textColor2;
            this.geniusCheckBtn.Foreground = textColor2;
            this.tekstowoCheckBtn.Foreground = textColor2;
        }

        private bool loadingCheckboxesFlag = false;
        private void loadCheckboxes()
        {
            loadingCheckboxesFlag = true;

            musixmatchCheck.SetValue(Grid.RowProperty, Properties.Settings.Default.musixmatchPriority);
            geniusCheck.SetValue(Grid.RowProperty, Properties.Settings.Default.geniusPriority);
            tekstowoCheck.SetValue(Grid.RowProperty, Properties.Settings.Default.tekstowoPriority);

            if (Properties.Settings.Default.musixmatchPriority == 0)
            {
                musixmatchCheckBtn.IsEnabled = false;
                geniusCheckBtn.IsEnabled = true;
                tekstowoCheckBtn.IsEnabled = true;
            } 
            else if (Properties.Settings.Default.geniusPriority == 0)
            {
                musixmatchCheckBtn.IsEnabled = true;
                geniusCheckBtn.IsEnabled = false;
                tekstowoCheckBtn.IsEnabled = true;
            }
            else
            {
                musixmatchCheckBtn.IsEnabled = true;
                geniusCheckBtn.IsEnabled = true;
                tekstowoCheckBtn.IsEnabled = false;
            }

            musixmatchCheck.IsChecked = Properties.Settings.Default.musixmatchFlag;
            geniusCheck.IsChecked = Properties.Settings.Default.geniusFlag;
            tekstowoCheck.IsChecked = Properties.Settings.Default.tekstowoFlag;

            loadingCheckboxesFlag = false;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            //
            this.Close();
        }

        private void tekstowoCheckBtn_Click(object sender, RoutedEventArgs e)
        {
            int oldPriority = Properties.Settings.Default.tekstowoPriority;
            int newPriority = oldPriority - 1;

            if (newPriority < 0) return;

            if (Properties.Settings.Default.musixmatchPriority == newPriority)
            {
                Properties.Settings.Default.musixmatchPriority = oldPriority;
            } 
            else
            {
                Properties.Settings.Default.geniusPriority = oldPriority;
            }

            Properties.Settings.Default.tekstowoPriority = newPriority;
            Properties.Settings.Default.Save();

            loadCheckboxes();
        }
    }
}