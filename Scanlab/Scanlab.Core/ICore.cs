using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab
{
    public interface ICore
    {
        /// <summary>스파이럴랩 Core 인터페이스</summary>
        /// <summary>코어 초기화</summary>
        /// <returns></returns>
        bool InitializeEngine();
    }
}
