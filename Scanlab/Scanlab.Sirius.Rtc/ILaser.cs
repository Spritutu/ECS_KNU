// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.ILaser
// Assembly: spirallab.sirius.rtc, Version=1.0.1.0, Culture=neutral, PublicKeyToken=null
// MVID: 330B13B0-CD9B-4679-A17E-EBB26CA3FE4F
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.rtc.dll

using System;

namespace Scanlab.Sirius
{
    /// <summary>레이저 소스 인터페이스</summary>
    public interface ILaser : IDisposable
    {
        /// <summary>동기화 객체</summary>
        object SyncRoot { get; }

        /// <summary>식별번호</summary>
        uint Index { get; set; }

        /// <summary>이름</summary>
        string Name { get; set; }

        /// <summary>최대 출력 에너지 (Watt)</summary>
        float MaxPowerWatt { get; set; }

        /// <summary>준비 상태</summary>
        bool IsReady { get; }

        /// <summary>출사중 여부</summary>
        bool IsBusy { get; }

        /// <summary>알람 발생 여부</summary>
        bool IsError { get; }

        /// <summary>레이저 제어에 필요한 IRtc 인터페이스</summary>
        IRtc Rtc { get; set; }

        /// <summary>사용자 정의 데이타</summary>
        object Tag { get; set; }

        /// <summary>초기화 (통신 등)</summary>
        /// <returns></returns>
        bool Initialize();

        /// <summary>출사 중지</summary>
        /// <returns></returns>
        bool CtlAbort();

        /// <summary>리셋 (에러 상태 해제 시도)</summary>
        /// <returns></returns>
        bool CtlReset();

        /// <summary>파워 변경 (즉시 명령)</summary>
        /// <param name="watt">출력 에너지(Watt)</param>
        /// <returns></returns>
        bool CtlPower(float watt);

        /// <summary>
        /// 파워 변경 (RTC제어기의 리스트 버퍼에 기록되어 실행되는 명령)
        /// 마커의 내부 쓰레드에 의해 호출됨
        /// </summary>
        /// <param name="watt">출력 에너지(Watt)</param>
        /// <returns></returns>
        bool ListPower(float watt);
    }
}
