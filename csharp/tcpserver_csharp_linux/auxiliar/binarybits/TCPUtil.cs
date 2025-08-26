using System.Collections;
using System.Net.Sockets;

namespace auxiliar.binarybits
{
    public class TCPUtil
    { 

        private static int PACKAGESIZE = 10;

        public static void sendPackage(BitArray data, NetworkStream stream){
            // headers
            data = BinaryBitsAux.Combine(BinaryBitsAux.Combine(BinaryBitsAux.toBits(data.Length, PACKAGESIZE), data));
            byte[] buffer = BinaryBitsAux.toByteArray(data);
            stream.Write(buffer, 0, buffer.Length);
            //Console.WriteLine("[sendPackage] buffer: {0} [{1}]", BinaryBitsAux.ToBitString(buffer), buffer.Length);
            Console.WriteLine("[sendPackage] data: {0} [{1}]", BinaryBitsAux.ToBitString(data), data.Length);
        }

        public static BitArray receivePackage(NetworkStream stream){
            byte[] buffer = new byte[200];
            int _ = stream.Read(buffer, 0, buffer.Length);
            
            BitArray aux = new BitArray(buffer);
            BitArray tamanhoBA = BinaryBitsAux.splitBitArray(aux, 0, PACKAGESIZE);
            int tamanho = BinaryBitsAux.toInt(tamanhoBA, true);
            BitArray data = BinaryBitsAux.splitBitArray(aux, PACKAGESIZE, tamanho);

            Console.WriteLine("[receivePackage] tamanho: {0}", tamanho);
            //Console.WriteLine("[receivePackage] buffer: {0} [{1}]", BinaryBitsAux.ToBitString(buffer), buffer.Length);
            Console.WriteLine("[receivePackage] data: {0} [{1}]", BinaryBitsAux.ToBitString(data), data.Length);

            return data;
        }

    }
}