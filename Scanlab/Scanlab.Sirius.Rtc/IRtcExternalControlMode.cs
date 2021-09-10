

namespace Scanlab.Sirius
{
    /// <summary>RTC 외부 제어 모드 값 (Rtc4,5 의 비트 플래그가 다르므로 각 버전별 상속 구현)</summary>
    public interface IRtcExternalControlMode
    {
        /// <summary>비트 구조체를 32 비트값으로 변환</summary>
        /// <returns></returns>
        uint ToUInt();
    }
}
