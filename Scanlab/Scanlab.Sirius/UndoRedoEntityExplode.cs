using System.Collections.Generic;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 분해</summary>
    internal class UndoRedoEntityExplode : UndoRedoSingle
    {
        private IDocument doc;
        private IEntity entity;
        private Layer layer;
        private UndoRedoMultiple urs;

        public override void Execute() => this.Redo();

        public override void Undo() => this.urs.Undo();

        public override void Redo()
        {
            this.urs = new UndoRedoMultiple();
            List<IEntity> list = new List<IEntity>();
            list.Add(this.entity);
            if (this.entity is IExplodable)
            {
                this.urs.Add((IUndoRedo)new UndoRedoEntityDelete(this.doc, list));
                foreach (IEntity entity in (this.entity as IExplodable).Explode())
                    this.urs.Add((IUndoRedo)new UndoRedoEntityAdd(this.doc, this.layer, entity));
            }
            this.urs.Redo();
        }

        public UndoRedoEntityExplode(IDocument doc, IEntity entity, Layer layer)
        {
            this.Name = "Entity Explode";
            this.doc = doc;
            this.entity = entity;
            this.layer = layer;
        }
    }
}
