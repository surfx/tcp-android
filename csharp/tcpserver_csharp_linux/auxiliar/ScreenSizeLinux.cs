using System.Runtime.InteropServices;

namespace auxiliar
{
    class ScreenSizeLinux
    {
        [DllImport("libX11.so.6")]
        private static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport("libX11.so.6")]
        private static extern int XCloseDisplay(IntPtr display);

        [DllImport("libX11.so.6")]
        private static extern IntPtr XDefaultRootWindow(IntPtr display);

        [DllImport("libX11.so.6")]
        private static extern int XGetGeometry(
            IntPtr display,
            IntPtr window,
            out IntPtr root_return,
            out int x,
            out int y,
            out uint width,
            out uint height,
            out uint border_width,
            out uint depth
        );

        public static int getWidth()
        {
            IntPtr display = XOpenDisplay(IntPtr.Zero);
            if (display == IntPtr.Zero) return 0;

            IntPtr root = XDefaultRootWindow(display);

            XGetGeometry(display, root, out _, out _, out _,
                         out uint width, out uint height,
                         out _, out _);

            XCloseDisplay(display);
            return (int)width;
        }

        public static int getHeight()
        {
            IntPtr display = XOpenDisplay(IntPtr.Zero);
            if (display == IntPtr.Zero) return 0;

            IntPtr root = XDefaultRootWindow(display);

            XGetGeometry(display, root, out _, out _, out _,
                         out uint width, out uint height,
                         out _, out _);

            XCloseDisplay(display);
            return (int)height;
        }
    }
}
