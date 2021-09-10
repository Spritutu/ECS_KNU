using System.Collections.Generic;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Explodable 인터페이스
    /// 분해가 가능한 엔티티가 상속 구현해야 하는 인터페이스
    /// </summary>
    public interface IExplodable
    {
        /// <summary>분해</summary>
        /// <returns>분해된 엔티티의 리스트</returns>
        List<IEntity> Explode();
    }
}