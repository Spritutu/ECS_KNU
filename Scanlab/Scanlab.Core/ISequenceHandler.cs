using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab
{
    public interface ISequenceHandler : ISequence, IDisposable
    {
        /// <summary>서비스 인터페이스</summary>
        IServiceHandler Service { get; }

        /// <summary>레이저 서비스 외부 연결용</summary>
        IServiceLaser ServiceLaser { get; set; }

        /// <summary>비전 서비스 외부 연결용</summary>
        IServiceVision ServiceVision { get; set; }

        /// <summary>Aux 외부 연결용</summary>
        IServiceAuxiliary ServiceAuxiliary { get; set; }

        /// <summary>초기화</summary>
        /// <returns>성공여부</returns>
        bool Initialize();
    }
}
