using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>자동 레이저 제어 모드</summary>
    public enum AutoLaserControlMode
    {
        /// <summary>비활성화</summary>
        Disabled = 0,
        /// <summary>스캐너 명령 속도</summary>
        SetVelocity = 1,
        /// <summary>스캐너 실제 속도 (iDRIVE 스캐너만 사용가능)</summary>
        ActualVelocity = 2,
        /// <summary>외부 엔코더 입력 속도</summary>
        EncoderSpeed = 5,
        /// <summary>ActualVelocity + EncoderSpeed 복합 속도</summary>
        ActualVelocityAndEncoderSpeed = 6,
    }

    /// <summary>자동 레이저 제어용 신호 종류</summary>
    public enum AutoLaserControlSignal
    {
        /// <summary>비활성화</summary>
        Disabled,
        /// <summary>
        /// 아나로그1 출력 (~10V)
        /// T 타입은 float
        /// </summary>
        Analog1,
        /// <summary>
        /// 아나로그2 출력 (~10V)
        /// T 타입은 float
        /// </summary>
        Analog2,
        /// <summary>
        /// 확장 8비트 DO 출력 (0~255)
        /// T 타입은 uint
        /// </summary>
        ExtDO8Bit,
        /// <summary>
        /// 펄스폭 출력 (usec)
        /// T 타입은 float
        /// </summary>
        PulseWidth,
        /// <summary>
        /// 주파수 출력 (Hz)
        /// T 타입은 float
        /// </summary>
        Frequency,
        /// <summary>
        /// 확장 16비트 DO 출력 (0~65535)
        /// T 타입은 uint
        /// </summary>
        ExtDO16,
    }

    /// <summary>모션 계산 상태</summary>
    internal enum CalculationStatus
    {
        Unknown,
        Start,
        InProgress,
        Finished,
    }

    /// <summary>문자 집합 (최대 4개까지 지원)</summary>
    public enum CharacterSet
    {
        /// <summary>0</summary>
        _0,
        /// <summary>1</summary>
        _1,
        /// <summary>2</summary>
        _2,
        /// <summary>3</summary>
        _3,
    }

    /// <summary>RTC 카드 내에는 4개의 스캐너 보정 테이블을 저장해 놓고 추후 사용 가능</summary>
    public enum CorrectionTableIndex
    {
        None = -1, // 0xFFFFFFFF
        Table1 = 0,
        Table2 = 1,
        Table3 = 2,
        Table4 = 3,
    }

    /// <summary>날짜 포맷</summary>
    public enum DateFormat
    {
        /// <summary>마지막 2 자리 연도 표기</summary>
        Year2Digits = 0,
        /// <summary>날짜</summary>
        Day = 2,
        /// <summary>4 자리 연도 표기</summary>
        Year4Digits = 5,
        /// <summary>월 표기</summary>
        MonthDigit = 6,
    }

    /// <summary>엔코더 대기 조건</summary>
    public enum EncoderWaitCondition
    {
        /// <summary>입력 엔코더 값이 지정된 값보다 작아질 때까지 대기</summary>
        Under = -1, // 0xFFFFFFFF
        /// <summary>
        /// 자동 = 호출될 당시의 입력 엔코더 위치를 기준으로 자동으로 판단 (리스트 명령이 추후 실행되고, 이때의 기구 상태에 따라 가변적임)
        /// </summary>
        Auto = 0,
        /// <summary>입력 엔코더 값이 지정된 값 보다 커질 때까지 대기</summary>
        Over = 1,
    }

    /// <summary>모션 실행 상태</summary>
    internal enum ExecutionStatus
    {
        Unknown,
        Executing,
        Finished,
    }

    /// <summary>RTC 카드의 확장 IO 포트 종류</summary>
    public enum ExtensionChannel
    {
        ExtDI2,
        ExtDO2,
        ExtDO8,
        ExtDO16,
        ExtDI16,
        ExtAO1,
        ExtAO2,
    }

    /// <summary>
    /// 레이저 핀의 펄스 모드 (LASER1, LASER2 output pulse timing methods)
    /// rf. Scanlab's Manual Doc
    /// </summary>
    public enum LaserMode
    {
        Co2 = 0,
        None = 0,
        Yag1 = 1,
        Yag2 = 2,
    }

    /// <summary>리스트 타입</summary>
    public enum ListType
    {
        /// <summary>
        /// single buffered list
        /// 단일한 리스트 버퍼 사용
        /// </summary>
        Single,
        /// <summary>
        /// double buffered list (auto)
        /// 자동 리스트 버퍼 사용 (예를 들어 두개의 리스트 버퍼를 번갈아 가며 사용하는 등 내부 처리됨)
        /// </summary>
        Auto,
    }

    /// <summary>Measurement 관련 채널 목록</summary>
    public enum MeasurementChannel
    {
        LaserOn = 0,
        StatusAX = 1,
        StatusAY = 2,
        StatusBX = 4,
        StatusBY = 5,
        SampleX = 7,
        SampleY = 8,
        SampleZ = 9,
        SampleAX_Coor = 10, // 0x0000000A
        SampleAY_Coor = 11, // 0x0000000B
        SampleAZ_Coor = 12, // 0x0000000C
        SampleBX_Coor = 13, // 0x0000000D
        SampleBY_Coor = 14, // 0x0000000E
        SampleBZ_Coor = 15, // 0x0000000F
        StatusAX_LaserOn = 16, // 0x00000010
        StatusAY_LaserOn = 17, // 0x00000011
        StatusBX_LaserOn = 18, // 0x00000012
        StatusBY_LaserOn = 19, // 0x00000013
        SampleAX_Out = 20, // 0x00000014
        SampleAY_Out = 21, // 0x00000015
        SampleBX_Out = 22, // 0x00000016
        SampleBY_Out = 23, // 0x00000017
        AutomaticLaserControlParam = 24, // 0x00000018
        SampleAX_Trans = 25, // 0x00000019
        SampleAY_Trans = 26, // 0x0000001A
        SampleAZ_Trans = 27, // 0x0000001B
        SampleBX_Trans = 28, // 0x0000001C
        SampleBY_Trans = 29, // 0x0000001D
        SampleBZ_Trans = 30, // 0x0000001E
        AutomaticLaserControlByVectorParam = 31, // 0x0000001F
        FocusShift = 32, // 0x00000020
        ExtAO1 = 33, // 0x00000021
        ExtAO2 = 34, // 0x00000022
        ExtDO16 = 35, // 0x00000023
        ExtDO8 = 36, // 0x00000024
        PulseLength = 37, // 0x00000025
        OutputPeroid = 38, // 0x00000026
        FreeVariable0 = 39, // 0x00000027
        FreeVariable1 = 40, // 0x00000028
        FreeVariable2 = 41, // 0x00000029
        FreeVariable3 = 42, // 0x0000002A
        Enc0Counter = 43, // 0x0000002B
        Enc1Counter = 44, // 0x0000002C
        MarkSpeed = 45, // 0x0000002D
        ExtDI16 = 46, // 0x0000002E
        ZoomValueForIntelliWeld = 47, // 0x0000002F
        FreeVariable4 = 48, // 0x00000030
        FreeVariable5 = 49, // 0x00000031
        FreeVariable6 = 50, // 0x00000032
        FreeVariable7 = 51, // 0x00000033
        TimeStampCounter = 52, // 0x00000034
        WobbelAmplitude = 53, // 0x00000035
        ExtAI = 54, // 0x00000036
    }

    /// <summary>모션 방식 (SYNCAXIS 기반의 MOTF 를 위한 내용)</summary>
    public enum MotionType
    {
        /// <summary>스캐너 단독 (일반적인 경우)</summary>
        ScannerOnly,
        /// <summary>스태이지 단독 (Route)</summary>
        StageOnly,
        /// <summary>스캐너 + 스테이지 (MOTF/Marking On the flying)</summary>
        StageAndScanner,
    }

    /// <summary>RTC 엔코더 종류</summary>
    public enum RtcEncoder
    {
        /// <summary>Enc0 = X</summary>
        EncX,
        /// <summary>Enc1 = Y</summary>
        EncY,
    }

    /// <summary>RTC 상태 확인용</summary>
    public enum RtcStatus
    {
        /// <summary>가공중</summary>
        Busy,
        /// <summary>가공중이 아님</summary>
        NotBusy,
        /// <summary>리스트 1번이 사용됨 (리스트 모드가 auto 일 경우 내부적으로 사용됨)</summary>
        List1Busy,
        /// <summary>리스트 2번이 사용됨 (리스트 모드가 auto 일 경우 내부적으로 사용됨)</summary>
        List2Busy,
        /// <summary>에러 발생 여부</summary>
        NoError,
        /// <summary>가공중 강제 종료 여부</summary>
        Aborted,
        /// <summary>스캐너의 위치 응답 오류 발생 여부</summary>
        PositionAckOK,
        /// <summary>스캔 헤드의 전원 공급 이상 여부</summary>
        PowerOK,
        /// <summary>스캔 헤드의 사용가능 온도 도달 여부</summary>
        TempOK,
    }

    /// <summary>스캔 헤드 번호 (멀티 헤드 사용시)</summary>
    public enum ScanDevice
    {
        /// <summary>1</summary>
        ScanDevice1 = 1,
        /// <summary>2</summary>
        ScanDevice2 = 2,
        /// <summary>3</summary>
        ScanDevice3 = 3,
        /// <summary>4</summary>
        ScanDevice4 = 4,
    }

    /// <summary>스캔 헤드 식별자</summary>
    public enum ScannerHead
    {
        None,
        Primary,
        Secondary,
    }

    /// <summary>시리얼 포맷</summary>
    public enum SerialFormat
    {
        /// <summary>앞을 0 으로 채우기 + 오른쪽 정렬</summary>
        LeadingWithZero,
        /// <summary>앞 채우기 없음 + 왼쪽 정렬</summary>
        NoLeadingAndLeftAligned,
        /// <summary>앞을 공백으로 채우기 + 오른쪽 정렬</summary>
        LeadingWithBlank,
    }


    /// <summary>시간 포맷</summary>
    public enum TimeFormat
    {
        /// <summary>24시간제 표기</summary>
        Hours24,
        /// <summary>분 표기</summary>
        Minutes,
        /// <summary>초 표기</summary>
        Seconds,
        /// <summary>12시간제 표기</summary>
        Hours12,
    }

    /// <summary>모션 경로 전송 상태</summary>
    internal enum TransferStatus
    {
        Unknown,
        LoadedEnough,
    }
}
