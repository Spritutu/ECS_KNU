using System;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 3x3 행렬 스택 인터페이스
    /// 스택 자료구조에 3*3 행렬구조체를 push/pop 하여 저장하고, 스택내의 모든 행렬을 곱셈 연산하는등의 유틸리티 클래스
    /// 스택내에 있는 행렬들의 곱셈을 빈번히 계산하지 않고 변경점이 있을때만 계산을 하기 위해 만들어짐
    /// 가장 마지막에 삽입된 행렬값부터 적용됨
    /// </summary>
    public interface IMatrixStack : ICloneable
    {
        /// <summary>스택에 있는 모든 행렬 연산 결과 얻기</summary>
        Matrix3x2 ToResult { get; }

        /// <summary>스택에 있는 행렬의 개수</summary>
        int Count { get; }

        /// <summary>스택에 모든 행렬을 초기화 하고 단위행렬로 설정</summary>
        void Clear();

        /// <summary>스택에 행렬 Push</summary>
        /// <param name="m"></param>
        void Push(Matrix3x2 m);

        /// <summary>스택에서 행렬 Pop</summary>
        /// <param name="m"></param>
        void Pop(out Matrix3x2 m);

        /// <summary>스택에서 행렬 Pop</summary>
        void Pop();

        /// <summary>회전</summary>
        /// <param name="angle">각도</param>
        void Push(double angle);

        /// <summary>이동</summary>
        /// <param name="dx">X 이동량 (mm)</param>
        /// <param name="dy">Y 이동량 (mm)</param>
        void Push(double dx, double dy);

        /// <summary>이동</summary>
        /// <param name="translate">x,y 이동량 (mm)</param>
        void Push(Vector2 translate);

        /// <summary>회전후 이동</summary>
        /// <param name="dx">이동량 dX (mm)</param>
        /// <param name="dy">이동량 dY (mm)</param>
        /// <param name="angle">회전 (각도)</param>
        void Push(double dx, double dy, double angle);
    }
}
