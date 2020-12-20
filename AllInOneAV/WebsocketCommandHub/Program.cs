using Model.ScanModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Topshelf;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace WebsocketCommandHub
{
    class Program
    {
        static void Main(string[] args)
        {
            System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);
            var rc = HostFactory.Run(x =>
            {
                x.Service<WebSocketService>(s =>
                {
                    s.ConstructUsing(name => new WebSocketService());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem();
                x.SetDescription("WebsocketService");
                x.SetDisplayName("WebsocketService");
                x.SetServiceName("WebsocketService");
            });
            var exitCode = (int)Convert.ChangeType(rc, rc.GetTypeCode());
            Environment.ExitCode = exitCode; 
        }
    }

    public class WebSocketService
    {
        public WebSocketServer wssv = new WebSocketServer(30000);

        public void Start()
        {
            wssv.ReuseAddress = true;
            wssv.AddWebSocketService<Report>("/report");
            wssv.AddWebSocketService<Match>("/match");
            wssv.Start();
        }

        public void Stop()
        {
            wssv.Stop();
        }
    }

    #region 报表
    public class Report : WebSocketBehavior
    {
        protected async override void OnMessage(MessageEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                try
                {
                    var message = JsonConvert.DeserializeObject<WebSocketCommandParameter>(e.Data);

                    if (e.IsPing)
                    { 
                        
                    }

                    if (message != null)
                    {
                        Process[] procs = Process.GetProcessesByName("GenerateReport");

                        if (procs != null && procs.Length > 0)
                        {
                            Send("结束 任务正在执行中");
                            return;
                        }

                        using (var p = new Process())
                        {
                            p.StartInfo.FileName = @"G:\Github\AllInOneAV\AllInOneAV\GenerateReport\bin\Debug\GenerateReport.exe";
                            p.StartInfo.Arguments = " report";

                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.CreateNoWindow = true;
                            p.StartInfo.RedirectStandardOutput = true;

                            p.OutputDataReceived += new DataReceivedEventHandler((sender, o) =>
                            {
                                // Prepend line numbers to each line of the output.
                                if (!String.IsNullOrEmpty(e.Data))
                                {
                                    Send(o.Data);
                                }
                            });

                            p.Start();
                            p.BeginOutputReadLine();
                            await p.WaitForExitAsync();

                            Send("结束 任务完成");
                        }
                    }
                } catch (Exception ee)
                { 
                    
                }
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
        }
    }
    #endregion

    #region 刷新匹配
    public class Match : WebSocketBehavior
    {
        protected async override void OnMessage(MessageEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                try
                {
                    var message = JsonConvert.DeserializeObject<WebSocketCommandParameter>(e.Data);

                    if (e.IsPing)
                    {

                    }

                    if (message != null)
                    {
                        Process[] procs = Process.GetProcessesByName("GenerateReport");

                        if (procs != null && procs.Length > 0)
                        {
                            Send("结束 任务正在执行中");
                            return;
                        }

                        using (var p = new Process())
                        {
                            p.StartInfo.FileName = @"G:\Github\AllInOneAV\AllInOneAV\ScanAllAndMatch\bin\Debug\ScanAllAndMatch.exe";
                            p.StartInfo.Arguments = "";

                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.CreateNoWindow = true;
                            p.StartInfo.RedirectStandardOutput = true;

                            p.OutputDataReceived += new DataReceivedEventHandler((sender, o) =>
                            {
                                // Prepend line numbers to each line of the output.
                                if (!String.IsNullOrEmpty(e.Data))
                                {
                                    Send(o.Data);
                                }
                            });

                            p.Start();
                            p.BeginOutputReadLine();
                            await p.WaitForExitAsync();

                            Send("结束 任务完成");
                        }
                    }
                }
                catch (Exception ee)
                {

                }
            }
        }

        protected override void OnClose(CloseEventArgs e)
        {
            base.OnClose(e);
        }
    }
    #endregion

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
