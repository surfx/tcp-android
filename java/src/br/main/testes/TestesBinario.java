package br.main.testes;

import java.nio.charset.Charset;

import br.main.util.BinaryUtil;
import br.main.util.MyBitSet;

public class TestesBinario {

	// 1 byte = 8 bits
	
	public static void testeMensagensBin(){
		//teste2();
		//teste3();
		//teste4();
		
		//teste5();
		//teste6();
		//teste7();
		//teste8();
		//teste9();
		teste10();
	}
	
    public static void teste1(){
        System.out.println("testeMensagensBin()");

        MyBitSet mbit = new MyBitSet(3);
        mbit.set(true, 0);
        mbit.set(false, 1);
        mbit.set(true, 2);
        System.out.println(mbit.toString());
        
        System.out.println(mbit.size());
        
        for(int i = 0; i < mbit.size(); i++) {
        	System.out.println(i+ ": " +mbit.get(i));
        }
        
        sep();
        mbit = new MyBitSet(30);
        for(int i = 0; i < mbit.size(); i++) {
        	mbit.set(true, i);
        }
        System.out.println(mbit.toString());
        System.out.println(mbit.size());
        
        sep();
        
        // TODO: boolean[] to byte[]
        // 1 byte = 4 bits

//        BitSet bitSet = new BitSet(3);
//        bitSet.set(0, true);
//        bitSet.set(1, false);
//        bitSet.set(2, true);
//
//        for(int i = 0; i< bitSet.length();i++) {
//        	System.out.println(bitSet.get(i)?"1":"0");
//        }
        
        //System.out.println(to01(bitSet));
        
        
////        data[3] = (byte) (i & 0xFF);
////        data[2] = (byte) ((i >> 8) & 0xFF);
////        data[1] = (byte) ((i >> 16) & 0xFF);
////        data[0] = (byte) ((i >> 24) & 0xFF);
//        
//        // 0xFF = 1111 1111
//        // 0xF =  1111

        
    }

    public static void teste2(){
    	boolean bo1 = true, bo2 = true, bo3 = true, bo4 = true,
    			bo5 = true, bo6 = true, bo7 = true, bo8 = true;
    	
    	// 0xF = 1111
    	byte b1 = (byte)((bo1?1:0) & 0xF);
    	byte b2 = (byte)((bo2?1:0) & 0xF);
    	byte b3 = (byte)((bo3?1:0) & 0xF);
    	byte b4 = (byte)((bo4?1:0) & 0xF);
    	byte b5 = (byte)((bo5?1:0) & 0xF);
    	byte b6 = (byte)((bo6?1:0) & 0xF);
    	byte b7 = (byte)((bo7?1:0) & 0xF);
    	byte b8 = (byte)((bo8?1:0) & 0xF);
    	
    	System.out.println(BinaryUtil.toStr(b1));
    	System.out.println(BinaryUtil.toStr(b2));
    	System.out.println(BinaryUtil.toStr(b3));
    	System.out.println(BinaryUtil.toStr(b4));
    	System.out.println(BinaryUtil.toStr(b5));
    	System.out.println(BinaryUtil.toStr(b6));
    	System.out.println(BinaryUtil.toStr(b7));
    	System.out.println(BinaryUtil.toStr(b8));
    	sep();
    	
    	// bigendian = false
    	boolean bigendian = false;
    	if (bigendian) {
    		b2 <<= 1;
        	b3 <<= 2;
        	b4 <<= 3;
        	b5 <<= 4;
        	b6 <<= 5;
        	b7 <<= 6;
        	b8 <<= 7;
        	
        	b1 &= 0x1;	// 0000 0001
        	b2 &= 0x2;	// 0000 0010
        	b3 &= 0x4;	// 0000 0100
        	b4 &= 0x8;	// 0000 1000
        	b5 &= 0x10; // 0001 0000
        	b6 &= 0x20; // 0010 0000
        	b7 &= 0x40;	// 0100 0000
        	b8 &= 0x80;	// 1000 0000
    	} else {
    		b1 <<= 7;
    		b2 <<= 6;
        	b3 <<= 5;
        	b4 <<= 4;
        	b5 <<= 3;
        	b6 <<= 2;
        	b7 <<= 1;
        	
        	// ao contrário - littleendian
        	b1 &= 0x80;	// 1000 0000
        	b2 &= 0x40;	// 0100 0000
        	b3 &= 0x20; // 0010 0000
        	b4 &= 0x10; // 0001 0000
        	b5 &= 0x8;	// 0000 1000
        	b6 &= 0x4;	// 0000 0100
        	b7 &= 0x2;	// 0000 0010
        	b8 &= 0x1;	// 0000 0001
    	}
    	
    	byte bF = (byte) (b1 | b2 | b3 | b4 | b5 | b6 | b7 | b8);
    	
    	System.out.println(BinaryUtil.toStr(b1));
    	System.out.println(BinaryUtil.toStr(b2));
    	System.out.println(BinaryUtil.toStr(b3));
    	System.out.println(BinaryUtil.toStr(b4));
    	System.out.println(BinaryUtil.toStr(b5));
    	System.out.println(BinaryUtil.toStr(b6));
    	System.out.println(BinaryUtil.toStr(b7));
    	System.out.println(BinaryUtil.toStr(b8));
    	sep();
    	
    	System.out.println(BinaryUtil.toStr(bF));
    }
    
