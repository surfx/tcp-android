using System.Collections;
using System.Text;

namespace auxiliar.binarybits
{
    public class BinaryBitsAux
    {

        /*
            converte bases númericas
        */
        public static string convertBaseNumber(string numero, int baseNumero, int paraBase)
        {
            return Convert.ToString(Convert.ToInt32(numero, baseNumero), paraBase);
        }

        #region combine
        public static byte[] CombineOld(byte[] first, byte[] second)
        {
            return first.Concat(second).ToArray();
        }

        public static byte[] CombineOld(BitArray first, byte[] second)
        {
            byte[] rt = new byte[first.Count + second.Length];
            int pos = 0;
            for (int i = 0; i < first.Count; i++)
            {
                rt[pos++] = (byte)(first[i] ? 1 : 0);
            }
            for (int i = 0; i < second.Length; i++)
            {
                rt[pos++] = second[i];
            }
            return rt;
        }


        public static BitArray Combine(byte[] first, byte[] second)
        {
            return Combine(new BitArray(first), new BitArray(second));
        }

        public static BitArray Combine(BitArray first, float second)
        {
            return Combine(first, BitConverter.GetBytes(second));
        }

        public static BitArray Combine(BitArray first, byte[] second)
        {
            return Combine(first, new BitArray(second));
        }

        public static BitArray Combine(params BitArray[] arrays)
        {
            if (arrays == null || arrays.Length <= 0) { return new BitArray(0); }
            if (arrays.Length == 1) { return arrays[0]; }
            int size = 0, pos = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                size += arrays[i].Count;
            }
            BitArray retorno = new BitArray(size);
            for (int i = 0; i < arrays.Length; i++)
            {
                for (int j = 0; j < arrays[i].Count; j++)
                {
                    retorno[pos++] = arrays[i][j];
                }
            }
            return retorno;
        }
        #endregion

        #region converters to
        public static BitArray toBits(int valor, int size)
        {
            BitArray p1 = new BitArray(new int[] { valor });
            BitArray rt = new BitArray(size);
            for (int i = 0; i < size; i++) { rt[i] = p1[i]; }
            return rt; // recupero os bytes de interesse
        }
        public static BitArray to1Bit(int valor)
        {
            return to1Bit(valor == 1);
        }
        public static BitArray to1Bit(bool valor)
        {
            return new BitArray(new bool[] { valor });
        }

        public static float toFloat(BitArray input)
        {
            return BitConverter.ToSingle(BitArrayToBytes(input));
        }

        /*
            convert Big-endian e Little-endian to int
        */
        public static int toInt(BitArray input, bool inverse = false)
        {
            int rt = 0;
            int length = input.Count;
            for (int i = 0; i < length; i++)
            {
                if (!input[i]) { continue; }
                rt += (int)Math.Pow(2, inverse ? i : length - i - 1);
            }
            return rt;
        }

        public static BitArray toBitArray(String msg)
        {
            BitArray stringBA = new BitArray(Encoding.UTF8.GetBytes(msg));
            // concatena o tamanho com a string em bits
            return Combine(toBits(stringBA.Count, 12), stringBA);
        }
        
        public static string toString(BitArray input, int posInicial = 1, int sizeBits = 12)
        {
            return Encoding.UTF8.GetString(BitArrayToBytes(splitBitArray(input, posInicial + sizeBits, toInt(splitBitArray(input, posInicial, sizeBits), true))));
        }
        #endregion

        public static byte[] BitArrayToBytes(BitArray bitarray)
        {
            if (bitarray.Length == 0) { throw new System.ArgumentException("must have at least length 1", "bitarray"); }

            int num_bytes = bitarray.Length / 8;
            if (bitarray.Length % 8 != 0) { num_bytes += 1; }

            var bytes = new byte[num_bytes];
            bitarray.CopyTo(bytes, 0);
            return bytes;
        }

        public static BitArray splitBitArray(BitArray input, int posInicial, int size)
        {
            BitArray rt = new BitArray(size);
            for (int i = 0; i < size; i++)
            {
                rt[i] = input[posInicial++];
            }
            return rt;
        }

        public static bool compareBitArray(BitArray input1, BitArray input2)
        {
            if (input1.Count != input2.Count) { return false; }
            for (int i = 0; i < input1.Count; i++)
            {
                if (input1[i] != input2[i]) { return false; }
            }
            return true;
        }

