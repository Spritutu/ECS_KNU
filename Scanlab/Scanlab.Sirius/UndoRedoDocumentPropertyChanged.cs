
namespace Scanlab.Sirius
{
    /// <summary>문서 속성 변경</summary>
    internal class UndoRedoDocumentPropertyChanged : UndoRedoSingle
    {
        private IDocument doc;
        private string propName;
        private object oldValue;
        private object newValue;

        public override void Execute() => this.Redo();

        public override void Undo() => this.doc.GetType().GetProperty(this.propName).SetValue((object)this.doc, this.oldValue);

        public override void Redo() => this.doc.GetType().GetProperty(this.propName).SetValue((object)this.doc, this.newValue);

        public UndoRedoDocumentPropertyChanged(
          IDocument doc,
          string propName,
          object oldValue,
          object newValue)
        {
            this.Name = "Document Property Changed";
            this.doc = doc;
            this.propName = propName;
            this.oldValue = oldValue;
            this.newValue = newValue;
        }
    }
}