    private static void teste3() {
    	byte res = BinaryUtil.toByte(new boolean[] {true, true,true, true,true, true,true, true}, false);
    	System.out.println("res: " + BinaryUtil.toStr(res));
    	sep();
    	res = BinaryUtil.toByte(new int[] {1,1,1,1,1,1,1,1}, false);
    	System.out.println("res: " + BinaryUtil.toStr(res));
    	
    	sep();
    	
        MyBitSet mbit = new MyBitSet(3);
        mbit.set(true, 0);
        mbit.set(false, 1);
        mbit.set(true, 2);

        mbit.append(new boolean[] {false, true, false, false, true, true});
//        
//        
//        System.out.println(mbit.toString());
//        
//        sep();
//        
//        int[] frommbit = mbit.toInt();
//        for(int i =0;i<frommbit.length;i++) {System.out.print(frommbit[i]+"");} System.out.println();
//        sep();
//        
//        mbit.fromInt(new int[] {0,1,0,1,1,0,1,1});
//        System.out.println(mbit.toString());
//        sep();
        
        
        System.out.println(mbit.toString());
        sep();
        System.out.println("res: " + BinaryUtil.toStr( BinaryUtil.toByte(mbit) ));
        
        sep();
        byte[] rt = mbit.toByte();
		for (int i = 0; i < rt.length; i++) System.out.println(BinaryUtil.toStr(rt[i]));
    }
    
    private static void teste4() {
    	boolean[] array = new boolean[] { true, true, true, false, true  };

    	array = BinaryUtil.preencherBits(array);
    	
    	for (int i = 0; i < array.length; i++) {
    		System.out.print((array[i]?"1":"0"));
    	}
    	System.out.println();
    	sep();
    	
    	int[] ar2 = new int[] {1,1,0,1,1,0,0,1,0,1,1,0,1,1,0,1,1,1,0,1,1,0,0,1,0,1,1,0,1,1,0,1,1,0,1,0,1,1};
    	byte[] rt = BinaryUtil.toByteN(ar2);
    	rt = BinaryUtil.toByteN(array);
    	
    	System.out.println("rt.length: " + rt.length);
    	for(int i =0;i<rt.length;i++)
    		System.out.println(BinaryUtil.toStr(rt[i]));
    	
    }
    
	private static void teste5() {
	    // 0 - sincronizar: 3 bits (valor: 000)
	    // retorno: 1 bit (valor: 1 - codOk) + 4 bytes (float volume) = 32 bits
		System.out.println("-----------------ENTRADA-------------------");
		MyBitSet entrada = BinaryUtil.toMBitByte((byte)0, 3, false);
		entrada.append(75.26f);
		System.out.println(entrada.toString()); // 00011111000101000010110100101000010

		System.out.println("-----------------PARSE ENTRADA-------------------");
		MyBitSet cod = entrada.slice(0, 3);
        System.out.println("cod: " + cod.toString() + " = " + BinaryUtil.toInt(cod));
        MyBitSet volumeEntrada = entrada.slice(3, 32);
        System.out.println(volumeEntrada.toString() + ", valor: " + BinaryUtil.byteArrayToFloat(volumeEntrada.toByte(), false));
        
        System.out.println("----------------RETORNO------------------");
        // -- resposta
        // 1 + 32 bits
        MyBitSet retorno = BinaryUtil.toMBitByte((byte)1, 1, false);
        retorno.append(75.26f); // float - 4 bytes = 32 bits
        System.out.println(retorno.toString());
        
        System.out.println("-----------------PARSE RETORNO-------------------");
        boolean bit0 = retorno.get(0);
        System.out.println("bit0:\t\t" + (bit0?1:0));
        
        MyBitSet volumeRetornoParser = retorno.slice(1, 32);
        System.out.println(volumeRetornoParser.toString() + ", valor: " + BinaryUtil.byteArrayToFloat(volumeRetornoParser.toByte(), false));
    }
    
