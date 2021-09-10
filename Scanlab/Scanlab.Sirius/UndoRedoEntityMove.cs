using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    internal class UndoRedoEntityMove : UndoRedoSingle
    {
        private IDocument doc;
        private IEntity entity;
        private Layer sourceLayer;
        private Layer targetLayer;
        private int targetIndex;
        private IEntity cloned;
        private UndoRedoEntityDelete ur;

        public override void Execute() => this.Redo();

        public override void Undo()
        {
            this.ur.Undo();
            this.targetLayer.Remove(this.cloned);
        }

        public override void Redo()
        {
            if (this.entity is Layer)
                return;
            this.sourceLayer = (Layer)this.entity.Owner;
            IEntity entity1 = this.entity;
            int num = this.entity.Node.Parent.Nodes.IndexOf(this.entity.Node);
            if (!(this.entity is ICloneable entity2))
                return;
            this.cloned = (IEntity)entity2.Clone();
            if (this.sourceLayer.Equals((object)this.targetLayer))
            {
                if (num > this.targetIndex)
                    this.targetLayer.Insert(this.targetIndex, this.cloned);
                else
                    this.targetLayer.Insert(this.targetIndex + 1, this.cloned);
            }
            else
                this.targetLayer.Insert(this.targetIndex, this.cloned);
            this.ur = new UndoRedoEntityDelete(this.doc, new List<IEntity>()
      {
        this.entity
      });
            this.ur.Execute();
        }

        public UndoRedoEntityMove(IDocument doc, IEntity entity, Layer targetLayer, int targetIndex)
        {
            this.Name = "Entity Move";
            this.doc = doc;
            this.entity = entity;
            this.targetLayer = targetLayer;
            this.targetIndex = targetIndex;
        }
    }
}
