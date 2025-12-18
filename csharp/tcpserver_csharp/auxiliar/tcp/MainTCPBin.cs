using System.Collections;
using auxiliar.binarybits;

namespace auxiliar.tcp
{
    public class MainTCPBin
    {
        
        public static void startTcpBinServer(int port = 9876){
            if (port <= 0) { port = 9876; }
            new ServerTCP(port).start();

            //Thread.Sleep(100);

            //new ClientTCP("localhost", port).sendMessage(msgSincronizar(), tratarMsgSincronizar);
            //new ClientTCP("localhost", port).sendMessage(msgAlterarVolume(), tratarMsgAlterarVolume);
            //new ClientTCP("localhost", port).sendMessage(msgDesligarPC(), tratarMsgDesligarPC);
            //new ClientTCP("localhost", port).sendMessage(msgMouseMove(), tratarMsgMouseMove);
            //new ClientTCP("localhost", port).sendMessage(msgClickMouse(), tratarMsgClickMouse);
            //new ClientTCP("localhost", port).sendMessage(msgLockScreen(), tratarMsgLockScreen);

        }

        #region auxiliar / testes
        #region sincronizar
        private static BitArray msgSincronizar(){
            return BinaryBitsAux.toBits(0, 3);
        }

        private static void tratarMsgSincronizar(BitArray retorno){
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            BitArray volumeRetornoParser = BinaryBitsAux.splitBitArray(retorno, 1, 32);

            Console.WriteLine("bit0:\t\t\t{0}", bit0);
            Console.WriteLine("volumeRetornoParser: \t{0}, valor: {1}", BinaryBitsAux.ToBitString(volumeRetornoParser), BinaryBitsAux.toFloat(volumeRetornoParser));
        }
        #endregion

        #region alterarVolume
        private static BitArray msgAlterarVolume(){
            return BinaryBitsAux.Combine(BinaryBitsAux.toBits(1, 3), 50.97f); // float - 4 bytes - 32 bits
        }

        private static void tratarMsgAlterarVolume(BitArray retorno){
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            BitArray volumeRetornoParser = BinaryBitsAux.splitBitArray(retorno, 1, 32);

            Console.WriteLine("bit0:\t\t\t{0}", bit0);
            Console.WriteLine("volumeRetornoParser: \t{0}, valor: {1}", BinaryBitsAux.ToBitString(volumeRetornoParser), BinaryBitsAux.toFloat(volumeRetornoParser));
        }
        #endregion

        #region desligarPC
        private static BitArray msgDesligarPC(){
            return BinaryBitsAux.toBits(2, 3);
        }

        private static void tratarMsgDesligarPC(BitArray retorno){
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            String mensagem = BinaryBitsAux.toString(retorno, 1, 12);

            Console.WriteLine("bit0:\t\t{0}", bit0 ? 1 : 0);
            Console.WriteLine("mensagem:\t{0}", mensagem);
        }
        #endregion

        #region mouseMove
        private static BitArray msgMouseMove(int wc = 1920, int hc = 1080,  int xc = 1370, int yc = 425){
            return BinaryBitsAux.Combine(
                BinaryBitsAux.toBits(3, 3), 
                BinaryBitsAux.toBits(wc, 13),
                BinaryBitsAux.toBits(hc, 13), 
                BinaryBitsAux.toBits(xc, 13), 
                BinaryBitsAux.toBits(yc, 13)
            );
        }

        private static void tratarMsgMouseMove(BitArray retorno){
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            String mensagem = BinaryBitsAux.toString(retorno, 1, 12);

            Console.WriteLine("bit0:\t\t{0}", bit0 ? 1 : 0);
            Console.WriteLine("mensagem:\t{0}", mensagem);
        }
        #endregion

        #region clickMouse
        private static BitArray msgClickMouse(){
            return BinaryBitsAux.toBits(4, 3);
        }

        private static void tratarMsgClickMouse(BitArray retorno){
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            String mensagem = BinaryBitsAux.toString(retorno, 1, 12);

            Console.WriteLine("bit0:\t\t{0}", bit0 ? 1 : 0);
            Console.WriteLine("mensagem:\t{0}", mensagem);
        }
        #endregion

        #region lockScreen
        private static BitArray msgLockScreen(){
            return BinaryBitsAux.toBits(5, 3);
        }

        private static void tratarMsgLockScreen(BitArray retorno){
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            String mensagem = BinaryBitsAux.toString(retorno, 1, 12);

            Console.WriteLine("bit0:\t\t{0}", bit0 ? 1 : 0);
            Console.WriteLine("mensagem:\t{0}", mensagem);
        }
        #endregion
        #endregion

    }
}