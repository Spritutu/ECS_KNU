using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    public partial class RotateForm : Form
    {
        private IDocument doc;

        public RotateForm(IDocument doc)
        {
            InitializeComponent();
            this.doc = doc;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.RotateForm_KeyDown);
        }

        private void RotateForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            float cx = float.Parse(this.txtX.Text);
            float cy = float.Parse(this.txtY.Text);
            this.doc.Action.ActEntityRotate(this.doc.Action.SelectedEntity, float.Parse(this.txtAngle.Text), cx, cy);
        }

        private void btnCancel_Click(object sender, EventArgs e) => this.Close();

    }
}
