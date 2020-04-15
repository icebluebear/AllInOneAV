using MangaDownloaderGUI.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MangaDownloaderGUI.Service
{
    public class SettingSevice
    {
        private static readonly string SettingFile = Environment.CurrentDirectory + "\\Setting.json";

        public static bool InitSetting()
        {
            bool ret = true;

            try
            {
                if (!File.Exists(SettingFile))
                {
                    SettingModel setting = new SettingModel
                    {
                        HistoryFolder = "",
                        MangaFolder = "",
                        ZipFolder = "",

                        ThreadCount = 5,
                        IsZip = false
                    };

                    var settingJson = JsonConvert.SerializeObject(setting);

                    File.Create(SettingFile).Close();

                    StreamWriter sw = new StreamWriter(SettingFile);
                    sw.WriteLine(settingJson);
                    sw.Close();
                }
            }
            catch(Exception ee)
            {
                ret = false;

                LogService.WriteLog("InitSetting", ee.ToString());
            }

            return ret;
        }

        public static SettingModel ReadSetting()
        {
            SettingModel ret = new SettingModel();

            var success = InitSetting();

            if(success)
            {
                try
                {
                    StreamReader sr = new StreamReader(SettingFile);
                    var json = sr.ReadToEnd();
                    sr.Close();

                    ret = JsonConvert.DeserializeObject<SettingModel>(json);
                }
                catch (Exception ee)
                {
                    LogService.WriteLog("ReadSetting", ee.ToString());
                }
            }

            return ret;
        }

        public static bool WriteSetting(SettingModel model)
        {
            bool ret = true;
            var success = InitSetting();

            if (success)
            {
                try
                {
                    StreamWriter sw = new StreamWriter(SettingFile, false);
                    sw.WriteLine(JsonConvert.SerializeObject(model));
                    sw.Close();
                }
                catch (Exception ee)
                {
                    ret = false;
                    LogService.WriteLog("WriteSetting", ee.ToString());
                }
            }

            return ret;
        }
    }
}
