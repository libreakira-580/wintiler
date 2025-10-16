
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

namespace WinTiler.Core {
    public class HotkeyManager : IDisposable {
        private readonly Dictionary<int, Action> _handlers = new();
        private int _idCounter = 0;
        private readonly Thread _msgThread;
        private bool _running = true;

        public HotkeyManager() {
            _msgThread = new Thread(MessageLoop) { IsBackground = true };
            _msgThread.Start();
        }

        public int RegisterHotkey(uint modifiers, uint vk, Action callback) {
            int id = System.Threading.Interlocked.Increment(ref _idCounter);
            if (!RegisterHotKey(IntPtr.Zero, id, modifiers, vk)) {
                throw new System.Exception("Failed to register hotkey");
            }
            _handlers[id] = callback;
            return id;
        }

        public void UnregisterAll() {
            foreach (var id in _handlers.Keys) UnregisterHotKey(IntPtr.Zero, id);
            _handlers.Clear();
        }

        private void MessageLoop() {
            var msg = new MSG();
            while (_running && GetMessage(out msg, IntPtr.Zero, 0, 0) != 0) {
                if (msg.message == WM_HOTKEY) {
                    int id = (int)msg.wParam;
                    if (_handlers.TryGetValue(id, out var cb)) {
                        try { cb(); } catch { }
                    }
                }
                TranslateMessage(ref msg);
                DispatchMessage(ref msg);
            }
        }

        public void Dispose() {
            _running = false;
            UnregisterAll();
            PostThreadMessage(GetCurrentThreadId(), WM_QUIT, UIntPtr.Zero, IntPtr.Zero);
        }

        private const int WM_HOTKEY = 0x0312;
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;
        
        [DllImport("user32.dll", SetLastError=true)]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);
        [DllImport("user32.dll", SetLastError=true)]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);
        [DllImport("user32.dll")] private static extern short GetAsyncKeyState(int vKey);

        [DllImport("user32.dll")] private static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
        [DllImport("user32.dll")] private static extern bool TranslateMessage(ref MSG lpMsg);
        [DllImport("user32.dll")] private static extern IntPtr DispatchMessage(ref MSG lpMsg);
        [DllImport("user32.dll")] private static extern bool PostThreadMessage(int idThread, int Msg, UIntPtr wParam, IntPtr lParam);
        [DllImport("kernel32.dll")] private static extern int GetCurrentThreadId();

        [StructLayout(LayoutKind.Sequential)]
        private struct MSG { public IntPtr hwnd; public uint message; public UIntPtr wParam; public IntPtr lParam; public uint time; public System.Drawing.Point pt; }

        private const int WM_QUIT = 0x0012;
    }
}
