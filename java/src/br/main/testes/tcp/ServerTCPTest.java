package br.main.testes.tcp;

import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;

import br.main.tcpip.interfaces.ITratarRequisicaoBin;
import br.main.tcpip.tratarMensagens.TratarRequisicoesBin;
import br.main.util.MyBitSet;
import br.main.util.tcp.TCPUtil;

/**
 * https://www.codejava.net/java-se/networking/java-socket-server-examples-tcp-ip
 */
public class ServerTCPTest {

	private boolean executar = true;
	
	private ITratarRequisicaoBin tratarRequests;
	
	public ServerTCPTest() {
		tratarRequests = new TratarRequisicoesBin();
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
							
//							String resposta = "0Erro";
//							if (iTratarRequisicao!=null) {
//								resposta = iTratarRequisicao.tratar(mensagemCliente);
//							}
							
							// -- send
							OutputStream output = socket.getOutputStream();
							PrintWriter writer = new PrintWriter(output, true);
							writer.println("return form server");

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

	// ---
	public void startBin(final int port) {
		startBin(port, -1);
	}
	public void startBin(final int port, final int timeout) {

		new Thread(() -> {
			try (ServerSocket serverSocket = new ServerSocket(port)) {
				if (timeout > 0) {serverSocket.setSoTimeout(timeout);}
				System.out.println("[s] Server is listening on port " + port);

				while (executar) {
					if (!executar) {
						System.out.println("bye");
						serverSocket.close();
						break;
					}

					Socket socket = serverSocket.accept();
					//System.out.println("[s] New client connected");

					new Thread(() -> {
						try {
							// -- receive
							InputStream input = socket.getInputStream();
							DataInputStream dis = new DataInputStream(input);
							
							MyBitSet recebido = TCPUtil.receive(dis);
							System.out.println("[s] recebido: " + recebido.toString());
							
							MyBitSet resposta = tratarRequests.tratar(recebido);
							
							// -- send
							OutputStream output = socket.getOutputStream();
							DataOutputStream dos = new DataOutputStream(output);
							TCPUtil.send(resposta, dos);

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
	
	
}