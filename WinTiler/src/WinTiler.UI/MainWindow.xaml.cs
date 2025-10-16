
using System.Windows;

namespace WinTiler.UI {
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void BtnLoadTheme_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Theme loaded (placeholder). In full build, this will apply live theme preview.", "WinTiler");
        }

        private void BtnEditKeybinds_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Keybinding editor (placeholder). Full UI will allow editing all keybindings.", "WinTiler");
        }
    }
}
