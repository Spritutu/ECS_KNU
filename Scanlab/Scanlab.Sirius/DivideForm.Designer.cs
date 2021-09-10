
namespace Scanlab.Sirius
{
    partial class DivideForm
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtHeight = new System.Windows.Forms.TextBox();
            this.txtWidth = new System.Windows.Forms.TextBox();
            this.numRows = new System.Windows.Forms.NumericUpDown();
            this.numCols = new System.Windows.Forms.NumericUpDown();
            this.txtOverlap = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblWidth = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblHeight = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoOrigin = new System.Windows.Forms.RadioButton();
            this.rdoEntity = new System.Windows.Forms.RadioButton();
            this.btnPreview = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCols)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new System.Drawing.Point(8, 9);
            this.listBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(234, 388);
            this.listBox1.TabIndex = 21;
            this.listBox1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox1_DrawItem);
            this.listBox1.MeasureItem += new System.Windows.Forms.MeasureItemEventHandler(this.listBox1_MeasureItem);
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(251, 407);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(90, 40);
            this.button1.TabIndex = 23;
            this.button1.Text = "&Ok";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(353, 407);
            this.button2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(90, 40);
            this.button2.TabIndex = 23;
            this.button2.Text = "&Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(251, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 16);
            this.label1.TabIndex = 54;
            this.label1.Text = "Overlap Length :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 16);
            this.label2.TabIndex = 50;
            this.label2.Text = "Cell Rows :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(251, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 16);
            this.label3.TabIndex = 51;
            this.label3.Text = "Cell Cols :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(353, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 16);
            this.label4.TabIndex = 52;
            this.label4.Text = "Cell Width :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(353, 89);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 16);
            this.label5.TabIndex = 51;
            this.label5.Text = "Cell Height :";
            // 
            // txtHeight
            // 
            this.txtHeight.Location = new System.Drawing.Point(353, 113);
            this.txtHeight.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new System.Drawing.Size(90, 22);
            this.txtHeight.TabIndex = 49;
            this.txtHeight.Text = "10";
            this.txtHeight.TextChanged += new System.EventHandler(this.txtWidth_TextChanged);
            // 
            // txtWidth
            // 
            this.txtWidth.Location = new System.Drawing.Point(353, 36);
            this.txtWidth.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new System.Drawing.Size(90, 22);
            this.txtWidth.TabIndex = 48;
            this.txtWidth.Text = "10";
            this.txtWidth.TextChanged += new System.EventHandler(this.txtWidth_TextChanged);
            // 
            // numRows
            // 
            this.numRows.Location = new System.Drawing.Point(251, 113);
            this.numRows.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.numRows.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRows.Name = "numRows";
            this.numRows.Size = new System.Drawing.Size(90, 22);
            this.numRows.TabIndex = 47;
            this.numRows.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRows.ValueChanged += new System.EventHandler(this.numRows_ValueChanged);
            this.numRows.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numCols_KeyUp);
            // 
            // numCols
            // 
            this.numCols.Location = new System.Drawing.Point(251, 36);
            this.numCols.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.numCols.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCols.Name = "numCols";
            this.numCols.Size = new System.Drawing.Size(90, 22);
            this.numCols.TabIndex = 46;
            this.numCols.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCols.ValueChanged += new System.EventHandler(this.numRows_ValueChanged);
            this.numCols.KeyUp += new System.Windows.Forms.KeyEventHandler(this.numCols_KeyUp);
            // 
            // txtOverlap
            // 
            this.txtOverlap.Location = new System.Drawing.Point(251, 196);
            this.txtOverlap.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtOverlap.Name = "txtOverlap";
            this.txtOverlap.Size = new System.Drawing.Size(90, 22);
            this.txtOverlap.TabIndex = 54;
            this.txtOverlap.Text = "0.0";
            this.txtOverlap.TextChanged += new System.EventHandler(this.txtWidth_TextChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel2,
            this.lblWidth,
            this.toolStripStatusLabel4,
            this.lblHeight});
            this.statusStrip1.Location = new System.Drawing.Point(3, 459);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(444, 22);
            this.statusStrip1.TabIndex = 56;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(84, 17);
            this.toolStripStatusLabel2.Text = " Width (mm) ";
            // 
            // lblWidth
            // 
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(54, 17);
            this.lblWidth.Text = "(Empty)";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(87, 17);
            this.toolStripStatusLabel4.Text = " Height (mm) ";
            // 
            // lblHeight
            // 
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(54, 17);
            this.lblHeight.Text = "(Empty)";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoOrigin);
            this.groupBox1.Controls.Add(this.rdoEntity);
            this.groupBox1.Location = new System.Drawing.Point(251, 252);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(192, 147);
            this.groupBox1.TabIndex = 57;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = " Reference Location ";
            // 
            // rdoOrigin
            // 
            this.rdoOrigin.AutoSize = true;
            this.rdoOrigin.Location = new System.Drawing.Point(18, 55);
            this.rdoOrigin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoOrigin.Name = "rdoOrigin";
            this.rdoOrigin.Size = new System.Drawing.Size(60, 20);
            this.rdoOrigin.TabIndex = 1;
            this.rdoOrigin.Text = "Origin";
            this.rdoOrigin.UseVisualStyleBackColor = true;
            this.rdoOrigin.VisibleChanged += new System.EventHandler(this.rdoOrigin_VisibleChanged);
            // 
            // rdoEntity
            // 
            this.rdoEntity.AutoSize = true;
            this.rdoEntity.Checked = true;
            this.rdoEntity.Location = new System.Drawing.Point(18, 89);
            this.rdoEntity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoEntity.Name = "rdoEntity";
            this.rdoEntity.Size = new System.Drawing.Size(102, 20);
            this.rdoEntity.TabIndex = 1;
            this.rdoEntity.TabStop = true;
            this.rdoEntity.Text = "Entity Center";
            this.rdoEntity.UseVisualStyleBackColor = true;
            this.rdoEntity.VisibleChanged += new System.EventHandler(this.rdoOrigin_VisibleChanged);
            // 
            // btnPreview
            // 
            this.btnPreview.Location = new System.Drawing.Point(8, 407);
            this.btnPreview.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(90, 40);
            this.btnPreview.TabIndex = 58;
            this.btnPreview.Text = "Redraw";
            this.btnPreview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // DivideForm
            // 
            this.AcceptButton = this.button1;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 485);
            this.Controls.Add(this.btnPreview);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.numCols);
            this.Controls.Add(this.numRows);
            this.Controls.Add(this.txtWidth);
            this.Controls.Add(this.txtOverlap);
            this.Controls.Add(this.txtHeight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DivideForm";
            this.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Divide Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DivideForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCols)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtHeight;
        private System.Windows.Forms.TextBox txtWidth;
        private System.Windows.Forms.NumericUpDown numRows;
        private System.Windows.Forms.NumericUpDown numCols;
        private System.Windows.Forms.TextBox txtOverlap;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel lblWidth;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel lblHeight;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoEntity;
        private System.Windows.Forms.RadioButton rdoOrigin;
        private System.Windows.Forms.Button btnPreview;
    }
}