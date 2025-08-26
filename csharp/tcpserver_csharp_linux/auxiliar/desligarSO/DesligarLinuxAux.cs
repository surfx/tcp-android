using System.Diagnostics;

namespace tcpserver_csharp.auxiliar.desligarSO
{
    internal class DesligarLinuxAux
    {
        public static bool DesligarLinux()
        {
            var psi = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = "-c \"shutdown now\"",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            Process.Start(psi);
            return true;
        }
    }
}