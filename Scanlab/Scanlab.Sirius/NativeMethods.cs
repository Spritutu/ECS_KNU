
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Scanlab.Sirius
{
    /// <summary>P/Invoke 용 네이티브 코드 집합</summary>
    [SuppressUnmanagedCodeSecurity]
    internal static class NativeMethods
    {
        /// <summary>Ini 파일에서 읽기</summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="def"></param>
        /// <param name="retVal"></param>
        /// <param name="size"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern int GetPrivateProfileString(
          string section,
          string key,
          string def,
          StringBuilder retVal,
          int size,
          string filePath);

        public static T ReadIni<T>(string fileName, string section, string key)
        {
            StringBuilder retVal = new StringBuilder((int)byte.MaxValue);
            NativeMethods.GetPrivateProfileString(section, key, string.Empty, retVal, (int)byte.MaxValue, fileName);
            return (T)Convert.ChangeType((object)retVal.ToString(), typeof(T));
        }

        /// <summary>Ini 파일에 쓰기</summary>
        /// <param name="Section"></param>
        /// <param name="key"></param>
        /// <param name="val"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern long WritePrivateProfileString(
          string Section,
          string key,
          string val,
          string filePath);

        public static void WriteIni<T>(string fileName, string section, string key, T value) => NativeMethods.WritePrivateProfileString(section, key, value.ToString(), fileName);
    }
}
