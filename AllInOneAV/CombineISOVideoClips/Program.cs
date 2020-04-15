using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace CombineISOVideoClips
{
    class Program
    {
        static void Main(string[] args)
        {
            //var hasFile = false;
            //var fPath = "";
            //var desc = "l:\\convert\\";

            //while (!hasFile)
            //{
            //    Console.WriteLine("输入需要处理的ISO文件...");
            //    fPath = Console.ReadLine();
            //    hasFile = File.Exists(fPath);
            //}

            //if(!Directory.Exists(desc))
            //{
            //    Directory.CreateDirectory(desc);
            //}

            //var f = new FileInfo(fPath);
            //System.Diagnostics.Process.Start(@"" + fPath);

            //
            //var clipsContainer = Environment.GetLogicalDrives().LastOrDefault() + "\\VIDEO_TS";

            //var clipsContainer = "D:\\New folder\\";

            //if (Directory.Exists(clipsContainer))
            //{
            //    var clips = Directory.GetFiles(clipsContainer);
            //    string commend = " -i \"concat:{0}\" -c copy \"{1}.mp4\"";
            //    string subCommend = "";
            //    List<string> res = new List<string>(); 

            //    foreach (var c in clips)
            //    {
            //        FileInfo fi = new FileInfo(c);

            //        if (fi.Length >= 1000 * 1024 * 1024)
            //        {
            //            string temp = "-i \"{0}\" -c copy -bsf:v h264_mp4toannexb -f mpegts \"{1}\"";
            //            string tempDesc = fi.FullName.Replace(fi.Extension, ".ts");
            //            res.Add(tempDesc);

            //            FileUtility.ConvertVideo("c:\\setting\\ffmpeg.exe", string.Format(temp, fi.FullName, tempDesc));
            //        }
            //    }

            //    string resCommend = "-i \"concat:{0}\" -c:v hevc_nvenc -preset:v fast \"{1}\"";
            //    string descF = desc + f.Name.Replace(f.Extension, ".mp4");

            //    FileUtility.ConvertVideo("c:\\setting\\ffmpeg.exe", string.Format(resCommend, string.Join("|", res), descF));
            //}

            FileUtility.ConvertVideo("c:\\setting\\ffmpeg.exe", "-f concat -safe 0 -i c:\\setting\\merge.txt -c:v hevc_nvenc -preset:v fast l:\\convert\\test.mp4");

            Console.ReadKey();
        }
    }
}