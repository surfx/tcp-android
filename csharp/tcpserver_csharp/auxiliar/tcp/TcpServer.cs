using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using auxiliar.tratarrequests;

namespace auxiliar.tcp
{
    [Obsolete]
    public class TcpServer
    {
        private int _port;

        public TcpServer(int port = 9876){
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
            TratarRequisicoes tratarRequisicoes = new();

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
                    String? data = null;

                    while (true)
                    {
                        Console.Write("Waiting for a connection... ");

                        TcpClient client = server.AcceptTcpClient();
                        //new Thread(new ThreadStart(() => {
                        Console.WriteLine("Connected!");

                        data = null;
                        NetworkStream stream = client.GetStream();


                        int i = stream.Read(bytes, 0, bytes.Length);
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        String resposta = "0Erro";
                        resposta = tratarRequisicoes.tratarRequisicoesTCP(data);

                        // Send back a response.
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(resposta);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", resposta);
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