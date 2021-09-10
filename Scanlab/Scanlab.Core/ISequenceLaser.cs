using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab
{
    /// <summary>레이저 시퀀스 인터페이스</summary>
    public interface ISequenceLaser : ISequence, IDisposable
    {
        /// <summary>서비스 인터페이스</summary>
        IServiceLaser Service { get; }

        /// <summary>핸들러 서비스 외부 연결용</summary>
        IServiceHandler ServiceHandler { get; set; }

        /// <summary>초기화</summary>
        /// <returns>성공여부</returns>
        bool Initialize();
    }
}
