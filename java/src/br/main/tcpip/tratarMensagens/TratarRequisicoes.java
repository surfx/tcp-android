package br.main.tcpip.tratarMensagens;

import br.main.controles.Audio;
import br.main.controles.ShutdownLinux;
import br.main.tcpip.interfaces.ITratarRequisicao;

public class TratarRequisicoes implements ITratarRequisicao {
	
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
	
}