using System.Diagnostics;

namespace auxiliar
{
    public static class LockScreenLinux
    {
        /// <summary>
        /// Tenta bloquear a sessão do usuário no Linux.
        /// Retorna true se algum método reportar sucesso (exit code 0) ou lançar nenhuma exceção.
        /// </summary>
        public static bool Lock()
        {
            // Comandos/caminhos tentados em ordem. Muitos ambientes suportam ao menos um destes.
            var attempts = new (string file, string args)[]
            {
                // padrão XDG (pode disparar o lock do DE)
                ("xdg-screensaver", "lock"),

                // gnome / unity
                ("gnome-screensaver-command", "--lock"),

                // suporte broad: kde (qdbus/qdbus-qt5)
                ("qdbus", "org.freedesktop.ScreenSaver /ScreenSaver Lock"),
                ("qdbus-qt5", "org.freedesktop.ScreenSaver /ScreenSaver Lock"),

                // KDE via dbus-send (fallback)
                ("dbus-send", "--session --dest=org.freedesktop.ScreenSaver /ScreenSaver org.freedesktop.ScreenSaver.Lock"),

                // xfce
                ("xflock4", ""),

                // simple Wayland-friendly method via systemd (locks session)
                ("loginctl", "lock-session"),

                // gnome-shell / mutter alternative via dbus-send path used by some setups
                ("dbus-send", "--session --dest=org.gnome.ScreenSaver /org/gnome/ScreenSaver org.gnome.ScreenSaver.Lock")
            };

            foreach (var (file, args) in attempts)
            {
                try
                {
                    if (!IsExecutableAvailable(file)) continue;

                    var psi = new ProcessStartInfo
                    {
                        FileName = file,
                        Arguments = args,
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = false,
                        RedirectStandardError = false
                    };

                    using (var p = Process.Start(psi))
                    {
                        if (p == null) continue;
                        // damos um tempo curto para o comando finalizar; muitos comandos retornam imediatamente.
                        bool exited = p.WaitForExit(1500);
                        if (exited)
                        {
                            // exit code 0 -> sucesso presumido
                            if (p.ExitCode == 0) return true;
                            // alguns comandos retornam >0 mesmo assim (xflock4 por exemplo delega e volta 1). Vamos considerar não-zero como possível sucesso
                            // mas só aceitaremos não-zero quando não houver uma alternativa mais confiável.
                        }
                        else
                        {
                            // não terminou imediatamente — presumimos que ele alcançou o locker (muitos bloqueadores ficam em background)
                            return true;
                        }
                    }
                }
                catch
                {
                    // ignora e tenta o próximo
                }
            }

            // última tentativa: executar via /bin/sh -c (para casos em que comando precisa de redirecionamento)
            try
            {
                string combined =
                    "xdg-screensaver lock || gnome-screensaver-command --lock || xflock4 || loginctl lock-session";
                var psi = new ProcessStartInfo
                {
                    FileName = "/bin/sh",
                    Arguments = $"-c \"{combined}\"",
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var p = Process.Start(psi))
                {
                    if (p == null) return false;
                    bool exited = p.WaitForExit(1500);
                    return !exited || p.ExitCode == 0;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checa se o executável está disponível no PATH.
        /// </summary>
        private static bool IsExecutableAvailable(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name)) return false;
                // procura em PATH: tenta 'which'
                var psi = new ProcessStartInfo
                {
                    FileName = "which",
                    Arguments = name,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };
                using (var p = Process.Start(psi))
                {
                    if (p == null) return false;
                    string outp = p.StandardOutput.ReadToEnd();
                    p.WaitForExit(500);
                    return !string.IsNullOrWhiteSpace(outp) && !outp.Contains("no ");
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
