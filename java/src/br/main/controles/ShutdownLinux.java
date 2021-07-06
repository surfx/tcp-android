package br.main.controles;

import java.io.IOException;

public class ShutdownLinux {

	/**
	 * desliga o linux
	 */
	public static void shutDownLinux() {
		try {
			Runtime.getRuntime().exec("shutdown -h now");
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

}