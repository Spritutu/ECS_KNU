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
    public partial class PasteForm : Form
    {
        /// <summary>클립보드 엔티티 목록</summary>
        public List<IEntity> Clipboard
        {
            set
            {
                this.lbsClipboard.Items.Clear();
                this.lbsClipboard.Items.AddRange((object[])value.ToArray());
                this.txtRowPitch.Text = string.Empty;
                this.txtColPitch.Text = string.Empty;
                BoundRect boundRect = new BoundRect();
                foreach (IEntity entity in value)
                    boundRect.Union(entity.BoundRect);
                if (!boundRect.IsEmpty)
                {
                    this.txtColPitch.Text = string.Format("{0:F3}", (object)boundRect.Width);
                    this.txtRowPitch.Text = string.Format("{0:F3}", (object)boundRect.Height);
                }
                else
                {
                    this.txtColPitch.Text = "1.0";
                    this.txtRowPitch.Text = "1.0";
                }
            }
        }

        /// <summary>붙혀넣기 결과로 생성된 좌표 목록</summary>
        public List<Vector2> Result
        {
            get
            {
                float num1 = float.Parse(this.txtRowPitch.Text);
                float num2 = float.Parse(this.txtColPitch.Text);
                int num3 = int.Parse(this.numRows.Value.ToString());
                int num4 = int.Parse(this.numCols.Value.ToString());
                List<Vector2> vector2List = new List<Vector2>();
                bool flag = this.chkZigZag.Checked;
                float num5 = 0.0f;
                for (int index1 = 0; index1 < num3; ++index1)
                {
                    num5 = 0.0f;
                    float y = (float)index1 * num1;
                    if (flag && index1 % 2 == 1)
                    {
                        for (int index2 = num4 - 1; index2 >= 0; --index2)
                        {
                            float x = (float)index2 * num2;
                            vector2List.Add(new Vector2(x, y));
                        }
                    }
                    else
                    {
                        for (int index2 = 0; index2 < num4; ++index2)
                        {
                            float x = (float)index2 * num2;
                            vector2List.Add(new Vector2(x, y));
                        }
                    }
                }
                return vector2List;
            }
        }

        /// <summary>기준 위치</summary>
        public Vector2 Position => new Vector2(float.Parse(this.txtX.Text), float.Parse(this.txtY.Text));


        public PasteForm()
        {
            InitializeComponent();
        }

        private void PasteForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;
            this.Close();
        }
    }
}
