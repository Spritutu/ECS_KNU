using NLog;
using NLog.Config;
using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;

namespace Scanlab
{
    public sealed class Core : ICore
    {
        private static bool IsInitialized;

        /// <summary>코어 초기화</summary>
        /// <returns></returns>
        public bool InitializeEngine() => Core.Initialize();

        /// <summary>
        /// 초기화 (로그 엔진등을 초기화)
        /// 최초에 한번 호출되어야 함
        /// </summary>
        /// <returns></returns>
        public static bool Initialize()
        {
            if (Core.IsInitialized)
                return true;
            string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            LogManager.Configuration = NativeMethods.SetDllDirectory(Path.Combine(directoryName, IntPtr.Size == 8 ? "x64" : "x86")) ? (LoggingConfiguration)new XmlLoggingConfiguration(directoryName + "\\logs\\NLogSpiralLab.config") : throw new Win32Exception();
            Logger.logger = (ILogger)LogManager.GetCurrentClassLogger();
            //string assemblyFile1 = Path.Combine(directoryName, "spirallab.sirius.dll");
            //Version version1 = Assembly.LoadFrom(assemblyFile1).GetName().Version;
            //Logger.Log(Logger.Type.Info, assemblyFile1 + " version= " + version1.ToString(), Array.Empty<object>());
            string assemblyFile2 = Path.Combine(directoryName, "scanlab.sirius.rtc.dll");
            Version version2 = Assembly.LoadFrom(assemblyFile2).GetName().Version;
            Logger.Log(Logger.Type.Info, assemblyFile2 + " version= " + version2.ToString(), Array.Empty<object>());
            Core.IsInitialized = true;
            return Core.IsInitialized;
        }
    }
}
