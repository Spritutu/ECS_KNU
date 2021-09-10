// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.Pens
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Pen 인터페이스 (레이저 가공 파라메터를 가지는 펜 인터페이스)
    /// 장비별로 프로젝트 별로 레이저 가공 기법이 다르므로, 펜(IPen)을 상속받아 Pen 객체를 구현
    /// </summary>
    [JsonObject]
    public class Pens : ObservableList<IPen>, IPens
    {
        [JsonIgnore]
        [Browsable(false)]
        public virtual IDocument Owner { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Name")]
        [Description("이름")]
        public virtual string Name { get; set; }

        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Basic")]
        [DisplayName("Filename")]
        [Description("현재 작업중인 파일 이름")]
        public virtual string FileName { get; set; }

        [JsonIgnore]
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Count")]
        [Description("개수")]
        public new virtual int Count => base.Count;

        [RefreshProperties(RefreshProperties.All)]
        [JsonProperty]
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Basic")]
        [DisplayName("Items")]
        [Description("펜 항목(들)")]
        public virtual IPen[] Items
        {
            get => this.ToArray();
            set
            {
                if (value == null)
                    return;
                this.Clear();
                this.AddRange((IEnumerable<IPen>)value);
            }
        }

        [JsonIgnore]
        [Browsable(false)]
        public virtual object Tag { get; set; }

        /// <summary>펜 이름으로 검색 (이름이 일치해야 검색 성공)</summary>
        /// <param name="penName">검색할 펜 엔티티 이름</param>
        /// <param name="ignoreCase">대소문자 구분여부</param>
        /// <returns></returns>
        public virtual IPen NameOf(string penName, bool ignoreCase = false)
        {
            lock (this.SyncRoot)
            {
                foreach (IPen pen in (ObservableList<IPen>)this)
                {
                    if (string.Compare(pen.Name, penName, ignoreCase) == 0)
                        return pen;
                }
            }
            return (IPen)null;
        }

        public virtual IPen ColorOf(Color color)
        {
            lock (this.SyncRoot)
            {
                foreach (IPen pen in (ObservableList<IPen>)this)
                {
                    if (pen.Color == color)
                        return pen;
                }
            }
            return (IPen)null;
        }

        public static Pens CreateNew()
        {
            Pens pens = new Pens();
            for (int index = 0; index < 10; ++index)
            {
                PenDefault penDefault = new PenDefault()
                {
                    Name = string.Format("Pen{0}", (object)(index + 1)),
                    Color = Config.PensColor[index],
                    Description = Config.PensColor[index].ToString()
                };
            }
            return pens;
        }
    }
}
