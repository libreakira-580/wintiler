using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace WinTiler.Core {
    public class ManagedWindow {
        public IntPtr Hwnd { get; set; }
        public Rectangle Bounds { get; set; }
        public string Title { get; set; } = "";
        public void SetBounds(int x, int y, int w, int h) {
            // wrapper - real implementation in native interop layer
            NativeMethods.SetWindowPos(Hwnd, IntPtr.Zero, x, y, w, h, NativeMethods.SWP_NOZORDER | NativeMethods.SWP_NOACTIVATE);
        }
    }

    internal static class NativeMethods {
        public const int SWP_NOZORDER = 0x4;
        public const int SWP_NOACTIVATE = 0x10;

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError=true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
    }
}
