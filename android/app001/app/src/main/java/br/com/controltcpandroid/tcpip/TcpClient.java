package br.com.controltcpandroid.tcpip;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.UnknownHostException;

/**
 * envia uma mensagem e morre, não mantém a conexão aberta
 */
public class TcpClient {

//    public void enviarMensagem(int port, String mensagem, ITratarRequisicao iTratarRequisicao) {
//        enviarMensagem("localhost", port, mensagem, iTratarRequisicao);
//    }

    public void enviarMensagem(String hostname, int port, String mensagem, ITratarRequisicao iTratarRequisicao) {
        new Thread(() -> {
            try (Socket socket = new Socket(hostname, port)) {
                // -- send
                OutputStream out = socket.getOutputStream();
                PrintWriter writer = new PrintWriter(out, true);
                writer.println(mensagem);

                // -- receive
                InputStream input = socket.getInputStream();
                BufferedReader reader = new BufferedReader(new InputStreamReader(input));
                String resposta = reader.readLine();
                System.out.println("[c] " + resposta);
                if (iTratarRequisicao!=null){  iTratarRequisicao.tratar(resposta);  }

                socket.close();
            } catch (UnknownHostException ex) {
                System.out.println("[c] Server not found: " + ex.getMessage());
            } catch (IOException ex) {
                System.out.println("[c] I/O error: " + ex.getMessage());
            }
        }).start();
    }

}
