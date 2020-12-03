using Fleck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WebSocketConsole
{
    class Program
    {
        //客户端url以及其对应的Socket对象字典
        private static IDictionary<string, IWebSocketConnection> dic_Sockets = new Dictionary<string, IWebSocketConnection>();
        private static IDictionary<string, DateTime> dic_SocketsTime = new Dictionary<string, DateTime>();
        private static WebSocketServer server = new WebSocketServer("ws://0.0.0.0:30000");//监听所有的的地址

        static void Main(string[] args)
        {
            server.RestartAfterListenError = true;

            //开始监听
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    string clientUrl = socket.ConnectionInfo.ClientIpAddress;

                    if (dic_Sockets.Count >= 99)
                    {
                        var removeKeys = dic_SocketsTime.OrderBy(x => x.Value.Ticks).Take(20).Select(x => x.Key);

                        foreach (var key in removeKeys)
                        {
                            if (dic_Sockets.ContainsKey(key))
                            {
                                if (dic_Sockets[key] != null)
                                {
                                    dic_Sockets[key].Close();
                                }

                                Console.WriteLine("清除 " + key);
                                dic_Sockets.Remove(key);
                            }
                        }
                    }

                    if (!dic_Sockets.ContainsKey(clientUrl))
                    {
                        dic_Sockets.Add(clientUrl, socket);
                    }

                    Console.WriteLine(DateTime.Now.ToString() + "|服务器:和客户端网页:" + clientUrl + " 建立WebSock连接！" + " 总数量: " + dic_Sockets.Count);
                };

                socket.OnClose = () =>
                {
                    string clientUrl = socket.ConnectionInfo.ClientIpAddress;
          
                    Console.WriteLine(DateTime.Now.ToString() + "|服务器:和客户端网页:" + clientUrl + " 断开WebSock连接！");
                };

                socket.OnMessage = message =>
                {
                    string clientUrl = socket.ConnectionInfo.ClientIpAddress;
                    Console.WriteLine(DateTime.Now.ToString() + "|服务器:【收到】来客户端网页:" + clientUrl + "的信息：\n" + message);

                    RunCommand(@"G:\Github\AllInOneAV\AllInOneAV\GenerateReport\bin\Debug\GenerateReport.exe", " report", socket).Wait();
                };
            });

            Console.ReadKey();
            foreach (var item in dic_Sockets.Values)
            {
                if (item.IsAvailable == true)
                {
                    item.Send("服务器消息：" + DateTime.Now.ToString());
                }
            }
            Console.ReadKey();

            foreach (var item in dic_Sockets.Values)
            {
                if (item != null)
                {
                    item.Close();
                }
            }

            Console.ReadKey();
        }

        private static async Task RunCommand(string exe, string arg, IWebSocketConnection socket)
        {
            using (var p = new Process())
            {
                p.StartInfo.FileName = exe;
                p.StartInfo.Arguments = arg;

                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;
                p.StartInfo.RedirectStandardOutput = true;

                p.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    // Prepend line numbers to each line of the output.
                    if (!String.IsNullOrEmpty(e.Data))
                    {
                        socket.Send(e.Data);
                    }
                });

                p.Start();
                p.BeginOutputReadLine();
                await p.WaitForExitAsync();
            }
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
