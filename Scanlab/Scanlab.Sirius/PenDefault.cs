// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.PenDefault
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>Pen 엔티티 객체 (기본 버전)</summary>
    [JsonObject]
    public class PenDefault : IPen, IEntity, IMarkerable, ICloneable, IEquatable<PenDefault>
    {
        protected string name;
        protected bool isMarkerable;
        protected bool isLocked;
        protected float power;
        protected float frequency;
        protected float pulseWidth;
        protected string dutyCycle;
        protected float laserOnDelay;
        protected float laserOffDelay;
        protected float scannerJumpDelay;
        protected float scannerMarkDelay;
        protected float scannerPolygonDelay;
        protected float jumpSpeed;
        protected float markSpeed;
        protected float laserOnShift;
        protected float timeLag;
        protected float angularLimit;

        [JsonIgnore]
        [Browsable(false)]
        public virtual IEntity Owner { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public virtual EType EntityType => EType.PenDefault;

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Name")]
        [System.ComponentModel.Description("엔티티의 이름")]
        public virtual string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.Node.Text = "Pen: " + value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Description")]
        [System.ComponentModel.Description("엔티티에 대한 설명")]
        public virtual string Description { get; set; }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Color")]
        [System.ComponentModel.Description("펜 지정 색상")]
        public virtual Color Color { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [JsonIgnore]
        [Browsable(false)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Bound")]
        [System.ComponentModel.Description("외각 영역")]
        public virtual BoundRect BoundRect { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Selected")]
        [System.ComponentModel.Description("선택여부")]
        public virtual bool IsSelected { get; set; }

        [Browsable(false)]
        public virtual bool IsHighlighted { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Markerable")]
        [System.ComponentModel.Description("레이저 가공 여부")]
        public virtual bool IsMarkerable
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
        [DisplayName("Locked")]
        [System.ComponentModel.Description("편집 금지 여부")]
        public virtual bool IsLocked
        {
            get => this.isLocked;
            set => this.isLocked = value;
        }

        [JsonIgnore]
        [Browsable(false)]
        public virtual TreeNode Node { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public virtual int Index { get; set; }

        [JsonIgnore]
        [Browsable(false)]
        public virtual object Tag { get; set; }

        [Browsable(false)]
        [ReadOnly(true)]
        public virtual uint Repeat { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Laser")]
        [DisplayName("Power")]
        [System.ComponentModel.Description("레이저 가공시의 설정 파워 (Watt)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float Power
        {
            get => this.power;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.power = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Laser")]
        [DisplayName("Frequency")]
        [System.ComponentModel.Description("레이저 가공시의 설정 주파수 (Hz)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float Frequency
        {
            get => this.frequency;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.frequency = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Laser")]
        [DisplayName("Pulse width")]
        [System.ComponentModel.Description("레이저 가공시 주파수의 펄스 폭 (usec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float PulseWidth
        {
            get => this.pulseWidth;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.pulseWidth = value;
                this.dutyCycle = string.Format("{0:F3} %", (object)(float)((double)this.pulseWidth / (1.0 / (double)this.frequency * 1000000.0) * 100.0));
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Laser")]
        [DisplayName("Duty cycle")]
        [System.ComponentModel.Description("펄스폭 비율 (0~100%)")]
        public virtual string DutyCycle
        {
            get => string.Format("{0:F3} %", (object)(float)((double)this.pulseWidth / (1.0 / (double)this.frequency * 1000000.0) * 100.0));
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.pulseWidth = (float)(1.0 / (double)this.frequency * 1000000.0 * (double)float.Parse(value.Trim().Split('%')[0]) / 100.0);
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Laser")]
        [DisplayName("On delay")]
        [System.ComponentModel.Description("레이저 가공시 레이저 시작 펄스의 지연시간 (usec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float LaserOnDelay
        {
            get => this.laserOnDelay;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.laserOnDelay = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Laser")]
        [DisplayName("Off delay")]
        [System.ComponentModel.Description("레이저 가공시 레이저 끝 펄스의 지연시간 (usec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float LaserOffDelay
        {
            get => this.laserOffDelay;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.laserOffDelay = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Scanner")]
        [DisplayName("Jump delay")]
        [System.ComponentModel.Description("스캐너 점프 이동후 안정화 시간 (usec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float ScannerJumpDelay
        {
            get => this.scannerJumpDelay;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.scannerJumpDelay = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Scanner")]
        [DisplayName("Mark delay")]
        [System.ComponentModel.Description("스캐너 직선/호 이동후 안정화 시간 (usec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float ScannerMarkDelay
        {
            get => this.scannerMarkDelay;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.scannerMarkDelay = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Scanner")]
        [DisplayName("Polygon delay")]
        [System.ComponentModel.Description("스캐너 폴리곤 혹은 코너(Corner) 이동 사이간의 지연 시간 (usec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float ScannerPolygonDelay
        {
            get => this.scannerPolygonDelay;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.scannerPolygonDelay = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Scanner")]
        [DisplayName("Jump speed")]
        [System.ComponentModel.Description("스캐너 점프 속도 (mm/sec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float JumpSpeed
        {
            get => this.jumpSpeed;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.jumpSpeed = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Scanner")]
        [DisplayName("Mark speed")]
        [System.ComponentModel.Description("스캐너 직선/호 가공 속도 (mm/sec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float MarkSpeed
        {
            get => this.markSpeed;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.markSpeed = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Sky Writing")]
        [DisplayName("Laser on shift")]
        [System.ComponentModel.Description("usec")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float LaserOnShift
        {
            get => this.laserOnShift;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.laserOnShift = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Sky Writing")]
        [DisplayName("Time lag")]
        [System.ComponentModel.Description("usec")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float TimeLag
        {
            get => this.timeLag;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.timeLag = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Sky Writing")]
        [DisplayName("Angular limit")]
        [System.ComponentModel.Description("단위 : 도(degree). 예: 90도")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float AngularLimit
        {
            get => this.angularLimit;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.angularLimit = value;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(false)]
        [ReadOnly(false)]
        [Category("Wobbel")]
        [DisplayName("Enable")]
        [System.ComponentModel.Description("사용 유무")]
        public virtual bool IsWobbelEnabled { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(false)]
        [ReadOnly(false)]
        [Category("Wobbel")]
        [DisplayName("Frequency")]
        [System.ComponentModel.Description("초당 반복 회수 (Hz)")]
        public virtual uint WobbelFrequency { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(false)]
        [ReadOnly(false)]
        [Category("Wobbel")]
        [DisplayName("Width")]
        [System.ComponentModel.Description("Parallel Movement / Amplitude X / Longitudinal (mm)")]
        public virtual float WobbelWidth { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(false)]
        [ReadOnly(false)]
        [Category("Wobbel")]
        [DisplayName("Height")]
        [System.ComponentModel.Description("Perpendicular Movement / Amplitude Y / Transversal (mm)")]
        public virtual float WobbelHeight { get; set; }

        public override string ToString() => "Pen: " + this.Name;

        public PenDefault()
        {
            this.Node = new TreeNode();
            this.Name = "Default";
            this.IsSelected = false;
            this.isMarkerable = true;
            this.isLocked = false;
            this.BoundRect = BoundRect.Empty;
            this.frequency = 50000f;
            this.pulseWidth = 2f;
            this.power = 5f;
            this.jumpSpeed = 500f;
            this.markSpeed = 500f;
            this.Color = Config.PensColor[0];
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public virtual object Clone() => (object)new PenDefault()
        {
            Name = this.Name,
            Description = this.Description,
            Color = this.Color,
            Owner = this.Owner,
            IsSelected = this.IsSelected,
            isMarkerable = this.IsMarkerable,
            isLocked = this.IsLocked,
            power = this.Power,
            frequency = this.Frequency,
            pulseWidth = this.PulseWidth,
            laserOnDelay = this.LaserOnDelay,
            laserOffDelay = this.LaserOffDelay,
            scannerJumpDelay = this.ScannerJumpDelay,
            scannerMarkDelay = this.ScannerMarkDelay,
            scannerPolygonDelay = this.ScannerPolygonDelay,
            jumpSpeed = this.JumpSpeed,
            markSpeed = this.MarkSpeed,
            laserOnShift = this.LaserOnShift,
            timeLag = this.TimeLag,
            angularLimit = this.AngularLimit,
            IsWobbelEnabled = this.IsWobbelEnabled,
            WobbelFrequency = this.WobbelFrequency,
            WobbelWidth = this.WobbelWidth,
            WobbelHeight = this.WobbelHeight,
            Tag = this.Tag,
            Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            }
        };

        /// <summary>동일성 검사</summary>
        /// <param name="other">Another color to compare to.</param>
        /// <returns>True if the three components are equal or false in any other case.</returns>
        public virtual bool Equals(PenDefault other) => other != null && (double)other.Power == (double)this.Power && ((double)other.Frequency == (double)this.Frequency && (double)other.PulseWidth == (double)this.PulseWidth) && ((double)other.LaserOnDelay == (double)this.LaserOnDelay && (double)other.LaserOffDelay == (double)this.LaserOffDelay && ((double)other.ScannerJumpDelay == (double)this.ScannerJumpDelay && (double)other.ScannerMarkDelay == (double)this.ScannerMarkDelay)) && ((double)other.ScannerPolygonDelay == (double)this.ScannerPolygonDelay && (double)other.JumpSpeed == (double)this.JumpSpeed && ((double)other.MarkSpeed == (double)this.MarkSpeed && (double)other.LaserOnShift == (double)this.LaserOnShift) && ((double)other.TimeLag == (double)this.TimeLag && (double)other.AngularLimit == (double)this.AngularLimit && (other.IsWobbelEnabled == this.IsWobbelEnabled && (int)other.WobbelFrequency == (int)this.WobbelFrequency))) && (double)other.WobbelHeight == (double)this.WobbelHeight && (double)other.WobbelWidth == (double)this.WobbelWidth;

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public virtual bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            IRtc rtc = markerArg.Rtc;
            ILaser laser = markerArg.Laser;
            bool flag1 = true;
            if (laser != null)
                flag1 &= laser.ListPower(this.Power);
            int num1 = rtc.Is3D ? 1 : 0;
            int num2 = rtc.Is2ndHead ? 1 : 0;
            int num3 = rtc.IsMOTF ? 1 : 0;
            int num4 = rtc.IsScanAhead ? 1 : 0;
            bool flag2 = flag1 & rtc.ListDelay(this.LaserOnDelay, this.LaserOffDelay, this.ScannerJumpDelay, this.ScannerMarkDelay, this.ScannerPolygonDelay) & rtc.ListSpeed(this.JumpSpeed, this.MarkSpeed) & rtc.ListFrequency(this.Frequency, this.PulseWidth);
            if (rtc is IRtcExtension rtcExtension)
            {
                bool flag3 = flag2 & rtcExtension.ListSkyWriting(this.LaserOnShift, this.TimeLag, this.AngularLimit);
                flag2 = !this.IsWobbelEnabled ? flag3 & rtcExtension.ListWobbel(0.0f, 0.0f, 0.0f) : flag3 & rtcExtension.ListWobbel(this.WobbelWidth, this.WobbelHeight, (float)this.WobbelFrequency);
            }
            if (flag2)
                markerArg.PenStack.Push((IPen)this);
            return flag2;
        }

        public virtual void Regen()
        {
        }
    }
}
