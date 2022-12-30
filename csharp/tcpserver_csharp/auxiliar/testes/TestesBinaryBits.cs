using System.Collections;
using System.Text;
using auxiliar.binarybits;

namespace auxiliar.testes
{
    public class TestesBinaryBits
    {

        /*
            https://www.programiz.com/csharp-programming/bitwise-operators
        */
        public static void referencia()
        {
            Console.WriteLine("-----------------------");
            int firstNumber = 14, secondNumber = 11;
            //14 = 00001110 (In Binary)
            //11 = 00001011 (In Binary)
            //00001111 = 15 (In Decimal)
            //00001010 = 10 (In Decimal)
            //00000101 = 5 (In Decimal)
            Console.WriteLine("{0} | {1} = {2}", firstNumber, secondNumber, firstNumber | secondNumber);    // 14 | 11 = 15
            Console.WriteLine("{0} & {1} = {2}", firstNumber, secondNumber, firstNumber & secondNumber);    // 14 & 11 = 10
            Console.WriteLine("{0} ^ {1} = {2}", firstNumber, secondNumber, firstNumber ^ secondNumber);    // 14 ^ 11 = 5 (XOR)
            Console.WriteLine("-----------------------");

            // 26 = 00011010 (In Binary)
            Console.WriteLine("~{0} = {1}", 26, ~26);   // ~26 = -27
                                                        // Decimal	Binary	2's Complement
                                                        // 0	00000000	-(11111111 + 1) = -00000000 = -0 (In Decimal)
                                                        // 1	00000001	-(11111110 + 1) = -11111111 = -256 (In Decimal)
                                                        // 229	11100101	-(00011010 + 1) = -00011011 = -27
            Console.WriteLine("-----------------------");

            //# Bitwise Left Shift, << = num * 2bits
            // 42 = 101010 (In Binary)
            // 42 << 1 = 84 (In binary 1010100)
            // 42 << 2 = 168 (In binary 10101000)
            // 42 << 4 = 672 (In binary 1010100000)
            int number = 42;
            Console.WriteLine("{0}<<1 = {1}", number, number << 1);       // 42<<1 = 84
            Console.WriteLine("{0}<<2 = {1}", number, number << 2);       // 42<<2 = 168
            Console.WriteLine("{0}<<4 = {1}", number, number << 4);       // 42<<4 = 672
            Console.WriteLine("-----------------------");

            //# Bitwise Right Shift, >> = floor(num / 2bits)
            // 42 = 101010 (In Binary)
            // 42 >> 1 = 21 (In binary 010101)
            // 42 >> 2 = 10 (In binary 001010)
            // 42 >> 4 = 2 (In binary 000010)
            number = 42;
            Console.WriteLine("{0}>>1 = {1}", number, number >> 1);       // 42>>1 = 21
            Console.WriteLine("{0}>>2 = {1}", number, number >> 2);       // 42>>2 = 10
            Console.WriteLine("{0}>>4 = {1}", number, number >> 4);       // 42>>4 = 2
            Console.WriteLine("-----------------------");

            Console.WriteLine(BinaryBitsAux.convertBaseNumber("42", 10, 2));
        }

        public static void testesDeConversoes()
        {
            // float
            float vIn = 1135.45f;
            byte[] bytes = BitConverter.GetBytes(vIn);  // size 4 bytes
            float retornoFloat = BitConverter.ToSingle(bytes);
            Console.WriteLine("byte[]: {0}[{1}], retornoFloat: {2}", Encoding.UTF8.GetString(bytes), bytes.Length, retornoFloat);

            // string
            bytes = Encoding.ASCII.GetBytes("Minha string C# $% é uma boa ? retorno de função dsf");
            string str = Encoding.ASCII.GetString(bytes);
            Console.WriteLine("bytes: {0}[{1}], retorno: '{2}'", Encoding.UTF8.GetString(bytes), bytes.Length, str);

            // int
            bytes = BitConverter.GetBytes(12345); // int
            Console.WriteLine("bytes: {0}[{1}], retorno: {2}", Encoding.UTF8.GetString(bytes), bytes.Length, BitConverter.ToInt32(bytes));

            // unir
            Console.WriteLine("-----------------------");
            BinaryBitsAux.printBytes(BitConverter.GetBytes(vIn));
            BinaryBitsAux.printBytes(BitConverter.GetBytes(12345));
            Console.WriteLine("-----------------------");
            bytes = BinaryBitsAux.CombineOld(BitConverter.GetBytes(vIn), BitConverter.GetBytes(12345));
            BinaryBitsAux.printBytes(bytes);

            // ler parte de um byte[]
            // 0 a 4
            int tamanho = 6;
            int posicaoCopiar = 2;
            byte[] parte = new byte[tamanho];
            // Array.Copy(Source,Source index, target,target index, length);
            Array.Copy(bytes, posicaoCopiar, parte, 0, tamanho);
            BinaryBitsAux.printBytes(parte);

        }

