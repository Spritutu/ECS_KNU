
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scanlab.Sirius
{
    /// <summary>문서 확장 파일 브라우저</summary>
    internal class DocExtFileBrowser : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            openFileDialog.Title = "Document Extension File ...";
            openFileDialog.Filter = "extension file (*.ini)|*.ini|text file (*.txt)|*.txt| All Files (*.*)|*.*";
        }
    }
}

