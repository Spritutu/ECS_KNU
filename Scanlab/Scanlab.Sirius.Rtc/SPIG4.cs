
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Scanlab.Sirius
{
    /// <summary>SPI - G3/G3 IR 파이버 레이저 소스 객체</summary>
    public class SPIG4 : ILaser, IDisposable
    {
        protected SerialPort serial;
        protected int[] alarms;
        protected Timer timer;
        protected bool disposed;
        //internal Rockey4ND rockey = new Rockey4ND();

        /// <summary>동기화 객체</summary>
        public virtual object SyncRoot { get; protected set; }

        public virtual uint Index { get; set; }

        public virtual string Name { get; set; }

        public virtual float MaxPowerWatt { get; set; }

        public virtual ICompensator<float> PowerCompensator { get; set; }

        public virtual bool IsReady => !this.IsError;

        public virtual bool IsError
        {
            get
            {
                lock (this.SyncRoot)
                {
                    int[] alarms = this.alarms;
                    return alarms != null && (uint)alarms.Length > 0U;
                }
            }
        }

        public virtual bool IsBusy => false;

        /// <summary>레이저 제어에 필요한 IRtc 인터페이스</summary>
        public virtual IRtc Rtc { get; set; }

        public virtual object Tag { get; set; }

        public virtual float Temperature { get; private set; }

        public virtual int OperationHrs { get; private set; }

        public virtual uint ComPort { get; }

        public virtual SPIStatus Status { get; }

        /// <summary>생성자</summary>
        public SPIG4()
        {
            this.SyncRoot = new object();
            this.serial = new SerialPort();
            this.Status = new SPIStatus();
        }

        /// <summary>생성자</summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="comPort"></param>
        /// <param name="maxPowerWatt"></param>
        public SPIG4(uint index, string name, uint comPort, float maxPowerWatt = 20f)
          : this()
        {
            this.Index = index;
            this.Name = name;
            this.Index = index;
            this.ComPort = comPort;
            this.MaxPowerWatt = maxPowerWatt;
            this.serial.PortName = string.Format("COM{0}", (object)comPort);
        }

        ~SPIG4()
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

        public void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                this.timer?.Dispose();
                this.serial?.Dispose();
            }
            this.disposed = true;
        }

        public virtual bool Initialize()
        {
            //if (!this.rockey.Initialize() || !this.rockey.IsRtcLicensed)
            //    this.rockey.InvalidLicense();
            this.serial.BaudRate = 38400;
            this.serial.DataBits = 8;
            this.serial.Parity = Parity.None;
            this.serial.StopBits = StopBits.One;
            this.serial.ReadTimeout = 500;
            this.serial.WriteTimeout = 500;
            this.serial.Open();
            if (!this.serial.IsOpen)
                return false;
            this.timer = new Timer(new TimerCallback(this.OnTimer), (object)null, 1000, 1000);
            return true;
        }

        public virtual bool CtlAbort()
        {
            lock (this.SyncRoot)
                return true;
        }

        public virtual bool CtlReset()
        {
            lock (this.SyncRoot)
                return true;
        }

        public virtual List<int> GetAlarms()
        {
            List<int> intList = new List<int>();
            lock (this.SyncRoot)
            {
                if (this.alarms != null)
                {
                    intList.Capacity = this.alarms.Length;
                    for (int index = 0; index < this.alarms.Length; ++index)
                        intList.Add(this.alarms[index]);
                }
            }
            return intList;
        }

        public virtual string GetAlarmText(int code)
        {
            switch (code)
            {
                case 1:
                    return "ER_SPI_NOT_RESPONDING";
                case 2:
                    return "ER_SPI_INTERLOCK_OPEN";
                case 40:
                    return "ER_SPI_PRE_AMP_SIMMER_CURRENT";
                case 41:
                    return "ER_SPI_PRE_AMP_ACTIVE_STATE_CURRENT";
                case 42:
                    return "ER_SPI_PRE_AMP_OVER_CURRENT";
                case 43:
                    return "ER_SPI_POWER_AMP_OVER_CURRENT";
                case 44:
                    return "ER_SPI_PRE_AMP_UNDER_CURRENT";
                case 50:
                    return "ER_SPI_SEED_LASER_DIODE_TEMP";
                case 51:
                    return "ER_SPI_SEED_LASER_OVER_CURRENT";
                case 52:
                    return "ER_SPI_SEED_LASER_OUTPUT_POWER";
                case 65:
                    return "ER_SPI_BEAM_DELIVERY_CABLE_OR_COLLIMATOR_OVER_TEMP";
                case 70:
                    return "ER_SPI_CONTROLLER_POWER_SUPPLY_FAULT";
                case 80:
                    return "ER_SPI_TEMP_MONITOR_1_OUT_OF_RANGE";
                case 81:
                    return "ER_SPI_TEMP_SENSOR_FAULT";
                case 90:
                    return "ER_SPI_EARTH";
                case 91:
                    return "ER_SPI_SEED_PRE_AMP_POWER_SUPPLY";
                case 92:
                    return "ER_SPI_POWER_AMP_POWER_SUPPLY";
                case 93:
                    return "ER_SPI_DIODE_POWER_SUPPLY";
                case 99:
                    return "ER_SPI_EMERGENCY_STOP";
                default:
                    return string.Format("UNKNOWN ERROR CODE: {0}", (object)code);
            }
        }

        /// <summary>쓰레드 타이머</summary>
        /// <param name="state"></param>
        protected virtual void OnTimer(object state)
        {
            queryStatus();
            if (this.Status.Contains(SPIStatus.Status.Alarm))
                queryAlarms();
            else if (!this.Status.Contains(SPIStatus.Status.HardwareInterface) || !this.Status.Contains(SPIStatus.Status.ExternalControl))
                cmdExtCtrl();
            queryTemp();
            queryHours();

            void queryStatus()
            {
                string s1 = "QD\r\n";
                this.serial.Write(Encoding.ASCII.GetBytes(s1), 0, s1.Length);
                string s2 = this.serial.ReadTo("\n");
                ushort result;
                if (s2.Length <= 0 || s2[0] == 'E' || !ushort.TryParse(s2, out result))
                    return;
                this.Status.Word = (SPIStatus.Status)result;
            }

            void queryAlarms()
            {
                string s = "QA\r\n";
                this.serial.Write(Encoding.ASCII.GetBytes(s), 0, s.Length);
                string str = this.serial.ReadTo("\n");
                if (str.Length <= 0)
                    return;
                string[] array = str.Split(',');
                lock (this.SyncRoot)
                    this.alarms = Array.ConvertAll<string, int>(array, new Converter<string, int>(int.Parse));
            }

            void queryTemp()
            {
                string s1 = "QT\r\n";
                this.serial.Write(Encoding.ASCII.GetBytes(s1), 0, s1.Length);
                string s2 = this.serial.ReadTo("\n");
                if (s2.Length <= 0)
                    return;
                float result;
                float.TryParse(s2, out result);
                this.Temperature = result;
            }

            void queryHours()
            {
                string s1 = "QH\r\n";
                this.serial.Write(Encoding.ASCII.GetBytes(s1), 0, s1.Length);
                string s2 = this.serial.ReadTo("\n");
                if (s2.Length <= 0)
                    return;
                int result;
                int.TryParse(s2, out result);
                this.OperationHrs = result;
            }

            void cmdExtCtrl()
            {
                string s1 = "SC 4\r\n";
                this.serial.Write(Encoding.ASCII.GetBytes(s1), 0, s1.Length);
                this.serial.ReadTo("\n");
                Thread.Sleep(100);
                string s2 = "SS 9\r\n";
                this.serial.Write(Encoding.ASCII.GetBytes(s2), 0, s2.Length);
                this.serial.ReadTo("\n");
            }
        }

        public virtual bool CtlPower(float watt)
        {
            lock (this.SyncRoot)
            {
                this.Rtc.CtlWriteData<float>(ExtensionChannel.ExtAO1, (float)((double)watt / (double)this.MaxPowerWatt * 10.0));
                if (this.Rtc.CtlWriteData<float>(ExtensionChannel.ExtAO2, (float)(2.0 / (double)this.MaxPowerWatt * 10.0)))
                    Logger.Log(Logger.Type.Warn, string.Format("laser [{0}]: set laser power to {1:F3}W", (object)this.Index, (object)watt), Array.Empty<object>());
                return true;
            }
        }

        public virtual bool ListPower(float watt)
        {
            lock (this.SyncRoot)
                return (1 & (this.Rtc.ListWriteData<float>(ExtensionChannel.ExtAO1, (float)((double)watt / (double)this.MaxPowerWatt * 10.0)) ? 1 : 0)) != 0;
        }
    }
}
