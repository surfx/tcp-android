// See https://aka.ms/new-console-template for more information
// https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcplistener?view=net-7.0

using System.Collections;
using auxiliar.binarybits;
using auxiliar.tcp;
using auxiliar.testes;
using auxiliar.testes.tcp;

class Program
{

    private const Int32 port = 9876;

    static void Main(string[] args)
    {
        //new TcpServer(port).start();
        //TestesBinaryBits.testeEntrada5(BinaryBitsAux.to1Bit(true));

        MainTCPTeste.testeTCPServerClient();
        //testeToByte();

        //teste1();
    }

    private static void testeToByte(){
        BitArray b1Teste = new BitArray(new byte[] { 0x1, 0x2, 0x45 });

        byte[] teste = BitConverter.GetBytes(5);
        BinaryBitsAux.printBytes(teste);
        BitArray p1 = new BitArray(teste);
        BitArray p2 = new BitArray(new bool[] { p1[0], p1[1], p1[2] });

        BitArray b3Teste = BinaryBitsAux.Combine(b1Teste, p2);

        Console.WriteLine("b1Teste:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(b1Teste), b1Teste.Count);
        Console.WriteLine("b3Teste:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(b3Teste), b3Teste.Count);

        byte[] convertido = BinaryBitsAux.toByteArray(b3Teste);
        BinaryBitsAux.printBytes(convertido);
        Console.WriteLine("convertido:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(convertido), convertido.Length);
        Console.WriteLine("--------------------");

        //-- voltar a conversão
        int size = b3Teste.Count; //27
        BitArray bitAux = new BitArray(convertido);
        BitArray retorno = new BitArray(27);
        for(int i = 0; i < retorno.Count; i++){
            retorno.Set(i, bitAux[i]);
        }
        Console.WriteLine("retorno:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(retorno), retorno.Length);


    }

    private static void teste1(){
        //Func<int, int, int> d1 = delegate(int x, int y) { return x * y; };
        teste2( (x,y)=> { return x*y; }, 3, 8 );
    }

    private static void teste2(Func<int, int, int> apply, int v1, int v2){
        int v3 = apply(v1,v2);
        Console.WriteLine("v3: {0}", v3);
    }
    

}