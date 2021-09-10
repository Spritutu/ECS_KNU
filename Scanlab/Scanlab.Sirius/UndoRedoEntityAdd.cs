using System.IO;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 추가</summary>
    internal class UndoRedoEntityAdd : UndoRedoSingle
    {
        private IDocument doc;
        private Layer layer;
        private IEntity entity;

        public override void Execute() => this.Redo();

        public override void Undo()
        {
            if (this.entity is Layer)
                this.doc.Layers.Remove(this.entity as Layer);
            else
                (this.entity.Owner as Layer).Remove(this.entity);
        }

        public override void Redo()
        {
            if (this.entity is Layer)
            {
                this.doc.Layers.Add(this.entity as Layer);
                this.entity.Index = this.doc.Layers.IndexOf(this.layer);
            }
            else if (this.layer != null)
            {
                this.entity.Owner = (IEntity)this.layer;
                this.layer.Add(this.entity);
                this.entity.Index = this.layer.IndexOf((IEntity)this.layer);
            }
            else
            {
                if (this.doc.Layers.Active == null)
                    throw new InvalidDataException("target layer is not exist !");
                this.entity.Owner = (IEntity)this.doc.Layers.Active;
                this.doc.Layers.Active.Add(this.entity);
                this.entity.Index = this.doc.Layers.Active.IndexOf(this.entity);
            }
        }

        public UndoRedoEntityAdd(IDocument doc, Layer layer, IEntity entity)
        {
            this.Name = "Entity Add";
            this.doc = doc;
            this.layer = layer;
            this.entity = entity;
        }
    }
}
