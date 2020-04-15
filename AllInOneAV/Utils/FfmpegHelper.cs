using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class FfmpegHelper
    {
        public static void ConvertVideo(DataReceivedEventHandler output, Process p)
        {
            p.Start();//启动线程
            p.BeginErrorReadLine();//开始异步读取
            p.WaitForExit();
            p.Close();
            p.Dispose();
            p = null;
        }
    }
}
