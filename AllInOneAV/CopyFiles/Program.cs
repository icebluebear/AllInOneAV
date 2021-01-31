using DataBaseManager.ScanDataBaseHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace CopyFiles
{
    class Program
    {
        static void Main(string[] args)
        {
            double giga = 0;
            string where = "";
            string folder = "";

            while (giga <= 0)
            {
                Console.WriteLine("输入复制文件的大小限制....");

                double.TryParse(Console.ReadLine(), out giga);
            }

            while (string.IsNullOrEmpty(folder))
            {
                Console.WriteLine("输入复制文件的目标位置....");

                folder = Console.ReadLine();

                if (!Directory.Exists(folder))
                {
                    folder = "";
                }
                else
                {
                    folder = (folder.EndsWith("\\") || folder.EndsWith("/")) ? folder : folder + "\\";
                }
            }

            Console.WriteLine("输入搜索WishList的Where条件，已 AND 开头，如果没有条件则输入空....");

            where = Console.ReadLine();

            CopyFiles(giga, folder, where);
        }

        public static void CopyFiles(double gigaLimits, string toFolder, string where = "")
        {
            double limits = gigaLimits * 1024 * 1024 * 1024;
            double currentSize = 0d;
            List<string> froms = new List<string>();
            List<string> tos = new List<string>();
            var files = ScanDataBaseManager.GetWishList(where);

            foreach (var f in files)
            {
                if (!string.IsNullOrEmpty(f.FilePath) && File.Exists(f.FilePath))
                {
                    var temp = new FileInfo(f.FilePath);

                    if (currentSize + temp.Length < limits)
                    {
                        currentSize += temp.Length;

                        froms.Add(f.FilePath);

                        tos.Add(toFolder + temp.Name);
                    }
                }
            }

            //FileUtility.TransferFileUsingSystem(froms, tos, false);
        }
    }
}
