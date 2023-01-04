package br.main.tcpip.tratarMensagens;

import br.main.tcpip.interfaces.ITratarRequisicaoBin;
import br.main.util.BinaryUtil;
import br.main.util.MyBitSet;

public class TratarRequisicoesBin implements ITratarRequisicaoBin {

	@Override
	public MyBitSet tratar(MyBitSet mensagem) {
		if (mensagem == null || mensagem.size() <= 0) { return msgErro(); }
		
		MyBitSet cod = mensagem.slice(0, 3);
		System.out.println("[tratar] cod: " + cod.toString() + " = " + BinaryUtil.toInt(cod, true));
		
		switch(BinaryUtil.toInt(cod, true)) {
			case 0: return sinchronizar();
			case 1: return alterarVolume(mensagem);
			case 2: return desligarPC();
			case 3: return mouseMove(mensagem);
			case 4: return clickMouse();
			case 5: return lockScreen();
		}
		
		return msgErro();
	}

	private MyBitSet msgErro() {
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x0, 1, false);
		retorno.append(BinaryUtil.toMBit("Erro"));
		return retorno;
	}
	
	private MyBitSet sinchronizar() {
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)1, 1, false);
        retorno.append(75.26f); // TODO: get volume
        return retorno;
	}
	
	private MyBitSet alterarVolume(MyBitSet entrada) {
        MyBitSet volumeEntrada = entrada.slice(3, 32);
        System.out.println(volumeEntrada.toString() + ", valor: " + BinaryUtil.byteArrayToFloat(volumeEntrada.toByte(), false));
        
        MyBitSet retorno = BinaryUtil.toMBitByte((byte)1, 1, false);
        retorno.append(93.27f); // float - 4 bytes = 32 bits
        System.out.println(retorno.toString());	// 110111100010100010101110101000010
        
        return retorno;
	}
	
	private MyBitSet desligarPC() {
		String rt = "Desligar";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		return retorno;
	}
	
	private MyBitSet mouseMove(MyBitSet entrada) {
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
		
		// --- retorno
		String rt = "Recebido";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		return retorno;
	}
	
	private MyBitSet clickMouse() {
		String rt = "Click Recebido";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		return retorno;
	}
	
	private MyBitSet lockScreen() {
		String rt = "Tela Bloqueada";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		return retorno;
	}
	
	
	
}