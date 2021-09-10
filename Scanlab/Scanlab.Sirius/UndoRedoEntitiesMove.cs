using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    internal class UndoRedoEntitiesMove : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> list;
        private Layer targetLayer;
        private int targetIndex;
        private UndoRedoMultiple urs;

        public override void Execute()
        {
            this.urs = new UndoRedoMultiple();
            if (this.list[0].Node.Index > this.targetIndex || !this.targetLayer.Equals((object)(this.list[0].Owner as Layer)))
            {
                int targetIndex = this.targetIndex;
                foreach (IEntity entity in this.list)
                    this.urs.Add((IUndoRedo)new UndoRedoEntityMove(this.doc, entity, this.targetLayer, targetIndex++));
            }
            else
            {
                foreach (IEntity entity in this.list)
                    this.urs.Add((IUndoRedo)new UndoRedoEntityMove(this.doc, entity, this.targetLayer, this.targetIndex));
            }
            this.Redo();
        }

        public override void Undo() => this.urs.Undo();

        public override void Redo() => this.urs.Redo();

        public UndoRedoEntitiesMove(
          IDocument doc,
          List<IEntity> list,
          Layer targetLayer,
          int targetIndex)
        {
            this.Name = "Entities Move";
            this.doc = doc;
            this.list = list;
            this.targetLayer = targetLayer;
            this.targetIndex = targetIndex;
        }
    }
}
