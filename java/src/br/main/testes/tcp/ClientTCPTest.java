package br.main.testes.tcp;

import java.io.BufferedReader;
import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.function.Consumer;

import br.main.RespostaServidor;
import br.main.util.MyBitSet;
import br.main.util.tcp.TCPUtil;

/**
 * https://www.codejava.net/java-se/networking/java-socket-server-examples-tcp-ip
 */
public class ClientTCPTest {

	private int clientId;
	public ClientTCPTest(int clientId) { this.clientId = clientId; }
	
	public void start(int port) {
		start("localhost", port);
	}
	
	public void start(String hostname, int port) {
		new Thread(() -> {
			try (Socket socket = new Socket(hostname, port)) {
	            // -- send
	            OutputStream out = socket.getOutputStream();
	            PrintWriter writer = new PrintWriter(out, true);
	            writer.println("["+clientId+"] mensagem do cliente para o servidor");

				// -- receive
				InputStream input = socket.getInputStream();
	            BufferedReader reader = new BufferedReader(new InputStreamReader(input));
	 
	            String mensagemServidor = reader.readLine();
	            System.out.println("[c] " + mensagemServidor);
	            
				socket.close();
			} catch (UnknownHostException ex) {
				System.out.println("[c] Server not found: " + ex.getMessage());
			} catch (IOException ex) {
				System.out.println("[c] I/O error: " + ex.getMessage());
			}
		}).start();
	}
	
	// ---
	public void startBin(MyBitSet pacote, int port, Consumer<MyBitSet> tratarRespostaServer) {
		startBin(pacote, "localhost", port, tratarRespostaServer);
	}
	public void startBin(MyBitSet pacote, String hostname, int port, Consumer<MyBitSet> tratarRespostaServer) {
		new Thread(() -> {
			try (Socket socket = new Socket(hostname, port)) {
	            // -- send
				OutputStream output = socket.getOutputStream();
				DataOutputStream dos = new DataOutputStream(output);
				
				// -- send
				TCPUtil.send(pacote, dos);

				// -- receive
				InputStream input = socket.getInputStream();
				DataInputStream dis = new DataInputStream(input);
				
				MyBitSet resposta = TCPUtil.receive(dis);
				tratarRespostaServer.accept(resposta);
				System.out.println("[c] resposta: " + resposta.toString());

				socket.close();
			} catch (UnknownHostException ex) {
				System.out.println("[c] Server not found: " + ex.getMessage());
			} catch (IOException ex) {
				System.out.println("[c] I/O error: " + ex.getMessage());
			}
		}).start();
	}
	
}