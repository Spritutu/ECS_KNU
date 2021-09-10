
using System;

namespace Scanlab.Sirius
{
    /// <summary>RTC5 외부 트리거 모드 비트 플래그용</summary>
    public sealed class Rtc5ExternalControlMode : IRtcExternalControlMode
    {
        /// <summary>4바이트 값</summary>
        public Rtc5ExternalControlMode.Bit Value { get; set; }

        /// <summary>비트 설정</summary>
        /// <param name="flag">비트</param>
        public void Add(Rtc5ExternalControlMode.Bit flag) => this.Value |= flag;

        /// <summary>비트 해제</summary>
        /// <param name="flag">비트</param>
        public void Remove(Rtc5ExternalControlMode.Bit flag) => this.Value &= ~flag;

        /// <summary>비트값 설정 여부</summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool Contains(Rtc5ExternalControlMode.Bit flag) => Convert.ToBoolean((object)(this.Value & flag));

        /// <summary>4바이트 uint 으로 변환</summary>
        /// <returns></returns>
        public uint ToUInt() => (uint)this.Value;

        /// <summary>기본값(0) 객체 생성</summary>
        public static Rtc5ExternalControlMode Empty => new Rtc5ExternalControlMode();

        [Flags]
        public enum Bit : uint
        {
            /// <summary>
            /// = 1: The external start input (by /START, /START2 or /Slave-START) is enabled.
            /// = 0: The external start input is disabled.
            /// </summary>
            ExternalStart = 1,
            /// <summary>
            /// = 1: An external list stop (/STOP, /STOP2, /Slave-STOP or simulate_ext_stop) causes explicit cancellation of the external start queue’s entries (/START, /START2, /Slave-START or simulate_ext_start).
            /// = 0: No effect.
            /// </summary>
            ExternalStop = 2,
            /// <summary>
            /// = 1: The track delay (defined by simulate_ext_start, set_ext_start_delay or set_ext_start_delay_list) that postpones execution of the list start relative to the triggering input signal or simulate_ext_start or simulate_ext_start_ctrl command (see ”External List Start”, page 240) is deactivated.
            /// = 0: No effect.To define and activate the track delay (e.g. for Processing-on-the-fly applications), use the command simulate_ext_start, set_ext_start_delay or set_ext_start_delay_list.
            /// </summary>
            TrackDelay = 4,
            /// <summary>
            /// = 1: The external start input is not disabled by an external stop request.
            /// = 0: The external start input is disabled by an external stop request
            /// </summary>
            ExternalStartAgain = 8,
            /// <summary>Disables simulate_ext_start_ctrl.</summary>
            DisableSimulateExtCtrl = 16, // 0x00000010
            /// <summary>
            /// 엔코더 리셋(초기화) 여부
            /// = 1 Encoder resets of the two internal encoder counters (initiated by the Processing-on-the-fly commands set_fly_x, set_fly_y or set_fly_rot) occur after the subsequent start trigger(i.e.the subsequent external start signal or simulate_ext_start or simulate_ext_start_ctrl command, possibly postponed by a track delay defined by simulate_ext_start, set_ext_start_delay or set_ext_start_delay_list, see also bit #2).
            /// = 0: Encoder resets occur immediately with each initiating Processing-on-the-fly command.
            /// </summary>
            EncoderReset = 512, // 0x00000200
            /// <summary>
            /// = 1: Track delay configured by simulate_ext_start, set_ext_start_delay or set_ext_start_delay_list is counted beginning with the most recent externally (but not with execute_list_pos etc.) triggered or simulated external list start.The interval between subsequent external list starts (in encoder pulses) is thus constant(see also page 242). For stop_execution or an external stop signal, bit #10 gets reset to “0”. This bit has no effect if the firmware version is 506 or lower(see get_rtc_version).
            /// = 0: Track delay configured by simulate_ext_start, set_ext_start_delay or set_ext_start_delay_list is counted beginning with the time point an external list start was requested(i.e.with the corresponding simulate_ext_start or simulate_ext_start_ctrl command or external start signal). The interval between subsequent external list starts (in encoder pulses) can thus vary.This is standard for firmware version 506 or lower(see get_rtc_version).
            /// </summary>
            TrackDelayConfig = 1024, // 0x00000400
        }
    }
}
