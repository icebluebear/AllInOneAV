using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace FIleChangeMonitor
{
    public class Program
    {
        private static List<string> formats = JavINIClass.IniReadValue("Scan", "Format").Split(',').ToList();

        static void Main(string[] args)
        {
            var wather = new FileSystemWather("c:/", "*.*", true);
            wather.OnChanged += OnChanged;
            wather.OnCreated += OnCreated;
            wather.OnRenamed += OnRenamed;
            wather.OnDeleted += OnDeleted;
            wather.Start();

            while (true)
            {

            }
        }

        private static void OnCreated(object source, FileSystemEventArgs e)
        {
            if (IsValidFormat(e.Name))
            {
                Console.WriteLine(e.FullPath);
            }
        }
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            //自定义逻辑处理
        }
        private static void OnDeleted(object source, FileSystemEventArgs e)
        {
            //自定义逻辑处理
        }
        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            //自定义逻辑处理
        }

        private static bool IsValidFormat(string filename)
        {
            foreach (var item in formats)
            {
                if (filename.EndsWith(item, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
