package br.main.util;

import java.io.IOException;

/**
 * Executa uma linha de comando no linux (por hora)
 * @author serpro
 *
 */
public class ExecuteLineCommand {

	private static final int EXPECTED_EXIT_CODE = 0;

	public static boolean execute(final String COMMAND) {
		final Runtime runtime = Runtime.getRuntime();
		Process process = null;
		try {
			process = runtime.exec(new String[]{ "/bin/sh", "-c", COMMAND });
			return process.waitFor() == EXPECTED_EXIT_CODE;
		} catch (final IOException e) {
			e.printStackTrace();
		} catch (final InterruptedException e) {
			e.printStackTrace();
		}
		return false;
	}
	
}
