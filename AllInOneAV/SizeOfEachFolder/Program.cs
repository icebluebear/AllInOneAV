using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace SizeOfEachFolder
{
    class Program
    {
        private static List<string> formats = JavINIClass.IniReadValue("Scan", "Format").Split(',').ToList();
        private static List<string> excludes = JavINIClass.IniReadValue("Scan", "Exclude").Split(',').ToList();
        private static List<FileInfo> srcFi = new List<FileInfo>();
        private static Dictionary<string, long> infos = new Dictionary<string, long>();

        static void Main(string[] args)
        {
            Console.WriteLine("Enter root folder...");

            var root = Console.ReadLine();

            var folders = Directory.GetDirectories(root);

            foreach (var folder in folders)
            {
                var total = 0l;
                FileUtility.GetFilesRecursive(folder, formats, excludes, srcFi);
                foreach (var file in srcFi)
                {
                    total += file.Length;
                }

                infos.Add(folder, total);
                srcFi = new List<FileInfo>();
            }

            infos.OrderBy(x => x.Value);

            foreach(var info in infos)
            {
                if (info.Value >= 100 * 1024 * 1024)
                {
                    Console.WriteLine(info.Key + " -> " + Utils.FileSize.GetAutoSizeString(info.Value, 1));
                }
            }

            Console.ReadKey();
        }
    }
}
