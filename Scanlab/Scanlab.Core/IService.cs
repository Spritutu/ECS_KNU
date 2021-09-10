using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab
{
    /// <summary>서비스 인터페이스</summary>
    public interface IService
    {
        /// <summary>이름</summary>
        string Name { get; }

        /// <summary>사용자 데이타</summary>
        object Tag { get; set; }
    }
}