        #region print + to 0/1
        public static void printBytes(byte[] bytes)
        {
            Console.Write("size: {0}. ", bytes.Length);
            bytes.ToList().ForEach(n => Console.Write("'{0}', ", n));
            Console.WriteLine();
        }

        public static string ToBitString(byte[] bytes)
        {
            return ToBitString(new BitArray(bytes));
        }

        public static string ToBitString(BitArray bits)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < bits.Count; i++)
            {
                sb.Append(bits[i] ? '1' : '0');
            }
            return sb.ToString();
        }
        #endregion



        //--------
        #region convert BitArray
        public static byte[] toByteArray(BitArray input){
            bool[] cbool = new bool[input.Count];
            for (int i = 0; i < input.Count; i++) { cbool[i] = input.Get(i); }

            byte[] convertido = toByteN(cbool);

            // BinaryBitsAux.printBytes(convertido);
            // Console.WriteLine("convertido:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(convertido), convertido.Length);

            return convertido;
        }

        public static BitArray returnBitArray(byte[] input, int size){
            BitArray bitAux = new BitArray(input);
            BitArray retorno = new BitArray(size);
            for(int i = 0; i < retorno.Count; i++){
                retorno.Set(i, bitAux[i]);
            }
            //Console.WriteLine("retorno:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(retorno), retorno.Length);
            return retorno;
        }

        public static byte[] toByteN(bool[] input) {
            if (input == null || input.Length <= 0) { return new byte[] { 0x0 }; }
            
            // corrige o tamanho do input para criar grupos de 8 bits
            input = preencherBits(input);
            
            // separar em grupos de 8 bits
            bool[] bits = new bool[8];
            int numerogrupos = (input.Length / 8) + (input.Length % 8 == 0 ? 0 : 1);
            byte[] rt = new byte[numerogrupos];
            
            int pos = 0;
            int group = 0;
            for (int i = 0; i < input.Length; i++) {
                bits[pos++] = input[i];
                if (i != 0 && (i+1) % 8 == 0) {
                    pos = 0;
                    rt[group++] = toByte(bits, true);
                }
            }
            
            return rt;
        }

        public static bool[] preencherBits(bool[] input) {
            return preencherBits(input, true);
        }
        public static bool[] preencherBits(bool[] input, bool appendFinal) {
            if (input == null || input.Length <= 0) { return input; }
            int length = input.Length;
            int falta = length % 8 == 0 ? 0 : ((length / 8) + 1) * 8 - length;
            
            if (falta <= 0) { return input; }
            bool[] rt = new bool[length + falta];
            if (appendFinal) {
                for (int i = 0; i < length; i++) {
                    rt[i] = input[i];
                }
                for (int i = length; i < falta; i++) {
                    rt[i] = false;
                }
            } else {
                for (int i = 0; i < falta; i++) {
                    rt[i] = false;
                }
                int pos = 0;
                for (int i = falta; i < length + falta; i++) {
                    rt[i] = input[pos++];
                }
            }
            input = rt;
            return rt;
        }

        public static byte toByte(bool[] bits, bool bigendian) {
            
            // 0xF = 1111
            byte[] aux = new byte[8];
            for(int i = 0; i < 8; i++) {
                aux[i] = (byte)((bits[i]?1:0) & 0xF);
                //System.out.println(toStr(aux[i]));
            }

            byte[] mask = new byte[] {
                0x1,  		// 0000 0001
                0x2,  		// 0000 0010
                0x4,  		// 0000 0100
                0x8,  		// 0000 1000
                0x10, 		// 0001 0000
                0x20, 		// 0010 0000
                0x40, 		// 0100 0000
                (byte) 0x80 // 1000 0000
            };
            
            if (bigendian) {
                aux[0] &= mask[0];	// 0x1 - 0000 0001
                for(int i = 1; i < 8; i++) {
                    aux[i] <<= i;
                    aux[i] &= mask[i];
                }
            } else {
                // littleendian - ao contrário
                for(int i = 0; i < 8; i++) {
                    aux[i] <<= 7-i;
                    aux[i] &= mask[7-i];
                }
            }
            //for(int i = 0; i < 8; i++) { System.out.println(toStr(aux[i])); }
            
            byte bf = 0x0;
            for(int i = 0; i < 8; i++) {
                bf |= aux[i];
            }
            bf &= 0xFF;	// 1111 1111
            //System.out.println(toStr(bf));
            return bf;
        }
        #endregion

    }

}