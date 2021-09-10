

namespace Scanlab.Sirius
{
    /// <summary>엔티티 해치하기</summary>
    internal class UndoRedoEntityHatch : UndoRedoSingle
    {
        private IDocument doc;
        private IEntity entity;
        private Layer layer;
        private HatchMode mode;
        private float angle;
        private float angle2;
        private float interval;
        private float exclude;
        private UndoRedoMultiple urs;

        public override void Execute() => this.Redo();

        public override void Undo() => this.urs.Undo();

        public override void Redo()
        {
            this.urs = new UndoRedoMultiple();
            if (this.entity is IHatchable)
            {
                Group group = (this.entity as IHatchable).Hatch(this.mode, this.angle, this.angle2, this.interval, this.exclude);
                if (group.Count != 0)
                    this.urs.Add((IUndoRedo)new UndoRedoEntityAdd(this.doc, this.layer, (IEntity)group));
            }
            this.urs.Redo();
        }

        public UndoRedoEntityHatch(
          IDocument doc,
          IEntity entity,
          Layer layer,
          HatchMode mode,
          float angle,
          float angle2,
          float interval,
          float exclude)
        {
            this.Name = "Entity Hatch";
            this.doc = doc;
            this.entity = entity;
            this.layer = layer;
            this.mode = mode;
            this.angle = angle;
            this.angle2 = angle2;
            this.interval = interval;
            this.exclude = exclude;
        }
    }
}
