package br.main.tcpip.interfaces;

import br.main.util.MyBitSet;

@FunctionalInterface
public interface ITratarRequisicaoBin {

	public MyBitSet tratar(MyBitSet mensagem);
	
}