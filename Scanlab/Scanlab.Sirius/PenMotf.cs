
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Pen Motf 엔티티 객체 (기본 버전에서 상속된 파생 펜)
    /// entity 에 motf begin/end/wait 등이 추가되어 불필요해짐 !!!
    /// deprecated version
    /// </summary>
    [JsonObject]
    [Obsolete("PenMotf 클래스는 이제 더이상 지원되지 않습니다. MotfBegin/End 등의 엔티티로 교체하세요.")]
    public class PenMotf : PenDefault, IEquatable<PenMotf>
    {
        [JsonIgnore]
        [Browsable(false)]
        public override EType EntityType => EType.PenMotf;

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("MOTF")]
        [DisplayName("Encoder")]
        [Description("엔코더 신호 종류")]
        public virtual RtcEncoder Signal { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("MOTF")]
        [DisplayName("Condition")]
        [Description("대기 조건 (엔코더 누적 위치의 대기 위치")]
        public virtual EncoderWaitCondition Wait { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("MOTF")]
        [DisplayName("Position")]
        [Description("대기할 위치 (mm)")]
        public virtual float Position { get; set; }

        public override string ToString() => "Pen: " + this.Name;

        public PenMotf()
        {
            this.Name = "Motf";
            this.Signal = RtcEncoder.EncX;
            this.Wait = EncoderWaitCondition.Auto;
            this.Position = 10f;
        }

        /// <summary>복사본 생성</summary>
        /// <returns>A new color that is a copy of this instance.</returns>
        public override object Clone()
        {
            PenMotf penMotf = new PenMotf();
            penMotf.Name = this.Name;
            penMotf.Description = this.Description;
            penMotf.Color = this.Color;
            penMotf.Owner = this.Owner;
            penMotf.IsSelected = this.IsSelected;
            penMotf.isMarkerable = this.IsMarkerable;
            penMotf.isLocked = this.IsLocked;
            penMotf.power = this.Power;
            penMotf.frequency = this.Frequency;
            penMotf.pulseWidth = this.PulseWidth;
            penMotf.laserOnDelay = this.LaserOnDelay;
            penMotf.laserOffDelay = this.LaserOffDelay;
            penMotf.scannerJumpDelay = this.ScannerJumpDelay;
            penMotf.scannerMarkDelay = this.ScannerMarkDelay;
            penMotf.scannerPolygonDelay = this.ScannerPolygonDelay;
            penMotf.jumpSpeed = this.JumpSpeed;
            penMotf.markSpeed = this.MarkSpeed;
            penMotf.laserOnShift = this.LaserOnShift;
            penMotf.timeLag = this.TimeLag;
            penMotf.angularLimit = this.AngularLimit;
            penMotf.IsWobbelEnabled = this.IsWobbelEnabled;
            penMotf.WobbelFrequency = this.WobbelFrequency;
            penMotf.WobbelWidth = this.WobbelWidth;
            penMotf.WobbelHeight = this.WobbelHeight;
            penMotf.Signal = this.Signal;
            penMotf.Wait = this.Wait;
            penMotf.Position = this.Position;
            penMotf.Tag = this.Tag;
            penMotf.Node = new TreeNode()
            {
                Text = this.Node.Text,
                Tag = this.Node.Tag
            };
            return (object)penMotf;
        }

        /// <summary>동일성 검사</summary>
        /// <param name="other">Another color to compare to.</param>
        /// <returns>True if the three components are equal or false in any other case.</returns>
        public virtual bool Equals(PenMotf other) => other != null && (double)other.Power == (double)this.Power && ((double)other.Frequency == (double)this.Frequency && (double)other.PulseWidth == (double)this.PulseWidth) && ((double)other.LaserOnDelay == (double)this.LaserOnDelay && (double)other.LaserOffDelay == (double)this.LaserOffDelay && ((double)other.ScannerJumpDelay == (double)this.ScannerJumpDelay && (double)other.ScannerMarkDelay == (double)this.ScannerMarkDelay)) && ((double)other.ScannerPolygonDelay == (double)this.ScannerPolygonDelay && (double)other.JumpSpeed == (double)this.JumpSpeed && ((double)other.MarkSpeed == (double)this.MarkSpeed && (double)other.LaserOnShift == (double)this.LaserOnShift) && ((double)other.TimeLag == (double)this.TimeLag && (double)other.AngularLimit == (double)this.AngularLimit && (other.IsWobbelEnabled == this.IsWobbelEnabled && (int)other.WobbelFrequency == (int)this.WobbelFrequency))) && ((double)other.WobbelHeight == (double)this.WobbelHeight && (double)other.WobbelWidth == (double)this.WobbelWidth && (other.Signal == this.Signal && other.Wait == this.Wait)) && (double)other.Position == (double)this.Position;

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public override bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            bool flag1 = true;
            IRtc rtc = markerArg.Rtc;
            ILaser laser = markerArg.Laser;
            bool flag2 = flag1 & base.Mark(markerArg);
            IRtcMOTF rtcMotf = rtc as IRtcMOTF;
            if (flag2 && rtcMotf != null)
                flag2 &= rtcMotf.ListMOTFWait(this.Signal, this.Position, this.Wait);
            return flag2;
        }
    }
}
