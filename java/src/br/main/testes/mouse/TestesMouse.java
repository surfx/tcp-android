package br.main.testes.mouse;

import java.awt.AWTException;
import java.awt.Dimension;
import java.awt.MouseInfo;
import java.awt.Point;
import java.awt.Robot;
import java.awt.Toolkit;
import java.awt.event.InputEvent;

import br.main.controles.MouseControl;

class MouseCorrectRobot extends Robot {
	final Dimension ScreenSize;// Primary Screen Size

	public MouseCorrectRobot() throws AWTException {
		super();
		ScreenSize = Toolkit.getDefaultToolkit().getScreenSize();
	}

	private static double getTav(Point a, Point b) {
		return Math.sqrt((double) ((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y)));
	}

	public void MoveMouseControlled(double xbe, double ybe)// Position of the cursor in [0,1] ranges. (0,0) is the upper
															// left corner
	{

		int xbepix = (int) (ScreenSize.width * xbe);
		int ybepix = (int) (ScreenSize.height * ybe);

		int x = xbepix;
		int y = ybepix;

		Point mert = MouseInfo.getPointerInfo().getLocation();
		Point ElozoInitPont = new Point(0, 0);

		int UgyanAztMeri = 0;
		final int UgyanAZtMeriLimit = 30;

		int i = 0;
		final int LepesLimit = 20000;
		while ((mert.x != xbepix || mert.y != ybepix) && i < LepesLimit && UgyanAztMeri < UgyanAZtMeriLimit) {
			++i;
			if (mert.x < xbepix)
				++x;
			else
				--x;
			if (mert.y < ybepix)
				++y;
			else
				--y;
			mouseMove(x, y);

			mert = MouseInfo.getPointerInfo().getLocation();

			if (getTav(ElozoInitPont, mert) < 5)
				++UgyanAztMeri;
			else {
				UgyanAztMeri = 0;
				ElozoInitPont.x = mert.x;
				ElozoInitPont.y = mert.y;
			}

		}
	}

}

public class TestesMouse {

	private MouseControl mc = new MouseControl();
	
	public void testeMouse1() {
//		Point screenSize = MouseControl.getScreenSize();
//		int wpc = screenSize.x;
//		int hpc = screenSize.y;
//		
//		System.out.println("wpc: " + wpc + ", hpc: " + hpc);
//		
//		mc.moveMouse(300, 400);
//		
//		Point pd = new Point(300, 400); // X,Y where mou;se must go
//		int n = 0;
//		Robot robot = null;
//		try {
//			robot = new Robot();
//		} catch (AWTException e) {
//			// TODO Auto-generated catch block
//			e.printStackTrace();
//		}
//		if (robot == null) {
//			return;
//		}
//		while ((!pd.equals(MouseInfo.getPointerInfo().getLocation())) && (++n <= 5)) {
//			robot.mouseMove(pd.x, pd.y);
//		}
//		
//		
//		try {
//			Thread.sleep(200);
//			System.out.println("-- MouseCorrectRobot --");
//			new MouseCorrectRobot().mouseMove(3000, 400);
//		} catch (AWTException e) {
//			e.printStackTrace();
//		} catch (InterruptedException e) {
//			e.printStackTrace();
//		}
		
//		try {
//			click(300, 400);
//		} catch (AWTException e) {
//			e.printStackTrace();
//		}
		
		keepAwake();
	}
	
	public void click(int x, int y) throws AWTException {
		Robot bot = new Robot();
		bot.mouseMove(x, y);
		bot.mousePress(InputEvent.BUTTON1_MASK);
		bot.mouseRelease(InputEvent.BUTTON1_MASK);
	}
	
	private void keepAwake() {
		System.out.println("Moving the mouse location slightly to keep the computer awake.");
		for (int i = 0; i < 20; i++) {
			try {
				Robot hal = new Robot();
				Point pObj = MouseInfo.getPointerInfo().getLocation();
				hal.mouseMove(pObj.x + 10, pObj.y + 10);
				//hal.mouseMove(pObj.x - 1, pObj.y - 1);
				pObj = MouseInfo.getPointerInfo().getLocation();
				System.out.println(pObj.toString() + "x>>" + pObj.x + "  y>>" + pObj.y);
				Thread.sleep(200);
			} catch (AWTException | NullPointerException ex) {
				System.out.println(ex);
			} catch (InterruptedException e) {
				e.printStackTrace();
			}
		}
	}
	
}