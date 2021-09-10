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
    public partial class HatchForm : Form
    {
        public HatchMode Mode { get; set; }

        /// <summary>해치 각도</summary>
        public float Angle { get; set; }

        /// <summary>Cross 사용시 두번째 해치 각도</summary>
        public float Angle2 { get; set; }

        /// <summary>해치 간격 (mm)</summary>
        public float Interval { get; set; }

        /// <summary>해치 제외 길이 (mm)</summary>
        public float Exclude { get; set; }

        public HatchForm()
        {
            InitializeComponent();
            this.cbbMode.DataSource = (object)Enum.GetValues(typeof(HatchMode));
            this.Interval = 0.1f;
            this.txtInterval.Text = string.Format("{0:F3}", (object)this.Interval);
            this.Angle = 90f;
            this.txtAngle.Text = string.Format("{0:F3}", (object)this.Angle);
            this.Angle2 = 0.0f;
            this.txtAngle2.Text = string.Format("{0:F3}", (object)this.Angle2);
            this.Exclude = 0.0f;
            this.txtExclude.Text = string.Format("{0}", (object)this.Exclude);
        }

        private void cbbMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            HatchMode result;
            Enum.TryParse<HatchMode>(this.cbbMode.SelectedValue.ToString(), out result);
            this.Mode = result;
        }

        private void txtAngle_TextChanged(object sender, EventArgs e)
        {
            float result;
            if (!float.TryParse(this.txtAngle.Text, out result))
                return;
            this.Angle = result;
        }

        private void txtAngle2_TextChanged(object sender, EventArgs e)
        {
            float result;
            if (!float.TryParse(this.txtAngle2.Text, out result))
                return;
            this.Angle2 = result;
        }

        private void txtInterval_TextChanged(object sender, EventArgs e)
        {
            float result;
            if (!float.TryParse(this.txtInterval.Text, out result))
                return;
            this.Interval = result;
        }

        private void txtExclude_TextChanged(object sender, EventArgs e)
        {
            float result;
            if (!float.TryParse(this.txtExclude.Text, out result))
                return;
            this.Exclude = result;
        }

        private void HatchForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;
            this.Close();
        }
    }
}
