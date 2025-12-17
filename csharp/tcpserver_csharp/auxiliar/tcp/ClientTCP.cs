
// https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcpclient?view=net-7.0
using System.Collections;
using System.Net.Sockets;
using auxiliar.binarybits;

namespace auxiliar.tcp
{
    public class ClientTCP
    {
        private int port = 9876;
        private string server="localhost";

        public ClientTCP(string server="localhost", int port = 9876){
            this.server = server;
            this.port = port;
        }

        public void sendMessage(BitArray pacote, Action<BitArray> tratarRespostaServer)
        {
            new Thread(new ThreadStart(() =>
            {
                try
                {

                    using TcpClient client = new TcpClient(server, port);

                    //Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    NetworkStream stream = client.GetStream();

                    TCPUtil.sendPackage(pacote, stream);

                    //---------------------------------------------
                    // Receive the server response.
                    BitArray resposta = TCPUtil.receivePackage(stream);
                    Console.WriteLine("[c] Received:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(resposta), resposta.Length);

                    tratarRespostaServer(resposta);

                    stream.Close();
                    client.Close();
                }
                catch (ArgumentNullException e)
                {
                    Console.WriteLine("ArgumentNullException: {0}", e);
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }

            })).Start();

            //Console.WriteLine("\n Press Enter to continue..."); Console.Read();

        }
    }
}