using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 잘라내기</summary>
    internal class UndoRedoEntityCut : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> list;
        private UndoRedoEntityDelete ur;

        public override void Execute() => this.Redo();

        public override void Undo() => this.ur.Undo();

        public override void Redo()
        {
            Action.ClipBoard.Clear();
            Action.ClipBoard.AddRange((IEnumerable<IEntity>)this.list);
            this.ur = new UndoRedoEntityDelete(this.doc, this.list);
            this.ur.Execute();
        }

        public UndoRedoEntityCut(IDocument doc, List<IEntity> list)
        {
            this.Name = "Entity Cut";
            this.doc = doc;
            this.list = new List<IEntity>((IEnumerable<IEntity>)list);
        }
    }
}
