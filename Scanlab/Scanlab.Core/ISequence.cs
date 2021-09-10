using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab
{
    /// <summary>시퀀스 인터페이스</summary>
    public interface ISequence : IDisposable
    {
        /// <summary>이름</summary>
        string Name { get; }

        /// <summary>시퀀스 쓰레드 동기화 객체</summary>
        object SyncRoot { get; }

        /// <summary>사용자 데이타</summary>
        object Tag { get; set; }
    }
}
