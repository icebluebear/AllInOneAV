using MangaDownloaderGUI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDownloaderGUI.Service
{
    public class HistoryService
    {
        public static HistoryModel ReadHistory(string sourceName, string mangaName, string historyFolder)
        {
            HistoryModel ret = new HistoryModel();
            ret.DownloadedChapters = new List<string>();

            if (File.Exists(historyFolder + sourceName + "_" + mangaName + ".json"))
            {
                StreamReader sr = new StreamReader(historyFolder + sourceName + "_" + mangaName + ".json");
                var json = sr.ReadToEnd();
                sr.Close();

                ret = JsonConvert.DeserializeObject<HistoryModel>(json);
            }
            else
            {
                File.Create(historyFolder + sourceName + "_" + mangaName + ".json").Close();
                StreamWriter sw = new StreamWriter(historyFolder + sourceName + "_" + mangaName + ".json");
                sw.WriteLine(JsonConvert.SerializeObject(ret));
                sw.Close();
            }

            return ret;
        }

        public static bool WriteHistory(string sourceName, string mangaName, string historyFolder, string url, string mangaUrl)
        {
            bool ret = true;

            if (File.Exists(historyFolder + sourceName + "_" + mangaName + ".json"))
            {
                StreamReader sr = new StreamReader(historyFolder + sourceName + "_" + mangaName + ".json");
                var json = sr.ReadToEnd();
                sr.Close();

                var history = JsonConvert.DeserializeObject<HistoryModel>(json);

                if (!history.DownloadedChapters.Contains(url))
                {
                    history.DownloadedChapters.Add(url);
                }

                history.Url = mangaUrl;

                StreamWriter sw = new StreamWriter(historyFolder + sourceName + "_" + mangaName + ".json", false);
                sw.WriteLine(JsonConvert.SerializeObject(history));
                sw.Close();
            }
            else
            {
                ret = false;
            }

            return ret;
        }
    }
}
