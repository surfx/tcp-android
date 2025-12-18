using System.Runtime.InteropServices;

namespace tcpserver_csharp.auxiliar.user32dll
{
    internal class FindWindowTestes
    {
        //https://stackoverflow.com/questions/10898560/how-to-set-focus-to-another-window/10898649
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr handle, int nCmdShow);

        // Alterado de string para string? para permitir passar null sem aviso CS8625
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true, CharSet = CharSet.Auto)]
        static extern IntPtr FindWindow(string? lpClassName, string? lpWindowName);

        public static void findwindowteste()
        {
            // Uso de null! ou string? trata o aviso de conversão de literal nula
            IntPtr hWnd = FindWindow("focusWindowClassName", null); 

            if (hWnd != IntPtr.Zero)
            {
                ShowWindow(hWnd, 9);
                SetForegroundWindow(hWnd);
            }
        }
    }
}