using IWshRuntimeLibrary;
using Spotify_Lyrics.NET.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using File = System.IO.File;

// Very basic data file
// Not optimized, and dumb, but should be fine for its task.

namespace Spotify_Lyrics.NET.API
{
    class FileSystemHelper
    {
        const string helperVERSION = "v1.2.0";
        const string helperNAME = "Spotify_Lyrics.NET_Helper.exe";
        string localResourcesDirBase;
        string localResourcesDir;
        string localResourcesData;

        public FileSystemHelper()
        {
            localResourcesDirBase = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Spotify Lyrics .NET";
            localResourcesDir = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\Spotify Lyrics .NET\Data";
            localResourcesData = localResourcesDir + @"\lyrics.dat";

            checkLocalResources();
            checkHelper();
        }

        private void checkHelper()
        {
            try
            {
                bool copyHelper = false;
                if (!File.Exists(localResourcesDirBase + @"\" + helperNAME))
                {
                    copyHelper = true;

                    if (!File.Exists(localResourcesDirBase + @"\helper_version"))
                    {
                        File.WriteAllText(localResourcesDirBase + @"\helper_version", helperVERSION);
                    }
                }
                else
                {
                    if (!File.Exists(localResourcesDirBase + @"\helper_version"))
                    {
                        copyHelper = true;
                        File.WriteAllText(localResourcesDirBase + @"\helper_version", helperVERSION);
                    }
                    else
                    {
                        string currHelperVERSION = File.ReadAllText(localResourcesDirBase + @"\helper_version");

                        if (currHelperVERSION != helperVERSION)
                        {
                            copyHelper = true;
                            File.WriteAllText(localResourcesDirBase + @"\helper_version", helperVERSION);
                        }
                    }

                    if (copyHelper)
                    {
                        // Delete older version
                        try
                        {
                            Process helperProcess = Process.GetProcessesByName(helperNAME.Replace(".exe", "")).First();
                            if (helperProcess != null)
                                helperProcess.Kill();
                        }
                        catch (Exception ex)
                        {
                        }

                        try
                        {
                            File.Delete(localResourcesDirBase + @"\" + helperNAME);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

                if (copyHelper)
                {
                    // Copy Helper
                    Dialog diag = new Dialog(true, "This app uses a \"helper\" program that runs in the background and starts with the startup of Windows to make the \"Launch with Spotify\" feature possible.\n\nN.B.\nIf you'll decide to remove this software in the future, you can find and delete the helper in\n\"Documents\\Spotify Lyrics .NET\".\n\nThanks for your attention, enjoy!");
                    File.WriteAllBytes(localResourcesDirBase + @"\" + helperNAME, Resources.Spotify_Lyrics_NET_Helper);

                    Process proc = new Process();
                    proc.StartInfo.FileName = localResourcesDirBase + @"\" + helperNAME;
                    proc.StartInfo.UseShellExecute = true;
                    //proc.StartInfo.Verb = "runas";
                    proc.Start();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void checkLnk()
        {
            try
            {
                if (!File.Exists(localResourcesDirBase + @"\Spotify_Lyrics.NET.exe.lnk"))
                {
                    // Create shorcut of yourself for the Helper
                    WshShell shell = new WshShell();
                    IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(localResourcesDirBase + @"\Spotify_Lyrics.NET.exe.lnk");
                    shortcut.TargetPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    shortcut.Save();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void checkLocalResources()
        {
            if (!Directory.Exists(localResourcesDir))
            {
                Directory.CreateDirectory(localResourcesDir);
            }

            if (!File.Exists(localResourcesData))
            {
                File.CreateText(localResourcesData);
            }
        }

        public List<string> getLyrics(string title)
        {
            try
            {
                title = title.ToLower().Trim();

                string[] lyricsList = File.ReadAllLines(localResourcesData);

                foreach (string ly in lyricsList)
                {
                    if (ly.Contains("="))
                    {
                        string t = ly.Substring(0, ly.IndexOf("=")).ToLower().Trim();
                               

                        if (t == title)
                        {
                            string vals = ly.Substring(ly.IndexOf("=") + 1);
                            List<string> valsArr = vals.Split(',').ToList<string>();
                            // 0 = id, 1 = coverImg, 2 = url
                            return valsArr;
                        }
                    }
                    else break;
                }
            }
            catch (Exception ex) { }
            return new List<String>();
        }

        public bool saveLyrics(string title, string id, string coverImg, string url)
        {
            try
            {
                checkLocalResources();
                File.AppendAllText(localResourcesData, title + "=" + id + "," + coverImg + "," + url + '\n');

                return true;
            } catch (Exception ex) { }
            return false;
        }

        public bool removeLyrics(string title)
        {
            try
            {
                title = title.ToLower().Trim();

                checkLocalResources();
                string[] lyricsList = File.ReadAllLines(localResourcesData);
                List<string> lyricsListTemp = new List<string>();
                foreach (string ly in lyricsList)
                {
                    if (ly.Contains("="))
                    {
                        string t = ly.Substring(0, ly.IndexOf("=")).ToLower().Trim();

                        if (t != title) lyricsListTemp.Add(ly);
                    }
                    else break;
                }

                File.Delete(localResourcesData);
                File.WriteAllLines(localResourcesData, lyricsListTemp);

                return true;
            }
            catch (Exception ex) { }
            return false;
        }

        public bool updateLaunchFlag(bool flag)
        {
            try
            {
                if (flag)
                    File.Create(localResourcesDirBase + @"\launch_flag");
                else
                    File.Delete(localResourcesDirBase + @"\launch_flag");
                return true;
            } 
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
