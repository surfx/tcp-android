using System.Runtime.InteropServices;

namespace tcpserver_csharp.auxiliar.user32dll
{
    internal class FindWindowTestes
    {
        //https://stackoverflow.com/questions/10898560/how-to-set-focus-to-another-window/10898649
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr handle, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        //[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        //public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        public static void findwindowteste()
        {
            //Then:
            // [Edit] Changed IntPrt to IntPtr
            IntPtr hWnd = FindWindow("focusWindowClassName", null); // this gives you the handle of the window you need.

            // then use this handle to bring the window to focus or forground(I guessed you wanted this).

            // sometimes the window may be minimized and the setforground function cannot bring it to focus so:

            /*use this ShowWindow(IntPtr handle, int nCmdShow);
            *there are various values of nCmdShow 3, 5 ,9. What 9 does is: 
            *Activates and displays the window. If the window is minimized or maximized, *the system restores it to its original size and position. An application *should specify this flag when restoring a minimized window */

            ShowWindow(hWnd, 9);
            //The bring the application to focus
            SetForegroundWindow(hWnd);

            // you wanted to bring the application to focus every 2 or few second
            // call other window as done above and recall this window again.
        }
    }
}