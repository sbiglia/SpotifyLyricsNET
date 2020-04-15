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

            loadSettings();
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

        private int musixmatchPriority;
        private int geniusPriority;
        private int tekstowoPriority;

        private void loadSettings()
        {
            musixmatchCheck.IsChecked = Properties.Settings.Default.musixmatchFlag;
            geniusCheck.IsChecked = Properties.Settings.Default.geniusFlag;
            tekstowoCheck.IsChecked = Properties.Settings.Default.tekstowoFlag;

            musixmatchPriority = Properties.Settings.Default.musixmatchPriority;
            geniusPriority = Properties.Settings.Default.geniusPriority;
            tekstowoPriority = Properties.Settings.Default.tekstowoPriority;
        }

        private void saveSettings()
        {
            Properties.Settings.Default.musixmatchFlag = (bool)musixmatchCheck.IsChecked;
            Properties.Settings.Default.geniusFlag = (bool)geniusCheck.IsChecked;
            Properties.Settings.Default.tekstowoFlag = (bool)tekstowoCheck.IsChecked;

            Properties.Settings.Default.musixmatchPriority = musixmatchPriority;
            Properties.Settings.Default.geniusPriority = geniusPriority;
            Properties.Settings.Default.tekstowoPriority = tekstowoPriority;

            Properties.Settings.Default.Save();
        }

        private void loadCheckboxes()
        {
            musixmatchCheck.SetValue(Grid.RowProperty, musixmatchPriority);
            geniusCheck.SetValue(Grid.RowProperty, geniusPriority);
            tekstowoCheck.SetValue(Grid.RowProperty, tekstowoPriority);

            if (musixmatchPriority == 0)
            {
                musixmatchCheckBtn.IsEnabled = false;
                geniusCheckBtn.IsEnabled = true;
                tekstowoCheckBtn.IsEnabled = true;
            }
            else if (geniusPriority == 0)
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
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            bool mFlag = (bool)musixmatchCheck.IsChecked;
            bool gFlag = (bool)geniusCheck.IsChecked;
            bool tFlag = (bool)tekstowoCheck.IsChecked;

            if (!mFlag && !gFlag && !tFlag)
            {
                MessageBox.Show("You have to enable at least one source.", "Not allowed", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            saveSettings();

            MessageBox.Show("The changes will be visible from the next search.", "Settings saved", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();
        }

        private void musixmatchCheckBtn_Click(object sender, RoutedEventArgs e)
        {
            int oldPriority = musixmatchPriority;
            int newPriority = oldPriority - 1;

            if (newPriority < 0) return;

            if (tekstowoPriority == newPriority)
            {
                tekstowoPriority = oldPriority;
            }
            else
            {
                geniusPriority = oldPriority;
            }

            musixmatchPriority = newPriority;

            loadCheckboxes();
        }

        private void geniusCheckBtn_Click(object sender, RoutedEventArgs e)
        {
            int oldPriority = geniusPriority;
            int newPriority = oldPriority - 1;

            if (newPriority < 0) return;

            if (musixmatchPriority == newPriority)
            {
                musixmatchPriority = oldPriority;
            }
            else
            {
                tekstowoPriority = oldPriority;
            }

            geniusPriority = newPriority;

            loadCheckboxes();
        }

        private void tekstowoCheckBtn_Click(object sender, RoutedEventArgs e)
        {
            int oldPriority = tekstowoPriority;
            int newPriority = oldPriority - 1;

            if (newPriority < 0) return;

            if (musixmatchPriority == newPriority)
            {
                musixmatchPriority = oldPriority;
            }
            else
            {
                geniusPriority = oldPriority;
            }

            tekstowoPriority = newPriority;

            loadCheckboxes();
        }
    }
}