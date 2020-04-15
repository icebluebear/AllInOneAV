using DataBaseManager.JavDataBaseHelper;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace BatchRenameDemo
{
    class Program
    {
        private static readonly string imageFolder = JavINIClass.IniReadValue("Jav", "imgFolder");
        private static List<string> allPrefix = new List<string>();

        static void Main(string[] args)
        {
            StringBuilder logSb = new StringBuilder();
            Dictionary<string, int> moveReocrd = new Dictionary<string, int>();
            Dictionary<FileInfo, List<AV>> ret = new Dictionary<FileInfo, List<AV>>();
            string folder = "";
            string moveFolder = "";
            string logFolder = "";
            int fount = 0;
            int notFount = 0;

            while (string.IsNullOrEmpty(folder))
            {
                Console.WriteLine("请输入文件夹，按回车键继续");
                var tempFolder = Console.ReadLine();

                if (!string.IsNullOrEmpty(tempFolder) && Directory.Exists(tempFolder))
                {
                    folder = tempFolder;
                    moveFolder = tempFolder + "/tempFin/";
                    logFolder = tempFolder + "/log/";

                    if (!Directory.Exists(moveFolder))
                    {
                        Directory.CreateDirectory(moveFolder);
                    }

                    if (!Directory.Exists(logFolder))
                    {
                        Directory.CreateDirectory(logFolder);
                    }
                }
            }

            Console.WriteLine("加载AV缓存");
            var avs = JavDataBaseManager.GetAllAV();
            Console.WriteLine("共加载 --> " + avs.Count);

            Console.WriteLine("处理番号前缀");
            foreach (var name in avs.Select(x => x.ID).ToList())
            {
                if (!allPrefix.Contains(name.Split('-')[0]))
                {
                    allPrefix.Add(name.Split('-')[0]);
                }
            }
            Console.WriteLine("共收集 --> " + allPrefix.Count + " 番号前缀");

            allPrefix = allPrefix.OrderByDescending(x => x.Length).ToList();

            Console.WriteLine("获取文件夹 --> " + folder + " 下的文件");
            var files = Directory.GetFiles(folder);
            Console.WriteLine("共获取 --> " + files.Count() + " 个文件");

            foreach (var f in files)
            {
                bool findMatch = false;
                string pi = "";
                FileInfo fi = new FileInfo(f);
                List<AV> fiMatchList = new List<AV>();
                var fiNameUpper = fi.Name.ToUpper();
                var fileNameWithoutFormat = fiNameUpper.Replace(fi.Extension.ToUpper(), "");

                ret.Add(fi, fiMatchList);
                //Console.WriteLine("正在处理 --> " + fi.FullName);

                foreach (var prefix in allPrefix)
                {
                    if (fileNameWithoutFormat.Contains(prefix))
                    {
                        //Console.WriteLine("\t找到适配的前缀 --> " + prefix);
                        var pattern = prefix + "{1}-?\\d{1,5}";
                        var possibleId = Regex.Match(fileNameWithoutFormat, pattern).Value;

                        if (possibleId.Contains("-"))
                        {
                            pi = possibleId;
                        }
                        else
                        {
                            bool isFirst = true;
                            StringBuilder sb = new StringBuilder();

                            foreach (var c in possibleId)
                            {
                                if (c >= '0' && c <= '9')
                                {
                                    if (isFirst)
                                    {
                                        sb.Append("-");
                                        isFirst = false;
                                    }
                                }
                                sb.Append(c);
                            }

                            pi = sb.ToString();
                        }

                        if (!string.IsNullOrEmpty(pi))
                        {
                            var possibleAv = avs.Where(x => x.ID == pi).ToList();
                            //Console.WriteLine("\t找到一个匹配 --> " + pi);
                            findMatch = true;
                            foreach (var av in possibleAv)
                            {
                                fiMatchList.AddRange(possibleAv);
                            }

                            break;
                        }
                    }
                }

                if (findMatch)
                {
                    fount++;
                }
                else
                {
                    notFount++;
                }
            }

            foreach (var item in ret)
            {
                if (item.Value.Count == 0)
                {
                    //Console.WriteLine(item.Key.FullName + "没有找到匹配");
                }
                else if (item.Value.Count > 1)
                {
                    //Console.WriteLine(item.Key.FullName + "找到多个匹配");

                    foreach (var subItem in item.Value)
                    {
                        //Console.WriteLine("\t" + subItem);
                    }
                }
                else if(item.Value.Count == 1)
                {
                    var tempFileName = item.Value.FirstOrDefault().ID + "-" + item.Value.FirstOrDefault().Name + item.Key.Extension;

                    if (moveReocrd.ContainsKey(tempFileName))
                    {
                        moveReocrd[tempFileName]++;
                    }
                    else
                    {
                        moveReocrd.Add(tempFileName, 1);
                    }

                    if (File.Exists(moveFolder + tempFileName))
                    {
                        //logSb.AppendLine("-------移动 --> " + moveFolder + tempFileName + " 到 --> " + moveFolder + item.Value.FirstOrDefault().ID + "-" + item.Value.FirstOrDefault().Name + "-1" + item.Key.Extension);
                        //File.Move(moveFolder + tempFileName, moveFolder + item.Value.FirstOrDefault().ID + "-" + item.Value.FirstOrDefault().Name + "-1" + item.Key.Extension);

                        tempFileName = item.Value.FirstOrDefault().ID + "-" + item.Value.FirstOrDefault().Name + "-" + moveReocrd[tempFileName] + item.Key.Extension;

                        logSb.AppendLine("*******移动 --> " + item.Key.FullName + " 到 --> " + moveFolder + tempFileName);
                    }
                    else
                    {
                        logSb.AppendLine("移动 --> " + item.Key.FullName + " 到 --> " + moveFolder + tempFileName);
                    }

                    try
                    {
                        File.Move(item.Key.FullName, moveFolder + tempFileName);
                    }
                    catch (Exception ee)
                    {
                        logSb.AppendLine(ee.ToString());
                    }
                }
            }

            Console.WriteLine("找到匹配 --> " + fount + " 未找到匹配 --> " + notFount);

            var logFile = logFolder + "log" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".txt";

            File.Create(logFile).Close();

            StreamWriter sw = new StreamWriter(logFile, true, Encoding.UTF8);
            sw.WriteLine(logSb.ToString());

            Console.WriteLine("按任意键结束");
            Console.ReadKey();
        }
    }
}
