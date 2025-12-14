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
                UdpClient udp = new UdpClient(DISCOVERY_PORT);
                IPEndPoint remoteEP = new IPEndPoint(IPAddress.Any, 0);

                Console.WriteLine("UDP Discovery listening on port " + DISCOVERY_PORT);

                while (true)
                {
                    byte[] data = udp.Receive(ref remoteEP);
                    string msg = Encoding.UTF8.GetString(data);

                    if (msg == "DISCOVER_TCP_SERVER")
                    {
                        string localIp = GetLocalIPv4();
                        string response = $"TCP_SERVER;{localIp};{_tcpPort}";
                        byte[] respBytes = Encoding.UTF8.GetBytes(response);

                        udp.Send(respBytes, respBytes.Length, remoteEP);
                    }
                }
            }).Start();
        }

        private string GetLocalIPv4()
        {
            foreach (var ip in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    return ip.ToString();
            }
            return "127.0.0.1";
        }
    }

}