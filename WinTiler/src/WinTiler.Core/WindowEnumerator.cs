using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace WinTiler.Core {
    public static class WindowEnumerator {
        public static List<ManagedWindow> EnumerateTopLevelWindows() {
            var list = new List<ManagedWindow>();
            EnumWindows((hwnd, lParam) => {
                if (!IsWindowVisible(hwnd)) return true;
                var len = GetWindowTextLength(hwnd);
                var sb = new StringBuilder(len + 1);
                GetWindowText(hwnd, sb, sb.Capacity);
                var title = sb.ToString();
                if (string.IsNullOrWhiteSpace(title)) return true;
                list.Add(new ManagedWindow { Hwnd = hwnd, Title = title });
                return true;
            }, IntPtr.Zero);
            return list;
        }

        private delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
        [DllImport("user32.dll")] private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        [DllImport("user32.dll")] private static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll", CharSet=CharSet.Unicode)] private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", CharSet=CharSet.Unicode)] private static extern int GetWindowTextLength(IntPtr hWnd);
    }
}
