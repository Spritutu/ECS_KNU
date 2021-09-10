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
    public partial class PropertyForm : Form
    {
        public PropertyGrid PropertyGrid => this.propertyGrid1;

        public PropertyForm(object obj)
        {
            InitializeComponent();
            this.propertyGrid1.PropertySort = PropertySort.Categorized;
            this.Text = obj.ToString() ?? "";
            this.propertyGrid1.SelectedObject = obj;
            this.KeyPreview = true;
        }

        private void PropertyForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;
            this.Close();
        }
    }
}
