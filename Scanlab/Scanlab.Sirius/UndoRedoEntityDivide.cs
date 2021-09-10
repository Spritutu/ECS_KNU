
using System.Collections.Generic;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>엔티티 나누기</summary>
    internal class UndoRedoEntityDivide : UndoRedoSingle
    {
        private IDocument doc;
        private List<IEntity> entities;
        private List<BoundRect> rects;
        private List<Group> results;
        private Layer layer;
        private UndoRedoMultiple urs;
        private LwPolyLineVertex vCurrent;
        private LwPolyLineVertex vPrev;

        public override void Execute() => this.Redo();

        public override void Undo() => this.urs.Undo();

        public override void Redo()
        {
            this.urs = new UndoRedoMultiple();
            this.results = new List<Group>();
            for (int index = 0; index < this.rects.Count; ++index)
            {
                Group group = new Group();
                group.Name = string.Format("Divided {0}", (object)(index + 1));
                group.IsEnableFastRendering = true;
                foreach (IEntity entity in this.entities)
                {
                    Group dividedGroup;
                    if (entity is IDrawable && this.DivideEntity(entity, this.rects[index], out dividedGroup))
                        group.Add((IEntity)dividedGroup);
                }
                if (group.Count > 0)
                {
                    group.Regen();
                    this.results.Add(group);
                }
            }
            this.urs.Add((IUndoRedo)new UndoRedoEntityDelete(this.doc, this.entities));
            foreach (IEntity result in this.results)
                this.urs.Add((IUndoRedo)new UndoRedoEntityAdd(this.doc, this.layer, result));
            this.urs.Redo();
        }

        private bool DivideEntity(IEntity entity, BoundRect rect, out Group dividedGroup)
        {
            dividedGroup = new Group();
            dividedGroup.Name = "Divided";
            if (!MathHelper.IntersectRectInRect(rect, entity.BoundRect, 0.005) && !MathHelper.CollisionRectWithRect(rect, entity.BoundRect))
                return false;
            switch (entity.EntityType)
            {
                case EType.Point:
                    dividedGroup.Add((IEntity)((Point)entity).Clone());
                    break;
                case EType.Line:
                    Line line1 = entity as Line;
                    List<Vector2> collisions;
                    if (MathHelper.IntersectLineInRectWithCollision(rect, (double)line1.Start.X, (double)line1.Start.Y, (double)line1.End.X, (double)line1.End.Y, out collisions))
                    {
                        if (collisions.Count == 0)
                        {
                            dividedGroup.Add((IEntity)((Line)entity).Clone());
                            break;
                        }
                        if (1 == collisions.Count)
                        {
                            if (MathHelper.IntersectPointInRect(rect, (double)line1.Start.X, (double)line1.Start.Y, 0.0))
                            {
                                Line line2 = new Line(line1.Start.X, line1.Start.Y, collisions[0].X, collisions[0].Y);
                                dividedGroup.Add((IEntity)line2);
                                break;
                            }
                            Line line3 = new Line(collisions[0].X, collisions[0].Y, line1.End.X, line1.End.Y);
                            dividedGroup.Add((IEntity)line3);
                            break;
                        }
                        if (2 == collisions.Count)
                        {
                            Line line2 = new Line(collisions[0].X, collisions[0].Y, collisions[1].X, collisions[1].Y);
                            dividedGroup.Add((IEntity)line2);
                            break;
                        }
                        break;
                    }
                    break;
                case EType.Arc:
                    List<LwPolyline> subLwPolylines1;
                    this.DivideLwPolyline(((Arc)entity).ToLwPolyline(), rect, out subLwPolylines1);
                    using (List<LwPolyline>.Enumerator enumerator = subLwPolylines1.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            LwPolyline current = enumerator.Current;
                            dividedGroup.Add((IEntity)current);
                        }
                        break;
                    }
                case EType.Circle:
                    List<LwPolyline> subLwPolylines2;
                    this.DivideLwPolyline(((Circle)entity).ToLwPolyline(), rect, out subLwPolylines2);
                    using (List<LwPolyline>.Enumerator enumerator = subLwPolylines2.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            LwPolyline current = enumerator.Current;
                            dividedGroup.Add((IEntity)current);
                        }
                        break;
                    }
                case EType.Rectangle:
                    List<LwPolyline> subLwPolylines3;
                    this.DivideLwPolyline(((Rectangle)entity).ToLwPolyline(), rect, out subLwPolylines3);
                    using (List<LwPolyline>.Enumerator enumerator = subLwPolylines3.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            LwPolyline current = enumerator.Current;
                            dividedGroup.Add((IEntity)current);
                        }
                        break;
                    }
                case EType.LWPolyline:
                    List<Vertex> vertices = ((LwPolyline)entity).ToVertices();
                    LwPolyline sourceLwPolyline = new LwPolyline();
                    foreach (Vertex vertex in vertices)
                        sourceLwPolyline.Add(vertex.ToPolylineVertex());
                    sourceLwPolyline.Regen();

                    if (sourceLwPolyline.Count == 0) break;

                    List<LwPolyline> subLwPolylines4;
                    this.DivideLwPolyline(sourceLwPolyline, rect, out subLwPolylines4);
                    using (List<LwPolyline>.Enumerator enumerator = subLwPolylines4.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            LwPolyline current = enumerator.Current;
                            dividedGroup.Add((IEntity)current);
                        }
                        break;
                    }
                case EType.Group:
                    using (IEnumerator<IEntity> enumerator = (entity as Group).GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            Group dividedGroup1;
                            if (this.DivideEntity(enumerator.Current, rect, out dividedGroup1))
                                dividedGroup.Add((IEntity)dividedGroup1);
                        }
                        break;
                    }
            }
            dividedGroup.Regen();
            return dividedGroup.Count != 0;
        }

        /// <summary>폴리라인은 bulge 가 모두 분해된 채로 전달되어야 한다</summary>
        /// <param name="sourceLwPolyline"></param>
        /// <param name="rect"></param>
        /// <param name="subLwPolylines"></param>
        private void DivideLwPolyline(
          LwPolyline sourceLwPolyline,
          BoundRect rect,
          out List<LwPolyline> subLwPolylines)
        {
            subLwPolylines = new List<LwPolyline>();
            LwPolyline subLwPolyline = new LwPolyline();
            this.vPrev = sourceLwPolyline[0];
            bool isInside = MathHelper.IntersectPointInRect(rect, (double)this.vPrev.X, (double)this.vPrev.Y, 0.0);
            if (isInside)
                subLwPolyline.Add(this.vPrev);
            bool isLast1 = false;
            for (int index = 1; index < sourceLwPolyline.Count; ++index)
            {
                if (!sourceLwPolyline.IsClosed && index == sourceLwPolyline.Count - 1)
                    isLast1 = true;
                this.PolyLineVertex(rect, ref subLwPolyline, sourceLwPolyline[index], ref isInside, ref subLwPolylines, isLast1);
            }
            if (sourceLwPolyline.IsClosed)
            {
                bool isLast2 = true;
                this.PolyLineVertex(rect, ref subLwPolyline, sourceLwPolyline[0], ref isInside, ref subLwPolylines, isLast2);
            }
            if (subLwPolyline.Count <= 0)
                return;
            subLwPolyline.Regen();
            subLwPolylines.Add(subLwPolyline);
        }

        private void PolyLineVertex(
          BoundRect rect,
          ref LwPolyline subLwPolyline,
          LwPolyLineVertex v,
          ref bool isInside,
          ref List<LwPolyline> subLwPolylines,
          bool isLast = false)
        {
            this.vCurrent = v;
            if (MathHelper.IntersectPointInRect(rect, (double)this.vCurrent.X, (double)this.vCurrent.Y, 0.0))
            {
                if (isInside)
                {
                    subLwPolyline.Add(this.vCurrent);
                }
                else
                {
                    List<Vector2> collisions;
                    MathHelper.IntersectLineInRectWithCollision(rect, (double)this.vPrev.X, (double)this.vPrev.Y, (double)this.vCurrent.X, (double)this.vCurrent.Y, out collisions);
                    if (collisions.Count >= 1)
                    {
                        if (collisions.Count == 1)
                        {
                            subLwPolyline.Add(new LwPolyLineVertex(collisions[0].X, collisions[0].Y));
                            subLwPolyline.Add(this.vCurrent);
                        }
                        else if (collisions.Count == 2)
                        {
                            foreach (Vector2 vector2 in collisions)
                                subLwPolyline.Add(new LwPolyLineVertex(vector2.X, vector2.Y));
                        }
                    }
                    else
                        subLwPolyline.Add(this.vCurrent);
                }
                isInside = true;
            }
            else
            {
                if (isInside)
                {
                    List<Vector2> collisions;
                    if (MathHelper.IntersectLineInRectWithCollision(rect, (double)this.vPrev.X, (double)this.vPrev.Y, (double)this.vCurrent.X, (double)this.vCurrent.Y, out collisions))
                    {
                        if (collisions.Count == 1)
                            subLwPolyline.Add(new LwPolyLineVertex(collisions[0].X, collisions[0].Y));
                        else if (collisions.Count == 2)
                        {
                            subLwPolyline.RemoveAt(subLwPolyline.Count - 1);
                            foreach (Vector2 vector2 in collisions)
                                subLwPolyline.Add(new LwPolyLineVertex(vector2.X, vector2.Y));
                        }
                        subLwPolyline.Regen();
                        subLwPolylines.Add(subLwPolyline);
                        subLwPolyline = new LwPolyline();
                    }
                }
                else
                {
                    List<Vector2> collisions;
                    if (MathHelper.IntersectLineInRectWithCollision(rect, (double)this.vPrev.X, (double)this.vPrev.Y, (double)this.vCurrent.X, (double)this.vCurrent.Y, out collisions))
                    {
                        subLwPolyline.Add(new LwPolyLineVertex(collisions[0].X, collisions[0].Y));
                        subLwPolyline.Add(new LwPolyLineVertex(collisions[1].X, collisions[1].Y));
                        subLwPolyline.Regen();
                        subLwPolylines.Add(subLwPolyline);
                        subLwPolyline = new LwPolyline();
                    }
                }
                if (isLast && subLwPolyline.Count > 0)
                {
                    subLwPolyline.Regen();
                    subLwPolylines.Add(subLwPolyline);
                }
                isInside = false;
            }
            this.vPrev = this.vCurrent;
        }

        public UndoRedoEntityDivide(
          IDocument doc,
          Layer layer,
          List<IEntity> entities,
          List<BoundRect> rects)
        {
            this.Name = "Entity Divide";
            this.doc = doc;
            this.layer = layer;
            this.entities = new List<IEntity>((IEnumerable<IEntity>)entities);
            this.rects = new List<BoundRect>((IEnumerable<BoundRect>)rects);
        }
    }
}
