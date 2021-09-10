
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 블럭 객체
    /// Autocad 의 블럭 기능
    /// </summary>
    [JsonObject]
    public class Block : List<IEntity>, ICloneable
    {
        private string name;

        [JsonIgnore]
        [Browsable(false)]
        public IEntity Owner { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Name")]
        [System.ComponentModel.Description("블럭의 이름")]
        public string Name
        {
            get => this.name;
            set => this.name = value;
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Description")]
        [System.ComponentModel.Description("블럭에 대한 설명")]
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
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Bound")]
        [System.ComponentModel.Description("외각 영역")]
        public BoundRect BoundRect { get; set; }

        [JsonIgnore]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Count")]
        [System.ComponentModel.Description("개수")]
        public new int Count => base.Count;

        [JsonProperty]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Items")]
        [System.ComponentModel.Description("블럭을 구성하는 엔티티 항목(들)")]
        public IEntity[] Items
        {
            get => this.ToArray();
            set
            {
                if (value == null)
                    return;
                this.Clear();
                this.AddRange((IEnumerable<IEntity>)value);
            }
        }

        public override string ToString() => string.Format("Block: {0} ({1})", (object)this.name, (object)this.Count);

        /// <summary>생성자</summary>
        public Block()
        {
            this.Name = nameof(Block);
            this.BoundRect = BoundRect.Empty;
        }

        /// <summary>생성자</summary>
        /// <param name="entities"></param>
        public Block(List<IEntity> entities)
          : this()
        {
            this.AddRange((IEnumerable<IEntity>)entities);
        }

        public object Clone()
        {
            List<IEntity> entities = new List<IEntity>(this.Count);
            foreach (IEntity entity1 in (List<IEntity>)this)
            {
                IEntity entity2 = entity1 is ICloneable cloneable2 ? (IEntity)cloneable2.Clone() : (IEntity)(object)null;
                entities.Add(entity2);
            }
            return (object)new Block(entities)
            {
                Name = this.Name,
                Description = this.Description,
                Owner = this.Owner,
                Color2 = this.Color2,
                BoundRect = this.BoundRect.Clone()
            };
        }

        public List<IEntity> Explode()
        {
            List<IEntity> entityList = new List<IEntity>();
            foreach (IEntity entity in (List<IEntity>)this)
            {
                if (entity is ICloneable cloneable1)
                    entityList.Add((IEntity)cloneable1.Clone());
            }
            return entityList;
        }

        private void RegenVertextList()
        {
            foreach (IEntity entity in (List<IEntity>)this)
                entity.Regen();
        }

        private void RegenBoundRect()
        {
            this.BoundRect.Clear();
            foreach (IEntity entity in (List<IEntity>)this)
                this.BoundRect.Union(entity.BoundRect);
        }

        public void Regen()
        {
            this.RegenVertextList();
            this.RegenBoundRect();
        }
    }
}
