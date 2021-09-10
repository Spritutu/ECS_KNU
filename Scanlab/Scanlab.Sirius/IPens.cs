using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>복수개의 펜 개체를 관리하는 인터페이스</summary>
    public interface IPens
    {
        /// <summary>부모 문서</summary>
        IDocument Owner { get; set; }

        /// <summary>파일 이름</summary>
        string FileName { get; set; }

        /// <summary>이름</summary>
        string Name { get; set; }

        /// <summary>내부 펜 개체의 개수</summary>
        int Count { get; }

        /// <summary>내부 펜 개체 배열 접근</summary>
        IPen[] Items { get; set; }

        /// <summary>사용자 정의 데이타</summary>
        object Tag { get; set; }
    }
}
