package br.main.tcpip.tratarMensagens;

import java.awt.Point;

import br.main.controles.Audio;
import br.main.controles.LockUnLockScreen;
import br.main.controles.MouseControl;
import br.main.controles.Shutdown;
import br.main.tcpip.interfaces.ITratarRequisicaoBin;
import br.main.util.BinaryUtil;
import br.main.util.MyBitSet;

public class TratarRequisicoesBin implements ITratarRequisicaoBin {

	private MouseControl mc = new MouseControl();
	
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
			case 6: return unLockScreen();
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
		try {
			float volume = Audio.getMasterOutputVolume() * 100f; // Linux - Audio
			System.out.println(String.format("Audio.getMasterOutputVolume(): %s", volume) );
			retorno.append(volume);	
		} catch (Exception e) {
			retorno.append(0f);
		}
        return retorno;
	}
	
	private MyBitSet alterarVolume(MyBitSet entrada) {
        MyBitSet volumeEntrada = entrada.slice(3, 32);
        System.out.println(volumeEntrada.toString() + ", valor: " + BinaryUtil.byteArrayToFloat(volumeEntrada.toByte(), false));
        
        float volume = BinaryUtil.byteArrayToFloat(volumeEntrada.toByte(), false);
        
        try {
        	Audio.setMasterOutputVolume(volume / 100.0f); // Linux - Audio
		} catch (Exception e) {
		}
        
        
        MyBitSet retorno = BinaryUtil.toMBitByte((byte)1, 1, false);
        
        try {
        	retorno.append(Audio.getMasterOutputVolume()); // float - 4 bytes = 32 bits	
		} catch (Exception e) {
			retorno.append(0f);
		}
        
        System.out.println(retorno.toString());	// 110111100010100010101110101000010
        
        return retorno;
	}
	
	private MyBitSet desligarPC() {
		String rt = "Desligar";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		
		Shutdown.shutDown();
		
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
		
		int wcel = BinaryUtil.toInt(wc, true);
		int hcel = BinaryUtil.toInt(hc, true);
		int xcel = BinaryUtil.toInt(xc, true) + 50; // ajuste, android faz um -50 ?
		int ycel = BinaryUtil.toInt(yc, true) + 130; // ajuste, android faz um -130 ?

		Point screenSize = MouseControl.getScreenSize();
		int wpc = screenSize.x;
		int hpc = screenSize.y;
		
		int xPc = conversorXY(xcel, wcel, wpc);
		int yPc = conversorXY(ycel, hcel, hpc);

		//System.out.println("wpc: " + wpc + ", hpc: " + hpc + ", xPc: " + xPc + ", yPc: " + yPc);
		
		mc.moveMouse(xPc, yPc);
		
		// --- retorno
		String rt = "Recebido";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		return retorno;
	}
	
	private int conversorXY(int xyCel, int whCel, int whPc) {
		return (xyCel * whPc)/whCel;
	}
	
	private MyBitSet clickMouse() {
		String rt = "Click Recebido";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		
		mc.clickMouse();
		
		System.out.println("retorno: " + retorno.toString());
		return retorno;
	}
	
	private MyBitSet lockScreen() {
		String rt = "Tela Bloqueada";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		
		LockUnLockScreen.lockSreen();

		return retorno;
	}

	private MyBitSet unLockScreen() {
		String rt = "Tela DesBloqueada";
		MyBitSet retorno = BinaryUtil.toMBitByte((byte)0x1, 1, false);
		retorno.append(BinaryUtil.toMBit(rt));
		System.out.println("retorno: " + retorno.toString());
		
		LockUnLockScreen.unLockSreen();

		return retorno;
	}
	
}