using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scan115AndMatch
{
    class Program
    {
        static void Main(string[] args)
        {
            string folder = "";

            while (string.IsNullOrEmpty(folder))
            {
                Console.WriteLine("输入盘符，如有多个用','分割....");

                folder = Console.ReadLine();
            }

            List<string> list = folder.Split(',').ToList();

            OneOneFiveService.Match115(list);
        }
    }
}
