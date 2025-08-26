using System.Runtime.InteropServices;

/*
    https://stackoverflow.com/questions/13745788/how-do-i-lock-a-windows-workstation-programmatically
*/
namespace auxiliar
{
    class LockScreen
    { 
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();
    }

}