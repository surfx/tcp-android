package br.com.controltcpandroid.tcpip.binary;

import android.os.Build;

import java.io.DataInputStream;
import java.io.DataOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.function.Consumer;

import br.com.controltcpandroid.util.MyBitSet;
import br.com.controltcpandroid.util.TCPUtil;

public class TCPClientBinary {

    private String hostname;
    private int port;

    public TCPClientBinary(String hostname, int port){
        this.hostname = hostname;
        this.port = port;
    }

    public void send(MyBitSet pacote, Consumer<MyBitSet> tratarRespostaServer) {
        new Thread(() -> {
            try (Socket socket = new Socket(hostname, port)) {
                // -- send
                OutputStream output = socket.getOutputStream();
                DataOutputStream dos = new DataOutputStream(output);

                // -- send
                TCPUtil.sendPackage(pacote, dos);

                // -- receive
                InputStream input = socket.getInputStream();
                DataInputStream dis = new DataInputStream(input);

                MyBitSet resposta = TCPUtil.receivePackage(dis);

                if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.N) {
                    tratarRespostaServer.accept(resposta);
                }
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