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
            // Garante que não ultrapasse o tamanho de um Int32 (32 bits)
            int limit = Math.Min(size, p1.Count);
            for (int i = 0; i < limit; i++) { rt[i] = p1[i]; }
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
            return BitConverter.ToSingle(BitArrayToBytes(input), 0);
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

        public static BitArray toBitArray(string msg)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(msg);
            BitArray stringBA = new BitArray(bytes);
            // concatena o tamanho com a string em bits
            return Combine(toBits(stringBA.Count, 12), stringBA);
        }
        
        public static string toString(BitArray input, int posInicial = 1, int sizeBits = 12)
        {
            int length = toInt(splitBitArray(input, posInicial, sizeBits), true);
            BitArray content = splitBitArray(input, posInicial + sizeBits, length);
            return Encoding.UTF8.GetString(BitArrayToBytes(content));
        }
        #endregion

        public static byte[] BitArrayToBytes(BitArray bitarray)
        {
            if (bitarray.Length == 0) { throw new ArgumentException("must have at least length 1", nameof(bitarray)); }

            int num_bytes = (bitarray.Length + 7) / 8;
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
            foreach (var n in bytes)
            {
                Console.Write("'{0}', ", n);
            }
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

        #region convert BitArray
        public static byte[] toByteArray(BitArray input){
            bool[] cbool = new bool[input.Count];
            for (int i = 0; i < input.Count; i++) { cbool[i] = input.Get(i); }

            byte[] convertido = toByteN(cbool);

            // BinaryBitsAux.printBytes(convertido);
            // Console.WriteLine("convertido:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(convertido), convertido.Length);

            return convertido;
        }

        public static BitArray returnBitArray(byte[] input, int size)
        {
            BitArray bitAux = new BitArray(input);
            BitArray retorno = new BitArray(size);
            for(int i = 0; i < retorno.Count; i++)
            {
                retorno.Set(i, bitAux[i]);
            }
            return retorno;
        }

        public static byte[] toByteN(bool[] input) 
        {
            if (input == null || input.Length <= 0) { return new byte[] { 0x0 }; }
            
            // corrige o tamanho do input para criar grupos de 8 bits
            bool[] paddedInput = preencherBits(input);
            
            int numerogrupos = paddedInput.Length / 8;
            byte[] rt = new byte[numerogrupos];
            
            bool[] bits = new bool[8];
            int group = 0;
            int pos = 0;
            
            for (int i = 0; i < paddedInput.Length; i++) 
            {
                bits[pos++] = paddedInput[i];
                if (pos == 8) 
                {
                    pos = 0;
                    rt[group++] = toByte(bits, true);
                }
            }
            
            return rt;
        }

        public static bool[] preencherBits(bool[] input) 
        {
            return preencherBits(input, true);
        }

        public static bool[] preencherBits(bool[] input, bool appendFinal) 
        {
            if (input == null || input.Length <= 0) { return Array.Empty<bool>(); }
            
            int length = input.Length;
            int falta = (8 - (length % 8)) % 8;
            
            if (falta <= 0) { return input; }
            
            bool[] rt = new bool[length + falta];
            if (appendFinal) 
            {
                Array.Copy(input, 0, rt, 0, length);
                // O restante já é false por padrão no C#
            } 
            else 
            {
                Array.Copy(input, 0, rt, falta, length);
            }
            return rt;
        }

        public static byte toByte(bool[] bits, bool bigendian) 
        {
            byte bf = 0x0;
            for (int i = 0; i < 8; i++) 
            {
                if (bits[i]) 
                {
                    if (bigendian)
                        bf |= (byte)(1 << i);
                    else
                        bf |= (byte)(1 << (7 - i));
                }
            }
            return bf;
        }
        #endregion
    }
}