using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>Undo  / Redo 용 싱글(단일) 명령용</summary>
    internal abstract class UndoRedoSingle : IUndoRedo
    {
        public string Name { get; set; }

        public abstract void Execute();

        public abstract void Undo();

        public abstract void Redo();
    }
}