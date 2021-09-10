using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Compensator 인터페이스
    /// 내부의 보간 테이블을 적용해 입력값을 가지고 새로운 출력값을 연산하는 연산기
    /// </summary>
    public interface ICompensator<T>
    {
        /// <summary>동기화 객체</summary>
        object SyncRoot { get; }

        /// <summary>식별자 (0, 1, 2, ...)</summary>
        uint Index { get; }

        /// <summary>이름</summary>
        string Name { get; }

        /// <summary>보간 연산하기</summary>
        /// <param name="input">입력값</param>
        /// <param name="output">보간된 출력값</param>
        /// <returns></returns>
        bool Interpolate(T input, out T output);
    }
}
