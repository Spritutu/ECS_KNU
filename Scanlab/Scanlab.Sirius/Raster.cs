// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Raster
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>Dot 의 래스터 가공시 가공 방향</summary>
    public enum RasterDirection
    {
        /// <summary>좌에서 우로</summary>
        LeftToRight,
        /// <summary>우에서 좌로</summary>
        RightToLeft,
        /// <summary>상에서 하로</summary>
        TopToBottom,
        /// <summary>하에서 상으로</summary>
        BottomToTop,
    }
    /// <summary>raster entity</summary>
    public class Raster : IEntity, IMarkerable, IDrawable, ICloneable, IDisposable
    {
        [JsonIgnore]
        [Browsable(false)]
        internal Dictionary<IView, uint> Views = new Dictionary<IView, uint>();
        private string name;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private Alignment align;
        private Vector2 location;
        private float width;
        private float height;
        private int px;
        private int py;
        private float angle;
        private RasterDirection direction;
        private float pixelPeriod;
        private float pixelTime;
        private ExtensionChannel pixelChannel;
        private bool isRegen;
        private bool disposed;
        private const int acc_decel_compensate_pixel_counts = 5;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Raster;

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

        [RefreshProperties(RefreshProperties.All)]
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

        /// <summary>
        /// opengl 의 고속 렌더링을 지원하여 렌더링 속도를 올리기 위한 내부 옵션
        /// false : 모두 새로 그림
        /// true : openg의 buffer를 이용한 버퍼 그리기
        /// </summary>
        [Browsable(false)]
        public bool IsEnableFastRendering { get; set; }

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
        public int Px
        {
            get => this.px;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.px = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Height (Pixel)")]
        [System.ComponentModel.Description("높이 (Pixels)")]
        public int Py
        {
            get => this.py;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.py = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

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
        [DisplayName("X Pixel Speed")]
        [System.ComponentModel.Description("픽셀간격을 가공하는 속도 (mm/s)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public float XPixelSpeed
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
        [DisplayName("Y Pixel Speed")]
        [System.ComponentModel.Description("픽셀간격을 가공하는 속도 (mm/s)")]
        public float YPixelSpeed
        {
            get
            {
                if (this.py < 1)
                    return 0.0f;
                float num1 = this.height / ((float)this.py - 1f);
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

        [JsonIgnore]
        [Browsable(false)]
        public TreeNode Node { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public int Index { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public object Tag { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public AciColor Color { get; set; }

        public override string ToString() => string.Format("{0}: {1}X{2}", (object)this.name, (object)this.px, (object)this.py);

        public Raster()
        {
            this.Node = new TreeNode();
            this.Name = nameof(Raster);
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.Color2 = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.location = Vector2.Zero;
            this.direction = RasterDirection.LeftToRight;
            this.align = Alignment.LeftBottom;
            this.Repeat = 1U;
            this.width = 1f;
            this.height = 1f;
            this.px = 100;
            this.py = 100;
            this.pixelTime = 10f;
            this.pixelPeriod = 100f;
            this.pixelChannel = ExtensionChannel.ExtAO2;
            this.IsEnableFastRendering = true;
            this.isRegen = true;
        }

        ~Raster()
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

        private void Dispose(bool disposing)
        {
            if (this.disposed || !disposing)
                return;
            foreach (IView key in this.Views.Keys.ToList<IView>())
            {
                ViewDefault viewDefault = key as ViewDefault;
                uint view = this.Views[key];
                ref uint local = ref view;
                viewDefault.DeleteList(ref local);
                this.Views[key] = view;
            }
            this.disposed = true;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone() => (object)new Raster()
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
            IsEnableFastRendering = this.IsEnableFastRendering,
            px = this.px,
            py = this.py,
            direction = this.Direction,
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

        /// <summary>수평방향 가공</summary>
        /// <param name="px"></param>
        /// <param name="btmToTop"></param>
        /// <param name="markerArg"></param>
        /// <returns></returns>
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
                flag2 = flag1 & rtc.ListJump(new Vector2((float)px * num, -5f * num));
                if (rtcExtension != null)
                    flag2 &= rtcExtension.ListPixelLine(this.PixelPeriod, vDelta, (uint)(this.py + 10), this.PixelChannel);
                for (int index = 0; index < 5; ++index)
                {
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(0.0f);
                    if (!flag2)
                        break;
                }
                for (int index = 0; index < this.py; ++index)
                {
                    float weight = 1f;
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(this.PixelTime * weight, weight);
                    if (!flag2)
                        break;
                }
                for (int index = 0; index < 5; ++index)
                {
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(0.0f);
                    if (!flag2)
                        break;
                }
            }
            else
            {
                Vector2 vDelta = new Vector2(0.0f, -y);
                flag2 = flag1 & rtc.ListJump(new Vector2((float)px * num, this.Height + 5f * num));
                if (rtcExtension != null)
                    flag2 &= rtcExtension.ListPixelLine(this.PixelPeriod, vDelta, (uint)(this.py + 10), this.PixelChannel);
                for (int index = 0; index < 5; ++index)
                {
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(0.0f);
                    if (!flag2)
                        break;
                }
                for (int index = this.py - 1; index >= 0; --index)
                {
                    float weight = 1f;
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(this.PixelTime * weight, weight);
                    if (!flag2)
                        break;
                }
                for (int index = 0; index < 5; ++index)
                {
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(0.0f);
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
            float x = this.width / (float)(this.px - 1);
            float num = this.height / (float)(this.py - 1);
            bool flag2;
            if (leftToRight)
            {
                Vector2 vDelta = new Vector2(x, 0.0f);
                flag2 = flag1 & rtc.ListJump(new Vector2(-5f * x, (float)py * num));
                if (rtcExtension != null)
                    flag2 &= rtcExtension.ListPixelLine(this.PixelPeriod, vDelta, (uint)(this.px + 10), this.PixelChannel);
                for (int index = 0; index < 5; ++index)
                {
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(0.0f);
                    if (!flag2)
                        break;
                }
                for (int index = 0; index < this.px; ++index)
                {
                    float weight = 1f;
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(this.PixelTime * weight, weight);
                    if (!flag2)
                        break;
                }
                for (int index = 0; index < 5; ++index)
                {
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(0.0f);
                    if (!flag2)
                        break;
                }
            }
            else
            {
                Vector2 vDelta = new Vector2(-x, 0.0f);
                flag2 = flag1 & rtc.ListJump(new Vector2(this.Width + 5f * x, (float)py * num));
                if (rtcExtension != null)
                    flag2 &= rtcExtension.ListPixelLine(this.PixelPeriod, vDelta, (uint)(this.px + 10), this.PixelChannel);
                for (int index = 0; index < 5; ++index)
                {
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(0.0f);
                    if (!flag2)
                        break;
                }
                for (int index = this.px - 1; index >= 0; --index)
                {
                    float weight = 1f;
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(this.PixelTime * weight, weight);
                    if (!flag2)
                        break;
                }
                for (int index = 0; index < 5; ++index)
                {
                    if (rtcExtension != null)
                        flag2 &= rtcExtension.ListPixel(0.0f);
                    if (!flag2)
                        break;
                }
            }
            return flag2;
        }

        private void RegenVertextList()
        {
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

        private void RegenListID()
        {
            float num1 = this.width / (float)(this.px - 1);
            float num2 = this.height / (float)(this.py - 1);
            foreach (IView key in this.Views.Keys.ToList<IView>())
            {
                OpenGL renderer = key.Renderer;
                ViewDefault viewDefault = key as ViewDefault;
                if (!viewDefault.IsRegeningListId)
                {
                    uint view = this.Views[key];
                    viewDefault.DeleteList(ref view);
                    this.Views[key] = view;
                    uint num3 = viewDefault.PrepareStartList();
                    renderer.Begin(0U);
                    for (int index1 = 0; index1 < this.px; ++index1)
                    {
                        for (int index2 = 0; index2 < this.py; ++index2)
                            renderer.Vertex((float)index1 * num1, (float)index2 * num2);
                    }
                    renderer.End();
                    viewDefault.PrepareEndList();
                    this.Views[key] = num3;
                }
            }
        }

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            if (this.IsEnableFastRendering)
                this.RegenListID();
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
                this.Views.Add(v, 0U);
                this.isRegen = true;
            }
            if (this.isRegen)
                this.Regen();
            if (!this.IsVisible)
                return true;
            ViewDefault viewDefault = v as ViewDefault;
            OpenGL renderer = v.Renderer;
            uint view = this.Views[v];
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
            if (this.IsEnableFastRendering & view > 0U && !viewDefault.IsRegeningListId)
            {
                renderer.PushMatrix();
                renderer.Translate(this.location.X, this.location.Y, 0.0f);
                renderer.Rotate(0.0f, 0.0f, this.angle);
                Vector2 vector2 = this.TransitByAlign(this.align);
                renderer.Translate(vector2.X, vector2.Y, 0.0f);
                viewDefault.DrawList(this.Views[v]);
                renderer.PopMatrix();
            }
            else
            {
                float num1 = this.width / (float)(this.px - 1);
                float num2 = this.height / (float)(this.py - 1);
                renderer.PushMatrix();
                renderer.Translate(this.location.X, this.location.Y, 0.0f);
                renderer.Rotate(0.0f, 0.0f, this.angle);
                Vector2 vector2 = this.TransitByAlign(this.align);
                renderer.Translate(vector2.X, vector2.Y, 0.0f);
                renderer.Begin(0U);
                for (int index1 = 0; index1 < this.px; ++index1)
                {
                    for (int index2 = 0; index2 < this.py; ++index2)
                        renderer.Vertex((float)index1 * num1, (float)index2 * num2);
                }
                renderer.End();
                renderer.PopMatrix();
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
