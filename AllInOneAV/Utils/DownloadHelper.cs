using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class DownloadHelper
    {
        public static string DownloadHttps(string url, string path, string agent, int retry = 3, int current = 1)
        {
            string ret = "";
            if (current <= retry)
            {
                try
                {
                    WebClient wc = new WebClient();
                    wc.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
                    if (!string.IsNullOrEmpty(agent))
                    {
                        wc.Headers.Add(HttpRequestHeader.UserAgent, agent);
                    }
                    wc.DownloadFile(url, path);
                }
                catch (Exception ee)
                {
                    Console.WriteLine("下载 " + url + " 失败, 重试");
                    DownloadHttps(url, path, agent, retry, current + 1);

                    ret = "下载 " + url + " 失败, 重试";
                }
            }

            return ret;
        }

        public static string DownloadHttpsWithHost(string url, string path, string host, string reffer, bool shaque = false, int retry = 3, int current = 1)
        {
            string ret = "";
            if (current <= retry)
            {
                try
                {
                    WebClient wc = new WebClient();
                    wc.Headers.Add(HttpRequestHeader.CacheControl, "no-cache");
                    wc.Headers.Add(HttpRequestHeader.Host, host);
                    wc.Headers.Add(HttpRequestHeader.Referer, reffer);
                    if (shaque)
                    {
                        wc.Headers.Add(HttpRequestHeader.AcceptEncoding, "gzip, deflate");
                        wc.Headers.Add(HttpRequestHeader.Accept, "image/webp,image/apng,image/*,*/*;q=0.8");
                    }
                    wc.DownloadFile(url, path);
                }
                catch (Exception ee)
                {
                    Console.WriteLine("下载 " + url + " 失败, 重试");
                    DownloadHttps(url, path, host, retry, current + 1);

                    ret = "下载 " + url + " 失败, 重试";
                }
            }

            return ret;
        }

        public static string DownloadFile(string url, string path, string host = "", string reff = "")
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            string ret = "";
            try
            {
                request.Proxy = null;
                if (!string.IsNullOrEmpty(host))
                {
                    request.Host = host;
                }

                if (!string.IsNullOrEmpty(reff))
                {
                    request.Referer = reff;
                }

                ServicePointManager.DefaultConnectionLimit = 5;
                ServicePointManager.Expect100Continue = false;
                //发送请求并获取相应回应数据
                //request.KeepAlive = false;
                //request.Timeout = 10000;
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();

                //创建本地文件写入流
                Stream stream = new FileStream(path, FileMode.Create);

                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
                response.Close();
            }
            catch (Exception e)
            {
                var exception = "Download file " + url + " failed" + e.ToString();
                Console.WriteLine(exception);
                ret = exception;
            }
            finally
            {
                if (request != null)
                {
                    request.Abort();
                }
            }

            return ret;
        }
    }
}
