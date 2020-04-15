using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class PlayerHelper
    {
        public static string GeneratePotPlayerPlayList(List<string> files)
        {
            var folder = "c:\\setting\\playlist\\";
            var fileName = DateTime.Now.ToString("yyyyMMdd") + "PlayList.dpl";
            var sb = new StringBuilder();
            int index = 1;

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            File.Create(folder + fileName).Close();

            sb.AppendLine("DAUMPLAYLIST");          

            foreach (var f in files)
            {
                sb.AppendLine(string.Format("{0}*file*{1}", index++, f));
                sb.AppendLine("1*played*0");
            }

            using (StreamWriter sw = new StreamWriter(folder + fileName))
            {
                sw.WriteLine(sb.ToString());
            }

            return folder + fileName;
        }
    }
}
