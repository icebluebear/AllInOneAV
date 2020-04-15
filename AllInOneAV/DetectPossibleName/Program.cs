using DataBaseManager.JavDataBaseHelper;
using Model.JavModels;
using Model.ScanModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DetectPossibleName
{
    class Program
    {
        static void Main(string[] args)
        {
            var files = GetFiles();
            var avs = JavDataBaseManager.GetAllAV();
            var prefix = FileUtility.GetPrefix(avs);
            List<Match> temp = new List<Match>();

            foreach (var f in files)
            {
                FileInfo file = new FileInfo(f);
                var scan = new Scan
                {
                    FileName = file.Name.Trim().ToLower(),
                    Location = file.DirectoryName.Trim().ToLower(),
                    Size = FileSize.GetAutoSizeString(file.Length, 2)
                };

                var possibleIDs = FileUtility.GetPossibleID(scan, prefix);

                AddTemp(scan, possibleIDs, temp);
            }

            Console.ReadKey();
        }

        static List<string> GetFiles()
        {
            List<string> files = new List<string>();
            StreamReader sr = new StreamReader("C:\\allav.txt");

            while (!sr.EndOfStream)
            {
                var str = sr.ReadLine().Split(' ');

                if (str.Length == 2)
                {
                    files.Add(str[1]);
                }

                if (str.Length > 2)
                {
                    var temp = "";

                    for (int i = 1; i < str.Length; i++)
                    {
                        temp += str[i] + " ";
                    }

                    files.Add(temp);
                }
            }

            sr.Close();

            return files;
        }

        private static void AddTemp(Scan scan, List<string> possibleIDs, List<Match> temp)
        {
            foreach (var id in possibleIDs)
            {
                var avs = JavDataBaseManager.GetAllAV(id);

                foreach (var av in avs)
                {
                    temp.Add(new Match
                    {
                        AvID = av.ID.ToLower(),
                        Location = scan.Location.ToLower(),
                        Name = scan.FileName.ToLower()
                    });
                }
            }
        }
    }
}
