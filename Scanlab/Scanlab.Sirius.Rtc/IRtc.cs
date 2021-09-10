
using System;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>RTC 인터페이스</summary>
    public interface IRtc : IDisposable
    {
        /// <summary>RTC 카드 식별자 (0, 1, 2, ...)</summary>
        uint Index { get; set; }

        /// <summary>이름</summary>
        string Name { get; set; }

        /// <summary>k factor = bits / mm</summary>
        float KFactor { get; set; }

        /// <summary>
        /// 레이저 모드(CO2, Yag1,2,3,4, ...)
        /// Laser1,2 출력핀의 타이밍 종류 선택
        /// </summary>
        LaserMode LaserMode { get; }

        /// <summary>First Pulse Killer 신호의 시간값 (usec)</summary>
        float Fpk { get; }

        /// <summary>보정 파일 테이블 목록 (최대 4개의 테이블을 로드할수있음 : 0,1,2,3)</summary>
        string[] CorrectionFiles { get; }

        /// <summary>Primary 헤드의 보정 파일 테이블 번호</summary>
        CorrectionTableIndex PrimaryHeadTable { get; }

        /// <summary>3x3 행렬 스택 (Push/Pop 을 통해 복수개의 행렬을 스택에 저장)</summary>
        IMatrixStack MatrixStack { get; set; }

        /// <summary>MOTF(Marking on the fly) 옵션 지원 여부</summary>
        bool IsMOTF { get; }

        /// <summary>듀얼 헤드 옵션 지원 여부</summary>
        bool Is2ndHead { get; }

        /// <summary>3D 옵션 (VarioScan, Z-Shift) 옵션 지원 여부</summary>
        bool Is3D { get; }

        /// <summary>ScanAhead (for syncAxis) 옵션 지원 여부</summary>
        bool IsScanAhead { get; }

        /// <summary>UFPM(Ultra Fast Pulse Modulation) 옵션 지원 여부</summary>
        bool IsUFPM { get; }

        /// <summary>SyncAxis (XL-SCAN) 옵션 지원 여부</summary>
        bool IsSyncAxis { get; }

        /// <summary>RTC 카드 초기화</summary>
        /// <summary>RTC 카드 초기화</summary>
        /// <param name="kFactor">k factor = bits/mm</param>
        /// <param name="laserMode">LaserMode 열거형</param>
        /// <param name="ctbFileName">주 스캐너의 보정 테이블(Table1)에 Load/Select 하려는 .ct5 스캐너 보정 파일</param>
        /// <returns></returns>
        bool Initialize(float kFactor, LaserMode laserMode, string ctbFileName);

        /// <summary>보정 파일(.ct5)을 RTC 내부 메모리로 로딩</summary>
        /// <param name="tableIndex">CorrectionTableIndex 열거형 </param>
        /// <param name="ctbFileName">.ct5 스캐너 보정 파일</param>
        /// <returns></returns>
        bool CtlLoadCorrectionFile(CorrectionTableIndex tableIndex, string ctbFileName);

        /// <summary>지정된 스캐너 헤드에 보정 파일을 설정</summary>
        /// <param name="primaryHeadTableIndex">CorrectionTableIndex 열거형 (Primary)</param>
        /// <param name="secondaryHeadTableIndex">CorrectionTableIndex 열거형 (Secondary)</param>
        /// <returns></returns>
        bool CtlSelectCorrection(
          CorrectionTableIndex primaryHeadTableIndex,
          CorrectionTableIndex secondaryHeadTableIndex = CorrectionTableIndex.None);

        /// <summary>현재 설정된 주파수, 펄스폭 으로 레이저 변조 신호(LASER1,2,ON) 출력</summary>
        /// <returns></returns>
        bool CtlLaserOn();

        /// <summary>레이저 변호 신호 (LASER1,2,ON) 중단</summary>
        /// <returns></returns>
        bool CtlLaserOff();

        /// <summary>지정된 위치로 스캐너 수동 이동</summary>
        /// <param name="vPosition">X,Y (mm)</param>
        /// <returns></returns>
        bool CtlMove(Vector2 vPosition);

        /// <summary>지정된 위치로 스캐너 수동 이동</summary>
        /// <param name="x">x mm</param>
        /// <param name="y">y mm</param>
        /// <returns></returns>
        bool CtlMove(float x, float y);

        /// <summary>주파수와 펄스폭 설정</summary>
        /// <param name="frequency">주파수 (Hz)</param>
        /// <param name="pulseWidth">펄스폭 (usec)</param>
        /// <returns></returns>
        bool CtlFrequency(float frequency, float pulseWidth);

        /// <summary>스캐너/ 레이저 지연값 설정</summary>
        /// <param name="laserOn">레이저 온 지연 (usec)</param>
        /// <param name="laserOff">레이저 오프 지연 (usec)</param>
        /// <param name="scannerJump">스캐너 점프 지연 (usec)</param>
        /// <param name="scannerMark">스캐너 마크 지연 (usec)</param>
        /// <param name="scannerPolygon">스캐너 폴리곤(코너) 지연 (usec)</param>
        /// <returns></returns>
        bool CtlDelay(
          float laserOn,
          float laserOff,
          float scannerJump,
          float scannerMark,
          float scannerPolygon);

        /// <summary>스캐너 점프/마크 속도 설정</summary>
        /// <param name="jump">점프(jump) 속도 (mm/s)</param>
        /// <param name="mark">마크(mark) 및 아크(arc) 속도 (mm/s)</param>
        /// <returns></returns>
        bool CtlSpeed(float jump, float mark);

        /// <summary>확장 포트에 데이타 쓰기</summary>
        /// <typeparam name="T">값(16비트, 8비트, 2비트 (uint), 아나로그(float 10V)</typeparam>
        /// <param name="ch">확장 커넥터 종류 </param>
        /// <param name="value">uint/float</param>
        /// <param name="compensator">compensator 보정용 객체</param>
        /// <returns></returns>
        bool CtlWriteData<T>(ExtensionChannel ch, T value, ICompensator<T> compensator = null);

        /// <summary>확장1 포트의 16비트 디지털 출력의 특정 비트값을 변경</summary>
        /// <param name="bitPosition">0~15</param>
        /// <param name="onOff">출력</param>
        /// <returns></returns>
        bool CtlWriteExtDO16(ushort bitPosition, bool onOff);

        /// <summary>확장 포트에서 데이타 읽기</summary>
        /// <typeparam name="T">값(16비트, 8비트, 2비트 (uint), 아나로그(float 10V)</typeparam>
        /// <param name="ch">확장 커넥터 종류</param>
        /// <param name="value">uint/float</param>
        /// <param name="compensator">보정기</param>
        /// <returns></returns>
        bool CtlReadData<T>(ExtensionChannel ch, out T value, ICompensator<T> compensator = null);

        /// <summary>RTC 카드의 상태 확인</summary>
        /// <param name="status">RtcStatus 열거형</param>
        /// <returns></returns>
        bool CtlGetStatus(RtcStatus status);

        /// <summary>리스트 명령이 완료될 때(busy 가 해제될때) 까지 대기하는 함수</summary>
        /// <returns></returns>
        bool CtlBusyWait();

        /// <summary>실행중인 리스트 명령(busy 상태를)을 강제 종료</summary>
        /// <returns></returns>
        bool CtlAbort();

        /// <summary>에러상태를 해제</summary>
        /// <returns></returns>
        bool CtlReset();

        /// <summary>리스트 명령 시작 - 버퍼 준비</summary>
        /// <param name="laser">레이저 소스</param>
        /// <param name="listType">리스트 타입 (하나의 거대한 리스트 : single, 더블 버퍼링되는 두개의 리스트 : double)</param>
        /// <returns></returns>
        bool ListBegin(ILaser laser, ListType listType = ListType.Single);

        /// <summary>리스트 명령 - 주파수, 펄스폭</summary>
        /// <param name="frequency">주파수 (Hz)</param>
        /// <param name="pulseWidth">펄스폭 (usec)</param>
        /// <returns></returns>
        bool ListFrequency(float frequency, float pulseWidth);

        /// <summary>리스트 명령 - 지연</summary>
        /// <param name="laserOn">레이저 온 지연 (usec)</param>
        /// <param name="laserOff">레이저 오프 지연 (usec)</param>
        /// <param name="scannerJump">스캐너 점프 지연 (usec)</param>
        /// <param name="scannerMark">스캐너 마크 지연 (usec)</param>
        /// <param name="scannerPolygon">스캐너 폴리곤(코너) 지연 (usec)</param>
        /// <returns></returns>
        bool ListDelay(
          float laserOn,
          float laserOff,
          float scannerJump,
          float scannerMark,
          float scannerPolygon);

        /// <summary>리스트 명령 - 속도</summary>
        /// <param name="jump">점프(jump 속도 (mm/s)</param>
        /// <param name="mark">마크(mark/arc) 속도 (mm/s)</param>
        /// <returns></returns>
        bool ListSpeed(float jump, float mark);

        /// <summary>리스트 명령 - 시간 대기</summary>
        /// <param name="msec">시간 (msec)</param>
        /// <returns></returns>
        bool ListWait(float msec);

        /// <summary>리스트 명령 - 레이저 출사 시간</summary>
        /// <param name="msec">시간 (msec)</param>
        /// <returns></returns>
        bool ListLaserOn(float msec);

        /// <summary>리스트 명령 - 레이저 출사 시작</summary>
        /// <returns></returns>
        bool ListLaserOn();

        /// <summary>리스트 명령 - 레이저 출사 중지</summary>
        /// <returns></returns>
        bool ListLaserOff();

        /// <summary>리스트 명령 - 점프</summary>
        /// <param name="vPosition">x,y 위치 (mm)</param>
        /// <param name="rampFactor">ALC(Automatic Laser Control) 사용시 비율값</param>
        /// <returns></returns>
        bool ListJump(Vector2 vPosition, float rampFactor = 1f);

        /// <summary>리스트 명령 - 점프</summary>
        /// <param name="x">x 위치 (mm)</param>
        /// <param name="y">y 위치 (mm)</param>
        /// <param name="rampFactor">ALC(Automatic Laser Control) 사용시 비율값</param>
        /// <returns></returns>
        bool ListJump(float x, float y, float rampFactor = 1f);

        /// <summary>리스트 명령 - 마크 (Mark : 선분)</summary>
        /// <param name="vPosition">x,y 위치 (mm)</param>
        /// <param name="rampFactor">ALC(Automatic Laser Control) 사용시 비율값</param>
        /// <returns></returns>
        bool ListMark(Vector2 vPosition, float rampFactor = 1f);

        /// <summary>리스트 명령 - 마크 (Mark : 선분)</summary>
        /// <param name="x">x 위치 (mm)</param>
        /// <param name="y">y 위치 (mm)</param>
        /// <param name="rampFactor">ALC(Automatic Laser Control) 사용시 비율값</param>
        /// <returns></returns>
        bool ListMark(float x, float y, float rampFactor = 1f);

        /// <summary>리스트 명령 - 아크 (Arc : 호)</summary>
        /// <param name="vCenter">회전 중심 위치 (cx, cy)</param>
        /// <param name="sweepAngle">회전량 (+ : CCW, - : CW)</param>
        /// <returns></returns>
        bool ListArc(Vector2 vCenter, float sweepAngle);

        /// <summary>리스트 명령 - 아크 (Arc : 호)</summary>
        /// <param name="cx">회전 중심 위치 (cx)</param>
        /// <param name="cy">회전 중심 위치 (cy)</param>
        /// <param name="sweepAngle">회전량 (+ : CCW, - : CW)</param>
        /// <returns></returns>
        bool ListArc(float cx, float cy, float sweepAngle);

        /// <summary>리스트 명령 - 마크 (Ellipse : 타원)</summary>
        /// <param name="vCenter">중심</param>
        /// <param name="majorHalf">A</param>
        /// <param name="minorHalf">B</param>
        /// <param name="startAngle">시작 각도</param>
        /// <param name="sweepAngle">각도 회전량 (+ : CCW, - : CW)</param>
        /// <param name="rotateAngle">타원 자체 회전량 (+ : CCW, - : CW)</param>
        /// <returns></returns>
        bool ListEllipse(
          Vector2 vCenter,
          float majorHalf,
          float minorHalf,
          float startAngle,
          float sweepAngle,
          float rotateAngle = 0.0f);

        /// <summary>Conic 베지어 곡선</summary>
        /// <param name="vStart">시작 위치</param>
        /// <param name="vControl">제어점 위치</param>
        /// <param name="vEnd">끝 위치</param>
        /// <param name="drawLength">직선 보간 거리 (mm)</param>
        /// <returns></returns>
        bool ListConicBezier(Vector2 vStart, Vector2 vControl, Vector2 vEnd, float drawLength = 0.0f);

        /// <summary>Cubic 베지어 곡선</summary>
        /// <param name="vStart">시작 위치</param>
        /// <param name="vControl1">제어점1 위치</param>
        /// <param name="vControl2">제어점2 위치</param>
        /// <param name="vEnd">끝 위치</param>
        /// <param name="drawLength">직선 보간 거리 (mm)</param>
        /// <returns></returns>
        bool ListCubicBezier(
          Vector2 vStart,
          Vector2 vControl1,
          Vector2 vControl2,
          Vector2 vEnd,
          float drawLength = 0.0f);

        /// <summary>리스트 명령 - 확장 포트에 데이타 쓰기</summary>
        /// <param name="ch">확장 커넥터 종류</param>
        /// <param name="value">값(16비트, 8비트, 2비트(int), 아나로그(float 10V)</param>
        /// <param name="compensator">compensator 보정용 객체</param>
        /// <returns></returns>
        bool ListWriteData<T>(ExtensionChannel ch, T value, ICompensator<T> compensator = null);

        /// <summary>확장1 포트의 16비트 디지털 출력의 특정 비트값을 변경</summary>
        /// <param name="bitPosition">0~15</param>
        /// <param name="onOff">출력</param>
        /// <returns></returns>
        bool ListWriteExtDO16(ushort bitPosition, bool onOff);

        /// <summary>리스트 명령 끝 - 버퍼 닫기</summary>
        /// <returns></returns>
        /// s
        bool ListEnd();

        /// <summary>리스트 명령 실행</summary>
        /// <param name="busyWait">모든 리스트 명령의 실행이 완료될때까지 대기</param>
        /// <returns></returns>
        bool ListExecute(bool busyWait = true);
    }
}
