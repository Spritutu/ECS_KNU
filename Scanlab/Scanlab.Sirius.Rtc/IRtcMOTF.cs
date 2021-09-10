
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>RTC MOTF 인터페이스</summary>
    public interface IRtcMOTF
    {
        /// <summary>
        /// mm 당 엔코더 X 의 펄스 개수
        /// 
        ///                       counts
        /// EncXCountsPerMm =   -----------
        ///                         mm
        /// * 주의 사항 (한축의 엔코더만 사용할 경우에도 임의의 값을 설정하는것을 추천)
        /// </summary>
        int EncXCountsPerMm { get; set; }

        /// <summary>
        /// mm 당 엔코더 Y 의 펄스 개수
        /// 
        ///                       counts
        /// EncYCountsPerMm =   -----------
        ///                         mm
        /// * 주의 사항 (한축의 엔코더만 사용할 경우에도 임의의 값을 설정하는것을 추천)
        /// </summary>
        int EncYCountsPerMm { get; set; }

        /// <summary>입력 엔코더의 초기화 (오프셋값 Dx, Dy를 설정 가능)</summary>
        /// <param name="offsetX">X 초기화 위치 (mm)</param>
        /// <param name="offsetY">Y 초기화 위치 (mm)</param>
        /// <returns></returns>
        bool CtlEncoderReset(float offsetX = 0.0f, float offsetY = 0.0f);

        /// <summary>외부 엔코더 입력 대신 내부 가상 엔코더를 활성화 및 가상 입력 엔코더 속도 지정</summary>
        /// <param name="encXSimulatedSpeed">RTC 내부 가상 엔코더X 속도 (mm/s)</param>
        /// <param name="encYSimulatedSpeed">RTC 내부 가상 엔코더Y 속도 (mm/s)</param>
        /// <returns></returns>
        bool CtlEncoderSpeed(float encXSimulatedSpeed, float encYSimulatedSpeed);

        /// <summary>현재 엔코더 값 얻기</summary>
        /// <param name="encX">X 엔코더 값 (counts)</param>
        /// <param name="encY">Y 엔코더 값 (counts)</param>
        /// <param name="encXmm">X 엔코더의 위치 (mm)</param>
        /// <param name="encYmm">Y 엔코더의 위치 (mm)</param>
        /// <returns></returns>
        bool CtlGetEncoder(out int encX, out int encY, out float encXmm, out float encYmm);

        /// <summary>
        /// 트래킹 에러 보상
        /// (추천 : 스캔 헤드의 메뉴얼에 명기된 Tracking Error 시간을 지정)
        /// </summary>
        /// <param name="xUsec">X 축 보상 시간(usec)</param>
        /// <param name="yUsec">Y축 보상 시간(usec)</param>
        /// <returns></returns>
        bool CtlTrackingError(uint xUsec, uint yUsec);

        /// <summary>
        /// 엔코더 테이블 보정 파일 로드
        /// 보정 테이블0 번의 포맷 예 :
        /// [Fly2DTable0]
        /// Encoder0 Encoder1 Encoder0_Delta Encoder1_Delta ;주석
        /// ...
        /// 추신) 모든 위치값은 bits 이므로 mm * kFactor 하여 bits 값을 구할것.
        /// 추신) 모든 bits 값은 +-524288 을 초과하지 말것
        /// </summary>
        /// <param name="fileName">보정 파일 이름 (경로포함), null 지정시 보정 리셋됨</param>
        /// <param name="tableNo">테이블 번호</param>
        /// <returns></returns>
        bool CtlMotfCompensateTable(string fileName, uint tableNo = 0);

        /// <summary>외부 트리거 시작 (External Start) 사용시 지연 설정</summary>
        /// <param name="enc">x/Y 엔코더 종류</param>
        /// <param name="distance">엔코더 지연 거리 (mm)</param>
        /// <returns></returns>
        bool CtlExternalControlDelay(RtcEncoder enc, float distance);

        /// <summary>
        /// 리스트 명령 - MOTF 리스트 명령 시작
        /// 엔코더 값 초기화시에는 CtlEncoderReset에서 설정한 오프셋 값으로 초기화되며,
        /// 초기화를 하지 않더라도 ListBegin 시에는 외부 트리거 (/START)를 사용가능하도록 설정하기 때문에
        /// 해당 트리거 신호가 활성화(Closed)되면 엔코더가 자동으로 리셋(초기화) 되도록 설정됨
        /// </summary>
        /// <param name="encoderReset">엔코더 X,Y 초기화 여부 (</param>
        /// <returns></returns>
        bool ListMOTFBegin(bool encoderReset = false);

        /// <summary>리스트 명령 - 외부 트리거 시작 (External Start) 사용시 지연 설정</summary>
        /// <param name="enc">x/Y 엔코더 종류</param>
        /// <param name="distance">엔코더 지연 거리</param>
        /// <returns></returns>
        bool ListExternalControlDelay(RtcEncoder enc, float distance);

        /// <summary>
        /// 리스트 명령 - 지정된 엔코더 단축(X 혹은 Y)의 위치가 특정 조건을 만족할때까지 리스트 명령 대기
        /// (단축 동기화 용)
        /// </summary>
        /// <param name="enc">엔코더 축 지정</param>
        /// <param name="position">위치값 (mm)</param>
        /// <param name="cond">대기 조건</param>
        /// <returns></returns>
        bool ListMOTFWait(RtcEncoder enc, float position, EncoderWaitCondition cond);

        /// <summary>리스트 명령 - 두개의 엔코더가 (X, Y)가 특정 조건이 될때 까지 대기 (다축 동기화 용)</summary>
        /// <param name="positionX">X 축 위치 (mm)</param>
        /// <param name="rangeX">조건 범위 (mm)</param>
        /// <param name="positionY">Y 축 위치 (mm)</param>
        /// <param name="rangeY">조건 범위 (mm)</param>
        /// <returns></returns>
        bool ListMOTFWaits(float positionX, float rangeX, float positionY, float rangeY);

        /// <summary>
        /// 리스트 명령 - MOTF 로 동작하는 리스트 명령 끝
        /// MOTF 종료시 스캐너를 지정된 위치로 점프 가능
        /// </summary>
        /// <param name="vPosition">점프 위치 (x,y) (mm)</param>
        /// <returns></returns>
        bool ListMOTFEnd(Vector2 vPosition);
    }
}
