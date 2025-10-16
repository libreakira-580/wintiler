using System;
using System.IO;
using System.Text.Json;

namespace WinTiler.Core {
    public class Config {
        public string ModKey { get; set; } = "LWin";
        public int MasterCount { get; set; } = 1;
        public double MasterRatio { get; set; } = 0.6;
        public int Gaps { get; set; } = 8;
        public int PollIntervalMs { get; set; } = 250;
        public Keybindings Keybindings { get; set; } = new Keybindings();

        public static Config Load(string path) {
            if (!File.Exists(path)) return new Config();
            var txt = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Config>(txt) ?? new Config();
        }
    }

    public class Keybindings {
        public string SpawnTerminal { get; set; } = "Mod+Enter";
        public string FocusNext { get; set; } = "Mod+j";
        public string FocusPrev { get; set; } = "Mod+k";
        public string CloseWindow { get; set; } = "Mod+Shift+q";
    }
}
