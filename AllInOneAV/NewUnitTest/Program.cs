using MonoTorrent;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace NewUnitTest
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 100000; i++)
            {
                var start = DateTime.Now;

                Torrent t = GetTorrentInfo(@"magnet:?xt=urn:btih:8941be61d3915dffae3156bdbf3b1f3580f1d103&amp;dn=JUFD-727%20%E3%82%80%E3%81%97%E3%82%83%E3%81%B6%E3%82%8A%E3%81%A4%E3%81%8D%E3%81%9F%E3%81%84%E5%A5%B3%20%E3%82%AA%E3%83%8A%E7%A6%81%E3%83%8F%E3%83%A1%E7%A6%81%E3%81%AE%E3%81%82%E3%81%92%E3%81%8F%E5%AA%9A%E8%96%AC%E3%81%A7%E7%90%86%E6%80%A7%E3%81%8C%E3%81%B6%E3%81%A3%E9%A3%9B%E3%82%93%E3%81%A0%E7%BE%8E%E3%81%97%E3%81%84%E7%86%9F%E6%88%90%E7%88%86%E4%B9%B3%20%E5%85%AB%E7%A5%9E%E3%81%95%E3%81%8A%E3%82%8A&amp;tr=http%3A%2F%2Fsukebei.tracker.wf%3A8888%2Fannounce&amp;tr=udp%3A%2F%2Fopen.stealth.si%3A80%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.opentrackr.org%3A1337%2Fannounce&amp;tr=udp%3A%2F%2Ftracker.coppersurfer.tk%3A6969%2Fannounce&amp;tr=udp%3A%2F%2Fexodus.desync.com%3A6969%2Fannounce", "http://itorrents.org/torrent/", "g:\\torrent\\", "123.torrent").Result;

                if (t != null)
                {
                    //foreach (var f in t.Files)
                    //{
                    //    Console.WriteLine(f.Path + " => " + FileSize.GetAutoSizeString(f.Length, 1));
                    //}

                    Console.Write(i + " ");
                }
                else
                {
                    Console.Write(i + " error ");
                }

                Console.WriteLine((DateTime.Now - start).TotalMilliseconds);
            }

            Console.ReadKey();
        }

        public static async Task<Torrent> GetTorrentInfo(string magurl, string webUrl, string saveFolder, string fileName)
        {
            Torrent ret = null;
            var torrent = SaveMagnetToTorrent(magurl, webUrl, saveFolder, fileName);

            if (!string.IsNullOrEmpty(torrent))
            {
                ret = await Torrent.LoadAsync(torrent);
            }

            return ret;
        }

        public static string SaveMagnetToTorrent(string magurl, string webUrl, string saveFolder, string fileName)
        {
            string ret = (saveFolder.EndsWith("\\") || saveFolder.EndsWith("/")) ? saveFolder + fileName : saveFolder + "\\" + fileName;

            if (string.IsNullOrEmpty(magurl) || (!magurl.StartsWith("magnet:?xt=") && magurl.Length != 40))
            {
                return ret;
            }

            try
            {
                if (!Directory.Exists(saveFolder))
                {
                    Directory.CreateDirectory(saveFolder);
                    Thread.Sleep(50);
                }
                else
                {
                    if (File.Exists(ret))
                    {
                        File.Delete(ret);
                        Thread.Sleep(50);
                    }
                }

                File.Create(ret).Close();


                if (magurl.Length == 40)
                {
                    DownloadHelper.DownloadFile(webUrl + magurl + ".torrent", ret);
                }
                else
                {
                    var hash = magurl.Substring(magurl.IndexOf("btih:") + "btih:".Length);
                    hash = hash.Substring(0, hash.IndexOf("&"));

                    DownloadHelper.DownloadFile(webUrl + hash + ".torrent", ret);
                }
            }
            catch (Exception ee)
            {
                ret = "";
            }

            return ret;
        }
    }
}
