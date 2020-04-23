using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Automation;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Utils
{
    public class Broswer
    {
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr hWnd);

        public static Process OpenBrowserUrl(string url)
        {
            try
            {
                // 64位注册表路径
                var openKey = @"SOFTWARE\Wow6432Node\Google\Chrome";
                if (IntPtr.Size == 4)
                {
                    // 32位注册表路径
                    openKey = @"SOFTWARE\Google\Chrome";
                }
                RegistryKey appPath = Registry.LocalMachine.OpenSubKey(openKey);
                // 谷歌浏览器就用谷歌打开，没找到就用系统默认的浏览器
                // 谷歌卸载了，注册表还没有清空，程序会返回一个"系统找不到指定的文件。"的bug
                if (appPath != null)
                {
                    return Process.Start("chrome.exe", url);
                }
                else
                {
                    return Process.Start("chrome.exe", url);
                }
            }
            catch
            {
                return null;
            }
        }

        public static void Refresh_click()
        {
            var p = Process.GetProcessById(GetCurrentChromeTabProcessId());

            if (p != null)
            {
                IntPtr ptr = p.MainWindowHandle;
                SetForegroundWindow(ptr);
                SendKeys.SendWait("{F5}");
            }
        }

        public static void CloseBroswer()
        {
            var p = Process.GetProcessById(GetCurrentChromeTabProcessId());

            if (p != null)
            {
                IntPtr ptr = p.MainWindowHandle;
                SetForegroundWindow(ptr);
                SendKeys.SendWait("%{F4}");

                p = null;
            }
        }

        public static int GetCurrentChromeTabProcessId()
        {
            Process[] procsChrome = Process.GetProcessesByName("chrome");

            if (procsChrome.Length <= 0)
                return -1;

            foreach (Process proc in procsChrome)
            {
                // the chrome process must have a window 
                if (proc.MainWindowHandle == IntPtr.Zero)
                    continue;

                // to find the tabs we first need to locate something reliable - the 'New Tab' button 
                AutomationElement root = AutomationElement.FromHandle(proc.MainWindowHandle);
                var test = root.Current;
                //var SearchBar = root.FindFirst(TreeScope.Descendants, new PropertyCondition(AutomationElement.NameProperty, "Address and search bar"));
                //if (SearchBar != null)
                //{
                    return test.ProcessId;
                //}
            }

            return -1;
        }
    }
}