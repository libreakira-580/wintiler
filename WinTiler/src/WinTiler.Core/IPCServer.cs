
using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WinTiler.Core {
    public class IPCServer : IDisposable {
        private readonly Thread _listenThread;
        private volatile bool _running = true;
        private readonly List<NamedPipeServerStream> _clients = new();

        public IPCServer() {
            _listenThread = new Thread(ListenLoop) { IsBackground = true };
            _listenThread.Start();
        }

        private void ListenLoop() {
            while (_running) {
                try {
                    var server = new NamedPipeServerStream("WinTiler_IPC", PipeDirection.Out, NamedPipeServerStream.MaxAllowedServerInstances, PipeTransmissionMode.Message, PipeOptions.Asynchronous);
                    server.WaitForConnection();
                    lock (_clients) { _clients.Add(server); }
                } catch { Thread.Sleep(500); }
            }
        }

        public void Broadcast(object obj) {
            string s = JsonSerializer.Serialize(obj) + "\n";
            var bytes = Encoding.UTF8.GetBytes(s);
            lock (_clients) {
                for (int i = _clients.Count - 1; i >= 0; i--) {
                    var c = _clients[i];
                    try {
                        if (!c.IsConnected) { c.Dispose(); _clients.RemoveAt(i); continue; }
                        c.Write(bytes, 0, bytes.Length);
                        c.Flush();
                    } catch { try { c.Dispose(); } catch { } _clients.RemoveAt(i); }
                }
            }
        }

        public void Dispose() {
            _running = false;
            lock (_clients) { foreach (var c in _clients) try { c.Dispose(); } catch { } _clients.Clear(); }
        }
    }
}
