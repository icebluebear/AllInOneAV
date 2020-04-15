using Model.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace CombineRemoveDuplicate
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, List<FileInfo>> fileContainer = new Dictionary<string, List<FileInfo>>();
            string folder = "fin\\";
            var drivers = Environment.GetLogicalDrives();
            int single = 0;
            int duplicate = 0;
            int total = 0;
            long totalSize = 0; ;
            long singleSize = 0;
            long duplicateSize = 0;
            List<CheckDuplcateModel> res = new List<CheckDuplcateModel>();

            foreach (var driver in drivers)
            {
                Console.WriteLine("正在处理驱动器 --> " + driver);

                string targetFolder = driver + folder;

                if (Directory.Exists(targetFolder))
                { 
                    Console.WriteLine("\t找到目标文件夹 --> " + targetFolder);

                    var files = Directory.GetFiles(targetFolder);

                    Console.WriteLine("\t共有 --> " + files.Count() + " 个文件");

                    foreach (var file in files)
                    {
                        FileInfo fi = new FileInfo(file);
                        var fileSplitName = fi.Name.Split('-');

                        if (fileSplitName.Length >= 3)
                        {
                            var key = fileSplitName[0] + "-" + fileSplitName[1] + "-" + fileSplitName[2];

                            if (fileContainer.ContainsKey(key))
                            {
                                fileContainer[key].Add(fi);
                            }
                            else
                            {
                                fileContainer.Add(key, new List<FileInfo>() { fi });
                            }
                        }
                    }
                }
            }

            foreach (var key in fileContainer)
            {
                Console.WriteLine("处理 --> " + key.Key);

                if (key.Value.Count > 1)
                {
                    duplicate++;
                    total++;

                    CheckDuplcateModel cdm = new CheckDuplcateModel();
                    List<string> files = new List<string>();

                    cdm.ContainsFiles = files;
                    cdm.Key = key.Key;

                    foreach (var fi in key.Value)
                    {
                        cdm.ContainsFiles.Add(fi.FullName);
                        var file = fi.FullName;
                        var fileName = fi.Name;
                        var fileSize = fi.Length;

                        duplicateSize += fileSize;
                        totalSize += fileSize;

                        Console.WriteLine("\t******" + file);
                    }

                    res.Add(cdm);
                }
                else
                {
                    var fi = key.Value.FirstOrDefault();

                    var file = fi.FullName;
                    var fileName = fi.Name;
                    var fileSize = fi.Length;

                    single++;
                    total++;
                    singleSize += fi.Length;
                    totalSize += fileSize;

                    Console.WriteLine("\t-----" + file);
                }
            }

            Console.WriteLine(string.Format("总数量 -> {0}", total ));
            Console.WriteLine(string.Format("独立AV数量-> {0}", single));
            Console.WriteLine(string.Format("存在重复可能的AV数量-> {0},", duplicate));
            Console.WriteLine(string.Format("总大小-> {0},", FileSize.GetAutoSizeString(totalSize, 2)));
            Console.WriteLine(string.Format("独立AV大小-> {0},", FileSize.GetAutoSizeString(singleSize, 2)));
            Console.WriteLine(string.Format("存在重复可能的AV大小-> {0}", FileSize.GetAutoSizeString(duplicateSize, 2)));

            var logFolder = "c:\\setting\\checkresult\\";
            var logFile = logFolder + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".json";
            if (!Directory.Exists(logFolder))
            {
                Directory.CreateDirectory(logFolder);
            }


            File.Create(logFile).Close();

            StreamWriter sw = new StreamWriter(logFile);
            sw.WriteLine(JsonConvert.SerializeObject(res));
            sw.Close();

            Console.ReadKey();
        }
    }
}
