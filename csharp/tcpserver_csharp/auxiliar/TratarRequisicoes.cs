using System.Diagnostics;
using AudioSwitcher.AudioApi.CoreAudio;

namespace auxiliar
{
    public class TratarRequisicoes
    {

        private const String msgErro = "0Erro";
        private const String codOk = "1";

        public string tratarRequisicoesTCP(string mensagem)
        {
            String rt = msgErro;
            if (mensagem == null || mensagem.Length <= 0) { return rt; }

            String tipo = mensagem.Substring(0, 1);
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
        private String getSincronizar()
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            return codOk + ((defaultPlaybackDevice.Volume / 100.0) + "").Replace(",", ".");
        }

        // 1 - alterar o volume
        private String getAlterarVolume(String mensagem)
        {
            try
            {
                String volumeStr = mensagem.Substring(1);
                float volumeFloat = float.Parse(volumeStr);
                CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
                defaultPlaybackDevice.Volume = volumeFloat;
                return codOk + ((defaultPlaybackDevice.Volume / 100.0) + "").Replace(",", ".");
            }
            catch (Exception e)
            {
                return msgErro;
            }
        }

        // 2 - desligar
        private String getDesligar()
        {
            Console.WriteLine("Shutdown windows");

            ProcessStartInfo psi = new ProcessStartInfo("shutdown","/s /t 0");
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            Process.Start(psi);
            return codOk + "Desligando";
        }

        //3 - mouse
        private String getMousePos(String mensagem)
        {
            //31920x1080,1370x425
            mensagem = mensagem.Substring(1);

            //Console.WriteLine("lenght: " + mensagem.Length);

            //1920x1080,1370x425
            int posX = mensagem.IndexOf("x");
            int posLX = mensagem.LastIndexOf("x");
            int posVirgula = mensagem.IndexOf(",");
            int wcel = int.TryParse(mensagem.Substring(0, posX), out wcel) ? wcel : -1;
            int hcel = int.TryParse(mensagem.Substring(posX + 1, posVirgula - (posX + 1)), out hcel) ? hcel : -1;
            int xcel = int.TryParse(mensagem.Substring(posVirgula + 1, posLX - (posVirgula + 1)), out xcel) ? xcel + 50 : -1; // ajuste, android faz um -50 ?
            int ycel = int.TryParse(mensagem.Substring(posLX + 1), out ycel) ? ycel + 50 : -1; // ajuste, android faz um -50 ?

            if (posX < 0 || posLX < 0 || posVirgula < 0 || wcel < 0 || hcel < 0 || xcel < 0 || ycel < 0)
            {
                return msgErro;
            }

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
        private String getClickMouse()
        {
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
            return codOk + "click recebido";
        }

        // 5 - Lock Screen
        private String lockScreen(){
            LockScreen.LockWorkStation();
            return codOk + "Tela Bloqueada";
        }

    }

}