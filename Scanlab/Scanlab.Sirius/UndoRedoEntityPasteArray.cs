using System;
using System.Collections.Generic;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 붙혀넣기 (배열)</summary>
    internal class UndoRedoEntityPasteArray : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> cloned;
        private Layer layer;
        private Vector2 baseLocation;
        private List<Vector2> targetLocations;
        private UndoRedoMultiple urs;

        public override void Execute()
        {
            BoundRect boundRect = new BoundRect();
            foreach (IEntity entity in Action.ClipBoard)
                boundRect.Union(entity.BoundRect);
            foreach (Vector2 targetLocation in this.targetLocations)
            {
                foreach (IEntity entity1 in Action.ClipBoard)
                {
                    if (!(entity1 is Layer) && entity1 is ICloneable cloneable2)
                    {
                        IEntity entity2 = (IEntity)cloneable2.Clone();
                        (entity2 as IDrawable)?.Transit(this.baseLocation + targetLocation - boundRect.Center);
                        this.cloned.Add(entity2);
                    }
                }
            }
            this.Redo();
        }

        public override void Undo() => this.urs.Undo();

        public override void Redo()
        {
            this.urs = new UndoRedoMultiple();
            foreach (IEntity entity in this.cloned)
                this.urs.Add((IUndoRedo)new UndoRedoEntityAdd(this.doc, this.layer, entity));
            this.urs.Execute();
        }

        public UndoRedoEntityPasteArray(
          IDocument doc,
          Layer layer,
          Vector2 baseLocation,
          List<Vector2> targetLocations)
        {
            this.Name = string.Format("Entity Paste Array : {0}", (object)targetLocations.Count);
            this.doc = doc;
            this.layer = layer;
            this.baseLocation = baseLocation;
            this.targetLocations = targetLocations;
            this.cloned = new List<IEntity>();
        }
    }
}
