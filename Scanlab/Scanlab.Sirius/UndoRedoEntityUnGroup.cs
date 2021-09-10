using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 그룹 해제</summary>
    internal class UndoRedoEntityUnGroup : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> list;
        private List<IEntity> groupList;
        private List<IEntity> items;
        private Layer layer;
        private UndoRedoMultiple urs;

        public override void Execute()
        {
            this.groupList = new List<IEntity>();
            this.items = new List<IEntity>();
            foreach (IEntity entity in this.list)
            {
                if (entity is Group)
                {
                    Group group = entity as Group;
                    this.groupList.Add((IEntity)group);
                    this.items.AddRange((IEnumerable<IEntity>)group.Items);
                }
            }
            this.Redo();
        }

        public override void Undo() => this.urs.Undo();

        public override void Redo()
        {
            this.urs = new UndoRedoMultiple();
            this.urs.Add((IUndoRedo)new UndoRedoEntityDelete(this.doc, this.groupList));
            foreach (IEntity entity in this.items)
                this.urs.Add((IUndoRedo)new UndoRedoEntityAdd(this.doc, this.layer, entity));
            this.urs.Execute();
        }

        public UndoRedoEntityUnGroup(IDocument doc, List<IEntity> list, Layer layer)
        {
            this.Name = "Entity Ungroup";
            this.doc = doc;
            this.list = new List<IEntity>((IEnumerable<IEntity>)list);
            this.layer = layer;
        }
    }
}
