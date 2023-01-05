using System.Collections;
using System.Net;
using System.Net.Sockets;
using auxiliar.binarybits;

namespace auxiliar.testes.tcp
{
    public class ServerTCP
    {
        private int _port;

        BitArray codErro = BinaryBitsAux.to1Bit(false);
        BitArray codOk = BinaryBitsAux.to1Bit(true);

        public ServerTCP(int port = 9876){
            this._port = port;
        }

        public void start(){
            this.printCabecalho();
            this.tcpServer();
        }

        private void printCabecalho()
        {
            Console.WriteLine("--------------------");
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine(ip.ToString());
                }
            }
            Console.WriteLine("Server TCP port " + _port);
            Console.WriteLine("--------------------");
        }

        /*
            https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcplistener?view=net-7.0

            Cliente TCP conecta, envia mensagem e morre
            TODO: talvez no futuro melhorar o código para manter a conexão ativa.
            TODO 2: se for o caso, melhorar o código tcp server em Java também
        */
        private void tcpServer()
        {
            TratarRequisicoes tratarRequisicoes = new TratarRequisicoes();

            new Thread(new ThreadStart(() =>
            {
                TcpListener? server = null;

                try
                {
                    //IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                    //server = new TcpListener(localAddr, port);
                    server = new TcpListener(IPAddress.Any, _port);
                    server.Start();

                    Byte[] bytes = new Byte[256];

                    while (true)
                    {
                        Console.Write("Waiting for a connection... ");

                        TcpClient client = server.AcceptTcpClient();
                        //new Thread(new ThreadStart(() => {
                        Console.WriteLine("Connected!");

                        NetworkStream stream = client.GetStream();

                        BitArray entrada = TCPUtil.receivePackage(stream);
                        //----------------------------

                        BitArray cod = BinaryBitsAux.splitBitArray(entrada, 0, 3);
                        Console.WriteLine("[s] cod: {0} = {1}", BinaryBitsAux.ToBitString(cod), BinaryBitsAux.toInt(cod, true));

                        BitArray volumeEntrada = BinaryBitsAux.splitBitArray(entrada, 3, 32);
                        float volume = BinaryBitsAux.toFloat(volumeEntrada);
                        Console.WriteLine("[s] volumeEntrada: {0}, volume: {1}", BinaryBitsAux.ToBitString(volumeEntrada), volume);

                        //-----------------
                        Console.WriteLine("----------------RETORNO------------------");
                        // -- resposta
                        // 1 + 32 bits
                        BitArray retorno = BinaryBitsAux.Combine(codOk, 75.26f); // 75.26 float - 4 bytes = 32 bits
                        Console.WriteLine("[s] retorno: {0}", BinaryBitsAux.ToBitString(retorno));

                        TCPUtil.sendPackage(retorno, stream);
                        
                        //resposta = tratarRequisicoes.tratarRequisicoesTCP(data);
                        stream.Close();

                        //})).Start();

                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    if (server != null) { server.Stop(); }
                }
            })).Start();
        }
    }

}