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
    public partial class AboutForm : Form
    {
        private TextBox textBox1;
        private Button button1;

        public AboutForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.AboutForm_KeyDown);
        }

        private void AboutForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;
            this.Close();
        }
    }
}
