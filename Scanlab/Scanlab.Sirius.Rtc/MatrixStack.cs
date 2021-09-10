
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 3x3 matrix with stack
    /// 스택에 push/pop 및 스택내의 모든 행렬을 연산하는등의 유틸리티 클래스
    /// </summary>
    public sealed class MatrixStack : IMatrixStack, ICloneable, IEquatable<MatrixStack>
    {
        private Stack<Matrix3x2> stack;
        private Matrix3x2 matrixResult;
        private bool isModified;

        /// <summary>스택에 있는 모든 행렬 연산 결과 얻기</summary>
        public Matrix3x2 ToResult
        {
            get
            {
                if (!this.isModified)
                    return this.matrixResult;
                this.matrixResult = Matrix3x2.Identity;
                foreach (Matrix3x2 matrix3x2 in this.stack)
                    this.matrixResult *= matrix3x2;
                return this.matrixResult;
            }
        }

        /// <summary>
        /// 스택에 있는 행렬의 개수
        /// 항상 1보다 큰값이 있다 (스택의 가장 밑바닥에는 단위행렬이 존재함)
        /// </summary>
        public int Count => this.stack.Count;

        /// <summary>생성자</summary>
        public MatrixStack()
        {
            this.stack = new Stack<Matrix3x2>();
            this.stack.Push(Matrix3x2.Identity);
            this.isModified = true;
        }

        /// <summary>복사 생성자</summary>
        /// <param name="matrixStack"></param>
        public MatrixStack(MatrixStack matrixStack)
        {
            this.stack = new Stack<Matrix3x2>((IEnumerable<Matrix3x2>)matrixStack.stack);
            this.isModified = true;
        }

        /// <summary>복제</summary>
        /// <returns></returns>
        public object Clone() => (object)new MatrixStack(this);

        public bool Equals(MatrixStack other) => other != null && MathHelper.IsEqual(other.ToResult.M11, this.ToResult.M11) && (MathHelper.IsEqual(other.ToResult.M12, this.ToResult.M12) && MathHelper.IsEqual(other.ToResult.M21, this.ToResult.M21)) && (MathHelper.IsEqual(other.ToResult.M22, this.ToResult.M22) && MathHelper.IsEqual(other.ToResult.M31, this.ToResult.M31)) && MathHelper.IsEqual(other.ToResult.M32, this.ToResult.M32);

        /// <summary>모두 삭제하고 단위행렬상태로 초기화</summary>
        public void Clear()
        {
            this.stack.Clear();
            this.stack.Push(Matrix3x2.Identity);
            this.isModified = true;
        }

        /// <summary>스택에 행렬 Push</summary>
        /// <param name="m"></param>
        public void Push(Matrix3x2 m)
        {
            this.stack.Push(m);
            this.isModified = true;
        }

        /// <summary>스택에서 행렬 Pop</summary>
        /// <param name="matrix">반환된 행렬</param>
        public void Pop(out Matrix3x2 matrix)
        {
            matrix = this.stack.Pop();
            this.isModified = true;
        }

        /// <summary>스택에서 행렬 Pop</summary>
        public void Pop()
        {
            this.stack.Pop();
            this.isModified = true;
        }

        /// <summary>원점을 중심으로 회전</summary>
        /// <param name="angle">회전 (각도)</param>
        public void Push(double angle)
        {
            this.stack.Push(Matrix3x2.CreateRotation((float)(angle * Math.PI / 180.0)));
            this.isModified = true;
        }

        /// <summary>이동하기</summary>
        /// <param name="dx">이동량 X (mm)</param>
        /// <param name="dy">이동량 Y (mm)</param>
        public void Push(double dx, double dy)
        {
            this.stack.Push(Matrix3x2.CreateTranslation(new Vector2((float)dx, (float)dy)));
            this.isModified = true;
        }

        /// <summary>이동하기</summary>
        /// <param name="translate">이동량 X,Y (mm)</param>
        public void Push(Vector2 translate)
        {
            this.stack.Push(Matrix3x2.CreateTranslation(translate));
            this.isModified = true;
        }

        /// <summary>회전후 이동</summary>
        /// <param name="dx">이동량 dX (mm)</param>
        /// <param name="dy">이동량 dY (mm)</param>
        /// <param name="angle">회전 (각도)</param>
        public void Push(double dx, double dy, double angle) => this.Push(Matrix3x2.CreateTranslation(new Vector2((float)dx, (float)dy)) * Matrix3x2.CreateRotation((float)(angle * Math.PI / 180.0)));
    }
}