        public static void testeCombine()
        {
            // ---- Combine
            BitArray b1Teste = new BitArray(new byte[] { 0x1, 0x2, 0x45 });
            BitArray b2Teste = new BitArray(new byte[] { 0x5, 0x24, 0x8 });
            BitArray b3Teste = BinaryBitsAux.Combine(b1Teste, b2Teste);
            Console.WriteLine("b1Teste:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(b1Teste), b1Teste.Count);
            Console.WriteLine("b2Teste:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(b2Teste), b2Teste.Count);
            Console.WriteLine("b3Teste:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(b3Teste), b3Teste.Count);


            BitArray b4Teste = BinaryBitsAux.splitBitArray(b3Teste, 0, 24);
            BitArray b5Teste = BinaryBitsAux.splitBitArray(b3Teste, 24, 24);
            Console.WriteLine("b4Teste:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(b4Teste), b4Teste.Count);
            Console.WriteLine("b5Teste:\t\t{0} [{1}]", BinaryBitsAux.ToBitString(b5Teste), b5Teste.Count);

            Console.WriteLine("compareBitArray(b4Teste, b1Teste): {0}", BinaryBitsAux.compareBitArray(b4Teste, b1Teste));
            Console.WriteLine("compareBitArray(b4Teste, b2Teste): {0}", BinaryBitsAux.compareBitArray(b4Teste, b2Teste));
            Console.WriteLine("compareBitArray(b5Teste, b2Teste): {0}", BinaryBitsAux.compareBitArray(b5Teste, b2Teste));
            Console.WriteLine("compareBitArray(b5Teste, b1Teste): {0}", BinaryBitsAux.compareBitArray(b5Teste, b1Teste));
        }
        //-----------------------------

        public static void testesMensagensServerTCP()
        {

            // 0 - erro, 1 - sucesso
            BitArray codErro = BinaryBitsAux.to1Bit(false);
            BitArray codOk = BinaryBitsAux.to1Bit(true);
            //---------------

            // 0 a 5 - 3 bits - 2^3 = 8 representações

            // 0 - 000  - 0*2^2 + 0*2^1 + 0*2^0 = 0+0+0 = 0
            // 1 - 001  - 0*2^2 + 0*2^1 + 1*2^0 = 0+0+1 = 1
            // 2 - 010  - 0*2^2 + 1*2^1 + 0*2^0 = 0+2+0 = 2
            // 3 - 011  - 0*2^2 + 1*2^1 + 1*2^0 = 0+2+1 = 3
            // 4 - 100  - 1*2^2 + 0*2^1 + 0*2^0 = 4+0+0 = 4
            // 5 - 101  - 1*2^2 + 0*2^1 + 1*2^0 = 4+0+1 = 5

            // 4 bits - 2^4 = 16 representações

            //BitArray bcod0 = new BitArray(3);

            byte[] teste = BitConverter.GetBytes(5);
            BinaryBitsAux.printBytes(teste);


            BitArray p1 = new BitArray(teste); // para ter acesso aos bytes
            BitArray p2 = new BitArray(new bool[] { p1[0], p1[1], p1[2] }); // recupero os bytes de interesse
            Console.WriteLine(BinaryBitsAux.ToBitString(p2));
            //convertNumber(string numero, int baseNumero, int paraBase)
            // for (int i = 0; i < 6; i++){
            //     Console.WriteLine(convertNumber("" + i, 10, 2));
            // }

            Console.WriteLine("-----------------------------------------");
            testeEntrada0(codOk);
            Console.WriteLine("-----------------------------------------");
            testeEntrada1(codOk);
            Console.WriteLine("-----------------------------------------");
            testeEntrada2(codOk);
            Console.WriteLine("-----------------------------------------");
            testeEntrada3(codOk);
            Console.WriteLine("-----------------------------------------");
            testeEntrada4(codOk);
            Console.WriteLine("-----------------------------------------");
            testeEntrada5(codOk);

            // BitArray entrada2BType = new BitArray(1); entrada2BType[0] = true;
            // byte[] entrada1B = Combine(entrada2BType, BitConverter.GetBytes(13.97f)); // 13.97 float - 4 bytes
            // printBytes(entrada1B);

        }


        #region entradas
        public static void testeEntrada0(BitArray codOk)
        {

            // 0 - sincronizar: 3 bits (valor: 000)
            // retorno: 1 bit (valor: 1 - codOk) + 4 bytes (float volume) = 32 bits

            Console.WriteLine("-----------------ENTRADA-------------------");
            // 3 + 32 bits
            BitArray entrada = BinaryBitsAux.Combine(BinaryBitsAux.toBits(0, 3), 75.26f);
            Console.WriteLine(BinaryBitsAux.ToBitString(entrada));

            Console.WriteLine("-----------------PARSE ENTRADA-------------------");
            // -- parse entrada0
            BitArray cod = BinaryBitsAux.splitBitArray(entrada, 0, 3);
            Console.WriteLine("cod: {0} = {1}", BinaryBitsAux.ToBitString(cod), BinaryBitsAux.toInt(cod, true));

            BitArray volumeEntrada = BinaryBitsAux.splitBitArray(entrada, 3, 32);
            Console.WriteLine(BinaryBitsAux.ToBitString(volumeEntrada));

            Console.WriteLine("----------------RETORNO------------------");
            // -- resposta
            // 1 + 32 bits
            BitArray retorno = BinaryBitsAux.Combine(codOk, 75.26f); // 75.26 float - 4 bytes = 32 bits
            Console.WriteLine(BinaryBitsAux.ToBitString(retorno));

            Console.WriteLine("-----------------PARSE RETORNO-------------------");

            // -- parse retorno
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            BitArray volumeRetornoParser = BinaryBitsAux.splitBitArray(retorno, 1, 32);

            Console.WriteLine("bit0:\t\t\t{0}", bit0);
            Console.WriteLine("volumeRetornoParser: \t{0}, valor: {1}", BinaryBitsAux.ToBitString(volumeRetornoParser), BinaryBitsAux.toFloat(volumeRetornoParser));

        }

        public static void testeEntrada1(BitArray codOk)
        {
            // 1 - alterar o volume: 3 bits (valor: 100) + 4 bytes (float volume)
            // retorno: 1 bit (valor: 1 - codOk) + 4 bytes (float volume) = 32 bits

            Console.WriteLine("-----------------ENTRADA-------------------");
            BitArray entrada = BinaryBitsAux.Combine(BinaryBitsAux.toBits(1, 3), 13.97f); // 13.97 float - 4 bytes - 32 bits
            Console.WriteLine(BinaryBitsAux.ToBitString(entrada));

            Console.WriteLine("-----------------PARSE ENTRADA-------------------");
            BitArray cod = BinaryBitsAux.splitBitArray(entrada, 0, 3);
            Console.WriteLine("cod: {0} = {1}", BinaryBitsAux.ToBitString(cod), BinaryBitsAux.toInt(cod, true));

            BitArray volumeEntrada = BinaryBitsAux.splitBitArray(entrada, 3, 32);
            Console.WriteLine(BinaryBitsAux.ToBitString(volumeEntrada));

            Console.WriteLine("----------------RETORNO------------------");
            // retorno: 1 bit (valor: 1 - codOk) + 4 bytes (float volume) = 32 bits
            BitArray retorno = BinaryBitsAux.Combine(codOk, 93.27f); // 93.27 float - 4 bytes = 32 bits
            Console.WriteLine(BinaryBitsAux.ToBitString(retorno));

            Console.WriteLine("-----------------PARSE RETORNO-------------------");
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            BitArray volumeRetornoParser = BinaryBitsAux.splitBitArray(retorno, 1, 32);

            Console.WriteLine("bit0:\t\t\t{0}", bit0);
            Console.WriteLine("volumeRetornoParser: \t{0}, valor: {1}", BinaryBitsAux.ToBitString(volumeRetornoParser), BinaryBitsAux.toFloat(volumeRetornoParser));

        }

        public static void testeEntrada2(BitArray codOk)
        {
            // tamanho do byte[] de uma string depende da entrada (length da string)
            // 2 - desligar: 3 bits (valor: 2)
            // retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)

            Console.WriteLine("-----------------ENTRADA-------------------");
            BitArray entrada = BinaryBitsAux.toBits(2, 3);
            Console.WriteLine(BinaryBitsAux.ToBitString(entrada));

            Console.WriteLine("-----------------PARSE ENTRADA-------------------");
            BitArray cod = BinaryBitsAux.splitBitArray(entrada, 0, 3);
            Console.WriteLine("cod: {0} = {1}", BinaryBitsAux.ToBitString(cod), BinaryBitsAux.toInt(cod, true));

            Console.WriteLine("----------------RETORNO------------------");
            // retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)

            String rt = "Minha string C# $% é uma boa ? retorno de função dsf";
            rt = "Lorem Ipsum é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde o século XVI, quando um impressor desconhecido pegou uma bandeja de tipos e os embaralhou para fazer um livro de modelos de tipos. Lorem Ipsum sobreviveu não só a cinco séculos, como também ao salto para a editoração eletrônica, permanecendo essencialmente inalterado. Se popularizou na década de 60, quando a Letraset lançou decalques contendo passagens de Lorem Ipsum, e mais recentemente quando passou a ser integrado a softwares de editoração eletrônica como Aldus PageMaker.  Porque nós o usamos? É um fato conhecido de todos que um leitor se distrairá com o conteúdo de texto legível de uma página quando estiver examinando sua diagramação. A vantagem de usar Lorem Ipsum é que ele tem uma distribuição normal de letras, ao contrário de \"Conteúdo aqui, conteúdo aqui\", fazendo com que ele tenha uma aparência similar a de um texto legível. Muitos softwares de publicação e editores de páginas na internet agora usam Lorem Ipsum como texto-modelo padrão, e uma rápida busca por 'lorem ipsum' mostra vários websites ainda em sua fase de construção. Várias versões novas surgiram ao longo dos anos, eventualmente por acidente, e às vezes de propósito (injetando humor, e coisas do gênero).   De onde ele vem? Ao contrário do que se acredita, Lorem Ipsum não é simplesmente um texto randômico. Com mais de 2000 anos, suas raízes podem ser encontradas em uma obra de literatura latina clássica datada de 45 AC. Richard McClintock, um professor de latim do Hampden-Sydney College na Virginia, pesquisou uma das mais obscuras palavras em latim, consectetur, oriunda de uma passagem de Lorem Ipsum, e, procurando por entre citações da palavra na literatura clássica, descobriu a sua indubitável origem. Lorem Ipsum vem das seções 1.10.32 e 1.10.33 do \"de Finibus Bonorum et Malorum\" (Os Extremos do Bem e do Mal), de Cícero, escrito em 45 AC. Este livro é um tratado de teoria da ética muito popular na época da Renascença. A primeira linha de Lorem Ipsum, \"Lorem Ipsum dolor sit amet...\" vem de uma linha na seção 1.10.32.  O trecho padrão original de Lorem Ipsum, usado desde o século XVI, está reproduzido abaixo para os interessados. Seções 1.10.32 e 1.10.33 de \"de Finibus Bonorum et Malorum\" de Cicero também foram reproduzidas abaixo em sua forma exata original, acompanhada das versões para o inglês da tradução feita por H. Rackham em 1914.";
            rt = rt.Substring(0, 300);

            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            Console.WriteLine("-----------------PARSE RETORNO-------------------");

            // -- parse retorno
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            String mensagem = BinaryBitsAux.toString(retorno, 1, 12);

            Console.WriteLine("bit0:\t\t{0}", bit0 ? 1 : 0);
            Console.WriteLine("mensagem:\t{0}", mensagem);

        }

        public static void testeEntrada3(BitArray codOk)
        {
            // 3 1920 x 1080 , 1370 x 425
            // 3 - mouse: 3 bits (valor: 3) + 13 bits (width celular) + 13 bits (height celular) + 13 bits (x celular) + 13 bits (y celular)
            // retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)

            Console.WriteLine("-----------------ENTRADA-------------------");
            BitArray entrada = BinaryBitsAux.Combine(BinaryBitsAux.toBits(3, 3), BinaryBitsAux.toBits(1920, 13), BinaryBitsAux.toBits(1080, 13), BinaryBitsAux.toBits(1370, 13), BinaryBitsAux.toBits(425, 13));
            Console.WriteLine("entrada:\t{0}", BinaryBitsAux.ToBitString(entrada));

            Console.WriteLine("-----------------PARSE ENTRADA-------------------");
            BitArray cod = BinaryBitsAux.splitBitArray(entrada, 0, 3);
            Console.WriteLine("cod: {0} = {1}, wc: {2}, hc: {3}, xc: {4}, yc: {5}",
                BinaryBitsAux.ToBitString(cod), BinaryBitsAux.toInt(cod, true),
                BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 3, 13), true),
                BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 13 + 3, 13), true),
                BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 13 + 13 + 3, 13), true),
                BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(entrada, 13 + 13 + 13 + 3, 13), true)
            );

            Console.WriteLine("----------------RETORNO------------------");
            // retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)

            String rt = "Recebido";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            Console.WriteLine("-----------------PARSE RETORNO-------------------");
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            String mensagem = BinaryBitsAux.toString(retorno, 1, 12);

            Console.WriteLine("bit0:\t\t{0}", bit0 ? 1 : 0);
            Console.WriteLine("mensagem:\t{0}", mensagem);

        }

        public static void testeEntrada4(BitArray codOk)
        {
            // 4 - click mouse: 3 bits (valor: 4)
            // retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)

            Console.WriteLine("-----------------ENTRADA-------------------");
            BitArray entrada = BinaryBitsAux.toBits(4, 3);
            Console.WriteLine("entrada: {0}", BinaryBitsAux.ToBitString(entrada));

            Console.WriteLine("-----------------PARSE ENTRADA-------------------");
            BitArray cod = BinaryBitsAux.splitBitArray(entrada, 0, 3);
            Console.WriteLine("cod: {0} = {1}", BinaryBitsAux.ToBitString(cod), BinaryBitsAux.toInt(cod, true));

            Console.WriteLine("----------------RETORNO------------------");
            // retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)

            String rt = "Recebido";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            Console.WriteLine("-----------------PARSE RETORNO-------------------");
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            String mensagem = BinaryBitsAux.toString(retorno, 1, 12);

            Console.WriteLine("bit0:\t\t{0}", bit0 ? 1 : 0);
            Console.WriteLine("mensagem:\t{0}", mensagem);


        }

        public static void testeEntrada5(BitArray codOk)
        {
            // 5 - Lock Screen: 3 bits (valor: 5)
            // retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)

            Console.WriteLine("-----------------ENTRADA-------------------");
            BitArray entrada = BinaryBitsAux.toBits(5, 3);
            Console.WriteLine("entrada: {0}", BinaryBitsAux.ToBitString(entrada));

            Console.WriteLine("-----------------PARSE ENTRADA-------------------");
            BitArray cod = BinaryBitsAux.splitBitArray(entrada, 0, 3);
            Console.WriteLine("cod: {0} = {1}", BinaryBitsAux.ToBitString(cod), BinaryBitsAux.toInt(cod, true));

            Console.WriteLine("----------------RETORNO------------------");
            // retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)

            String rt = "Recebido";
            BitArray retorno = BinaryBitsAux.Combine(codOk, BinaryBitsAux.toBitArray(rt));
            Console.WriteLine("retorno: {0}", BinaryBitsAux.ToBitString(retorno));

            Console.WriteLine("-----------------PARSE RETORNO-------------------");
            bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
            String mensagem = BinaryBitsAux.toString(retorno, 1, 12);

            Console.WriteLine("bit0:\t\t{0}", bit0 ? 1 : 0);
            Console.WriteLine("mensagem:\t{0}", mensagem);
        }


        #endregion
    }
}