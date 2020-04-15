using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckDuplicated
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> folders = new List<string>();
            Dictionary<string, List<FileInfo>> res = new Dictionary<string, List<FileInfo>>();

            while (folders.Count == 0)
            {
                Console.WriteLine("输入需要查重的文件夹，用‘,’分割，按回车键继续");
                var tempFolder = Console.ReadLine().Split(',');

                foreach (var tf in tempFolder)
                {
                    if (Directory.Exists(tf))
                    {
                        folders.Add(tf);
                    }
                }
            }

            foreach (var folder in folders)
            {
                var files = Directory.GetFiles(folder);

                foreach (var file in files)
                {
                    if (file.Split('-').Length >= 3)
                    {
                        FileInfo fi = new FileInfo(file);

                        if (res.ContainsKey(file.Split('-')[0] + file.Split('-')[1]))
                        {
                            res[file.Split('-')[0] + file.Split('-')[1]].Add(fi);
                        }
                        else
                        {
                            res.Add(file.Split('-')[0] + file.Split('-')[1], new List<FileInfo> { fi });
                        }
                    }                 
                }
            }

            foreach (var r in res)
            {
                if (r.Value.Count > 1)
                {
                    Console.WriteLine("AV --> " + r.Key + " 对应 --> " + r.Value.Count + " 个文件");

                    foreach (var v in r.Value)
                    {
                        Console.WriteLine("\t" + v.FullName + " --> " + Utils.FileSize.GetAutoSizeString(v.Length, 2));
                    }
                }
            }

            Console.ReadKey();
        }
    }
}
