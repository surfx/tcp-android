package br.main.util.tcp;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.nio.charset.Charset;

import br.main.RespostaServidor;
import br.main.util.BinaryUtil;
import br.main.util.MyBitSet;

public class TCPUtil {

	private static final int PACKAGESIZE = 10;
	
	public static void sendPackage(MyBitSet data, DataOutputStream stream) throws IOException {
		// headers
		MyBitSet aux = BinaryUtil.toMBitByte((byte)data.size(), PACKAGESIZE, false);
		aux.append(data);
		byte[] buffer = aux.toByte();
		stream.write(buffer);
		System.out.println(String.format("[sendPackage] data: %s [%s]", aux.toString(), data.size()));
	}

	public static MyBitSet receivePackage(DataInputStream stream) throws IOException {
		byte[] buffer = new byte[200];
		int i = stream.read(buffer, 0, buffer.length);
		
		MyBitSet allbits = BinaryUtil.getBitsInteresse(buffer, 200);
		int tamanho = BinaryUtil.toInt(allbits.slice(0, PACKAGESIZE), true);

		MyBitSet data = allbits.slice(PACKAGESIZE, tamanho);
		
		System.out.println(String.format("[receivePackage] tamanho: %s", tamanho));
		System.out.println(String.format("[receivePackage] data: %s [%s]", data.toString(), data.size()));

		return data;
	}
	
	public static RespostaServidor parserMensagemServer(MyBitSet input) {
		RespostaServidor rt = new RespostaServidor();
		
        boolean bit0 = input.get(0);
        //System.out.println("bit0:\t\t" + (bit0?1:0));
        
        MyBitSet sizeStringMB = input.slice(1, 12);
        int sizeString = BinaryUtil.toInt(sizeStringMB, true);
        //System.out.println(sizeStringMB.toString() + " = " + sizeString);
		
        MyBitSet stringMB = input.slice(13, sizeString); // System.out.println(stringMB.toString());
        String mensagem = new String(stringMB.toByte(), Charset.forName("UTF-8"));
        //System.out.println(mensagem);
        
        rt.setSucesso(bit0);
        rt.setMensagem(mensagem);
        
        return rt;
	}
	
}