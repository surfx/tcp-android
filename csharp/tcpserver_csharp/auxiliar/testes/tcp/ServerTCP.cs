using System.Collections;
using System.Net;
using System.Net.Sockets;
using auxiliar.binarybits;
using auxiliar.tratarrequests;

namespace auxiliar.testes.tcp
{
    public class ServerTCP
    {
        private int _port;

        //readonly BitArray codErro = BinaryBitsAux.to1Bit(false);
        //readonly BitArray codOk = BinaryBitsAux.to1Bit(true);

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
            Console.WriteLine("Server TCP bin port " + _port);
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
            TratarRequisicoesBin tratarRequests = new TratarRequisicoesBin();

            new Thread(new ThreadStart(() =>
            {
                TcpListener? server = null;

                try
                {
                    //IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                    //server = new TcpListener(localAddr, port);
                    server = new TcpListener(IPAddress.Any, _port);
                    server.Start();

                    while (true)
                    {
                        Console.Write("Waiting for a connection... ");

                        TcpClient client = server.AcceptTcpClient();

                        ThreadPool.QueueUserWorkItem((obj) => {
                            if (obj == null){ return; }
                            TcpClient myClient = (TcpClient)obj;

                            Console.WriteLine("Connected!");

                            NetworkStream stream = myClient.GetStream();

                            // -- receive
                            BitArray entrada = TCPUtil.receivePackage(stream);
                            //----------------------------

                            BitArray resposta = tratarRequests.tratar(entrada);

                            TCPUtil.sendPackage(resposta, stream);
                            
                            //resposta = tratarRequisicoes.tratarRequisicoesTCP(data);
                            stream.Close();
                            
                            myClient.Close(); myClient.Dispose();

                        }, client);

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