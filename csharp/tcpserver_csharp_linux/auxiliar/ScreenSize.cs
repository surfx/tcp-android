using System.Runtime.InteropServices;

namespace auxiliar
{
    class ScreenSize
    {

        public static int getWidth(){
            EnumWindows(E, IntPtr.Zero);
            return _.Item1;
        }

        public static int getHeight(){
            EnumWindows(E, IntPtr.Zero);
            return _.Item2;
        }

        struct R
        {
            int l;
            int t;
            public int r;
            public int b;

            public override string ToString() => $"{l},{t},{r},{b}";
            public bool i() => l == 0 && r != 00;
        }

        static (int, int) _;

        static bool E(IntPtr w, IntPtr l)
        {
            var r = new R();
            GetWindowRect(w, ref r);
            if (r.i() && _.Item1 == 0)
                _ = (r.r, r.b);
            return true;
        }

        delegate bool P(IntPtr w, IntPtr l);

        [DllImport("user32.dll")]
        static extern bool EnumWindows(P e, IntPtr l);

        [DllImport("user32.dll")]
        static extern bool GetWindowRect(IntPtr w, ref R r);
    }
}