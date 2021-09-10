using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    public partial class DivideForm : Form
    {
        private BindingList<IEntity> entities = new BindingList<IEntity>();
        private IView view;
        private BoundRect boundRect = new BoundRect();


        public List<BoundRect> Rects { get; private set; }

        public BindingList<IEntity> Entities
        {
            get => this.entities;
            set => this.entities = value;
        }

        public DivideForm(List<IEntity> targetEntities, IView view)
        {
            InitializeComponent();
            this.Rects = new List<BoundRect>();
            this.view = view;
            this.listBox1.DrawMode = DrawMode.OwnerDrawVariable;
            foreach (IEntity targetEntity in targetEntities)
            {
                this.entities.Add(targetEntity);
                this.boundRect.Union(targetEntity.BoundRect);
            }

            this.txtWidth.Text = string.Format("{0:F3}", (object)this.boundRect.Width);
            this.txtHeight.Text = string.Format("{0:F3}", (object)this.boundRect.Height);
            this.listBox1.DataSource = (object)this.entities;
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.DivideForm_KeyDown);
            this.lblWidth.Text = string.Format(" {0:F3} ", (object)this.boundRect.Width);
            this.lblHeight.Text = string.Format(" {0:F3} ", (object)this.boundRect.Height);
            this.CreateRects();
        }

        private void DivideForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;
            this.Close();
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            IEntity entity = (IEntity)(sender as ListBox).Items[e.Index];
            e.DrawBackground();
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.DrawString(string.Format("{0}: {1}", (object)(e.Index + 1), (object)entity.ToString()), this.listBox1.Font, SystemBrushes.HighlightText, (float)e.Bounds.Left, (float)e.Bounds.Top);
            }
            else
            {
                using (SolidBrush solidBrush = new SolidBrush(e.ForeColor))
                    e.Graphics.DrawString(string.Format("{0}: {1}", (object)(e.Index + 1), (object)entity.ToString()), this.listBox1.Font, (Brush)solidBrush, (float)e.Bounds.Left, (float)e.Bounds.Top);
            }
            e.DrawFocusRectangle();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            object selectedItem = this.listBox1.SelectedItem;
        }

        private void listBox1_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            if (e.Index < 0)
                return;
            IEntity entity = (IEntity)(sender as ListBox).Items[e.Index];
            SizeF sizeF = e.Graphics.MeasureString(string.Format("{0}: {1}", (object)e.Index, (object)entity.ToString()), this.listBox1.Font);
            e.ItemHeight = (int)sizeF.Height + 2;
            e.ItemWidth = (int)sizeF.Width;
        }

        private void DivideForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.CreateRects();
            if (!(this.view is ViewDefault view))
                return;
            view.DivideRects = (List<BoundRect>)null;
            view.Render();
        }

        public void Preview()
        {
            if (!(this.view is ViewDefault view))
                return;
            view.DivideRects = this.Rects;
            view.Render();
        }

        private void CreateRects()
        {
            this.Rects.Clear();
            int num1 = (int)this.numRows.Value;
            int num2 = (int)this.numCols.Value;
            float result1;
            float result2;
            float result3;
            if ((1 & (float.TryParse(this.txtWidth.Text, out result1) ? 1 : 0) & (float.TryParse(this.txtHeight.Text, out result2) ? 1 : 0) & (float.TryParse(this.txtOverlap.Text, out result3) ? 1 : 0)) == 0)
                return;
            this.Rects = new List<BoundRect>(num1 * num2);
            float num3 = num2 % 2 != 0 ? (float)-(int)((double)(num2 - 1) / 2.0) * result1 : (float)((double)-(int)((double)num2 / 2.0) * (double)result1 + (double)result1 / 2.0);
            float num4 = num1 % 2 != 0 ? (float)(int)((double)(num1 - 1) / 2.0) * result2 : (float)((double)(int)((double)num1 / 2.0) * (double)result2 - (double)result2 / 2.0);
            for (int index1 = 0; index1 < num1; ++index1)
            {
                for (int index2 = 0; index2 < num2; ++index2)
                {
                    BoundRect boundRect = new BoundRect(0.0f - result3, result2 + result3, result1 + result3, 0.0f - result3);
                    boundRect.Transit(new Vector2((float)(-(double)result1 / 2.0), (float)(-(double)result2 / 2.0)));
                    float x = num3 + result1 * (float)index2;
                    float y = num4 - result2 * (float)index1;
                    boundRect.Transit(new Vector2(x, y));
                    if (this.rdoEntity.Checked)
                        boundRect.Transit(this.boundRect.Center);
                    this.Rects.Add(boundRect);
                }
            }
            this.Preview();
        }

        private void txtWidth_TextChanged(object sender, EventArgs e) => this.CreateRects();


        private void numRows_ValueChanged(object sender, EventArgs e) => this.CreateRects();

        private void numCols_KeyUp(object sender, KeyEventArgs e) => this.CreateRects();

        private void rdoOrigin_VisibleChanged(object sender, EventArgs e) => this.CreateRects();
        private void btnPreview_Click(object sender, EventArgs e) => this.CreateRects();
    }
}
