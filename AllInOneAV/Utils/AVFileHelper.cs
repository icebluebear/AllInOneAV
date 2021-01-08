using Model.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Utils
{
    public class AVFileHelper
    {
        public static string GetAvName(string file)
        {
            FileInfo fi = null;

            if (File.Exists(file))
            {
                fi = new FileInfo(file);

                return GetAvNameHelper(fi);
            }

            return "";
        }

        public static string GetAvName(FileInfo file)
        {
            if (file != null)
            {
                return GetAvNameHelper(file);
            }

            return "";
        }

        public static AvFile ParseAvFile(string file)
        {
            AvFile ret = new AvFile();

            if (File.Exists(file))
            {
                ret = ParseAvFileHelper(new FileInfo(file));
            }

            return ret;
        }

        public static AvFile ParseAvFile(FileInfo file)
        {
            AvFile ret = new AvFile();

            if (file != null)
            {
                ret = ParseAvFileHelper(file);
            }

            return ret;
        }

        public static bool IsReformated(string file)
        {
            bool ret = false;

            if (File.Exists(file))
            {
                ret = IsReformatedHelper(new FileInfo(file));
            }

            return ret;
        }

        public static bool IsReformated(FileInfo file)
        {
            bool ret = false;

            if (file != null)
            {
                ret = IsReformatedHelper(file);
            }

            return ret;
        }

        private static string GetAvNameHelper(FileInfo file)
        {
            var lowerName = file.Name.ToLower();
            var lowerFolder = file.DirectoryName;
            var avId = "";
            var avName = "";
            var isChinese = false;
            var isH265 = false;
            var isConvertFaile = false;

            var extesion = file.Extension.ToLower();

            lowerName = lowerName.Replace(extesion, "");
            string[] split = lowerName.Split('-');

            if (split.Length >= 3)
            {
                avId = split[0] + "-" + split[1];

                lowerName = lowerName.Replace(avId + "-", "");

                if (lowerName.Contains("-nomerge"))
                {
                    isConvertFaile = true;

                    lowerName = lowerName.Replace("-nomerge", "");
                }

                if (lowerName.Contains("-c" + extesion))
                {
                    isChinese = true;

                    lowerName = lowerName.Replace("-c" + extesion, "");
                }

                isH265 = FileUtility.IsH265(file.FullName, @"c:\setting\ffmpeg.exe").Result;

                avName = lowerName;
            }

            var ret = string.Format("{0}[{1}][{2}][{3}][{4}][{5}]{6}", lowerFolder + "\\", isH265 ? "H265" : "H264", avId, avName, isChinese ? "CN" : "JP", isConvertFaile ? "NOMERGE" : "NORMAL", extesion );

            return ret;
        }

        private static AvFile ParseAvFileHelper(FileInfo file)
        {
            AvFile ret = new AvFile();

            var reg = @"\[(.*?)\]\[(.*?)\]\[(.*?)\]\[(.*?)\]\[(.*?)\]";
            var name = file.Name.Replace(file.Extension, "");

            var matches = Regex.Matches(name, reg);

            if (matches.Count <= 0)
            {
                return null;
            }

            foreach (Match m in matches)
            {
                ret.IsH265 = m.Groups[1].Value == "H265";
                ret.AvId = m.Groups[2].Value;
                ret.AvName = m.Groups[3].Value;
                ret.IsChinese = m.Groups[4].Value == "CN";
                ret.IsConvertFail = m.Groups[5].Value == "NOMERGE";
            }

            return ret;
        }

        private static bool IsReformatedHelper(FileInfo file)
        {
            bool ret = false;

            var reg = @"\[(.*?)\]\[(.*?)\]\[(.*?)\]\[(.*?)\]\[(.*?)\]";
            var name = file.Name.Replace(file.Extension, "");

            var matches = Regex.Matches(name, reg);

            if (matches.Count <= 0)
            {
                ret = false;
            }
            else
            {
                ret = true;
            }

            return ret;
        }
    }
}
