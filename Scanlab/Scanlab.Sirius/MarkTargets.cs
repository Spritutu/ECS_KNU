using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>가공 대상 종류</summary>
    public enum MarkTargets
    {
        /// <summary>전체 가공</summary>
        All,
        /// <summary>선택된 엔티티만 가공</summary>
        Selected,
        /// <summary>
        /// 선택된 엔티티의 외곽(사각) 영역만 가공
        /// 이때 반복회수는 별도로 지정 가능함
        /// </summary>
        SelectedButBoundRect,
        /// <summary>사용자 지정된 방식으로 가공</summary>
        Custom,
    }
}
