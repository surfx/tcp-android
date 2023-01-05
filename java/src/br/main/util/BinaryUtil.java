package br.main.util;

import java.nio.charset.Charset;

public class BinaryUtil {

	public static MyBitSet getBitsInteresse(byte[] buffer, final int numBits) {
		int pos = 0;
		boolean bigendian = false;
		boolean[] bitsInteresse = new boolean[numBits];
		for(int i = 0; i < buffer.length; i++) {
			if (pos >= numBits) {break;}
			for(int j = 0; j < 8; j++) {
				bitsInteresse[pos++] = getBit( buffer[i], bigendian ? 7 - j : j ) == 1;
				if (pos >= numBits) {break;}
			}
		}
		
		//for(int i = 0; i < pacoteSize; i++) { System.out.print(bitsInteresse[i]?1:0); } System.out.println();
		MyBitSet rt = new MyBitSet(numBits);
		rt.fromBoolean(bitsInteresse);
		return rt;
	}
	
    public static byte[] toByte(float input) {
	    byte[] floatB = floatToByteArray(input, false);
	    //for (int i = 0; i < floatB.length; i++) System.out.println(  toStr(floatB[i]) );
	    //System.out.println(byteArrayToFloat(floatB, false));
	    
	    int pos = 0;
	    byte[] convert = new byte[32];
	    for (int i = 0; i < floatB.length; i++) {
	  	  for(int j = 0; j < 8; j++) {
	  		  convert[pos++] = getBit(floatB[i], j);
	  	  }
	    }
	    return convert;
    }
    
    public static MyBitSet toMBit(String input) {
		byte[] bStr = input.getBytes(Charset.forName("UTF-8"));
		int pos = 0;
		byte[] toAppend = new byte[8 * bStr.length];
		for (int j = 0; j < bStr.length; j++) {
			for (int i = 0; i < 8; i++) {
				toAppend[pos++] = getBit(bStr[j], i);
			}
		}

		MyBitSet retorno = toMBit(toAppend.length, 12, false);
		retorno.append(toAppend);
		return retorno;
    }
    
    
    public static MyBitSet toMBit(final int input, final int numeroBits) {
    	return toMBit(input, numeroBits, false);
    }
    
    public static MyBitSet toMBit(final int input, final int numeroBits, final boolean inverse) {
    	byte[] bytes = intToBytes(input);
		int pos = 0;
		MyBitSet rt = new MyBitSet(numeroBits);
		
		for (int j = 0; j < bytes.length; j++) {
			if (pos >= numeroBits) {break;}
			for (int i = 0; i < 8; i++) {
				if (pos >= numeroBits) {break;}
				rt.set(getBit(bytes[j], inverse ? numeroBits - i : i) == 1, pos++);
			}
		}
    	return rt;
    }
    
    public static MyBitSet toMBitByte(byte input, final int numeroBits, final boolean inverse) {
    	if (numeroBits <= 0) { return null; }
        MyBitSet mbit = new MyBitSet(numeroBits);
        for(int i = 0; i < numeroBits; i++) {
      	  mbit.set(getBit(input, i)==1, inverse ? numeroBits - i : i);
        }
        return mbit;
    }
    
    //------------------
    
    
    public static int toInt(MyBitSet input) { return toInt(input, false); }
    /**
     * convert Big-endian e Little-endian to int
     * @param input
     * @param inverse
     * @return
     */
	public static int toInt(MyBitSet input, boolean inverse) {
	    int rt = 0;
	    int length = input.size();
	    for (int i = 0; i < length; i++) {
	        if (!input.get(i)) { continue; }
	        rt += (int)Math.pow(2, inverse ? i : length - i - 1);
	    }
	    return rt;
	}
    
    public static boolean[] convert(byte[] input) {
    	int length = input == null ? 0 : input.length;
		if (input == null || length <= 0) { return null; }
    	boolean[] rt = new boolean[length];
    	for(int i = 0; i < length; i++) {
    		rt[i] = input[i] == 1;
    	}
    	return rt;
    }
    