	private static void teste6() {
		// 1 - alterar o volume: 3 bits (valor: 100) + 4 bytes (float volume)
	    // retorno: 1 bit (valor: 1 - codOk) + 4 bytes (float volume) = 32 bits
		System.out.println("-----------------ENTRADA-------------------");
        MyBitSet entrada = BinaryUtil.toMBitByte((byte)1, 3, false);
        entrada.append(13.97f); // float - 4 bytes = 32 bits
        System.out.println(entrada.toString()); // 10011111000101000011111101010000010
		
        System.out.println("-----------------PARSE ENTRADA-------------------");
		MyBitSet cod = entrada.slice(0, 3);
        System.out.println("cod: " + cod.toString() + " = " + BinaryUtil.toInt(cod, true));
        MyBitSet volumeEntrada = entrada.slice(3, 32);
        System.out.println(volumeEntrada.toString() + ", valor: " + BinaryUtil.byteArrayToFloat(volumeEntrada.toByte(), false));

        System.out.println("----------------RETORNO------------------");
        // retorno: 1 bit (valor: 1 - codOk) + 4 bytes (float volume) = 32 bits
        MyBitSet retorno = BinaryUtil.toMBitByte((byte)1, 1, false);
        retorno.append(93.27f); // float - 4 bytes = 32 bits
        System.out.println(retorno.toString());	// 110111100010100010101110101000010
        
        System.out.println("-----------------PARSE RETORNO-------------------");
        boolean bit0 = retorno.get(0);
        System.out.println("bit0:\t\t" + (bit0?1:0));
        
        MyBitSet volumeRetornoParser = retorno.slice(1, 32);
        System.out.println(volumeRetornoParser.toString() + ", valor: " + BinaryUtil.byteArrayToFloat(volumeRetornoParser.toByte(), false));
	}

