
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>
    /// RTC 듀얼헤드 인터페이스
    /// (여기에서 제공되는 Offset/Angle 은 행렬스택과 무관하게 적용됨. 즉 스캐너의 HW 적인 장착 방향에 대한 오프셋 처리용으로 사용 권장함)
    /// </summary>
    public interface IRtcDualHead
    {
        /// <summary>Secondary 헤드의 보정 파일 테이블 번호</summary>
        CorrectionTableIndex SecondaryHeadTable { get; }

        /// <summary>Primary 헤드의 오프셋 (듀얼 헤드 사용시)</summary>
        Vector2 PrimaryHeadOffset { get; }

        /// <summary>Primary 헤드의 회전각도 (듀얼 헤드 사용시)</summary>
        float PrimaryHeadAngle { get; }

        /// <summary>Secondary 헤드의 오프셋 (듀얼 헤드 사용시)</summary>
        Vector2 SecondaryHeadOffset { get; }

        /// <summary>Secondary 헤드의 회전각도 (듀얼 헤드 사용시)</summary>
        float SecondaryHeadAngle { get; }

        /// <summary>듀얼 헤드 사용시 개별 헤드에 대한 오프셋 이동 회전량 설정</summary>
        /// <param name="head">primary or secondary</param>
        /// <param name="offset">dx,dy (mm)</param>
        /// <param name="angle">회전 (각도)</param>
        /// <returns></returns>
        bool CtlHeadOffset(ScannerHead head, Vector2 offset, float angle);

        /// <summary>리스트 명령 - 듀얼 헤드 사용시 개별 헤드에 대한 오프셋 이동 회전량 설정</summary>
        /// <param name="head">primary or secondary</param>
        /// <param name="offset">dx, dy (mm)</param>
        /// <param name="angle">회전 (각도)</param>
        /// <returns></returns>
        bool ListHeadOffset(ScannerHead head, Vector2 offset, float angle);
    }
}
