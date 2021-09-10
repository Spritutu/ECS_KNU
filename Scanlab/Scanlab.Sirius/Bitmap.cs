
using Newtonsoft.Json;
using SharpGL;
using SharpGL.SceneGraph.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>bitmap entity</summary>
    public class Bitmap : IEntity, IMarkerable, IDrawable, ICloneable, IDisposable
    {
        [JsonIgnore]
        [Browsable(false)]
        internal Dictionary<IView, Texture> Views = new Dictionary<IView, Texture>();
        private string name;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private Alignment align;
        private Vector2 location;
        private string fileName;
        private float width;
        private float height;
        private int px;
        private int py;
        private float angle;
        private RasterDirection direction;
        private bool isInvertColor;
        private float pixelPeriod;
        private float pixelTime;
        private ExtensionChannel pixelChannel;
        private bool isRegen;
        private System.Drawing.Bitmap bitmap;
        private bool disposed;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Bitmap;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Name")]
        [System.ComponentModel.Description("엔티티의 이름")]
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Description")]
        [System.ComponentModel.Description("엔티티에 대한 설명")]
        public string Description { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public AciColor Color { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Color")]
        [System.ComponentModel.Description("색상")]
        public System.Drawing.Color Color2 { get; set; } = Config.DefaultColor;

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Bound")]
        [System.ComponentModel.Description("외각 영역")]
        public BoundRect BoundRect { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Selected")]
        [System.ComponentModel.Description("선택여부")]
        public bool IsSelected { get; set; }

        [Browsable(false)]
        public bool IsHighlighted { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Visible")]
        [System.ComponentModel.Description("스크린에 출력 여부")]
        public bool IsVisible
        {
            get => this.isVisible;
            set => this.isVisible = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Markerable")]
        [System.ComponentModel.Description("레이저 가공 여부")]
        public bool IsMarkerable
        {
            get => this.isMarkerable;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isMarkerable = value;
                if (this.Node.NodeFont == null)
                    this.Node.NodeFont = new Font(Config.NodeFont, (float)Config.NodeFontSize);
                if (this.isMarkerable)
                    this.Node.NodeFont = new Font(Config.NodeFont, (float)Config.NodeFontSize, this.Node.NodeFont.Style & ~FontStyle.Strikeout);
                else
                    this.Node.NodeFont = new Font(Config.NodeFont, (float)Config.NodeFontSize, this.Node.NodeFont.Style | FontStyle.Strikeout);
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Mark Path")]
        [System.ComponentModel.Description("가공 경로를 표시")]
        public bool IsDrawPath { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Locked")]
        [System.ComponentModel.Description("편집 금지 여부")]
        public bool IsLocked
        {
            get => this.isLocked;
            set => this.isLocked = value;
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Align")]
        [System.ComponentModel.Description("정렬 기준위치")]
        public Alignment Align
        {
            get => this.align;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                if (this.Owner != null && this.align != value)
                    this.location = this.BoundRect.LocationByAlign(value);
                this.align = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Location")]
        [System.ComponentModel.Description("위치 좌표값")]
        [TypeConverter(typeof(Vector2Converter))]
        public Vector2 Location
        {
            get => this.location;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                Vector2 delta = Vector2.Subtract(value, this.location);
                if (this.Owner != null)
                    this.Transit(delta);
                this.location = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Repeat")]
        [System.ComponentModel.Description("가공 반복 횟수")]
        public uint Repeat { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("File Name")]
        [System.ComponentModel.Description("Bitmap File name ")]
        public string FileName
        {
            get => this.fileName;
            set
            {
                this.fileName = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Fixed Aspect Ratio")]
        [System.ComponentModel.Description("고정 비율 유지 여부")]
        public bool IsFixedAspectRatio { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Width")]
        [System.ComponentModel.Description("폭")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Width
        {
            get => this.width;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                if (this.Owner != null)
                {
                    float num = value / this.width;
                    if (this.IsFixedAspectRatio)
                        this.height *= num;
                }
                this.width = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Height")]
        [System.ComponentModel.Description("높이")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Height
        {
            get => this.height;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                if (this.Owner != null)
                {
                    float num = value / this.height;
                    if (this.IsFixedAspectRatio)
                        this.width *= num;
                }
                this.height = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Width (Pixel)")]
        [System.ComponentModel.Description("폭 (Pixels)")]
        public int Px => this.px;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Height (Pixel)")]
        [System.ComponentModel.Description("높이 (Pixels)")]
        public int Py => this.py;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Angle")]
        [System.ComponentModel.Description("회전 각도")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float Angle
        {
            get => this.angle;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                float angle = value - this.angle;
                if (this.Owner != null)
                    this.Rotate(angle);
                this.angle = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [DisplayName("Pitch (Width)")]
        [System.ComponentModel.Description("mm/pixel")]
        public string PitchX => string.Format("{0:F3}", (object)(float)((double)this.width / (double)(this.px - 1)));

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [DisplayName("Pitch (Height)")]
        [System.ComponentModel.Description("mm/pixel")]
        public string PitchY => string.Format("{0:F3}", (object)(float)((double)this.height / (double)(this.py - 1)));

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Direction")]
        [System.ComponentModel.Description("레스터 가공 방향: 좌에서 우로, 우에서 좌로")]
        public RasterDirection Direction
        {
            get => this.direction;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.direction = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Invert Color")]
        [System.ComponentModel.Description("흑백 반전 여부 (false : 검은색 기본)")]
        public bool IsInvertColor
        {
            get => this.isInvertColor;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isInvertColor = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Pixel Period Time")]
        [System.ComponentModel.Description("usec")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float PixelPeriod
        {
            get => this.pixelPeriod;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.pixelPeriod = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Pixel Speed")]
        [System.ComponentModel.Description("mm/s")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float PixelSpeed
        {
            get
            {
                if (this.px < 1)
                    return 0.0f;
                float num1 = this.width / ((float)this.px - 1f);
                float num2 = this.PixelPeriod / 1000000f;
                return (double)num2 <= 0.0 ? 0.0f : (float)Math.Round((double)num1 / (double)num2, 3);
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Pixel Time")]
        [System.ComponentModel.Description("usec (less than pixel period time)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float PixelTime
        {
            get => this.pixelTime;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                if ((double)value > (double)this.pixelPeriod)
                    this.pixelTime = this.pixelPeriod;
                else
                    this.pixelTime = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Extension channel")]
        [System.ComponentModel.Description("usec")]
        public ExtensionChannel PixelChannel
        {
            get => this.pixelChannel;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.pixelChannel = value;
            }
        }

        [JsonProperty]
        [Browsable(false)]
        [NotifyParentProperty(true)]
        public string Base64EncodedImage
        {
            get => BitmapHelper.EncodeTo(this.bitmap);
            set
            {
                this.bitmap = BitmapHelper.DecodeFrom(value);
                this.px = this.bitmap.Width;
                this.py = this.bitmap.Height;
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public TreeNode Node { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public int Index { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public object Tag { get; set; }

        public override string ToString() => this.Name + " : " + Path.GetFileName(this.FileName);

        public Bitmap()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Bitmap);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.Color2 = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.location = Vector2.Zero;
            this.direction = RasterDirection.LeftToRight;
            this.align = Alignment.LeftBottom;
            this.IsFixedAspectRatio = true;
            this.PixelChannel = ExtensionChannel.ExtAO2;
            this.pixelTime = 10f;
            this.pixelPeriod = 100f;
            this.Repeat = 1U;
            this.isRegen = true;
        }

        public Bitmap(string fileName)
          : this()
        {
            this.FileName = fileName;
            this.bitmap = BitmapHelper.ConvertToBitmap(this.FileName);
            this.px = this.bitmap.Width;
            this.py = this.bitmap.Height;
            this.width = 10f;
            this.height = this.width * ((float)this.py / (float)this.px);
        }

        public Bitmap(float x, float y, string fileName)
          : this(fileName)
        {
            this.location = new Vector2(x, y);
        }

        public Bitmap(System.Drawing.Bitmap bitmap)
          : this()
        {
            this.bitmap = (System.Drawing.Bitmap)bitmap.Clone();
            this.px = this.bitmap.Width;
            this.py = this.bitmap.Height;
            this.width = 10f;
            this.height = this.width * ((float)this.py / (float)this.px);
        }

        ~Bitmap()
        {
            if (this.disposed)
                return;
            this.Dispose(false);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize((object)this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
                return;
            if (disposing)
            {
                foreach (KeyValuePair<IView, Texture> view in this.Views)
                {
                    OpenGL renderer = (view.Key as ViewDefault).Renderer;
                    view.Value?.Destroy(renderer);
                }
                this.bitmap?.Dispose();
            }
            this.disposed = true;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new Bitmap()
        {
            Name = this.Name,
            Description = this.Description,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            IsHighlighted = this.IsHighlighted,
            isVisible = this.isVisible,
            isMarkerable = this.isMarkerable,
            isLocked = this.IsLocked,
            Color2 = this.Color2,
            BoundRect = this.BoundRect.Clone(),
            Repeat = this.Repeat,
            align = this.align,
            location = this.location,
            width = this.width,
            height = this.height,
            px = this.px,
            py = this.py,
            bitmap = (System.Drawing.Bitmap)this.bitmap.Clone(),
            direction = this.Direction,
            IsInvertColor = this.IsInvertColor,
            IsFixedAspectRatio = this.IsFixedAspectRatio,
            fileName = this.FileName,
            Base64EncodedImage = this.Base64EncodedImage,
            pixelChannel = this.pixelChannel,
            pixelPeriod = this.pixelPeriod,
            pixelTime = this.pixelTime,
            Angle = this.Angle,
            Tag = this.Tag,
            Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            }
        };

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            bool flag = true;
            IRtc rtc = markerArg.Rtc;
            rtc.MatrixStack.Push(this.TransitByAlign(this.align));
            rtc.MatrixStack.Push(this.location);
            rtc.MatrixStack.Push((double)this.angle);
            double num1 = (double)this.width / (double)(this.px - 1);
            double num2 = (double)this.height / (double)(this.py - 1);
            for (int index1 = 0; (long)index1 < (long)this.Repeat; ++index1)
            {
                switch (this.Direction)
                {
                    case RasterDirection.LeftToRight:
                        for (int index2 = 0; index2 < this.px && flag; ++index2)
                        {
                            int result;
                            Math.DivRem(index2, 2, out result);
                            flag &= this.MarkHorizontalLine(index2, result == 0, markerArg);
                            if (!flag)
                                break;
                        }
                        break;
                    case RasterDirection.RightToLeft:
                        for (int index2 = this.px - 1; index2 >= 0 && flag; --index2)
                        {
                            int result;
                            Math.DivRem(index2, 2, out result);
                            flag &= this.MarkHorizontalLine(index2, result == 0, markerArg);
                            if (!flag)
                                break;
                        }
                        break;
                    case RasterDirection.TopToBottom:
                        for (int index2 = this.py - 1; index2 >= 0 && flag; --index2)
                        {
                            int result;
                            Math.DivRem(index2, 2, out result);
                            flag &= this.MarkVerticalLine(index2, result == 0, markerArg);
                            if (!flag)
                                break;
                        }
                        break;
                    case RasterDirection.BottomToTop:
                        for (int index2 = 0; index2 < this.py && flag; ++index2)
                        {
                            int result;
                            Math.DivRem(index2, 2, out result);
                            flag &= this.MarkVerticalLine(index2, result == 0, markerArg);
                            if (!flag)
                                break;
                        }
                        break;
                }
            }
            rtc.MatrixStack.Pop();
            rtc.MatrixStack.Pop();
            rtc.MatrixStack.Pop();
            return flag;
        }

        private bool MarkHorizontalLine(int px, bool btmToTop, IMarkerArg markerArg)
        {
            bool flag1 = true;
            IRtc rtc = markerArg.Rtc;
            IRtcExtension rtcExtension = rtc as IRtcExtension;
            float num = this.width / (float)(this.px - 1);
            float y = this.height / (float)(this.py - 1);
            bool flag2;
            if (btmToTop)
            {
                Vector2 vDelta = new Vector2(0.0f, y);
                flag2 = flag1 & rtc.ListJump(new Vector2((float)px * num, 0.0f));
                if (rtcExtension != null)
                    flag2 &= rtcExtension.ListPixelLine(this.PixelPeriod, vDelta, (uint)this.py, this.PixelChannel);
                for (int index = 0; index < this.py; ++index)
                {
                    System.Drawing.Color pixel = this.bitmap.GetPixel(px, this.py - index - 1);
                    float weight = (float)(1.0 - (0.300000011920929 * (double)pixel.R + 0.589999973773956 * (double)pixel.G + 0.109999999403954 * (double)pixel.B) / (double)byte.MaxValue);
                    if (this.IsInvertColor)
                        weight = 1f - weight;
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(this.PixelTime * weight, weight);
                    if (!flag2)
                        break;
                }
            }
            else
            {
                Vector2 vDelta = new Vector2(0.0f, -y);
                flag2 = flag1 & rtc.ListJump(new Vector2((float)px * num, this.Height));
                if (rtcExtension != null)
                    flag2 &= rtcExtension.ListPixelLine(this.PixelPeriod, vDelta, (uint)this.py, this.PixelChannel);
                for (int index = this.py - 1; index >= 0; --index)
                {
                    System.Drawing.Color pixel = this.bitmap.GetPixel(px, this.py - index - 1);
                    float weight = (float)(1.0 - (0.300000011920929 * (double)pixel.R + 0.589999973773956 * (double)pixel.G + 0.109999999403954 * (double)pixel.B) / (double)byte.MaxValue);
                    if (this.IsInvertColor)
                        weight = 1f - weight;
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(this.PixelTime * weight, weight);
                    if (!flag2)
                        break;
                }
            }
            return flag2;
        }

        /// <summary>수직방향 가공</summary>
        /// <param name="py"></param>
        /// <param name="leftToRight"></param>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        private bool MarkVerticalLine(int py, bool leftToRight, IMarkerArg markerArg)
        {
            bool flag1 = true;
            IRtc rtc = markerArg.Rtc;
            IRtcExtension rtcExtension = rtc as IRtcExtension;
            float x1 = this.width / (float)(this.px - 1);
            float num = this.height / (float)(this.py - 1);
            bool flag2;
            if (leftToRight)
            {
                Vector2 vDelta = new Vector2(x1, 0.0f);
                flag2 = flag1 & rtc.ListJump(new Vector2(0.0f, (float)py * num));
                if (rtcExtension != null)
                    flag2 &= rtcExtension.ListPixelLine(this.PixelPeriod, vDelta, (uint)this.px, this.PixelChannel);
                for (int x2 = 0; x2 < this.px; ++x2)
                {
                    System.Drawing.Color pixel = this.bitmap.GetPixel(x2, this.py - py - 1);
                    float weight = (float)(1.0 - (0.300000011920929 * (double)pixel.R + 0.589999973773956 * (double)pixel.G + 0.109999999403954 * (double)pixel.B) / (double)byte.MaxValue);
                    if (this.IsInvertColor)
                        weight = 1f - weight;
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(this.PixelTime * weight, weight);
                    if (!flag2)
                        break;
                }
            }
            else
            {
                Vector2 vDelta = new Vector2(-x1, 0.0f);
                flag2 = flag1 & rtc.ListJump(new Vector2(this.Width, (float)py * num));
                if (rtcExtension != null)
                    flag2 &= rtcExtension.ListPixelLine(this.PixelPeriod, vDelta, (uint)this.px, this.PixelChannel);
                for (int x2 = this.px - 1; x2 >= 0; --x2)
                {
                    System.Drawing.Color pixel = this.bitmap.GetPixel(x2, this.py - py - 1);
                    float weight = (float)(1.0 - (0.300000011920929 * (double)pixel.R + 0.589999973773956 * (double)pixel.G + 0.109999999403954 * (double)pixel.B) / (double)byte.MaxValue);
                    if (this.IsInvertColor)
                        weight = 1f - weight;
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(this.PixelTime * weight, weight);
                    if (!flag2)
                        break;
                }
            }
            return flag2;
        }

        private void RegenVertextList()
        {
            foreach (IView key in this.Views.Keys.ToList<IView>())
            {
                OpenGL renderer = key.Renderer;
                renderer.Enable(3553U);
                Texture texture = this.Views[key];
                texture?.Destroy(renderer);
                using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(this.Base64EncodedImage)))
                {
                    Image image = Image.FromStream((Stream)memoryStream);
                    texture = new Texture();
                    texture.Create(renderer, (System.Drawing.Bitmap)image);
                }
                this.Views[key] = texture;
                renderer.Disable(3553U);
            }
        }

        private void RegenBoundRect()
        {
            Rectangle rectangle = new Rectangle(0.0f, 0.0f, this.width, this.height);
            rectangle.Owner = (IEntity)this;
            rectangle.Regen();
            rectangle.Align = this.Align;
            rectangle.Location = this.Location;
            rectangle.Rotate(this.Angle);
            this.BoundRect = rectangle.BoundRect.Clone();
        }

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            this.isRegen = false;
        }

        private Vector2 TransitByAlign(Alignment target)
        {
            switch (this.align)
            {
                case Alignment.LeftTop:
                    return new Vector2(0.0f, -this.height);
                case Alignment.MiddleTop:
                    return new Vector2((float)(-(double)this.width / 2.0), -this.height);
                case Alignment.RightTop:
                    return new Vector2(-this.width, -this.height);
                case Alignment.LeftMiddle:
                    return new Vector2(0.0f, (float)(-(double)this.height / 2.0));
                case Alignment.Center:
                    return new Vector2((float)(-(double)this.width / 2.0), (float)(-(double)this.height / 2.0));
                case Alignment.RightMiddle:
                    return new Vector2(-this.width, (float)(-(double)this.height / 2.0));
                case Alignment.LeftBottom:
                    return Vector2.Zero;
                case Alignment.MiddleBottom:
                    return new Vector2((float)(-(double)this.width / 2.0), 0.0f);
                case Alignment.RightBottom:
                    return new Vector2(-this.width, 0.0f);
                default:
                    throw new InvalidOperationException("invalid alignment value !");
            }
        }

        public bool Draw(IView v)
        {
            if (!this.Views.ContainsKey(v))
            {
                this.Views.Add(v, (Texture)null);
                this.isRegen = true;
            }
            if (this.isRegen)
                this.Regen();
            if (!this.IsVisible)
                return true;
            foreach (IView key in this.Views.Keys)
            {
                OpenGL renderer = key.Renderer;
                if (this.IsSelected)
                {
                    renderer.Color(Config.EntitySelectedColor);
                }
                else
                {
                    OpenGL openGl = renderer;
                    System.Drawing.Color color2 = this.Color2;
                    int r = (int)color2.R;
                    color2 = this.Color2;
                    int g = (int)color2.G;
                    color2 = this.Color2;
                    int b = (int)color2.B;
                    openGl.Color((byte)r, (byte)g, (byte)b);
                }
                renderer.Enable(3553U);
                renderer.Disable(2929U);
                renderer.PushMatrix();
                renderer.Translate(this.location.X, this.location.Y, -0.1f);
                renderer.Rotate(0.0f, 0.0f, this.angle);
                Vector2 vector2 = this.TransitByAlign(this.align);
                renderer.Translate(vector2.X, vector2.Y, 0.0f);
                this.Views[key].Bind(renderer);
                renderer.TexParameter(3553U, 10241U, 9728f);
                renderer.TexParameter(3553U, 10240U, 9728f);
                renderer.Begin(7U);
                renderer.TexCoord(0.0f, 0.0f);
                renderer.Vertex(0.0f, this.height);
                renderer.TexCoord(1f, 0.0f);
                renderer.Vertex(this.width, this.height);
                renderer.TexCoord(1f, 1f);
                renderer.Vertex(this.width, 0.0f);
                renderer.TexCoord(0.0f, 1f);
                renderer.Vertex(0, 0);
                renderer.End();
                renderer.PopMatrix();
                renderer.Enable(2929U);
                renderer.Disable(3553U);
            }
            return true;
        }

        public void Transit(Vector2 delta)
        {
            if (this.IsLocked || delta == Vector2.Zero)
                return;
            this.location = Vector2.Add(this.location, delta);
            this.BoundRect.Transit(delta);
        }

        public void Rotate(float angle)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Rotate(float angle, Vector2 rotateCenter)
        {
            if (this.IsLocked || MathHelper.IsZero(angle))
                return;
            this.location = Vector2.Transform(this.location, Matrix3x2.CreateRotation(angle * ((float)Math.PI / 180f), rotateCenter));
            this.angle += angle;
            this.angle = MathHelper.NormalizeAngle(this.angle);
            this.Regen();
        }

        public void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.width *= scale.X;
            this.height *= scale.Y;
            this.Regen();
        }

        public void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero || scale == Vector2.One)
                return;
            this.location = (this.location - scaleCenter) * scale + scaleCenter;
            this.width *= scale.X;
            this.height *= scale.Y;
            this.Regen();
        }

        public bool HitTest(float x, float y, float threshold) => this.IsVisible && this.BoundRect.HitTest(x, y, threshold);

        public bool HitTest(float left, float top, float right, float bottom, float threshold) => this.IsVisible && this.HitTest(new BoundRect(left, top, right, bottom), threshold);

        public bool HitTest(BoundRect br, float threshold) => this.IsVisible && this.BoundRect.HitTest(br, threshold);
    }
}
