
using System.Collections.Generic;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 이동</summary>
    internal class UndoRedoEntityTransit : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> list;
        private Vector2 delta;

        public override void Execute() => this.Redo();

        public override void Undo()
        {
            Vector2 delta = new Vector2(-this.delta.X, -this.delta.Y);
            foreach (IEntity entity in this.list)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Transit(delta);
            }
        }

        public override void Redo()
        {
            foreach (IEntity entity in this.list)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Transit(this.delta);
            }
        }

        public UndoRedoEntityTransit(IDocument doc, List<IEntity> list, Vector2 delta)
        {
            this.Name = "Entity Transit";
            this.doc = doc;
            this.list = new List<IEntity>((IEnumerable<IEntity>)list);
            this.delta = delta;
        }
    }
}
