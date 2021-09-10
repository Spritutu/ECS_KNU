using Newtonsoft.Json;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Numerics;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;
using Scanlab.Sirius.ClipperLib;

namespace Scanlab.Sirius
{
    /// <summary>barcode 1d entity</summary>
    public class Barcode1D : IEntity, IMarkerable, IDrawable, ICloneable
    {
        private string name;
        private System.Drawing.Color color;
        private bool isVisible;
        private bool isMarkerable;
        private bool isLocked;
        private Alignment align;
        private Vector2 location;
        private Barcode1DFormat format;
        private bool isGS1;
        private string data;
        private BarcodeWriter barcodeWriter;
        private BitMatrix bitMatrix;
        private BarcodeShapeType shapeType;
        private bool isInvertCell;
        private float width;
        private float height;
        private int px;
        private int py;
        private float angle;
        private float pixelPeriod;
        private float pixelTime;
        private ExtensionChannel pixelChannel;
        private float hatchAngle;
        private float hatchAngle2;
        private float hatchInterval;
        private float hatchExclude;
        private bool isHatchOutline;
        private string patternFileName;
        private Group patternGroup;
        private List<LwPolyline> outlineList = new List<LwPolyline>();
        private Group hatch = new Group()
        {
            Name = "Hatch"
        };
        private bool isRegen;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public EType EntityType => EType.Barcode1D;

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
        public System.Drawing.Color Color2
        {
            get => this.color;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.color = value;
                this.patternGroup.Color2 = this.color;
                foreach (LwPolyline outline in this.outlineList)
                    outline.Color2 = this.color;
                this.hatch.Color2 = this.color;
                this.isRegen = true;
            }
        }

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
                if (this.Owner != null && this.location != value)
                    this.Transit(Vector2.Subtract(value, this.location));
                this.location = value;
                this.isRegen = true;
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
        [DisplayName("Format")]
        [System.ComponentModel.Description("바코드 포맷")]
        public Barcode1DFormat Format
        {
            get => this.format;
            set
            {
                this.format = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("GS1")]
        [System.ComponentModel.Description("GS1 여부")]
        public bool IsGS1
        {
            get => this.isGS1;
            set
            {
                this.isGS1 = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Data")]
        [System.ComponentModel.Description("포함할 데이타 값")]
        public string Data
        {
            get => this.data;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.data = value;
                this.isRegen = true;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Cell Type")]
        [System.ComponentModel.Description("개별 셀 타입")]
        public BarcodeShapeType ShapeType
        {
            get => this.shapeType;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.shapeType = value;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Cell Invsersion")]
        [System.ComponentModel.Description("셀 반전 여부")]
        public bool IsInvertCell
        {
            get => this.isInvertCell;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isInvertCell = value;
                this.isRegen = true;
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
        public int Px
        {
            get => this.px;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                if (this.Owner != null)
                {
                    if (value <= 0)
                        return;
                    float num = (float)value / (float)this.px;
                    if (this.IsFixedAspectRatio)
                        this.py = (int)((double)this.py * (double)num);
                }
                this.px = value;
                this.isRegen = true;
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
                if (this.Owner != null)
                {
                    if (value <= 0)
                        return;
                    float num = (float)value / (float)this.py;
                    if (this.IsFixedAspectRatio)
                        this.px = (int)((double)this.px * (double)num);
                }
                this.py = value;
                this.isRegen = true;
            }
        }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [DisplayName("Real Pixel (Width) ")]
        [System.ComponentModel.Description("폭 (Pixels) : 실제 크기값")]
        public string RealPx
        {
            get
            {
                int? width = this.bitMatrix?.Width;
                BitMatrix bitMatrix = this.bitMatrix;
                if (bitMatrix != null)
                {
                    int height = bitMatrix.Height;
                }
                return width.HasValue ? string.Format("{0}", (object)width) : "(Unknown)";
            }
        }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [DisplayName("Real Pixel (Height) ")]
        [System.ComponentModel.Description("높이 (Pixels) : 실제 크기값")]
        public string RealPy
        {
            get
            {
                BitMatrix bitMatrix = this.bitMatrix;
                if (bitMatrix != null)
                {
                    int width = bitMatrix.Width;
                }
                int? height = this.bitMatrix?.Height;
                return height.HasValue ? string.Format("{0}", (object)height) : "(Unknown)";
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

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [DisplayName("Pitch (Width)")]
        [System.ComponentModel.Description("mm/pixel")]
        public string PitchX
        {
            get
            {
                int? width1 = this.bitMatrix?.Width;
                int? height = this.bitMatrix?.Height;
                if (!width1.HasValue || !height.HasValue)
                    return string.Format("{0:F3}", (object)(float)((double)this.width / (double)(this.px - 1)));
                float width2 = this.width;
                int? nullable1 = width1;
                float? nullable2 = nullable1.HasValue ? new float?((float)(nullable1.GetValueOrDefault() - 1)) : new float?();
                return string.Format("{0:F3}", (object)(nullable2.HasValue ? new float?(width2 / nullable2.GetValueOrDefault()) : new float?()));
            }
        }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [DisplayName("Pitch (Height)")]
        [System.ComponentModel.Description("mm/pixel")]
        public string PitchY
        {
            get
            {
                int? width = this.bitMatrix?.Width;
                int? height1 = this.bitMatrix?.Height;
                if (!width.HasValue || !height1.HasValue)
                    return string.Format("{0:F3}", (object)(float)((double)this.height / (double)(this.py - 1)));
                float height2 = this.height;
                int? nullable1 = height1;
                float? nullable2 = nullable1.HasValue ? new float?((float)(nullable1.GetValueOrDefault() - 1)) : new float?();
                return string.Format("{0:F3}", (object)(nullable2.HasValue ? new float?(height2 / nullable2.GetValueOrDefault()) : new float?()));
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
                int? width = this.bitMatrix?.Width;
                BitMatrix bitMatrix = this.bitMatrix;
                if (bitMatrix != null)
                {
                    int height = bitMatrix.Height;
                }
                float num1 = !width.HasValue ? this.width / (float)(this.px - 1) : this.width / ((float)width.Value - 1f);
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

        public override string ToString() => this.Name + ": " + this.data;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch Angle")]
        [System.ComponentModel.Description("Hatch Angle")]
        public float HatchAngle
        {
            get => this.hatchAngle;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.hatchAngle = value;
                if (this.ShapeType != BarcodeShapeType.Hatch)
                    return;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch Angle2")]
        [System.ComponentModel.Description("2nd Hatch Angle for Cross Hatch")]
        public float HatchAngle2
        {
            get => this.hatchAngle2;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.hatchAngle2 = value;
                if (this.ShapeType != BarcodeShapeType.Hatch)
                    return;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch Interval")]
        [System.ComponentModel.Description("Hatch Interval = Line Interval (mm)")]
        public float HatchInterval
        {
            get => this.hatchInterval;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.hatchInterval = value;
                if (this.ShapeType != BarcodeShapeType.Hatch)
                    return;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch Exclude")]
        [System.ComponentModel.Description("Hatch Exclude = Gap Interval (mm)")]
        public float HatchExclude
        {
            get => this.hatchExclude;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.hatchExclude = value;
                if (this.ShapeType != BarcodeShapeType.Hatch)
                    return;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Hatch")]
        [DisplayName("Hatch Outline")]
        [System.ComponentModel.Description("Remove Hatch Outline")]
        public bool IsHatchOutline
        {
            get => this.isHatchOutline;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.isHatchOutline = value;
                if (this.ShapeType != BarcodeShapeType.Hatch)
                    return;
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Pattern")]
        [DisplayName("File Name")]
        [System.ComponentModel.Description("패턴 파일 이름 (sirius)")]
        [Editor(typeof(BarcodePatternFileBrowser), typeof(UITypeEditor))]
        public string PatternFileName
        {
            get => this.patternFileName;
            set
            {
                if (this.Owner != null && this.isLocked || (value == null || !File.Exists(value)))
                    return;
                Group group = new Group();
                group.IsEnableFastRendering = true;
                group.Name = Path.GetFileName(value);
                IDocument document = DocumentSerializer.OpenSirius(value);
                if (document == null)
                {
                    Logger.Log(Logger.Type.Debug, "fail to open pattern file in barcode: " + value, Array.Empty<object>());
                }
                else
                {
                    foreach (ObservableList<IEntity> layer in (ObservableList<Layer>)document.Layers)
                    {
                        foreach (IEntity entity in layer)
                        {
                            if (!(entity is IPen))
                                group.Add(entity);
                        }
                    }
                    this.PatternGroup = group;
                    this.patternFileName = value;
                    Logger.Log(Logger.Type.Debug, "pattern file has imported into barcode: " + this.patternFileName, Array.Empty<object>());
                    this.isRegen = true;
                }
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Pattern")]
        [DisplayName("Items")]
        [System.ComponentModel.Description("패턴을 구성하는 엔티티 항목(들)")]
        public Group PatternGroup
        {
            get => this.patternGroup;
            set
            {
                this.patternGroup = value;
                if (this.patternGroup == null)
                    return;
                this.patternGroup.IsEnableFastRendering = true;
                this.patternGroup.Regen();
            }
        }

        public Barcode1D()
        {
            this.Node = new TreeNode();
            this.Name = "Barcode";
            this.IsSelected = false;
            this.isVisible = true;
            this.isMarkerable = true;
            this.isLocked = false;
            this.patternGroup = new Group();
            this.color = Config.DefaultColor;
            this.BoundRect = BoundRect.Empty;
            this.location = Vector2.Zero;
            this.align = Alignment.LeftBottom;
            this.isRegen = true;
            this.barcodeWriter = new BarcodeWriter();
            this.format = Barcode1DFormat.Code128;
            this.shapeType = BarcodeShapeType.Line;
            this.width = 5f;
            this.height = 1f;
            this.px = 100;
            this.py = 10;
            this.hatchInterval = (float)Math.Round((double)this.width / (double)this.px / 2.0, 3);
            this.hatchAngle = 90f;
            this.hatchAngle2 = 0.0f;
            this.isHatchOutline = true;
            this.isInvertCell = false;
            this.pixelPeriod = 50f;
            this.pixelTime = 10f;
            this.pixelChannel = ExtensionChannel.ExtAO2;
            this.IsFixedAspectRatio = false;
            this.Repeat = 1U;
        }

        public Barcode1D(string data)
          : this()
        {
            this.Data = data;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public object Clone()
        {
            Barcode1D barcode1D = new Barcode1D()
            {
                Name = this.Name,
                Description = this.Description,
                Owner = this.Owner,
                IsSelected = this.IsSelected,
                IsHighlighted = this.IsHighlighted,
                isVisible = this.IsVisible,
                isMarkerable = this.IsMarkerable,
                isLocked = this.IsLocked,
                color = this.Color2,
                hatchInterval = this.hatchInterval,
                hatchExclude = this.hatchExclude,
                hatchAngle = this.hatchAngle,
                hatchAngle2 = this.hatchAngle2,
                isHatchOutline = this.isHatchOutline,
                isInvertCell = this.isInvertCell,
                BoundRect = this.BoundRect.Clone(),
                Repeat = this.Repeat,
                align = this.align,
                location = this.location,
                width = this.width,
                height = this.height,
                px = this.px,
                py = this.py,
                format = this.format,
                shapeType = this.shapeType,
                data = this.Data,
                isGS1 = this.isGS1,
                IsFixedAspectRatio = this.IsFixedAspectRatio,
                patternFileName = this.PatternFileName,
                pixelChannel = this.pixelChannel,
                pixelPeriod = this.pixelPeriod,
                pixelTime = this.pixelTime,
                bitMatrix = (BitMatrix)this.bitMatrix.Clone(),
                angle = this.angle,
                Tag = this.Tag,
                Node = new TreeNode()
                {
                    Text = this.Node.Text,
                    Tag = this.Node.Tag
                }
            };
            if (this.PatternGroup != null)
                barcode1D.PatternGroup = this.PatternGroup.Clone() as Group;
            foreach (LwPolyline outline in this.outlineList)
                barcode1D.outlineList.Add((LwPolyline)outline.Clone());
            barcode1D.hatch = (Group)this.hatch.Clone();
            return (object)barcode1D;
        }

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            bool flag1 = true;
            IRtc rtc = markerArg.Rtc;
            IRtcExtension rtcExtension = rtc as IRtcExtension;
            rtc.MatrixStack.Push(this.TransitByAlign(this.align));
            rtc.MatrixStack.Push(this.location);
            rtc.MatrixStack.Push((double)this.angle);
            float num1 = this.width / (float)(this.bitMatrix.Width - 1);
            float y = this.height / (float)(this.bitMatrix.Height - 1);
            switch (this.shapeType)
            {
                case BarcodeShapeType.Dot:
                    for (int index1 = 0; (long)index1 < (long)this.Repeat && flag1; ++index1)
                    {
                        for (int a = 0; a < this.bitMatrix.Width && flag1; ++a)
                        {
                            int result;
                            Math.DivRem(a, 2, out result);
                            if (result == 0)
                            {
                                Vector2 vDelta = new Vector2(0.0f, y);
                                flag1 &= rtc.ListJump(new Vector2((float)a * num1, 0.0f));
                                if (rtcExtension != null)
                                    flag1 &= rtcExtension.ListPixelLine(this.PixelPeriod, vDelta, (uint)this.bitMatrix.Height, this.PixelChannel);
                                for (int index2 = 0; index2 < this.bitMatrix.Height; ++index2)
                                {
                                    float pixelTime = this.PixelTime;
                                    float weight = 1f;
                                    bool flag2 = this.bitMatrix[a, this.bitMatrix.Height - index2 - 1];
                                    if (this.isInvertCell)
                                        flag2 = !flag2;
                                    if (flag2)
                                    {
                                        if (rtcExtension != null)
                                            flag1 &= rtcExtension.ListPixel(pixelTime, weight);
                                    }
                                    else if (rtcExtension != null)
                                        flag1 &= rtcExtension.ListPixel(0.0f);
                                    if (!flag1)
                                        break;
                                }
                            }
                            else
                            {
                                Vector2 vDelta = new Vector2(0.0f, -y);
                                flag1 &= rtc.ListJump(new Vector2((float)a * num1, this.Height));
                                if (rtcExtension != null)
                                    flag1 &= rtcExtension.ListPixelLine(this.PixelPeriod, vDelta, (uint)this.bitMatrix.Height, this.PixelChannel);
                                for (int index2 = this.bitMatrix.Height - 1; index2 >= 0; --index2)
                                {
                                    float pixelTime = this.PixelTime;
                                    float weight = 1f;
                                    bool flag2 = this.bitMatrix[a, this.bitMatrix.Height - index2 - 1];
                                    if (this.isInvertCell)
                                        flag2 = !flag2;
                                    if (flag2)
                                    {
                                        if (rtcExtension != null)
                                            flag1 &= rtcExtension.ListPixel(pixelTime, weight);
                                    }
                                    else if (rtcExtension != null)
                                        flag1 &= rtcExtension.ListPixel(0.0f);
                                    if (!flag1)
                                        break;
                                }
                            }
                        }
                    }
                    break;
                case BarcodeShapeType.Line:
                    for (int index1 = 0; (long)index1 < (long)this.Repeat && flag1; ++index1)
                    {
                        int a = 0;
                        for (int index2 = 0; index2 < this.bitMatrix.Width && flag1; ++index2)
                        {
                            bool flag2 = this.bitMatrix[index2, 0];
                            if (this.isInvertCell)
                                flag2 = !flag2;
                            if (flag2)
                            {
                                int result;
                                Math.DivRem(a, 2, out result);
                                flag1 = result != 0 ? flag1 & rtc.ListJump(new Vector2((float)index2 * num1, this.Height)) & rtc.ListMark(new Vector2((float)index2 * num1, 0.0f)) : flag1 & rtc.ListJump(new Vector2((float)index2 * num1, 0.0f)) & rtc.ListMark(new Vector2((float)index2 * num1, this.Height));
                                ++a;
                            }
                        }
                    }
                    break;
                case BarcodeShapeType.Outline:
                    for (int index = 0; (long)index < (long)this.Repeat; ++index)
                    {
                        foreach (LwPolyline outline in this.outlineList)
                        {
                            flag1 &= outline.Mark(markerArg);
                            if (!flag1)
                                break;
                        }
                        if (!flag1)
                            break;
                    }
                    break;
                case BarcodeShapeType.Hatch:
                    for (int index = 0; (long)index < (long)this.Repeat; ++index)
                    {
                        foreach (IMarkerable markerable in (ObservableList<IEntity>)this.hatch)
                        {
                            flag1 &= markerable.Mark(markerArg);
                            if (!flag1)
                                break;
                        }
                        if (this.isHatchOutline)
                        {
                            foreach (LwPolyline outline in this.outlineList)
                            {
                                flag1 &= outline.Mark(markerArg);
                                if (!flag1)
                                    break;
                            }
                        }
                        if (!flag1)
                            break;
                    }
                    break;
                case BarcodeShapeType.Pattern:
                    if (this.PatternGroup != null)
                    {
                        float num2 = this.width / (float)(this.bitMatrix.Width + 1);
                        float num3 = this.height / (float)(this.bitMatrix.Height + 1);
                        for (int index1 = 0; (long)index1 < (long)this.Repeat; ++index1)
                        {
                            for (int index2 = 0; index2 < this.bitMatrix.Width; ++index2)
                            {
                                for (int index3 = 0; index3 < this.bitMatrix.Height; ++index3)
                                {
                                    bool flag2 = this.bitMatrix[index2, this.bitMatrix.Height - index3 - 1];
                                    if (this.isInvertCell)
                                        flag2 = !flag2;
                                    if (flag2)
                                    {
                                        float num4 = (float)index2 * num2 + num2;
                                        float num5 = (float)index3 * num3 + num3;
                                        rtc.MatrixStack.Push((double)num4, (double)num5);
                                        flag1 &= this.PatternGroup.Mark(markerArg);
                                        rtc.MatrixStack.Pop();
                                        if (!flag1)
                                            break;
                                    }
                                    if (!flag1)
                                        break;
                                }
                                if (!flag1)
                                    break;
                            }
                            if (!flag1)
                                break;
                        }
                        break;
                    }
                    break;
            }
            rtc.MatrixStack.Pop();
            rtc.MatrixStack.Pop();
            rtc.MatrixStack.Pop();
            return flag1;
        }

        private void RegenVertextList()
        {
            switch (this.format)
            {
                case Barcode1DFormat.CODABAR:
                    ((BarcodeWriterGeneric)this.barcodeWriter).Format = (BarcodeFormat)2;
                    break;
                case Barcode1DFormat.Code39:
                    ((BarcodeWriterGeneric)this.barcodeWriter).Format = (BarcodeFormat)4;
                    break;
                case Barcode1DFormat.Code93:
                    ((BarcodeWriterGeneric)this.barcodeWriter).Format = (BarcodeFormat)8;
                    break;
                case Barcode1DFormat.Code128:
                    ((BarcodeWriterGeneric)this.barcodeWriter).Format = (BarcodeFormat)16;
                    break;
                case Barcode1DFormat.EAN8:
                    ((BarcodeWriterGeneric)this.barcodeWriter).Format = (BarcodeFormat)64;
                    break;
                case Barcode1DFormat.EAN13:
                    ((BarcodeWriterGeneric)this.barcodeWriter).Format = (BarcodeFormat)128;
                    break;
                case Barcode1DFormat.UPCA:
                    ((BarcodeWriterGeneric)this.barcodeWriter).Format = (BarcodeFormat)16384;
                    break;
                case Barcode1DFormat.UPCE:
                    ((BarcodeWriterGeneric)this.barcodeWriter).Format = (BarcodeFormat)32768;
                    break;
                case Barcode1DFormat.UPCEAN_EXT:
                    ((BarcodeWriterGeneric)this.barcodeWriter).Format = (BarcodeFormat)65536;
                    break;
            }
          ((BarcodeWriterGeneric)this.barcodeWriter).Options = new EncodingOptions()
          {
              Width = this.px,
              Height = this.py,
              Margin = 0,
              PureBarcode = true,
              GS1Format = this.IsGS1
          };
            this.bitMatrix = (BitMatrix)null;
            try
            {
                this.bitMatrix = ((BarcodeWriterGeneric)this.barcodeWriter).Encode(this.Data);
            }
            catch (Exception ex)
            {
                Logger.Log(Logger.Type.Error, ex, "fail to regen barcode 1d : " + this.Data);
                int num = (int)MessageBox.Show(ex.Message ?? "", string.Format("{0} Exception", (object)this.Format), MessageBoxButtons.OK, MessageBoxIcon.Hand);
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
            this.RegenHatch();
            this.isRegen = false;
        }

        private void RegenHatch()
        {
            if (this.bitMatrix == null)
                return;
            this.outlineList.Clear();
            this.hatch.Clear();
            double num1 = (double)this.width / (double)(this.bitMatrix.Width + 1);
            double num2 = (double)this.height / (double)(this.bitMatrix.Height + 1);
            Clipper clipper = new Clipper();
            for (int index1 = 0; index1 < this.bitMatrix.Height; ++index1)
            {
                for (int index2 = 0; index2 < this.bitMatrix.Width; ++index2)
                {
                    bool flag = this.bitMatrix[index2, this.bitMatrix.Height - index1 - 1];
                    if (this.isInvertCell)
                        flag = !flag;
                    if (flag)
                    {
                        double num3 = (double)index2 * num1 + num1;
                        double num4 = (double)index1 * num2 + num2;
                        double num5 = Math.Round(num1 / 2.0 + num3, 3);
                        double num6 = Math.Round(num2 / 2.0 + num4, 3);
                        double num7 = Math.Round(-num1 / 2.0 + num3, 3);
                        double num8 = Math.Round(num2 / 2.0 + num4, 3);
                        double num9 = Math.Round(-num1 / 2.0 + num3, 3);
                        double num10 = Math.Round(-num2 / 2.0 + num4, 3);
                        double num11 = Math.Round(num1 / 2.0 + num3, 3);
                        double num12 = Math.Round(-num2 / 2.0 + num4, 3);
                        clipper.AddPath(new List<IntPoint>(4)
            {
              new IntPoint(num5 * 1000.0, num6 * 1000.0),
              new IntPoint(num7 * 1000.0, num8 * 1000.0),
              new IntPoint(num9 * 1000.0, num10 * 1000.0),
              new IntPoint(num11 * 1000.0, num12 * 1000.0)
            }, PolyType.ptSubject, true);
                    }
                }
            }
            List<List<IntPoint>> solution = new List<List<IntPoint>>();
            clipper.Execute(ClipType.ctUnion, solution);
            foreach (List<IntPoint> intPointList in solution)
            {
                LwPolyline lwPolyline = new LwPolyline();
                foreach (IntPoint intPoint in intPointList)
                {
                    double num3 = Math.Round((double)intPoint.X / 1000.0, 3);
                    double num4 = Math.Round((double)intPoint.Y / 1000.0, 3);
                    lwPolyline.Add(new LwPolyLineVertex((float)num3, (float)num4));
                }
                lwPolyline.IsClosed = true;
                lwPolyline.Regen();
                this.outlineList.Add(lwPolyline);
            }
            if (this.ShapeType == BarcodeShapeType.Hatch)
                this.outlineList.Sort((Comparison<LwPolyline>)((x1, x2) => x2.BoundRect.Left.CompareTo(x1.BoundRect.Left)));
            else
                this.outlineList.Sort((Comparison<LwPolyline>)((x1, x2) => x1.BoundRect.Left.CompareTo(x2.BoundRect.Left)));
            this.hatch = this.Hatch(HatchMode.Line, this.HatchAngle, this.hatchAngle2, this.HatchInterval, this.hatchExclude);
        }

        public Group Hatch(
          HatchMode mode,
          float angle,
          float angle2,
          float interval,
          float exclude)
        {
            Group group1 = new Group();
            group1.IsEnableFastRendering = true;
            foreach (LwPolyline outline in this.outlineList)
            {
                Group group2 = outline.Hatch(mode, angle, angle2, interval, exclude);
                group1.Add((IEntity)group2);
            }
            group1.Sort((Comparison<IEntity>)((x1, x2) => x1.BoundRect.Left.CompareTo(x2.BoundRect.Left)));
            return group1;
        }

        private Vector2 LocationByAlign(Alignment target)
        {
            switch (target)
            {
                case Alignment.LeftTop:
                    return new Vector2(this.BoundRect.Left, this.BoundRect.Top);
                case Alignment.MiddleTop:
                    return new Vector2(this.BoundRect.Center.X, this.BoundRect.Top);
                case Alignment.RightTop:
                    return new Vector2(this.BoundRect.Right, this.BoundRect.Top);
                case Alignment.LeftMiddle:
                    return new Vector2(this.BoundRect.Left, this.BoundRect.Center.Y);
                case Alignment.Center:
                    return this.BoundRect.Center;
                case Alignment.RightMiddle:
                    return new Vector2(this.BoundRect.Right, this.BoundRect.Center.Y);
                case Alignment.LeftBottom:
                    return new Vector2(this.BoundRect.Left, this.BoundRect.Bottom);
                case Alignment.MiddleBottom:
                    return new Vector2(this.BoundRect.Center.X, this.BoundRect.Bottom);
                case Alignment.RightBottom:
                    return new Vector2(this.BoundRect.Right, this.BoundRect.Bottom);
                default:
                    throw new InvalidOperationException("invalid alignment value !");
            }
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

        public bool Draw(IView view)
        {
            if (this.isRegen)
                this.Regen();
            if (!this.IsVisible)
                return true;
            OpenGL renderer = view.Renderer;
            if (this.IsSelected)
            {
                renderer.Color(Config.EntitySelectedColor[0], Config.EntitySelectedColor[1], Config.EntitySelectedColor[2]);
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
            if (this.bitMatrix == null)
                return true;
            float num1 = this.width / (float)(this.bitMatrix.Width - 1);
            float num2 = this.height / (float)(this.bitMatrix.Height - 1);
            renderer.PushMatrix();
            renderer.Translate(this.location.X, this.location.Y, 0.0f);
            renderer.Rotate(0.0f, 0.0f, this.angle);
            Vector2 vector2 = this.TransitByAlign(this.align);
            renderer.Translate(vector2.X, vector2.Y, 0.0f);
            switch (this.shapeType)
            {
                case BarcodeShapeType.Dot:
                    renderer.Begin(0U);
                    for (int index1 = 0; index1 < this.bitMatrix.Height; ++index1)
                    {
                        for (int index2 = 0; index2 < this.bitMatrix.Width; ++index2)
                        {
                            bool flag = this.bitMatrix[index2, this.bitMatrix.Height - index1 - 1];
                            if (this.isInvertCell)
                                flag = !flag;
                            if (flag)
                                renderer.Vertex((float)index2 * num1, (float)index1 * num2);
                        }
                    }
                    renderer.End();
                    break;
                case BarcodeShapeType.Line:
                    renderer.Begin(1U);
                    for (int index = 0; index < this.bitMatrix.Width; ++index)
                    {
                        bool flag = this.bitMatrix[index, 0];
                        if (this.isInvertCell)
                            flag = !flag;
                        if (flag)
                        {
                            renderer.Vertex(Math.Round((double)index * (double)num1, 3), 0.0);
                            renderer.Vertex(Math.Round((double)index * (double)num1, 3), (double)this.Height);
                        }
                    }
                    renderer.End();
                    break;
                case BarcodeShapeType.Outline:
                    using (List<LwPolyline>.Enumerator enumerator = this.outlineList.GetEnumerator())
                    {
                        while (enumerator.MoveNext())
                        {
                            LwPolyline current = enumerator.Current;
                            current.IsSelected = this.IsSelected;
                            current.Color2 = this.color;
                            current.Draw(view);
                        }
                        break;
                    }
                case BarcodeShapeType.Hatch:
                    foreach (IEntity entity in (ObservableList<IEntity>)this.hatch)
                    {
                        IDrawable drawable = entity as IDrawable;
                        entity.IsSelected = this.IsSelected;
                        if (drawable != null)
                        {
                            drawable.Color2 = this.color;
                            drawable?.Draw(view);
                        }
                    }
                    if (this.isHatchOutline)
                    {
                        using (List<LwPolyline>.Enumerator enumerator = this.outlineList.GetEnumerator())
                        {
                            while (enumerator.MoveNext())
                            {
                                LwPolyline current = enumerator.Current;
                                current.IsSelected = this.IsSelected;
                                current.Color2 = this.color;
                                current.Draw(view);
                            }
                            break;
                        }
                    }
                    else
                        break;
                case BarcodeShapeType.Pattern:
                    if (this.PatternGroup != null)
                    {
                        float num3 = this.width / (float)(this.bitMatrix.Width + 1);
                        float num4 = this.height / (float)(this.bitMatrix.Height + 1);
                        for (int index1 = 0; index1 < this.bitMatrix.Height; ++index1)
                        {
                            for (int index2 = 0; index2 < this.bitMatrix.Width; ++index2)
                            {
                                bool flag = this.bitMatrix[index2, this.bitMatrix.Height - index1 - 1];
                                if (this.isInvertCell)
                                    flag = !flag;
                                if (flag)
                                {
                                    float x = (float)index2 * num3 + num3;
                                    float y = (float)index1 * num4 + num4;
                                    renderer.PushMatrix();
                                    renderer.Translate(x, y, 0.0f);
                                    foreach (IEntity entity in (ObservableList<IEntity>)this.PatternGroup)
                                    {
                                        entity.IsSelected = this.IsSelected;
                                        if (entity is IDrawable drawable10)
                                        {
                                            drawable10.Color2 = this.color;
                                            drawable10?.Draw(view);
                                        }
                                    }
                                    renderer.PopMatrix();
                                }
                            }
                        }
                        break;
                    }
                    break;
            }
            renderer.PopMatrix();
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