	private static void teste7() {
		System.out.println("-----------------ENTRADA-------------------");
		MyBitSet entrada = BinaryUtil.toMBitByte((byte)2, 3, false);
		System.out.println(entrada.toString()); // 010

		System.out.println("-----------------PARSE ENTRADA-------------------");
		MyBitSet cod = entrada.slice(0, 3);
		System.out.println("cod: " + cod.toString() + " = " + BinaryUtil.toInt(cod, true)); // cod: 010 = 2

		System.out.println("----------------RETORNO------------------");
		// retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)
		String rt = "Minha string C# $% é uma boa ? retorno de função dsf";
		rt = "Lorem Ipsum é simplesmente uma simulação de texto da indústria tipográfica e de impressos, e vem sendo utilizado desde o século XVI, quando um impressor desconhecido pegou uma bandeja de tipos e os embaralhou para fazer um livro de modelos de tipos. Lorem Ipsum sobreviveu não só a cinco séculos, como também ao salto para a editoração eletrônica, permanecendo essencialmente inalterado. Se popularizou na década de 60, quando a Letraset lançou decalques contendo passagens de Lorem Ipsum, e mais recentemente quando passou a ser integrado a softwares de editoração eletrônica como Aldus PageMaker.  Porque nós o usamos? É um fato conhecido de todos que um leitor se distrairá com o conteúdo de texto legível de uma página quando estiver examinando sua diagramação. A vantagem de usar Lorem Ipsum é que ele tem uma distribuição normal de letras, ao contrário de \"Conteúdo aqui, conteúdo aqui\", fazendo com que ele tenha uma aparência similar a de um texto legível. Muitos softwares de publicação e editores de páginas na internet agora usam Lorem Ipsum como texto-modelo padrão, e uma rápida busca por 'lorem ipsum' mostra vários websites ainda em sua fase de construção. Várias versões novas surgiram ao longo dos anos, eventualmente por acidente, e às vezes de propósito (injetando humor, e coisas do gênero).   De onde ele vem? Ao contrário do que se acredita, Lorem Ipsum não é simplesmente um texto randômico. Com mais de 2000 anos, suas raízes podem ser encontradas em uma obra de literatura latina clássica datada de 45 AC. Richard McClintock, um professor de latim do Hampden-Sydney College na Virginia, pesquisou uma das mais obscuras palavras em latim, consectetur, oriunda de uma passagem de Lorem Ipsum, e, procurando por entre citações da palavra na literatura clássica, descobriu a sua indubitável origem. Lorem Ipsum vem das seções 1.10.32 e 1.10.33 do \"de Finibus Bonorum et Malorum\" (Os Extremos do Bem e do Mal), de Cícero, escrito em 45 AC. Este livro é um tratado de teoria da ética muito popular na época da Renascença. A primeira linha de Lorem Ipsum, \"Lorem Ipsum dolor sit amet...\" vem de uma linha na seção 1.10.32.  O trecho padrão original de Lorem Ipsum, usado desde o século XVI, está reproduzido abaixo para os interessados. Seções 1.10.32 e 1.10.33 de \"de Finibus Bonorum et Malorum\" de Cicero também foram reproduzidas abaixo em sua forma exata original, acompanhada das versões para o inglês da tradução feita por H. Rackham em 1914.";
		rt = rt.substring(0, 300);

		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		// 1000101011001001100101111011001001110101001101011011000000100100100100000111011001110101011101011011000000100110000111001010100000100110011101001011010110110000011100011011010100110110011101011011010100110011101100010111010100110000001001010111010110110100001100000010011001110100101101011011010101110001101101000011011000011111001011100001111000101111101100000010000100110101001100000010000101110101001100001111000101110111101100000010000100110100001100000010010010110011101100010011011000011010111011100111000101110010011101001011010000110000001000010111010010110000011101111011011100110010011101100001110000101011001101001011011000110100001100000010010100110000001000010011010100110000001001001011010110110000011100100111010100110110011101100111011110110110011100011010000000100101001100000010001101110101001101011011000000100110011101010011001110110001001101111011000000100101011100010111010010110001101101001011001011110100001100010011011110110000001000010011010100110110011100010011010100110000001001111011000000100110011101100001110010101110001101010111000110110111101100000010000011010011010101001001000110100000001001000111010101110100001100111011000100110111101100000010010101110101101100000010010010110101101100000111001001110101001101100111011001110111101100100111000000100001001101010011011001110110001101111011001110110000101101010011011000110100101100010011011110110000001000000111010100110111001101111011010101110000001001010111010110110100001100000010001000110100001100111011000100110101001100101011010000110000001000010011010100110000001000010111010010110000011101111011011001110000001001010011000000100111101101100111000000100101001101011011001000110100001100100111010000110001101100001011011110110101011100000010000001110100001100100111010000110000001000110011010000110010111101010011001001110000001001010111010110110000001000011011010010110011011100100111011110110000001000010011010100110000001001011011011110110001001101010011000110110111101101100111000000100001001101010011000000100001011101001011000001110111101101100111001110100000001000011001011110110010011101010011010110110000001001001001000001110110011101010111010110110000001001100111011110110010001100100111010100110011011101001011001101110101001101010111000000100011101101100001111000101111101100000010011001110110000111100110100000100100001100000010011000110100101100111011011000110111101100000010011001110110000111001010111000110101011100011011011110110110011100011010000000100110001101111011010110110

		System.out.println("-----------------PARSE RETORNO-------------------");
        boolean bit0 = retorno.get(0);
        System.out.println("bit0:\t\t" + (bit0?1:0));
        
        MyBitSet sizeStringMB = retorno.slice(1, 12);
        int sizeString = BinaryUtil.toInt(sizeStringMB, true);
        System.out.println(sizeStringMB.toString() + " = " + sizeString);
		
        MyBitSet stringMB = retorno.slice(13, sizeString); // System.out.println(stringMB.toString());
        String mensagem = new String(stringMB.toByte(), Charset.forName("UTF-8"));
        System.out.println(mensagem);
	}
	
