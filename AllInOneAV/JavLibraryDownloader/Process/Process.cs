using DataBaseManager.JavDataBaseHelper;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace JavLibraryDownloader.Process
{
    public class Process
    {
        private static string UpdateURL = JavINIClass.IniReadValue("Jav", "update");

        public static void Start(RunType type, string args = "")
        {
            var cc = InitHelper.InitManager.UpdateCookie(null, UpdateURL);

            var res = InitHelper.InitManager.InitCategory(cc.CC);

            if (res)
            {
                if (type == RunType.Both || type == RunType.Scan)
                {
                    DoScan(new List<Category>(), cc.CC);
                }

                if (type == RunType.Both || type == RunType.Download)
                {
                    DoDownload(cc.CC);
                }

                if (type == RunType.Update)
                {
                    DoScan(new List<Category>()
                    {
                        new Category() {
                            Name = "Update",
                            Url = UpdateURL
                        }
                    },cc.CC);
                }

                if (type == RunType.Skip)
                {
                    var howMuchSkip = int.Parse(args);

                    DoScan(new List<Category>(), cc.CC, howMuchSkip);
                }
            }
            else
            {
                Console.WriteLine("Nothing to do");
            }
        }

        public static void DoScan(List<Category> categories, CookieContainer cc, int skip = 0)
        {
            bool isUpdate = false;

            if (categories.Count == 0)
            {
                categories = JavDataBaseManager.GetCategories();
            }
            else
            {
                isUpdate = true;
            }

            int currentCategory = 1;

            if (skip > 0)
            {
                categories = categories.Skip(skip - 1).ToList();
            }

            foreach (var category in categories)
            {
                cc = ScanHelper.ScanManager.Scan(category.Url, category.Name, currentCategory, categories.Count, cc, isUpdate);
                currentCategory++;
            }
        }

        public static void DoDownload(CookieContainer cc)
        {
            var noDownloads = JavDataBaseManager.GetScanURL().Where(x => x.IsDownload == false);
            int currentItems = 1;

            foreach (var item in noDownloads)
            {
                cc = DownloadHelper.DownloadManager.Download(item.URL, currentItems, noDownloads.Count(), cc);
            }
        }
    }
}
