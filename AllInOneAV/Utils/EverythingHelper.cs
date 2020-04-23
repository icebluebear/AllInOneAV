using Model.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class EverythingHelper
    {
        private List<FileInfo> files = new List<FileInfo>();

        public List<FileInfo> SearchFile(string content, EverythingSearchEnum type)
        {
            switch (type)
            {
                case EverythingSearchEnum.Video:
                    content = "ext:3g2;3gp;3gp2;3gpp;amr;amv;asf;avi;bdmv;bik;d2v;divx;drc;dsa;dsm;dss;dsv;evo;f4v;flc;fli;flic;flv;hdmov;ifo;ivf;m1v;m2p;m2t;m2ts;m2v;m4b;m4p;m4v;mkv;mp2v;mp4;mp4v;mpe;mpeg;mpg;mpls;mpv2;mpv4;mov;mts;ogm;ogv;pss;pva;qt;ram;ratdvd;rm;rmm;rmvb;roq;rpm;smil;smk;swf;tp;tpr;ts;vob;vp6;webm;wm;wmp;wmv " + content;
                    break;
            }

            files = new List<FileInfo>();

            SearchEverythign(content);

            return files;
        }

        private void SearchEverythign(string strArg)
        {
            Process p = new Process();//建立外部调用线程
            p.StartInfo.FileName = "c:\\setting\\es.exe";//要调用外部程序的绝对路径
            p.StartInfo.Arguments = strArg;
            p.StartInfo.UseShellExecute = false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;//不创建进程窗口
            p.OutputDataReceived += new DataReceivedEventHandler(Output);
            p.Start();//启动线程
            p.BeginOutputReadLine();//开始异步读取
            p.WaitForExit();//阻塞等待进程结束
            p.Close();//关闭进程
            p.Dispose();//释放资源
        }

        private void Output(object sendProcess, DataReceivedEventArgs output)
        {
            if (!String.IsNullOrEmpty(output.Data))
            {
                if (File.Exists(output.Data))
                {
                    files.Add(new FileInfo(output.Data));
                }
            }
        }
    }
}
