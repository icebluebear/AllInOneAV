using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utils
{
    public class ApplicationLog
    {
        static ILog _Log = LogManager.GetLogger("root");

        public static void Debug(string msg)
        {
            _Log.Debug(msg);
        }

        public static void Debug(string msg, Exception e)
        {
            _Log.Debug(msg + GenerateContextStr() + e.ToString());
        }

        public static void Info(string msg)
        {
            _Log.Info(msg);
        }

        public static void Warn(string msg)
        {
            _Log.Warn(msg);
        }

        public static void Error(string msg)
        {
            _Log.Error(msg);
        }

        public static void Error(Exception e)
        {
            _Log.Error(GenerateContextStr() + e.ToString());
        }

        public static void Error(Exception e, string label)
        {
            var labelStr = "[" + label + "]" + Environment.NewLine;

            _Log.Error(labelStr + GenerateContextStr() + e.ToString());
        }

        #region front logger
        static ILog frontLog = LogManager.GetLogger("FrontLog");

        /// <summary>
        /// 专门给前端看的日志
        /// </summary>
        /// <param name="log"></param>
        /// <param name="label"></param>
        public static void FrontLog(string log, string label = "")
        {
            var labelStr = !string.IsNullOrEmpty(label) ? ("[" + label + "]" + Environment.NewLine) : "";
            frontLog.Info(labelStr + log + Environment.NewLine + GenerateContextStr());
        }
        #endregion


        private static string GenerateContextStr()
        {
            StringBuilder sb = new StringBuilder();
            if (HttpContext.Current != null)
            {
                var param = string.Format("[Header:{0}]", HttpContext.Current.Request.Headers);
                if (HttpContext.Current.Request.HttpMethod == "POST")
                {
                    param += string.Format("[Form:{0}]", HttpContext.Current.Request.Form);
                }
                sb.AppendLine("[URL]" + HttpContext.Current.Request.Url);
                sb.AppendLine("[请求参数]" + param);
                sb.AppendLine("[UserAgent]" + HttpContext.Current.Request.UserAgent);
            }

            return HttpUtility.UrlDecode(sb.ToString());
        }
    }
}
