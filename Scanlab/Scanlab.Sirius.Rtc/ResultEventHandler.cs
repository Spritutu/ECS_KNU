
namespace Scanlab.Sirius
{
    /// <summary>스캐너 필드 보정에 대한 결과 이벤트 통지용 델리게이트</summary>
    /// <param name="sender">IRtcCorrection 인터페이스</param>
    /// <param name="success">변환 성공 여부</param>
    /// <param name="message">변환 로그 메시지</param>
    public delegate void ResultEventHandler(object sender, bool success, string message);
}
