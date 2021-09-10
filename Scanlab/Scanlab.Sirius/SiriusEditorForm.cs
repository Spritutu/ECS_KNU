using ECS.Recipe;
using ECS.Recipe.Model;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using INNO6.Core;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace Scanlab.Sirius
{
    public partial class SiriusEditorForm : UserControl
    {
        protected IView view;
        protected IDocument doc;
        protected IRtc rtc;
        protected ILaser laser;
        protected IMarker marker;
        protected IMotor motorZ;
        protected bool hidePropertyGrid;
        protected System.Drawing.Point currentMousePos;

        private OpenFileDialog ofd;
        private SaveFileDialog sfd;
        //internal static LogForm logForm;
        public SiriusEditorForm()
        {
            this.ofd = new OpenFileDialog();
            this.sfd = new SaveFileDialog();



            InitializeComponent();
        }

        /// <summary>문서(Document) 가 변경되었을때 발생하는 이벤트</summary>
        public event SiriusDocumentSourceChanged OnDocumentSourceChanged;

        /// <summary>
        /// 문서(Document)를 신규로 생성할때 발생하는 이벤트
        /// Document.Action.ActNew() 가 호출되어야 함
        /// </summary>
        public event SiriusDocumentNew OnDocumentNew;

        /// <summary>
        /// 문서(Document)가 로드될때 발생하는 이벤트
        /// DocumentSerializer.OpenSirius(파일이름) 으로 불러들인후 Document 에 문서객체를 설정
        /// </summary>
        public event SiriusDocumentOpen OnDocumentOpen;

        /// <summary>
        /// 문서(Document)가 저장될때 발생하는 이벤트
        /// 미리 지정된 파일이름(FileName)에 덮어쓰기 저장됨
        /// Document.Action.ActSave(파일이름) 가 호출되어야 함
        /// </summary>
        public event SiriusDocumentSave OnDocumentSave;

        /// 문서(Document)가 다른이름으로 저장될때 발생하는 이벤트
        ///             Document.Action.ActSave(파일이름) 가 호출되어야 함
        ///             파일이름(FileName ) 속성 변경이 호출되어야 함
        public event SiriusDocumentSave OnDocumentSaveAs;

        /// <summary>편집기에서 새로운 펜을 생성할때 발생하는 이벤트</summary>
        public event SiriusDocumentPenNew OnDocumentPenNew;

        /// <summary>편집기에서 새로운 레이어를 생성할때 발생하는 이벤트</summary>
        public event SiriusDocumentLayerNew OnDocumentLayerNew;

        /// <summary>
        /// 문서(Document)에서 Marker 팝업 창을 띄울때 발생하는 이벤트
        /// 미 등록시 자체 Marker Form 이 팝업됨
        /// </summary>
        public event EventHandler OnMarker;

        /// <summary>문서(Document)에서 Laser 팝업 창을 띄울때 발생하는 이벤트</summary>
        public event EventHandler OnLaser;

        /// <summary>문서(Document)에서 스캐너 보정 2D 팝업 창을 띄울때 발생하는 이벤트</summary>
        public event EventHandler OnCorrection2D;

        /// <summary>문서(Document)에서 스캐너 보정 3D 팝업 창을 띄울때 발생하는 이벤트</summary>
        public event EventHandler OnCorrection3D;

        /// <summary>문서(Document)에서 디지털 입출력 창을 띄울때 발생하는 이벤트</summary>
        public event EventHandler OnIO;

        /// <summary>식별 번호</summary>
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

        /// <summary>뷰 객체</summary>
        public virtual IView View => this.view;

        public virtual IDocument Document
        {
            get => this.doc;
            set
            {
                if (value == null || value.Equals((object)this.doc))
                    return;
                this.doc = value;
                if (this.doc.Layers.Count == 0)
                    this.doc.Action.ActNew();
                List<IView> viewList = new List<IView>();
                if (this.Document != null)
                {
                    viewList.AddRange((IEnumerable<IView>)this.Document.Views);
                    this.doc.Action.OnEntitySelectedChanged -= new Action.EntitySelectedChangedEvent(this.Action_OnSelectedEntityChanged);
                    this.doc.Layers.OnAddItem -= new ObservableList<Layer>.AddItemEventHandler(this.Layer_OnAddItem);
                    this.doc.Layers.OnRemoveItem -= new ObservableList<Layer>.RemoveItemEventHandler(this.Layer_OnRemoveItem);
                    if (this.view != null)
                    {
                        this.doc.Views.Remove(this.view);
                        viewList.Remove(this.view);
                    }
                }
                this.doc.Action.OnEntitySelectedChanged += new Action.EntitySelectedChangedEvent(this.Action_OnSelectedEntityChanged);
                this.doc.Layers.OnAddItem += new ObservableList<Layer>.AddItemEventHandler(this.Layer_OnAddItem);
                this.doc.Layers.OnRemoveItem += new ObservableList<Layer>.RemoveItemEventHandler(this.Layer_OnRemoveItem);
                this.trvEntity.Nodes.Clear();
                this.FileName = this.doc.FileName;
                this.view = (IView)new ViewDefault(this.doc, this.GLcontrol);
                this.doc.Views.Add(this.view);
                this.doc.Views.AddRange((IEnumerable<IView>)viewList);
                this.view.Render();
                this.view.OnZoomFit();
                this.RegenTreeView();
                Delegate[] invocationList = this.OnDocumentSourceChanged?.GetInvocationList();
                if (invocationList != null)
                {
                    foreach (SiriusDocumentSourceChanged documentSourceChanged in invocationList)
                        documentSourceChanged((object)this, this.doc);
                }
                Logger.Log(Logger.Type.Debug, "sirius editor document has changed", Array.Empty<object>());
            }
        }

        /// <summary>RTC 제어 객체</summary>
        public virtual IRtc Rtc
        {
            get => this.rtc;
            set => this.rtc = value;
        }

        /// <summary>레이저 소스 제어 객체</summary>
        public virtual ILaser Laser
        {
            get => this.laser;
            set => this.laser = value;
        }

        /// <summary>마커 제어 객체</summary>
        public virtual IMarker Marker
        {
            get => this.marker;
            set
            {
                if (this.marker != null)
                {
                    this.marker.OnProgress -= new MarkerEventHandler(this.Marker_OnProgress);
                    this.marker.OnFinished -= new MarkerEventHandler(this.Marker_OnFinished);
                    this.pgbProgress.Value = 0;
                }
                this.marker = value;
                if (this.marker == null)
                    return;
                this.marker.OnProgress += new MarkerEventHandler(this.Marker_OnProgress);
                this.marker.OnFinished += new MarkerEventHandler(this.Marker_OnFinished);
            }
        }

        /// <summary>스캐너 Z 축 제어용 객체</summary>
        public virtual IMotor MotorZ
        {
            get => this.motorZ;
            set => this.motorZ = value;
        }

        protected virtual void Document_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) => this.Document.Action.ActDocumentPropertyChanged(this.Document, e.ChangedItem.PropertyDescriptor.Name, e.OldValue, e.ChangedItem.Value);

        private void Marker_OnProgress(IMarker sender, IMarkerArg arg)
        {
            TimeSpan timeSpan = arg.EndTime - arg.StartTime;
            if (this.statusStrip1.InvokeRequired)
                this.statusStrip1.BeginInvoke((MethodInvoker)(() => this.pgbProgress.Value = (int)arg.Progress));            
            else
                this.pgbProgress.Value = (int)arg.Progress;
        }

        private void Marker_OnFinished(IMarker sender, IMarkerArg arg)
        {
            TimeSpan timeSpan = arg.EndTime - arg.StartTime;
            if (this.statusStrip1.InvokeRequired)
                this.statusStrip1.BeginInvoke((MethodInvoker)(() => this.pgbProgress.Value = (int)arg.Progress));
            else
                this.pgbProgress.Value = (int)arg.Progress;
        }

        /// <summary>오른쪽 속성창 보여주기 여부</summary>
        public virtual bool HidePropertyGrid
        {
            get => this.hidePropertyGrid;
            set
            {
                this.hidePropertyGrid = value;
                if (this.hidePropertyGrid)
                {
                    this.splitContainer1.Panel2Collapsed = true;
                    this.splitContainer1.Panel2.Hide();
                }
                else
                {
                    this.splitContainer1.Panel2Collapsed = false;
                    this.splitContainer1.Panel2.Show();
                }
            }
        }

        protected virtual void SiriusEditorForm_Disposed(object sender, EventArgs e) => this.Document?.Action?.ActEntityLaserPathSimulateStop();

        private void Logger_OnLogged(Logger.Type type, string message)
        {
            if (!this.IsHandleCreated)
                return;
            //this.BeginInvoke((MethodInvoker)(() => SiriusEditorForm.logForm.Log(type, message)));
        }




        public virtual float JogDistance { get; set; } = 1f;

        public override void Refresh()
        {
            base.Refresh();
            this.trvEntity.Refresh();
            this.ppgEntity.Refresh();
        }

        protected virtual void OnInitialized(object sender, EventArgs e) => this.view?.OnInitialized(sender, e);

        protected virtual void OnResized(object sender, EventArgs e) => this.view?.OnResized(sender, e);

        protected virtual void OnMouseDown(object sender, MouseEventArgs e) => this.view?.OnMouseDown(sender, e);

        protected virtual void OnMouseUp(object sender, MouseEventArgs e) => this.view?.OnMouseUp(sender, e);

        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            this.currentMousePos = e.Location;
            if (this.view == null)
                return;
            this.view.OnMouseMove(sender, e);
            float x;
            float y;
            this.view.Dp2Lp(e.Location, out x, out y);
            this.lblXPos.Text = string.Format("X: {0:F3}", (object)x);
            this.lblYPos.Text = string.Format("Y: {0:F3}", (object)y);
        }

        protected virtual void OnMouseWheel(object sender, MouseEventArgs e) => this.view?.OnMouseWheel(sender, e);

        protected virtual void OnDraw(object sender, RenderEventArgs args)
        {
            if (this.view == null)
                return;
            this.lblRenderTime.Text = string.Format("Render: {0} ms", (object)this.view.OnDraw());
            if (this.view.SelectedBoundRect.IsEmpty)
            {
                this.lblBound.Text = string.Empty;
                this.lblCenter.Text = string.Empty;
                this.lblWH.Text = string.Empty;
            }
            else
            {
                this.lblBound.Text = this.view.SelectedBoundRect.ToString();
                this.lblCenter.Text = string.Format("{0:F3}, {1:F3}", (object)this.view.SelectedBoundRect.Center.X, (object)this.view.SelectedBoundRect.Center.Y);
                this.lblWH.Text = string.Format("{0:F3}, {1:F3}", (object)this.view.SelectedBoundRect.Width, (object)this.view.SelectedBoundRect.Height);
            }
        }

        protected virtual void RegenTreeView()
        {
            this.trvEntity.BeginUpdate();
            this.trvEntity.Nodes.Clear();
            int num1 = 0;
            foreach (Layer layer in (ObservableList<Layer>)this.Document.Layers)
            {
                this.Layer_OnAddItem((ObservableList<Layer>)this.Document.Layers, num1++, layer);
                int num2 = 0;
                foreach (IEntity e in (ObservableList<IEntity>)layer)
                    this.Entity_OnAddItem((ObservableList<IEntity>)layer, num2++, e);
            }
            this.trvEntity.EndUpdate();
        }

        protected virtual void btnDocumentInfo_Click(object sender, EventArgs e)
        {
            PropertyForm propertyForm = new PropertyForm((object)this.Document);
            propertyForm.PropertyGrid.PropertyValueChanged += new PropertyValueChangedEventHandler(this.Document_PropertyValueChanged);
            try
            {
                int num = (int)propertyForm.ShowDialog((IWin32Window)this);
            }
            finally
            {
                propertyForm.PropertyGrid.PropertyValueChanged -= new PropertyValueChangedEventHandler(this.Document_PropertyValueChanged);
            }
        }

        /// <summary>마우스 선택, 트리뷰 선택 등의 이벤트 발생시 내부 action 에 의해 콜백됨</summary>
        /// <param name="doc"></param>
        /// <param name="list"></param>
        protected virtual void Action_OnSelectedEntityChanged(IDocument doc, List<IEntity> list)
        {
            List<TreeNode> treeNodeList = new List<TreeNode>(list.Count);
            foreach (IEntity entity in list)
                treeNodeList.Add(entity.Node);
            this.trvEntity.SelectedNodes = treeNodeList.Count <= 0 ? (List<TreeNode>)null : treeNodeList;
            if (treeNodeList.Count > 0)
                treeNodeList[treeNodeList.Count - 1].EnsureVisible();
            this.trvEntity.Refresh();
            this.lblEntityCount.Text = "Selected: " + list.Count.ToString();
            if (list.Count == 0)
                this.ppgEntity.SelectedObjects = (object[])null;
            else
                this.ppgEntity.SelectedObjects = (object[])list.ToArray();
        }

        protected virtual void Layer_OnAddItem(ObservableList<Layer> sender, int index, Layer l)
        {
            this.trvEntity.BeginUpdate();
            l.OnAddItem += new ObservableList<IEntity>.AddItemEventHandler(this.Entity_OnAddItem);
            l.OnRemoveItem += new ObservableList<IEntity>.RemoveItemEventHandler(this.Entity_OnRemoveItem);
            l.Node.Tag = (object)l;
            if ((sender as Layers).Count == index)
                this.trvEntity.Nodes.Add(l.Node);
            else
                this.trvEntity.Nodes.Insert(index, l.Node);
            l.Index = index;
            this.trvEntity.EndUpdate();
        }

        protected virtual void Layer_OnRemoveItem(ObservableList<Layer> sender, int index, Layer l)
        {
            this.trvEntity.BeginUpdate();
            l.OnAddItem -= new ObservableList<IEntity>.AddItemEventHandler(this.Entity_OnAddItem);
            l.OnRemoveItem -= new ObservableList<IEntity>.RemoveItemEventHandler(this.Entity_OnRemoveItem);
            this.trvEntity.Nodes.Remove(l.Node);
            this.trvEntity.EndUpdate();
        }

        protected virtual void Entity_OnAddItem(ObservableList<IEntity> sender, int index, IEntity e)
        {
            this.trvEntity.BeginUpdate();
            e.Node.Tag = (object)e;
            Layer layer = sender as Layer;
            if (layer.Node.Nodes.Count == index)
                layer.Node.Nodes.Add(e.Node);
            else
                layer.Node.Nodes.Insert(index, e.Node);
            e.Owner = (IEntity)layer;
            e.Index = index;
            this.trvEntity.EndUpdate();
        }

        protected virtual void Entity_OnRemoveItem(
          ObservableList<IEntity> sender,
          int index,
          IEntity e)
        {
            this.trvEntity.BeginUpdate();
            (sender as Layer).Node.Nodes.Remove(e.Node);
            this.trvEntity.EndUpdate();
        }

        /// <summary>트리뷰에서 엔티티 선택시</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void trvEntity_AfterSelect(object sender, TreeViewEventArgs e)
        {
            List<IEntity> eneities = new List<IEntity>(this.trvEntity.SelectedNodes.Count);
            foreach (Layer layer in (ObservableList<Layer>)this.Document.Layers)
            {
                if (this.trvEntity.SelectedNodes.Contains(layer.Node))
                    eneities.Add((IEntity)layer);
                foreach (IEntity entity in (ObservableList<IEntity>)layer)
                {
                    if (this.trvEntity.SelectedNodes.Contains(entity.Node))
                        eneities.Add(entity);
                }
            }
            this.Document.Action.ActEntitySelect(eneities);
        }

        protected virtual void trvEntity_DragEnter(object sender, DragEventArgs e)
        {
            bool flag = false;
            foreach (IEntity entity in this.Document.Action.SelectedEntity)
            {
                if (entity is Layer)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
                e.Effect = DragDropEffects.None;
            else
                e.Effect = e.AllowedEffect;
        }

        protected virtual void trvEntity_DragDrop(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(TreeNode)))
                return;
            TreeNode nodeAt = this.trvEntity.GetNodeAt(this.trvEntity.PointToClient(new System.Drawing.Point(e.X, e.Y)));
            if (this.trvEntity.SelectedNodes.Count == 1 && this.trvEntity.SelectedNodes[0].Equals((object)nodeAt))
                return;
            this.trvEntity.BeginUpdate();
            IEntity tag = (IEntity)nodeAt.Tag;
            if (tag is Layer)
            {
                Layer targetLayer = tag as Layer;
                if (this.trvEntity.SelectedNodes.Count == 1 && this.trvEntity.SelectedNodes[0].Tag is Layer)
                {
                    this.Document.Action.ActLayerDragMove(this.trvEntity.SelectedNodes[0].Tag as Layer, targetLayer, nodeAt.Index);
                    this.trvEntity.EndUpdate();
                    return;
                }
                this.Document.Action.ActEntityDragMove(this.Document.Action.SelectedEntity, targetLayer, 0);
            }
            else
                this.Document.Action.ActEntityDragMove(this.Document.Action.SelectedEntity, tag.Owner as Layer, nodeAt.Index);
            this.trvEntity.EndUpdate();
        }

        protected virtual void trvEntity_DragOver(object sender, DragEventArgs e)
        {
            if (this.trvEntity.GetNodeAt(this.trvEntity.PointToClient(new System.Drawing.Point(e.X, e.Y))) == null)
                e.Effect = DragDropEffects.None;
            else
                e.Effect = DragDropEffects.Move;
        }

        protected virtual void ppgEntity_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) => this.Document.Action.ActEntityPropertyChanged(this.Document.Action.SelectedEntity, e.ChangedItem.PropertyDescriptor.Name, e.OldValue, e.ChangedItem.Value);

        protected virtual void btnNew_Click(object sender, EventArgs e)
        {
            if (this.OnDocumentNew != null)
            {
                Delegate[] invocationList = this.OnDocumentNew?.GetInvocationList();
                if (invocationList == null)
                    return;
                foreach (SiriusDocumentNew siriusDocumentNew in invocationList)
                    siriusDocumentNew((object)this);
            }
            else
            {
                if (DialogResult.Yes != MessageBox.Show("Do you really want to clear the document ?", "Document New", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    return;
                this.OnNew();
            }
        }

        /// <summary>
        /// 문서를 신규로 생성시 호출되는 내부 함수
        /// OnDocumentNew 이벤트 핸들러 사용시 사용자의 이벤트 함수내에서 특정 작업후
        /// 반드시 OnNew를 호출해 주어야 한다
        /// </summary>
        public virtual void OnNew()
        {
            this.trvEntity.Nodes.Clear();
            this.FileName = string.Empty;
            this.Document.Action.ActNew();
            this.Document.Action.ActEntityAdd((IEntity)new PenDefault());
        }


        protected virtual void btnOpen_Click(object sender, EventArgs e)
        {
            if (this.OnDocumentOpen != null)
            {
                Delegate[] invocationList = this.OnDocumentOpen?.GetInvocationList();
                if (invocationList == null)
                    return;
                foreach (SiriusDocumentOpen siriusDocumentOpen in invocationList)
                    siriusDocumentOpen((object)this);
            }
            else
            {
                this.ofd.Filter = "Sirius data files (*.sirius)|*.sirius|All Files (*.*)|*.*";
                this.ofd.Title = "Open File";
                this.ofd.FileName = string.Empty;
                this.ofd.Multiselect = false;
                if (this.ofd.ShowDialog((IWin32Window)this) != DialogResult.OK)
                    return;
                this.OnOpen(this.ofd.FileName);
            }
        }

        protected virtual void btnExport_Click(object sender, EventArgs e)
        {
            this.sfd.Filter = "ECS Recipe data files (*.rcp)|*.rcp|All Files (*.*)|*.*";
            this.sfd.Title = "Save As ...";
            this.sfd.FileName = string.Empty;
            
            if (this.sfd.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;

            OnExport(sfd.FileName);
        }

        public virtual bool OnExport(string fileName)
        {         
            if (!string.IsNullOrEmpty(fileName))
            {
                int i = 0;
                char[] sep = { '.' };
                RECIPE rcp = new RECIPE();

                string recipeName = Path.GetFileName(fileName);
                rcp.RECIPE_NAME = recipeName.Split(sep)[0];
                rcp.RECIPEFILEPATH = fileName;
                
                foreach (var layer in Document.Layers)
                {
                    foreach(var entity in layer.Items)
                    {
                        if(entity.EntityType != EType.Group)
                            continue;

                        Group g = entity as Group;

                        RECIPE_STEP step = new RECIPE_STEP();

                        step.STEP_ID = i++;
                        step.X_POS = g.Location.X;
                        step.Y_POS = g.Location.Y;
                        step.Z_POS = 0;
                        step.R_POS = 0;
                        step.T_POS = 0;
                        step.REPEAT = 1;
                        step.POWER_PERCENT = 10.0;
                        
                        Layer newLayer = new Layer(g.Name);
                        g.Align = Alignment.Center;
                        Vector2 temp = g.Location;
                        g.Location = new Vector2(0.0f, 0.0f);
                        g.Regen();
                        newLayer.Add(g);

                        string directory = Path.GetDirectoryName(fileName);
                        DirectoryInfo info = Directory.CreateDirectory(directory + @"\" + rcp.RECIPE_NAME);
                        string filePath = info.FullName + @"\" + g.Name + @".sirius";
                       
                        IDocument doc = new DocumentDefault(g.Name);
                        doc.Layers.Add(newLayer);
                        DocumentSerializer.Save(doc, filePath);

                        g.Location = temp;

                        step.SCAN_FILE = filePath;
                        rcp.STEP_LIST.Add(step);
                    }                 
                }

                RecipeManager.Instance.SAVE_RECIPE(rcp);
                return true;
            }

            return false;
        }

        /// <summary>
        /// OnDocumentOpen 이벤트 핸들러 사용시 사용자의 이벤트 함수내에서 특정 작업후
        /// 반드시 OnOpen 호출해 주어야 한다
        /// </summary>
        /// <param name="fileName">파일 이름</param>
        /// <returns></returns>
        public virtual bool OnOpen(string fileName)
        {
            if (string.Compare(Path.GetExtension(fileName), ".sirius", true) == 0)
            {
                this.trvEntity.BeginUpdate();
                IDocument document = DocumentSerializer.OpenSirius(fileName);
                this.trvEntity.EndUpdate();
                if (document == null)
                {
                    int num = (int)MessageBox.Show("Fail to open : " + fileName, "File Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    return false;
                }
                this.Document = document;
                this.FileName = this.ofd.FileName;
                return true;
            }
            int num1 = (int)MessageBox.Show("Unsupported file extension : " + fileName, "File Type Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            return false;
        }

        protected virtual void btnImport_Click(object sender, EventArgs e)
        {
            SiriusPreviewForm siriusPreviewForm = new SiriusPreviewForm();
            if (siriusPreviewForm.ShowDialog((IWin32Window)this) != DialogResult.OK || siriusPreviewForm.Entity == null)
                return;
            this.Document.Action.ActEntityAdd(siriusPreviewForm.Entity);
        }


        protected virtual void btnSave_Click(object sender, EventArgs e)
        {
            if (this.OnDocumentSave != null)
            {
                Delegate[] invocationList = this.OnDocumentSave?.GetInvocationList();
                if (invocationList == null)
                    return;
                foreach (SiriusDocumentSave siriusDocumentSave in invocationList)
                    siriusDocumentSave((object)this);
            }
            else if (string.IsNullOrEmpty(this.FileName))
                this.btnSaveAs_Click((object)null, EventArgs.Empty);
            else if (this.OnSave(this.FileName))
            {
                int num1 = (int)MessageBox.Show("Success to save : " + this.FileName, "Document Save", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                int num2 = (int)MessageBox.Show("Fail to save : " + this.FileName, "Document Save", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }


        /// <summary>
        /// OnDocumentSave 이벤트 핸들러 사용시 사용자의 이벤트 함수내에서 특정 작업후
        /// 반드시 OnSave 호출해 주어야 한다
        /// </summary>
        /// <param name="fileName">파일 이름</param>
        /// <returns></returns>
        public bool OnSave(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
                return this.Document.Action.ActSave(fileName);
            this.btnSaveAs_Click((object)null, EventArgs.Empty);
            return true;
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            if (this.OnDocumentSaveAs != null)
            {
                Delegate[] invocationList = this.OnDocumentSaveAs?.GetInvocationList();
                if (invocationList == null)
                    return;
                foreach (SiriusDocumentSave siriusDocumentSave in invocationList)
                    siriusDocumentSave((object)this);
            }
            else
            {
                this.sfd.Filter = "Sirius data files (*.sirius)|*.sirius|All Files (*.*)|*.*";
                this.sfd.Title = "Save As ...";
                this.sfd.FileName = string.Empty;
                if (this.sfd.ShowDialog((IWin32Window)this) != DialogResult.OK)
                    return;
                if (this.OnSaveAs(this.sfd.FileName))
                {
                    int num1 = (int)MessageBox.Show("Success to save : " + this.FileName, "Document Save", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    int num2 = (int)MessageBox.Show("Fail to save : " + this.FileName, "Document Save", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                }
            }
        }

        /// <summary>
        /// OnDocumentSaveAs 이벤트 핸들러 사용시 사용자의 이벤트 함수내에서 특정 작업후
        /// 반드시 OnSaveAs 호출해 주어야 한다
        /// </summary>
        /// <param name="fileName">파일 이름</param>
        /// <returns></returns>
        public bool OnSaveAs(string fileName)
        {
            this.Document.Action.ActSave(fileName);
            this.FileName = this.sfd.FileName;
            return true;
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            this.trvEntity.BeginUpdate();
            this.Document.Action.ActUndo();
            this.trvEntity.EndUpdate();
        }

        private void btnReDo_Click(object sender, EventArgs e)
        {
            this.trvEntity.BeginUpdate();
            this.Document.Action.ActRedo();
            this.trvEntity.EndUpdate();
        }

        protected virtual void btnCopy_Click(object sender, EventArgs e) => this.Document.Action.ActEntityCopy(this.Document.Action.SelectedEntity);

        protected virtual void btnCut_Click(object sender, EventArgs e) => this.Document.Action.ActEntityCut(this.Document.Action.SelectedEntity);

        protected virtual void btnPaste_Click(object sender, EventArgs e)
        {
            if (Action.ClipBoard.Count <= 0)
            {
                int num = (int)MessageBox.Show("No Data in ClipBoard", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                float x;
                float y;
                this.view.Dp2Lp(this.currentMousePos, out x, out y);
                this.Document.Action.ActEntityPaste(this.Document.Layers.Active, x, y);
            }
        }

        private void btnPasteClone_Click(object sender, EventArgs e)
        {
            if (Action.ClipBoard.Count <= 0)
            {
                int num = (int)MessageBox.Show("No Data in ClipBoard", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
                this.Document.Action.ActEntityPasteClone(this.Document.Layers.Active);
        }

        protected virtual void btnPasteArray_Click(object sender, EventArgs e)
        {
            if (Action.ClipBoard.Count <= 0)
            {
                int num = (int)MessageBox.Show("No Data in ClipBoard", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                PasteForm pasteForm = new PasteForm();
                pasteForm.Clipboard = Action.ClipBoard;
                if (pasteForm.ShowDialog((IWin32Window)this) != DialogResult.OK)
                    return;
                this.trvEntity.BeginUpdate();
                this.Document.Action.ActEntityPasteArray(this.Document.Layers.Active, pasteForm.Result, pasteForm.Position.X, pasteForm.Position.Y);
                this.trvEntity.EndUpdate();
            }
        }

        protected virtual void btnZoomOut_Click(object sender, EventArgs e) => this.view?.OnZoomOut(new System.Drawing.Point(this.GLcontrol.Width / 2, this.GLcontrol.Height / 2));

        protected virtual void btnZoomIn_Click(object sender, EventArgs e) => this.view?.OnZoomIn(new System.Drawing.Point(this.GLcontrol.Width / 2, this.GLcontrol.Height / 2));

        protected virtual void btnZoomFit_Click(object sender, EventArgs e) => this.view?.OnZoomFit();

        protected virtual void btnPan_Click(object sender, EventArgs e) => this.view?.OnPan(this.btnPan.Checked);

        protected virtual void btnExplode_Click(object sender, EventArgs e)
        {
            this.trvEntity.BeginUpdate();
            this.Document.Action.ActEntityExplode(this.Document.Action.SelectedEntity);
            this.trvEntity.EndUpdate();
        }


        protected virtual void btnHatch_Click(object sender, EventArgs e)
        {
            if (this.Document.Action.SelectedEntity == null || this.Document.Action.SelectedEntity.Count == 0)
            {
                int num = (int)MessageBox.Show("Please select target entity at first", "Hatch", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                HatchForm hatchForm = new HatchForm();
                if (DialogResult.OK != hatchForm.ShowDialog((IWin32Window)this))
                    return;
                this.Document.Action.ActEntityHatch(this.Document.Action.SelectedEntity, hatchForm.Mode, hatchForm.Angle, hatchForm.Angle2, hatchForm.Interval, hatchForm.Exclude);
            }
        }

        protected virtual void btnDelete_Click(object sender, EventArgs e)
        {
            this.trvEntity.BeginUpdate();
            this.Document.Action.ActEntityDelete(this.Document.Action.SelectedEntity);
            this.trvEntity.EndUpdate();
        }

        protected virtual void btnRotateCCW_Click(object sender, EventArgs e)
        {
            BoundRect boundRect = new BoundRect();
            foreach (IEntity entity in this.Document.Action.SelectedEntity)
                boundRect.Union(entity.BoundRect);
            if (boundRect.IsEmpty)
                return;
            this.Document.Action.ActEntityRotate(this.Document.Action.SelectedEntity, 90f, boundRect.Center.X, boundRect.Center.Y);
        }

        protected virtual void btnRotateCW_Click(object sender, EventArgs e)
        {
            BoundRect boundRect = new BoundRect();
            foreach (IEntity entity in this.Document.Action.SelectedEntity)
                boundRect.Union(entity.BoundRect);
            if (boundRect.IsEmpty)
                return;
            this.Document.Action.ActEntityRotate(this.Document.Action.SelectedEntity, -90f, boundRect.Center.X, boundRect.Center.Y);
        }

        protected virtual void btnRotateCustom_Click(object sender, EventArgs e)
        {
            int num = (int)new RotateForm(this.Document).ShowDialog((IWin32Window)this);
        }

        protected virtual void originToolStripMenuItem1_Click(object sender, EventArgs e) => this.Document.Action.ActEntityAlign(this.Document.Action.SelectedEntity, EntityAlign.Origin);
        protected virtual void leftToolStripMenuItem1_Click(object sender, EventArgs e) => this.Document.Action.ActEntityAlign(this.Document.Action.SelectedEntity, EntityAlign.Left);

        protected virtual void rightToolStripMenuItem1_Click(object sender, EventArgs e) => this.Document.Action.ActEntityAlign(this.Document.Action.SelectedEntity, EntityAlign.Right);

        protected virtual void topToolStripMenuItem1_Click(object sender, EventArgs e) => this.Document.Action.ActEntityAlign(this.Document.Action.SelectedEntity, EntityAlign.Top);

        protected virtual void bottomToolStripMenuItem1_Click(object sender, EventArgs e) => this.Document.Action.ActEntityAlign(this.Document.Action.SelectedEntity, EntityAlign.Bottom);

        protected virtual void bottomTopToolStripMenuItem_Click(object sender, EventArgs e) => this.Document.Action.ActEntitySort(this.Document.Action.SelectedEntity, this.Document.Layers.Active, EntitySort.BottomToTop);

        protected virtual void topBottomToolStripMenuItem_Click(object sender, EventArgs e) => this.Document.Action.ActEntitySort(this.Document.Action.SelectedEntity, this.Document.Layers.Active, EntitySort.TopToBottom);

        protected virtual void leftRightToolStripMenuItem_Click(object sender, EventArgs e) => this.Document.Action.ActEntitySort(this.Document.Action.SelectedEntity, this.Document.Layers.Active, EntitySort.LeftToRight);

        protected virtual void rightLeftToolStripMenuItem_Click(object sender, EventArgs e) => this.Document.Action.ActEntitySort(this.Document.Action.SelectedEntity, this.Document.Layers.Active, EntitySort.RightToLeft);

        protected virtual void slowToolStripMenuItem_Click(object sender, EventArgs e) => this.Document.Action.ActEntityLaserPathSimulateStart(this.Document.Action.SelectedEntity, this.View, Action.LaserPathSimSpped.Slow);

        protected virtual void normalToolStripMenuItem_Click(object sender, EventArgs e) => this.Document.Action.ActEntityLaserPathSimulateStart(this.Document.Action.SelectedEntity, this.View, Action.LaserPathSimSpped.Normal);

        protected virtual void fastToolStripMenuItem_Click(object sender, EventArgs e) => this.Document.Action.ActEntityLaserPathSimulateStart(this.Document.Action.SelectedEntity, this.View, Action.LaserPathSimSpped.Fast);

        protected virtual void stopToolStripMenuItem_Click(object sender, EventArgs e) => this.Document.Action.ActEntityLaserPathSimulateStop();

        protected virtual void btnPoint_Click(object sender, EventArgs e)
        {
            Point point = new Point(0.0f, 0.0f);
            if (DialogResult.OK != new PropertyForm((object)point).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)point);
        }

        protected virtual void btnPoints_Click(object sender, EventArgs e)
        {
            Points points = new Points(new List<Vertex>()
            {
                new Vertex(1f, 1f),
                new Vertex(1f, 2f),
                new Vertex(2f, 2f),
                new Vertex(3f, 4f)
            });

            if (DialogResult.OK != new PropertyForm((object)points).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)points);
        }

        protected virtual void btnRaster_Click(object sender, EventArgs e)
        {
            Raster raster = new Raster();
            if (DialogResult.OK != new PropertyForm((object)raster).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)raster);
        }

        protected virtual void btnLine_Click(object sender, EventArgs e)
        {
            Line line = new Line(0.0f, 0.0f, 10f, 10f);
            if (DialogResult.OK != new PropertyForm((object)line).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)line);
        }

        protected virtual void btnArc_Click(object sender, EventArgs e)
        {
            Arc arc = new Arc(0.0f, 0.0f, 10f, 0.0f, 90f);
            if (DialogResult.OK != new PropertyForm((object)arc).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)arc);
        }

        protected virtual void btnCircle_Click(object sender, EventArgs e)
        {
            Circle circle = new Circle(0.0f, 0.0f, 10f);
            if (DialogResult.OK != new PropertyForm((object)circle).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)circle);
        }

        protected virtual void btnEllipse_Click(object sender, EventArgs e)
        {
            Ellipse ellipse = new Ellipse(0.0f, 0.0f, 10f, 6f, 0.0f, 360f, 0.0f);
            if (DialogResult.OK != new PropertyForm((object)ellipse).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)ellipse);
        }

        protected virtual void btnTrepan_Click(object sender, EventArgs e)
        {
            Trepan trepan = new Trepan(0.0f, 0.0f, 10f, 2f);
            if (DialogResult.OK != new PropertyForm((object)trepan).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)trepan);
        }

        protected virtual void btnRectangle_Click(object sender, EventArgs e)
        {
            Rectangle rectangle = new Rectangle(0.0f, 0.0f, 10f, 10f);
            if (DialogResult.OK != new PropertyForm((object)rectangle).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)rectangle);
        }

        protected virtual void btnLWPolyline_Click(object sender, EventArgs e)
        {
            LwPolyline lwPolyline = new LwPolyline();
            lwPolyline.Add(new LwPolyLineVertex(55.3005f, 125.1903f));
            lwPolyline.Add(new LwPolyLineVertex(80.5351f, 161.2085f));
            lwPolyline.Add(new LwPolyLineVertex(129.8027f, 148.6021f, -1.3108f));
            lwPolyline.Add(new LwPolyLineVertex(107.5722f, 109.5824f, 0.8155f));
            lwPolyline.Add(new LwPolyLineVertex(77.531f, 89.7724f));
            lwPolyline.IsClosed = true;
            if (DialogResult.OK != new PropertyForm((object)lwPolyline).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)lwPolyline);
        }

        protected virtual void btnSpiral_Click(object sender, EventArgs e)
        {
            Spiral spiral = new Spiral(2f, 10f, 10, true);
            if (DialogResult.OK != new PropertyForm((object)spiral).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)spiral);
        }

        private void mnuText_Click(object sender, EventArgs e)
        {

        }

        protected virtual void btnText_Click(object sender, EventArgs e)
        {
        }

        protected virtual void mnuSiriusText_Click(object sender, EventArgs e)
        {
            SiriusText siriusText = new SiriusText("HELLO");
            if (DialogResult.OK != new PropertyForm((object)siriusText).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)siriusText);
        }

        protected virtual void timeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiriusTextTime siriusTextTime = new SiriusTextTime();
            if (DialogResult.OK != new PropertyForm((object)siriusTextTime).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)siriusTextTime);
        }

        protected virtual void dateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiriusTextDate siriusTextDate = new SiriusTextDate();
            if (DialogResult.OK != new PropertyForm((object)siriusTextDate).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)siriusTextDate);
        }

        protected virtual void serialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiriusTextSerial siriusTextSerial = new SiriusTextSerial();
            if (DialogResult.OK != new PropertyForm((object)siriusTextSerial).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)siriusTextSerial);
        }

        protected virtual void siriusTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SiriusText siriusText = new SiriusText("HELLO");
            if (DialogResult.OK != new PropertyForm((object)siriusText).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)siriusText);
        }

        protected virtual void btnSiriusArcText_Click(object sender, EventArgs e)
        {
            SiriusTextArc siriusTextArc = new SiriusTextArc("HELLO");
            if (DialogResult.OK != new PropertyForm((object)siriusTextArc).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)siriusTextArc);
        }

        protected virtual void btnTextArc_Click(object sender, EventArgs e)
        {
            TextArc textArc = new TextArc("HELLO");
            if (DialogResult.OK != new PropertyForm((object)textArc).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)textArc);
        }

        protected virtual void btnBarcode1D_Click(object sender, EventArgs e)
        {
            Barcode1D barcode1D = new Barcode1D("123456789");
            if (DialogResult.OK != new PropertyForm((object)barcode1D).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)barcode1D);
        }

        protected virtual void mnuDataMatrix_Click(object sender, EventArgs e)
        {
            BarcodeDataMatrix barcodeDataMatrix = new BarcodeDataMatrix("SIRIUS");
            if (DialogResult.OK != new PropertyForm((object)barcodeDataMatrix).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)barcodeDataMatrix);
        }

        protected virtual void mnuQRCode_Click(object sender, EventArgs e)
        {
            BarcodeQR barcodeQr = new BarcodeQR("SIRIUS");
            if (DialogResult.OK != new PropertyForm((object)barcodeQr).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)barcodeQr);
        }

        protected virtual void btnHPGL_Click(object sender, EventArgs e) => this.btnImport_Click(sender, e);

        protected virtual void mnuHPGL_Click(object sender, EventArgs e)
        {
            this.ofd.Filter = "DXF data files (*.dxf)|*.dxf|HPGL Files (*.plt)|*.plt";
            this.ofd.Title = "Open File";
            this.ofd.FileName = string.Empty;
            this.ofd.Multiselect = false;
            if (this.ofd.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            IDocument document = this.Document;
            document.Action.ActNew();
            string extension = Path.GetExtension(this.ofd.FileName);
            
            if (string.Compare(extension, ".dxf", true) == 0)
                document.Action.ActImportDxf(this.ofd.FileName, out IEntity importedEntity);
            else if (string.Compare(extension, ".plt", true) == 0)
                document.Action.ActImportHPGL(this.ofd.FileName, out IEntity importedEntity);
    
            this.View.OnZoomFit();   
        }

        protected virtual void btnImage_Click(object sender, EventArgs e)
        {
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            string str1 = string.Empty;
            this.ofd.Filter = "";
            this.ofd.Multiselect = false;
            foreach (ImageCodecInfo imageCodecInfo in imageEncoders)
            {
                string str2 = imageCodecInfo.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                this.ofd.Filter = string.Format("{0}{1}{2} ({3})|{3}", (object)this.ofd.Filter, (object)str1, (object)str2, (object)imageCodecInfo.FilenameExtension);
                str1 = "|";
            }
            this.ofd.Filter = string.Format("{0}{1}{2} ({3})|{3}", (object)this.ofd.Filter, (object)str1, (object)"All Files", (object)"*.*");
            this.ofd.DefaultExt = ".bmp";
            this.ofd.Title = "Import Image File";
            this.ofd.FileName = string.Empty;
            if (this.ofd.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            this.Document.Action.ActEntityAdd((IEntity)new Bitmap(this.ofd.FileName));
        }

        protected virtual void stitchedImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StitchedImage stitchedImage = new StitchedImage();
            if (DialogResult.OK != new PropertyForm((object)stitchedImage).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)stitchedImage);
        }

        private void mnuLoadCells_Click(object sender, EventArgs e)
        {
            if (this.Document.Action.SelectedEntity.Count == 0 || !(this.Document.Action.SelectedEntity[0] is StitchedImage stitchedImage))
                return;
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            string str1 = string.Empty;
            this.ofd.Filter = "";
            foreach (ImageCodecInfo imageCodecInfo in imageEncoders)
            {
                string str2 = imageCodecInfo.CodecName.Substring(8).Replace("Codec", "Files").Trim();
                this.ofd.Filter = string.Format("{0}{1}{2} ({3})|{3}", (object)this.ofd.Filter, (object)str1, (object)str2, (object)imageCodecInfo.FilenameExtension);
                str1 = "|";
            }
            this.ofd.Filter = string.Format("{0}{1}{2} ({3})|{3}", (object)this.ofd.Filter, (object)str1, (object)"All Files", (object)"*.*");
            this.ofd.DefaultExt = ".bmp";
            this.ofd.Title = "Import Image File";
            this.ofd.FileName = string.Empty;
            this.ofd.Multiselect = true;
            if (this.ofd.ShowDialog((IWin32Window)this) != DialogResult.OK)
                return;
            Array.Sort<string>(this.ofd.FileNames);
            for (int index = 0; index < this.ofd.FileNames.Length; ++index)
                stitchedImage.UpdateImage(index, this.ofd.FileNames[index]);
            this.view.Render();
        }

        private void mnuClearCells_Click(object sender, EventArgs e)
        {
            if (this.Document.Action.SelectedEntity.Count == 0 || !(this.Document.Action.SelectedEntity[0] is StitchedImage stitchedImage))
                return;
            stitchedImage.Clear();
            this.view.Render();
        }

        private void mnuPenReturn_Click(object sender, EventArgs e)
        {
            Layer active = this.Document.Layers.Active;
            uint num1 = 0;
            foreach (IEntity entity in (ObservableList<IEntity>)active)
            {
                if (entity is IPen)
                    ++num1;
            }
            if (num1 < 2U)
            {
                int num2 = (int)MessageBox.Show("Pen entity counts must be greater than 2");
            }
            else
                this.Document.Action.ActEntityAdd((IEntity)new PenReturn());
        }

        protected virtual void mnuPen_Click(object sender, EventArgs e)
        {
            if (this.OnDocumentPenNew != null)
            {
                Delegate[] invocationList = this.OnDocumentPenNew?.GetInvocationList();
                if (invocationList == null)
                    return;
                foreach (SiriusDocumentPenNew siriusDocumentPenNew in invocationList)
                    siriusDocumentPenNew((object)this);
            }
            else
            {
                IPen pen1 = (IPen)new PenDefault();
                if (this.Document.Layers.Active != null)
                {
                    IPen pen2 = (IPen)null;
                    foreach (IEntity entity in (ObservableList<IEntity>)this.Document.Layers.Active)
                    {
                        if (entity is IPen)
                            pen2 = entity as IPen;
                    }
                    if (pen2 != null)
                        pen1 = pen2.Clone() as IPen;
                }
                if (DialogResult.OK != new PropertyForm((object)pen1).ShowDialog((IWin32Window)this))
                    return;
                this.OnPenNew(pen1);
            }
        }
        public virtual bool OnPenNew(IPen pen) => pen != null && this.Document.Action.ActEntityAdd((IEntity)pen);

        protected virtual void mnuPenMOTF_Click(object sender, EventArgs e)
        {
            PenMotf penMotf = new PenMotf();
            if (DialogResult.OK != new PropertyForm((object)penMotf).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)penMotf);
        }

        protected virtual void writeDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WriteData writeData = new WriteData();
            if (DialogResult.OK != new PropertyForm((object)writeData).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)writeData);
        }

        protected virtual void mnuWriteExt16_Click(object sender, EventArgs e)
        {
            WriteDataExt16 writeDataExt16 = new WriteDataExt16();
            if (DialogResult.OK != new PropertyForm((object)writeDataExt16).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)writeDataExt16);
        }


        protected virtual void btnTimer_Click(object sender, EventArgs e)
        {
            Timer timer = new Timer();
            if (DialogResult.OK != new PropertyForm((object)timer).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)timer);
        }

        private void mnuMOTFBeginEnd_Click(object sender, EventArgs e)
        {
            MotfBegin motfBegin = new MotfBegin();
            if (DialogResult.OK != new PropertyForm((object)motfBegin).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)motfBegin);
            MotfEnd motfEnd = new MotfEnd();
            if (DialogResult.OK != new PropertyForm((object)motfEnd).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)motfEnd);
        }

        protected virtual void mnuOTFExtStartDelay_Click(object sender, EventArgs e)
        {
            MotfExternalStartDelay externalStartDelay = new MotfExternalStartDelay();
            if (DialogResult.OK != new PropertyForm((object)externalStartDelay).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)externalStartDelay);
        }

        protected virtual void mnuMOTFWait_Click(object sender, EventArgs e)
        {
            MotfWait motfWait = new MotfWait();
            if (DialogResult.OK != new PropertyForm((object)motfWait).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)motfWait);
        }

        protected virtual void mnuVectorBeginEnd_Click(object sender, EventArgs e)
        {
            VectorBegin vectorBegin = new VectorBegin();
            if (DialogResult.OK != new PropertyForm((object)vectorBegin).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)vectorBegin);
            VectorEnd vectorEnd = new VectorEnd();
            if (DialogResult.OK != new PropertyForm((object)vectorEnd).ShowDialog((IWin32Window)this))
                return;
            this.Document.Action.ActEntityAdd((IEntity)vectorEnd);
        }

        private void btnLayer_Click(object sender, EventArgs e)
        {
            if (this.OnDocumentLayerNew != null)
            {
                Delegate[] invocationList = this.OnDocumentLayerNew?.GetInvocationList();
                if (invocationList == null)
                    return;
                foreach (SiriusDocumentLayerNew documentLayerNew in invocationList)
                    documentLayerNew((object)this);
            }
            else
            {
                Layer layer = new Layer(string.Format("NoName{0}", (object)this.Document.Action.NewLayerIndex++));
                if (DialogResult.OK != new PropertyForm((object)layer).ShowDialog((IWin32Window)this))
                    return;
                this.OnLayerNew(layer);
            }
        }

        /// <summary>
        /// OnDocumentLayerNew 이벤트 사용시 사용자가 원하는 레이어(Layer) 생성후 OnLayerNew 함수를 통해 생성된 레이어 개체를 전달해 주어야 한다
        /// </summary>
        /// <param name="layer">Layer 상속 구현 객체</param>
        /// <returns></returns>
        public virtual bool OnLayerNew(Layer layer) => layer != null && this.Document.Action.ActEntityAdd((IEntity)layer);


        protected virtual void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {
            bool flag1 = false;
            bool flag2 = false;
            foreach (IEntity entity in this.Document.Action.SelectedEntity)
            {
                if (entity is Layer)
                {
                    flag1 = true;
                    break;
                }
                if (entity is Group)
                    flag2 = ((flag2 ? 1 : 0) | 1) != 0;
            }
            if (flag1)
            {
                e.Cancel = true;
            }
            else
            {
                bool flag3 = this.Document.Action.SelectedEntity.Count >= 1;
                this.groupToolStripMenuItem.Enabled = flag3;
                bool flag4 = flag2;
                this.ungroupToolStripMenuItem.Enabled = flag4;
                e.Cancel = !flag3 && !flag4;
            }
        }

        private void groupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.trvEntity.BeginUpdate();
            this.Document.Action.ActEntityGroup(this.Document.Action.SelectedEntity);
            this.trvEntity.EndUpdate();
        }

        private void ungroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.trvEntity.BeginUpdate();
            this.Document.Action.ActEntityUngroup(this.Document.Action.SelectedEntity);
            this.trvEntity.EndUpdate();
        }

        private void mnuJogMenuItem_Click(object sender, EventArgs e)
        {
            string[] strArray = ((string)(sender as ToolStripMenuItem).Tag).Split(',');
            this.Document.Action.ActEntityTransit(this.Document.Action.SelectedEntity, float.Parse(strArray[0]), float.Parse(strArray[1]));
        }

        protected virtual void mnuCorrection2D_Click(object sender, EventArgs e)
        {
            if (this.OnCorrection2D == null)
            {
                int rows = 5;
                int cols = 5;
                float interval = 10f;
                RtcCorrection2D rtcCorrection = new RtcCorrection2D(this.Rtc.KFactor, rows, cols, interval, this.Rtc.CorrectionFiles[0], string.Empty);
                float num1 = -interval * (float)(cols / 2);
                float num2 = interval * (float)(rows / 2);
                Random random = new Random();
                for (int row = 0; row < rows; ++row)
                {
                    for (int col = 0; col < cols; ++col)
                        rtcCorrection.AddRelative(row, col, new Vector2(num1 + (float)col * interval, num2 - (float)row * interval), new Vector2((float)((double)random.Next(20) / 1000.0 - 0.00999999977648258), (float)((double)random.Next(20) / 1000.0 - 0.00999999977648258)));
                }
                int num3 = (int)new Correction2DForm(rtcCorrection).ShowDialog((IWin32Window)this);
            }
            else
            {
                Delegate[] invocationList = this.OnCorrection2D?.GetInvocationList();
                if (invocationList == null)
                    return;
                foreach (EventHandler eventHandler in invocationList)
                    eventHandler((object)this, EventArgs.Empty);
            }
        }

        protected virtual void mnuCorrection3D_Click(object sender, EventArgs e)
        {
            if (this.OnCorrection3D == null)
            {
                int rows = 5;
                int cols = 5;
                float interval = 10f;
                float num1 = 5f;
                float num2 = -5f;
                RtcCorrection3D rtcCorrection = new RtcCorrection3D(this.Rtc.KFactor, rows, cols, interval, num1, num2, this.Rtc.CorrectionFiles[0], string.Empty);
                float num3 = -interval * (float)(cols / 2);
                float num4 = interval * (float)(rows / 2);
                Random random = new Random();
                for (int row = 0; row < rows; ++row)
                {
                    for (int col = 0; col < cols; ++col)
                        rtcCorrection.AddRelative(row, col, new Vector3(num3 + (float)col * interval, num4 - (float)row * interval, num1), new Vector3((float)((double)random.Next(20) / 1000.0 - 0.00999999977648258), (float)((double)random.Next(20) / 1000.0 - 0.00999999977648258), num1));
                }
                for (int row = 0; row < rows; ++row)
                {
                    for (int col = 0; col < cols; ++col)
                        rtcCorrection.AddRelative(row, col, new Vector3(num3 + (float)col * interval, num4 - (float)row * interval, num2), new Vector3((float)((double)random.Next(20) / 1000.0 - 0.00999999977648258), (float)((double)random.Next(20) / 1000.0 - 0.00999999977648258), num2));
                }
                //int num5 = (int)new Correction3DForm(rtcCorrection).ShowDialog((IWin32Window)this);
            }
            else
            {
                Delegate[] invocationList = this.OnCorrection3D?.GetInvocationList();
                if (invocationList == null)
                    return;
                foreach (EventHandler eventHandler in invocationList)
                    eventHandler((object)this, EventArgs.Empty);
            }
        }

        protected virtual void trvEntity_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            int num = (int)this.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            if (this.Document.Action.SelectedEntity == null || this.Document.Action.SelectedEntity.Count == 0)
            {
                int num = (int)MessageBox.Show("Please select target entity at first", "Hatch", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                DivideForm divideForm = new DivideForm(this.Document.Action.SelectedEntity, this.View);
                if (DialogResult.OK != divideForm.ShowDialog((IWin32Window)this))
                    return;
                this.Document.Action.ActEntityDivide(this.Document.Action.SelectedEntity, divideForm.Rects);
            }
        }

        private void mnuCorrection2D_Click_1(object sender, EventArgs e)
        {
            if (this.OnCorrection2D == null)
            {
                int rows = 5;
                int cols = 5;
                float interval = 10f;
                RtcCorrection2D rtcCorrection = new RtcCorrection2D(this.Rtc.KFactor, rows, cols, interval, this.Rtc.CorrectionFiles[0], string.Empty);
                float num1 = -interval * (float)(cols / 2);
                float num2 = interval * (float)(rows / 2);
                Random random = new Random();
                for (int row = 0; row < rows; ++row)
                {
                    for (int col = 0; col < cols; ++col)
                        rtcCorrection.AddRelative(row, col, new Vector2(num1 + (float)col * interval, num2 - (float)row * interval), new Vector2((float)((double)random.Next(20) / 1000.0 - 0.00999999977648258), (float)((double)random.Next(20) / 1000.0 - 0.00999999977648258)));
                }
                int num3 = (int)new Correction2DForm(rtcCorrection).ShowDialog((IWin32Window)this);
            }
            else
            {
                Delegate[] invocationList = this.OnCorrection2D?.GetInvocationList();
                if (invocationList == null)
                    return;
                foreach (EventHandler eventHandler in invocationList)
                    eventHandler((object)this, EventArgs.Empty);
            }
        }
    }


}
