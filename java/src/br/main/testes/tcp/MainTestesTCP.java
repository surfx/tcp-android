package br.main.testes.tcp;

import br.main.RespostaServidor;
import br.main.util.BinaryUtil;
import br.main.util.MyBitSet;
import br.main.util.tcp.TCPUtil;

public class MainTestesTCP {

	private final static int port = 7893;
	
	public static void main(String[] args) {

//		new ServerTCPTest().start(port);
//		try { Thread.sleep(100); } catch (InterruptedException e) { }
//		
//		new ClientTCPTest(47).start(port);

		//--------
		
		ServerTCPTest server = new ServerTCPTest();
		server.startBin(port);
		try { Thread.sleep(100); } catch (InterruptedException e) { }

		new ClientTCPTest(47).startBin(getMsgSinchronizar() , "localhost", port, mbit -> tratarMensagemSinchronizar(mbit));
		try { Thread.sleep(200); } catch (InterruptedException e) { }
		
		new ClientTCPTest(47).startBin(getMsgAlterarVolume() , "localhost", port, mbit -> tratarMensagemAlterarVolume(mbit));
		try { Thread.sleep(200); } catch (InterruptedException e) { }

		new ClientTCPTest(47).startBin(getMsgDesligarPC() , "localhost", port, mbit -> tratarMensagemDesligarPC(mbit));
		try { Thread.sleep(200); } catch (InterruptedException e) { }
		
		new ClientTCPTest(47).startBin(getMsgMouseMove() , "localhost", port, mbit -> tratarMensagemMouseMove(mbit));
		try { Thread.sleep(200); } catch (InterruptedException e) { }

		new ClientTCPTest(47).startBin(getMsgClickMouse() , "localhost", port, mbit -> tratarMensagemClickMouse(mbit));
		try { Thread.sleep(200); } catch (InterruptedException e) { }

		new ClientTCPTest(47).startBin(getMsgLockScreen() , "localhost", port, mbit -> tratarMensagemLockScreen(mbit));
		try { Thread.sleep(200); } catch (InterruptedException e) { }
		
		try { Thread.sleep(800); } catch (InterruptedException e) { }
		System.out.println("-- end");
		server.stop();
		System.exit(0);
	}
	
	// ---
	private static MyBitSet getMsgSinchronizar() {
		return BinaryUtil.toMBitByte((byte)0, 3, false);
	}
	private static void tratarMensagemSinchronizar(MyBitSet retorno) {
        boolean bit0 = retorno.get(0);
        System.out.println("bit0:\t\t" + (bit0?1:0));
        
        MyBitSet volumeRetornoParser = retorno.slice(1, 32);
        System.out.println(volumeRetornoParser.toString() + ", valor: " + BinaryUtil.byteArrayToFloat(volumeRetornoParser.toByte(), false));
	}
	
	// ---
	private static MyBitSet getMsgAlterarVolume() {
        MyBitSet entrada = BinaryUtil.toMBitByte((byte)1, 3, false);
        entrada.append(13.97f); // float - 4 bytes = 32 bits
        return entrada;
	}
	private static void tratarMensagemAlterarVolume(MyBitSet retorno) {
        boolean bit0 = retorno.get(0);
        System.out.println("bit0:\t\t" + (bit0?1:0));
        
        MyBitSet volumeRetornoParser = retorno.slice(1, 32);
        System.out.println(volumeRetornoParser.toString() + ", valor: " + BinaryUtil.byteArrayToFloat(volumeRetornoParser.toByte(), false));
	}

	// ---
	private static MyBitSet getMsgDesligarPC() {
		return BinaryUtil.toMBitByte((byte)2, 3, false);
	}
	private static void tratarMensagemDesligarPC(MyBitSet retorno) {
		RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
		System.out.println(msg.toString());
	}
	
	// ---
	private static MyBitSet getMsgMouseMove() {
		MyBitSet entrada = BinaryUtil.toMBitByte((byte)3, 3, false);
		entrada.append(BinaryUtil.toMBit(1920, 13, false));
		entrada.append(BinaryUtil.toMBit(1080, 13, false));
		entrada.append(BinaryUtil.toMBit(1370, 13, false));
		entrada.append(BinaryUtil.toMBit(425, 13, false));
		return entrada;
	}
	private static void tratarMensagemMouseMove(MyBitSet retorno) {
		RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
		System.out.println(msg.toString());
	}
	
	// ---
	private static MyBitSet getMsgClickMouse() {
		return BinaryUtil.toMBitByte((byte)4, 3, false);
	}
	private static void tratarMensagemClickMouse(MyBitSet retorno) {
		RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
		System.out.println(msg.toString());
	}
	
	// ---
	private static MyBitSet getMsgLockScreen() {
		return BinaryUtil.toMBitByte((byte)5, 3, false);
	}
	private static void tratarMensagemLockScreen(MyBitSet retorno) {
		RespostaServidor msg = TCPUtil.parserMensagemServer(retorno);
		System.out.println(msg.toString());
	}
	
}