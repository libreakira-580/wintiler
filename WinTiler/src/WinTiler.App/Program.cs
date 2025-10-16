
using System;
using System.Threading.Tasks;
using WinTiler.Core;
using System.Drawing;
using System.IO;

namespace WinTiler.App {
    class Program {
        static async Task Main(string[] args) {
            Console.WriteLine("WinTiler.App starting (upgraded)...");
            var cfgPath = "examples/config.json";
            var cfg = Config.Load(cfgPath);

            using var ipc = new IPCServer();
            using var hook = new WinEventHook();
            using var hk = new HotkeyManager();

            hook.OnWinEvent += (hwnd, ev, h, idObj, idChild) => {
                // On any event, enumerate and broadcast minimal window list and focused title
                var wins = WindowEnumerator.EnumerateTopLevelWindows();
                ipc.Broadcast(new { type = "window_list", count = wins.Count, focused = wins.Count > 0 ? wins[0].Title : "" });
            };

            // Register a simple hotkey: Mod+Enter => spawn terminal (uses cmd.exe)
            try {
                // MOD_WIN + VK_RETURN
                hk.RegisterHotkey(0x0008, 0x0D, () => {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo { FileName = "cmd.exe" });
                });
            } catch (Exception ex) {
                Console.WriteLine($"Hotkey registration failed: {ex.Message}");
            }

            // Watch config file for changes and broadcast
            var watcher = new FileSystemWatcher(Path.GetDirectoryName(cfgPath) ?? "./", Path.GetFileName(cfgPath));
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;
            watcher.Changed += (s,e) => {
                try {
                    var newCfg = Config.Load(cfgPath);
                    ipc.Broadcast(new { type = "config_reload", theme = newCfg.Theme });
                    Console.WriteLine("Config reloaded."); 
                } catch { }
            };
            watcher.EnableRaisingEvents = true;

            Console.WriteLine("WinTiler running. Press Ctrl+C to exit.");
            await Task.Delay(-1);
        }
    }
}
