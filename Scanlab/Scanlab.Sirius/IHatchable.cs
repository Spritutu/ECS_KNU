using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Hatchable 인터페이스
    /// 해치 기능을 제공해야 하는 엔티티가 상속 구현해야 하는 인터페이스
    /// 폐(댣힌) 영역을 가진 엔티티에 대해서만 동작
    /// </summary>
    public interface IHatchable
    {
        /// <summary>해치 인자를 통해 해칭된 그룹 객체를 얻는다</summary>
        /// <param name="mode">해치 모드</param>
        /// <param name="angle">해치 각도</param>
        /// <param name="angle2">Cross Line 모드시 두번재 해치 각도</param>
        /// <param name="interval">해치 간격 (mm)</param>
        /// <param name="exclude">해치 외부 제외 영역 길이 (mm)</param>
        /// <returns></returns>
        Group Hatch(HatchMode mode, float angle, float angle2, float interval, float exclude);
    }
}
