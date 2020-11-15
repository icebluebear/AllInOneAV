using DataBaseManager.ScanDataBaseHelper;
using Model.Common;
using Model.ScanModels;
using Newtonsoft.Json;
using Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Utils;

namespace ScanJavMagUrl
{
    class Program
    {
        static List<RefreshModel> models = new List<RefreshModel>();
        static bool IsFinish = true;

        static void Main(string[] args)
        {
            var arg = " refresh " + 20;

            DoJob(arg);

            while (IsFinish)
            { 
                
            }
        }

        async static void DoJob(string arg)
        {
            ScanDataBaseManager.DeleteRemoteScanMag();

            await StartJavRefresh("", arg, OutputJavRefresh);

            await Task.Run(() => UpdateRefreshUi());

            IsFinish = false;
        }

        private async static Task StartJavRefresh(string exe, string arg, DataReceivedEventHandler output)
        {
            exe = "G:\\Github\\AllInOneAV\\AllInOneAV\\BatchJavScanerAndMacthMagUrl\\bin\\Debug\\BatchJavScanerAndMacthMagUrl.exe";

            using (var p = new Process())
            {
                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = arg;

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;

                p.OutputDataReceived += output;

                p.Start();
                p.BeginOutputReadLine();

                await p.WaitForExitAsync();
            }
        }

        private static void OutputJavRefresh(object sendProcess, DataReceivedEventArgs output)
        {
            if (!string.IsNullOrEmpty(output.Data) && output.Data.StartsWith("AV:"))
            {
                var jsonStr = output.Data.Replace("AV:", "");

                RefreshModel rm = JsonConvert.DeserializeObject<RefreshModel>(jsonStr);


                Console.WriteLine("扫描 --> " + rm.Name);

                models.Add(rm);
            }
        }

        private static void UpdateRefreshUi()
        {
            Random ran = new Random();

            Parallel.ForEach(models, new ParallelOptions { MaxDegreeOfParallelism = 5 }, rm =>
            {
                RemoteScanMag entity = new RemoteScanMag();

                Console.WriteLine("处理 --> " + rm.Name + models.IndexOf(rm) + "/" + models.Count);

                var matchFiles = new EverythingHelper().SearchFile(rm.Id + " | " + rm.Id.Replace("-", ""), EverythingSearchEnum.Video);

                var list = MagService.SearchSukebei(rm.Id);

                if (list != null && list.Count > 0)
                {
                    ScanDataBaseManager.DeleteMagUrlById(rm.Id);

                    if (matchFiles.Count > 0)
                    {
                        entity.SearchStatus = 2;
                        entity.MatchFile = matchFiles.FirstOrDefault(x => x.Length == matchFiles.Max(y => y.Length)).FullName;
                    }
                    else
                    {
                        entity.SearchStatus = 1;
                    }

                    foreach (var seed in list)
                    {
                        entity.AvId = rm.Id;
                        entity.AvName = FileUtility.ReplaceInvalidChar(rm.Name);
                        entity.AvUrl = rm.Url;
                        entity.MagDate = seed.Date;
                        entity.MagSize = seed.Size;
                        entity.MagTitle = FileUtility.ReplaceInvalidChar(seed.Title);
                        entity.MagUrl = seed.MagUrl;
                        //entity.SearchStatus = 1;

                        ScanDataBaseManager.InsertRemoteScanMag(entity);
                    }
                }
                else
                {
                    Console.WriteLine("没搜到");
                    entity.SearchStatus = 0;
                }

                Thread.Sleep(10 * ran.Next(5));
            });
        }
    }

    public static class ProcessExtensions
    {
        public static Task WaitForExitAsync(this Process process, CancellationToken cancellationToken = default(CancellationToken))
        {
            var tcs = new TaskCompletionSource<object>();
            process.EnableRaisingEvents = true;
            process.Exited += (sender, args) => tcs.TrySetResult(null);
            if (cancellationToken != default(CancellationToken))
            {
                cancellationToken.Register(tcs.SetCanceled);
            }

            return tcs.Task;
        }
    }
}
