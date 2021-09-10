
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    internal sealed class Rockey4ND
    {
        private byte[] buffer = new byte[1000];
        private object obbuffer = new object();
        private ushort handle;
        private ushort p1 = 9976;
        private ushort p2 = 9494;
        private ushort p3 = 59119;
        private ushort p4 = 55849;
        private uint lp1;
        private uint lp2;
        private int iMaxRockey;
        private uint[] uiarrRy4ID = new uint[32];
        private uint iCurrID;
        private const string rtcFingerprint = "spirallab.sirius.rtc";
        private const string rtcSyncAxisFingerprint = "spirallab.sirius.rtc.syncaxis";
        private static bool isTaskRunning;

        [DllImport("Rockey4ND.dll", EntryPoint = "Rockey")]
        private static extern ushort Rockey32(
          ushort function,
          out ushort handle,
          out uint lp1,
          out uint lp2,
          out ushort p1,
          out ushort p2,
          out ushort p3,
          out ushort p4,
          ref byte buffer);

        [DllImport("Rockey4ND_X64.dll", EntryPoint = "Rockey")]
        private static extern ushort Rockey64(
          ushort function,
          out ushort handle,
          out uint lp1,
          out uint lp2,
          out ushort p1,
          out ushort p2,
          out ushort p3,
          out ushort p4,
          ref byte buffer);

        public static ushort Rockey(
          ushort function,
          out ushort handle,
          out uint lp1,
          out uint lp2,
          out ushort p1,
          out ushort p2,
          out ushort p3,
          out ushort p4,
          ref byte buffer)
        {
            return IntPtr.Size != 8 ? Rockey4ND.Rockey32(function, out handle, out lp1, out lp2, out p1, out p2, out p3, out p4, ref buffer) : Rockey4ND.Rockey64(function, out handle, out lp1, out lp2, out p1, out p2, out p3, out p4, ref buffer);
        }

        internal bool IsRtcLicensed { get; private set; }

        internal bool IsRtcSyncLicensed { get; private set; }

        internal Rockey4ND()
        {
        }

        internal bool Initialize()
        {
            if (this.IsRtcLicensed || this.IsRtcSyncLicensed)
                return true;
            this.IsRtcLicensed = false;
            this.IsRtcSyncLicensed = false;
            ulong num = 0;
            try
            {
                num = (ulong)Rockey4ND.Rockey((ushort)1, out this.handle, out this.lp1, out this.lp2, out this.p1, out this.p2, out this.p3, out this.p4, ref this.buffer[0]);
            }
            catch (Exception ex)
            {
                if (IntPtr.Size == 4)
                {
                    this.IsRtcLicensed = true;
                    Logger.Log(Logger.Type.Error, ex, ex.Message);
                    return true;
                }
            }
            if (num != 0UL)
            {
                Logger.Log(Logger.Type.Error, "usb dongle is not detected", Array.Empty<object>());
                return false;
            }
            this.uiarrRy4ID[this.iMaxRockey] = this.lp1;
            Logger.Log(Logger.Type.Debug, string.Format("usb dongle detected : {0:X8}", (object)this.uiarrRy4ID[0]), Array.Empty<object>());
            string data;
            if (!this.Read(out data))
            {
                Logger.Log(Logger.Type.Error, "usb dongle fingerprint invalid", Array.Empty<object>());
                return false;
            }
            this.IsRtcLicensed = data.StartsWith("spirallab.sirius.rtc");
            this.IsRtcSyncLicensed = data.StartsWith("spirallab.sirius.rtc.syncaxis");
            if (this.IsRtcSyncLicensed)
                this.IsRtcLicensed = true;
            return true;
        }

        internal void InvalidLicense()
        {
            if (Rockey4ND.isTaskRunning)
                return;
            Logger.Log(Logger.Type.Error, "fail to authenticate usb dongle fingerprint", Array.Empty<object>());
            int num1 = (int)MessageBox.Show("\rProgram terminated after 3 hours !\rPlease contact to labspiral@gmail.com if you want to purchase this products.", "Evaluation Copy - (c)SpiralLab", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Task.Run((Action)(() =>
            {
                Rockey4ND.isTaskRunning = true;
                Thread.Sleep(10800000);
                int num2 = (int)MessageBox.Show("\rProgram terminated after 60 secs !", "Evaluation Copy - (c)SpiralLab", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Thread.Sleep(60000);
                Logger.Log(Logger.Type.Fatal, "spirallab.sirius.rtc terminated by end of evaluation time !", Array.Empty<object>());
                Environment.Exit(0);
            }));
        }

        internal bool Read(out string data)
        {
            data = string.Empty;
            this.iCurrID = this.uiarrRy4ID[0];
            for (int index = 0; index < 1; ++index)
            {
                this.lp1 = this.uiarrRy4ID[index];
                this.lp2 = 0U;
                Array.Clear((Array)this.buffer, 0, this.buffer.Length);
                if (Rockey4ND.Rockey((ushort)3, out this.handle, out this.lp1, out this.lp2, out this.p1, out this.p2, out this.p3, out this.p4, ref this.buffer[0]) != (ushort)0)
                    return false;
                this.p1 = (ushort)0;
                this.p2 = (ushort)1000;
                if (Rockey4ND.Rockey((ushort)5, out this.handle, out this.lp1, out this.lp2, out this.p1, out this.p2, out this.p3, out this.p4, ref this.buffer[0]) != (ushort)0)
                {
                    int num = (int)Rockey4ND.Rockey((ushort)4, out this.handle, out this.lp1, out this.lp2, out this.p1, out this.p2, out this.p3, out this.p4, ref this.buffer[0]);
                    return false;
                }
                data = Encoding.Default.GetString(this.buffer);
            }
            return true;
        }

        internal bool Write()
        {
            ulong num1;
            ulong num2 = num1 = (ulong)Rockey4ND.Rockey((ushort)1, out this.handle, out this.lp1, out this.lp2, out this.p1, out this.p2, out this.p3, out this.p4, ref this.buffer[0]);
            if (num2 != 0UL)
            {
                Logger.Log(Logger.Type.Error, "usb dongle is not detected", Array.Empty<object>());
                return false;
            }
            this.uiarrRy4ID[this.iMaxRockey] = this.lp1;
            Logger.Log(Logger.Type.Debug, string.Format("usb dongle detected : {0:X8}", (object)this.uiarrRy4ID[0]), Array.Empty<object>());
            string empty = string.Empty;
            this.iCurrID = this.uiarrRy4ID[0];
            for (int index = 0; index < 1; ++index)
            {
                this.lp1 = this.uiarrRy4ID[index];
                this.lp2 = 0U;
                Array.Clear((Array)this.buffer, 0, this.buffer.Length);
                if (Rockey4ND.Rockey((ushort)3, out this.handle, out this.lp1, out this.lp2, out this.p1, out this.p2, out this.p3, out this.p4, ref this.buffer[0]) != (ushort)0)
                    return false;
                byte[] numArray = new byte[1000];
                Encoding.ASCII.GetBytes("spirallab.sirius.rtc").CopyTo((Array)numArray, 0);
                this.p1 = (ushort)0;
                this.p2 = (ushort)"spirallab.sirius.rtc".Length;
                if (Rockey4ND.Rockey((ushort)6, out this.handle, out this.lp1, out this.lp2, out this.p1, out this.p2, out this.p3, out this.p4, ref numArray[0]) != (ushort)0)
                {
                    int num3 = (int)Rockey4ND.Rockey((ushort)4, out this.handle, out this.lp1, out this.lp2, out this.p1, out this.p2, out this.p3, out this.p4, ref this.buffer[0]);
                    return false;
                }
                num2 = (ulong)Rockey4ND.Rockey((ushort)4, out this.handle, out this.lp1, out this.lp2, out this.p1, out this.p2, out this.p3, out this.p4, ref this.buffer[0]);
            }
            return num2 == 0UL;
        }

        internal enum Ry4Cmd : ushort
        {
            RY_FIND = 1,
            RY_FIND_NEXT = 2,
            RY_OPEN = 3,
            RY_CLOSE = 4,
            RY_READ = 5,
            RY_WRITE = 6,
            RY_RANDOM = 7,
            RY_SEED = 8,
            RY_READ_USERID = 10, // 0x000A
            RY_CHECK_MODULE = 12, // 0x000C
            RY_CALCULATE1 = 14, // 0x000E
            RY_CALCULATE2 = 15, // 0x000F
            RY_CALCULATE3 = 16, // 0x0010
        }

        internal enum Ry4ErrCode : uint
        {
            ERR_SUCCESS = 0,
            ERR_NO_PARALLEL_PORT = 2150629377, // 0x80300001
            ERR_NO_DRIVER = 2150629378, // 0x80300002
            ERR_NO_ROCKEY = 2150629379, // 0x80300003
            ERR_INVALID_PASSWORD = 2150629380, // 0x80300004
            ERR_INVALID_PASSWORD_OR_ID = 2150629381, // 0x80300005
            ERR_SETID = 2150629382, // 0x80300006
            ERR_INVALID_ADDR_OR_SIZE = 2150629383, // 0x80300007
            ERR_UNKNOWN_COMMAND = 2150629384, // 0x80300008
            ERR_NOTBELEVEL3 = 2150629385, // 0x80300009
            ERR_READ = 2150629386, // 0x8030000A
            ERR_WRITE = 2150629387, // 0x8030000B
            ERR_RANDOM = 2150629388, // 0x8030000C
            ERR_SEED = 2150629389, // 0x8030000D
            ERR_CALCULATE = 2150629390, // 0x8030000E
            ERR_NO_OPEN = 2150629391, // 0x8030000F
            ERR_OPEN_OVERFLOW = 2150629392, // 0x80300010
            ERR_NOMORE = 2150629393, // 0x80300011
            ERR_NEED_FIND = 2150629394, // 0x80300012
            ERR_DECREASE = 2150629395, // 0x80300013
            ERR_AR_BADCOMMAND = 2150629396, // 0x80300014
            ERR_AR_UNKNOWN_OPCODE = 2150629397, // 0x80300015
            ERR_AR_WRONGBEGIN = 2150629398, // 0x80300016
            ERR_AR_WRONG_END = 2150629399, // 0x80300017
            ERR_AR_VALUEOVERFLOW = 2150629400, // 0x80300018
            ERR_RECEIVE_NULL = 2150629632, // 0x80300100
            ERR_PRNPORT_BUSY = 2150629633, // 0x80300101
            ERR_UNKNOWN = 2150694911, // 0x8030FFFF
        }
    }
}
