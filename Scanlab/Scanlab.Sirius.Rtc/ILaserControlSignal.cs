using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 레이저 제어 신호의 레벨 설정(Laser1, 2, On 등의 신호 레벨 : Active Low/High)
    /// RTC5 이상만 지원됨
    /// </summary>
    public interface ILaserControlSignal
    {
        /// <summary>비트 구조체를 32비트 값으로 변환</summary>
        /// <returns></returns>
        uint ToUInt();
    }
}
