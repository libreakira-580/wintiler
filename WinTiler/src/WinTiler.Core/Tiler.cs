using System;
using System.Collections.Generic;
using System.Drawing;

namespace WinTiler.Core {
    public class Tiler {
        private readonly Config _cfg;
        public Tiler(Config cfg) {
            _cfg = cfg;
        }

        public void ApplyLayout(List<ManagedWindow> windows, Rectangle screen) {
            if (windows.Count == 0) return;
            var masterCount = Math.Max(1, Math.Min(_cfg.MasterCount, windows.Count));
            var masterWidth = (int)(screen.Width * _cfg.MasterRatio);
            var gap = _cfg.Gaps;

            var masters = windows.GetRange(0, masterCount);
            var stack = windows.GetRange(masterCount, windows.Count - masterCount);

            int masterX = screen.X + gap;
            int masterY = screen.Y + gap;
            int masterH = screen.Height - gap * 2;
            int masterW = masterWidth - gap;

            for (int i = 0; i < masters.Count; i++) {
                var w = masters[i];
                int h = masterH / masters.Count - gap;
                w.SetBounds(masterX, masterY + i * (h + gap), masterW, h);
            }

            int stackX = masterX + masterW + gap;
            int stackW = screen.Width - masterW - gap * 3;
            if (stack.Count == 0) return;
            int stackHUnit = (screen.Height - gap * 2) / Math.Max(1, stack.Count);
            for (int i = 0; i < stack.Count; i++) {
                var w = stack[i];
                w.SetBounds(stackX, screen.Y + gap + i * (stackHUnit + gap), stackW, stackHUnit - gap);
            }
        }
    }
}
