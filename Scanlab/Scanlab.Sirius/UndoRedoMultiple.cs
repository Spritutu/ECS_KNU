using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>
    ///  Undo / Redo 용 멀티(다중) 명령용
    ///  (여러개의 IUndoRedo 집합을 하나의 IUndoRedo 로 처리하기위한 컨테이너)
    /// </summary>
    internal class UndoRedoMultiple : List<IUndoRedo>, IUndoRedo
    {
        public string Name { get; set; }

        public virtual void Execute()
        {
            foreach (IUndoRedo undoRedo in (List<IUndoRedo>)this)
                undoRedo.Execute();
        }

        public virtual void Undo()
        {
            List<IUndoRedo> undoRedoList = new List<IUndoRedo>((IEnumerable<IUndoRedo>)this);
            undoRedoList.Reverse();
            foreach (IUndoRedo undoRedo in undoRedoList)
                undoRedo.Undo();
        }

        public virtual void Redo()
        {
            foreach (IUndoRedo undoRedo in (List<IUndoRedo>)this)
                undoRedo.Redo();
        }
    }
}
