using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>레이어 이동 (TreeView 에서 마우스로 Drag and Drop)</summary>
    internal class UndoRedoLayerMove : UndoRedoSingle
    {
        private IDocument doc;
        private Layer sourceLayer;
        private Layer targetLayer;
        private int sourceIndex;
        private int targetIndex;

        public override void Execute() => this.Redo();

        public override void Undo()
        {
            this.doc.Layers.Remove(this.sourceLayer);
            this.doc.Layers.Insert(this.sourceIndex, this.sourceLayer);
        }

        public override void Redo()
        {
            this.doc.Layers.Remove(this.sourceLayer);
            this.doc.Layers.Insert(this.targetIndex, this.sourceLayer);
        }

        public UndoRedoLayerMove(IDocument doc, Layer sourceLayer, Layer targetLayer, int targetIndex)
        {
            this.Name = "Layer Move";
            this.doc = doc;
            this.sourceLayer = sourceLayer;
            this.sourceIndex = sourceLayer.Node.Index;
            this.targetLayer = targetLayer;
            this.targetIndex = targetIndex;
        }
    }
}
