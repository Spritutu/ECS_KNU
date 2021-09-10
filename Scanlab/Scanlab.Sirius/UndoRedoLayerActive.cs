
namespace Scanlab.Sirius
{
    /// <summary>선택 레이어 활성화</summary>
    internal class UndoRedoLayerActive : UndoRedoSingle
    {
        private IDocument doc;
        private Layer layer;
        private Layer old;

        public override void Execute()
        {
            this.old = this.doc.Layers.Active;
            this.doc.Layers.Active = this.layer;
        }

        public override void Undo() => this.doc.Layers.Active = this.old;

        public override void Redo() => this.doc.Layers.Active = this.layer;

        public UndoRedoLayerActive(IDocument doc, Layer layer)
        {
            this.Name = "Layer Active";
            this.doc = doc;
            this.layer = layer;
        }
    }
}
