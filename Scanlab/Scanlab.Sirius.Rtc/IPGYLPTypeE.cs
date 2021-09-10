
using System;
using System.Threading;

namespace Scanlab.Sirius
{
    /// <summary>레이저 소스 (IPG YLP  Type E)</summary>
    public class IPGYLPTypeE : ILaser, IDisposable
    {
        private bool isReady;
        private bool isBusy;
        private bool isError;
        private bool disposed;
        private IPGYLPTypeE.Extension2Out ext2Out;
        private IPGYLPTypeE.Extension1In ext1In;
        private IPGYLPTypeE.Extension1Out ext1Out;
        private Timer timer;
        //private Rockey4ND rockey = new Rockey4ND();

        /// <summary>동기화 객체</summary>
        public object SyncRoot { get; protected set; }

        /// <summary>식별 번호</summary>
        public uint Index { get; set; }

        /// <summary>이름</summary>
        public string Name { get; set; }

        /// <summary>최대 파워 (W)</summary>
        public float MaxPowerWatt { get; set; }

        /// <summary>상태 (준비완료 여부)</summary>
        public bool IsReady
        {
            get => this.isReady;
            set => this.isReady = value;
        }

        /// <summary>상태 (출사중 여부)</summary>
        public bool IsBusy
        {
            get => this.isBusy;
            set => this.isBusy = value;
        }

        /// <summary>상태 (에러 발생 여부)</summary>
        public bool IsError
        {
            get => this.isError;
            set => this.isError = value;
        }

        /// <summary>IRtc 인터페이스</summary>
        public IRtc Rtc { get; set; }

        /// <summary>사용자 데이타</summary>
        public object Tag { get; set; }

        /// <summary>Extension2 출력 비트 객체</summary>
        internal IPGYLPTypeE.Extension2Out Ext2Out => this.ext2Out;

        /// <summary>Extension1 입력 비트 객체</summary>
        internal IPGYLPTypeE.Extension1In Ext1In => this.ext1In;

        /// <summary>Extension1 출력 비트 객체</summary>
        internal IPGYLPTypeE.Extension1Out Ext1Out => this.ext1Out;

        /// <summary>Power Set or Get (Watt)</summary>
        public float PowerWatt
        {
            get => this.ext2Out.PowerWatt;
            set => this.CtlPower(value);
        }

        /// <summary>Laser Emission Is On/Off?</summary>
        public bool IsEmission
        {
            get => this.ext1Out.Contains(IPGYLPTypeE.Extension1Out.Bit.EmissionEnable);
            set => this.Emission(value);
        }

        /// <summary>Guide (Pilot/Red) Laser Is On/Off?</summary>
        public bool IsGuideLaser
        {
            get => this.ext1Out.Contains(IPGYLPTypeE.Extension1Out.Bit.GuideLaser);
            set => this.GuideLaser(value);
        }

        public IPGYLPTypeE.AlarmCode AlarmMessage => this.ext1In.ToAlarm();

        /// <summary>생성자</summary>
        public IPGYLPTypeE()
        {
            this.SyncRoot = new object();
            this.Name = "IPG YLP Type E";
        }

        /// <summary>생성자</summary>
        /// <param name="index">식별번호</param>
        /// <param name="name">이름</param>
        /// <param name="maxPowerWatt">최대 파워 (W)</param>
        public IPGYLPTypeE(uint index, string name, float maxPowerWatt)
          : this()
        {
            this.Index = index;
            this.Name = name;
            this.MaxPowerWatt = maxPowerWatt;
        }

        /// <summary>종결자</summary>
        ~IPGYLPTypeE()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
                this.timer?.Dispose();
            this.disposed = true;
        }

        /// <summary>SPI 레이저 초기화</summary>
        /// <returns></returns>
        public bool Initialize()
        {
            //if (!this.rockey.Initialize() || !this.rockey.IsRtcLicensed)
                //this.rockey.InvalidLicense();
            bool flag1 = true;
            if (this.Rtc == null)
            {
                Logger.Log(Logger.Type.Error, string.Format("laser [{0}]: rtc is null !", (object)this.Index), Array.Empty<object>());
                return false;
            }
            this.ext2Out = new IPGYLPTypeE.Extension2Out(this.MaxPowerWatt);
            this.ext1Out = IPGYLPTypeE.Extension1Out.Empty;
            this.ext1In = IPGYLPTypeE.Extension1In.Empty;
            uint num;
            bool flag2 = flag1 & this.Rtc.CtlReadData<uint>(ExtensionChannel.ExtDI16, out num);
            if (flag2)
                this.ext1In.Value = (IPGYLPTypeE.Extension1In.Bit)num;
            bool flag3 = flag2 & this.Emission(true) & this.CtlPower(this.MaxPowerWatt * 0.1f);
            this.timer?.Dispose();
            this.timer = new Timer(new TimerCallback(this.OnUpdate), (object)null, 0, 100);
            return flag3;
        }

