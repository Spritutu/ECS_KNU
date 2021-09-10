
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 마커 인자 인터페이스
    /// 가공 인자 전달용
    /// </summary>
    public interface IMarkerArg : ICloneable
    {
        /// <summary>가공 데이타 객체</summary>
        IDocument Document { get; set; }

        /// <summary>가공 대상에 대한 옵션</summary>
        MarkTargets MarkTargets { get; set; }

        /// <summary>
        /// MarkTargets 을 SelectedButBoundRect 사용할 경우만 사용됨
        /// 외곽 영역을 반복 가공할 회수를 지정  (미지정시 1회만 가공됨)
        /// </summary>
        uint RepeatSelectedButBoundRect { get; set; }

        /// <summary>
        /// External /START 사용여부
        /// 사용시 : 레이어는 한개(단일)만 사용가능하며, 가공 시작시 데이타만 RTC 버퍼에 모두 저장된후 곧바로 OnFinished 이벤트가 호출된다. 이때 IRtcExtension 의 CtlExternalControl 함수 이용해 사용 옵션사항들을 설정해 주어야 한다
        /// </summary>
        bool IsExternalStart { get; set; }

        /// <summary>RTC 객체</summary>
        IRtc Rtc { get; set; }

        /// <summary>Rtc 내부 버퍼 처리 방식 (기본값 : Single)</summary>
        ListType RtcListType { get; set; }

        /// <summary>Z 축 제어를 위한 모터 인터페이스 (레이어별 높이 제어후 가공 방식을 사용할 경우 지정)</summary>
        IMotor MotorZ { get; set; }

        /// <summary>레이저 객체</summary>
        ILaser Laser { get; set; }

        /// <summary>동일한 형상(Doc)을 여러번 오프셋으로 가공할때의 위치 정보</summary>
        List<Offset> Offsets { get; set; }

        /// <summary>
        /// 가공중 펜 개체의 스택
        /// 이전 펜의 가공 파라메터로 복귀하는 기능을 사용할 경우 펜 개체를 추적하기 위한 스택 자료구조
        /// </summary>
        ConcurrentStack<IPen> PenStack { get; }

        /// <summary>가공 시작 시간</summary>
        DateTime StartTime { get; set; }

        /// <summary>가공 완료 시간</summary>
        DateTime EndTime { get; set; }

        /// <summary>가공 진행율 (0~100%)</summary>
        double Progress { get; set; }

        /// <summary>가공 성공적으로 완료 여부</summary>
        bool IsSuccess { get; set; }

        /// <summary>사용자 정의 데이타</summary>
        object Tag { get; set; }
    }
}
