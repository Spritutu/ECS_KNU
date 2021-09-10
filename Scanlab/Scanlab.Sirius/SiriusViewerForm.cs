using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    public partial class SiriusViewerForm : UserControl
    {
        private IView _View;
        private IDocument _Doc;

        /// <summary>뷰 객체</summary>
        public virtual IView View => this._View;


        public virtual uint Index { get; set; }

        /// <summary>상태바에 출력되는 이름</summary>
        public virtual string AliasName
        {
            get => this.lblName.Text;
            set => this.lblName.Text = value;
        }

        /// <summary>상태바에 출력되는 진행상태 (0~100)</summary>
        public virtual int Progress
        {
            get => this.pgbProgress.Value;
            set => this.pgbProgress.Value = value;
        }

        /// <summary>상태바에 출력되는 작업 파일이름</summary>
        public virtual string FileName
        {
            get => this.lblFileName.Text;
            set => this.lblFileName.Text = value;
        }

        /// <summary>문서 컨테이너 객체</summary>
        public virtual IDocument Document
        {
            get => this._Doc;
            set
            {
                if (value == null || value.Equals((object)this._Doc))
                    return;
                this._Doc = value;
                if (this._Doc.Layers.Count == 0)
                    this._Doc.Action.ActNew();
                List<IView> viewList = new List<IView>();
                if (this.Document != null)
                {
                    viewList.AddRange((IEnumerable<IView>)this.Document.Views);
                    if (this._View != null)
                    {
                        this._Doc.Views.Remove(this._View);
                        viewList.Remove(this._View);
                    }
                }
                this.FileName = this._Doc.FileName;
                this._View = (IView)new ViewDefault(this._Doc, this.GLcontrol)
                {
                    EditorMode = false
                };
                this._Doc.Views.Add(this._View);
                this._Doc.Views.AddRange((IEnumerable<IView>)viewList);
                this._View.Render();
                this._View.OnZoomFit();
            }
        }
        public SiriusViewerForm()
        {
            InitializeComponent();
            this.GLcontrol.OpenGLInitialized += new EventHandler(this.OnInitialized);
            ((Control)this.GLcontrol).Resize += new EventHandler(this.OnResized);
            ((Control)this.GLcontrol).MouseDown += new MouseEventHandler(this.OnMouseDown);
            ((Control)this.GLcontrol).MouseUp += new MouseEventHandler(this.OnMouseUp);
            ((Control)this.GLcontrol).MouseMove += new MouseEventHandler(this.OnMouseMove);
            ((Control)this.GLcontrol).MouseWheel += new MouseEventHandler(this.OnMouseWheel);
            OpenGLControl glcontrol = this.GLcontrol;
            SiriusViewerForm siriusViewerForm = this;
            // ISSUE: virtual method pointer
            RenderEventHandler renderEventHandler = new RenderEventHandler(OnDraw);
            glcontrol.OpenGLDraw += renderEventHandler;
        }

        protected virtual void OnInitialized(object sender, EventArgs e) => this._View?.OnInitialized(sender, e);

        protected virtual void OnResized(object sender, EventArgs e) => this._View?.OnResized(sender, e);

        protected virtual void OnMouseDown(object sender, MouseEventArgs e) => this._View?.OnMouseDown(sender, e);

        protected virtual void OnMouseUp(object sender, MouseEventArgs e) => this._View?.OnMouseUp(sender, e);

        protected virtual void OnMouseWheel(object sender, MouseEventArgs e) => this._View?.OnMouseWheel(sender, e);


        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (this._View == null)
                return;
            this._View.OnMouseMove(sender, e);
            float x;
            float y;
            this._View.Dp2Lp(e.Location, out x, out y);
            this.lblXPos.Text = string.Format("X: {0:F3}", (object)x);
            this.lblYPos.Text = string.Format("Y: {0:F3}", (object)y);
        }

        protected virtual void OnDraw(object sender, RenderEventArgs args)
        {
            if (this._View == null)
                return;
            Stopwatch stopwatch = Stopwatch.StartNew();
            this._View.OnDraw();
            this.lblRenderTime.Text = string.Format("Render: {0} ms", (object)stopwatch.ElapsedMilliseconds);
        }

        protected virtual void btnZoomOut_Click(object sender, EventArgs e) => this._View?.OnZoomOut(new System.Drawing.Point(((Control)this.GLcontrol).Width / 2, ((Control)this.GLcontrol).Height / 2));

        protected virtual void btnZoomIn_Click(object sender, EventArgs e) => this._View?.OnZoomIn(new System.Drawing.Point(((Control)this.GLcontrol).Width / 2, ((Control)this.GLcontrol).Height / 2));

        protected virtual void btnZoomFit_Click(object sender, EventArgs e) => this._View?.OnZoomFit();

        protected virtual void btnPan_Click(object sender, EventArgs e) => this._View?.OnPan(this.btnPan.Checked);


    }
}
