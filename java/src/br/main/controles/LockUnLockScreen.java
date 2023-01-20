package br.main.controles;

import java.io.IOException;

import br.main.util.ExecuteLineCommand;
import br.main.util.OSValidator;

public class LockUnLockScreen {

	public static void lockSreen() {
		if (OSValidator.isWindows()) {
			try {
				Runtime.getRuntime().exec("C:\\Windows\\System32\\rundll32.exe user32.dll,LockWorkStation");
			} catch (IOException e) { e.printStackTrace(); }
			return;
		}
		/**
		 * https://vitux.com/three-ways-to-lock-your-ubuntu-screen/
		 * $ sudo apt-get install gnome-screensaver -y
		 * $ gnome-screensaver-command -l
		 */
		ExecuteLineCommand.execute("gnome-screensaver-command -l");
	}

	public static void unLockSreen() {
		if (OSValidator.isWindows()) {
			System.out.println("-- não implementado no Windows");
			return;
		}
		ExecuteLineCommand.execute("loginctl unlock-session");
	}
	
}