        /// <summary>extension1 port 의 입력상태를 읽어 업데이트</summary>
        /// <param name="state"></param>
        private void OnUpdate(object state)
        {
            if (this.Rtc == null)
                return;
            uint num;
            if ((1 & (this.Rtc.CtlReadData<uint>(ExtensionChannel.ExtDI16, out num) ? 1 : 0)) != 0)
            {
                this.ext1In.Value = (IPGYLPTypeE.Extension1In.Bit)num;
                this.isReady = this.ext1In.ToAlarm() == IPGYLPTypeE.AlarmCode.Normal;
                this.isError = (uint)this.ext1In.ToAlarm() > 0U;
            }
            else
            {
                this.isReady = false;
                this.isError = true;
            }
        }

        /// <summary>
        /// Abort (turn off emmission signal)
        /// 강제 중지 (Emission 출력 Disable 됨)
        /// </summary>
        /// <returns></returns>
        public bool CtlAbort()
        {
            if (this.Rtc == null)
                return false;
            Logger.Log(Logger.Type.Warn, string.Format("laser [{0}]: trying to abort ... ", (object)this.Index), Array.Empty<object>());
            return (1 & (this.Emission(false) ? 1 : 0)) != 0;
        }

        /// <summary>
        /// Reset (turn on emmission signal)
        /// 리셋 (에러상태 해제 시도)
        /// </summary>
        /// <returns></returns>
        public bool CtlReset()
        {
            if (this.Rtc == null)
                return false;
            Logger.Log(Logger.Type.Info, string.Format("laser [{0}]: trying to reset ... ", (object)this.Index), Array.Empty<object>());
            return (1 & (this.Emission(true) ? 1 : 0)) != 0;
        }

        /// <summary>
        /// Output power change to assigned watt with control command
        /// 파워 변경  (RTC의 컨트롤 명령으로 처리시)
        /// </summary>
        /// <param name="watt">파워 (W)</param>
        /// <returns></returns>
        public bool CtlPower(float watt)
        {
            if (this.Rtc == null)
                return false;
            if ((double)watt > (double)this.MaxPowerWatt)
            {
                Logger.Log(Logger.Type.Error, string.Format("laser [{0}]: out of range laser power: {1:F3}W", (object)this.Index, (object)watt), Array.Empty<object>());
                return false;
            }
            Logger.Log(Logger.Type.Info, string.Format("laser [{0}]: trying to change power to {1:F3}", (object)this.Index, (object)watt), Array.Empty<object>());
            this.ext2Out.PowerWatt = watt;
            this.ext2Out.Remove(IPGYLPTypeE.Extension2Out.Bit.Latch);
            int num1 = 1 & (this.Rtc.CtlWriteData<uint>(ExtensionChannel.ExtDO8, this.ext2Out.ToUInt()) ? 1 : 0);
            Thread.Sleep(1);
            this.ext2Out.Add(IPGYLPTypeE.Extension2Out.Bit.Latch);
            int num2 = this.Rtc.CtlWriteData<uint>(ExtensionChannel.ExtDO8, this.ext2Out.ToUInt()) ? 1 : 0;
            int num3 = num1 & num2;
            Thread.Sleep(1);
            this.ext2Out.Remove(IPGYLPTypeE.Extension2Out.Bit.Latch);
            int num4 = this.Rtc.CtlWriteData<uint>(ExtensionChannel.ExtDO8, this.ext2Out.ToUInt()) ? 1 : 0;
            int num5 = num3 & num4;
            if (num5 != 0)
            {
                Logger.Log(Logger.Type.Warn, string.Format("laser [{0}]: power to {1:F3}W", (object)this.Index, (object)watt), Array.Empty<object>());
                return num5 != 0;
            }
            Logger.Log(Logger.Type.Error, string.Format("laser [{0}]: fail to change power to {1:F3}", (object)this.Index, (object)watt), Array.Empty<object>());
            return num5 != 0;
        }

