
using System.Collections.Generic;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 선택</summary>
    internal class UndoRedoEntitySelect : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> list;
        private List<IEntity> old;
        private UndoRedoLayerActive ur;

        public override void Execute()
        {
            this.old.AddRange((IEnumerable<IEntity>)this.doc.Action.SelectedEntity);
            this.Redo();
        }

        public override void Undo()
        {
            foreach (Layer layer in (ObservableList<Layer>)this.doc.Layers)
            {
                layer.IsSelected = false;
                foreach (IEntity entity in (ObservableList<IEntity>)layer)
                    entity.IsSelected = false;
            }
            foreach (IEntity entity in this.old)
                entity.IsSelected = true;
            this.doc.Action.SelectedEntity = this.old;
            this.ur?.Undo();
        }

        public override void Redo()
        {
            foreach (Layer layer in (ObservableList<Layer>)this.doc.Layers)
            {
                layer.IsSelected = false;
                foreach (IEntity entity in (ObservableList<IEntity>)layer)
                    entity.IsSelected = false;
            }
            Layer layer1 = (Layer)null;
            foreach (IEntity entity in this.list)
            {
                entity.IsSelected = true;
                if (entity is Layer)
                    layer1 = entity as Layer;
            }
            this.doc.Action.SelectedEntity = this.list;
            if (layer1 == null)
                return;
            this.ur = new UndoRedoLayerActive(this.doc, layer1);
            this.ur.Execute();
        }

        public UndoRedoEntitySelect(IDocument doc, List<IEntity> list)
        {
            this.Name = "Entity Select";
            this.doc = doc;
            this.old = new List<IEntity>();
            this.list = new List<IEntity>((IEnumerable<IEntity>)list);
        }
    }
}
