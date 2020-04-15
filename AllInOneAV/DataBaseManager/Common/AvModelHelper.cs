using DataBaseManager.JavDataBaseHelper;
using HtmlAgilityPack;
using Model.JavModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace DataBaseManager.Common
{
    public class AvModelHelper
    {
        public static AV GenerateAVModel(string html, string avUrl)
        {
            AV av = new AV();

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var titlePath = "//h3[@class='post-title text']";
            var picPath = "//img[@id='video_jacket_img']";

            var releasdPath = "//div[@id='video_date']//td[@class='text']";
            var lengthPath = "//div[@id='video_length']//span[@class='text']";

            var dirPath = "//span[@class='director']//a";
            var comPath = "//span[@class='maker']//a";
            var pubPath = "//span[@class='label']//a";

            var catPath = "//span[@class='genre']//a";
            var staPath = "//span[@class='star']//a";


            var titleNode = htmlDocument.DocumentNode.SelectSingleNode(titlePath);
            var title = titleNode.InnerText.Trim();
            var id = title.Substring(0, title.IndexOf(" "));
            title = FileUtility.ReplaceInvalidChar(title.Substring(title.IndexOf(" ") + 1));
            var picUrl = htmlDocument.DocumentNode.SelectSingleNode(picPath);

            av.URL = avUrl;
            av.PictureURL = picUrl.Attributes["src"].Value;
            av.Name = title;
            av.ID = id;

            var release = htmlDocument.DocumentNode.SelectSingleNode(releasdPath);
            if (release != null && !string.IsNullOrEmpty(release.InnerText))
            {
                av.ReleaseDate = DateTime.Parse(release.InnerText.Trim());
            }

            var length = htmlDocument.DocumentNode.SelectSingleNode(lengthPath);
            if (length != null && !string.IsNullOrEmpty(length.InnerText))
            {
                av.AvLength = int.Parse(length.InnerText.Trim());
            }

            var dirNode = htmlDocument.DocumentNode.SelectNodes(dirPath);
            foreach (var dir in dirNode)
            {
                var name = dir.InnerHtml.Trim();
                var url = "http://www.javlibrary.com/cn/" + dir.Attributes["href"].Value;

                Director d = new Director
                {
                    CreateTime = DateTime.Now,
                    Name = name,
                    URL = url
                };

                if (!JavDataBaseManager.HasDirector(d.URL))
                {
                    //JavDataBaseManager.InsertDirector(d);
                }

                av.Director += name + ",";
            }

            var comNode = htmlDocument.DocumentNode.SelectNodes(comPath);
            foreach (var com in comNode)
            {
                var name = com.InnerHtml.Trim();
                var url = "http://www.javlibrary.com/cn/" + com.Attributes["href"].Value;

                Company c = new Company
                {
                    CreateTime = DateTime.Now,
                    Name = name,
                    URL = url
                };

                if (!JavDataBaseManager.HasCompany(c.URL))
                {
                    //JavDataBaseManager.InsertCompany(c);
                }

                av.Company += name + ",";
            }

            var pubNode = htmlDocument.DocumentNode.SelectNodes(pubPath);
            foreach (var pub in pubNode)
            {
                var name = pub.InnerHtml.Trim();
                var url = "http://www.javlibrary.com/cn/" + pub.Attributes["href"].Value;

                Publisher p = new Publisher
                {
                    CreateTime = DateTime.Now,
                    Name = name,
                    URL = url
                };

                if (!JavDataBaseManager.HasPublisher(p.URL))
                {
                    //JavDataBaseManager.InsertPublisher(p);
                }

                av.Publisher += name + ",";
            }

            var catNodes = htmlDocument.DocumentNode.SelectNodes(catPath);
            foreach (var cat in catNodes)
            {
                var name = cat.InnerHtml.Trim();
                var url = "http://www.javlibrary.com/cn/" + cat.Attributes["href"].Value;

                av.Category += name + ",";
            }

            var starNodes = htmlDocument.DocumentNode.SelectNodes(staPath);
            foreach (var star in starNodes)
            {
                var name = star.InnerHtml.Trim();
                var url = "http://www.javlibrary.com/cn/" + star.Attributes["href"].Value;

                Actress a = new Actress
                {
                    CreateTime = DateTime.Now,
                    Name = name,
                    URL = url
                };

                if (!JavDataBaseManager.HasActress(a.URL))
                {
                    //JavDataBaseManager.InsertActress(a);
                }

                av.Actress += name + ",";
            }

            return av;
        }
    }
}
