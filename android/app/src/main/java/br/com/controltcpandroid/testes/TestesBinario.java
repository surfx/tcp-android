package br.com.controltcpandroid.testes;

import java.util.BitSet;

public class TestesBinario {

    public static void testeMensagensBin(){
        System.out.println("testeMensagensBin()");

        BitSet bitSet = new BitSet(3);
        bitSet.set(0, true);
        bitSet.set(1, false);
        bitSet.set(2, true);



    }

}

//-----------------ENTRADA-------------------
//00011111000101000010110100101000010
//-----------------PARSE ENTRADA-------------------
//cod: 000 = 0
//11111000101000010110100101000010
//----------------RETORNO------------------
//111111000101000010110100101000010
//-----------------PARSE RETORNO-------------------
//bit0:                   True
//volumeRetornoParser:    11111000101000010110100101000010, valor: 75,26



//    public static void testeEntrada0(BitArray codOk)
//    {
//
//        // 0 - sincronizar: 3 bits (valor: 000)
//        // retorno: 1 bit (valor: 1 - codOk) + 4 bytes (float volume) = 32 bits
//
//        Console.WriteLine("-----------------ENTRADA-------------------");
//        // 3 + 32 bits
//        BitArray entrada = BinaryBitsAux.Combine(BinaryBitsAux.toBits(0, 3), 75.26f);
//        Console.WriteLine(BinaryBitsAux.ToBitString(entrada));
//
//        Console.WriteLine("-----------------PARSE ENTRADA-------------------");
//        // -- parse entrada0
//        BitArray cod = BinaryBitsAux.splitBitArray(entrada, 0, 3);
//        Console.WriteLine("cod: {0} = {1}", BinaryBitsAux.ToBitString(cod), BinaryBitsAux.toInt(cod, true));
//
//        BitArray volumeEntrada = BinaryBitsAux.splitBitArray(entrada, 3, 32);
//        Console.WriteLine(BinaryBitsAux.ToBitString(volumeEntrada));
//
//        Console.WriteLine("----------------RETORNO------------------");
//        // -- resposta
//        // 1 + 32 bits
//        BitArray retorno = BinaryBitsAux.Combine(codOk, 75.26f); // 75.26 float - 4 bytes = 32 bits
//        Console.WriteLine(BinaryBitsAux.ToBitString(retorno));
//
//        Console.WriteLine("-----------------PARSE RETORNO-------------------");
//
//        // -- parse retorno
//        bool bit0 = BinaryBitsAux.splitBitArray(retorno, 0, 1)[0];
//        BitArray volumeRetornoParser = BinaryBitsAux.splitBitArray(retorno, 1, 32);
//
//        Console.WriteLine("bit0:\t\t\t{0}", bit0);
//        Console.WriteLine("volumeRetornoParser: \t{0}, valor: {1}", BinaryBitsAux.ToBitString(volumeRetornoParser), BinaryBitsAux.toFloat(volumeRetornoParser));
//
//    }