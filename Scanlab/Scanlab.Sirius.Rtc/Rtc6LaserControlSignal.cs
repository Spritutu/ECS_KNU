
using System;

namespace Scanlab.Sirius
{
    /// <summary>RTC6 용 레이저 제어 신호 비트 플래그용</summary>
    public class Rtc6LaserControlSignal : ILaserControlSignal
    {
        /// <summary>4바이트 값</summary>
        public Rtc6LaserControlSignal.Bit Value { get; set; }

        /// <summary>비트 설정</summary>
        /// <param name="flag">비트</param>
        public void Add(Rtc6LaserControlSignal.Bit flag) => this.Value |= flag;

        /// <summary>비트 해제</summary>
        /// <param name="flag">비트</param>
        public void Remove(Rtc6LaserControlSignal.Bit flag) => this.Value &= ~flag;

        /// <summary>비트값 설정 여부</summary>
        /// <param name="flag"></param>
        /// <returns></returns>
        public bool Contains(Rtc6LaserControlSignal.Bit flag) => Convert.ToBoolean((object)(this.Value & flag));

        /// <summary>4바이트 uint 으로 변환</summary>
        /// <returns></returns>
        public uint ToUInt() => (uint)this.Value;

        /// <summary>기본값(0) 객체 생성</summary>
        public static Rtc6LaserControlSignal Empty => new Rtc6LaserControlSignal();

        [Flags]
        public enum Bit : uint
        {
            /// <summary>
            /// Pulse Switch Setting (does not concern Laser Mode 4 or Laser Mode 6).
            /// The setting only affects those laser control signals (more precisely: those LASER1 or LASER2 “laser active” modulation pulses in CO2 Mode or LASER1 Q-Switch pulses in the YAG modes) that are not yet fully processed at completion of the LASERON signal.
            /// = 0: The signals are cut off at the end of the LASERON signal.
            /// = 1: The final pulse fully executes despite completion of the LASERON signal.
            /// </summary>
            PulseSwitchSetting = 1,
            /// <summary>
            /// Phase shift of the laser control signals (does not concern Laser Mode 4 or Laser Mode 6).
            /// = 0: No phase shift.
            /// = 1: CO2 Mode: The LASER1 signal is exchanged with the LASER2 signal. YAG modes: The LASER1 is shifted back 180° (half a signal period).
            /// </summary>
            PhaseShift = 2,
            /// <summary>
            /// Enabling or disabling of laser control signals for “Laser active” operation.
            /// = 0: The “Laser active” laser control signals are enabled.
            /// = 1: The “Laser active” laser control signals are disabled (then the laser output ports are in the high impedance tristate mode).
            /// </summary>
            DisableLaserActiveSignal = 4,
            /// <summary>
            ///  LASERON signal level.
            /// = 0: The signal at the LASERON port is set to active-HIGH.
            /// = 1: The signal at the LASERON port is set to active-LOW.
            /// </summary>
            LaserOnSignalLevelLow = 8,
            /// <summary>
            ///  LASER1/LASER2 signal level.
            /// = 0: The signals at the LASER1 and LASER2 output ports are set to active-HIGH.
            /// = 1: The signals at the LASER1 and LASER2 output ports are set to active-LOW
            /// </summary>
            Laser12SignalLevelLow = 16, // 0x00000010
            /// <summary>
            /// Determines for laser_on_pulses_list whether external signal pulses (at the
            /// LASER connector’s DIGITAL IN1 digital input) are to be counted at rising or falling edges:
            /// = 0: At the falling edge.
            /// = 1: At the rising edge.
            /// </summary>
            ExtPulseSignalRisingEdge = 32, // 0x00000020
            /// <summary>
            /// = 0: Output synchronization is switched off (default setting).
            /// = 1: Output synchronization is switched on.
            /// </summary>
            OutputSynchronization = 64, // 0x00000040
            /// <summary>
            /// = 0: The “constant pulse length” mode is switched off (default setting).
            /// = 1: The “constant pulse length” mode is switched on.
            /// </summary>
            ConstantLaserPulseLength = 128, // 0x00000080
            /// <summary>
            /// = 1: In case of error, automatic monitoring (automatic suppression of laser control signals) automatically generates a /STOP signal (list stops, laser control signals get permanently switched off).
            /// </summary>
            AutomaticMonitoringToStop = 268435456, // 0x10000000
            /// <summary>
            /// = 1: In case of error according to Bit #28, the stop_execution is forwarded as /Master-STOP
            /// </summary>
            StopToAllMasterSlave = AutomaticMonitoringToStop, // 0x10000000
        }
    }
}
