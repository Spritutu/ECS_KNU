// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.AlcPositionFileBrowser
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scanlab.Sirius
{
    /// <summary>RTC 의 Position Dep. 용 보정 파일</summary>
    internal class AlcPositionFileBrowser : FileNameEditor
    {
        protected override void InitializeDialog(OpenFileDialog openFileDialog)
        {
            base.InitializeDialog(openFileDialog);
            openFileDialog.Title = "Automatic Laser Control By Position Dependent";
            openFileDialog.Filter = "position dep file (*.txt)|*.txt|All Files (*.*)|*.*";
        }
    }
}
