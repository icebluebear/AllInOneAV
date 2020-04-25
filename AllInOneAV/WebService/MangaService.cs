using AVWeb.Models;
using DataBaseManager.MangaDataBaseHelper;
using HtmlAgilityPack;
using Model.MangaModel;
using Model.WebModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Utils;

namespace WebService
{
    public class MangaService
    {
        public static MangaCategoryVM GetMangaCategory(MangaCategorySourceType sourceType)
        {
            MangaCategoryVM ret = new MangaCategoryVM();
            ret.Categories = new List<MangaCategoryItem>();

            var model = MangaDatabaseHelper.GetMangaCategoryByType(sourceType);

            if (model != null && model.Count > 0)
            {
                foreach (var m in model)
                {
                    MangaCategoryItem temp = new MangaCategoryItem();

                    temp.Category = m.Category;
                    temp.IsRoot = m.Category == "全部";
                    temp.RootCategory = m.RootCategory;
                    temp.Url = m.Url;

                    ret.Categories.Add(temp);
                }
            }
            else
            {
                ret.MsgCode = VMCode.Error;
                ret.Msg = "没有找到Type = " + sourceType + " 的漫画分类";
            }

            return ret;
        }

        public static MangaCategoryListVM GetMangaCategoryList(MangaCategorySourceType sourceType, string category, int page)
        {
            MangaCategoryListVM ret = new MangaCategoryListVM();

            switch (sourceType)
            {
                case MangaCategorySourceType.憨憨漫画:
                    ret = GetManageCategoryListHanhan(category.Split(','), page);
                    break;
            }

            return ret;
        }

        public static MangaDetailVM GetMangaDetail(MangaCategorySourceType sourceType, string path)
        {
            MangaDetailVM ret = new MangaDetailVM();

            switch (sourceType)
            {
                case MangaCategorySourceType.憨憨漫画:
                    ret = GetMangaDetailHanhan(sourceType, path);
                    break;
            }

            return ret;
        }

        public static MangaDetailVM GetMangaDetailHanhan(MangaCategorySourceType sourceType, string path)
        {
            MangaDetailVM ret = new MangaDetailVM();
            ret.Chapters = new List<MangaChapter>();

            var htmlRet = HtmlManager.GetHtmlWebClient("http://www.hanhan.net", path);

            if (htmlRet.Success)
            {
                try
                {
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(htmlRet.Content);

                    var picPath = "//img[@class='pic']";
                    var detailPath = "//ul[@class='detail-list cf']/li";
                    var infoPath = "//div[@id='intro-all']//p";
                    var chapterPath = "//ul[@id='chapter-list-4']//a";

                    var picNode = document.DocumentNode.SelectSingleNode(picPath);
                    var infoNode = document.DocumentNode.SelectSingleNode(infoPath);

                    var chapterNodes = document.DocumentNode.SelectNodes(chapterPath);
                    var detailNodes = document.DocumentNode.SelectNodes(detailPath);

                    if (picNode != null)
                    {
                        ret.PicUrl = picNode.Attributes["src"].Value.Trim();
                        ret.MangaName = picNode.Attributes["alt"].Value.Trim();
                    }

                    if (detailNodes != null && detailNodes.Count > 0)
                    {
                        foreach (var node in detailNodes)
                        {
                            if (node.Attributes.Count <= 0)
                            {
                                foreach (var subNode in node.ChildNodes)
                                {
                                    if (subNode.InnerText.StartsWith("漫画作者："))
                                    {
                                        ret.Author = subNode.InnerText.Replace("漫画作者：", "");
                                    }
                                }
                            }
                            else
                            {
                                ret.MangaStatus = node.ChildNodes.FindFirst("a").InnerHtml;

                                ret.UpdateDate = DateTime.Parse(node.ChildNodes[1].ChildNodes[5].InnerText);
                                ret.UpdateInfo = "更新到：" + node.ChildNodes[1].ChildNodes[7].InnerText;
                            }
                        }
                    }

                    if (infoNode != null)
                    {
                        ret.Description = infoNode.InnerText.Trim();
                    }

                    if (chapterNodes != null)
                    {
                        foreach (var node in chapterNodes)
                        {
                            ret.Chapters.Add(new MangaChapter
                            {
                                Url = "http://www.hanhande.net" + node.Attributes["href"].Value.Trim(),
                                ChapterName = node.ChildNodes.FindFirst("span").InnerText.Trim()
                            });
                        }
                    }
                }
                catch (Exception e)
                {
                    ret.MsgCode = VMCode.Exception;
                    ret.Msg = e.ToString();
                }
            }
            else
            {
                ret.MsgCode = VMCode.Success;
                ret.Msg = "网页获取失败";
            }

            return ret;
        }

        public static MangaPicturesVM GetMangaPic(MangaCategorySourceType sourceType, string path)
        {
            MangaPicturesVM ret = new MangaPicturesVM();

            switch (sourceType)
            {
                case MangaCategorySourceType.憨憨漫画:
                    ret = GetMangePicHanhan(sourceType, path);
                    break;
            }

            return ret;
        }

