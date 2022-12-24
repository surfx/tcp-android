package br.main.controles;

import java.io.IOException;

import br.main.util.OSValidator;

public class Shutdown {


	public static void shutDown() {
		if (OSValidator.isWindows()) {
			shutDownWindows();
		}
		shutDownLinux();
	}

	/**
	 * desliga o windows
	 * https://learn.microsoft.com/en-us/windows-server/administration/windows-commands/shutdown
	 */
	private static void shutDownWindows() {
		try {
			Runtime.getRuntime().exec("shutdown /s /t 0");
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
	/**
	 * desliga o linux
	 */
	private static void shutDownLinux() {
		try {
			Runtime.getRuntime().exec("shutdown -h now");
		} catch (IOException e) {
			e.printStackTrace();
		}
	}
	
}