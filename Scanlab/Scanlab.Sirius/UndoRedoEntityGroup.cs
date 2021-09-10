
using System.Collections.Generic;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 그룹으로 만들기</summary>
    internal class UndoRedoEntityGroup : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> list;
        private Group group;
        private Layer layer;
        private UndoRedoMultiple urs;

        public override void Execute()
        {
            foreach (IEntity entity in this.list)
                entity.IsSelected = false;
            this.group = new Group(this.list);
            this.Redo();
        }

        public override void Undo() => this.urs.Undo();

        public override void Redo()
        {
            this.urs = new UndoRedoMultiple();
            this.urs.Add((IUndoRedo)new UndoRedoEntityDelete(this.doc, this.list));
            this.urs.Add((IUndoRedo)new UndoRedoEntityAdd(this.doc, this.layer, (IEntity)this.group));
            this.urs.Execute();
        }

        public UndoRedoEntityGroup(IDocument doc, List<IEntity> list, Layer layer)
        {
            this.Name = "Entity Group";
            this.doc = doc;
            this.list = new List<IEntity>((IEnumerable<IEntity>)list);
            this.layer = layer;
        }
    }
}
