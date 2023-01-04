package br.main;

public class RespostaServidor {

	private boolean sucesso;
	private String mensagem;
	
	public boolean isSucesso() {
		return sucesso;
	}
	public void setSucesso(boolean sucesso) {
		this.sucesso = sucesso;
	}
	public String getMensagem() {
		return mensagem;
	}
	public void setMensagem(String mensagem) {
		this.mensagem = mensagem;
	}

	@Override
	public String toString() {
		return "RespostaServidor [sucesso=" + sucesso + ", mensagem=" + mensagem + "]";
	}

}