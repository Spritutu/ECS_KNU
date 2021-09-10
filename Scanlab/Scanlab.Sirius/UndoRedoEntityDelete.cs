
using System;
using System.Collections.Generic;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 삭제</summary>
    internal class UndoRedoEntityDelete : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> list;
        private List<UndoRedoEntityDeleteLayer> targetLayer;
        private List<UndoRedoEntityDeleteEntity> targetEntity;

        public override void Execute() => this.Redo();

        public override void Undo()
        {
            foreach (UndoRedoEntityDeleteLayer entityDeleteLayer in this.targetLayer)
                this.doc.Layers.Insert(entityDeleteLayer.Index, entityDeleteLayer.Layer);
            foreach (UndoRedoEntityDeleteEntity entityDeleteEntity in this.targetEntity)
                this.doc.Layers[entityDeleteEntity.IndexLayer].Insert(entityDeleteEntity.IndexEntity, entityDeleteEntity.Entity);
            this.targetLayer.Clear();
            this.targetEntity.Clear();
            this.doc.Action.SelectedEntity = this.list;
        }

        public override void Redo()
        {
            List<IEntity> deleted = new List<IEntity>();
            foreach (IEntity entity in this.list)
            {
                if (entity is Layer)
                {
                    Layer layer = entity as Layer;
                    if (!layer.IsLocked)
                    {
                        int num = this.doc.Layers.IndexOf(layer);
                        this.targetLayer.Add(new UndoRedoEntityDeleteLayer()
                        {
                            Index = num,
                            Layer = layer
                        });
                        deleted.Add(entity);
                    }
                }
                else
                {
                    Layer owner = entity.Owner as Layer;
                    int num1 = this.doc.Layers.IndexOf(owner);
                    int num2 = owner.IndexOf(entity);
                    this.targetEntity.Add(new UndoRedoEntityDeleteEntity()
                    {
                        IndexLayer = num1,
                        Layer = owner,
                        IndexEntity = num2,
                        Entity = entity
                    });
                    deleted.Add(entity);
                }
            }
            foreach (UndoRedoEntityDeleteEntity entityDeleteEntity in this.targetEntity)
                entityDeleteEntity.Layer.Remove(entityDeleteEntity.Entity);
            foreach (UndoRedoEntityDeleteLayer entityDeleteLayer in this.targetLayer)
                this.doc.Layers.Remove(entityDeleteLayer.Layer);
            List<IEntity> entityList = new List<IEntity>((IEnumerable<IEntity>)this.list);
            entityList.RemoveAll((Predicate<IEntity>)(x => deleted.Contains(x)));
            this.doc.Action.SelectedEntity = entityList;
        }

        public UndoRedoEntityDelete(IDocument doc, List<IEntity> list)
        {
            this.Name = "Entity Delete";
            this.doc = doc;
            this.list = new List<IEntity>((IEnumerable<IEntity>)list);
            this.targetLayer = new List<UndoRedoEntityDeleteLayer>();
            this.targetEntity = new List<UndoRedoEntityDeleteEntity>();
        }
    }
}
