using DataBaseManager.JavDataBaseHelper;
using Model.JavModels;
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
                            var pattern = prefix + "{1}-?\\d{1,6}";
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
                            }
                        }

                        foreach (var av in possibleAv)
                        {
                            var chinese = (fileNameWithoutFormat.EndsWith("-c") || fileNameWithoutFormat.EndsWith("-ch") || fileNameWithoutFormat.EndsWith("ch")) ? "-C" : "";

                            var tempName = descFolder + av.ID + "-" + av.Name + chinese + file.Extension;

                            if (MoveFileCheck.ContainsKey(tempName))
                            {
                                var index = MoveFileCheck[tempName] + 1;
                                tempName = descFolder + av.ID + "-" + av.Name + chinese + index + file.Extension;
                                MoveFileCheck[tempName]++;
                            }
                            else
                            {
                                MoveFileCheck.Add(tempName, 1);
                            }

                            tempRet.Add(new RenameModel
                            {
                                AvId = av.ID,
                                AvName = av.Name,
                                AVImg = imageFolder + av.ID + av.Name + ".jpg",
                                MoveFile = tempName
                            });
                        }

                        ret.Add(file.Name, tempRet);
                    }
                }
            }

            return ret;
        }
    }
}
