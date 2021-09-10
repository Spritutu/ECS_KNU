
using System.Collections.Generic;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 속성 변경</summary>
    internal class UndoRedoEntityPropertyChanged : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> list;
        private string propName;
        private object oldValue;
        private object newValue;

        public override void Execute() => this.Redo();

        public override void Undo()
        {
            foreach (IEntity entity in this.list)
                entity.GetType().GetProperty(this.propName).SetValue((object)entity, this.oldValue);
        }

        public override void Redo()
        {
            foreach (IEntity entity in this.list)
                entity.GetType().GetProperty(this.propName).SetValue((object)entity, this.newValue);
        }

        public UndoRedoEntityPropertyChanged(
          IDocument doc,
          List<IEntity> list,
          string propName,
          object oldValue,
          object newValue)
        {
            this.Name = "Entity Property Changed";
            this.doc = doc;
            this.list = new List<IEntity>((IEnumerable<IEntity>)list);
            this.propName = propName;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }
}
