
using System.Collections.Generic;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 정렬</summary>
    internal class UndoRedoEntityAlign : UndoRedoSingle
    {
        private IDocument doc;
        private List<UndoRedoEntityAlignPair> transit;
        private List<IEntity> list;
        private EntityAlign align;

        public override void Execute()
        {
            this.transit = new List<UndoRedoEntityAlignPair>(this.list.Count);
            BoundRect boundRect = new BoundRect();
            foreach (IEntity entity in this.list)
                boundRect.Union(entity.BoundRect);
            if (1 == this.list.Count && !boundRect.IsEmpty)
            {
                Vector2 vector2 = Vector2.Zero;
                switch (this.align)
                {
                    case EntityAlign.Left:
                        if ((double)this.doc.Dimension.Width > 0.0 && (double)this.doc.Dimension.Height > 0.0)
                        {
                            vector2.X = this.doc.Dimension.Left - boundRect.Left;
                            vector2.Y = 0.0f;
                            break;
                        }
                        break;
                    case EntityAlign.Right:
                        if ((double)this.doc.Dimension.Width > 0.0 && (double)this.doc.Dimension.Height > 0.0)
                        {
                            vector2.X = this.doc.Dimension.Right - boundRect.Right;
                            vector2.Y = 0.0f;
                            break;
                        }
                        break;
                    case EntityAlign.Origin:
                        vector2 = this.doc.Dimension.Center - boundRect.Center;
                        break;
                    case EntityAlign.Top:
                        if ((double)this.doc.Dimension.Width > 0.0 && (double)this.doc.Dimension.Height > 0.0)
                        {
                            vector2.X = 0.0f;
                            vector2.Y = this.doc.Dimension.Top - boundRect.Top;
                            break;
                        }
                        break;
                    case EntityAlign.Bottom:
                        if ((double)this.doc.Dimension.Width > 0.0 && (double)this.doc.Dimension.Height > 0.0)
                        {
                            vector2.X = 0.0f;
                            vector2.Y = this.doc.Dimension.Bottom - boundRect.Bottom;
                            break;
                        }
                        break;
                }
                IDrawable drawable = this.list[0] as IDrawable;
                this.transit.Add(new UndoRedoEntityAlignPair()
                {
                    Drawable = drawable,
                    Delta = vector2
                });
            }
            else
            {
                foreach (IEntity entity in this.list)
                {
                    IDrawable drawable = entity as IDrawable;
                    Vector2 zero = Vector2.Zero;
                    if (!entity.BoundRect.IsEmpty && drawable != null)
                    {
                        switch (this.align)
                        {
                            case EntityAlign.Left:
                                zero.X = entity.BoundRect.Left - boundRect.Left;
                                zero.Y = 0.0f;
                                break;
                            case EntityAlign.Right:
                                zero.X = entity.BoundRect.Right - boundRect.Right;
                                zero.Y = 0.0f;
                                break;
                            case EntityAlign.Origin:
                                zero.X = boundRect.Center.X - this.doc.Dimension.Center.X;
                                zero.Y = boundRect.Center.Y - this.doc.Dimension.Center.Y;
                                break;
                            case EntityAlign.Top:
                                zero.X = 0.0f;
                                zero.Y = entity.BoundRect.Top - boundRect.Top;
                                break;
                            case EntityAlign.Bottom:
                                zero.X = 0.0f;
                                zero.Y = entity.BoundRect.Bottom - boundRect.Bottom;
                                break;
                        }
                        this.transit.Add(new UndoRedoEntityAlignPair()
                        {
                            Drawable = drawable,
                            Delta = -zero
                        });
                    }
                }
            }
            this.Redo();
        }

        public override void Undo()
        {
            foreach (UndoRedoEntityAlignPair redoEntityAlignPair in this.transit)
                redoEntityAlignPair.Drawable.Transit(-redoEntityAlignPair.Delta);
        }

        public override void Redo()
        {
            foreach (UndoRedoEntityAlignPair redoEntityAlignPair in this.transit)
                redoEntityAlignPair.Drawable.Transit(redoEntityAlignPair.Delta);
        }

        public UndoRedoEntityAlign(IDocument doc, List<IEntity> list, EntityAlign align)
        {
            this.Name = "Entity Align";
            this.doc = doc;
            this.list = new List<IEntity>((IEnumerable<IEntity>)list);
            this.align = align;
        }
    }
}
