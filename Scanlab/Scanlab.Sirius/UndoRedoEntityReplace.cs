using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 교체하기</summary>
    internal class UndoRedoEntityReplace : UndoRedoSingle
    {
        private IDocument doc;
        private Layer layer;
        private IEntity target;
        private IEntity replace;
        private int index;

        public override void Execute() => this.Redo();

        public override void Undo() => this.layer[this.index] = this.target;

        public override void Redo()
        {
            this.index = this.layer.IndexOf(this.target);
            this.layer[this.index] = this.replace;
        }

        public UndoRedoEntityReplace(
          IDocument doc,
          Layer layer,
          IEntity targetEntity,
          IEntity replaceEntity)
        {
            this.Name = "Entity Replace";
            this.doc = doc;
            this.index = -1;
            this.layer = layer;
            this.target = targetEntity;
            this.replace = replaceEntity;
        }
    }
}
