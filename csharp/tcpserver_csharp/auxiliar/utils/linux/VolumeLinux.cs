using System.Diagnostics;

namespace tcpserver_csharp.auxiliar.utils.linux
{
    internal class VolumeLinux
    {
        public static string GetVolume(bool semPorcentagem = true)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = "-c \"pactl get-sink-volume @DEFAULT_SINK@\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process? process = Process.Start(psi))
            {
                if (process == null) return "Erro ao iniciar processo";

                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                
                string rt = ParseVolume(output);
                return semPorcentagem ? rt.Replace("%", "").Trim() : rt;
            }
        }

        private static string ParseVolume(string pactlOutput)
        {
            if (string.IsNullOrWhiteSpace(pactlOutput)) return "Vazio";

            int percentIndex = pactlOutput.IndexOf('%');
            if (percentIndex > 0)
            {
                int start = pactlOutput.LastIndexOf(' ', percentIndex) + 1;
                if (start >= 0 && percentIndex > start)
                {
                    return pactlOutput.Substring(start, percentIndex - start + 1);
                }
            }
            return "Desconhecido";
        }

        public static void SetVolume(float volume)
        {
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"pactl set-sink-volume @DEFAULT_SINK@ {volume}%\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process? process = Process.Start(psi))
            {
                if (process == null)
                {
                    Console.WriteLine("Erro: Não foi possível iniciar o processo pactl.");
                    return;
                }

                process.WaitForExit();

                // StandardOutput lido para evitar deadlock, embora não usado no if
                _ = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error))
                    Console.WriteLine("Erro: " + error);
                else
                    Console.WriteLine($"Volume alterado para: {volume}");
            }
        }
    }
}