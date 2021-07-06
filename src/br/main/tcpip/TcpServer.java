package br.main.tcpip;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;

import br.main.tcpip.interfaces.ITratarRequisicao;

//https://www.codejava.net/java-se/networking/java-socket-server-examples-tcp-ip
public class TcpServer {

	private boolean executar = true;

	private ITratarRequisicao iTratarRequisicao;
	
	public TcpServer(ITratarRequisicao iTratarRequisicao) {
		this.iTratarRequisicao = iTratarRequisicao;
	}
	
	public void start(final int port) {
		this.start(port, -1);
	}
	
	public void start(final int port, final int timeout) {

		new Thread(() -> {
			try (ServerSocket serverSocket = new ServerSocket(port)) {
				if (timeout > 0) {serverSocket.setSoTimeout(timeout);}
				System.out.println("[s] Server is listening on port " + port);

				while (executar) {
					if (!executar) {
						break;
					}

					Socket socket = serverSocket.accept();
					//System.out.println("[s] New client connected");

					new Thread(() -> {
						try {
							// -- receive
							InputStream input = socket.getInputStream();
							BufferedReader reader = new BufferedReader(new InputStreamReader(input));
							String mensagemCliente = reader.readLine();
							System.out.println("[s] " + mensagemCliente);
							
							String resposta = "0Erro";
							if (iTratarRequisicao!=null) {
								resposta = iTratarRequisicao.tratar(mensagemCliente);
							}
							
							// -- send
							OutputStream output = socket.getOutputStream();
							PrintWriter writer = new PrintWriter(output, true);
							writer.println(resposta);

							socket.close();
						} catch (IOException e) {
							System.out.println("[s] Server exception: " + e.getMessage());
						}
					}).start();

				}

				serverSocket.close();

			} catch (IOException ex) {
				System.out.println("[s] Server exception: " + ex.getMessage());
				ex.printStackTrace();
			}
		}).start();

	}

	public void stop() {
		executar = false;
	}

}