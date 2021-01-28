using AVWeb.Filter;
using DataBaseManager.JavDataBaseHelper;
using DataBaseManager.ScanDataBaseHelper;
using Microsoft.Win32.TaskScheduler;
using Model.Common;
using Model.JavModels;
using Model.ScanModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using Utils;
using WebService;

namespace AVWeb.Controllers
{
    /// <summary>
    /// 提供整理AV,播放AV的接口
    /// </summary>
    [RoutePrefix("avapi")]
    public class AVApiController : ApiController
    {
        private static log4net.ILog _log = log4net.LogManager.GetLogger(typeof(AVApiController));

        [Base]
        /// <summary>
        /// 获取与输入文件名最接近的AV匹配记录,不要输入文件后缀名
        /// </summary>
        /// <param name="inputName">文件名不要后缀,比如"某某某大神发布的VDD100HD"</param>
        /// <returns>最接近的所有AV详细资料列表</returns>
        [Route("GetClosetMatchResult")]
        [HttpGet]
        public List<AV> GetClosetMatchResult(string inputName)
        {
            return WebService.WebService.GetClosetMatch(inputName);
        }

        [Base]
        /// <summary>
        /// 根据获取AV详细资料列表,参数都为非必须. 默认按照发行日期倒序,每页20
        /// </summary>
        /// <param name="page">页数</param>
        /// <param name="pageSize">分页大小</param>
        /// <param name="id">番号格式(xxx-xxx)</param>
        /// <param name="category">分类 例如 巨乳</param>
        /// <param name="actress">演员名称 例如 波多野结衣</param>
        /// <param name="director">导演名称 例如 庵野</param>
        /// <param name="company">制作公司名称 例如 XX</param>
        /// <param name="publisher">发行型公司名称 例如 XX</param>
        /// <param name="releaseDate">发行日期 例如 2010-01-01</param>
        /// <param name="orderBy">按照什么字段排序 比如 ReleaseDate, ID, Actress等等 就是上述的参数</param>
        /// <param name="orderType">DESC 或者 ASC</param>
        /// <returns>AV详细资料列表</returns>
        [Route("GetAv")]
        [HttpGet]
        public List<AV> GetAv(int page = 1, int pageSize = 20, string id = "", string category = "", string actress = "", string director = "", string company = "", string publisher = "", string releaseDate = "", string orderBy = " ReleaseDate ", string orderType = " DESC ")
        {
            List<AV> ret = new List<AV>();
            string orderStr = orderBy + orderType;
            string where = " 1 = 1 ";
            string pageStr = @" AND t.OnePage BETWEEN " + (((page - 1) * pageSize) + 1) + " AND " + page * pageSize; ;

            if (!string.IsNullOrEmpty(id))
            {
                where += string.Format(" AND ID = '{0}' ", id);
            }

            if (!string.IsNullOrEmpty(category))
            {
                where += string.Format(" AND Category LIKE '%{0}%' ", category);
            }

            if (!string.IsNullOrEmpty(actress))
            {
                where += string.Format(" AND Actress LIKE '%{0}%' ", actress);
            }

            if (!string.IsNullOrEmpty(director))
            {
                where += string.Format(" AND Director LIKE '%{0}%' ", director);
            }

            if (!string.IsNullOrEmpty(company))
            {
                where += string.Format(" AND Company LIKE '%{0}%' ", company);
            }

            if (!string.IsNullOrEmpty(publisher))
            {
                where += string.Format(" AND Publisher = '%{0}%' ", publisher);
            }

            if (!string.IsNullOrEmpty(releaseDate))
            {
                var date = DateTime.Parse(releaseDate);

                where += string.Format(" AND ReleaseDate = '{0}' ", date.ToString("yyyy-MM-dd") + " 00:00:00.000");
            }

            return WebService.WebService.GetAv(orderBy, where, pageStr);
        }

