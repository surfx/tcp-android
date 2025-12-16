package br.com.controltcpandroid.udpdiscovery;

import android.content.Context;
import android.net.wifi.WifiManager;
import android.os.Build;
import android.text.format.Formatter;
import android.util.Log;

import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.nio.charset.StandardCharsets;

/**
 * Envia uma request UDP via BROADCAST para descobrir o servidor TCP na LAN
 */
public class UDPServerTCPDiscovery {

    private static final String LOG_TAG = "UDPServerTCPDiscovery";
    private static final int DISCOVERY_PORT = 9875;
    private static final int TIMEOUT_MS = 2000;
    private static final int MAX_TRIES = 2;

    /**
     * Checks if the app is running on an emulator.
     */
    public static boolean isEmulator() {
        return (Build.FINGERPRINT.startsWith("generic")
                || Build.FINGERPRINT.startsWith("unknown")
                || Build.MODEL.contains("google_sdk")
                || Build.MODEL.contains("Emulator")
                || Build.MODEL.contains("Android SDK built for x86")
                || Build.MANUFACTURER.contains("Genymotion")
                || (Build.BRAND.startsWith("generic") && Build.DEVICE.startsWith("generic"))
                || "google_sdk".equals(Build.PRODUCT));
    }

    /**
     * Obtém o IP local do Wi-Fi ativo
     */
    private static InetAddress getLocalWifiIp(Context context) {
        try {
            WifiManager wifiManager =
                    (WifiManager) context.getApplicationContext()
                            .getSystemService(Context.WIFI_SERVICE);

            if (wifiManager == null || !wifiManager.isWifiEnabled()) {
                return null;
            }

            int ipInt = wifiManager.getConnectionInfo().getIpAddress();
            if (ipInt == 0) return null;

            String ipStr = Formatter.formatIpAddress(ipInt);
            return InetAddress.getByName(ipStr);

        } catch (Exception e) {
            Log.e(LOG_TAG, "Failed to get local Wi-Fi IP", e);
            return null;
        }
    }

    /**
     * Verifica se dois IPs estão na mesma sub-rede /24
     * (suficiente para 99% das redes Wi-Fi)
     */
    private static boolean isSameSubnet24(InetAddress local, InetAddress remote) {
        if (local == null || remote == null) return false;

        byte[] l = local.getAddress();
        byte[] r = remote.getAddress();

        // Apenas IPv4
        if (l.length != 4 || r.length != 4) return false;

        return l[0] == r[0] && l[1] == r[1] && l[2] == r[2];
    }

    /**
     * Discovers the TCP server via UDP broadcast.
     *
     * @return InetSocketAddress or null if not found
     */
    public static InetSocketAddress discover(Context context) {

        boolean isEmulator = isEmulator();
        String targetIp;

        InetAddress localWifiIp = null;

        if (isEmulator) {
            targetIp = "10.0.2.2"; // Host machine from Android Emulator
            Log.d(LOG_TAG, "Running on Emulator, targeting host IP: " + targetIp);
        } else {
            targetIp = "255.255.255.255"; // LAN broadcast
            localWifiIp = getLocalWifiIp(context);

            if (localWifiIp == null) {
                Log.w(LOG_TAG, "Wi-Fi not connected or IP not available");
                return null;
            }

            Log.d(LOG_TAG, "Running on Real Device");
            Log.d(LOG_TAG, "Local Wi-Fi IP: " + localWifiIp.getHostAddress());
        }

        for (int i = 0; i < MAX_TRIES; i++) {
            DatagramSocket socket = null;

            try {
                socket = new DatagramSocket();
                socket.setSoTimeout(TIMEOUT_MS);

                if (!isEmulator) {
                    socket.setBroadcast(true);
                }

                byte[] sendData = "DISCOVER_TCP_SERVER"
                        .getBytes(StandardCharsets.UTF_8);

                Log.d(LOG_TAG, "Attempt " + (i + 1) + "/" + MAX_TRIES +
                        " - Sending discovery packet");

                DatagramPacket sendPacket = new DatagramPacket(
                        sendData,
                        sendData.length,
                        InetAddress.getByName(targetIp),
                        DISCOVERY_PORT
                );

                socket.send(sendPacket);
                Log.d(LOG_TAG, "Packet sent. Waiting for response...");

                byte[] recvBuf = new byte[256];
                DatagramPacket receivePacket =
                        new DatagramPacket(recvBuf, recvBuf.length);

                socket.receive(receivePacket);

                String msg = new String(
                        receivePacket.getData(),
                        0,
                        receivePacket.getLength(),
                        StandardCharsets.UTF_8
                );

                Log.d(LOG_TAG, "Received response: '" + msg +
                        "' from " + receivePacket.getAddress().getHostAddress());

                // ---------- Validação da resposta ----------
                if (!msg.startsWith("TCP_SERVER")) {
                    Log.w(LOG_TAG, "Invalid response format, ignoring.");
                    continue;
                }

                String[] parts = msg.split(";");
                if (parts.length < 3) {
                    Log.w(LOG_TAG, "Malformed server response, ignoring.");
                    continue;
                }

                String serverLanIp = parts[1];
                int port;

                try {
                    port = Integer.parseInt(parts[2]);
                } catch (NumberFormatException e) {
                    Log.w(LOG_TAG, "Invalid port in server response, ignoring.");
                    continue;
                }

                InetAddress serverAddr = InetAddress.getByName(serverLanIp);

                // ❌ Ignorar loopback
                if (serverAddr.isLoopbackAddress()) {
                    Log.w(LOG_TAG, "Ignoring loopback IP: " + serverLanIp);
                    continue;
                }

                // ❌ Ignorar IP fora da sub-rede do Wi-Fi
                if (!isEmulator && !isSameSubnet24(localWifiIp, serverAddr)) {
                    Log.w(LOG_TAG, "Server not in same subnet. Server="
                            + serverLanIp + ", Local="
                            + localWifiIp.getHostAddress());
                    continue;
                }

                // ---------- Conexão correta ----------
                if (isEmulator) {
                    Log.d(LOG_TAG, "Server found. Emulator will connect to " +
                            targetIp + ":" + port);
                    return new InetSocketAddress(targetIp, port);
                } else {
                    Log.d(LOG_TAG, "Server found on LAN at " +
                            serverLanIp + ":" + port);
                    return new InetSocketAddress(serverLanIp, port);
                }

            } catch (java.net.SocketTimeoutException e) {
                Log.w(LOG_TAG, "Timeout waiting for server response. (" +
                        (i + 1) + "/" + MAX_TRIES + ")");
            } catch (Exception e) {
                Log.e(LOG_TAG, "Error during UDP discovery", e);
            } finally {
                if (socket != null && !socket.isClosed()) {
                    socket.close();
                    Log.d(LOG_TAG, "Socket closed.");
                }
            }
        }

        Log.w(LOG_TAG, "Server not found after " + MAX_TRIES + " tries.");
        return null;
    }

}