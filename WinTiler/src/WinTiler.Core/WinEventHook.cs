
using System;
using System.Runtime.InteropServices;

namespace WinTiler.Core {
    public class WinEventHook : IDisposable {
        private IntPtr _hook;
        private readonly WinEventDelegate _callback;

        public event Action<IntPtr, uint, IntPtr, int, int> OnWinEvent = delegate { };

        public WinEventHook() {
            _callback = new WinEventDelegate(Callback);
            // EVENT_OBJECT_CREATE, DESTROY, SHOW, HIDE, LOCATIONCHANGE
            _hook = SetWinEventHook(EVENT_MIN, EVENT_MAX, IntPtr.Zero, _callback, 0, 0, WINEVENT_OUTOFCONTEXT | WINEVENT_SKIPOWNPROCESS);
        }

        private void Callback(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild) {
            OnWinEvent?.Invoke(hwnd, eventType, hWinEventHook, idObject, idChild);
        }

        public void Dispose() {
            if (_hook != IntPtr.Zero) UnhookWinEvent(_hook);
            _hook = IntPtr.Zero;
        }

        private const uint EVENT_MIN = 0x00000001;
        private const uint EVENT_MAX = 0x7FFFFFFF;
        private const uint WINEVENT_OUTOFCONTEXT = 0x0000;
        private const uint WINEVENT_SKIPOWNPROCESS = 0x0002;

        private delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild);
        [DllImport("user32.dll")] private static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax, IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);
        [DllImport("user32.dll")] private static extern bool UnhookWinEvent(IntPtr hWinEventHook);
    }
}
