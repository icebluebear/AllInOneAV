using HtmlAgilityPack;
using Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class SearchSeedHelper
    {
        public static List<SeedMagnetSearchModel> SearchBtsow(string id)
        {
            //var c = HtmlManager.GetCookies("https://btsow.club/search/");
            //CookieContainer cc = new CookieContainer();
            //cc.Add(c);

            List<SeedMagnetSearchModel> ret = new List<SeedMagnetSearchModel>();

            try
            {
                var serachContent = "https://btsow.club/search/" + id;
                var htmlRet = HtmlManager.GetHtmlWebClient("https://btsow.club", serachContent, null, true);

                if (htmlRet.Success)
                {
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlRet.Content);

                    string xpath = "//div[@class='row']";

                    HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes(xpath);

                    foreach (var node in nodes.Take(nodes.Count - 1))
                    {
                        var text = node.ChildNodes[1].ChildNodes[1].InnerText.Trim();
                        var size = FileUtility.GetFileSizeFromString(node.ChildNodes[3].InnerText.Trim());
                        var date = node.ChildNodes[5].InnerText.Trim();
                        var a = node.ChildNodes[1].OuterHtml;
                        var url = a.Substring(a.IndexOf("\"") + 1);
                        url = url.Substring(0, url.IndexOf("\""));

                        SeedMagnetSearchModel temp = new SeedMagnetSearchModel
                        {
                            Title = text,
                            Size = size,
                            Date = DateTime.Parse(date),
                            Url = url,
                            Source = SearchSeedSiteEnum.Btsow
                        };

                        ret.Add(temp);
                    }

                    foreach (var r in ret)
                    {
                        var subHtmlRet = HtmlManager.GetHtmlContentViaUrl(r.Url);

                        if (subHtmlRet.Success)
                        {
                            htmlDocument = new HtmlDocument();
                            htmlDocument.LoadHtml(subHtmlRet.Content);

                            xpath = "//textarea[@class='magnet-link hidden-xs']";

                            HtmlNode node = htmlDocument.DocumentNode.SelectSingleNode(xpath);

                            if (node != null)
                            {
                                r.MagUrl = node.InnerText;
                            }
                        }
                    }
                }
            }
            catch (Exception ee)
            {

            }

            return ret.OrderByDescending(x=>x.Size).ToList();
        }

        public static List<SeedMagnetSearchModel> SearchSukebei(string id, CookieContainer cc = null)
        {
            //if (cc == null)
            //{
            //    var c = HtmlManager.GetCookies("https://sukebei.nyaa.si/");
            //    cc = new CookieContainer();
            //    cc.Add(c);
            //}

            List<SeedMagnetSearchModel> ret = new List<SeedMagnetSearchModel>();

            try
            {
                var serachContent = "https://sukebei.nyaa.si/?f=0&c=0_0&q=" + id;
                var htmlRet = HtmlManager.GetHtmlWebClient("https://sukebei.nyaa.si", serachContent, cc);

                if (htmlRet.Success)
                {
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlRet.Content);

                    string xpath = "//tr";

                    HtmlNodeCollection nodes = htmlDocument.DocumentNode.SelectNodes(xpath);

                    foreach (var node in nodes.Skip(1))
                    {
                        var text = node.ChildNodes[3].InnerText.Trim();
                        var a = node.ChildNodes[5].OuterHtml;
                        var size = node.ChildNodes[7].InnerText.Trim();
                        var date = node.ChildNodes[9].InnerText.Trim();
                        var complete = node.ChildNodes[15].InnerText.Trim();

                        var url = a.Substring(a.IndexOf("<a href=\"magnet:?xt") + 9);
                        url = url.Substring(0, url.IndexOf("\""));

                        SeedMagnetSearchModel temp = new SeedMagnetSearchModel
                        {
                            Title = text,
                            Size = FileUtility.GetFileSizeFromString(size),
                            Date = DateTime.Parse(date),
                            Url = "",
                            CompleteCount = int.Parse(complete),
                            MagUrl = url,
                            Source = SearchSeedSiteEnum.Sukebei
                        };

                        ret.Add(temp);
                    }
                }
            }
            catch (Exception ee)
            {

            }

            return ret.Where(x=>x.Size >= 0).OrderByDescending(x=>x.CompleteCount).ThenByDescending(x => x.Size).ToList();
        }
    }
}
