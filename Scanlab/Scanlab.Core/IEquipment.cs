using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab
{
    /// <summary>Equipment 인터페이스</summary>
    public interface IEquipment
    {
        /// <summary>장비 이름</summary>
        string Name { get; set; }

        /// <summary>시퀀스 핸들러</summary>
        ISequenceHandler SeqHandler { get; set; }

        /// <summary>시퀀스 레이저</summary>
        ISequenceLaser SeqLaser { get; set; }

        /// <summary>시퀀스 비전</summary>
        ISequenceVision SeqVision { get; set; }

        /// <summary>시퀀스 확장</summary>
        ISequenceAuxiliary SeqAux { get; set; }

        /// <summary>데이타 메모리</summary>
        IMemory DataMemory { get; set; }

        /// <summary>사용자 정의 데이타</summary>
        object Tag { get; set; }

        /// <summary>초기화</summary>
        /// <returns>성공여부</returns>
        bool Initialize();
    }
}
