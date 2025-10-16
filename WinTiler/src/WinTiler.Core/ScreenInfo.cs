using System.Drawing;
using System.Windows.Forms;

namespace WinTiler.Core {
    public static class ScreenInfo {
        public static Rectangle GetPrimaryWorkingArea() {
            var r = Screen.PrimaryScreen.WorkingArea;
            return new Rectangle(r.X, r.Y, r.Width, r.Height);
        }
    }
}
