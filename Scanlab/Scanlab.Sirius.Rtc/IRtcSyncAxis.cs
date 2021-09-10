
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>RTC SYNCAXIS 인터페이스</summary>
    public interface IRtcSyncAxis
    {
        /// <summary>Stage Move 시 사용할 속도 (초기설정값 : 10mm/s)</summary>
        float StageMoveSpeed { get; set; }

        /// <summary>
        /// Stage Move 시 사용할 시간제한 (초)
        /// 기본 5초
        /// </summary>
        float StageMoveTimeOut { get; set; }

        /// <summary>
        /// LPF (Low Pass Filter) 주파수
        /// 초기값은 XML 설정 파일에 있는 값
        /// </summary>
        float BandWidth { get; }

        /// <summary>
        /// LPF(Low Pass Filter) 주파수를 설정한다
        /// (xml 설정 파일에 저장 되지 않음)
        /// </summary>
        /// <param name="filterBandWidth">Hz</param>
        /// <returns></returns>
        bool CtlBandWidth(float filterBandWidth);

        /// <summary>스테이지 혹은 스캐너 를 수동 이동할때 사용</summary>
        /// <param name="motionType">모션 종류</param>
        /// <param name="position">x,y 위치 (mm)</param>
        /// <returns></returns>
        bool CtlMove(MotionType motionType, Vector2 position);

        /// <summary>스테이지 혹은 스캐너 를 수동 이동할때 사용</summary>
        /// <param name="motionType">모션 종류</param>
        /// <param name="x">X 위치 (mm)</param>
        /// <param name="y">Y 위치 (mm)</param>
        /// <returns></returns>
        bool CtlMove(MotionType motionType, float x, float y);

        /// <summary>멀티 헤드 사용시 개별 헤드에 대한 오프셋, 회전 처리</summary>
        /// <param name="scanDevice">ScanDevice 열거형</param>
        /// <param name="offset">dx, dy 이동량 (mm)</param>
        /// <param name="angle">회전량 (각도) </param>
        /// <returns></returns>
        bool CtlHeadOffset(ScanDevice scanDevice, Vector2 offset, float angle);

        /// <summary>
        /// 리스트 명령 시작 - 버퍼 준비
        /// syncAxis 는 버퍼 처리 방식이 전혀 다르므로  IRtc 인터페이스의 ListBegin 를 사용하지 않고, 전용의 ListBegin 을 지원한다
        /// 내부적으로는 slsc_ListHandlingMode_RepeatWhileBufferFull 방식으로 고정된다.
        /// 주의사항 : 버퍼가 고갈되지 않도록 지속적으로 리스트 명령을 삽입해야 주어야 한다. 그렇지 않으면 버퍼 고갈(buffer underrun) 이 발생된다.
        /// 때문에 리스트 데이타를 넣는동안 디버깅(디버깅 중지) 할때는 매우 조심해야 한다
        /// </summary>
        /// <param name="laser">ILaser 인터페이스</param>
        /// <param name="motionType">MotionType 열거형</param>
        /// <returns></returns>
        bool ListBegin(ILaser laser, MotionType motionType = MotionType.ScannerOnly);
    }
}