	private static void teste8() {
		System.out.println("-----------------ENTRADA-------------------");
		MyBitSet entrada = BinaryUtil.toMBitByte((byte)3, 3, false);
		entrada.append(BinaryUtil.toMBit(1920, 13, false));
		entrada.append(BinaryUtil.toMBit(1080, 13, false));
		entrada.append(BinaryUtil.toMBit(1370, 13, false));
		entrada.append(BinaryUtil.toMBit(425, 13, false));
		System.out.println(entrada.toString()); // 1100000000111100000111000010001011010101001001010110000

		System.out.println("-----------------PARSE ENTRADA-------------------");
		MyBitSet cod = entrada.slice(0, 3);
		System.out.println("cod: " + cod.toString() + " = " + BinaryUtil.toInt(cod, true)); // cod: 110 = 3
		
		MyBitSet wc = entrada.slice(3, 13);
		MyBitSet hc = entrada.slice(13+3, 13);
		MyBitSet xc = entrada.slice(13 + 13 + 3, 13);
		MyBitSet yc = entrada.slice(13 + 13 + 13 + 3, 13);
		System.out.println("wc: " + wc.toString() + " = " + BinaryUtil.toInt(wc, true));
		System.out.println("hc: " + hc.toString() + " = " + BinaryUtil.toInt(hc, true));
		System.out.println("xc: " + xc.toString() + " = " + BinaryUtil.toInt(xc, true));
		System.out.println("yc: " + wc.toString() + " = " + BinaryUtil.toInt(yc, true));
		
		System.out.println("----------------RETORNO------------------");
		// retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)
		String rt = "Recebido";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		
		System.out.println("-----------------PARSE RETORNO-------------------");
        boolean bit0 = retorno.get(0);
        System.out.println("bit0:\t\t" + (bit0?1:0));
        
        MyBitSet sizeStringMB = retorno.slice(1, 12);
        int sizeString = BinaryUtil.toInt(sizeStringMB, true);
        System.out.println(sizeStringMB.toString() + " = " + sizeString);
		
        MyBitSet stringMB = retorno.slice(13, sizeString); // System.out.println(stringMB.toString());
        String mensagem = new String(stringMB.toByte(), Charset.forName("UTF-8"));
        System.out.println(mensagem);
	}
	
	private static void teste9() {
		// 4 - click mouse: 3 bits (valor: 4)
	    // retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)
		System.out.println("-----------------ENTRADA-------------------");
		MyBitSet entrada = BinaryUtil.toMBitByte((byte)4, 3, false);
		System.out.println(entrada.toString());

		System.out.println("-----------------PARSE ENTRADA-------------------");
		MyBitSet cod = entrada.slice(0, 3);
		System.out.println("cod: " + cod.toString() + " = " + BinaryUtil.toInt(cod, true)); // cod: 001 = 4
		
		System.out.println("----------------RETORNO------------------");
		// retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)
		String rt = "Click Recebido";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		
		System.out.println("-----------------PARSE RETORNO-------------------");
        boolean bit0 = retorno.get(0);
        System.out.println("bit0:\t\t" + (bit0?1:0));
        
        MyBitSet sizeStringMB = retorno.slice(1, 12);
        int sizeString = BinaryUtil.toInt(sizeStringMB, true);
        System.out.println(sizeStringMB.toString() + " = " + sizeString);
		
        MyBitSet stringMB = retorno.slice(13, sizeString); // System.out.println(stringMB.toString());
        String mensagem = new String(stringMB.toByte(), Charset.forName("UTF-8"));
        System.out.println(mensagem);
	}
	
	private static void teste10() {
		// 5 - Lock Screen: 3 bits (valor: 5)
	    // retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string
		System.out.println("-----------------ENTRADA-------------------");
		MyBitSet entrada = BinaryUtil.toMBitByte((byte)5, 3, false);
		System.out.println(entrada.toString());

		System.out.println("-----------------PARSE ENTRADA-------------------");
		MyBitSet cod = entrada.slice(0, 3);
		System.out.println("cod: " + cod.toString() + " = " + BinaryUtil.toInt(cod, true)); // cod: 101 = 5
		
		System.out.println("----------------RETORNO------------------");
		// retorno: 1 bit (valor: 1) + 12 bits (size string) + n bits (string)
		String rt = "Tela Bloqueada";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		
		System.out.println("-----------------PARSE RETORNO-------------------");
        boolean bit0 = retorno.get(0);
        System.out.println("bit0:\t\t" + (bit0?1:0));
        
        MyBitSet sizeStringMB = retorno.slice(1, 12);
        int sizeString = BinaryUtil.toInt(sizeStringMB, true);
        System.out.println(sizeStringMB.toString() + " = " + sizeString);
		
        MyBitSet stringMB = retorno.slice(13, sizeString); // System.out.println(stringMB.toString());
        String mensagem = new String(stringMB.toByte(), Charset.forName("UTF-8"));
        System.out.println(mensagem);
	}
	

	private static void sep() {System.out.println("-------------------------");}
}
