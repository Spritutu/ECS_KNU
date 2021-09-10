using System;
using System.Collections.Generic;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 붙혀넣기</summary>
    internal class UndoRedoEntityPaste : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> pastedList;
        private Layer layer;
        private Vector2 location;
        private bool isMove;
        private UndoRedoMultiple urs;

        public override void Execute()
        {
            BoundRect boundRect = new BoundRect();
            foreach (IEntity entity in Action.ClipBoard)
                boundRect.Union(entity.BoundRect);
            Vector2 delta = this.location - boundRect.Center;
            foreach (IEntity entity1 in Action.ClipBoard)
            {
                if (!(entity1 is Layer) && entity1 is ICloneable cloneable1)
                {
                    IEntity entity2 = (IEntity)cloneable1.Clone();
                    IDrawable drawable = entity2 as IDrawable;
                    if (this.isMove && drawable != null)
                        drawable.Transit(delta);
                    this.pastedList.Add(entity2);
                }
            }
            this.Redo();
        }

        public override void Undo() => this.urs.Undo();

        public override void Redo()
        {
            this.urs = new UndoRedoMultiple();
            foreach (IEntity pasted in this.pastedList)
                this.urs.Add((IUndoRedo)new UndoRedoEntityAdd(this.doc, this.layer, pasted));
            this.urs.Execute();
        }

        public UndoRedoEntityPaste(IDocument doc, Layer layer)
        {
            this.Name = "Entity Paste";
            this.doc = doc;
            this.layer = layer;
            this.pastedList = new List<IEntity>();
            this.isMove = false;
        }

        public UndoRedoEntityPaste(IDocument doc, Layer layer, Vector2 location)
          : this(doc, layer)
        {
            this.location = location;
            this.isMove = true;
        }
    }
}
