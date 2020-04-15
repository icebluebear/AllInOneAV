using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace AvMover
{
    class Program
    {
        private static List<string> formats = JavINIClass.IniReadValue("Scan", "Format").Split(',').ToList();
        private static List<string> excludes = JavINIClass.IniReadValue("Scan", "Exclude").Split(',').ToList();

        static void Main(string[] args)
        {
            string folder = "";
            string moveFolder = "";
            Dictionary<string, int> moveRecord = new Dictionary<string, int>();
            Dictionary<string, string> remainSize = new Dictionary<string, string>();
            List<FileInfo> fis = new List<FileInfo>();

            while (string.IsNullOrEmpty(folder))
            {
                Console.WriteLine("输入需要整理的文件夹,按回车键继续");
                var temp = Console.ReadLine();

                try
                {
                    if (Directory.Exists(temp))
                    {
                        folder = temp;
                        moveFolder = folder + "/movefiles/";
                        excludes.Add(moveFolder);

                        if (!Directory.Exists(moveFolder))
                        {
                            Directory.CreateDirectory(moveFolder);
                        }
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.ToString());
                }
            }

            var status = FileUtility.GetFilesRecursive(folder, formats, excludes, fis, 200);

            if (string.IsNullOrEmpty(status))
            {
                try
                {
                    foreach (var fi in fis)
                    {
                        var n = fi.Name.Replace(fi.Extension, "");
                        var e = fi.Extension;

                        if (moveRecord.ContainsKey(fi.Name))
                        {
                            moveRecord[fi.Name]++;
                        }
                        else
                        {
                            moveRecord.Add(fi.Name, 1);
                        }

                        if (File.Exists(moveFolder + n + e))
                        {
                            n += "_" + moveRecord[fi.Name];
                        }

                        File.Move(fi.FullName, moveFolder + n + e);
                    }
                }
                catch (Exception ee)
                {
                    Console.WriteLine(ee.ToString());
                }
            }

            var subFolders = Directory.GetDirectories(folder);

            foreach (var sub in subFolders)
            {
                List<FileInfo> tempFi = new List<FileInfo>();
                string tempStatus = FileUtility.GetFilesRecursive(sub, formats, excludes, tempFi);
                double tempSize = 0D;

                if (string.IsNullOrEmpty(tempStatus))
                {
                    foreach (var fi in tempFi)
                    {
                        tempSize += fi.Length;
                    }

                    remainSize.Add(sub, FileSize.GetAutoSizeString(tempSize, 2));

                    Console.WriteLine(sub + " ===> " + FileSize.GetAutoSizeString(tempSize, 2));
                }
            }

            Console.ReadKey();
        }
    }
}
