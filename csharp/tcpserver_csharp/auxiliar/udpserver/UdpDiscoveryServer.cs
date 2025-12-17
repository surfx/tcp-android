using System.Net;
using System.Net.Sockets;
using System.Text;

namespace tcpserver_csharp.auxiliar.udpserver
{

    public class UdpDiscoveryServer
    {
        private const int DISCOVERY_PORT = 9875;
        private readonly int _tcpPort;

        public UdpDiscoveryServer(int tcpPort)
        {
            _tcpPort = tcpPort;
        }

        public void Start()
        {
            new Thread(() =>
            {
                using UdpClient udp = new(DISCOVERY_PORT);
                IPEndPoint remoteEP = new(IPAddress.Any, 0);

                Console.WriteLine("UDP Discovery listening on port " + DISCOVERY_PORT);

                while (true)
                {
                    byte[] data = udp.Receive(ref remoteEP);
                    string msg = Encoding.UTF8.GetString(data);

                    if (msg == "DISCOVER_TCP_SERVER")
                    {
                        Console.WriteLine("Discovery request from " + remoteEP.Address);

                        string? localIp = GetLocalIPv4SameSubnet(remoteEP.Address);

                        if (localIp == null)
                        {
                            Console.WriteLine("No matching LAN IP found, ignoring request");
                            continue;
                        }

                        string response = $"TCP_SERVER;{localIp};{_tcpPort}";
                        byte[] respBytes = Encoding.UTF8.GetBytes(response);

                        udp.Send(respBytes, respBytes.Length, remoteEP);

                        Console.WriteLine("Responded with " + response);
                    }
                }
            })
            { IsBackground = true }
            .Start();
        }

        /**
         * Retorna o IP local que está na MESMA sub-rede /24
         * do cliente que enviou o UDP
         */
        private string? GetLocalIPv4SameSubnet(IPAddress remoteAddress)
        {
            try
            {
                using Socket socket = new Socket(
                    AddressFamily.InterNetwork,
                    SocketType.Dgram,
                    ProtocolType.Udp);

                // Não envia nada, só força o SO a escolher a rota
                socket.Connect(remoteAddress, 65530);

                if (socket.LocalEndPoint is IPEndPoint localEp)
                {
                    return localEp.Address.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Route-based IP detection failed: " + ex.Message);
            }

            return null;
        }

    }

}