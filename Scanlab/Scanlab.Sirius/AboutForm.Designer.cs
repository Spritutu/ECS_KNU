
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Scanlab.Sirius
{

    partial class AboutForm : Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        /// 
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AboutForm));
            this.textBox1 = new TextBox();
            this.button1 = new Button();
            this.SuspendLayout();
            this.textBox1.Location = new System.Drawing.Point(7, 7);
            this.textBox1.Margin = new Padding(8, 9, 8, 9);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = ScrollBars.Vertical;
            this.textBox1.Size = new Size(358, 113);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "2020 Copyright To (c)SPIRAL LAB.\r\nSirius library is a trademark of spirallab.\r\nContact to E-mail: labspiral@gmail.com\r\nHomepage  http://www.spirallab.co.kr";
            this.button1.DialogResult = DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(285, 132);
            this.button1.Name = "button1";
            this.button1.Size = new Size(80, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            this.AcceptButton = (IButtonControl)this.button1;
            this.AutoScaleDimensions = new SizeF(7f, 16f);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(372, 173);
            this.Controls.Add((Control)this.button1);
            this.Controls.Add((Control)this.textBox1);
            this.Font = new Font("Arial", 9.75f, FontStyle.Regular, GraphicsUnit.Point, (byte)0);
            this.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
            this.Margin = new Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = nameof(AboutForm);
            this.Opacity = 0.98;
            this.SizeGripStyle = SizeGripStyle.Hide;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Text = "About - (c)SpiralLab";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
    }
}