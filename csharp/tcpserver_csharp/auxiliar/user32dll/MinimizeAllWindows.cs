using System.Runtime.InteropServices;

namespace tcpserver_csharp.auxiliar.user32dll
{
    internal class MinimizeAllWindows
    {
        // Alterado para string? para aceitar null sem aviso CS8625
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        const int WM_COMMAND = 0x111;
        const int MIN_ALL = 419;

        public static void minimizeAllWindows()
        {
            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            
            if (lHwnd != IntPtr.Zero)
            {
                SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
            }
        }
    }
}