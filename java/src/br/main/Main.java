package br.main;

import java.net.InetAddress;
import java.net.UnknownHostException;

import br.main.tcpip.TcpServer;
import br.main.tcpip.tratarMensagens.TratarRequisicoes;
import br.main.testes.TestesBinario;

public class Main {

	public static void main(String[] args) throws UnknownHostException {
		//startTCPServer();
		
		TestesBinario.testeMensagensBin();
	}

	private static void startTCPServer() throws UnknownHostException {
		final int port = 9876;
		
		sp();
		InetAddress inetAddress = InetAddress.getLocalHost();
        System.out.println("IP Address:- " + inetAddress.getHostAddress());
        System.out.println("Host Name:- " + inetAddress.getHostName());
        System.out.println("Porta: " + port);
        sp();

		//------- servidor
		TcpServer server = new TcpServer(new TratarRequisicoes());
		server.start(port);
	}
	
	private static void sp() { System.out.println("----------------------------"); }
	
}