using System.Runtime.InteropServices;
using System.Security;

namespace Scanlab
{
    [SuppressUnmanagedCodeSecurity]
    internal sealed class NativeMethods
    {
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetDllDirectory(string lpPathName);
    }
}
