package br.main.controles;

import java.io.IOException;

import br.main.util.OSValidator;

public class LockScreen {

	public static void lockSreen() {
		if (OSValidator.isWindows()) {
			try {
				Runtime.getRuntime().exec("C:\\Windows\\System32\\rundll32.exe user32.dll,LockWorkStation");
			} catch (IOException e) { e.printStackTrace(); }
		}
	}
	
}