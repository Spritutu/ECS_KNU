
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scanlab.Sirius
{
    /// <summary>바코드 개별셀의 패턴을 외부 시리우스 파일로 처리</summary>
    internal class BarcodePatternFileBrowser : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            openFileDialog.Title = "Import Sirius Pattern File ...";
            openFileDialog.Filter = "sirius data file (*.sirius)|*.sirius|All Files (*.*)|*.*";
        }
    }
}
