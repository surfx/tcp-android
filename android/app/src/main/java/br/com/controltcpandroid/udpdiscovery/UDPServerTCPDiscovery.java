package br.com.controltcpandroid.udpdiscovery;

import android.content.Context;
import android.net.DhcpInfo;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.util.Log;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.nio.charset.StandardCharsets;

/**
 * Classe responsável por descobrir o servidor TCP via UDP Broadcast.
 * Otimizada para funcionar em Windows e Linux/Android.
 */
public class UDPServerTCPDiscovery {

    private static final String LOG_TAG = "UDPServerTCPDiscovery";
    private static final int DISCOVERY_PORT = 9875;
    private static final int TIMEOUT_MS = 2500;
    private static final int MAX_TRIES = 2;

    /**
     * Calcula o endereço de broadcast específico da sub-rede atual.
     * Necessário para sistemas baseados em Linux (Android) que ignoram o 255.255.255.255.
     */
    private static InetAddress getBroadcastAddress(Context context) throws IOException {
        WifiManager wifi = (WifiManager) context.getApplicationContext().getSystemService(Context.WIFI_SERVICE);
        if (wifi == null) return InetAddress.getByName("255.255.255.255");
        
        DhcpInfo dhcp = wifi.getDhcpInfo();
        if (dhcp == null || dhcp.ipAddress == 0) return InetAddress.getByName("255.255.255.255");

        // Calcula o broadcast usando o IP e a Máscara de rede do DHCP
        int broadcast = (dhcp.ipAddress & dhcp.netmask) | ~dhcp.netmask;
        byte[] quads = new byte[4];
        for (int k = 0; k < 4; k++) {
            quads[k] = (byte) ((broadcast >> (k * 8)) & 0xFF);
        }
        return InetAddress.getByAddress(quads);
    }

    public static InetSocketAddress discover(Context context) {
        WifiManager.MulticastLock lock = null;
        DatagramSocket socket = null;

        try {
            // 1. ATIVAR O LOCK: Diz ao Android para não descartar pacotes UDP de broadcast
            WifiManager wifi = (WifiManager) context.getApplicationContext().getSystemService(Context.WIFI_SERVICE);
            if (wifi != null) {
                lock = wifi.createMulticastLock("udp_discovery_lock");
                lock.setReferenceCounted(true);
                lock.acquire();
            }

            // Define o alvo inicial
            InetAddress targetAddr;
            if (isEmulator()) {
                targetAddr = InetAddress.getByName("10.0.2.2");
                Log.d(LOG_TAG, "Modo Emulador: alvo 10.0.2.2");
            } else {
                targetAddr = getBroadcastAddress(context);
                Log.d(LOG_TAG, "Modo Real: Broadcast calculado: " + targetAddr.getHostAddress());
            }

            for (int i = 0; i < MAX_TRIES; i++) {
                try {
                    // Fallback na segunda tentativa: se o broadcast calculado falhou, tenta o global
                    if (i == 1 && !isEmulator()) {
                        targetAddr = InetAddress.getByName("255.255.255.255");
                        Log.d(LOG_TAG, "Tentativa 2: Fallback para 255.255.255.255");
                    }

                    if (socket != null) socket.close();
                    socket = new DatagramSocket();
                    socket.setSoTimeout(TIMEOUT_MS);
                    socket.setBroadcast(true);

                    byte[] sendData = "DISCOVER_TCP_SERVER".getBytes(StandardCharsets.UTF_8);
                    DatagramPacket sendPacket = new DatagramPacket(
                            sendData, sendData.length, targetAddr, DISCOVERY_PORT);

                    Log.d(LOG_TAG, "Enviando pacote para " + targetAddr.getHostAddress());
                    socket.send(sendPacket);

                    byte[] recvBuf = new byte[1024];
                    DatagramPacket receivePacket = new DatagramPacket(recvBuf, recvBuf.length);
                    
                    socket.receive(receivePacket);

                    String msg = new String(receivePacket.getData(), 0, receivePacket.getLength(), StandardCharsets.UTF_8);
                    Log.d(LOG_TAG, "Resposta do servidor: " + msg);

                    if (msg.startsWith("TCP_SERVER")) {
                        String[] parts = msg.split(";");
                        if (parts.length >= 3) {
                            String serverIp = parts[1];
                            int port = Integer.parseInt(parts[2]);
                            
                            // Emuladores redirecionam 10.0.2.2 para o localhost da máquina hospedeira
                            if (isEmulator()) serverIp = "10.0.2.2";
                            
                            return new InetSocketAddress(serverIp, port);
                        }
                    }

                } catch (java.net.SocketTimeoutException e) {
                    Log.w(LOG_TAG, "Timeout na tentativa " + (i + 1));
                }
            }

        } catch (Exception e) {
            Log.e(LOG_TAG, "Erro fatal no UDP Discovery", e);
        } finally {
            // LIBERAR RECURSOS: Evita vazamento de memória e gasto de bateria
            if (socket != null) socket.close();
            if (lock != null && lock.isHeld()) {
                lock.release();
            }
            Log.d(LOG_TAG, "Socket e MulticastLock liberados.");
        }
        return null;
    }

    public static boolean isEmulator() {
        return (Build.FINGERPRINT.startsWith("generic")
                || Build.FINGERPRINT.startsWith("unknown")
                || Build.MODEL.contains("google_sdk")
                || Build.MODEL.contains("Emulator")
                || Build.MODEL.contains("Android SDK built for x86")
                || Build.MANUFACTURER.contains("Genymotion")
                || "google_sdk".equals(Build.PRODUCT));
    }
}