using auxiliar.testes.tcp;
using tcpserver_csharp_linux.linuxaux;

class Program
{

    private const Int32 port = 9876;

    static void Main(string[] args)
    {
        init();
    }


    // melhora o tempo de resposta para o primeiro 'TratarRequisicoesBin.sinchronizar'
    private static void init()
    {
        // string volume = VolumeLinux.GetVolume();
        // Console.WriteLine("Volume atual: " + volume);
        // VolumeLinux.SetVolume(24);
        // volume = VolumeLinux.GetVolume();
        // Console.WriteLine("Volume atual: " + volume);

        MainTCPBin.startTcpBinServer();
    }

    


}