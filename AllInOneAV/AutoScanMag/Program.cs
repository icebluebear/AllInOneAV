using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AutoScanMag
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var folder = @"G:\AllInOneAV\RemoteCommands";

                var files = Directory.GetFiles(folder);

                if (files == null || files.Count() <= 0)
                {
                    Thread.Sleep(30 * 1000);
                }
                else
                {
                    foreach (var file in files)
                    {
                        File.Delete(file);

                        Process p = new Process();                //加载CMD命令行并传递参数执行
                        p.StartInfo.FileName = @"G:\Github\AllInOneAV\AllInOneAV\ScanJavMagUrl\bin\Debug\ScanJavMagUrl.exe";         //这里是设置要调用的目标程序或文件，FileName 属性不
                        p.StartInfo.UseShellExecute = false;      //是否使用操作系统外壳程序启动进程
                        p.StartInfo.CreateNoWindow = false;        //是否显示CMD命令提示符窗口

                        p.Start();
                        p.WaitForExit();
                    }
                }
            }
        }
    }
}
