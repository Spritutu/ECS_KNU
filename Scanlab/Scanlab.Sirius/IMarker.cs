
namespace Scanlab.Sirius
{
    /// <summary>
    /// 마커 인터페이스
    /// 레이저, 스캐너 등의 하드웨어를 이용해 전달된 가공 데이타(Document) 기반으로 실제 출사를 위해 내부에 쓰레드를 생성하여
    /// 다양한 오프셋 위치에 레이저 가공을 수행하는 마커(Marker) 인터페이스
    /// </summary>
    public interface IMarker
    {
        /// <summary>진행률 이벤트 핸들러</summary>
        event MarkerEventHandler OnProgress;

        /// <summary>완료 이벤트 핸들러</summary>
        event MarkerEventHandler OnFinished;

        /// <summary>식별 번호</summary>
        uint Index { get; }

        /// <summary>이름</summary>
        string Name { get; set; }

        /// <summary>가공 준비 상태</summary>
        bool IsReady { get; }

        /// <summary>출사중 여부</summary>
        bool IsBusy { get; }

        /// <summary>에러 여부</summary>
        bool IsError { get; }

        /// <summary>마커 시작시 전달 인자 (Ready 에 의해 업데이트 되고, Start 시 내부적으로 사용됨)</summary>
        IMarkerArg MarkerArg { get; }

        /// <summary>
        /// 스캐너가 회전되어 장착되어 있는 경우 설정. 기본값 (0)
        /// 지정된 각도만큼 내부에서 회전 처리됨
        /// </summary>
        double ScannerRotateAngle { get; set; }

        /// <summary>복제된 문서(IDocument) 객체에 대한 접근 (Ready 호출시 복제됨)</summary>
        IDocument Document { get; }

        /// <summary>사용자 정의 데이타</summary>
        object Tag { get; set; }

        /// <summary>
        /// 마커는 내부 쓰레드에 의해 가공 데이타를 처리하게 되는데, 이때 가공 데이타(IDocument)에
        /// 크로스 쓰레드 상태가 될수있으므로, 준비(Prepare)시에는 가공 데이타를 모두 복제(Clone) 하여 가공시
        /// 데이타에 대한 쓰레드 안전 접근을 처리하게 된다. 또한 가공중 뷰에 의해 원본 데이타가 조작, 수정되더라도
        /// 준비(Ready) 즉 신규 데이타를 다운로드하지 않으면 아무런 영향이 없게 된다.
        /// </summary>
        /// <param name="markerArg">IMarkerArg 인터페이스</param>
        /// <returns></returns>
        bool Ready(IMarkerArg markerArg);

        /// <summary>복제된 문서 데이타를 초기화 (다시 Ready 를 호출하여 문서 복제 필요)</summary>
        /// <returns></returns>
        bool Clear();

        /// <summary>가공 시작</summary>
        /// <returns></returns>
        bool Start();

        /// <summary>가공 강제 정지</summary>
        /// <returns></returns>
        bool Stop();

        /// <summary>리셋</summary>
        /// <returns></returns>
        bool Reset();
    }
}
