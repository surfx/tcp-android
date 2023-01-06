using System.Collections;
using System.Diagnostics;
using AudioSwitcher.AudioApi.CoreAudio;
using auxiliar.binarybits;

namespace auxiliar.tratarrequests
{
    public class TratarRequisicoesBin
    {

        BitArray codErro = BinaryBitsAux.to1Bit(false);
        BitArray codOk = BinaryBitsAux.to1Bit(true);

        public BitArray tratar(BitArray entrada)
        {
            if (entrada == null || entrada.Count <= 0) { return msgErro(); }

            BitArray cod = BinaryBitsAux.splitBitArray(entrada, 0, 3);
            Console.WriteLine("cod: {0} = {1}", BinaryBitsAux.ToBitString(cod), BinaryBitsAux.toInt(cod, true));

            switch(BinaryBitsAux.toInt(cod, true)){
                case 0: return sinchronizar();
                case 1: return alterarVolume(entrada);
                case 2: return desligarPC();
                case 3: return mouseMove(entrada);
                case 4: return clickMouse();
                case 5: return lockScreen();    
            }

            return msgErro();
        }

        private BitArray msgErro() {
            BitArray retorno = BinaryBitsAux.Combine(codErro, BinaryBitsAux.toBitArray("Erro"));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));
            return retorno;
        }

        // 0 - sincronizar
        private BitArray sinchronizar()
        {
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            float volume = (float)(defaultPlaybackDevice.Volume);
            BitArray retorno = BinaryBitsAux.Combine(codOk, volume); // float - 4 bytes = 32 bits
            Console.WriteLine("retorno: {0}, volume: {1}, defaultPlaybackDevice.Volume: {2}", BinaryBitsAux.ToBitString(retorno), volume, defaultPlaybackDevice.Volume);
            return retorno;
        }

        // 1 - alterar o volume
        private BitArray alterarVolume(BitArray entrada)
        {
            BitArray volumeEntrada = BinaryBitsAux.splitBitArray(entrada, 3, 32);
            float volume = BinaryBitsAux.toFloat(volumeEntrada);
            Console.WriteLine("volumeEntrada: {0}, volume: {1}", BinaryBitsAux.ToBitString(volumeEntrada), volume);
            
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            defaultPlaybackDevice.Volume = volume;

            volume = (float)(defaultPlaybackDevice.Volume);
            BitArray retorno = BinaryBitsAux.Combine(codOk, volume); // float - 4 bytes = 32 bits
            Console.WriteLine("retorno: {0}, volume: {1}, defaultPlaybackDevice.Volume: {2}", BinaryBitsAux.ToBitString(retorno), volume, defaultPlaybackDevice.Volume);

            return retorno;
        }

        // 2 - desligar
        private BitArray desligarPC()
        {
            Console.WriteLine("Shutdown windows");

            // ProcessStartInfo psi = new ProcessStartInfo("shutdown","/s /t 0");
            // psi.CreateNoWindow = true;
            // psi.UseShellExecute = false;
            // Process.Start(psi);

            String rt = "Desligando";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

        //3 - mouse
        private BitArray mouseMove(BitArray entrada)
        {
            int wc = BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 3, 13), true);
            int hc = BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 13 + 3, 13), true);
            int xc = BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 13 + 13 + 3, 13), true) + 50; // ajuste, android faz um -50 ?
            int yc = BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 13 + 13 + 13 + 3, 13), true)  + 50; // ajuste, android faz um -50 ?
            Console.WriteLine("wc: {0}, hc: {1}, xc: {2}, yc: {3}", wc, hc, xc, yc);

            if (wc < 0 || hc < 0 || xc < 0 || yc < 0) { return msgErro(); }

            int wpc = ScreenSize.getWidth();
            int hpc = ScreenSize.getHeight();
            wpc = wpc <= 0 ? 1920 : wpc;
            hpc = hpc <= 0 ? 1080 : (hpc + 50);

            int xPc = conversorXY(xc, wc, wpc);
            int yPc = conversorXY(yc, hc, hpc);
            MouseOperations.SetCursorPosition(xPc, yPc);

            String rt = "Recebido";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

        private int conversorXY(int xyCel, int whCel, int whPc)
        {
            return (xyCel * whPc) / whCel;
        }

        // 4 - click mouse
        private BitArray clickMouse()
        {
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftDown);
            MouseOperations.MouseEvent(MouseOperations.MouseEventFlags.LeftUp);
            
            String rt = "Click Recebido";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

        // 5 - Lock Screen
        private BitArray lockScreen(){
            LockScreen.LockWorkStation();

            String rt = "Tela Bloqueada";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

    }

}