
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 마커 인자 전달 객체
    /// 기본 버전
    /// </summary>
    public class MarkerArgDefault : IMarkerArg, ICloneable
    {
        protected List<Offset> offsets;

        /// <summary>가공 데이타 객체</summary>
        public virtual IDocument Document { get; set; }

        /// <summary>가공 대상에 대한 옵션</summary>
        public virtual MarkTargets MarkTargets { get; set; }

        /// <summary>
        /// MarkTargets 을 SelectedButBoundRect 사용할 경우만 사용됨
        /// 외곽 영역을 반복 가공할 회수를 지정  (미지정시 1회만 가공됨)
        /// </summary>
        public virtual uint RepeatSelectedButBoundRect { get; set; }

        /// <summary>
        /// External /START 사용여부
        /// 사용시 : 레이어는 한개(단일)만 사용가능하며, 가공 시작시 데이타만 RTC 버퍼에 모두 저장된후 곧바로 OnFinished 이벤트가 호출된다. 이때 IRtcExtension 의 CtlExternalControl 함수 이용해 사용 옵션사항들을 설정해 주어야 한다
        /// </summary>
        public bool IsExternalStart { get; set; }

        /// <summary>RTC 객체</summary>
        public virtual IRtc Rtc { get; set; }

        /// <summary>Rtc 내부 버퍼 처리 방식 (기본값 : Single)</summary>
        public ListType RtcListType { get; set; }

        /// <summary>Z 축 제어를 위한 모터 인터페이스 (레이어별 높이 제어후 가공 방식을 사용할 경우 지정)</summary>
        public virtual IMotor MotorZ { get; set; }

        /// <summary>레이저 객체</summary>
        public virtual ILaser Laser { get; set; }

        /// <summary>동일한 형상(Doc)을 여러번 오프셋으로 가공할때의 위치 정보</summary>
        public virtual List<Offset> Offsets
        {
            get => this.offsets;
            set => this.offsets = value;
        }

        /// <summary>펜(IPen) 개체의 스택</summary>
        public virtual ConcurrentStack<IPen> PenStack { get; private set; }

        /// <summary>가공 시작 시간</summary>
        public virtual DateTime StartTime { get; set; }

        /// <summary>가공 완료 시간</summary>
        public virtual DateTime EndTime { get; set; }

        /// <summary>가공 진행율 (0~100%)</summary>
        public virtual double Progress { get; set; }

        /// <summary>가공 성공적으로 완료 여부</summary>
        public virtual bool IsSuccess { get; set; }

        /// <summary>사용자 정의 데이타</summary>
        public virtual object Tag { get; set; }

        /// <summary>생성자</summary>
        public MarkerArgDefault()
        {
            this.RtcListType = ListType.Auto;
            this.offsets = new List<Offset>();
            this.PenStack = new ConcurrentStack<IPen>();
            this.StartTime = DateTime.Now;
            this.MarkTargets = MarkTargets.All;
            this.RepeatSelectedButBoundRect = 1U;
        }

        /// <summary>생성자</summary>
        /// <param name="doc">문서 데이타</param>
        /// <param name="rtc">RTC 객체</param>
        /// <param name="laser">레이저 객체</param>
        /// <param name="motorZ">Z 축 제어용 모터 객체</param>
        public MarkerArgDefault(IDocument doc, IRtc rtc, ILaser laser, IMotor motorZ = null)
          : this()
        {
            this.Document = doc;
            this.Rtc = rtc;
            this.Laser = laser;
            this.MotorZ = motorZ;
        }

        /// <summary>ICloneable 인터페이스 구현</summary>
        /// <returns></returns>
        public object Clone()
        {
            MarkerArgDefault markerArgDefault = new MarkerArgDefault()
            {
                MarkTargets = this.MarkTargets,
                RepeatSelectedButBoundRect = this.RepeatSelectedButBoundRect,
                IsExternalStart = this.IsExternalStart,
                Rtc = this.Rtc,
                Laser = this.Laser,
                MotorZ = this.MotorZ,
                RtcListType = this.RtcListType,
                Offsets = new List<Offset>((IEnumerable<Offset>)this.Offsets),
                PenStack = new ConcurrentStack<IPen>((IEnumerable<IPen>)this.PenStack),
                StartTime = this.StartTime,
                EndTime = this.EndTime,
                Progress = this.Progress,
                IsSuccess = this.IsSuccess,
                Tag = this.Tag
            };
            if (this.Document != null)
                markerArgDefault.Document = (IDocument)this.Document.Clone();
            return (object)markerArgDefault;
        }
    }
}