        /// <summary>
        /// Output power change to assigned watt with list command
        /// 파워 변경 (RTC의 리스트 명령으로 처리시)
        /// </summary>
        /// <param name="watt">파워 (W)</param>
        /// <returns></returns>
        public bool ListPower(float watt)
        {
            if (this.Rtc == null)
                return false;
            if ((double)watt > (double)this.MaxPowerWatt)
            {
                Logger.Log(Logger.Type.Error, string.Format("laser [{0}]: out of range laser power: {1:F3}W", (object)this.Index, (object)watt), Array.Empty<object>());
                return false;
            }
            float msec = 0.1f;
            this.ext2Out.PowerWatt = watt;
            this.ext2Out.Remove(IPGYLPTypeE.Extension2Out.Bit.Latch);
            int num1 = 1 & (this.Rtc.ListWriteData<uint>(ExtensionChannel.ExtDO8, this.ext2Out.ToUInt()) ? 1 : 0) & (this.Rtc.ListWait(msec) ? 1 : 0);
            this.ext2Out.Add(IPGYLPTypeE.Extension2Out.Bit.Latch);
            int num2 = this.Rtc.ListWriteData<uint>(ExtensionChannel.ExtDO8, this.ext2Out.ToUInt()) ? 1 : 0;
            int num3 = num1 & num2 & (this.Rtc.ListWait(msec) ? 1 : 0);
            this.ext2Out.Remove(IPGYLPTypeE.Extension2Out.Bit.Latch);
            int num4 = this.Rtc.ListWriteData<uint>(ExtensionChannel.ExtDO8, this.ext2Out.ToUInt()) ? 1 : 0;
            return (num3 & num4) != 0;
        }

        /// <summary>Enable/Disable Emission</summary>
        /// <param name="onOff">On/Off</param>
        /// <returns></returns>
        public bool Emission(bool onOff)
        {
            if (this.Rtc == null)
                return false;
            if (onOff)
                this.ext1Out.Add(IPGYLPTypeE.Extension1Out.Bit.EmissionEnable);
            else
                this.ext1Out.Remove(IPGYLPTypeE.Extension1Out.Bit.EmissionEnable);
            int num = 1 & (this.Rtc.CtlWriteData<uint>(ExtensionChannel.ExtDO16, this.ext1Out.ToUInt()) ? 1 : 0);
            Thread.Sleep(1);
            if (num != 0)
            {
                Logger.Log(Logger.Type.Info, string.Format("laser [{0}]: success to emission to {1}", (object)this.Index, (object)onOff), Array.Empty<object>());
                return num != 0;
            }
            Logger.Log(Logger.Type.Error, string.Format("laser [{0}]: fail to emission to {1}", (object)this.Index, (object)onOff), Array.Empty<object>());
            return num != 0;
        }

        /// <summary>Guide(Pilot/Red) Laser On/Off</summary>
        /// <param name="onOff">On/Off</param>
        /// <returns></returns>
        public bool GuideLaser(bool onOff)
        {
            if (this.Rtc == null)
                return false;
            if (onOff)
                this.ext1Out.Add(IPGYLPTypeE.Extension1Out.Bit.GuideLaser);
            else
                this.ext1Out.Remove(IPGYLPTypeE.Extension1Out.Bit.GuideLaser);
            int num = 1 & (this.Rtc.CtlWriteData<uint>(ExtensionChannel.ExtDO16, this.ext1Out.ToUInt()) ? 1 : 0);
            Thread.Sleep(1);
            if (num != 0)
            {
                Logger.Log(Logger.Type.Info, string.Format("laser [{0}]: success to guid laser to {1}", (object)this.Index, (object)onOff), Array.Empty<object>());
                return num != 0;
            }
            Logger.Log(Logger.Type.Error, string.Format("laser [{0}]: fail to guid laser to {1}", (object)this.Index, (object)onOff), Array.Empty<object>());
            return num != 0;
        }

        /// <summary>RTC Ext1 포트</summary>
        internal sealed class Extension2Out
        {
            public IPGYLPTypeE.Extension2Out.Bit Value { get; set; }

            /// <summary>
            /// 7 bit resolution 으로 동작됨
            /// set 동작시 latch 비트가 초기화 됨
            /// </summary>
            public ushort Power7Bits
            {
                get => (ushort)((uint)this.Value / 2U);
                set => this.Value = (IPGYLPTypeE.Extension2Out.Bit)((int)value * 2);
            }

            /// <summary>0~최대파워값(Watt)으로 동작됨</summary>
            public float PowerWatt
            {
                get => (float)this.Power7Bits / 128f * this.MaxWatt;
                set => this.Power7Bits = (ushort)((double)value / (double)this.MaxWatt * 128.0);
            }

            public void Add(IPGYLPTypeE.Extension2Out.Bit flag) => this.Value |= flag;

            public void Remove(IPGYLPTypeE.Extension2Out.Bit flag) => this.Value &= ~flag;

            public bool Contains(IPGYLPTypeE.Extension2Out.Bit flag) => Convert.ToBoolean((object)(this.Value & flag));

