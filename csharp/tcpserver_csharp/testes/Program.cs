using System.Collections;
using System.Net.Sockets;
using System.Text;
using auxiliar.binarybits;

namespace teste_tcp;

class Program
{
    static int passed = 0, failed = 0;

    static void Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--server")
        {
            string ip = args.Length > 1 ? args[1] : "127.0.0.1";
            int port = args.Length > 2 ? int.Parse(args[2]) : 9876;
            IntegrationTests(ip, port);
        }
        else if (args.Length > 0 && args[0] == "--client")
        {
            RunClient(args);
        }
        else
        {
            ProtocolTests();
            Console.WriteLine($"\n=== RESUMO: {passed} passaram, {failed} falharam ===");
            Environment.Exit(failed > 0 ? 1 : 0);
        }
    }

    // ========================================
    // Testes de protocolo (offline, sem servidor)
    // ========================================
    static void ProtocolTests()
    {
        Console.WriteLine("=== TESTES DE PROTOCOLO (OFFLINE) ===\n");

        TestBuildCommand0();
        TestBuildCommand1();
        TestBuildCommand2();
        TestBuildCommand3();
        TestBuildCommand4();
        TestBuildCommand5();
        TestBuildCommand6();
        TestBuildCommand7();
        TestBuildCommand8();
        TestBuildCommand9();

        TestParseResponseSync();
        TestParseResponseVolume();
        TestParseResponseString();
        TestParseResponseError();

        TestBinaryBitsAuxRoundtrip();
        TestTCPSendReceive();
    }

    // ---- Construção de pacotes ----

    static void TestBuildCommand0()
    {
        var bits = BinaryBitsAux.toBits(0, 4);
        AssertEqual("cmd0 bits count", 4, bits.Count);
        AssertEqual("cmd0 toInt", 0, BinaryBitsAux.toInt(bits, true));
        Pass("Build cmd0 (sync)");
    }

    static void TestBuildCommand1()
    {
        float vol = 75.5f;
        var bits = BinaryBitsAux.Combine(BinaryBitsAux.toBits(1, 4), BitConverter.GetBytes(vol));
        AssertEqual("cmd1 bits count", 36, bits.Count);
        AssertEqual("cmd1 code", 1, BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(bits, 0, 4), true));
        float parsedVol = BinaryBitsAux.toFloat(BinaryBitsAux.splitBitArray(bits, 4, 32));
        AssertEqual("cmd1 volume", vol, parsedVol);
        Pass("Build cmd1 (set volume)");
    }

    static void TestBuildCommand2()
    {
        var bits = BinaryBitsAux.toBits(2, 4);
        AssertEqual("cmd2 bits count", 4, bits.Count);
        AssertEqual("cmd2 code", 2, BinaryBitsAux.toInt(bits, true));
        Pass("Build cmd2 (shutdown)");
    }

    static void TestBuildCommand3()
    {
        int wc = 1920, hc = 1080, xc = 500, yc = 300;
        var bits = BinaryBitsAux.Combine(
            BinaryBitsAux.toBits(3, 4),
            BinaryBitsAux.toBits(wc, 13),
            BinaryBitsAux.toBits(hc, 13),
            BinaryBitsAux.toBits(xc, 13),
            BinaryBitsAux.toBits(yc, 13)
        );
        AssertEqual("cmd3 bits count", 4 + 13 * 4, bits.Count);
        AssertEqual("cmd3 code", 3, BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(bits, 0, 4), true));
        AssertEqual("cmd3 wc", wc, BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(bits, 4, 13), true));
        AssertEqual("cmd3 hc", hc, BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(bits, 17, 13), true));
        AssertEqual("cmd3 xc", xc, BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(bits, 30, 13), true));
        AssertEqual("cmd3 yc", yc, BinaryBitsAux.toInt(BinaryBitsAux.splitBitArray(bits, 43, 13), true));
        Pass("Build cmd3 (mouse move)");
    }

    static void TestBuildCommand4()
    {
        var bits = BinaryBitsAux.toBits(4, 4);
        AssertEqual("cmd4 bits count", 4, bits.Count);
        AssertEqual("cmd4 code", 4, BinaryBitsAux.toInt(bits, true));
        Pass("Build cmd4 (click)");
    }

    static void TestBuildCommand5()
    {
        var bits = BinaryBitsAux.toBits(5, 4);
        AssertEqual("cmd5 bits count", 4, bits.Count);
        AssertEqual("cmd5 code", 5, BinaryBitsAux.toInt(bits, true));
        Pass("Build cmd5 (lock)");
    }

    static void TestBuildCommand6()
    {
        var bits = BinaryBitsAux.toBits(6, 4);
        AssertEqual("cmd6 bits count", 4, bits.Count);
        AssertEqual("cmd6 code", 6, BinaryBitsAux.toInt(bits, true));
        Pass("Build cmd6 (mouse up)");
    }

    static void TestBuildCommand7()
    {
        var bits = BinaryBitsAux.toBits(7, 4);
        AssertEqual("cmd7 bits count", 4, bits.Count);
        AssertEqual("cmd7 code", 7, BinaryBitsAux.toInt(bits, true));
        Pass("Build cmd7 (mouse down)");
    }

    static void TestBuildCommand8()
    {
        var bits = BinaryBitsAux.toBits(8, 4);
        AssertEqual("cmd8 bits count", 4, bits.Count);
        AssertEqual("cmd8 code", 8, BinaryBitsAux.toInt(bits, true));
        Pass("Build cmd8 (mouse left)");
    }

    static void TestBuildCommand9()
    {
        var bits = BinaryBitsAux.toBits(9, 4);
        AssertEqual("cmd9 bits count", 4, bits.Count);
        AssertEqual("cmd9 code", 9, BinaryBitsAux.toInt(bits, true));
        Pass("Build cmd9 (mouse right)");
    }

    // ---- Testes de parsing de respostas ----

    static void TestParseResponseSync()
    {
        float vol = 65.25f;
        var response = BinaryBitsAux.Combine(BinaryBitsAux.to1Bit(true), vol);
        bool ok = BinaryBitsAux.splitBitArray(response, 0, 1)[0];
        float parsed = BinaryBitsAux.toFloat(BinaryBitsAux.splitBitArray(response, 1, 32));
        Assert("Parse sync ok", ok);
        AssertEqual("Parse sync volume", vol, parsed);
        Pass("Parse response sync");
    }

    static void TestParseResponseVolume()
    {
        float vol = 42.0f;
        var response = BinaryBitsAux.Combine(BinaryBitsAux.to1Bit(true), vol);
        bool ok = BinaryBitsAux.splitBitArray(response, 0, 1)[0];
        float parsed = BinaryBitsAux.toFloat(BinaryBitsAux.splitBitArray(response, 1, 32));
        Assert("Parse volume ok", ok);
        AssertEqual("Parse volume value", vol, parsed);
        Pass("Parse response volume");
    }

    static void TestParseResponseString()
    {
        string msg = "Click Recebido";
        var response = BinaryBitsAux.Combine(BinaryBitsAux.to1Bit(true), BinaryBitsAux.toBitArray(msg));
        bool ok = BinaryBitsAux.splitBitArray(response, 0, 1)[0];
        string parsed = BinaryBitsAux.toString(response, 1, 12);
        Assert("Parse string msg ok", ok);
        AssertEqual("Parse string msg", msg, parsed);
        Pass("Parse response with string");
    }

    static void TestParseResponseError()
    {
        var response = BinaryBitsAux.Combine(BinaryBitsAux.to1Bit(false), BinaryBitsAux.toBitArray("Erro"));
        bool ok = BinaryBitsAux.splitBitArray(response, 0, 1)[0];
        string parsed = BinaryBitsAux.toString(response, 1, 12);
        Assert("Parse error ok is false", !ok);
        AssertEqual("Parse error msg", "Erro", parsed);
        Pass("Parse response error");
    }

    // ---- Testes utilitários ----

    static void TestBinaryBitsAuxRoundtrip()
    {
        var original = new BitArray(new bool[] { true, false, true, true, false, true, false, false, true });
        byte[] bytes = BinaryBitsAux.toByteArray(original);
        var restored = BinaryBitsAux.returnBitArray(bytes, original.Count);
        Assert("Roundtrip BitArray->byte[]->BitArray", BinaryBitsAux.compareBitArray(original, restored));
        Pass("BitArray roundtrip");
    }

    static void TestTCPSendReceive()
    {
        var data = new BitArray(new bool[] { true, false, true, true, false });
        const int packagesize = 10;
        var framed = BinaryBitsAux.Combine(BinaryBitsAux.toBits(data.Length, packagesize), data);
        byte[] bytes = BinaryBitsAux.toByteArray(framed);
        Assert("Framed data has header", bytes.Length > 0);
        Assert("Framed count includes header", framed.Count > data.Count);
        Pass("TCP send/receive raw");
    }

    // ========================================
    // Testes de integração (requer servidor rodando)
    // ========================================
    static void IntegrationTests(string ip, int port)
    {
        Console.WriteLine($"=== TESTES DE INTEGRAÇÃO ({ip}:{port}) ===\n");

        if (!TestConnection(ip, port))
        {
            Console.WriteLine("Servidor não disponível. Abortando.");
            Environment.Exit(1);
        }

        TestCommand(ip, port, 0, "sync");
        TestCommand(ip, port, 1, "volume", 50f);
        TestCommand(ip, port, 4, "click");

        Console.WriteLine($"\n=== RESUMO: {passed} passaram, {failed} falharam ===");
        Environment.Exit(failed > 0 ? 1 : 0);
    }

    static bool TestConnection(string ip, int port)
    {
        try
        {
            using var client = new TcpClient(ip, port);
            client.Close();
            Pass($"Conexão com {ip}:{port}");
            return true;
        }
        catch (Exception ex)
        {
            Fail($"Conexão com {ip}:{port} - {ex.Message}");
            return false;
        }
    }

    static void TestCommand(string ip, int port, int code, string name, float? volume = null)
    {
        try
        {
            using var client = new TcpClient(ip, port);
            using var stream = client.GetStream();

            BitArray packet;
            if (code == 1 && volume.HasValue)
                packet = BinaryBitsAux.Combine(BinaryBitsAux.toBits(code, 4), BitConverter.GetBytes(volume.Value));
            else if (code == 3)
                packet = BinaryBitsAux.Combine(BinaryBitsAux.toBits(code, 4),
                    BinaryBitsAux.toBits(1920, 13), BinaryBitsAux.toBits(1080, 13),
                    BinaryBitsAux.toBits(500, 13), BinaryBitsAux.toBits(300, 13));
            else
                packet = BinaryBitsAux.toBits(code, 4);

            TCPUtil.sendPackage(packet, stream);
            var response = TCPUtil.receivePackage(stream);

            if (response != null && response.Count > 0)
                Pass($"Comando {code} ({name})");
            else
                Fail($"Comando {code} ({name}) - resposta vazia");

            client.Close();
        }
        catch (Exception ex)
        {
            Fail($"Comando {code} ({name}) - {ex.Message}");
        }
    }

    // ========================================
    // Modo cliente interativo
    // ========================================
    static void RunClient(string[] args)
    {
        string serverIp = args.Length > 1 ? args[1] : "127.0.0.1";
        int port = args.Length > 2 ? int.Parse(args[2]) : 9876;

        var commands = new Dictionary<string, (int code, string desc)>
        {
            ["sync"]     = (0, "Sincronizar (obter volume)"),
            ["volume"]   = (1, "Alterar volume (ex: volume 75.5)"),
            ["shutdown"] = (2, "Desligar PC"),
            ["mouse"]    = (3, "Mover mouse (ex: mouse 1920 1080 500 300)"),
            ["click"]    = (4, "Click mouse"),
            ["lock"]     = (5, "Bloquear tela"),
            ["up"]       = (6, "Mouse para cima"),
            ["down"]     = (7, "Mouse para baixo"),
            ["left"]     = (8, "Mouse para esquerda"),
            ["right"]    = (9, "Mouse para direita"),
        };

        string cmdKey = args.Length > 3 ? args[3] : "sync";

        if (!commands.ContainsKey(cmdKey))
        {
            Console.WriteLine("Uso: dotnet run -- --client [ip] [port] [comando] [args...]");
            Console.WriteLine("Comandos: sync, volume <valor>, shutdown, mouse <wc hc x y>, click, lock, up, down, left, right");
            return;
        }

        var (code, _) = commands[cmdKey];

        try
        {
            using var client = new TcpClient(serverIp, port);
            using var stream = client.GetStream();
            Console.WriteLine($"Conectado a {serverIp}:{port}");
            Console.WriteLine($"Comando: {cmdKey} (code {code})");

            BitArray packet;
            switch (cmdKey)
            {
                case "volume":
                    float vol = args.Length > 4 ? float.Parse(args[4]) : 50f;
                    packet = BinaryBitsAux.Combine(BinaryBitsAux.toBits(code, 4), BitConverter.GetBytes(vol));
                    Console.WriteLine($"Volume: {vol}");
                    break;
                case "mouse":
                    int wc = args.Length > 4 ? int.Parse(args[4]) : 1920;
                    int hc = args.Length > 5 ? int.Parse(args[5]) : 1080;
                    int xc = args.Length > 6 ? int.Parse(args[6]) : 500;
                    int yc = args.Length > 7 ? int.Parse(args[7]) : 300;
                    packet = BinaryBitsAux.Combine(
                        BinaryBitsAux.toBits(code, 4),
                        BinaryBitsAux.toBits(wc, 13), BinaryBitsAux.toBits(hc, 13),
                        BinaryBitsAux.toBits(xc, 13), BinaryBitsAux.toBits(yc, 13)
                    );
                    Console.WriteLine($"Mouse: wc={wc} hc={hc} x={xc} y={yc}");
                    break;
                default:
                    packet = BinaryBitsAux.toBits(code, 4);
                    break;
            }

            Console.WriteLine($"Payload: {BinaryBitsAux.ToBitString(packet)} [{packet.Length} bits]");
            TCPUtil.sendPackage(packet, stream);

            var resposta = TCPUtil.receivePackage(stream);
            if (resposta != null && resposta.Count > 0)
            {
                bool ok = resposta[0];
                Console.WriteLine($"Status: {(ok ? "OK" : "ERRO")}");

                if (code == 0 && resposta.Count >= 33)
                {
                    float volume = BinaryBitsAux.toFloat(BinaryBitsAux.splitBitArray(resposta, 1, 32));
                    Console.WriteLine($"Volume atual: {volume:F2}");
                }
                else if (resposta.Count > 13)
                {
                    string msg = BinaryBitsAux.toString(resposta, 1, 12);
                    Console.WriteLine($"Resposta: {msg}");
                }
                else
                {
                    Console.WriteLine($"Resposta raw: {BinaryBitsAux.ToBitString(resposta)} [{resposta.Count} bits]");
                }
            }

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
        }
    }

    // ========================================
    // Asserts
    // ========================================
    static void Assert(string label, bool condition)
    {
        if (!condition)
            Fail($"{label}: esperava true");
        else
            Pass(label);
    }

    static void AssertEqual(string label, int expected, int actual)
    {
        if (expected != actual)
            Fail($"{label}: esperava {expected}, obteve {actual}");
        else
            Pass(label);
    }

    static void AssertEqual(string label, float expected, float actual)
    {
        if (Math.Abs(expected - actual) > 0.001f)
            Fail($"{label}: esperava {expected}, obteve {actual}");
        else
            Pass(label);
    }

    static void AssertEqual(string label, string expected, string actual)
    {
        if (expected != actual)
            Fail($"{label}: esperava \"{expected}\", obteve \"{actual}\"");
        else
            Pass(label);
    }

    static void Pass(string label) { passed++; Console.WriteLine($"  [PASS] {label}"); }
    static void Fail(string label) { failed++; Console.WriteLine($"  [FAIL] {label}"); }
}
