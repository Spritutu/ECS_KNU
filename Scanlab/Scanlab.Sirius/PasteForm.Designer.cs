
namespace Scanlab.Sirius
{
    partial class PasteForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.txtY = new System.Windows.Forms.TextBox();
            this.txtX = new System.Windows.Forms.TextBox();
            this.chkZigZag = new System.Windows.Forms.CheckBox();
            this.txtColPitch = new System.Windows.Forms.TextBox();
            this.txtRowPitch = new System.Windows.Forms.TextBox();
            this.numRows = new System.Windows.Forms.NumericUpDown();
            this.numCols = new System.Windows.Forms.NumericUpDown();
            this.lbsClipboard = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.numRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCols)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 12);
            this.label1.TabIndex = 22;
            this.label1.Text = "Clipboard :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 12);
            this.label2.TabIndex = 23;
            this.label2.Text = "Rows :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(307, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 12);
            this.label3.TabIndex = 24;
            this.label3.Text = "Cols :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(221, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 12);
            this.label4.TabIndex = 29;
            this.label4.Text = "Row Pitch :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(307, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 12);
            this.label5.TabIndex = 30;
            this.label5.Text = "Col Pitch :";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(309, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(54, 12);
            this.label6.TabIndex = 34;
            this.label6.Text = "Base Y :";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(223, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 12);
            this.label7.TabIndex = 33;
            this.label7.Text = "Base X :";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(221, 215);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 27;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(312, 215);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 28;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // txtY
            // 
            this.txtY.Location = new System.Drawing.Point(312, 137);
            this.txtY.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtY.Name = "txtY";
            this.txtY.Size = new System.Drawing.Size(80, 21);
            this.txtY.TabIndex = 32;
            this.txtY.Text = "0.0";
            // 
            // txtX
            // 
            this.txtX.Location = new System.Drawing.Point(226, 137);
            this.txtX.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtX.Name = "txtX";
            this.txtX.Size = new System.Drawing.Size(80, 21);
            this.txtX.TabIndex = 31;
            this.txtX.Text = "0.0";
            // 
            // chkZigZag
            // 
            this.chkZigZag.AutoSize = true;
            this.chkZigZag.Location = new System.Drawing.Point(224, 173);
            this.chkZigZag.Name = "chkZigZag";
            this.chkZigZag.Size = new System.Drawing.Size(70, 16);
            this.chkZigZag.TabIndex = 35;
            this.chkZigZag.Text = "Zig-Zag";
            this.chkZigZag.UseVisualStyleBackColor = true;
            // 
            // txtColPitch
            // 
            this.txtColPitch.Location = new System.Drawing.Point(310, 85);
            this.txtColPitch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtColPitch.Name = "txtColPitch";
            this.txtColPitch.Size = new System.Drawing.Size(80, 21);
            this.txtColPitch.TabIndex = 21;
            // 
            // txtRowPitch
            // 
            this.txtRowPitch.Location = new System.Drawing.Point(224, 85);
            this.txtRowPitch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRowPitch.Name = "txtRowPitch";
            this.txtRowPitch.Size = new System.Drawing.Size(80, 21);
            this.txtRowPitch.TabIndex = 20;
            // 
            // numRows
            // 
            this.numRows.Location = new System.Drawing.Point(224, 33);
            this.numRows.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRows.Name = "numRows";
            this.numRows.Size = new System.Drawing.Size(80, 21);
            this.numRows.TabIndex = 19;
            this.numRows.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numCols
            // 
            this.numCols.Location = new System.Drawing.Point(310, 33);
            this.numCols.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numCols.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCols.Name = "numCols";
            this.numCols.Size = new System.Drawing.Size(80, 21);
            this.numCols.TabIndex = 20;
            this.numCols.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lbsClipboard
            // 
            this.lbsClipboard.Enabled = false;
            this.lbsClipboard.FormattingEnabled = true;
            this.lbsClipboard.ItemHeight = 12;
            this.lbsClipboard.Location = new System.Drawing.Point(12, 33);
            this.lbsClipboard.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.lbsClipboard.Name = "lbsClipboard";
            this.lbsClipboard.Size = new System.Drawing.Size(203, 208);
            this.lbsClipboard.TabIndex = 0;
            // 
            // PasteForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(397, 247);
            this.Controls.Add(this.lbsClipboard);
            this.Controls.Add(this.numCols);
            this.Controls.Add(this.numRows);
            this.Controls.Add(this.chkZigZag);
            this.Controls.Add(this.txtRowPitch);
            this.Controls.Add(this.txtColPitch);
            this.Controls.Add(this.txtX);
            this.Controls.Add(this.txtY);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasteForm";
            this.Text = "Paste Wizard";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PasteForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.numRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCols)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.CheckBox chkZigZag;
        private System.Windows.Forms.TextBox txtColPitch;
        private System.Windows.Forms.TextBox txtRowPitch;
        private System.Windows.Forms.NumericUpDown numRows;
        private System.Windows.Forms.NumericUpDown numCols;
        private System.Windows.Forms.ListBox lbsClipboard;
    }
}