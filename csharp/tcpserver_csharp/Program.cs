﻿// See https://aka.ms/new-console-template for more information
// https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcplistener?view=net-7.0

using System.Net;
using System.Net.Sockets;
using auxiliar;

class Program
{

    static void Main(string[] args)
    {
        tcpServer();
    }

    /*
        https://learn.microsoft.com/pt-br/dotnet/api/system.net.sockets.tcplistener?view=net-7.0

        Cliente TCP conecta, envia mensagem e morre
        TODO: talvez no futuro melhorar o código para manter a conexão ativa.
        TODO 2: se for o caso, melhorar o código tcp server em Java também
    */
    private static void tcpServer() {
        TratarRequisicoes tratarRequisicoes = new TratarRequisicoes();

        new Thread(new ThreadStart(() => {
            TcpListener? server = null;

            try {
                const Int32 port = 9876;
                //IPAddress localAddr = IPAddress.Parse("127.0.0.1");
                //server = new TcpListener(localAddr, port);
                server = new TcpListener(IPAddress.Any, port);
                server.Start();

                Byte[] bytes = new Byte[256];
                String? data = null;

                while (true) {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = server.AcceptTcpClient();
                    //new Thread(new ThreadStart(() => {
                        Console.WriteLine("Connected!");

                        data = null;
                        NetworkStream stream = client.GetStream();


                        int i = stream.Read(bytes, 0, bytes.Length);
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        String resposta = "0Erro";
                        resposta = tratarRequisicoes.tratarRequisicoesTCP(data);

                        // Send back a response.
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(resposta);
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", resposta);
                        stream.Close();
                        
                    //})).Start();
                    
                }
            } catch (SocketException e) {
                Console.WriteLine("SocketException: {0}", e);
            } finally {
                if (server != null) { server.Stop(); }
            }
        })).Start();
    }

}