        private static MangaCategoryListVM GetManageCategoryListHanhan(string[] category, int page)
        {
            MangaCategoryListVM ret = new MangaCategoryListVM();
            ret.Mangas = new List<MangaCategoryListItem>();
            ret.CurrentPage = page;

            var urlPrefix = "http://www.hanhande.net/";
            var url = "list";
            var searchUrl = "";

            if (category.Length == 1 && string.IsNullOrEmpty(category[0]))
            {
                url += "_" + page + "/";
            }
            else
            {
                url += "/" + string.Join("-", category) + "/" + page + "/";
            }

            searchUrl = urlPrefix + url;
            var htmlRet = HtmlManager.GetHtmlWebClient("http://www.hanhan.net", searchUrl);

            if (htmlRet.Success)
            {
                try
                {
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(htmlRet.Content);

                    var listPath = "//li[@class='item-lg']";
                    var pagePath = "//li[@class='last']";

                    var listNodes = document.DocumentNode.SelectNodes(listPath);
                    var pageNode = document.DocumentNode.SelectSingleNode(pagePath);

                    if (pageNode != null && pageNode.ChildNodes.FindFirst("a") != null)
                    {
                        int total = -1;

                        int.TryParse(pageNode.ChildNodes.FindFirst("a").Attributes["data-page"].Value.Trim(), out total);

                        ret.TotalPage = total + 1;
                    }

                    if (listNodes != null)
                    {
                        foreach (var node in listNodes)
                        {
                            var aTag = node.ChildNodes.FindFirst("a");

                            if (aTag != null)
                            {
                                MangaCategoryListItem temp = new MangaCategoryListItem();

                                temp.MangaUrl = aTag.Attributes["href"].Value.Trim();
                                temp.MangaName = aTag.Attributes["title"].Value.Trim();

                                foreach (var subNode in aTag.ChildNodes)
                                {
                                    if (subNode.Name == "img")
                                    {
                                        temp.PicUrl = subNode.Attributes["src"].Value.Trim();
                                    }

                                    if (subNode.Name == "span" && subNode.Attributes["class"].Value.Trim() == "fd")
                                    {
                                        temp.IsFinished = true;
                                    }

                                    if (subNode.Name == "span" && subNode.Attributes["class"].Value.Trim() == "tt")
                                    {
                                        temp.UpdateInfo = subNode.InnerHtml.Trim();
                                    }
                                }

                                foreach (var subNode in node.ChildNodes)
                                {
                                    DateTime updateDate = DateTime.Now;

                                    if (subNode.Name == "span" && subNode.Attributes["class"].Value.Trim() == "updateon")
                                    {
                                        DateTime.TryParse(subNode.InnerHtml.Substring(subNode.InnerHtml.IndexOf("：") + 1, subNode.InnerHtml.IndexOf(" ")), out updateDate);
                                        temp.UpdateDate = updateDate;
                                    }
                                }

                                ret.Mangas.Add(temp);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    ret.MsgCode = VMCode.Exception;
                    ret.Msg = e.ToString();
                }
            }
            else
            {
                ret.MsgCode = VMCode.Error;
                ret.Msg = "网页获取失败";
            }

            ret.PageSize = ret.Mangas.Count;
            return ret;
        }

        private static MangaPicturesVM GetMangePicHanhan(MangaCategorySourceType sourceType, string path)
        {
            MangaPicturesVM ret = new MangaPicturesVM();
            ret.PicList = new List<string>();

            var htmlRet = HtmlManager.GetHtmlWebClient("http://www.hanhan.net", path);

            if (htmlRet.Success)
            {
                try
                {
                    HtmlDocument htmlDocument = new HtmlDocument();
                    htmlDocument.LoadHtml(htmlRet.Content);

                    var urlPath = "//link[@rel='miphtml']";

                    var urlNode = htmlDocument.DocumentNode.SelectSingleNode(urlPath);

                    var decrypt = htmlRet.Content.Substring(htmlRet.Content.IndexOf("chapterImages = ") + "chapterImages = ".Length);
                    decrypt = decrypt.Substring(0, decrypt.IndexOf("]") + 1);

                    var urlHeader = htmlRet.Content.Substring(htmlRet.Content.IndexOf("chapterPath = \"") + "chapterPath = \"".Length);
                    urlHeader = urlHeader.Substring(0, urlHeader.IndexOf("\""));

                    var picList = JsonConvert.DeserializeObject<List<string>>(decrypt);

                    foreach (var pic in picList)
                    {
                        if (!pic.StartsWith("http"))
                        {
                            ret.PicList.Add("http://img001.shaque.vip/" + urlHeader + pic);
                        }
                        else
                        {
                            ret.PicList.Add(pic);
                        }
                    }
                }
                catch (Exception e)
                {
                    ret.MsgCode = VMCode.Exception;
                    ret.Msg = e.ToString();
                }
            }
            else
            {
                ret.MsgCode = VMCode.Error;
                ret.Msg = "获取网页失败";
            }

            return ret;
        }
    }
}