        [Base]
        /// <summary>
        /// 播放AV的流地址,目前测试阶段,传入从GetMatch接口获得的filePath
        /// </summary>
        /// <param name="filename">输入文件地址</param>
        /// <returns>异步视频流,HTML用video标签接收</returns>
        [Route("PlayAv")]
        [HttpGet]
        public HttpResponseMessage PlayAv(string filename)
        {
            filename = HttpUtility.UrlDecode(filename);
            _log.Debug(filename);
            if (Request.Headers.Range != null)
            {
                try
                {
                    Encoder stringEncoder = Encoding.UTF8.GetEncoder();
                    byte[] stringBytes = new byte[stringEncoder.GetByteCount(filename.ToCharArray(), 0, filename.Length, true)];
                    stringEncoder.GetBytes(filename.ToCharArray(), 0, filename.Length, stringBytes, 0, true);
                    MD5CryptoServiceProvider MD5Enc = new MD5CryptoServiceProvider();
                    string hash = BitConverter.ToString(MD5Enc.ComputeHash(stringBytes)).Replace("-", string.Empty);

                    HttpResponseMessage partialResponse = Request.CreateResponse(HttpStatusCode.PartialContent);
                    partialResponse.Headers.AcceptRanges.Add("bytes");
                    partialResponse.Headers.ETag = new EntityTagHeaderValue("\"" + hash + "\"");

                    var stream = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    partialResponse.Content = new ByteRangeStreamContent(stream, Request.Headers.Range, new MediaTypeHeaderValue("video/mp4"));
                    return partialResponse;
                }
                catch (Exception ex)
                {
                    _log.Debug(ex.ToString());
                    return new HttpResponseMessage(HttpStatusCode.InternalServerError);
                }
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.RequestedRangeNotSatisfiable);
            }
        }

        [Base]
        /// <summary>
        /// 获取分类信息
        /// </summary>
        /// <param name="type">需要按照什么类别查询,分别有actress,category,director,company,publisher</param>
        /// <returns>对应类别的查询url</returns>
        [Route("GetNames")]
        [HttpGet]
        public List<CommonModel> GetNames(string type)
        {
            return WebService.WebService.GetNames(type);
        }

        [Base]
        /// <summary>
        /// 与GetAv相同,唯一区别在于这个接口返回的AV都可以用PlayAv接口播放,传入Id即可
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <param name="actress"></param>
        /// <param name="director"></param>
        /// <param name="company"></param>
        /// <param name="publisher"></param>
        /// <param name="releaseDate"></param>
        /// <param name="orderBy"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        [Route("GetMatch")]
        [HttpGet]
        public List<MatchMap> GetMatch(int page = 1, int pageSize = 20, string id = "", string category = "", string actress = "", string director = "", string company = "", string publisher = "", string releaseDate = "", string orderBy = " a.ReleaseDate ", string orderType = " DESC ")
        {
            //string ret = "";
            List<MatchMap> ret = new List<MatchMap>();

            string orderStr = orderBy + orderType;
            string where = " 1 = 1 ";
            string pageStr = @" AND t.OnePage BETWEEN " + (((page - 1) * pageSize) + 1) + " AND " + page * pageSize; ;

            if (!string.IsNullOrEmpty(id))
            {
                where += string.Format(" AND m.ID = '{0}' ", id);
            }

            if (!string.IsNullOrEmpty(category))
            {
                where += string.Format(" AND a.Category LIKE '%{0}%' ", category);
            }

            if (!string.IsNullOrEmpty(actress))
            {
                where += string.Format(" AND a.Actress LIKE '%{0}%' ", actress);
            }

            if (!string.IsNullOrEmpty(director))
            {
                where += string.Format(" AND a.Director LIKE '%{0}%' ", director);
            }

            if (!string.IsNullOrEmpty(company))
            {
                where += string.Format(" AND a.Company LIKE '%{0}%' ", company);
            }

            if (!string.IsNullOrEmpty(publisher))
            {
                where += string.Format(" AND a.Publisher = '%{0}%' ", publisher);
            }

            if (!string.IsNullOrEmpty(releaseDate))
            {
                var date = DateTime.Parse(releaseDate);

                where += string.Format(" AND a.ReleaseDate = '{0}' ", date.ToString("yyyy-MM-dd") + " 00:00:00.000");
            }

            return WebService.WebService.GetMatch(orderStr, where, pageStr);
        }

        [Base]
        /// <summary>
        /// 获取所有没有匹配的AV
        /// </summary>
        /// <param name="includePlayed">是否包含播放过的</param>
        /// <returns>MatchMap></returns>
        [Route("GetUnMatched")]
        [HttpGet]
        public List<UnmatchVW> GetUnMatched(bool includePlayed = true)
        {
            List<UnmatchVW> ret = new List<UnmatchVW>();

            ret = WebService.WebService.GetUnMatch(includePlayed);

            return ret;
        }

        [Base]
        /// <summary>
        /// 上传需要自动化下载的种子文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("PostSeedFiles")]
        public string PostSeedFiles()
        {
            return PostFiles(HttpContext.Current.Request.Files, "c:\\FileUpload\\Seeds\\", false, ".torrent");
        }

        [Base]
        [HttpPost]
        [Route("PostComicFiles")]
        public string PostComicFiles()
        {
            return PostFiles(HttpContext.Current.Request.Files, "G:\\AVWeb\\ComicDownload\\", false, ".zip");
        }

