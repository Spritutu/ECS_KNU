using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>정렬 기준점</summary>
    public enum SortMethod
    {
        /// <summary>엔티티 중심 위치 기준</summary>
        ByCenter,
        /// <summary>외곽 영역 기준</summary>
        ByBoundRect,
    }
}
