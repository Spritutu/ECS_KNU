
namespace Scanlab.Sirius
{
    /// <summary>바코드 셀(Cell)의 형상</summary>
    public enum BarcodeShapeType
    {
        /// <summary>개별 점</summary>
        Dot,
        /// <summary>셀간 연결된 선분</summary>
        Line,
        /// <summary>해치가 제외된 외곽 폐곡선</summary>
        Outline,
        /// <summary>Hatch (Line)</summary>
        Hatch,
        /// <summary>사용자 정의 패턴</summary>
        Pattern,
    }
}
