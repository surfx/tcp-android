package br.main.controles;

import java.io.IOException;

import br.main.util.OSValidator;

public class LockScreen {

	public static void lockSreen() {
		if (OSValidator.isWindows()) {
			try {
				Runtime.getRuntime().exec("C:\\Windows\\System32\\rundll32.exe user32.dll,LockWorkStation");
			} catch (IOException e) { e.printStackTrace(); }
			return;
		}
		ubuntuLockScreen();
	}

	
	private static final String COMMAND = "gnome-screensaver-command -l";
	private static final String[] OPEN_SHELL = { "/bin/sh", "-c", COMMAND };
	private static final int EXPECTED_EXIT_CODE = 0;

	/**
	 * https://vitux.com/three-ways-to-lock-your-ubuntu-screen/
	 * $ sudo apt-get install gnome-screensaver -y
	 * $ gnome-screensaver-command -l
	 */
	private static boolean ubuntuLockScreen() {
		final Runtime runtime = Runtime.getRuntime();
		Process process = null;
		try {
			process = runtime.exec(OPEN_SHELL);
			return process.waitFor() == EXPECTED_EXIT_CODE;
		} catch (final IOException e) {
			e.printStackTrace();
		} catch (final InterruptedException e) {
			e.printStackTrace();
		}
		return false;
	}

	
}