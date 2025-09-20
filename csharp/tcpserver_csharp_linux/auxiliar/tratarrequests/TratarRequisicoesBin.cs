using System.Collections;
using auxiliar.binarybits;
using tcpserver_csharp.auxiliar.desligarSO;
using tcpserver_csharp_linux.linuxaux;

namespace auxiliar.tratarrequests
{
    public class TratarRequisicoesBin
    {

        readonly BitArray codErro = BinaryBitsAux.to1Bit(false);
        readonly BitArray codOk = BinaryBitsAux.to1Bit(true);

        private readonly int mouseIncrement = 30;

        public BitArray tratar(BitArray entrada)
        {
            if (entrada == null || entrada.Count <= 0) { return msgErro(); }

            BitArray cod = BinaryBitsAux.splitBitArray(entrada, 0, 4);
            Console.WriteLine("cod: {0} = {1}", BinaryBitsAux.ToBitString(cod), BinaryBitsAux.toInt(cod, true));

            switch (BinaryBitsAux.toInt(cod, true))
            {
                case 0: return sinchronizar();
                case 1: return alterarVolume(entrada);
                case 2: return desligarPC();
                case 3: return mouseMove(entrada);
                case 4: return clickMouse();
                case 5: return lockScreen();
                case 6: return upMouse();
                case 7: return downMouse();
                case 8: return leftMouse();
                case 9: return rightMouse();
            }

            return msgErro();
        }

        private BitArray msgErro()
        {
            BitArray retorno = BinaryBitsAux.Combine(codErro, BinaryBitsAux.toBitArray("Erro"));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));
            return retorno;
        }

        // 0 - sincronizar
        private BitArray sinchronizar()
        {
            float volume = float.Parse(VolumeLinux.GetVolume());
            // CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            // float volume = (float)(defaultPlaybackDevice.Volume);
            BitArray retorno = BinaryBitsAux.Combine(codOk, volume); // float - 4 bytes = 32 bits
            Console.WriteLine("retorno: {0}, volume: {1}", BinaryBitsAux.ToBitString(retorno), volume);
            return retorno;
        }

        // 1 - alterar o volume
        private BitArray alterarVolume(BitArray entrada)
        {
            BitArray volumeEntrada = BinaryBitsAux.splitBitArray(entrada, 4, 32);
            float volume = BinaryBitsAux.toFloat(volumeEntrada);
            Console.WriteLine("volumeEntrada: {0}, volume: {1}", BinaryBitsAux.ToBitString(volumeEntrada), volume);
            VolumeLinux.SetVolume(volume);
            BitArray retorno = BinaryBitsAux.Combine(codOk, volume); // float - 4 bytes = 32 bits
            Console.WriteLine("retorno: {0}, volume: {1}", BinaryBitsAux.ToBitString(retorno), volume);

            return retorno;
        }

        // 2 - desligar
        private BitArray desligarPC()
        {
            Console.WriteLine("Shutdown linux");

            DesligarLinuxAux.DesligarLinux();

            Thread.Sleep(1500);
            DesligarLinuxAux.DesligarLinuxBash();
            // if (!DesligarWindows.DesligarWinKey())
            // {
            //     return msgErro();
            // }

            string rt = "Desligando";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

        //3 - mouse
        private BitArray mouseMove(BitArray entrada)
        {
            int wc = BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 4, 13), true);
            int hc = BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 13 + 3, 13), true);
            int xc = BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 13 + 13 + 3, 13), true) + 50; // ajuste, android faz um -50 ?
            int yc = BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 13 + 13 + 13 + 3, 13), true) + 50; // ajuste, android faz um -50 ?
            Console.WriteLine("wc: {0}, hc: {1}, xc: {2}, yc: {3}", wc, hc, xc, yc);

            if (wc < 0 || hc < 0 || xc < 0 || yc < 0) { return msgErro(); }

            int wpc = ScreenSizeLinux.getWidth();
            int hpc = ScreenSizeLinux.getHeight();
            wpc = wpc <= 0 ? 1920 : wpc;
            hpc = hpc <= 0 ? 1080 : (hpc + 50);

            int xPc = conversorXY(xc, wc, wpc);
            int yPc = conversorXY(yc, hc, hpc);
            MouseOperationsLinux.SetCursorPosition(xPc, yPc);

            string rt = "Recebido";
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
            MouseOperationsLinux.MouseEvent(MouseOperationsLinux.MouseEventFlags.LeftDown);
            MouseOperationsLinux.MouseEvent(MouseOperationsLinux.MouseEventFlags.LeftUp);

            string rt = "Click Recebido";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

        // 5 - Lock Screen
        private BitArray lockScreen()
        {
            LockScreenLinux.Lock();

            string rt = "Tela Bloqueada";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

        // 6 - Up Mouse
        private BitArray upMouse()
        {
            MouseOperationsLinux.MousePoint positionMouse = MouseOperationsLinux.GetCursorPosition();

            int posY = positionMouse.Y;
            posY -= mouseIncrement; if (posY < 0) { posY = 0; }
            MouseOperationsLinux.SetCursorPosition(positionMouse.X, posY);

            string rt = "Mouse Up";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

        // 7 - Down Mouse
        private BitArray downMouse()
        {
            MouseOperationsLinux.MousePoint positionMouse = MouseOperationsLinux.GetCursorPosition();

            int posY = positionMouse.Y;
            posY += mouseIncrement; //if (posY >= hpc) { posY = hpc; }
            MouseOperationsLinux.SetCursorPosition(positionMouse.X, posY);

            string rt = "Mouse Down";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

        // 8 - Left Mouse
        private BitArray leftMouse()
        {
            MouseOperationsLinux.MousePoint positionMouse = MouseOperationsLinux.GetCursorPosition();

            int posX = positionMouse.X;
            posX -= mouseIncrement; if (posX < 0) { posX = 0; }
            MouseOperationsLinux.SetCursorPosition(posX, positionMouse.Y);

            string rt = "Mouse Left";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

        // 9 - Right Mouse
        private BitArray rightMouse()
        {
            MouseOperationsLinux.MousePoint positionMouse = MouseOperationsLinux.GetCursorPosition();

            int posX = positionMouse.X;
            posX += mouseIncrement; //if (posX >= wpc) { posX = wpc; }
            MouseOperationsLinux.SetCursorPosition(posX, positionMouse.Y);

            string rt = "Mouse Right";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            return retorno;
        }

    }

}