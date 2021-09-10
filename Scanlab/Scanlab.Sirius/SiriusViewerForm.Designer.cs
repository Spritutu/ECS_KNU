
namespace Scanlab.Sirius
{
    partial class SiriusViewerForm
    {
        /// <summary> 
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 구성 요소 디자이너에서 생성한 코드

        /// <summary> 
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiriusViewerForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnZoomOut = new System.Windows.Forms.ToolStripButton();
            this.btnZoomIn = new System.Windows.Forms.ToolStripButton();
            this.btnZoomFit = new System.Windows.Forms.ToolStripButton();
            this.btnPan = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblName = new System.Windows.Forms.ToolStripStatusLabel();
            this.pgbProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.lblYPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblXPos = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblRenderTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblEntityCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblFileName = new System.Windows.Forms.ToolStripStatusLabel();
            this.GLcontrol = new SharpGL.OpenGLControl();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GLcontrol)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnZoomOut,
            this.btnZoomIn,
            this.btnZoomFit,
            this.btnPan,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(792, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.AutoSize = false;
            this.btnZoomOut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
            this.btnZoomOut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(23, 22);
            this.btnZoomOut.ToolTipText = "Zoom Out";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
            this.btnZoomIn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(23, 22);
            this.btnZoomIn.Text = "toolStripButton1";
            this.btnZoomIn.ToolTipText = "Zoom In";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomFit
            // 
            this.btnZoomFit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnZoomFit.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomFit.Image")));
            this.btnZoomFit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnZoomFit.Name = "btnZoomFit";
            this.btnZoomFit.Size = new System.Drawing.Size(23, 22);
            this.btnZoomFit.Text = "toolStripButton1";
            this.btnZoomFit.ToolTipText = "Zoom Fit";
            this.btnZoomFit.Click += new System.EventHandler(this.btnZoomFit_Click);
            // 
            // btnPan
            // 
            this.btnPan.CheckOnClick = true;
            this.btnPan.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnPan.Image = ((System.Drawing.Image)(resources.GetObject("btnPan.Image")));
            this.btnPan.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPan.Name = "btnPan";
            this.btnPan.Size = new System.Drawing.Size(23, 22);
            this.btnPan.Text = "toolStripButton1";
            this.btnPan.ToolTipText = "Pan";
            this.btnPan.Click += new System.EventHandler(this.btnPan_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblName,
            this.pgbProgress,
            this.lblYPos,
            this.lblXPos,
            this.lblRenderTime,
            this.lblEntityCount,
            this.lblFileName});
            this.statusStrip1.Location = new System.Drawing.Point(0, 442);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblName
            // 
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(55, 17);
            this.lblName.Text = "NoName";
            // 
            // pgbProgress
            // 
            this.pgbProgress.Name = "pgbProgress";
            this.pgbProgress.Size = new System.Drawing.Size(100, 16);
            this.pgbProgress.Step = 1;
            // 
            // lblYPos
            // 
            this.lblYPos.AutoSize = false;
            this.lblYPos.Name = "lblYPos";
            this.lblYPos.Size = new System.Drawing.Size(52, 17);
            this.lblYPos.Text = "Y: 0.000";
            // 
            // lblXPos
            // 
            this.lblXPos.AutoSize = false;
            this.lblXPos.Name = "lblXPos";
            this.lblXPos.Size = new System.Drawing.Size(121, 17);
            this.lblXPos.Text = "X: 0.000";
            // 
            // lblRenderTime
            // 
            this.lblRenderTime.AutoSize = false;
            this.lblRenderTime.Name = "lblRenderTime";
            this.lblRenderTime.Size = new System.Drawing.Size(121, 17);
            this.lblRenderTime.Text = "Render: 0 ms";
            // 
            // lblEntityCount
            // 
            this.lblEntityCount.Name = "lblEntityCount";
            this.lblEntityCount.Size = new System.Drawing.Size(66, 17);
            this.lblEntityCount.Text = "Selected: 0";
            // 
            // lblFileName
            // 
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(55, 17);
            this.lblFileName.Text = "NoName";
            // 
            // GLcontrol
            // 
            this.GLcontrol.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GLcontrol.DrawFPS = false;
            this.GLcontrol.FrameRate = 60;
            this.GLcontrol.Location = new System.Drawing.Point(0, 25);
            this.GLcontrol.Name = "GLcontrol";
            this.GLcontrol.OpenGLVersion = SharpGL.Version.OpenGLVersion.OpenGL2_1;
            this.GLcontrol.RenderContextType = SharpGL.RenderContextType.FBO;
            this.GLcontrol.RenderTrigger = SharpGL.RenderTrigger.Manual;
            this.GLcontrol.Size = new System.Drawing.Size(792, 417);
            this.GLcontrol.TabIndex = 2;
            // 
            // SiriusViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.GLcontrol);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "SiriusViewerForm";
            this.Size = new System.Drawing.Size(792, 464);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GLcontrol)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnZoomOut;
        private System.Windows.Forms.ToolStripButton btnZoomIn;
        private System.Windows.Forms.ToolStripButton btnZoomFit;
        private System.Windows.Forms.ToolStripButton btnPan;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblName;
        private System.Windows.Forms.ToolStripProgressBar pgbProgress;
        private System.Windows.Forms.ToolStripStatusLabel lblYPos;
        private System.Windows.Forms.ToolStripStatusLabel lblXPos;
        private System.Windows.Forms.ToolStripStatusLabel lblRenderTime;
        private System.Windows.Forms.ToolStripStatusLabel lblEntityCount;
        private System.Windows.Forms.ToolStripStatusLabel lblFileName;
        private SharpGL.OpenGLControl GLcontrol;
    }
}