    public static boolean[] convert(int[] input) {
    	int length = input == null ? 0 : input.length;
		if (input == null || length <= 0) { return null; }
    	boolean[] rt = new boolean[length];
    	for(int i = 0; i < length; i++) {
    		rt[i] = input[i] == 1;
    	}
    	return rt;
    }
    
    /**
     * https://www.baeldung.com/java-convert-float-to-byte-array
     * @param value
     * @return
     */
    public static byte[] floatToByteArray(float value, boolean bigendian) {
        int intBits = Float.floatToIntBits(value);
        return
        bigendian ?
        new byte[] {
          (byte) (intBits >> 24), 
          (byte) (intBits >> 16), 
          (byte) (intBits >> 8), 
          (byte) (intBits) 
        } : 
    	new byte[] {
          (byte) (intBits), 
          (byte) (intBits >> 8), 
          (byte) (intBits >> 16), 
          (byte) (intBits >> 24) 
        };
    }
    
    public static float byteArrayToFloat(byte[] bytes, boolean bigendian) {
        int intBits =
        bigendian ?
          bytes[0] << 24 | 
          (bytes[1] & 0xFF) << 16 | 
          (bytes[2] & 0xFF) << 8 | 
          (bytes[3] & 0xFF) :
          bytes[0] & 0xFF | 
          (bytes[1] & 0xFF) << 8 | 
          (bytes[2] & 0xFF) << 16 | 
          (bytes[3] & 0xFF) << 24
        ;
        return Float.intBitsToFloat(intBits);  
    }
    
    //------------------
    
    public static boolean[] preencherBits(int[] input) {
    	return preencherBits(input, true);
    }
    public static boolean[] preencherBits(int[] input, boolean appendFinal) {
		if (input == null || input.length <= 0) { return null; }
    	return preencherBits(convert(input), appendFinal);
    }
    
    public static boolean[] preencherBits(boolean[] input) {
    	return preencherBits(input, true);
    }
    public static boolean[] preencherBits(boolean[] input, boolean appendFinal) {
		if (input == null || input.length <= 0) { return input; }
    	int length = input.length;
		int falta = length % 8 == 0 ? 0 : ((length / 8) + 1) * 8 - length;
    	
		if (falta <= 0) { return input; }
    	boolean[] rt = new boolean[length + falta];
    	if (appendFinal) {
			for (int i = 0; i < length; i++) {
				rt[i] = input[i];
    		}
			for (int i = length; i < falta; i++) {
				rt[i] = false;
    		}
    	} else {
    		for (int i = 0; i < falta; i++) {
				rt[i] = false;
    		}
    		int pos = 0;
    		for (int i = falta; i < length + falta; i++) {
				rt[i] = input[pos++];
    		}
    	}
    	input = rt;
    	return rt;
    }
    
    //------------------
    
    /**
     * converte um vetor de inteiros (01) para um vetor de bits - byte<br />
     * preenche os bits faltantes com 0 no fim do array
     * @param input
     * @return
     */
    public static byte[] toByteN(int[] input) {
    	return toByteN(convert(input));
    }
    
    public static byte[] toByteN(byte[] input) {
    	return toByteN(convert(input));
    }
    
    
    public static byte[] toByteN(boolean[] input) {
		if (input == null || input.length <= 0) { return new byte[] { 0x0 }; }
    	
		// corrige o tamanho do input para criar grupos de 8 bits
		input = preencherBits(input);
		
		// separar em grupos de 8 bits
    	boolean[] bits = new boolean[8];
    	int numerogrupos = (input.length / 8) + (input.length % 8 == 0 ? 0 : 1);
    	byte[] rt = new byte[numerogrupos];
    	
    	int pos = 0;
    	int group = 0;
    	for (int i = 0; i < input.length; i++) {
    		bits[pos++] = input[i];
    		if (i != 0 && (i+1) % 8 == 0) {
				pos = 0;
    			rt[group++] = toByte(bits, true);
    		}
    	}
    	
		return rt;
    }
    
    //------------------
    
