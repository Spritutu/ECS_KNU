using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab
{
    /// <summary>비전 시퀀스 인터페이스</summary>
    public interface ISequenceVision : ISequence, IDisposable
    {
        /// <summary>서비스 인터페이스</summary>
        IServiceVision Service { get; }

        /// <summary>핸들러 서비스 외부 연결용</summary>
        IServiceHandler ServiceHandler { get; set; }

        /// <summary>초기화</summary>
        /// <returns>성공여부</returns>
        bool Initialize();
    }
}
