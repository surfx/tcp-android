using System.Runtime.InteropServices;

namespace auxiliar
{
    public class MouseOperationsLinux
    {
        [Flags]
        public enum MouseEventFlags
        {
            LeftDown,
            LeftUp,
            RightDown,
            RightUp,
            MiddleDown,
            MiddleUp,
            Move
        }

        [DllImport("libX11.so.6")]
        private static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport("libX11.so.6")]
        private static extern int XCloseDisplay(IntPtr display);

        [DllImport("libX11.so.6")]
        private static extern int XWarpPointer(IntPtr display, IntPtr src_w, IntPtr dest_w,
                                               int src_x, int src_y, uint src_width, uint src_height,
                                               int dest_x, int dest_y);

        [DllImport("libX11.so.6")]
        private static extern int XFlush(IntPtr display);

        [DllImport("libXtst.so.6")]
        private static extern int XTestFakeButtonEvent(IntPtr display, uint button, bool is_press, ulong delay);

        [DllImport("libXtst.so.6")]
        private static extern int XTestFakeMotionEvent(IntPtr display, int screen_number, int x, int y, ulong delay);


        public static void SetCursorPosition(int x, int y)
        {
            IntPtr display = XOpenDisplay(IntPtr.Zero);
            if (display == IntPtr.Zero) return;

            IntPtr root = XDefaultRootWindow(display);

            XWarpPointer(display, IntPtr.Zero, root, 0, 0, 0, 0, x, y);

            // força envio imediato
            XFlush(display);

            XCloseDisplay(display);
        }


        [DllImport("libX11.so.6")]
        private static extern IntPtr XDefaultRootWindow(IntPtr display);



        [DllImport("libX11.so.6")]
        private static extern bool XQueryPointer(
    IntPtr display,
    IntPtr window,
    out IntPtr root_return,
    out IntPtr child_return,
    out int root_x,
    out int root_y,
    out int win_x,
    out int win_y,
    out uint mask_return);

        public static MousePoint GetCursorPosition()
        {
            IntPtr display = XOpenDisplay(IntPtr.Zero);
            if (display == IntPtr.Zero) return new MousePoint(0, 0);

            IntPtr root = XDefaultRootWindow(display);

            bool result = XQueryPointer(display, root,
                out _, out _,
                out int root_x, out int root_y,
                out _, out _, out _);

            XCloseDisplay(display);

            if (!result)
                return new MousePoint(0, 0);

            return new MousePoint(root_x, root_y);
        }


        public static void MouseEvent(MouseEventFlags value)
        {
            IntPtr display = XOpenDisplay(IntPtr.Zero);
            if (display == IntPtr.Zero) return;

            switch (value)
            {
                case MouseEventFlags.LeftDown:
                    XTestFakeButtonEvent(display, 1, true, 0);
                    break;
                case MouseEventFlags.LeftUp:
                    XTestFakeButtonEvent(display, 1, false, 0);
                    break;
                case MouseEventFlags.RightDown:
                    XTestFakeButtonEvent(display, 3, true, 0);
                    break;
                case MouseEventFlags.RightUp:
                    XTestFakeButtonEvent(display, 3, false, 0);
                    break;
                case MouseEventFlags.MiddleDown:
                    XTestFakeButtonEvent(display, 2, true, 0);
                    break;
                case MouseEventFlags.MiddleUp:
                    XTestFakeButtonEvent(display, 2, false, 0);
                    break;
            }

            XFlush(display);
            XCloseDisplay(display);
        }

        public struct MousePoint
        {
            public int X;
            public int Y;

            public MousePoint(int x, int y)
            {
                X = x;
                Y = y;
            }
        }
    }
}
