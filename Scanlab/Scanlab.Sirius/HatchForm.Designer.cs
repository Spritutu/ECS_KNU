
namespace Scanlab.Sirius
{
    partial class HatchForm
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
            this.cbbMode = new System.Windows.Forms.ComboBox();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.txtAngle = new System.Windows.Forms.TextBox();
            this.txtAngle2 = new System.Windows.Forms.TextBox();
            this.txtExclude = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 12);
            this.label1.TabIndex = 54;
            this.label1.Text = "Mode :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 144);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 12);
            this.label2.TabIndex = 56;
            this.label2.Text = "2nd Angle :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 47;
            this.label3.Text = "Interval :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 50;
            this.label4.Text = "Exclude :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 12);
            this.label5.TabIndex = 46;
            this.label5.Text = "Angle :";
            // 
            // cbbMode
            // 
            this.cbbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbMode.FormattingEnabled = true;
            this.cbbMode.Location = new System.Drawing.Point(88, 13);
            this.cbbMode.Name = "cbbMode";
            this.cbbMode.Size = new System.Drawing.Size(143, 20);
            this.cbbMode.TabIndex = 53;
            this.cbbMode.SelectedIndexChanged += new System.EventHandler(this.cbbMode_SelectedIndexChanged);
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(88, 45);
            this.txtInterval.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.Size = new System.Drawing.Size(80, 21);
            this.txtInterval.TabIndex = 40;
            this.txtInterval.Text = "0.1";
            this.txtInterval.TextChanged += new System.EventHandler(this.txtInterval_TextChanged);
            // 
            // txtAngle
            // 
            this.txtAngle.Location = new System.Drawing.Point(88, 107);
            this.txtAngle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAngle.Name = "txtAngle";
            this.txtAngle.Size = new System.Drawing.Size(80, 21);
            this.txtAngle.TabIndex = 39;
            this.txtAngle.Text = "0.0";
            this.txtAngle.TextChanged += new System.EventHandler(this.txtAngle_TextChanged);
            // 
            // txtAngle2
            // 
            this.txtAngle2.Location = new System.Drawing.Point(88, 137);
            this.txtAngle2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtAngle2.Name = "txtAngle2";
            this.txtAngle2.Size = new System.Drawing.Size(80, 21);
            this.txtAngle2.TabIndex = 57;
            this.txtAngle2.Text = "90.0";
            this.txtAngle2.TextChanged += new System.EventHandler(this.txtAngle2_TextChanged);
            // 
            // txtExclude
            // 
            this.txtExclude.Location = new System.Drawing.Point(88, 76);
            this.txtExclude.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtExclude.Name = "txtExclude";
            this.txtExclude.Size = new System.Drawing.Size(80, 21);
            this.txtExclude.TabIndex = 48;
            this.txtExclude.Text = "0.0";
            this.txtExclude.TextChanged += new System.EventHandler(this.txtExclude_TextChanged);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(65, 187);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 30);
            this.button1.TabIndex = 58;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(155, 187);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 30);
            this.button2.TabIndex = 45;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // HatchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 233);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtExclude);
            this.Controls.Add(this.txtAngle2);
            this.Controls.Add(this.txtAngle);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.cbbMode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "HatchForm";
            this.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Text = "Hatch";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.HatchForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbbMode;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.TextBox txtAngle;
        private System.Windows.Forms.TextBox txtAngle2;
        private System.Windows.Forms.TextBox txtExclude;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
    }
}