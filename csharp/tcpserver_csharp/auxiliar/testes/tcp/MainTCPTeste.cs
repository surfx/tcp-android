using System.Collections;
using auxiliar.binarybits;

namespace auxiliar.testes.tcp
{
    public class MainTCPTeste
    {
        
        private static int port = 9876;

        public static void testeTCPServerClient(){
            new ServerTCP(port).start();

            Thread.Sleep(100);

            // BitArray pacote = BinaryBitsAux.Combine(BinaryBitsAux.toBits(0, 3), 75.26f);
            // ClientTCP.sendMessage(pacote, "localhost", port);
        }

    }
}