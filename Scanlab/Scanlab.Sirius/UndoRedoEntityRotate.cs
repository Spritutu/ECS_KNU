
using System.Collections.Generic;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 회전</summary>
    internal class UndoRedoEntityRotate : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> list;
        private float angle;
        private Vector2 rotateCenter;

        public override void Execute() => this.Redo();

        public override void Undo()
        {
            foreach (IEntity entity in this.list)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Rotate(-this.angle, this.rotateCenter);
            }
        }

        public override void Redo()
        {
            foreach (IEntity entity in this.list)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Rotate(this.angle, this.rotateCenter);
            }
        }

        public UndoRedoEntityRotate(
          IDocument doc,
          List<IEntity> list,
          float angle,
          Vector2 rotateCenter)
        {
            this.Name = "Entity Rotate";
            this.doc = doc;
            this.list = new List<IEntity>((IEnumerable<IEntity>)list);
            this.angle = angle;
            this.rotateCenter = rotateCenter;
        }
    }
}
