using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Utils
{
    public class WindowHelper
    {
        // 查找窗口句柄
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        // 显示/隐藏窗口
        [DllImport("user32.dll", EntryPoint = "ShowWindow")]
        public static extern bool ShowWindow(IntPtr hWnd, uint nCmdShow);

        // 获取窗口信息
        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(IntPtr hwnd, int nIndex);

        // 设置窗口属性
        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]

        public static extern int SetWindowLong(IntPtr hMenu, int nIndex, int dwNewLong);
    }
}
