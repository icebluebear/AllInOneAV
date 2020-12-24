using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.ScanDataBaseHelper;
using Microsoft.Win32.TaskScheduler;
using Model.ScanModels;
using MonoTorrent;
using Newtonsoft.Json;
using Service;
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
            var text = "终于等到你！\n在这里我们会给提供专属服务~\n伴学小能手就是我，哈嘿！";

            string[] sArray = text.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        public static void Match115(List<string> drivers, bool overRide = false, bool writeJson = true)
        {
            Dictionary<string, bool> SearchResult = new Dictionary<string, bool>();
            var cc = OneOneFiveService.Get115Cookie();

            foreach (var d in drivers)
            {
                var files = new DirectoryInfo(d + @"\fin\").GetFiles();
                int index = 1;

                foreach (var file in files)
                {
                    var result = false;
                    var possibleSha = ScanDataBaseManager.GetPossibleMaping(file.FullName, file.Length);

                    if (possibleSha != null && overRide == false)
                    {
                        ScanDataBaseManager.DeleteShaMapping(possibleSha.Sha1);

                        if (possibleSha.IsExist == 0)
                        {
                            result = OneOneFiveService.Get115SearchResultInFinFolder(cc, possibleSha.Sha1);
                        }
                        else
                        {
                            result = true;
                            possibleSha.IsExist = 1;

                            ScanDataBaseManager.InsertShaMapping(possibleSha);
                        }
                    }

                    if(possibleSha == null || overRide)
                    {
                        DateTime start = DateTime.Now;

                        var sha1 = FileUtility.ComputeSHA1(file.FullName);

                        DateTime finishSha1 = DateTime.Now;

                        result = OneOneFiveService.Get115SearchResultInFinFolder(cc, sha1);

                        Console.WriteLine("处理 " + index++ + " / " + files.Count() + " 结果 " + (result ? "存在" : "不存在") + " 计算用时 " + (finishSha1 - start).TotalSeconds + " 秒 搜索用时 " + (DateTime.Now - finishSha1).TotalSeconds + " 秒 总共用时 " + (DateTime.Now - start).TotalSeconds + " 秒");

                        AvAndShaMapping aasm = new AvAndShaMapping();
                        aasm.FilePath = file.FullName;
                        aasm.FileSize = file.Length;
                        aasm.IsExist = result ? 1 : 0;
                        aasm.Sha1 = sha1;

                        ScanDataBaseManager.DeleteShaMapping(aasm.Sha1);
                        ScanDataBaseManager.InsertShaMapping(aasm);
                    }
                    
                    SearchResult.Add(file.FullName, result);
                }
            }

            if (writeJson)
            {
                var fileResult = @"c:\setting\filter115.json";

                if (File.Exists(fileResult))
                {
                    File.Delete(fileResult);
                    Thread.Sleep(50);
                }

                File.Create(fileResult).Close();
                StreamWriter sw = new StreamWriter(fileResult);
                sw.WriteLine(JsonConvert.SerializeObject(SearchResult));
                sw.Close();
            }
        }

        public static void BackUpJav()
        {
            var dateStr = DateTime.Today.ToString("yyyyMMdd");
            var extension = ".json";
            var folder = @"G:\Github\AllInOneAV\Scripts\\";
            var rawFolder = folder + "dataRaw\\";
            var zipFileFoler = folder + "dataZip\\";
            var zipFile = zipFileFoler + dateStr + ".zip";
            var avFile = rawFolder + "av" + dateStr + extension;
            var actressFile = rawFolder + "actress" + dateStr + extension;
            var directorFile = rawFolder + "director" + dateStr + extension;
            var companyFile = rawFolder + "company" + dateStr + extension;
            var publisherFile = rawFolder + "publisher" + dateStr + extension;

            StreamWriter sw = null;

            var avs = JavDataBaseManager.GetAllAV();
            var actress = JavDataBaseManager.GetActress();
            var director = JavDataBaseManager.GetDirector();
            var company = JavDataBaseManager.GetCompany();
            var publisher = JavDataBaseManager.GetPublisher();

            if (!Directory.Exists(rawFolder))
            {
                Directory.CreateDirectory(rawFolder);
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            if (!Directory.Exists(zipFileFoler))
            {
                Directory.CreateDirectory(zipFileFoler);
            }

            foreach (var file in new DirectoryInfo(rawFolder).GetFiles())
            {
                file.Delete();
                Thread.Sleep(50);
            }

            if (!File.Exists(avFile))
            {
                File.Create(avFile).Close();

                sw = new StreamWriter(avFile);
                sw.WriteLine(JsonConvert.SerializeObject(avs));
                sw.Close();
            }

            if (!File.Exists(actressFile))
            {
                File.Create(actressFile).Close();

                sw = new StreamWriter(actressFile);
                sw.WriteLine(JsonConvert.SerializeObject(actress));
                sw.Close();
            }

            if (!File.Exists(directorFile))
            {
                File.Create(directorFile).Close();

                sw = new StreamWriter(directorFile);
                sw.WriteLine(JsonConvert.SerializeObject(director));
                sw.Close();
            }

            if (!File.Exists(companyFile))
            {
                File.Create(companyFile).Close();

                sw = new StreamWriter(companyFile);
                sw.WriteLine(JsonConvert.SerializeObject(company));
                sw.Close();
            }

            if (!File.Exists(publisherFile))
            {
                File.Create(publisherFile).Close();

                sw = new StreamWriter(publisherFile);
                sw.WriteLine(JsonConvert.SerializeObject(publisher));
                sw.Close();
            }
        }

        public static void GetTaskNextRunTime(string taskName)
        {
            TaskService ts = new TaskService();
            var task = ts.FindTask("ScanJavJob");

            task.Run();

        }

        public static string GetNextRunTimeString(Microsoft.Win32.TaskScheduler.Task t)
        {
            if (t.State == TaskState.Disabled || t.NextRunTime < DateTime.Now)
                return string.Empty;
            return t.NextRunTime.ToString("G");
        }
    }
}
