using System.Diagnostics;
using auxiliar;

namespace tcpserver_csharp.auxiliar.desligarSO
{
    internal class DesligarLinuxAux
    {
        public static bool DesligarLinuxBash()
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

        public static bool DesligarLinux()
        {

            // Pega resolução da tela
            int w = ScreenSizeLinux.getWidth();   // largura da tela
            int h = ScreenSizeLinux.getHeight();  // altura da tela (não usado aqui)

            int x = w - 10; // próximo do canto direito
            int y = 10;     // topo da tela, pequeno deslocamento para não ir 0 absoluto


            Console.WriteLine($"Tela: {w}x{h}");

            // Move mouse para o canto superior esquerdo
            MouseOperationsLinux.SetCursorPosition(x, y);
            System.Threading.Thread.Sleep(200);

            // Clica com o botão esquerdo
            MouseOperationsLinux.MouseEvent(MouseOperationsLinux.MouseEventFlags.LeftDown);
            System.Threading.Thread.Sleep(50);
            MouseOperationsLinux.MouseEvent(MouseOperationsLinux.MouseEventFlags.LeftUp);
            System.Threading.Thread.Sleep(50);

            SetasLinux.PressionarSeta(SetasLinux.Tecla.Down, 9, 50);

            SetasLinux.PressionarSeta(SetasLinux.Tecla.Right, 50);

            SetasLinux.PressionarSeta(SetasLinux.Tecla.Down, 50);

            SetasLinux.PressionarSeta(SetasLinux.Tecla.Enter, 50);

            return true;
        }
    }
}