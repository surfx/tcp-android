namespace tcpserver_csharp.auxiliar.utils
{
    public static class OSDetector
    {
        #region Runtime detection

        public static bool IsWindows()
        {
            return OperatingSystem.IsWindows();
        }

        public static bool IsLinux()
        {
            return OperatingSystem.IsLinux();
        }

        public static bool IsMacOS()
        {
            return OperatingSystem.IsMacOS();
        }

        public static string GetOSName()
        {
            if (IsWindows()) return "Windows";
            if (IsLinux()) return "Linux";
            if (IsMacOS()) return "MacOS";
            return "Desconhecido";
        }

        #endregion
    }
}