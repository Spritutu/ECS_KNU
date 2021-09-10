
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 정렬 (순서)</summary>
    internal class UndoRedoEntitySort : UndoRedoSingle
    {
        private IDocument doc;
        private EntitySort sort;
        private SortMethod method;
        private Layer layer;
        private List<IEntity> list;
        private UndoRedoMultiple urs;

        public override void Execute()
        {
            this.urs = new UndoRedoMultiple();
            int index = this.list.Last<IEntity>().Index;
            List<IEntity> entityList = new List<IEntity>((IEnumerable<IEntity>)this.list);
            switch (this.sort)
            {
                case EntitySort.TopToBottom:
                    if (this.method == SortMethod.ByCenter)
                    {
                        entityList.Sort((Comparison<IEntity>)((a, b) => -a.BoundRect.Center.Y.CompareTo(b.BoundRect.Center.Y)));
                        break;
                    }
                    entityList.Sort((Comparison<IEntity>)((a, b) => -a.BoundRect.Top.CompareTo(b.BoundRect.Top)));
                    break;
                case EntitySort.BottomToTop:
                    if (this.method == SortMethod.ByCenter)
                    {
                        entityList.Sort((Comparison<IEntity>)((a, b) => a.BoundRect.Center.Y.CompareTo(b.BoundRect.Center.Y)));
                        break;
                    }
                    entityList.Sort((Comparison<IEntity>)((a, b) => a.BoundRect.Bottom.CompareTo(b.BoundRect.Bottom)));
                    break;
                case EntitySort.LeftToRight:
                    if (this.method == SortMethod.ByCenter)
                    {
                        entityList.Sort((Comparison<IEntity>)((a, b) => a.BoundRect.Center.X.CompareTo(b.BoundRect.Center.X)));
                        break;
                    }
                    entityList.Sort((Comparison<IEntity>)((a, b) => a.BoundRect.Left.CompareTo(b.BoundRect.Left)));
                    break;
                case EntitySort.RightToLeft:
                    if (this.method == SortMethod.ByCenter)
                    {
                        entityList.Sort((Comparison<IEntity>)((a, b) => -a.BoundRect.Center.X.CompareTo(b.BoundRect.Center.X)));
                        break;
                    }
                    entityList.Sort((Comparison<IEntity>)((a, b) => -a.BoundRect.Right.CompareTo(b.BoundRect.Right)));
                    break;
            }
            foreach (IEntity entity in entityList)
                this.urs.Add((IUndoRedo)new UndoRedoEntityMove(this.doc, entity, this.layer, index));
            this.Redo();
        }

        public override void Undo() => this.urs.Undo();

        public override void Redo() => this.urs.Redo();

        public UndoRedoEntitySort(
          IDocument doc,
          Layer layer,
          List<IEntity> list,
          EntitySort sort,
          SortMethod method = SortMethod.ByCenter)
        {
            this.Name = "Entity Sort";
            this.doc = doc;
            this.layer = layer;
            this.list = new List<IEntity>((IEnumerable<IEntity>)list);
            this.sort = sort;
            this.method = method;
        }
    }
}
