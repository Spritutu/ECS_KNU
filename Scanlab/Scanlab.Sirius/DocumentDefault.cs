
using Newtonsoft.Json;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;

namespace Scanlab.Sirius
{
    /// <summary>
    /// IDocument 객체를 실제로 구현한 기본 문서 객체
    /// 주의) DocumentDefault 를 상속 구현하는 모든 문서 객체들은 SpiralLab.Sirius 네임스페이스 유지해야함
    /// </summary>
    public class DocumentDefault : IDocument, ICloneable
    {
        protected BoundRect dimension;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Version")]
        [System.ComponentModel.Description("문서 버전")]
        public virtual string Version { get; internal set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Name")]
        [System.ComponentModel.Description("문서 버전")]
        public virtual string Name { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Description")]
        [System.ComponentModel.Description("문서 설명")]
        public virtual string Description { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Basic")]
        [DisplayName("Filename")]
        [System.ComponentModel.Description("현재 작업중인 파일 이름")]
        public virtual string FileName { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Extension Data")]
        [System.ComponentModel.Description("문서의 사용자 부가 데이타")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string ExtensionData { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Extension File")]
        [System.ComponentModel.Description("문서의 외부 확장용 파일 이름")]
        [Editor(typeof(DocExtFileBrowser), typeof(UITypeEditor))]
        public string ExtensionFilePath { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public virtual Action Action { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Dimension")]
        [System.ComponentModel.Description("문서 크기및 위치")]
        //[Editor(typeof(DimensionEditor), typeof(UITypeEditor))]
        public virtual BoundRect Dimension
        {
            get => this.dimension;
            set => this.dimension = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Offset And Rotate")]
        [System.ComponentModel.Description("X, Y 오프셋(mm) 및 회전각도")]
        [TypeConverter(typeof(OffsetTypeConverter))]
        public virtual Offset RotateOffset { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public virtual object Tag { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public virtual List<IView> Views { get; set; }

        /// <summary>container for blocks</summary>
        [Browsable(false)]
        public virtual Blocks Blocks { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        internal virtual IView View { get; set; }

        /// <summary>container for layers</summary>
        [Browsable(false)]
        public virtual Layers Layers { get; set; }

        /// <summary>생성자</summary>
        public DocumentDefault()
        {
            this.Version = Config.DocumentVersion;
            this.Layers = new Layers((IDocument)this);
            this.Blocks = new Blocks((IDocument)this);
            this.Action = new Action((IDocument)this);
            this.Views = new List<IView>();
            this.dimension = BoundRect.Empty;
        }

        /// <summary>생성자</summary>
        /// <param name="name">문서 이름</param>
        public DocumentDefault(string name)
          : this()
        {
            this.Name = name;
        }

        /// <summary>문서 복제</summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            DocumentDefault documentDefault = new DocumentDefault()
            {
                Version = this.Version,
                Name = this.Name,
                Description = this.Description,
                FileName = this.FileName,
                ExtensionFilePath = this.ExtensionFilePath,
                ExtensionData = this.ExtensionData,
                Layers = (Layers)this.Layers.Clone(),
                Blocks = (Blocks)this.Blocks.Clone(),
                Dimension = this.Dimension.Clone(),
                RotateOffset = this.RotateOffset.Clone(),
                Tag = this.Tag,
                Views = new List<IView>((IEnumerable<IView>)this.Views)
            };
            documentDefault.Layers.Owner = (IDocument)documentDefault;
            documentDefault.Blocks.Owner = (IDocument)documentDefault;
            return (object)documentDefault;
        }

        /// <summary>새로운 문서의 초기 상태로 변경</summary>
        public virtual void New()
        {
            this.Version = Config.DocumentVersion;
            this.Name = string.Empty;
            this.Description = string.Empty;
            this.Layers.Clear();
            Layer layer = new Layer(string.Format("NoName{0}", (object)this.Action.NewLayerIndex++));
            this.Layers.Add(layer);
            this.Layers.Active = layer;
            this.Blocks.Clear();
            foreach (IView view in this.Views)
            {
                view.Scale = 0.5f;
                view.ScaleWidth = (float)view.Width / 0.5f;
                view.ScaleHeight = (float)view.Height / 0.5f;
                double num1;
                float num2 = (float)(num1 = 0.0);
                view.CameraY = (float)num1;
                view.CameraX = num2;
                view.Render();
            }
            this.Action.UndoRedoClear();
            this.Action.SelectedEntity = (List<IEntity>)null;
            foreach (IView view in this.Views)
                view.Render();
            Logger.Log(Logger.Type.Debug, "document " + this.Name + ": cleared by new", Array.Empty<object>());
        }

        /// <summary>문서 정보를 뷰(IView)에 렌더링</summary>
        /// <param name="view"></param>
        internal virtual void Draw(IView view)
        {
            OpenGL renderer = view.Renderer;
            this.View = view;
            if ((double)this.RotateOffset.X != 0.0 || (double)this.RotateOffset.Y != 0.0)
            {
                float num = view.Dp2Lp(7);
                renderer.Begin(1U);
                renderer.Color(Config.DocumentRotateOriginColor);
                renderer.Vertex(this.RotateOffset.X - num, this.RotateOffset.Y + num, 0.0f);
                renderer.Vertex(this.RotateOffset.X + num, this.RotateOffset.Y - num, 0.0f);
                renderer.Vertex(this.RotateOffset.X + num, this.RotateOffset.Y + num, 0.0f);
                renderer.Vertex(this.RotateOffset.X - num, this.RotateOffset.Y - num, 0.0f);
                renderer.End();
            }
            if (!this.Dimension.IsEmpty)
            {
                float num = view.Dp2Lp(10);
                renderer.Begin(1U);
                renderer.Color(Config.DocumentOriginColor);
                renderer.Vertex(this.Dimension.Center.X - num, this.Dimension.Center.Y, 0.0f);
                renderer.Vertex(this.Dimension.Center.X + num, this.Dimension.Center.Y, 0.0f);
                renderer.Vertex(this.Dimension.Center.X, this.Dimension.Center.Y - num, 0.0f);
                renderer.Vertex(this.Dimension.Center.X, this.Dimension.Center.Y + num, 0.0f);
                renderer.End();
            }
            if (this.Dimension.IsEmpty)
                return;
            renderer.PushMatrix();
            renderer.Color(Config.DocumentBoundRectColor);
            this.Dimension.Draw(renderer);
            renderer.End();
            renderer.PopMatrix();
        }
    }
}
