using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spotify_Lyrics.NET.API
{
    class TekstowoplAPI
    {
        public void getLyrics(string artist, string song, ref List<MainWindow.lyricsURL> lyricsURLs, string lyricsURL = "")
        {
            if (lyricsURL.Length == 0)
            {
                // Search the song on Tekstowo.pl
                string searchURL = "https://www.tekstowo.pl/szukaj,wykonawca," + artist.Replace(" ", "+") + ",tytul," + song.Replace(" ", "+") +  ".html";
                string response = getHTTPSRequest(searchURL);
                response = response.Replace("\"", "'");
                List<string> responseArr = response.Split('\n').ToList();

                // Save all the valid search results
                foreach (string ln in responseArr)
                {
                    if (ln.ToLower().Trim().Contains(song.ToLower().Trim()) && ln.ToLower().Trim().Contains(artist.ToLower().Trim()) && ln.Contains("href"))
                    {
                        try
                        {
                            // Found
                            int firstPoint = ln.IndexOf("href='") + "href='".Length;
                            int secondPoint = ln.IndexOf("'", firstPoint);
                            string songLink = "https://www.tekstowo.pl" + ln.Substring(firstPoint, secondPoint - firstPoint);

                            MainWindow.lyricsURL lyricsObj = new MainWindow.lyricsURL();
                            lyricsObj.id = "";
                            lyricsObj.img = "";
                            lyricsObj.url = songLink;
                            lyricsObj.source = "Tekstowo.pl";

                            lyricsURLs.Add(lyricsObj);
                        }
                        catch (Exception ex)
                        {
                        }
                        break;
                    }
                }
            }
            else
            {
                MainWindow.lyricsURL lyricsObj = new MainWindow.lyricsURL();
                lyricsObj.id = "";
                lyricsObj.img = "";
                lyricsObj.url = lyricsURL;
                lyricsObj.source = "Tekstowo.pl";

                lyricsURLs.Add(lyricsObj);
            }
        }

        public string setLyrics(int indx, ref List<MainWindow.lyricsURL> lyricsURLs)
        {
            string response = getHTTPSRequest(WebUtility.HtmlEncode(lyricsURLs[indx].url));
            response = response.Replace("\"", "'");
            List<string> responseArr = response.Split('\n').ToList();
            string lyricsText = "";

            // Save all the valid search results
            int step = 0;
            foreach (string ln in responseArr)
            {
                if (step == 0 && ln.Contains("song-text"))
                {
                    step = 1;
                }
                else if (step == 1 && (ln.Contains("song_revisions_link") || ln.Contains("<p>&nbsp;</p>")))
                {
                    break;
                }
                else if (step == 1)
                {
                    if (!ln.Contains("<h2>Tekst piosenki:</h2>") && !ln.Contains("<p>&nbsp;</p>"))
                        lyricsText += ln.Trim().Replace("<br />", "\n");
                }
            }

            return lyricsText;
        }

        private static string getHTTPSRequest(string strRequest)
        {
            try
            {
                WebRequest ThisRequest = WebRequest.Create(strRequest);
                ThisRequest.ContentType = "application/x-www-form-urlencoded";
                ThisRequest.Method = "GET";

                System.Text.ASCIIEncoding Encoder = new System.Text.ASCIIEncoding();
                byte[] BytesToSend = Encoder.GetBytes(strRequest);
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpWebResponse TheirResponse = (HttpWebResponse)ThisRequest.GetResponse();

                StreamReader sr = new StreamReader(TheirResponse.GetResponseStream());
                string strResponse = sr.ReadToEnd();
                sr.Close();

                return strResponse;
            }
            catch (Exception ex)
            {
                return "";
            }
        }
    }
}
