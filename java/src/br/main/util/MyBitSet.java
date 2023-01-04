package br.main.util;

/**
 * Representa a classe BitSet, mas que funciona
 */
public class MyBitSet {

	private boolean[] _array;
	private int _size;
	
	public boolean[] getArray() { return _array; }
	public int size() { return _size; }
	
	public MyBitSet(final int size) {
		_array = new boolean[size];
		_size = size;
	}

	// se o 'pos' for inválido, ignora
	public void set(final boolean valor, final int pos) {
		if (pos < 0 || pos >= _size || _array == null || _array.length <= 0) {return;}
		_array[pos] = valor;
	}
	
	public boolean get(final int pos) {
		return (pos < 0 || pos >= _size || _array == null || _array.length <= 0) ? false : _array[pos];
	}

	public void clear() {
		if (_size <= 0 || _array == null || _array.length <= 0) { return; }
		for(int i = 0; i < _size; i++) { _array[i] = false; }
	}
	
	public boolean isValid() {
		return _size > 0 && _array != null && _array.length > 0;
	}
	
	// --- slice
	public MyBitSet slice(int posInicial, int size) {
		if (size<=0) {return null;}
		MyBitSet rt = new MyBitSet(size);
        for (int i = 0; i < size; i++) {
			rt.set(_array[posInicial++], i);
        }
        return rt;
	}
	
	// --- append
	public boolean[] append(MyBitSet mbit) {
		return append(mbit.getArray());
	}
	
	public boolean[] append(int[] inputAppend) {
		if (inputAppend==null||inputAppend.length<=0) {return null;}
		boolean[] aux = new boolean[inputAppend.length];
		for(int i = 0; i < inputAppend.length; i++) {
			aux[i] = inputAppend[i] == 1;
		}
		return append(aux);
	}
	
	
	public boolean[] append(float input) {
		return append(BinaryUtil.toByte(input));
	}
	
	public boolean[] append(byte[] inputAppend) {
		if (inputAppend==null||inputAppend.length<=0) {return null;}
		boolean[] aux = new boolean[inputAppend.length];
		for(int i = 0; i < inputAppend.length; i++) {
			aux[i] = inputAppend[i] == 1;
		}
		return append(aux);
	}
	
	public boolean[] append(boolean[] inputAppend) {
		if (inputAppend == null || inputAppend.length <= 0) { return _array; }
		_size = _array.length + inputAppend.length;
		boolean[] rt = new boolean[_size];
		int i = 0;
		for(i = 0; i < _array.length; i++) { rt[i] = _array[i]; }
		for(int j = 0; j < inputAppend.length; j++) { rt[i++] = inputAppend[j]; }
		_array = rt;
		return _array;
	}
	
	// -- to int
	public int[] toInt() {
		if (_size <= 0 || _array == null || _array.length <= 0) { return null; }
		int[] rt = new int[_size];
		for(int i = 0; i < _size; i++) { rt[i] = _array[i] ? 1 : 0; }
		return rt;
	}
	
	public void fromInt(int[] input) {
		if (input == null || input.length <= 0) { return; }
		_size = input.length;
		_array = new boolean[size()];
		for (int i = 0; i < _size; i++) { _array[i] = input[i] == 1; }
	}
	
	/**
	 * separa em grupos de 8 bits e converte para um vetor de byte.<br />
	 * Já faz a conversão (!)
	 * @return
	 */
	public byte[] toByte() {
		return BinaryUtil.toByteN(_array);
	}
	
	@Override
	public String toString() {
		String rt = "";
		if (_size <= 0 || _array == null || _array.length <= 0) { return rt; }
		for(int i = 0; i < _size; i++) { rt += _array[i] ? "1" : "0"; }
		return rt;
	}

}