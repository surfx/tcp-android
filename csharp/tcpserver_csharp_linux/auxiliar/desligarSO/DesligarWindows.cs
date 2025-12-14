using System.Diagnostics;
using tcpserver_csharp.auxiliar.user32dll;

namespace tcpserver_csharp.auxiliar.desligarSO
{
    internal class DesligarWindows
    {

        #region DesligarWinKey
        /// <summary>
        /// desligar o windows pelo winkey
        /// </summary>
        /// <returns></returns>
        public static bool DesligarWinKey()
        {
            desligarWinKeyAux(); Thread.Sleep(500);
            desligarWinKeyAux(); Thread.Sleep(500);
            desligarWinKeyAux();

            return true;
        }

        private static void desligarWinKeyAux()
        {
            //MinimizeAllWindows.minimizeAllWindows();
            MultiKeyPressClass.MultiKeyPress([MultiKeyPressClass.KeyCode.LWIN, MultiKeyPressClass.KeyCode.KEY_X]);
            Thread.Sleep(100);
            MultiKeyPressClass.SendKeyPress(MultiKeyPressClass.KeyCode.UP);
            Thread.Sleep(20);
            MultiKeyPressClass.SendKeyPress(MultiKeyPressClass.KeyCode.UP);
            Thread.Sleep(20);
            MultiKeyPressClass.SendKeyPress(MultiKeyPressClass.KeyCode.SPACE_BAR);
            Thread.Sleep(20);
            MultiKeyPressClass.SendKeyPress(MultiKeyPressClass.KeyCode.UP);
            Thread.Sleep(20);
            MultiKeyPressClass.SendKeyPress(MultiKeyPressClass.KeyCode.UP);
            Thread.Sleep(20);
            MultiKeyPressClass.SendKeyPress(MultiKeyPressClass.KeyCode.SPACE_BAR);
        }

        #endregion

        #region desligar pelo teclado - not ok
        /// <summary>
        /// simula desligar o windows pelo teclado
        /// </summary>
        public static bool Desligar()
        {
            int tentativas = 0;
            while (tentativas <= 10)
            {
                if (simularDesligar())
                {
                    return true;
                }
                tentativas++;
                Console.WriteLine($"tentativas: {tentativas}");
            }
            return false;
        }

        private static bool simularDesligar()
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
                return false;
            }

            //Console.WriteLine("-- enviei um Enter");
            MultiKeyPressClass.SendKeyPress(MultiKeyPressClass.KeyCode.ENTER);
            //Console.ReadLine();
            return true;
        }
        #endregion

        #region desligar pelo terminal
        public static bool DesligarTerminal()
        {
            ProcessStartInfo psi = new("shutdown", "/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);

            return true;
        }
        #endregion

    }
}
