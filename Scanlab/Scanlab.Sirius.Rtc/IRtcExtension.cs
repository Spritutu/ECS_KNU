
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>RTC 확장 기능 인터페이스</summary>
    public interface IRtcExtension
    {
        /// <summary>FPK(First Pulse Killer) 시간값 설정</summary>
        /// <param name="usec">usec</param>
        /// <returns></returns>
        bool CtlFirstPulseKiller(float usec);

        /// <summary>레이저 출력 신호 레벨 설정</summary>
        /// <param name="laserControlSignal">RTC 모델에 맞는 ILaserControlSignal 구현된 인스턴스 (Rtc5LaserControlSignal, Rtc6LaserControlSignal)</param>
        /// <returns></returns>
        bool CtlLaserSignalLevel(ILaserControlSignal laserControlSignal);

        /// <summary>외부 트리거 사용시 설정</summary>
        /// <param name="mode">RTC (RTC5,6 경우 15핀) /START, /STOP 등의 트리거 사용여부 설정</param>
        /// <param name="maxStartCounts">/START 트리거 최대 허용 개수 설정</param>
        /// <returns></returns>
        bool CtlExternalControl(IRtcExternalControlMode mode, uint maxStartCounts = 0);

        /// <summary>외부 /START 실행된 회수 조회</summary>
        /// <param name="counts">회수값</param>
        /// <returns></returns>
        bool CtlExternalStartCounts(out uint counts);

        /// <summary>FPK(First Pulse Killer) 시간값 설정</summary>
        /// <param name="usec">usec</param>
        /// <returns></returns>
        bool ListFirstPulseKiller(float usec);

        /// <summary>
        /// 리스트 명령 - 레이저 가감속 구간의 모션 지연으로 인한 레이저 펄스의 중첩을 예방하기 위한 sky-writing 모드 사용
        /// </summary>
        /// <param name="laserOnShift">usec</param>
        /// <param name="timeLag">usec</param>
        /// <param name="angularLimit">활성화될 각도 설정 (예: 90도)</param>
        /// <returns></returns>
        bool ListSkyWriting(float laserOnShift, float timeLag, float angularLimit);

        /// <summary>리스트 명령 - 레스터 처리 (Pixel Raster Operation)</summary>
        /// <param name="usec">매 픽셀의 주기 시간 (usec) : 가공 속도를 결정</param>
        /// <param name="vDelta">픽셀간 간격 (dx, dy) (mm)</param>
        /// <param name="pixelCount">한줄을 구성하는 픽셀의 개수</param>
        /// <param name="ext">아나로그 1 or 2 반드시 선택</param>
        /// <returns></returns>
        bool ListPixelLine(float usec, Vector2 vDelta, uint pixelCount, ExtensionChannel ext = ExtensionChannel.ExtAO2);

        /// <summary>
        /// 리스트 명령 - 개별 픽셀 명령
        /// 반드시 ListPixelLine 명령이 호출된후에 픽셀 개수만큼의 ListPixel 함수가 호출되어야 함
        /// </summary>
        /// <param name="usec">현재 픽셀의 출력 주기(lower than usec in ListPixelLine ) </param>
        /// <param name="weight">ExtensionChannel 출력의 가중치 값(0~1)</param>
        /// <param name="compensator">아나로그 출력값 보정기 사용시 지정</param>
        /// <returns></returns>
        bool ListPixel(float usec, float weight = 0.0f, ICompensator<float> compensator = null);

        /// <summary>리스트 명령 - 위 ListPixelLine + ListPixel * n 을 통합한 편이용 함수</summary>
        /// <param name="vStart">가공 시작점</param>
        /// <param name="vEnd">가공 끝점</param>
        /// <param name="period">픽셀 주기 (usec)</param>
        /// <param name="usecValues">매 픽셀 가공 시간 배열</param>
        /// <param name="ext">아나로그 확장 출력 1,2 지정</param>
        /// <param name="analogValues">아나로그 값(0~10) 배열</param>
        /// <param name="usecCompensator">픽셀 출력시간값 보정기 사용시</param>
        /// <param name="analogCompensator">아나로그 출력 보정기 사용시</param>
        /// <returns></returns>
        bool ListPixels(
          Vector2 vStart,
          Vector2 vEnd,
          float period,
          float[] usecValues,
          ExtensionChannel ext = ExtensionChannel.ExtAO1,
          float[] analogValues = null,
          ICompensator<float> usecCompensator = null,
          ICompensator<float> analogCompensator = null);

        /// <summary>리스트 명령 - 와블 (Wobbel Operation)</summary>
        /// <param name="amplitudeX">size of W (parallel movement) (mm)</param>
        /// <param name="amplitudeY">size of Y (perpendicular movement) (mm)</param>
        /// <param name="frequencyHz">초당 반복회수 (Hz)</param>
        /// <returns></returns>
        bool ListWobbel(float amplitudeX, float amplitudeY, float frequencyHz);
    }
}
