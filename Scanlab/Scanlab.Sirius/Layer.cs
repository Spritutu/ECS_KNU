
using Newtonsoft.Json;
using SharpGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Numerics;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>레이어 객체 (엔티티의 리스트 컨테이너로 동작)</summary>
    [JsonObject]
    public class Layer : ObservableList<IEntity>, IEntity, IMarkerable, ICloneable, IDrawable
    {
        protected string name;
        protected BoundRect boundRect;
        protected bool isVisible;
        protected bool isMarkerable;
        protected bool isLocked;
        protected MotionType motionType;
        protected float zPosition;
        protected string alcPositionFileName;
        protected float angle;
        protected bool isRegen;

        /// <summary>부모 객체</summary>
        [JsonIgnore]
        [Browsable(false)]
        public virtual IEntity Owner { get; set; }

        /// <summary>레이어의 타입</summary>
        [JsonIgnore]
        [Browsable(false)]
        public virtual EType EntityType => EType.Layer;

        /// <summary>레이어의 이름</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Name")]
        [System.ComponentModel.Description("레이어의 이름")]
        public virtual string Name
        {
            get => this.name;
            set
            {
                if (value == null)
                    return;
                this.name = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        /// <summary>레이어 설명</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Description")]
        [System.ComponentModel.Description("레이어에 대한 설명")]
        public virtual string Description { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Extension Data")]
        [System.ComponentModel.Description("레이어의 사용자 부가 데이타")]
        [Editor(typeof(MultilineStringEditor), typeof(UITypeEditor))]
        public string ExtensionData { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public AciColor Color { get; set; }

        [Browsable(false)]
        [JsonIgnore]
        public System.Drawing.Color Color2 { get; set; }

        /// <summary>레이어의 외각 영역 (모든 엔티티를 합친 외곽 영역)</summary>
        [RefreshProperties(RefreshProperties.All)]
        [JsonIgnore]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Bound")]
        [System.ComponentModel.Description("외각 영역")]
        public virtual BoundRect BoundRect
        {
            get
            {
                this.RegenBoundRect();
                return this.boundRect;
            }
        }

        [Browsable(false)]
        public bool IsHighlighted { get; set; }

        /// <summary>레이어에 포함된 엔티티의 개수</summary>
        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Count")]
        [System.ComponentModel.Description("개수")]
        public new virtual int Count => base.Count;

        /// <summary>레이어에 포함된 엔티티의 배열 목록</summary>
        [RefreshProperties(RefreshProperties.All)]
        [JsonProperty]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Items")]
        [System.ComponentModel.Description("항목(들)")]
        public virtual IEntity[] Items
        {
            get => this.ToArray();
            set
            {
                if (value == null)
                    return;
                this.Clear();
                this.AddRange((IEnumerable<IEntity>)value);
                this.isRegen = true;
            }
        }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Selected")]
        [System.ComponentModel.Description("선택여부")]
        public virtual bool IsSelected { get; set; }

        /// <summary>레이어 가시성 여부</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Status")]
        [DisplayName("Visible")]
        [System.ComponentModel.Description("스크린에 출력 여부")]
        public virtual bool IsVisible
        {
            get => this.isVisible;
            set => this.isVisible = value;
        }

        /// <summary>레이어 마킹(가공) 여부</summary>
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
        [DisplayName("Mark Path")]
        [System.ComponentModel.Description("가공 경로를 표시")]
        public virtual bool IsDrawPath { get; set; }

        /// <summary>레이어 편집 금지 여부</summary>
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

        /// <summary>
        /// 레이어 별로 가공 방식을 변경 가능
        /// 해당 타입에 따라 사용자의 마커(IMarker)에서 세부 구현 가능
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Motion Type")]
        [System.ComponentModel.Description("모션 타입")]
        public virtual MotionType MotionType
        {
            get => this.motionType;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.motionType = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        /// <summary>레이어의 반복 가공회수</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Data")]
        [DisplayName("Repeat")]
        [System.ComponentModel.Description("가공 반복 횟수")]
        public virtual uint Repeat { get; set; }

        /// <summary>레이어 별 Z 축 (초점 거리) 위치값(mm) 설정</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Motor")]
        [DisplayName("Z Position")]
        [System.ComponentModel.Description("Z 축 가공 위치 데이타")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float ZPosition
        {
            get => this.zPosition;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.zPosition = value;
                this.Node.Text = this.ToString() ?? "";
            }
        }

        /// <summary>위치 의존적 레이저 출력 보상용 테이블 파일</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Optimized Laser Control")]
        [DisplayName("Enabled")]
        [System.ComponentModel.Description("활성화 여부")]
        public virtual bool IsALC { get; set; }

        /// <summary>위치 의존적 레이저 출력 보상용 테이블 파일</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Optimized Laser Control")]
        [DisplayName("Position Dependent File Name")]
        [System.ComponentModel.Description("위치 의존적 레이저 출력 보상용 테이블 파일")]
        [Editor(typeof(AlcPositionFileBrowser), typeof(UITypeEditor))]
        public virtual string AlcPositionFileName
        {
            get => this.alcPositionFileName;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                this.alcPositionFileName = value;
            }
        }

        /// <summary>위치 의존적 레이저 출력 보상용 테이블 파일내의 테이블 인덱스 번호</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Optimized Laser Control")]
        [DisplayName("Table No")]
        [System.ComponentModel.Description("보정 파일내 테이블 인덱스 번호")]
        public virtual uint AlcPositionTableNo { get; set; }

        /// <summary>레이저 제어 신호 자동 보정 신호 종류</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Optimized Laser Control")]
        [DisplayName("Control Signal")]
        [System.ComponentModel.Description("레이저 제어 신호 자동 보정 신호 종류")]
        public virtual AutoLaserControlSignal AlcSignal { get; set; }

        /// <summary>레이저 제어 신호 자동 보정 신호 종류</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Optimized Laser Control")]
        [DisplayName("Control Signal")]
        [System.ComponentModel.Description("레이저 제어 신호 자동 보정 신호 종류")]
        public virtual AutoLaserControlMode AlcMode { get; set; }

        /// <summary>ALC 사용시 100% 출력값</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Optimized Laser Control")]
        [DisplayName("100% Signal Value")]
        [System.ComponentModel.Description("100% 시 출력값 (Analog: ~10, DO8: ~255, DO16: ~65535, Freq: ~Hz , PW: ~usec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float AlcPercentage100 { get; set; }

        /// <summary>ALC 사용시 최소 출력값 (Cut off)</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Optimized Laser Control")]
        [DisplayName("Min Signal Value")]
        [System.ComponentModel.Description("최소 신호 출력값 (Analog: ~10, DO8: ~255, DO16: ~65535, Freq: ~Hz , PW: ~usec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float AlcMinValue { get; set; }

        /// <summary>ALC 사용시 최대 출력값 (Cut off)</summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Optimized Laser Control")]
        [DisplayName("Max Signal Value")]
        [System.ComponentModel.Description("최대 신호 출력값 (Analog: ~10, DO8: ~255, DO16: ~65535, Freq: ~Hz , PW: ~usec)")]
        [TypeConverter(typeof(FloatTypeConverter))]
        public virtual float AlcMaxValue { get; set; }

        [Browsable(false)]
        public virtual float Angle
        {
            get => this.angle;
            set
            {
                if (this.Owner != null && this.isLocked)
                    return;
                float angle = value - this.angle;
                if (this.Owner == null)
                    return;
                this.Rotate(angle);
            }
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

        public override string ToString() => "Layer: " + this.name;

        /// <summary>생성자</summary>
        public Layer()
        {
            this.Node = new TreeNode();
            this.Owner = (IEntity)null;
            this.isVisible = true;
            this.isMarkerable = true;
            this.boundRect = BoundRect.Empty;
            this.MotionType = MotionType.ScannerOnly;
            this.isRegen = false;
            this.Repeat = 1U;
            this.IsALC = false;
            this.alcPositionFileName = string.Empty;
            this.AlcPositionTableNo = 0U;
            this.AlcSignal = AutoLaserControlSignal.Analog2;
            this.AlcMode = AutoLaserControlMode.Disabled;
            this.AlcPercentage100 = 10f;
            this.AlcMinValue = 1f;
            this.AlcMaxValue = 10f;
        }

        /// <summary>생성자</summary>
        /// <param name="name"></param>
        public Layer(string name)
          : this()
        {
            this.Name = name;
        }

        /// <summary>엔티티 이름으로 검색 (이름이 일치해야 검색 성공)</summary>
        /// <param name="entityName">검색할 엔티티 이름</param>
        /// <param name="ignoreCase">대소문자 구분여부</param>
        /// <returns></returns>
        public virtual IEntity NameOf(string entityName, bool ignoreCase = false)
        {
            lock (this.SyncRoot)
            {
                foreach (IEntity entity in (ObservableList<IEntity>)this)
                {
                    if (string.Compare(entity.Name, entityName, ignoreCase) == 0)
                        return entity;
                }
            }
            return (IEntity)null;
        }

        /// <summary>엔티티 이름으로 검색 (이름의 일부만 있어도 검색 성공)</summary>
        /// <param name="entityName">검색할 엔티티 이름</param>
        /// <param name="ignoreCase">대소문자 구분여부</param>
        /// <returns></returns>
        public virtual List<IEntity> Contains(string entityName, bool ignoreCase = false)
        {
            List<IEntity> entityList = new List<IEntity>();
            lock (this.SyncRoot)
            {
                foreach (IEntity entity in (ObservableList<IEntity>)this)
                {
                    if (ignoreCase)
                    {
                        if (entity.Name.IndexOf(entityName, StringComparison.CurrentCultureIgnoreCase) >= 0)
                            entityList.Add(entity);
                    }
                    else if (entity.Name.Contains(entityName))
                        entityList.Add(entity);
                }
            }
            return entityList;
        }

        /// <summary>엔티티 타입으로 검색</summary>
        /// <param name="entityType">대상 엔티티 타입</param>
        /// <returns></returns>
        public virtual List<IEntity> Contains(EType entityType)
        {
            List<IEntity> entityList = new List<IEntity>();
            lock (this.SyncRoot)
            {
                foreach (IEntity entity in (ObservableList<IEntity>)this)
                {
                    if (entity.EntityType == entityType)
                        entityList.Add(entity);
                }
            }
            return entityList;
        }

        /// <summary>복사본 생성</summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            Layer layer = new Layer()
            {
                Name = this.Name,
                Description = this.Description,
                ExtensionData = this.ExtensionData,
                Owner = this.Owner,
                IsSelected = this.IsSelected,
                IsHighlighted = this.IsHighlighted,
                isVisible = this.IsVisible,
                IsDrawPath = this.IsDrawPath,
                isLocked = this.IsLocked,
                isMarkerable = this.IsMarkerable,
                boundRect = this.boundRect.Clone(),
                Repeat = this.Repeat,
                angle = this.angle,
                motionType = this.motionType,
                zPosition = this.zPosition,
                IsALC = this.IsALC,
                alcPositionFileName = this.alcPositionFileName,
                AlcPositionTableNo = this.AlcPositionTableNo,
                AlcSignal = this.AlcSignal,
                AlcMode = this.AlcMode,
                AlcPercentage100 = this.AlcPercentage100,
                AlcMinValue = this.AlcMinValue,
                AlcMaxValue = this.AlcMaxValue,
                Tag = this.Tag,
                Node = new TreeNode()
                {
                    Text = this.Node.Text,
                    Tag = this.Node.Tag
                }
            };
            lock (this.SyncRoot)
            {
                foreach (IEntity entity1 in (ObservableList<IEntity>)this)
                {
                    if (entity1 is ICloneable cloneable2)
                    {
                        IEntity entity2 = (IEntity)cloneable2.Clone();
                        layer.Add(entity2);
                    }
                }
            }
            return (object)layer;
        }

        /// <summary>laser processing</summary>
        /// <param name="markerArg"></param>
        /// <returns></returns>
        public virtual bool Mark(IMarkerArg markerArg)
        {
            if (!this.IsMarkerable)
                return true;
            bool flag = true;
            IEntity[] array = this.list.ToArray();
            for (int index = 0; (long)index < (long)this.Repeat; ++index)
            {
                foreach (IEntity entity in array)
                {
                    if (entity is IMarkerable markerable3)
                    {                     
                        flag &= markerable3.Mark(markerArg);
                        if (!flag)
                            break;
                    }
                }
                if (!flag)
                    break;
            }
            return flag;
        }

        protected virtual void RegenVertextList()
        {
            foreach (IEntity entity in this.list.ToArray())
                entity.Regen();
        }

        protected virtual void RegenBoundRect()
        {
            this.boundRect.Clear();
            foreach (IEntity entity in this.list.ToArray())
            {
                if (entity is IDrawable drawable1 && drawable1.IsVisible)
                    this.boundRect.Union(entity.BoundRect);
            }
        }

        public virtual void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
            this.isRegen = false;
        }

        public virtual bool Draw(IView view)
        {
            if (this.isRegen)
                this.Regen();
            if (!this.IsVisible)
                return true;
            OpenGL renderer = view.Renderer;
            if (!this.IsMarkerable)
            {
                renderer.Enable(2852U);
                renderer.LineStipple(1, (ushort)1);
            }
            foreach (IEntity entity in this.list.ToArray())
            {
                if (entity is IDrawable drawable1)
                {
                    if (!(entity is IMarkerable markerable3) || !markerable3.IsMarkerable)
                    {
                        renderer.Enable(2852U);
                        renderer.LineStipple(1, (ushort)1);
                        renderer.Disable(2852U);
                    }

                    markerable3 = entity as IMarkerable;
                    drawable1.IsDrawPath = this.IsDrawPath;
                    drawable1.Draw(view);
                    if (markerable3 == null || !markerable3.IsMarkerable)
                        renderer.Disable(2852U);
                }
            }
            if (!this.IsMarkerable)
                renderer.Disable(2852U);
            return true;
        }

        public virtual void Transit(Vector2 delta)
        {
            int num = this.IsLocked ? 1 : 0;
        }

        public virtual void Rotate(float angle)
        {
            int num = this.IsLocked ? 1 : 0;
        }

        public virtual void Rotate(float angle, Vector2 rotateCenter)
        {
            int num = this.IsLocked ? 1 : 0;
        }

        public virtual void Scale(Vector2 scale)
        {
            if (this.IsLocked || scale == Vector2.Zero)
                return;
            int num = scale == Vector2.One ? 1 : 0;
        }

        public virtual void Scale(Vector2 scale, Vector2 scaleCenter)
        {
            if (this.IsLocked || scale == Vector2.Zero)
                return;
            int num = scale == Vector2.One ? 1 : 0;
        }

        public virtual bool HitTest(float x, float y, float threshold)
        {
            if (!this.IsVisible || !this.boundRect.HitTest(x, y, threshold))
                return false;
            int num1 = -1;
            int num2 = 0;
            foreach (IEntity entity in this.list.ToArray())
            {
                if (entity is IDrawable drawable1)
                {
                    if (drawable1.HitTest(x, y, threshold))
                    {
                        num1 = num2;
                        break;
                    }
                    ++num2;
                }
            }
            return num1 >= 0;
        }

        public virtual bool HitTest(
          float left,
          float top,
          float right,
          float bottom,
          float threshold)
        {
            return this.IsVisible && this.HitTest(new BoundRect(left, top, right, bottom), threshold);
        }

        public virtual bool HitTest(BoundRect br, float threshold)
        {
            if (!this.IsVisible || !this.boundRect.HitTest(br, threshold))
                return false;
            int num1 = -1;
            int num2 = 0;
            foreach (IEntity entity in this.list.ToArray())
            {
                if (entity is IDrawable drawable1)
                {
                    if (drawable1.HitTest(br, threshold))
                    {
                        num1 = num2;
                        break;
                    }
                    ++num2;
                }
            }
            return num1 >= 0;
        }
    }
}
