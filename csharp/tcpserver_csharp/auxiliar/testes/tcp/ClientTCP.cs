
// https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcpclient?view=net-7.0
using System.Collections;
using System.Net.Sockets;
using auxiliar.binarybits;

namespace auxiliar.testes.tcp
{
    public class ClientTCP
    {

        public static void sendMessage(BitArray pacote, String server="localhost", int port = 9876)
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

                    // -- parse retorno
                    bool bit0 = BinaryBitsAux.splitBitArray(resposta, 0, 1)[0];
                    BitArray volumeRetornoParser = BinaryBitsAux.splitBitArray(resposta, 1, 32);

                    Console.WriteLine("bit0:\t\t\t{0}", bit0);
                    Console.WriteLine("volumeRetornoParser: \t{0}, valor: {1}", BinaryBitsAux.ToBitString(volumeRetornoParser), BinaryBitsAux.toFloat(volumeRetornoParser));

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