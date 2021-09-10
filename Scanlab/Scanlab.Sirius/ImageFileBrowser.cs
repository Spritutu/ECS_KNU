using System;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scanlab.Sirius
{
    /// <summary>시스템에서 지원하는 이미지 코덱 목록</summary>
    internal class ImageFileBrowser : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            openFileDialog.Title = "Image File ...";
            string str1 = string.Empty;
            openFileDialog.Filter = "";
            foreach (ImageCodecInfo imageCodecInfo in imageEncoders)
            {
                string str2 = imageCodecInfo.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                openFileDialog.Filter = string.Format("{0}{1}{2} ({3})|{3}", (object)openFileDialog.Filter, (object)str1, (object)str2, (object)imageCodecInfo.FilenameExtension);
                str1 = "|";
            }
            openFileDialog.Filter = string.Format("{0}{1}{2} ({3})|{3}", (object)openFileDialog.Filter, (object)str1, (object)"All Files", (object)"*.*");
            openFileDialog.DefaultExt = ".bmp";
            openFileDialog.Title = "Import Image File";
            openFileDialog.FileName = string.Empty;
        }
    }
}
