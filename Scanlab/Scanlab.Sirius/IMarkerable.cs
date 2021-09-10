using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Markerable 인터페이스
    /// 레이저 가공이 가능한 엔티티들이 상속구현을 해야 하는 인터페이스
    /// </summary>
    public interface IMarkerable
    {
        /// <summary>레이저 가공 유무</summary>
        bool IsMarkerable { get; set; }

        /// <summary>반복 가공 회수 (기본값 1)</summary>
        uint Repeat { get; set; }

        /// <summary>지정된 Rtc 인터페이스와 Laser 인터페이스를 이용한 엔티티 가공</summary>
        /// <param name="markerArg">IMarkerArg 인터페이스</param>
        /// <returns></returns>
        bool Mark(IMarkerArg markerArg);
    }
}
