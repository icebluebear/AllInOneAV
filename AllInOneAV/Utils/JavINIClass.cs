using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class JavINIClass
    {
        public static string inipath;
        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileString(string segName, string keyName, string sDefault, StringBuilder buffer, int nSize, string fileName);
        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileStringA(string segName, string keyName, string sDefault, byte[] buffer, int iLen, string fileName); // ANSI版本
        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileSection(string segName, StringBuilder buffer, int nSize, string fileName);
        [DllImport("kernel32.dll")]
        public extern static int WritePrivateProfileSection(string segName, string sValue, string fileName);
        [DllImport("kernel32.dll")]
        public extern static int WritePrivateProfileString(string segName, string keyName, string sValue, string fileName);
        [DllImport("kernel32.dll")]
        public extern static int GetPrivateProfileSectionNamesA(byte[] buffer, int iLen, string fileName);

        static JavINIClass()
        {
            inipath = "c:/Setting/database.ini";
        }

        public static void IniWriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, inipath);
        }

        public static string IniReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(50000);
            int i = GetPrivateProfileString(Section, Key, "", temp, 50000, inipath);
            return temp.ToString();
        }

        public static ArrayList ReadSections()
        {
            byte[] buffer = new byte[65535];
            int rel = GetPrivateProfileSectionNamesA(buffer, buffer.GetUpperBound(0), inipath);
            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }

        public static ArrayList ReadKeys(string sectionName)
        {
            byte[] buffer = new byte[5120];
            int rel = GetPrivateProfileStringA(sectionName, null, "", buffer, buffer.GetUpperBound(0), inipath);
            int iCnt, iPos;
            ArrayList arrayList = new ArrayList();
            string tmp;
            if (rel > 0)
            {
                iCnt = 0; iPos = 0;
                for (iCnt = 0; iCnt < rel; iCnt++)
                {
                    if (buffer[iCnt] == 0x00)
                    {
                        tmp = System.Text.ASCIIEncoding.Default.GetString(buffer, iPos, iCnt - iPos).Trim();
                        iPos = iCnt + 1;
                        if (tmp != "")
                            arrayList.Add(tmp);
                    }
                }
            }
            return arrayList;
        }

        public bool ExistINIFile()
        {
            return File.Exists(inipath);
        }
    }
}
