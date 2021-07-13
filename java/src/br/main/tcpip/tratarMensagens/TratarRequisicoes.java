package br.main.tcpip.tratarMensagens;

import java.awt.Point;

import br.main.controles.Audio;
import br.main.controles.MouseControl;
import br.main.controles.ShutdownLinux;
import br.main.tcpip.interfaces.ITratarRequisicao;

public class TratarRequisicoes implements ITratarRequisicao {
	
	private MouseControl mc = new MouseControl();
	
	private final String msgErro = "0Erro";
	private final String codOk = "1";
	
	@Override
	public String tratar(String mensagem) {
		String rt = msgErro;
		if (mensagem == null || mensagem.isEmpty() || mensagem.length() <= 0) { return rt; }
		
		String tipo = mensagem.substring(0, 1);
		if (tipo.equals("0")) {
			return getSincronizar();
		} else if (tipo.equals("1")) {
			return getAlterarVolume(mensagem);
		} else if (tipo.equals("2")) {
			return getDesligar();
		} else if (tipo.equals("3")) {
			return getMousePos(mensagem);
		} else if (tipo.equals("4")) {
			return getClickMouse();
		}

		return rt;
	}

	// 0 - sincronizar
	private String getSincronizar() {
		return codOk + Audio.getMasterOutputVolume();
	}
	
	// 1 - alterar o volume
	private String getAlterarVolume(String mensagem) {
		try {
			String volumeStr = mensagem.substring(1);
			float volumeFloat = Float.parseFloat(volumeStr);
			Audio.setMasterOutputVolume(volumeFloat);
			return codOk + Audio.getMasterOutputVolume();
		} catch (Exception e) {
			return msgErro;
		}
	}
	
	// 2 - sincronizar
	private String getDesligar() {
		ShutdownLinux.shutDownLinux();
		return codOk + "Desligando";
	}
	
	//3 - mouse
	private String getMousePos(String mensagem) {
		//31920x1080,1370x425
		mensagem = mensagem.substring(1);
		
		//1920x1080,1370x425
		int posX = mensagem.indexOf("x");
		int posLX = mensagem.lastIndexOf("x");
		int posVirgula = mensagem.indexOf(",");
		int wcel = Integer.parseInt(mensagem.substring(0, posX));
		int hcel = Integer.parseInt(mensagem.substring(posX + 1, posVirgula));
		int xcel = Integer.parseInt(mensagem.substring(posVirgula + 1, posLX)) + 50; // ajuste, android faz um -50 ?
		int ycel = Integer.parseInt(mensagem.substring(posLX + 1)) + 130; // ajuste, android faz um -130 ?

		Point screenSize = MouseControl.getScreenSize();
		int wpc = screenSize.x;
		int hpc = screenSize.y;
		
		int xPc = conversorXY(xcel, wcel, wpc);
		int yPc = conversorXY(ycel, hcel, hpc);
		mc.moveMouse(xPc, yPc);
		
		return codOk + "Recebido";
	}
	
	private int conversorXY(int xyCel, int whCel, int whPc) {
		return (xyCel * whPc)/whCel;
	}
	
	// 4 - click mouse
	private String getClickMouse() {
		mc.clickMouse();
		return codOk + "click recebido";
	}
	
}