// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.LaserVirtual
// Assembly: spirallab.sirius.rtc, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 330B13B0-CD9B-4679-A17E-EBB26CA3FE4F
// Assembly location: C:\Users\sean0\AppData\Local\Temp\Hevabaq\70527ef0f3\sirius-master\bin\spirallab.sirius.rtc.dll

using System;

namespace Scanlab.Sirius
{
    /// <summary>레이저 소스 (가상)</summary>
    public class LaserVirtual : ILaser, IDisposable
    {
        private bool disposed;

        /// <summary>동기화 객체</summary>
        public object SyncRoot { get; protected set; }

        /// <summary>식별 번호</summary>
        public uint Index { get; set; }

        /// <summary>이름</summary>
        public string Name { get; set; }

        /// <summary>최대 파워 (W)</summary>
        public float MaxPowerWatt { get; set; }

        /// <summary>상태 (준비완료 여부)</summary>
        public bool IsReady => !this.IsError;

        /// <summary>상태 (출사중 여부)</summary>
        public bool IsBusy => false;

        /// <summary>상태 (에러 발생 여부)</summary>
        public bool IsError { get; set; }

        /// <summary>레이저 제어에 필요한 IRtc 인터페이스</summary>
        public IRtc Rtc { get; set; }

        /// <summary>사용자 데이타</summary>
        public object Tag { get; set; }

        /// <summary>생성자</summary>
        public LaserVirtual()
        {
            this.SyncRoot = new object();
            this.Name = "Laser Virtual";
        }

        /// <summary>생성자</summary>
        /// <param name="index">식별번호</param>
        /// <param name="name">이름</param>
        /// <param name="maxPowerWatt">최대 파워 (W)</param>
        public LaserVirtual(uint index, string name, float maxPowerWatt)
          : this()
        {
            this.Index = index;
            this.Name = name;
            this.MaxPowerWatt = maxPowerWatt;
        }

        ~LaserVirtual()
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
            int num = disposing ? 1 : 0;
            this.disposed = true;
        }

        /// <summary>초기화</summary>
        /// <returns></returns>
        public bool Initialize()
        {
            lock (this.SyncRoot)
                return true;
        }

        public bool CtlAbort()
        {
            lock (this.SyncRoot)
                return true;
        }

        /// <summary>리셋</summary>
        /// <returns></returns>
        public bool CtlReset()
        {
            lock (this.SyncRoot)
            {
                this.IsError = false;
                return true;
            }
        }

        /// <summary>파워 변경</summary>
        /// <param name="watt">파워 (W)</param>
        /// <returns></returns>
        public bool CtlPower(float watt)
        {
            lock (this.SyncRoot)
            {
                int num = 1;
                if (num != 0)
                    Logger.Log(Logger.Type.Warn, string.Format("laser [{0}]: power to {1:F3}W", (object)this.Index, (object)watt), Array.Empty<object>());
                return num != 0;
            }
        }

        /// <summary>파워 변경 (RTC의 리스트 명령으로 처리시)</summary>
        /// <param name="watt">파워 (W)</param>
        /// <returns></returns>
        public bool ListPower(float watt)
        {
            lock (this.SyncRoot)
                return true;
        }
    }
}
