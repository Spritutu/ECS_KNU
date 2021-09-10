
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>RTC 의 3D 옵션 (VarioScan 혹은 excelliSHIFT 등의 장치에서 사용됨)</summary>
    public interface IRtc3D
    {
        /// <summary>
        /// Z 오프셋 (단위 : mm)
        /// + 값이 위방향
        /// </summary>
        float ZOffset { get; }

        /// <summary>
        /// Z 디포커스 (defocus) (단위 : mm)
        /// + 값이 위방향
        /// </summary>
        float ZDefocus { get; }

        /// <summary>Z 축의 KZ 값 = bits/mm</summary>
        float KZFactor { get; }

        /// <summary>스캐너 이동</summary>
        /// <param name="vPosition">x, y, z (mm)</param>
        /// <returns></returns>
        bool CtlMove(Vector3 vPosition);

        /// <summary>Z 오프셋</summary>
        /// <param name="zOffset">포커스 Z 이동 오프셋 량 (mm))</param>
        /// <returns></returns>
        bool CtlZOffset(float zOffset);

        /// <summary>Z 디포커스</summary>
        /// <param name="zDefocus">디포커스 Z 이동량 (mm)</param>
        /// <returns></returns>
        bool CtlZDefocus(float zDefocus);

        /// <summary>리스트 명령 - Z 오프셋</summary>
        /// <param name="zOffset">포커스 Z 이동 오프셋 량 (mm))</param>
        /// <returns></returns>
        bool ListZOffset(float zOffset);

        /// <summary>리스트 명령 - Z 디포커스</summary>
        /// <param name="zDefocus">디포커스 Z 이동량 (mm)</param>
        /// <returns></returns>
        bool ListZDefocus(float zDefocus);

        /// <summary>리스트 명령 - 점프</summary>
        /// <param name="vPosition">x,y,z 위치 (mm)</param>
        /// <param name="rampFactor">자동 레이저 제어시의 비율값</param>
        /// <returns></returns>
        bool ListJump(Vector3 vPosition, float rampFactor = 1f);

        /// <summary>리스트 명령 - 점프</summary>
        /// <param name="x">x 위치 (mm)</param>
        /// <param name="y">y 위치 (mm)</param>
        /// <param name="z">z 위치 (mm)</param>
        /// <param name="rampFactor">자동 레이저 제어시의 비율값</param>
        /// <returns></returns>
        bool ListJump(float x, float y, float z, float rampFactor = 1f);

        /// <summary>리스트 명령 - 마크</summary>
        /// <param name="vPosition">x,y,z 위치 (mm)</param>
        /// <param name="rampFactor">자동 레이저 제어시의 비율값</param>
        /// <returns></returns>
        bool ListMark(Vector3 vPosition, float rampFactor = 1f);

        /// <summary>리스트 명령 - 마크</summary>
        /// <param name="x">x 위치 (mm)</param>
        /// <param name="y">y 위치 (mm)</param>
        /// <param name="z">z 위치 (mm)</param>
        /// <param name="rampFactor">자동 레이저 제어시의 비율값</param>
        /// <returns></returns>
        bool ListMark(float x, float y, float z, float rampFactor = 1f);

        /// <summary>리스트 명령 - 아크(호)</summary>
        /// <param name="vCenter">중심 위치 (cx, cy, cz) (mm)</param>
        /// <param name="sweepAngle">회전 각도 (+ : 반시계방향)</param>
        /// <returns></returns>
        bool ListArc(Vector3 vCenter, float sweepAngle);

        /// <summary>리스트 명령 - 아크(호)</summary>
        /// <param name="cx">중심 위치 (cx) (mm)</param>
        /// <param name="cy">중심 위치 (cy) (mm)</param>
        /// <param name="cz">중심 위치 (cz) (mm)</param>
        /// <param name="sweepAngle">회전 각도 (+ : 반시계방향)</param>
        /// <returns></returns>
        bool ListArc(float cx, float cy, float cz, float sweepAngle);
    }
}
