using System.Runtime.InteropServices;

namespace tcpserver_csharp.auxiliar.utils.linux
{
public static class SetasLinux
    {
        // Enum com as setas
        public enum Tecla
        {
            Up,
            Down,
            Left,
            Right,
            Enter
        }

        #region X11 imports
        [DllImport("libX11.so.6")]
        private static extern IntPtr XOpenDisplay(IntPtr display);

        [DllImport("libX11.so.6")]
        private static extern int XCloseDisplay(IntPtr display);

        [DllImport("libX11.so.6")]
        private static extern uint XKeysymToKeycode(IntPtr display, uint keysym);

        [DllImport("libXtst.so.6")]
        private static extern int XTestFakeKeyEvent(IntPtr display, uint keycode, bool is_press, uint delay);

        [DllImport("libX11.so.6")]
        private static extern int XFlush(IntPtr display);

        #endregion

        // Keysyms para setas
        private const uint XK_Up = 0xff52;
        private const uint XK_Down = 0xff54;
        private const uint XK_Left = 0xff51;
        private const uint XK_Right = 0xff53;
        private const uint XK_Return = 0xff0d;

        private static uint GetKeyCode(IntPtr display, Tecla tecla)
        {
            uint keysym = tecla switch
            {
                Tecla.Up => XK_Up,
                Tecla.Down => XK_Down,
                Tecla.Left => XK_Left,
                Tecla.Right => XK_Right,
                Tecla.Enter => XK_Return,
                _ => throw new ArgumentOutOfRangeException()
            };
            return XKeysymToKeycode(display, keysym);
        }

        /// <summary>
        /// Pressiona e solta uma tecla de seta
        /// </summary>
        public static void PressionarSeta(Tecla tecla, int delayMs = 100)
        {
            IntPtr display = XOpenDisplay(IntPtr.Zero);
            if (display == IntPtr.Zero) throw new Exception("Não conseguiu abrir display X11");

            uint keycode = GetKeyCode(display, tecla);

            // Pressiona
            XTestFakeKeyEvent(display, keycode, true, 0);
            XFlush(display);

            Thread.Sleep(delayMs);

            // Solta
            XTestFakeKeyEvent(display, keycode, false, 0);
            XFlush(display);

            XCloseDisplay(display);
        }

        /// <summary>
        /// Pressiona uma tecla de seta N vezes
        /// </summary>
        public static void PressionarSeta(Tecla tecla, int vezes, int delayMs = 100)
        {
            for (int i = 0; i < vezes; i++)
            {
                PressionarSeta(tecla, delayMs);
                Thread.Sleep(50); // intervalo entre pressionamentos
            }
        }

        private const uint XK_Super_L = 0xffeb;
        private const uint XK_d = 0x0064;

        public static void MostrarDesktop()
        {
            IntPtr display = XOpenDisplay(IntPtr.Zero);
            if (display == IntPtr.Zero) throw new Exception("Não conseguiu abrir display X11");

            uint super = XKeysymToKeycode(display, XK_Super_L);
            uint d = XKeysymToKeycode(display, XK_d);

            // Pressiona Super
            XTestFakeKeyEvent(display, super, true, 0);
            // Pressiona D
            XTestFakeKeyEvent(display, d, true, 0);
            XFlush(display);

            Thread.Sleep(100);

            // Solta D
            XTestFakeKeyEvent(display, d, false, 0);
            // Solta Super
            XTestFakeKeyEvent(display, super, false, 0);
            XFlush(display);

            XCloseDisplay(display);
        }

    }
}