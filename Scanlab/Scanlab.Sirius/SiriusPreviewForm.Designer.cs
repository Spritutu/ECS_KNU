
namespace Scanlab.Sirius
{
    partial class SiriusPreviewForm
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.fileSystemWatcher1 = new System.IO.FileSystemWatcher();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDistance = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lblRoot = new System.Windows.Forms.Label();
            this.siriusViewerForm1 = new Scanlab.Sirius.SiriusViewerForm();
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(909, 921);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnOk.Size = new System.Drawing.Size(80, 38);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "&Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(995, 921);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnCancel.Size = new System.Drawing.Size(80, 38);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(768, 36);
            this.listView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new System.Drawing.Size(310, 799);
            this.listView1.TabIndex = 4;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            // 
            // fileSystemWatcher1
            // 
            this.fileSystemWatcher1.EnableRaisingEvents = true;
            this.fileSystemWatcher1.SynchronizingObject = this;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(797, 865);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Closed Path Distance (mm) :";
            // 
            // txtDistance
            // 
            this.txtDistance.Location = new System.Drawing.Point(977, 858);
            this.txtDistance.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(80, 21);
            this.txtDistance.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(797, 925);
            this.button1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 38);
            this.button1.TabIndex = 7;
            this.button1.Text = "&Apply";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblRoot
            // 
            this.lblRoot.AutoSize = true;
            this.lblRoot.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoot.Location = new System.Drawing.Point(771, 13);
            this.lblRoot.Name = "lblRoot";
            this.lblRoot.Size = new System.Drawing.Size(29, 16);
            this.lblRoot.TabIndex = 8;
            this.lblRoot.Text = "C:\\\\";
            // 
            // siriusViewerForm1
            // 
            this.siriusViewerForm1.AliasName = "NoName";
            this.siriusViewerForm1.Document = null;
            this.siriusViewerForm1.FileName = "NoName";
            this.siriusViewerForm1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.siriusViewerForm1.Index = ((uint)(0u));
            this.siriusViewerForm1.Location = new System.Drawing.Point(1, 1);
            this.siriusViewerForm1.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.siriusViewerForm1.Name = "siriusViewerForm1";
            this.siriusViewerForm1.Progress = 0;
            this.siriusViewerForm1.Size = new System.Drawing.Size(761, 976);
            this.siriusViewerForm1.TabIndex = 3;
            // 
            // SiriusPreviewForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1093, 1002);
            this.Controls.Add(this.siriusViewerForm1);
            this.Controls.Add(this.lblRoot);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtDistance);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "SiriusPreviewForm";
            this.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Text = "Priview";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PropertyForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.fileSystemWatcher1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ListView listView1;
        private System.IO.FileSystemWatcher fileSystemWatcher1;
        private SiriusViewerForm siriusViewerForm1;
        private System.Windows.Forms.Label lblRoot;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox txtDistance;
        private System.Windows.Forms.Label label1;
    }
}