    public static byte toByte(MyBitSet mbit) {
    	return toByte(mbit, true);
    }
    
    public static byte toByte(MyBitSet mbit, boolean bigendian) {
    	return toByte(mbit.getArray(), bigendian);
    }
    
    public static byte toByte(int[] bits) {
    	return toByte(bits, false);	
    }
    public static byte toByte(int[] bits, boolean bigendian) {
    	return toByte(new boolean[] { bits[0]==1,bits[1]==1,bits[2]==1,bits[3]==1,bits[4]==1,bits[5]==1,bits[6]==1,bits[7]==1}, bigendian);
    }
    
    public static byte toByte(boolean[] bits) { return toByte(bits, false); }
    public static byte toByte(boolean[] bits, boolean bigendian) {
    	
    	// 0xF = 1111
    	byte[] aux = new byte[8];
    	for(int i = 0; i < 8; i++) {
    		aux[i] = (byte)((bits[i]?1:0) & 0xF);
    		//System.out.println(toStr(aux[i]));
    	}

    	byte[] mask = new byte[] {
    		0x1,  		// 0000 0001
    		0x2,  		// 0000 0010
    		0x4,  		// 0000 0100
    		0x8,  		// 0000 1000
    		0x10, 		// 0001 0000
    		0x20, 		// 0010 0000
    		0x40, 		// 0100 0000
    		(byte) 0x80 // 1000 0000
    	};
    	
    	if (bigendian) {
    		aux[0] &= mask[0];	// 0x1 - 0000 0001
    		for(int i = 1; i < 8; i++) {
				aux[i] <<= i;
				aux[i] &= mask[i];
    		}
    	} else {
    		// littleendian - ao contrário
    		for(int i = 0; i < 8; i++) {
				aux[i] <<= 7-i;
				aux[i] &= mask[7-i];
    		}
    	}
    	//for(int i = 0; i < 8; i++) { System.out.println(toStr(aux[i])); }
    	
    	byte bf = 0x0;
    	for(int i = 0; i < 8; i++) {
    		bf |= aux[i];
    	}
    	bf &= 0xFF;	// 1111 1111
    	//System.out.println(toStr(bf));
    	return bf;
    }
    
    // https://javadeveloperzone.com/java-basic/java-convert-int-to-byte-array/
    public static byte[] intToBytes(final int data) {
    	return intToBytes(data, false);
    }
    public static byte[] intToBytes(final int data, final boolean bigendian) {
        return bigendian ? 
        new byte[] {
            (byte)((data >> 24) & 0xff),
            (byte)((data >> 16) & 0xff),
            (byte)((data >> 8) & 0xff),
            (byte)((data >> 0) & 0xff),
        } : 
    	new byte[] {
  			(byte)((data >> 0) & 0xff),
            (byte)((data >> 8) & 0xff),
            (byte)((data >> 16) & 0xff),
            (byte)((data >> 24) & 0xff),
        };
    }
    public static int convertByteArrayToInt(final byte[] data) {
    	return convertByteArrayToInt(data, false);
    }
    public static int convertByteArrayToInt(final byte[] data, final boolean bigendian) {
        if (data == null || data.length != 4) return 0x0;
        return bigendian ?
		(int)(
	        (0xff & data[0]) << 24  |
	        (0xff & data[1]) << 16  |
	        (0xff & data[2]) << 8   |
	        (0xff & data[3]) << 0
        ) :
    	(int)(
    		(0xff & data[0]) << 0	|
    		(0xff & data[1]) << 8   |
    		(0xff & data[2]) << 16  |
	        (0xff & data[3]) << 24
        );
    }

    
    public static String toStr(final byte b) {
    	return toStr(b, false);
    }
    public static String toStr(final byte b, final boolean bigendian) {
        String rt = "";
    	for (int i = 0; i < 8; i++) rt += getBit(b, bigendian ? 7-i : i);
        return rt;
    }
    
    public static byte getBit(final byte numero, final int position) {
       return (byte) ((numero >> position) & 1);
    }   

	
}