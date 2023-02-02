using AudioSwitcher.AudioApi.CoreAudio;
using auxiliar.tcp;
using auxiliar.testes.tcp;

class Program
{

    private const Int32 port = 9876;

    static void Main(string[] args)
    {
        //new TcpServer(port).start();
        //TestesBinaryBits.testeEntrada5(BinaryBitsAux.to1Bit(true));

        init();
        MainTCPTeste.testeTCPServerClient();
    }


    // melhora o tempo de resposta para o primeiro 'TratarRequisicoesBin.sinchronizar'
    private static void init(){
        CoreAudioDevice defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
        float volume = (float)(defaultPlaybackDevice.Volume);
        //Console.WriteLine("volume: {0}", volume);
    }

}