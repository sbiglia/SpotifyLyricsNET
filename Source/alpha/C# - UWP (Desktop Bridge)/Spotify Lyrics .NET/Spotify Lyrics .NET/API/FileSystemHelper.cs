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
                if (!File.Exists(localResourcesDirBase + @"\Spotify_Lyrics.NET_Helper_UWP.exe"))
                {
                    // Copy Helper
                    File.WriteAllBytes(localResourcesDirBase + @"\Spotify_Lyrics.NET_Helper_UWP.exe", Resources.Spotify_Lyrics_NET_Helper_UWP);
                    Process.Start(localResourcesDirBase + @"\Spotify_Lyrics.NET_Helper_UWP.exe");
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
