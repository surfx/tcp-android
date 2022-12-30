// See https://aka.ms/new-console-template for more information
// https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcplistener?view=net-7.0

using auxiliar.tcp;

class Program
{

    private const Int32 port = 9876;

    static void Main(string[] args)
    {
        new TcpServer(port).start();
    }

}