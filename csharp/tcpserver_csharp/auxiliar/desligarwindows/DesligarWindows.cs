using System.Diagnostics;
using tcpserver_csharp.auxiliar.user32dll;

namespace tcpserver_csharp.auxiliar.desligarwindows
{
    internal class DesligarWindows
    {

        /// <summary>
        /// simula desligar o windows pelo teclado
        /// </summary>
        public static void Desligar()
        {
            MinimizeAllWindows.minimizeAllWindows();
            Thread.Sleep(2000);
            //Process.GetCurrentProcess().CloseMainWindow();

            //findwindowteste();

            // call close windows
            MultiKeyPressClass.MultiKeyPress([MultiKeyPressClass.KeyCode.ALT, MultiKeyPressClass.KeyCode.F4]);
            //MultiKeyPress(new KeyCode[] { KeyCode.ALT, KeyCode.F4 });
            //%{f4}

            Thread.Sleep(300);
            //Console.WriteLine($"GetActiveWindowTitle(): {WindowTitle.GetActiveWindowTitle()}");

            string windowshutdownname = "Desligar o Windows".ToLower();
            bool found = false;
            for (int i = 0; i <= 10; i++)
            {
                if (WindowTitle.GetActiveWindowTitle().ToLower().Equals(windowshutdownname))
                {
                    found = true; break;
                }
                Thread.Sleep(300);
            }

            if (!found)
            {
                Console.WriteLine("-- não consegui encontrar a janela para desligar o windows");
                return;
            }

            //Console.WriteLine("-- enviei um Enter");
            MultiKeyPressClass.SendKeyPress(MultiKeyPressClass.KeyCode.ENTER);
            //Console.ReadLine();
        }

        public static void DesligarTerminal()
        {
            ProcessStartInfo psi = new("shutdown", "/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
        }

    }
}
