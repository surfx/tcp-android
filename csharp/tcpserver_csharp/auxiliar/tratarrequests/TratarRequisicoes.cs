using System.Diagnostics;
using AudioSwitcher.AudioApi.CoreAudio;
using System.Globalization;

namespace auxiliar.tratarrequests
{
    /// <summary>
    /// deprecated
    /// </summary>
    [Obsolete]
    public class TratarRequisicoes
    {
        private const string msgErro = "0Erro";
        private const string codOk = "1";

        public string tratarRequisicoesTCP(string mensagem)
        {
            string rt = msgErro;
            if (string.IsNullOrEmpty(mensagem)) { return rt; }

            string tipo = mensagem.Substring(0, 1);
            if (tipo.Equals("0"))
            {
                return getSincronizar();
            }
            else if (tipo.Equals("1"))
            {
                return getAlterarVolume(mensagem);
            }
            else if (tipo.Equals("2"))
            {
                return getDesligar();
            }
            else if (tipo.Equals("3"))
            {
                return getMousePos(mensagem);
            }
            else if (tipo.Equals("4"))
            {
                return getClickMouse();
            }
            else if (tipo.Equals("5"))
            {
                return lockScreen();
            }

            return rt;
        }

        // 0 - sincronizar
        private string getSincronizar()
        {
            var controller = new CoreAudioController();
            var defaultPlaybackDevice = controller.DefaultPlaybackDevice;
            
            if (defaultPlaybackDevice == null) return msgErro;

            double volumeValue = defaultPlaybackDevice.Volume / 100.0;
            // Usando InvariantCulture para garantir o ponto como separador decimal
            return codOk + volumeValue.ToString("0.00", CultureInfo.InvariantCulture);
        }

        // 1 - alterar o volume
        private string getAlterarVolume(string mensagem)
        {
            try
            {
                string volumeStr = mensagem.Substring(1);
                // Parse usando InvariantCulture para aceitar pontos e evitar erro de vírgula
                if (float.TryParse(volumeStr, CultureInfo.InvariantCulture, out float volumeFloat))
                {
                    var controller = new CoreAudioController();
                    var defaultPlaybackDevice = controller.DefaultPlaybackDevice;

                    if (defaultPlaybackDevice != null)
                    {
                        defaultPlaybackDevice.Volume = volumeFloat;
                        double volumeValue = defaultPlaybackDevice.Volume / 100.0;
                        return codOk + volumeValue.ToString("0.00", CultureInfo.InvariantCulture);
                    }
                }
                return msgErro;
            }
            catch (Exception)
            {
                return msgErro;
            }
        }

        // 2 - desligar
        private string getDesligar()
        {
            Console.WriteLine("Shutdown windows");

            ProcessStartInfo psi = new ProcessStartInfo("shutdown", "/s /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };
            Process.Start(psi);
            return codOk + "Desligando";
        }

        //3 - mouse
        private string getMousePos(string mensagem)
        {
            if (mensagem.Length < 2) return msgErro;
            mensagem = mensagem.Substring(1);

            int posX = mensagem.IndexOf("x");
            int posLX = mensagem.LastIndexOf("x");
            int posVirgula = mensagem.IndexOf(",");

            if (posX < 0 || posLX < 0 || posVirgula < 0) return msgErro;

            // Tratamento de parse para evitar exceções e aviso de variáveis não utilizadas
            if (!int.TryParse(mensagem.Substring(0, posX), out int wcel)) return msgErro;
            if (!int.TryParse(mensagem.Substring(posX + 1, posVirgula - (posX + 1)), out int hcel)) return msgErro;
            if (!int.TryParse(mensagem.Substring(posVirgula + 1, posLX - (posVirgula + 1)), out int xcel)) return msgErro;
            if (!int.TryParse(mensagem.Substring(posLX + 1), out int ycel)) return msgErro;

            // Ajuste conforme regra original
            xcel += 50;
            ycel += 50;

            if (wcel <= 0 || hcel <= 0) return msgErro;

            int wpc = ScreenSize.getWidth();
            int hpc = ScreenSize.getHeight();
            wpc = wpc <= 0 ? 1920 : wpc;
            hpc = hpc <= 0 ? 1080 : (hpc + 50);

            int xPc = conversorXY(xcel, wcel, wpc);
            int yPc = conversorXY(ycel, hcel, hpc);
            
            MouseOperations.SetCursorPosition(xPc, yPc);

            return codOk + "Recebido";
        }

        private int conversorXY(int xyCel, int whCel, int whPc)
        {
            return (xyCel * whPc) / whCel;
        }

        // 4 - click mouse
        private string getClickMouse()
        {
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
            return codOk + "click recebido";
        }

        // 5 - Lock Screen
        private string lockScreen()
        {
            LockScreen.LockWorkStation();
            return codOk + "Tela Bloqueada";
        }
    }
}