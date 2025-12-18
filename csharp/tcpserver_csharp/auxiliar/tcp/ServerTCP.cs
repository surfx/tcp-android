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
        private readonly int _port;

        public ServerTCP(int port = 9876)
        {
            this._port = port <= 0 ? 9876 : port;
        }

        public void start()
        {
            this.printCabecalho();
            this.tcpServer();
        }

        private void printCabecalho()
        {
            Console.WriteLine("--------------------");
            Console.WriteLine("Local LAN address:");

            // Verifica se está rodando no Docker
            if (IsRunningInContainer())
            {
                Console.WriteLine("[Ambiente: Container/Docker]");
            }

            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (ni.OperationalStatus != OperationalStatus.Up)
                    continue;

                if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                    ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                    continue;

                var ipProps = ni.GetIPProperties();
                if (ipProps == null) continue;

                foreach (var addr in ipProps.UnicastAddresses)
                {
                    if (addr.Address.AddressFamily != AddressFamily.InterNetwork)
                        continue;

                    var ip = addr.Address.ToString();

                    if (IPAddress.IsLoopback(addr.Address))
                        continue;

                    // Se for IP de LAN clássica ou IP de rede Docker (172.x)
                    if (IsClassicLanIp(ip) || ip.StartsWith("172."))
                    {
                        string sufixo = ip.StartsWith("172.") ? " (Docker/Virtual)" : "";
                        Console.WriteLine($"{ip}{sufixo}");
                    }
                }
            }

            Console.WriteLine("Server TCP port " + _port);
            Console.WriteLine("--------------------");
        }

        private bool IsClassicLanIp(string ip)
        {
            if (string.IsNullOrEmpty(ip)) return false;

            // 192.168.x.x (LAN doméstica)
            if (ip.StartsWith("192.168."))
                return true;

            // 10.x.x.x (Empresarial/LAN)
            if (ip.StartsWith("10."))
                return true;

            return false;
        }

        /// <summary>
        /// Detecta se o código está rodando dentro de um container Docker
        /// </summary>
        private bool IsRunningInContainer()
        {
            // O arquivo /.dockerenv é criado pelo Docker no root do container
            bool dockerFileExists = File.Exists("/.dockerenv");
            
            // Em orquestradores (como K8s), pode-se checar variáveis de ambiente
            string? containerEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
            
            return dockerFileExists || (containerEnv != null && containerEnv.ToLower() == "true");
        }

        private void tcpServer()
        {
            ITratarRequisicoesBin tratarRequests = OSDetector.IsWindows() 
                ? new TratarRequisicoesBinWindows() 
                : new TratarRequisicoesBinLinux();

            new Thread(() =>
            {
                TcpListener? server = null;

                try
                {
                    server = new TcpListener(IPAddress.Any, _port);
                    server.Start();

                    while (true)
                    {
                        Console.WriteLine("Waiting for a connection... ");
                        TcpClient client = server.AcceptTcpClient();

                        ThreadPool.QueueUserWorkItem((obj) => {
                            if (obj is not TcpClient myClient) return;

                            try 
                            {
                                Console.WriteLine("Connected!");
                                using NetworkStream stream = myClient.GetStream();

                                // -- receive
                                BitArray entrada = TCPUtil.receivePackage(stream);
                                
                                // -- process
                                BitArray resposta = tratarRequests.tratar(entrada);

                                // -- send
                                TCPUtil.sendPackage(resposta, stream);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Erro no processamento do cliente: {ex.Message}");
                            }
                            finally
                            {
                                myClient.Close();
                            }

                        }, client);
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    server?.Stop();
                }
            }).Start();
        }
    }
}