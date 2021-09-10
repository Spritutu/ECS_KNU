
using SharpGL;
using SharpGL.Enumerations;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    public class ViewDefault : IView
    {
        private IDocument owner;
        protected bool isPerspective;
        /// <summary>마우스 위치 정보</summary>
        private System.Drawing.Point mouseCurrentLocation;
        private System.Drawing.Point mouseLeftDownLocation;
        private bool drawRubberBand;
        private bool drawDragging;
        private bool panningEnabled;
        private bool drawPanning;
        private float dragDx;
        private float dragDy;
        internal OpenGLControl glControl;
        private int spotCount;
        private uint glListAxes;
        private uint glListMarkTo;
        private uint glListLaserStartFrom;
        private uint glListLaserSpotCross;
        private uint glListLaserSpotCircle;
        internal bool IsRegeningListId;

        public IDocument Owner
        {
            get => this.owner;
            set => this.owner = value;
        }

        public OpenGL Renderer => this.glControl.OpenGL;

        public int Width => this.glControl.Width;

        public int Height => this.glControl.Height;

        public float CameraX { get; set; }

        public float CameraY { get; set; }

        public float ScaleWidth { get; set; }

        public float ScaleHeight { get; set; }

        public float Scale { get; set; }

        public BoundRect SelectedBoundRect { get; set; }

        public long RenderTime { get; private set; }

        public Vector2 LaserSpot { get; set; }

        public List<BoundRect> DivideRects { get; set; }

        public bool EditorMode { get; set; }

        public bool IsPerspective
        {
            get => this.isPerspective;
            set
            {
                this.isPerspective = value;
                this.Render();
            }
        }

        public object Tag { get; set; }

        public ViewDefault(IDocument owner, OpenGLControl control)
        {
            this.owner = owner;
            this.glControl = control;
            this.Scale = 0.5f;
            this.ScaleWidth = (float)control.Width;
            this.ScaleHeight = (float)control.Height;
            this.SelectedBoundRect = BoundRect.Empty;
            this.OnResized((object)this, EventArgs.Empty);
            this.LaserSpot = Vector2.Zero;
            this.EditorMode = true;
            this.isPerspective = false;
        }

        /// <summary>
        /// 물리 좌표 -&gt; 사용자 좌표계를 변환
        /// pixel -&gt; mm
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public void Dp2Lp(System.Drawing.Point p, out float x, out float y)
        {
            float num1 = (float)((double)p.X / (double)this.glControl.Width - 0.5);
            float num2 = (float)((double)(this.glControl.Height - p.Y) / (double)this.glControl.Height - 0.5);
            x = num1 * this.ScaleWidth + this.CameraX;
            y = num2 * this.ScaleHeight + this.CameraY;
        }

        public void Lp2Dp(float x, float y, out System.Drawing.Point p)
        {
            p = System.Drawing.Point.Empty;
            p.X = (int)(((double)x - (double)this.CameraX) / (double)this.ScaleWidth * (double)this.glControl.Width);
        }

        /// <summary>
        /// 해당 픽셀크기 값을 사용자 좌표(mm)로 변환
        /// pixel -&gt; mm
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public float Dp2Lp(int pixel)
        {
            float x1;
            this.Dp2Lp(new System.Drawing.Point(pixel, 0), out x1, out float _);
            float x2;
            this.Dp2Lp(new System.Drawing.Point(0, 0), out x2, out float _);
            return Math.Abs(x1 - x2);
        }

        /// <summary>사용자 단위 좌표 -&gt; pixel</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public int Lp2Dp(float value)
        {
            System.Drawing.Point p;
            this.Lp2Dp(value, value, out p);
            return p.X;
        }

        public void OnInitialized(object sender, EventArgs e)
        {
            OpenGL openGl = this.glControl.OpenGL;
            float[] parameters1 = new float[4]
            {
        0.5f,
        0.5f,
        0.5f,
        1f
            };
            float[] parameters2 = new float[4]
            {
        0.0f,
        0.0f,
        100f,
        1f
            };
            float[] parameters3 = new float[4]
            {
        0.2f,
        0.2f,
        0.2f,
        1f
            };
            float[] parameters4 = new float[4]
            {
        0.9f,
        0.9f,
        0.3f,
        1f
            };
            float[] parameters5 = new float[4]
            {
        0.8f,
        0.8f,
        0.8f,
        1f
            };
            float[] parameters6 = new float[4]
            {
        0.2f,
        0.2f,
        0.2f,
        1f
            };
            openGl.LightModel(2899U, parameters6);
            openGl.LightModel(2899U, parameters1);
            openGl.Light(16384U, 4611U, parameters2);
            openGl.Light(16384U, 4608U, parameters3);
            openGl.Light(16384U, 4609U, parameters4);
            openGl.Light(16384U, 4610U, parameters5);
            openGl.Enable(2896U);
            openGl.Enable(16384U);
            openGl.ShadeModel(7425U);
            openGl.Enable(3553U);
            openGl.ClearColor(0.0f, 0.0f, 0.0f, 0.0f);
            openGl.Enable(3042U);
        }

        public void OnResized(object sender, EventArgs e)
        {
            OpenGL openGl = this.glControl.OpenGL;
            openGl.MakeCurrent();
            openGl.Enable(2929U);
            openGl.DepthFunc(515U);
            openGl.Hint(3152U, 4354U);
            openGl.Enable(2896U);
            openGl.Enable(16385U);
            float[] parameters1 = new float[4]
            {
        0.2f,
        0.2f,
        0.2f,
        1f
            };
            float[] parameters2 = new float[4]
            {
        0.6f,
        0.6f,
        0.6f,
        1f
            };
            float[] parameters3 = new float[4] { 1f, 1f, 1f, 1f };
            float[] parameters4 = new float[4]
            {
        50f,
        100f,
        10000f,
        1f
            };
            openGl.Light(LightName.Light1, LightParameter.Ambient, parameters1);
            openGl.Light(LightName.Light1, LightParameter.Diffuse, parameters2);
            openGl.Light(LightName.Light0, LightParameter.Specular, parameters3);
            openGl.Light(LightName.Light1, LightParameter.Position, parameters4);
            openGl.Enable(2903U);
            openGl.ColorMaterial(1028U, 5634U);
            float[] parameters5 = new float[4] { 1f, 1f, 1f, 1f };
            openGl.Material(1028U, 4610U, parameters5);
            openGl.Material(1028U, 5633U, 64);
            openGl.Enable(3042U);
            openGl.BlendFunc(770U, 771U);
            openGl.Viewport(0, 0, this.glControl.Width, this.glControl.Height);
            openGl.MatrixMode(5889U);
            openGl.LoadIdentity();
            openGl.MatrixMode(5888U);
            openGl.LoadIdentity();
            this.ScaleWidth = (float)this.glControl.Width;
            this.ScaleHeight = (float)this.glControl.Height;
            this.Render();
            this.OnZoomFit((BoundRect)null);
        }

        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    this.mouseLeftDownLocation = e.Location;
                    this.mouseCurrentLocation = e.Location;
                    this.drawRubberBand = false;
                    if (this.panningEnabled)
                    {
                        this.drawPanning = true;
                        Cursor.Current = Cursors.Hand;
                        break;
                    }
                    List<IEntity> selectedEntity = this.owner.Action.SelectedEntity;
                    if (selectedEntity.Count > 0)
                    {
                        BoundRect boundRect = new BoundRect();
                        foreach (IEntity entity in selectedEntity)
                            boundRect.Union(entity.BoundRect);
                        if (!boundRect.IsEmpty)
                        {
                            float x;
                            float y;
                            this.Dp2Lp(this.mouseLeftDownLocation, out x, out y);
                            if (boundRect.HitTest(x, y, 0.1f) && Control.ModifierKeys != Keys.Control && (Control.ModifierKeys != Keys.Shift && Control.ModifierKeys != Keys.Alt))
                            {
                                this.dragDx = this.dragDy = 0.0f;
                                this.drawDragging = true;
                            }
                            else
                                this.drawRubberBand = true;
                        }
                    }
                    else
                        this.drawRubberBand = true;
                    this.glControl.DoRender();
                    break;
                case MouseButtons.Middle:
                    this.mouseCurrentLocation = e.Location;
                    break;
            }
            this.mouseCurrentLocation = e.Location;
        }

        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (this.drawPanning)
                    {
                        this.mouseCurrentLocation = e.Location;
                        this.drawPanning = false;
                        Cursor.Current = Cursors.Default;
                    }
                    else
                    {
                        this.mouseCurrentLocation = e.Location;
                        float x1;
                        float y1;
                        this.Dp2Lp(this.mouseLeftDownLocation, out x1, out y1);
                        float x2;
                        float y2;
                        this.Dp2Lp(e.Location, out x2, out y2);
                        this.drawRubberBand = false;
                        this.drawPanning = false;
                        if (this.drawDragging)
                        {
                            this.drawDragging = false;
                            if (this.EditorMode)
                                this.owner.Action.ActEntityTransit(this.owner.Action.SelectedEntity, x2 - x1, y2 - y1);
                        }
                        else
                        {
                            float threshold = this.Dp2Lp(2);
                            if (this.mouseLeftDownLocation.Equals((object)e.Location))
                                this.owner.Action.HitTest(Control.ModifierKeys, x1, y1, threshold);
                            else
                                this.owner.Action.HitTest(Control.ModifierKeys, x1, y1, x2, y2, threshold);
                        }
                    }
                    this.glControl.DoRender();
                    break;
                case MouseButtons.Middle:
                    this.mouseCurrentLocation = e.Location;
                    Cursor.Current = Cursors.Default;
                    break;
            }
        }

        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (this.panningEnabled && this.drawPanning)
                    {
                        float num1 = (float)(e.Location.X - this.mouseCurrentLocation.X);
                        float num2 = (float)(e.Location.Y - this.mouseCurrentLocation.Y);
                        this.CameraX -= num1 * (this.ScaleWidth / (float)this.glControl.Width);
                        this.CameraY += num2 * (this.ScaleHeight / (float)this.glControl.Height);
                        this.mouseCurrentLocation = e.Location;
                    }
                    else
                    {
                        this.mouseCurrentLocation = e.Location;
                        float x1;
                        float y1;
                        this.Dp2Lp(this.mouseLeftDownLocation, out x1, out y1);
                        float x2;
                        float y2;
                        this.Dp2Lp(e.Location, out x2, out y2);
                        this.dragDx = x2 - x1;
                        this.dragDy = y2 - y1;
                    }
                    this.glControl.DoRender();
                    break;
                case MouseButtons.Middle:
                    Cursor.Current = Cursors.Hand;
                    System.Drawing.Point location = e.Location;
                    float num3 = (float)(location.X - this.mouseCurrentLocation.X);
                    location = e.Location;
                    float num4 = (float)(location.Y - this.mouseCurrentLocation.Y);
                    this.CameraX -= num3 * (this.ScaleWidth / (float)this.glControl.Width);
                    this.CameraY += num4 * (this.ScaleHeight / (float)this.glControl.Height);
                    this.mouseCurrentLocation = e.Location;
                    this.glControl.DoRender();
                    break;
            }
        }

        public void OnMouseWheel(object sender, MouseEventArgs e)
        {
            float num1 = (float)((double)e.Location.X / (double)this.glControl.Width - 0.5);
            double num2 = (double)(this.glControl.Height - e.Location.Y) / (double)this.glControl.Height - 0.5;
            float num3 = num1 * this.ScaleWidth;
            float num4 = (float)num2 * this.ScaleHeight;
            float num5 = 1.2f;
            if (e.Delta > 0)
            {
                this.ScaleWidth /= num5;
                this.ScaleHeight /= num5;
                this.Scale /= num5;
            }
            else
            {
                this.ScaleWidth *= num5;
                this.ScaleHeight *= num5;
                this.Scale *= num5;
            }
            float num6 = num1 * this.ScaleWidth;
            float num7 = (float)num2 * this.ScaleHeight;
            this.CameraX += num3 - num6;
            this.CameraY += num4 - num7;
            this.glControl.DoRender();
        }

        public void OnZoomIn(System.Drawing.Point p)
        {
            float num1 = (float)((double)p.X / (double)this.glControl.Width - 0.5);
            double num2 = (double)(this.glControl.Height - p.Y) / (double)this.glControl.Height - 0.5;
            float num3 = num1 * this.ScaleWidth;
            float num4 = (float)num2 * this.ScaleHeight;
            float num5 = 1.2f;
            this.ScaleWidth /= num5;
            this.ScaleHeight /= num5;
            this.Scale /= num5;
            float num6 = num1 * this.ScaleWidth;
            float num7 = (float)num2 * this.ScaleHeight;
            this.CameraX += num3 - num6;
            this.CameraY += num4 - num7;
            this.glControl.DoRender();
        }

        public void OnZoomOut(System.Drawing.Point p)
        {
            float num1 = (float)((double)p.X / (double)this.glControl.Width - 0.5);
            double num2 = (double)(this.glControl.Height - p.Y) / (double)this.glControl.Height - 0.5;
            float num3 = num1 * this.ScaleWidth;
            float num4 = (float)num2 * this.ScaleHeight;
            float num5 = 1.2f;
            this.ScaleWidth *= num5;
            this.ScaleHeight *= num5;
            this.Scale *= num5;
            float num6 = num1 * this.ScaleWidth;
            float num7 = (float)num2 * this.ScaleHeight;
            this.CameraX += num3 - num6;
            this.CameraY += num4 - num7;
            this.glControl.DoRender();
        }

        public void OnZoomFit(BoundRect br = null)
        {
            if (br == null)
            {
                br = new BoundRect();
                foreach (Layer layer in (ObservableList<Layer>)this.owner.Layers)
                    br.Union(layer.BoundRect);
                if (br.IsEmpty)
                    return;
            }
            this.ScaleWidth = (float)this.glControl.Width;
            this.ScaleHeight = (float)this.glControl.Height;
            this.Scale = 1f;
            this.CameraX = 0.0f;
            this.CameraY = 0.0f;
            this.CameraX = br.Center.X;
            this.CameraY = br.Center.Y;
            float num = Math.Max(br.Width / this.ScaleWidth, br.Height / this.ScaleHeight);
            this.ScaleWidth *= num * 1.05f;
            this.ScaleHeight *= num * 1.05f;
            this.glControl.DoRender();
        }

        public void OnPan(bool onOff) => this.panningEnabled = onOff;

        public void OnCameraMove(float x, float y)
        {
            this.CameraX = x;
            this.CameraY = y;
            this.glControl?.DoRender();
        }

        public void Render()
        {
            if (this.glControl.IsDisposed)
                return;
            this.glControl.DoRender();
        }

        public long OnDraw()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            OpenGL openGl = this.glControl.OpenGL;
            openGl.MakeCurrent();
            openGl.ClearColor(Config.DocumentClearColor[0], Config.DocumentClearColor[1], Config.DocumentClearColor[2], Config.DocumentClearColor[3]);
            openGl.Clear(16640U);
            if (!this.IsPerspective)
            {
                openGl.Disable(2929U);
                openGl.Disable(2896U);
                openGl.Disable(16385U);
                openGl.MatrixMode(5889U);
                openGl.LoadIdentity();
                openGl.Ortho((double)this.CameraX - (double)this.ScaleWidth / 2.0, (double)this.CameraX + (double)this.ScaleWidth / 2.0, (double)this.CameraY - (double)this.ScaleHeight / 2.0, (double)this.CameraY + (double)this.ScaleHeight / 2.0, -1000.0, 1000.0);
            }
            else
            {
                openGl.Enable(2929U);
                openGl.DepthFunc(515U);
                openGl.Hint(3152U, 4354U);
                openGl.Enable(2896U);
                openGl.Enable(16385U);
                float[] parameters1 = new float[4]
                {
          0.2f,
          0.2f,
          0.2f,
          1f
                };
                float[] parameters2 = new float[4]
                {
          0.6f,
          0.6f,
          0.6f,
          1f
                };
                float[] parameters3 = new float[4] { 1f, 1f, 1f, 1f };
                float[] parameters4 = new float[4]
                {
          50f,
          100f,
          10000f,
          1f
                };
                openGl.Light(16385U, 4608U, parameters1);
                openGl.Light(16385U, 4609U, parameters2);
                openGl.Light(16384U, 4610U, parameters3);
                openGl.Light(16385U, 4611U, parameters4);
                openGl.Enable(2903U);
                openGl.ColorMaterial(1028U, 5634U);
                float[] parameters5 = new float[4] { 1f, 1f, 1f, 1f };
                openGl.Material(1028U, 4610U, parameters5);
                openGl.Material(1028U, 5633U, 64);
                openGl.Viewport(0, 0, this.Width, this.Height);
                openGl.MatrixMode(5889U);
                openGl.LoadIdentity();
                float num = (float)this.Width / (float)this.Height;
                openGl.Perspective(60.0, (double)num, 0.100000001490116, 10000.0);
                openGl.LookAt((double)this.CameraX, (double)this.CameraY, 1000.0 / (double)this.Scale, (double)this.CameraX * 0.1, (double)this.CameraY * 0.1, 0.0, 0.0, 1.0, 0.0);
            }
            openGl.MatrixMode(5888U);
            openGl.LoadIdentity();
            openGl.PushMatrix();
            openGl.LineWidth(0.0f);
            this.DrawAxes(openGl);
            if (this.owner != null)
                this.DrawLayers(openGl, this.owner.Layers);
            if (this.drawRubberBand)
                this.DrawRubberBand(openGl);
            this.DrawSelectedBoundRect(openGl);
            if (this.drawDragging)
                this.DrawDragging(openGl);
            this.DrawDoucmentInfo(openGl);
            if (!object.Equals((object)this.LaserSpot, (object)Vector2.Zero))
                this.DrawLaserSpot(openGl, this.LaserSpot);
            if (this.DivideRects != null)
                this.DrawDivideRects(openGl);
            openGl.PopMatrix();
            openGl.Flush();
            this.RenderTime = stopwatch.ElapsedMilliseconds;
            return this.RenderTime;
        }

        /// <summary>마우스를 이용해 엔티티를 Dragging 하는 과정을 렌더링</summary>
        /// <param name="gl"></param>
        private void DrawDragging(OpenGL gl)
        {
            gl.Enable(2852U);
            gl.LineStipple(1, (ushort)257);
            gl.PushMatrix();
            gl.Translate(this.dragDx, this.dragDy, 0.0f);
            foreach (IEntity entity in this.owner.Action.SelectedEntity)
            {
                if (entity is IDrawable drawable1)
                    drawable1.Draw((IView)this);
            }
            gl.PopMatrix();
            gl.Disable(2852U);
        }

        /// <summary>문서 정보 렌더링</summary>
        /// <param name="gl"></param>
        private void DrawDoucmentInfo(OpenGL gl)
        {
            if (!(this.owner is DocumentDefault owner))
                return;
            owner.Draw((IView)this);
        }

        /// <summary>X,Y 축 렌더링</summary>
        /// <param name="gl"></param>
        private void DrawAxes(OpenGL gl)
        {
            gl.Color(Config.DocumentAxesColor);
            this.DrawAxes();
        }

        /// <summary>선택 사각형 (러버 밴딩 효과) 렌더링</summary>
        /// <param name="gl"></param>
        private void DrawRubberBand(OpenGL gl)
        {
            float x1;
            float y1;
            this.Dp2Lp(this.mouseLeftDownLocation, out x1, out y1);
            float x2;
            float y2;
            this.Dp2Lp(this.mouseCurrentLocation, out x2, out y2);
            gl.Begin(2U);
            gl.Color(Config.DocumentRubberBandRectColor);
            gl.Vertex(x1, y1, 0.0f);
            gl.Vertex(x2, y1, 0.0f);
            gl.Vertex(x2, y2, 0.0f);
            gl.Vertex(x1, y2, 0.0f);
            gl.End();
        }

        /// <summary>모든 레이어 렌더링</summary>
        /// <param name="gl"></param>
        /// <param name="layers"></param>
        private void DrawLayers(OpenGL gl, Layers layers)
        {
            foreach (Layer layer in (ObservableList<Layer>)layers)
            {
                if (layer.IsVisible)
                {
                    if (!layer.IsMarkerable)
                    {
                        gl.Enable(2852U);
                        gl.LineStipple(1, (ushort)1);
                    }
                    foreach (IEntity entity in (ObservableList<IEntity>)layer)
                    {
                        if (entity is IDrawable drawable4)
                        {
                            if (!(entity is IMarkerable markerable6) || !markerable6.IsMarkerable)
                            {                                
                                gl.Enable(2852U);
                                gl.LineStipple(1, (ushort)1);
                            }

                            markerable6 = entity as IMarkerable;
                            
                            
                            gl.PushMatrix();
                            drawable4.Draw((IView)this);
                            gl.PopMatrix();
                            if (markerable6 == null || !markerable6.IsMarkerable)
                                gl.Disable(2852U);
                        }
                    }
                    if (!layer.IsMarkerable)
                        gl.Disable(2852U);
                }
            }
        }

        /// <summary>선택된 엔티티를 아우르는 외곽 사각형 렌더링</summary>
        /// <param name="gl"></param>
        private void DrawSelectedBoundRect(OpenGL gl)
        {
            this.SelectedBoundRect.Clear();
            foreach (IEntity entity in this.owner.Action.SelectedEntity)
                this.SelectedBoundRect.Union(entity.BoundRect);
            if (this.SelectedBoundRect.IsEmpty)
                return;
            gl.Color(Config.DocumentSelectedBoundRectColor);
            this.SelectedBoundRect.Draw(gl);
        }

        private void DrawLaserSpot(OpenGL gl, Vector2 v)
        {
            float size = this.Dp2Lp(100);
            if (this.spotCount++ % 2 == 0)
            {
                gl.Color(Config.LaserSpotCrossColor1);
                this.DrawLaserSpotCross(v.X, v.Y, size * 0.9f);
                gl.Color(Config.LaserSpotCircleColor1);
                this.DrawLaserSpotCircle(v.X, v.Y, size * 0.5f);
            }
            else
            {
                gl.Color(Config.LaserSpotCrossColor2);
                this.DrawLaserSpotCross(v.X, v.Y, size);
                gl.Color(Config.LaserSpotCircleColor2);
                this.DrawLaserSpotCircle(v.X, v.Y, size * 0.6f);
            }
        }

        private void DrawDivideRects(OpenGL gl)
        {
            if (this.DivideRects == null)
                return;
            gl.Color(Config.DivideRectColor);
            foreach (BoundRect divideRect in this.DivideRects)
                divideRect.Draw(gl);
        }

        internal void PrepareAxes()
        {
            OpenGL openGl = this.glControl.OpenGL;
            this.glListAxes = openGl.GenLists(1);
            openGl.NewList(this.glListAxes, 4864U);
            openGl.Begin(1U);
            openGl.Vertex(0, 10000, 0);
            openGl.Vertex(0, -10000, 0);
            openGl.Vertex(-10000, 0, 0);
            openGl.Vertex(10000, 0, 0);
            openGl.End();
            openGl.EndList();
        }

        internal void DrawAxes()
        {
            OpenGL openGl = this.glControl.OpenGL;
            if (this.glListAxes == 0U)
                this.PrepareAxes();
            openGl.PushMatrix();
            openGl.CallList(this.glListAxes);
            openGl.PopMatrix();
        }

        internal void PrepareLaserMarkTo()
        {
            OpenGL openGl = this.glControl.OpenGL;
            this.glListMarkTo = openGl.GenLists(1);
            openGl.NewList(this.glListMarkTo, 4864U);
            openGl.Begin(4U);
            openGl.Color(Config.EntitySelectedColor);
            openGl.Vertex(0, 0);
            openGl.Color(Config.EntityArrowColor);
            openGl.Vertex(-1.0, 0.5);
            openGl.Color(Config.EntityArrowColor);
            openGl.Vertex(-1.0, -0.5);
            openGl.End();
            openGl.EndList();
        }

        internal void DrawLaserMarkTo(float x, float y, float angle, float size)
        {
            OpenGL openGl = this.glControl.OpenGL;
            if (this.glListMarkTo == 0U)
                this.PrepareLaserMarkTo();
            openGl.PushMatrix();
            openGl.Translate(x, y, 0.0f);
            openGl.Rotate(0.0f, 0.0f, angle);
            openGl.Scale(size, size, 1f);
            openGl.CallList(this.glListMarkTo);
            openGl.PopMatrix();
        }

        internal void PrepareLaserStartFrom()
        {
            OpenGL openGl = this.glControl.OpenGL;
            this.glListLaserStartFrom = openGl.GenLists(1);
            openGl.NewList(this.glListLaserStartFrom, 4864U);
            openGl.Begin(4U);
            openGl.Color(Config.EntitySelectedColor);
            openGl.Vertex(0, 0);
            openGl.Color(Config.EntityArrowColor);
            for (double num = 0.0; num < 360.0; num += (double)Config.AngleFactor)
            {
                double x = Math.Cos(num * (Math.PI / 180.0)) * 0.5;
                double y = Math.Sin(num * (Math.PI / 180.0)) * 0.5;
                openGl.Vertex(x, y);
            }
            openGl.End();
            openGl.EndList();
        }

        internal void DrawLaserStartFrom(float x, float y, float size)
        {
            OpenGL openGl = this.glControl.OpenGL;
            if (this.glListLaserStartFrom == 0U)
                this.PrepareLaserMarkTo();
            openGl.PushMatrix();
            openGl.Translate(x, y, 0.0f);
            openGl.Scale(size, size, 1f);
            openGl.CallList(this.glListLaserStartFrom);
            openGl.PopMatrix();
        }

        internal void PrepareLaserSpotCross()
        {
            OpenGL openGl = this.glControl.OpenGL;
            this.glListLaserSpotCross = openGl.GenLists(1);
            openGl.NewList(this.glListLaserSpotCross, 4864U);
            openGl.Begin(1U);
            openGl.Vertex(-0.5, 0.0, 0.0);
            openGl.Vertex(0.5, 0.0, 0.0);
            openGl.Vertex(0.0, 0.5, 0.0);
            openGl.Vertex(0.0, -0.5, 0.0);
            openGl.End();
            openGl.EndList();
        }

        internal void DrawLaserSpotCross(float x, float y, float size)
        {
            OpenGL openGl = this.glControl.OpenGL;
            if (this.glListLaserSpotCross == 0U)
                this.PrepareLaserSpotCross();
            openGl.PushMatrix();
            openGl.Translate(x, y, 0.0f);
            openGl.Scale(size, size, 1f);
            openGl.CallList(this.glListLaserSpotCross);
            openGl.PopMatrix();
        }

        internal void PrepareLaserSpotCircle()
        {
            OpenGL openGl = this.glControl.OpenGL;
            this.glListLaserSpotCircle = openGl.GenLists(1);
            openGl.NewList(this.glListLaserSpotCircle, 4864U);
            openGl.Begin(6U);
            openGl.Vertex(0, 0);
            for (double num = 0.0; num < 360.0; num += (double)Config.AngleFactor)
            {
                double x = Math.Cos(num * (Math.PI / 180.0)) * 0.5;
                double y = Math.Sin(num * (Math.PI / 180.0)) * 0.5;
                openGl.Vertex(x, y);
            }
            double x1 = Math.Cos(0.0) * 0.5;
            double y1 = Math.Sin(0.0) * 0.5;
            openGl.Vertex(x1, y1);
            openGl.End();
            openGl.EndList();
        }

        internal void DrawLaserSpotCircle(float x, float y, float diameter)
        {
            OpenGL openGl = this.glControl.OpenGL;
            if (this.glListLaserSpotCircle == 0U)
                this.PrepareLaserSpotCircle();
            openGl.PushMatrix();
            openGl.Translate(x, y, 0.0f);
            openGl.Scale(diameter / 2f, diameter / 2f, 1f);
            openGl.CallList(this.glListLaserSpotCircle);
            openGl.PopMatrix();
        }

        /// <summary>외부에서 리스트 버퍼 사용시</summary>
        /// <returns></returns>
        internal uint PrepareStartList()
        {
            this.IsRegeningListId = true;
            OpenGL openGl = this.glControl.OpenGL;
            uint list = openGl.GenLists(1);
            openGl.NewList(list, 4864U);
            return list;
        }

        internal void PrepareEndList()
        {
            this.glControl.OpenGL.EndList();
            this.IsRegeningListId = false;
        }

        internal void DrawList(uint listId) => this.glControl.OpenGL.CallList(listId);

        internal void DeleteList(ref uint listID)
        {
            this.glControl.OpenGL.DeleteLists(listID, 1);
            listID = 0U;
        }
    }
}
