
using System;

namespace Scanlab.Sirius
{
    /// <summary>RTC4 외부 트리거 모드 비트 플래그용</summary>
    public sealed class Rtc4ExternalControlMode : IRtcExternalControlMode
    {
        /// <summary>4바이트 값</summary>
        public Rtc4ExternalControlMode.Bit Value { get; set; }

        /// <summary>비트 설정</summary>
        /// <param name="flag">비트</param>
        public void Add(Rtc4ExternalControlMode.Bit flag) => this.Value |= flag;

        /// <summary>비트 해제</summary>
        /// <param name="flag">비트</param>
        public void Remove(Rtc4ExternalControlMode.Bit flag) => this.Value &= ~flag;

        /// <summary>비트값 설정 여부</summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool Contains(Rtc4ExternalControlMode.Bit flag) => Convert.ToBoolean((object)(this.Value & flag));

        /// <summary>4바이트 uint 으로 변환</summary>
        /// <returns></returns>
        public uint ToUInt() => (uint)this.Value;

        /// <summary>기본값(0) 객체 생성</summary>
        public static Rtc4ExternalControlMode Empty => new Rtc4ExternalControlMode();

        [Flags]
        public enum Bit : uint
        {
            /// <summary>
            /// /START 핀 사용
            /// = 1 The external start input is enabled. The externals tart signal corresponds to the command execute_list_1 or execute_list_1. The external stop signal corresponds to the command stop_execution.
            /// = 0 no external start signal
            /// </summary>
            ExternalStart = 1,
            /// <summary>
            /// = 1 The external start delay (encoder delay) is turned off.
            /// = 0 No effect
            /// </summary>
            ExternalStartDelayOff = 4,
            /// <summary>
            /// = 1 The external start input is not disabled by an external stop request
            /// = 0 The external start input is disabled by an external stop request
            /// </summary>
            ExternalStartAgain = 8,
        }
    }
}