        [HttpGet]
        [Route("GetNextRunTime")]
        public NextRunModel GetNextRunTime(string token, string name = "ScanJavJob")
        {
            NextRunModel ret = new NextRunModel();
            var to = ScanDataBaseManager.GetToken().Token;

            if (to == token)
            {
                TaskService ts = new TaskService();
                var task = ts.FindTask("ScanJavJob");

                if (task != null)
                {
                    ret.NextRunTime = task.NextRunTime;
                    ret.NextRunCountMinutes = (int)Math.Ceiling((task.NextRunTime - DateTime.Now).TotalMinutes);
                }
            }

            return ret;
        }


        [HttpGet]
        [Route("SiriRunJob")]
        public TaskCommonModel SiriRunJob(string token, string jobName = "SiriRun", int page = 15)
        {
            TaskCommonModel ret = new TaskCommonModel();
            var to = ScanDataBaseManager.GetToken().Token;

            if (to == token)
            {
                var parameter = new ScanParameter();
                parameter.IsAsc = true;
                parameter.PageSize = page;
                parameter.StartingPage = new List<string>() { "http://www.javlibrary.com/cn/vl_update.php?mode=" };

                var jobId = ScanDataBaseManager.InsertScanJob(jobName, JsonConvert.SerializeObject(parameter));

                ret.Message = "建立成功";
            }
            else
            {
                ret.Message = "没有权限";
            }

            return ret;
        }


        [HttpGet]
        [Route("RunTask")]
        public TaskCommonModel RunTask(string token, string name = "ScanJavJob")
        {
            TaskCommonModel ret = new TaskCommonModel();
            var to = ScanDataBaseManager.GetToken().Token;

            if (to == token)
            {
                TaskService ts = new TaskService();
                var task = ts.FindTask(name);

                ret.Message = "程序没有执行";

                if (task != null && task.State == TaskState.Ready)
                {
                    task.Run();

                    ret.Message = "开始执行";
                }
            }
            else
            {
                ret.Message = "没有权限";
            }

            return ret;
        }

        [HttpGet]
        [Route("EverythingSearch")]
        public Model.ScanModels.EverythingResult EverythingSearch(string token, string content)
        {
            var to = ScanDataBaseManager.GetToken().Token;

            if (to == token)
            {
                var htmlModel = HtmlManager.GetHtmlContentViaUrl("http://localhost:8086/" + @"?s=&o=0&j=1&p=c&path_column=1&size_column=1&j=1&q=!c:\ " + EverythingHelper.Extensions + " " + content);

                if (htmlModel.Success)
                {
                    var retModel = JsonConvert.DeserializeObject<Model.ScanModels.EverythingResult>(htmlModel.Content);

                    if (retModel != null && retModel.results != null)
                    {
                        retModel.results = retModel.results.OrderByDescending(x => double.Parse(x.size)).ToList();

                        foreach (var r in retModel.results)
                        {
                            r.sizeStr = FileSize.GetAutoSizeString(double.Parse(r.size), 1);
                        }

                        return retModel;
                    }
                }
            }

            return new Model.ScanModels.EverythingResult();
        }

