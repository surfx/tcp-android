using auxiliar.binarybits;
using System.Collections;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using tcpserver_csharp.auxiliar.tratarrequests;
using tcpserver_csharp.auxiliar.tratarrequests.linux;
using tcpserver_csharp.auxiliar.tratarrequests.windows;
using tcpserver_csharp.auxiliar.utils;

namespace auxiliar.tcp
{
    public class ServerTCP
    {
        private int _port;

        //readonly BitArray codErro = BinaryBitsAux.to1Bit(false);
        //readonly BitArray codOk = BinaryBitsAux.to1Bit(true);

        public ServerTCP(int port = 9876){
            if (port <= 0) { port = 9876; }
            this._port = port;
        }

        public void start(){
            this.printCabecalho();
            this.tcpServer();
        }

        private void printCabecalho()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("Local LAN address:");

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Interface ativa
                if (ni.OperationalStatus != OperationalStatus.Up)
                    continue;

                // Ignora loopback e túnel
                if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                    continue;

                var ipProps = ni.GetIPProperties();

                foreach (var addr in ipProps.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    var ip = addr.Address.ToString();

                    // Ignora loopback explícito
                    if (IPAddress.IsLoopback(addr.Address))
                        continue;

                    // Aceita apenas LANs "clássicas"
                    if (IsClassicLanIp(ip))
                    {
                        Console.WriteLine(ip);
                    }
                }
            }

            Console.WriteLine("Server TCP port " + _port);
            Console.WriteLine("--------------------");
        }

        private bool IsClassicLanIp(string ip)
        {
            // 192.168.x.x (LAN doméstica)
            if (ip.StartsWith("192.168."))
                return true;

            // 10.x.x.x (algumas LANs)
            if (ip.StartsWith("10."))
                return true;

            // Rejeita 172.16–172.31 (Docker, WSL, VPN na maioria dos casos)
            if (ip.StartsWith("172."))
                return false;

            return false;
        }

        /*
            https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcplistener?view=net-7.0

            Cliente TCP conecta, envia mensagem e morre
            TODO: talvez no futuro melhorar o código para manter a conexão ativa.
            TODO 2: se for o caso, melhorar o código tcp server em Java também
        */
        private void tcpServer()
        {

            ITratarRequisicoesBin tratarRequests = OSDetector.IsWindows() ? new TratarRequisicoesBinWindows() : new TratarRequisicoesBinLinux();

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