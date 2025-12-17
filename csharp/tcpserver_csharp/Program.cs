using AudioSwitcher.AudioApi.CoreAudio;
using auxiliar.tcp;

//using auxiliar.tcp;
using tcpserver_csharp.auxiliar.udpserver;
using tcpserver_csharp.auxiliar.utils;

class Program
{

    private const int port_tcp = 9876; // porta do servidor tcp
    private const int port_udp = 9875; // porta para broadcast udp

    static void Main(string[] args)
    {
        init();
    }
    
    private static void init()
    {
        if (OSDetector.IsWindows())
        {
            // melhora o tempo de resposta para o primeiro 'TratarRequisicoesBin.sinchronizar'
            CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            float volume = (float)(defaultPlaybackDevice.Volume);
            //Console.WriteLine("volume: {0}", volume);
        }


        MainTCPBin.startTcpBinServer(port_tcp);

        var udpDiscovery = new UdpDiscoveryServer(port_udp, port_tcp);
        udpDiscovery.Start();
    }

    // Código legado
    //new TcpServer(port).start();
    //TestesBinaryBits.testeEntrada5(BinaryBitsAux.to1Bit(true));

}