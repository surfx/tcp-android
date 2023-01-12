using auxiliar.tcp;

class Program
{

    private const Int32 port = 9876;

    static void Main(string[] args)
    {
        new TcpServer(port).start();
        //TestesBinaryBits.testeEntrada5(BinaryBitsAux.to1Bit(true));

        //MainTCPTeste.testeTCPServerClient();
    }


}