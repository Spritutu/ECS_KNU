
namespace Scanlab.Sirius
{
    partial class Correction2DForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSource = new System.Windows.Forms.TextBox();
            this.txtTarget = new System.Windows.Forms.TextBox();
            this.numRows = new System.Windows.Forms.NumericUpDown();
            this.numCols = new System.Windows.Forms.NumericUpDown();
            this.txtInterval = new System.Windows.Forms.TextBox();
            this.btnTargetFile = new System.Windows.Forms.Button();
            this.btnSourceFile = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.importFromToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.numRows)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCols)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(397, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 16);
            this.label1.TabIndex = 27;
            this.label1.Text = "Interval :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 16);
            this.label2.TabIndex = 9;
            this.label2.Text = "Rows :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(198, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "Columns :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(57, 16);
            this.label4.TabIndex = 28;
            this.label4.Text = "Source :";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 96);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(51, 16);
            this.label5.TabIndex = 19;
            this.label5.Text = "Target :";
            // 
            // txtSource
            // 
            this.txtSource.Location = new System.Drawing.Point(77, 55);
            this.txtSource.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSource.Name = "txtSource";
            this.txtSource.ReadOnly = true;
            this.txtSource.Size = new System.Drawing.Size(350, 22);
            this.txtSource.TabIndex = 16;
            // 
            // txtTarget
            // 
            this.txtTarget.Location = new System.Drawing.Point(77, 89);
            this.txtTarget.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTarget.Name = "txtTarget";
            this.txtTarget.ReadOnly = true;
            this.txtTarget.Size = new System.Drawing.Size(350, 22);
            this.txtTarget.TabIndex = 17;
            // 
            // numRows
            // 
            this.numRows.Enabled = false;
            this.numRows.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numRows.Location = new System.Drawing.Point(78, 17);
            this.numRows.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numRows.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.numRows.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numRows.Name = "numRows";
            this.numRows.ReadOnly = true;
            this.numRows.Size = new System.Drawing.Size(100, 22);
            this.numRows.TabIndex = 8;
            this.numRows.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // numCols
            // 
            this.numCols.Enabled = false;
            this.numCols.Increment = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.numCols.Location = new System.Drawing.Point(266, 16);
            this.numCols.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.numCols.Maximum = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.numCols.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numCols.Name = "numCols";
            this.numCols.ReadOnly = true;
            this.numCols.Size = new System.Drawing.Size(100, 22);
            this.numCols.TabIndex = 7;
            this.numCols.Value = new decimal(new int[] {
            17,
            0,
            0,
            0});
            // 
            // txtInterval
            // 
            this.txtInterval.Location = new System.Drawing.Point(456, 17);
            this.txtInterval.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtInterval.Name = "txtInterval";
            this.txtInterval.ReadOnly = true;
            this.txtInterval.Size = new System.Drawing.Size(100, 22);
            this.txtInterval.TabIndex = 28;
            // 
            // btnTargetFile
            // 
            this.btnTargetFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTargetFile.Location = new System.Drawing.Point(433, 91);
            this.btnTargetFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTargetFile.Name = "btnTargetFile";
            this.btnTargetFile.Size = new System.Drawing.Size(40, 28);
            this.btnTargetFile.TabIndex = 20;
            this.btnTargetFile.Text = "...";
            this.btnTargetFile.UseVisualStyleBackColor = true;
            this.btnTargetFile.Click += new System.EventHandler(this.btnTargetFile_Click);
            // 
            // btnSourceFile
            // 
            this.btnSourceFile.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSourceFile.Location = new System.Drawing.Point(433, 55);
            this.btnSourceFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSourceFile.Name = "btnSourceFile";
            this.btnSourceFile.Size = new System.Drawing.Size(40, 28);
            this.btnSourceFile.TabIndex = 24;
            this.btnSourceFile.Text = "...";
            this.btnSourceFile.UseVisualStyleBackColor = true;
            this.btnSourceFile.Click += new System.EventHandler(this.btnSourceFile_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(479, 55);
            this.btnConvert.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(86, 65);
            this.btnConvert.TabIndex = 13;
            this.btnConvert.Text = "Convert";
            this.btnConvert.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(568, 55);
            this.btnApply.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(86, 65);
            this.btnApply.TabIndex = 25;
            this.btnApply.Text = "Apply";
            this.btnApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importFromToolStripMenuItem,
            this.exportToToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(143, 48);
            // 
            // importFromToolStripMenuItem
            // 
            this.importFromToolStripMenuItem.Name = "importFromToolStripMenuItem";
            this.importFromToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.importFromToolStripMenuItem.Text = "&Import From";
            this.importFromToolStripMenuItem.Click += new System.EventHandler(this.importFromToolStripMenuItem_Click);
            // 
            // exportToToolStripMenuItem
            // 
            this.exportToToolStripMenuItem.Name = "exportToToolStripMenuItem";
            this.exportToToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.exportToToolStripMenuItem.Text = "&Export To";
            this.exportToToolStripMenuItem.Click += new System.EventHandler(this.exportToToolStripMenuItem_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 132);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(610, 549);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage1.Size = new System.Drawing.Size(602, 520);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Measured Data (Z=0) ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.txtResult);
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabPage2.Size = new System.Drawing.Size(602, 520);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridView1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.GridColor = System.Drawing.SystemColors.Control;
            this.dataGridView1.Location = new System.Drawing.Point(3, 4);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersWidth = 60;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(596, 512);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DataGridView1_CellBeginEdit);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridView1_CellEndEdit);
            this.dataGridView1.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.DataGridView1_CellPainting);
            // 
            // txtResult
            // 
            this.txtResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtResult.Location = new System.Drawing.Point(3, 4);
            this.txtResult.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtResult.Size = new System.Drawing.Size(596, 512);
            this.txtResult.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // Correction2DForm
            // 
            this.AcceptButton = this.btnTargetFile;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 557);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnSourceFile);
            this.Controls.Add(this.btnTargetFile);
            this.Controls.Add(this.numCols);
            this.Controls.Add(this.numRows);
            this.Controls.Add(this.txtTarget);
            this.Controls.Add(this.txtInterval);
            this.Controls.Add(this.txtSource);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Correction2DForm";
            this.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Scanner Field Correction 2D";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Correction2DForm_FormClosed);
            this.Shown += new System.EventHandler(this.Correction2DForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.numRows)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCols)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSource;
        private System.Windows.Forms.TextBox txtTarget;
        private System.Windows.Forms.NumericUpDown numRows;
        private System.Windows.Forms.NumericUpDown numCols;
        private System.Windows.Forms.TextBox txtInterval;
        private System.Windows.Forms.Button btnTargetFile;
        private System.Windows.Forms.Button btnSourceFile;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem importFromToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToToolStripMenuItem;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}