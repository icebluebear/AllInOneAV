using DataBaseManager.MangaDataBaseHelper;
using HtmlAgilityPack;
using Model.MangaModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class MangaService
    {
        public static void InitHanhanCategory(string htmlContent)
        {
            HtmlDocument document = new HtmlDocument();
            document.LoadHtml(htmlContent);

            var categoryPath = "//div[@class='filter-item clearfix']";

            var categoryNodes = document.DocumentNode.SelectNodes(categoryPath);

            if (categoryNodes != null && categoryNodes.Count > 0)
            {
                foreach (var node in categoryNodes)
                {
                    var rootNode = node.ChildNodes.FindFirst("label");

                    if (rootNode != null)
                    {
                        MangaCategory temp = new MangaCategory();
                        temp.SourceType = MangaCategorySourceType.憨憨漫画;
                        temp.RootCategory = rootNode.InnerHtml;

                        var categoryNode = node.ChildNodes.FindFirst("ul").ChildNodes;

                        foreach (var subNode in categoryNode)
                        {
                            var aTag = subNode.ChildNodes.FindFirst("a");

                            if (aTag != null)
                            {
                                var url = aTag.Attributes["href"].Value.Trim();
                                var name = aTag.InnerHtml.Trim();

                                if (!string.IsNullOrEmpty(url) && !string.IsNullOrEmpty(name))
                                {
                                    url = url.Substring(url.IndexOf("/list/") + "/list/".Length);

                                    temp.Url = string.IsNullOrEmpty(url) ? "" : url.Substring(0, url.LastIndexOf("/"));
                                    temp.Category = name;

                                    MangaDatabaseHelper.InsertMangaCategory(temp);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
