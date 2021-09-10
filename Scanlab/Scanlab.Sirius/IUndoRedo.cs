using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>Undo  / Redo 용 인터페이스</summary>
    internal interface IUndoRedo
    {
        /// <summary>이름</summary>
        string Name { get; }

        /// <summary>redo 실행</summary>
        void Execute();

        /// <summary>undo 실행</summary>
        void Undo();

        /// <summary>redo 실행</summary>
        void Redo();
    }
}