            public uint ToUInt() => (uint)this.Value;

            public float MaxWatt { get; private set; }

            public Extension2Out(float maxWatt) => this.MaxWatt = maxWatt;

            /// <summary>7bit output power resolution (128 단계 : 하위 1 비트는 삭제됨)</summary>
            [System.Flags]
            public enum Bit : uint
            {
                /// <summary>LSB</summary>
                Latch = 1,
                /// <summary>Data1</summary>
                Power0 = 2,
                Power1 = 4,
                Power2 = 8,
                Power3 = 16, // 0x00000010
                Power4 = 32, // 0x00000020
                Power5 = 64, // 0x00000040
                Power6 = 128, // 0x00000080
            }
        }

        /// <summary>알람코드</summary>
        public enum AlarmCode
        {
            /// <summary>Unknown/Invalid status</summary>
            Unknown = -1, // 0xFFFFFFFF
            /// <summary>Normal operation</summary>
            Normal = 0,
            /// <summary>
            /// Laser temperature is out of the operating temperature range
            /// </summary>
            Temperature = 1,
            /// <summary>External supply voltage is out of the specified range</summary>
            PowerSupply = 2,
            /// <summary>Laser is not ready for emission</summary>
            LaserIsNotReady = 3,
            /// <summary>
            /// Laser automatically switched OFF due to high optical power reflected back to the laser
            /// </summary>
            BackReflection = 4,
            /// <summary>Laser protection system detectes internal failure</summary>
            System = 5,
            /// <summary>Reserved</summary>
            Reserved = 6,
        }

        /// <summary>Extension1 입력 비트 객체</summary>
        internal sealed class Extension1In
        {
            public IPGYLPTypeE.Extension1In.Bit Value { get; set; }

            public void Add(IPGYLPTypeE.Extension1In.Bit flag) => this.Value |= flag;

            public void Remove(IPGYLPTypeE.Extension1In.Bit flag) => this.Value &= ~flag;

            public bool Contains(IPGYLPTypeE.Extension1In.Bit flag) => Convert.ToBoolean((object)(this.Value & flag));

            public IPGYLPTypeE.AlarmCode ToAlarm()
            {
                if (!this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm3) && !this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm1) && this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm2))
                    return IPGYLPTypeE.AlarmCode.Normal;
                if (!this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm3) && !this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm1) && !this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm2))
                    return IPGYLPTypeE.AlarmCode.Temperature;
                if (this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm3) && !this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm1) && !this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm2))
                    return IPGYLPTypeE.AlarmCode.PowerSupply;
                if (this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm3) && !this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm1) && this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm2))
                    return IPGYLPTypeE.AlarmCode.LaserIsNotReady;
                if (this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm3) && this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm1) && !this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm2))
                    return IPGYLPTypeE.AlarmCode.Reserved;
                return !this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm3) && this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm1) && this.Contains(IPGYLPTypeE.Extension1In.Bit.Alarm2) ? IPGYLPTypeE.AlarmCode.System : IPGYLPTypeE.AlarmCode.Unknown;
            }

            public uint ToUInt() => (uint)this.Value;

            public static IPGYLPTypeE.Extension1In Empty => new IPGYLPTypeE.Extension1In();

            [System.Flags]
            public enum Bit : uint
            {
                /// <summary>IPG Pin 16</summary>
                Alarm1 = 1,
                /// <summary>IPG Pin 21</summary>
                Alarm2 = 2,
                /// <summary>IPG Pin 11</summary>
                Alarm3 = 4,
                SerialDataOutput = 16, // 0x00000010
            }
        }

        /// <summary>Extension1 출력 비트 객체</summary>
        internal sealed class Extension1Out
        {
            public IPGYLPTypeE.Extension1Out.Bit Value { get; set; }

            public void Add(IPGYLPTypeE.Extension1Out.Bit flag) => this.Value |= flag;

            public void Remove(IPGYLPTypeE.Extension1Out.Bit flag) => this.Value &= ~flag;

            public bool Contains(IPGYLPTypeE.Extension1Out.Bit flag) => Convert.ToBoolean((object)(this.Value & flag));

            public uint ToUInt() => (uint)this.Value;

            public static IPGYLPTypeE.Extension1Out Empty => new IPGYLPTypeE.Extension1Out();

            [System.Flags]
            public enum Bit : uint
            {
                EmissionEnable = 1,
                GuideLaser = 4,
                SerialDataInput = 8,
                SerialDataClocks = 16, // 0x00000010
                InterfaceEnable = 32, // 0x00000020
            }
        }
    }
}
