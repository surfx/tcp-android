// See https://aka.ms/new-console-template for more information
// https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcplistener?view=net-7.0

using auxiliar.binarybits;
using auxiliar.tcp;
using auxiliar.testes;

class Program
{

    private const Int32 port = 9876;

    static void Main(string[] args)
    {
        //new TcpServer(port).start();
        TestesBinaryBits.testeEntrada5(BinaryBitsAux.to1Bit(true));
    }

}