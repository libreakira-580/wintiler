# WinTiler — i3-style Tiler for Windows (Starter "World's Best" Pack)

This is a starter, production-oriented C# .NET solution for a Windows tiler inspired by i3,
including a headless core, a console host, and a minimal WPF config UI & waybar-win stub.

**What's included**
- WinTiler.Core (class library): window management, tiler, config loader (basic).
- WinTiler.App (console): host that runs the core and registers hotkeys.
- WinTiler.UI (WPF): minimal config UI skeleton (theme + keybindings page).
- waybar-win (console): simple status-bar stub that connects over named pipes.
- examples/: config.json, theme-dracula.json, dotfiles/arch-mode hints.
- tests/: basic unit test project skeleton.
- CI: GitHub Actions workflow skeleton.

This is a starter pack — many pieces are implemented minimally but structurally complete so you can iterate and rice it.

Build:
1. Install .NET 8+ SDK.
2. `dotnet build WinTiler.sln -c Release`
3. Run `src/WinTiler.App/bin/Debug/net8.0/WinTiler.App.exe`

License: MIT


## Upgraded: Worlds-Best features implemented (starter)

- SetWinEventHook listener (WinEventHook.cs) to receive window events.
- Hotkey registration (HotkeyManager.cs) with a minimal background message loop.
- IPC NamedPipe server (IPCServer.cs) broadcasting JSON messages to clients (waybar-win client reads them).
- Config hot-reload via FileSystemWatcher that broadcasts `config_reload` events.

This is a functional, opinionated starter implementation — polish, security hardening, and UX improvements remain possible.


## Full Keybindings Implemented (Focus Tiling)

- Mod+Enter: Spawn Terminal (cmd.exe)
- Mod+d: Run launcher (placeholder)
- Mod+j / Mod+k: Focus next / previous window
- Mod+Shift+Space: Toggle floating
- Mod+Shift+q: Close window
- Mod+Ctrl+m: Minimize window
- Mod+f: Maximize / Restore window
- Mod+Ctrl+f: Toggle fullscreen
- Mod+t: Toggle always-on-top
- Mod+Space: Cycle layout (Master/Stack/Monocle/Grid)
- Mod+= / Mod+-: Increase / Decrease gaps
- Mod+N / Mod+Shift+N: Send window to workspace / Move window to workspace (N=1..9)
- Mod+Shift+r: Rename workspace
- Mod+` (backtick): Toggle scratchpad
- Mod+Ctrl+r: Restart WinTiler
- Mod+Shift+e: Quit WinTiler

All bindings configurable via JSON in examples/config.json or future UI.
