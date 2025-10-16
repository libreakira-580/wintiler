
using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace waybar_win {
    class Program {
        static async Task Main(string[] args) {
            Console.WriteLine("waybar-win starting (improved client)...");
            while (true) {
                try {
                    using var client = new NamedPipeClientStream(".", "WinTiler_IPC", PipeDirection.In);
                    Console.WriteLine("Connecting to WinTiler IPC...");
                    await client.ConnectAsync(5000);
                    Console.WriteLine("Connected. Reading messages (press Ctrl+C to quit)...");
                    var buf = new byte[4096];
                    while (true) {
                        int read = await client.ReadAsync(buf, 0, buf.Length);
                        if (read <= 0) break;
                        var s = Encoding.UTF8.GetString(buf, 0, read);
                        foreach (var line in s.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries)) {
                            Console.WriteLine($"[WAYBAR] {line}");
                        }
                    }
                } catch (Exception ex) {
                    Console.WriteLine($"IPC connection failed: {ex.Message}"); 
                }
                await Task.Delay(1000);
            }
        }
    }
}
