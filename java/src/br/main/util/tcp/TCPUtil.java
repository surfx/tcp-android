package br.main.util.tcp;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.nio.charset.Charset;

import br.main.RespostaServidor;
import br.main.util.BinaryUtil;
import br.main.util.MyBitSet;

public class TCPUtil {

	public static void send(MyBitSet pacote, DataOutputStream dos) throws IOException {
		byte[] toSend = pacote.toByte();
		
		dos.writeByte((byte)pacote.size());
		dos.writeInt(toSend.length);
		dos.write(toSend);
	}

	public static MyBitSet receive(DataInputStream dis) throws IOException {
		byte pacoteSize = dis.readByte();
		int size = dis.readInt();
		//System.out.println("pacoteSize: " + pacoteSize +  ", size: " + size);
		byte[] bytes = dis.readNBytes(size);
		//System.out.println("bytes.length: " + bytes.length);

		int pos = 0;
		boolean bigendian = false;
		boolean[] bitsInteresse = new boolean[pacoteSize];
		for(int i = 0; i < bytes.length; i++) {
			if (pos >= pacoteSize) {break;}
			for(int j = 0; j < 8; j++) {
				bitsInteresse[pos++] = BinaryUtil.getBit( bytes[i], bigendian ? 7 - j : j ) == 1;
				if (pos >= pacoteSize) {break;}
			}
		}
		
		//for(int i = 0; i < pacoteSize; i++) { System.out.print(bitsInteresse[i]?1:0); } System.out.println();

		MyBitSet rt = new MyBitSet(pacoteSize);
		for(int i = 0; i < pacoteSize; i++) {
			rt.set(bitsInteresse[i], i);
		}
		//System.out.println("[s] receive: " + rt.toString());
		return rt;
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