        [HttpGet]
        [Route("GetReport")]
        public ReportVM GetReport(string token, int top = 5)
        {
            ReportVM ret = new ReportVM();
            var to = ScanDataBaseManager.GetToken().Token;

            if (to == token)
            {
                StringBuilder sb = new StringBuilder();
                var report = ScanDataBaseManager.GetReport();
                var items = ScanDataBaseManager.ReportItem(report.ReportId);

                ret.TotalCount = report.TotalExist;
                sb.AppendLine($"总Av数量: [{ret.TotalCount}]");
                ret.TotalSizeStr = FileSize.GetAutoSizeString((double)report.TotalExistSize, 1);
                sb.AppendLine($"总Av大小: [{ret.TotalSizeStr}]");
                ret.TotalSize = (double)report.TotalExistSize;
                ret.ChineseCount = report.ChineseCount;
                sb.AppendLine($"中文Av数量: [{ret.ChineseCount}]");
                ret.FileLessThan1G = report.LessThenOneGiga;
                sb.AppendLine($"文件小于1GB: [{ret.FileLessThan1G}]");
                ret.FileLargeThan1G = report.OneGigaToTwo;
                sb.AppendLine($"大于1GB小于2GB: [{ret.FileLargeThan1G}]");
                ret.FileLargeThan2G = report.TwoGigaToFour;
                sb.AppendLine($"大于2GB小于4GB: [{ret.FileLargeThan2G}]");
                ret.FileLargeThan4G = report.FourGigaToSix;
                sb.AppendLine($"大于4GB小于6GB: [{ret.FileLargeThan4G}]");
                ret.FileLargeThan6G = report.GreaterThenSixGiga;
                sb.AppendLine($"文件大于6GB: [{ret.FileLargeThan6G}]");

                var extensionModel = JsonConvert.DeserializeObject<Dictionary<string, int>>(report.Extension);

                ret.Formats = extensionModel;
                sb.AppendLine("后缀分布:");
                foreach (var ext in extensionModel)
                {
                    sb.AppendLine($"\t{ext.Key} : {ext.Value}");
                }

                foreach (ReportType type in Enum.GetValues(typeof(ReportType)))
                {
                    List<ReportItem> i = new List<ReportItem>();
                    switch (type)
                    {
                        case ReportType.Actress:
                            i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(top).ToList();

                            sb.AppendLine("女优TOP" + top);

                            foreach (var temp in i)
                            {
                                var name = temp.ItemName;
                                var count = temp.ExistCount;
                                var ratio = $"{temp.ExistCount} / {temp.TotalCount}";
                                var size = FileSize.GetAutoSizeString(temp.TotalSize, 1);

                                sb.AppendLine($"\t{name} -> 作品 {ratio}，总大小 {size}");
                            }

                            break;
                        case ReportType.Category:
                            i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(top).ToList();

                            sb.AppendLine("分类TOP" + top);

                            foreach (var temp in i)
                            {
                                var name = temp.ItemName;
                                var count = temp.ExistCount;
                                var ratio = $"{temp.ExistCount} / {temp.TotalCount}";
                                var size = FileSize.GetAutoSizeString(temp.TotalSize, 1);

                                sb.AppendLine($"\t{name} -> 作品 {ratio}，总大小 {size}");
                            }

                            break;                
                        case ReportType.Prefix:
                            i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(top).ToList();

                            sb.AppendLine("番号TOP" + top);

                            foreach (var temp in i)
                            {
                                var name = temp.ItemName;
                                var count = temp.ExistCount;
                                var ratio = $"{temp.ExistCount} / {temp.TotalCount}";
                                var size = FileSize.GetAutoSizeString(temp.TotalSize, 1);

                                sb.AppendLine($"\t{name} -> 作品 {ratio}，总大小 {size}");
                            }

                            break;
                        case ReportType.Company:
                            i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(top).ToList();

                            sb.AppendLine("公司TOP" + top);

                            foreach (var temp in i)
                            {
                                var name = temp.ItemName;
                                var count = temp.ExistCount;
                                var ratio = $"{temp.ExistCount} / {temp.TotalCount}";
                                var size = FileSize.GetAutoSizeString(temp.TotalSize, 1);

                                sb.AppendLine($"\t{name} -> 作品 {ratio}，总大小 {size}");
                            }

                            break;
                        case ReportType.Date:
                            i = items.Where(x => (ReportType)x.ReportType == type).OrderByDescending(x => x.ExistCount).Take(top).ToList();

                            sb.AppendLine("日期TOP" + top);

                            foreach (var temp in i)
                            {
                                var name = temp.ItemName;
                                var count = temp.ExistCount;
                                var ratio = $"{temp.ExistCount} / {temp.TotalCount}";
                                var size = FileSize.GetAutoSizeString(temp.TotalSize, 1);

                                sb.AppendLine($"\t{name} -> 作品 {ratio}，总大小 {size}");
                            }

                            break;
                    }
                }

                ret.ShowContent = sb.ToString();

            }

            return ret;
        }

        #region 工具
        private string PostFiles(HttpFileCollection filelist, string folder, bool addDate, string ext)
        {
            StringBuilder sb = new StringBuilder();

            if (filelist != null && filelist.Count > 0)
            {
                for (int i = 0; i < filelist.Count; i++)
                {
                    HttpPostedFile file = filelist[i];
                    string fileName = file.FileName;

                    if (fileName.ToLower().Contains(ext))
                    {
                        if (addDate)
                        {
                            folder = folder + DateTime.Now.ToString("yyyy-MM-dd") + "\\";
                        }

                        DirectoryInfo di = new DirectoryInfo(folder);

                        if (!di.Exists)
                        {
                            di.Create();
                        }

                        try
                        {
                            file.SaveAs(folder + fileName);
                            sb.AppendLine("上传文件写入成功: " + (folder + fileName).Replace("\\", "/") );
                        }
                        catch (Exception ex)
                        {
                            sb.AppendLine("上传文件写入失败: " + fileName + Environment.NewLine + ex.ToString());
                        }
                    }
                    else {
                        sb.AppendLine("传入格式不正确: " + fileName);
                    }
                }
            }
            else
            {
                sb.AppendLine("上传的文件信息不存在！");
            }

            return sb.ToString();
        }
        #endregion
    }
}
