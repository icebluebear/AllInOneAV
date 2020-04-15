using DataBaseManager.JavDataBaseHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace JavBusDownload
{
    class Program
    {
        static void Main(string[] args)
        {
            var cc = JavBusDownloadHelper.GetJavBusCookie();
            JavBusDownloadHelper.UpdateJavBusCategory(cc);
            var category = JavDataBaseManager.GetJavBusCategoryMapping();
            var list = JavBusDownloadHelper.GetJavBusSearchListModel("DGL-075", cc);

            foreach (var item in list)
            {
                var detail = JavBusDownloadHelper.GetJavBusSearchDetail(item.Url, cc, category);

                if (detail != null)
                {
                    var avs = JavBusDownloadHelper.GetMatchJavDetail(detail);

                    var av = JavBusDownloadHelper.GetCloseLibAVModel(avs.Matches.FirstOrDefault(), category);

                    Console.WriteLine(av.ID);
                    Console.WriteLine(av.Name);
                    Console.WriteLine(av.Actress);
                    Console.WriteLine(av.Director);
                    Console.WriteLine(av.Company);
                    Console.WriteLine(av.Publisher);
                    Console.WriteLine(av.Category);
                    Console.WriteLine(av.AvLength);
                    Console.WriteLine(av.ReleaseDate);
                    Console.WriteLine(av.PictureURL);

                    Console.WriteLine();
                }
                else
                {
                    Console.WriteLine("Parse fail " + item.Url);
                }
            }

            Console.ReadKey();
        }
    }
}
