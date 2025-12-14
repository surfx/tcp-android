package br.com.controltcpandroid.udpdiscovery;

import android.os.Build;
import android.util.Log;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.nio.charset.StandardCharsets;

/**
 * envia uma request UDP via BROADCAST
 */
public class UDPServerTCPDiscovery {

    private static final String LOG_TAG = "UDPServerTCPDiscovery";
    private static final int DISCOVERY_PORT = 9875;
    private static final int TIMEOUT_MS = 2000; // Increased timeout for Wi-Fi
    private static final int MAX_TRIES = 2;

    /**
     * Checks if the app is running on an emulator.
     * @return true if running on an emulator, false otherwise.
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

    public static InetSocketAddress discover() {
        boolean isEmulator = isEmulator();
        String targetIp;
        if (isEmulator) {
            targetIp = "10.0.2.2"; // Special IP for the host machine from the emulator
            Log.d(LOG_TAG, "Running on Emulator, targeting host IP: " + targetIp);
        } else {
            targetIp = "255.255.255.255"; // Broadcast address for physical devices on LAN
            Log.d(LOG_TAG, "Running on Real Device, broadcasting to: " + targetIp);
        }

        for (int i = 0; i < MAX_TRIES; i++) {
            DatagramSocket socket = null;
            try {
                socket = new DatagramSocket();
                // Broadcasting is only needed for physical devices
                if (!isEmulator) {
                    socket.setBroadcast(true);
                }
                socket.setSoTimeout(TIMEOUT_MS);

                byte[] sendData = "DISCOVER_TCP_SERVER".getBytes(StandardCharsets.UTF_8);

                Log.d(LOG_TAG, "Attempt " + (i + 1) + "/" + MAX_TRIES + ": Sending discovery packet to " + targetIp + ":" + DISCOVERY_PORT);
                DatagramPacket sendPacket = new DatagramPacket(
                        sendData,
                        sendData.length,
                        InetAddress.getByName(targetIp),
                        DISCOVERY_PORT
                );

                socket.send(sendPacket);
                Log.d(LOG_TAG, "Packet sent. Waiting for response...");

                byte[] recvBuf = new byte[256];
                DatagramPacket receivePacket = new DatagramPacket(recvBuf, recvBuf.length);

                socket.receive(receivePacket); // This will timeout after TIMEOUT_MS

                String msg = new String(receivePacket.getData(), 0, receivePacket.getLength(), StandardCharsets.UTF_8);
                Log.d(LOG_TAG, "Received response: '" + msg + "' from " + receivePacket.getAddress().getHostAddress());

                if (msg.startsWith("TCP_SERVER")) {
                    Log.d(LOG_TAG, "Server response confirmed.");
                    String[] parts = msg.split(";");
                    String serverLanIp = parts[1];
                    int port = Integer.parseInt(parts[2]);

                    if (isEmulator) {
                        // For the emulator, ignore the server's LAN IP and use the host IP alias
                        Log.d(LOG_TAG, "Server responded with LAN IP " + serverLanIp + ", but we will connect to " + targetIp + " for emulator.");
                        return new InetSocketAddress(targetIp, port);
                    } else {
                        // For a real device, the server's reported LAN IP is the correct one to connect to
                        Log.d(LOG_TAG, "Server found on LAN at " + serverLanIp + ":" + port);
                        return new InetSocketAddress(serverLanIp, port);
                    }
                }

            } catch (java.net.SocketTimeoutException e) {
                Log.w(LOG_TAG, "Timeout waiting for server response. (Try " + (i + 1) + "/" + MAX_TRIES + ")");
            } catch (Exception e) {
                Log.e(LOG_TAG, "Error during discovery", e);
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
