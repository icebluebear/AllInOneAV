using DataBaseManager.JavDataBaseHelper;
using Model.JavModels;
using Model.WebModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace Service
{
    public class RenameService
    {
        private static readonly string imageFolder = JavINIClass.IniReadValue("Jav", "imgFolder");
        private static List<string> formats = JavINIClass.IniReadValue("Scan", "Format").Split(',').ToList();
        private static List<string> excludes = JavINIClass.IniReadValue("Scan", "Exclude").Split(',').ToList();

        public static Dictionary<string, List<RenameModel>> PrepareRename(string sourceFolder, string descFolder, int fileSizeLimit)
        {
            Dictionary<string, List<RenameModel>> ret = new Dictionary<string, List<RenameModel>>();
            Dictionary<string, int> MoveFileCheck = new Dictionary<string, int>();

            if (Directory.Exists(sourceFolder))
            {
                if (!Directory.Exists(descFolder))
                {
                    Directory.CreateDirectory(descFolder);
                }

                var files = new DirectoryInfo(sourceFolder).GetFiles();

                if (fileSizeLimit > 0)
                {
                    files = files.Where(x => x.Length >= fileSizeLimit * 1024 * 1024 * 1024).ToArray();
                }

                if (files.Count() > 0)
                {
                    var avs = JavDataBaseManager.GetAllAV();
                    List<string> allPrefix = new List<string>();

                    foreach (var name in avs.Select(x => x.ID).ToList())
                    {
                        var tempPrefix = name.Split('-')[0];
                        if (!allPrefix.Contains(tempPrefix))
                        {
                            allPrefix.Add(tempPrefix);
                        }
                    }

                    allPrefix = allPrefix.OrderByDescending(x => x.Length).ToList();

                    foreach (var file in files)
                    {
                        List<RenameModel> tempRet = new List<RenameModel>();
                        List<AV> possibleAv = new List<AV>();
                        var fileNameWithoutFormat = file.Name.Replace(file.Extension, "").ToLower();

                        foreach (var prefix in allPrefix)
                        {
                            var pattern = prefix + "{1}-?\\d{1,7}";
                            var matches = Regex.Matches(fileNameWithoutFormat, pattern, RegexOptions.IgnoreCase);

                            foreach (Match m in matches)
                            {
                                var possibleAvId = m.Groups[0].Value;

                                if (!possibleAvId.Contains("-"))
                                {
                                    bool isFirst = true;
                                    StringBuilder sb = new StringBuilder();

                                    foreach (var c in possibleAvId)
                                    {
                                        if (c >= '0' && c <= '9')
                                        {
                                            if (isFirst)
                                            {
                                                sb.Append("-");
                                                isFirst = false;
                                            }
                                        }
                                        sb.Append(c);
                                    }
                                    possibleAvId = sb.ToString();
                                }

                                var tempAv = JavDataBaseManager.GetAllAV(possibleAvId);

                                if (tempAv != null && tempAv.Count > 0)
                                {
                                    possibleAv.AddRange(tempAv);
                                }
                                else
                                {
                                    var prefixPart = possibleAvId.Split('-')[0];
                                    var numberPart = possibleAvId.Split('-')[1];

                                    while (numberPart.StartsWith("0"))
                                    {
                                        numberPart = numberPart.Substring(1);
                                        possibleAvId = prefixPart + "-" + numberPart;
                                        tempAv = JavDataBaseManager.GetAllAV(possibleAvId);
                                        if (tempAv != null && tempAv.Count > 0)
                                        {
                                            possibleAv.AddRange(tempAv);
                                        }
                                    }
                                }
                            }
                        }

                        foreach (var av in possibleAv)
                        {
                            var chinese = (fileNameWithoutFormat.EndsWith("-c") || fileNameWithoutFormat.EndsWith("-ch") || fileNameWithoutFormat.EndsWith("ch")) ? "-C" : "";

                            var tempName = descFolder + av.ID + "-" + av.Name + chinese + file.Extension;
                            var beforeChange = tempName;

                            if (MoveFileCheck.ContainsKey(tempName))
                            {
                                var index = MoveFileCheck[tempName] + 1;
                                tempName = descFolder + av.ID + "-" + av.Name + chinese + "_" + index + file.Extension;
                                MoveFileCheck[beforeChange] = index;
                            }
                            else
                            {
                                MoveFileCheck.Add(tempName, 1);
                            }

                            tempRet.Add(new RenameModel
                            {
                                AvId = av.ID,
                                AvName = av.Name,
                                AvImg = av.PictureURL,
                                MoveFile = tempName
                            });
                        }

                        ret.Add(file.FullName, tempRet);
                    }
                }
            }

            return ret;
        }

        public static List<RemoveSubModel> RemoveSubFolder(string sourceFolder, string descFolder, string excludeFolder = "tempFin,Fin,movefiles", int fileSizeLimit = 200)
        {
            Dictionary<string, int> moveFileCheck = new Dictionary<string, int>();
            List<RemoveSubModel> ret = new List<RemoveSubModel>();

            if (Directory.Exists(sourceFolder))
            {
                descFolder = (descFolder.EndsWith("\\") || descFolder.EndsWith("/")) ? descFolder : descFolder + "\\";

                if (!Directory.Exists(descFolder))
                {
                    Directory.CreateDirectory(descFolder);
                }

                excludes.AddRange(excludeFolder.Split(',').ToList());

                List<FileInfo> files = new List<FileInfo>();
                var status = FileUtility.GetFilesRecursive(sourceFolder, formats, excludes, files, fileSizeLimit);

                if (string.IsNullOrWhiteSpace(status))
                {
                    foreach (var file in files)
                    {
                        var tempFile = descFolder + file.Name;

                        if (moveFileCheck.ContainsKey(tempFile))
                        {
                            var index = moveFileCheck[tempFile] + 1;
                            tempFile = descFolder + file.Name.Replace(file.Extension, "") + "_" + index + file.Extension;
                            moveFileCheck[tempFile] = index;
                        }
                        else
                        {
                            moveFileCheck.Add(tempFile, 1);
                        }

                        var template = "_\\d{1,}\\.";

                        ret.Add(new RemoveSubModel {
                            SrcFile = file.FullName,
                            DescFile = tempFile,
                            IsDuplicate = Regex.Matches(tempFile, template, RegexOptions.IgnoreCase).Count > 0 ? true : false,
                            SrcFileSize = file.Length,
                            ScrFileSizeStr = FileSize.GetAutoSizeString(file.Length, 1)
                        });
                    }
                }
            }

            return ret;
        }
    }
}
