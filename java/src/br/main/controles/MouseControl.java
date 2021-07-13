package br.main.controles;

import java.awt.AWTException;
import java.awt.Dimension;
import java.awt.Point;
import java.awt.Robot;
import java.awt.Toolkit;
import java.awt.event.InputEvent;

public class MouseControl {

	private Robot robot = null;

	public MouseControl() {
		try {
			robot = new Robot();
		} catch (AWTException e) {
			e.printStackTrace();
		}
	}

	public void moveMouse(int x, int y) {
		if (robot == null) { return; }
		robot.mouseMove(x, y);
	}

	public void clickMouse() {
		if (robot == null) { return; }
        robot.mousePress(InputEvent.BUTTON1_MASK);
        robot.mouseRelease(InputEvent.BUTTON1_MASK);
	}

	public static Point getScreenSize() {
        Dimension size = Toolkit.getDefaultToolkit().getScreenSize();
        return new Point((int)size.getWidth(), (int)size.getHeight());
	}
	
}