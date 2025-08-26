using System.Diagnostics;

namespace tcpserver_csharp_linux.linuxaux
{
    internal class VolumeLinux
    {
        public static string GetVolume(bool semPorcentagem = true)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "bash";
            psi.Arguments = "-c \"pactl get-sink-volume @DEFAULT_SINK@\"";
            psi.RedirectStandardOutput = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            using (Process process = Process.Start(psi))
            {
                string output = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                string rt = ParseVolume(output);
                return semPorcentagem ? rt.Replace("%", "").Trim() : rt;
            }
        }

        private static string ParseVolume(string pactlOutput)
        {
            // Exemplo de saída:
            // Volume: front-left: 65536 / 100% / 0,00 dB,   front-right: 65536 / 100% / 0,00 dB
            int percentIndex = pactlOutput.IndexOf('%');
            if (percentIndex > 0)
            {
                int start = pactlOutput.LastIndexOf(' ', percentIndex) + 1;
                return pactlOutput.Substring(start, percentIndex - start + 1);
            }
            return "Desconhecido";
        }

        public static void SetVolume(float volume)
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = "bash";
            psi.Arguments = $"-c \"pactl set-sink-volume @DEFAULT_SINK@ {volume}%\"";
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;

            using (Process process = Process.Start(psi))
            {
                process.WaitForExit();

                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(error))
                    Console.WriteLine("Erro: " + error);
                else
                    Console.WriteLine($"Volume alterado para: {volume}");
            }
        }
    }
}