
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Scanlab.Sirius
{
    /// <summary>레이어의 집합 객체 (레이어의 리스트 컨테이너로 동작)</summary>
    [JsonObject]
    public class Layers : ObservableList<Layer>, ICloneable
    {
        private IDocument owner;
        private Layer active;
        private BoundRect boundRect;

        /// <summary>레이어들의 배열</summary>
        [JsonProperty]
        public Layer[] Items
        {
            get => this.ToArray();
            set
            {
                if (value == null)
                    return;
                lock (this.SyncRoot)
                {
                    this.Clear();
                    this.AddRange((IEnumerable<Layer>)value);
                }
            }
        }

        /// <summary>activated layer</summary>
        [JsonIgnore]
        public Layer Active
        {
            get => this.active;
            set
            {
                if (this.active != null)
                {
                    if (this.active.Node.NodeFont == null)
                        this.active.Node.NodeFont = new Font("Arial", 9f);
                    this.active.Node.NodeFont = new Font("Arial", 9f, this.active.Node.NodeFont.Style & ~FontStyle.Bold);
                }
                this.active = value;
                if (this.active.Node.NodeFont == null)
                    this.active.Node.NodeFont = new Font("Arial", 9f);
                this.active.Node.NodeFont = new Font("Arial", 9f, this.active.Node.NodeFont.Style | FontStyle.Bold);
            }
        }

        /// <summary>부모 문서</summary>
        [JsonIgnore]
        public IDocument Owner
        {
            get => this.owner;
            internal set => this.owner = value;
        }

        /// <summary>레이어 내의 최대 외곽 영역</summary>
        [JsonIgnore]
        public BoundRect BoundRect
        {
            set => this.boundRect = value;
            get
            {
                BoundRect boundRect = this.boundRect.Clone();
                foreach (Layer layer in (ObservableList<Layer>)this)
                {
                    if (layer.IsVisible)
                        boundRect.Union(layer.BoundRect);
                }
                return boundRect;
            }
        }

        /// <summary>생성자</summary>
        public Layers()
        {
        }

        /// <summary>생성자</summary>
        /// <param name="owner"></param>
        public Layers(IDocument owner)
          : this()
        {
            this.owner = owner;
            this.boundRect = new BoundRect();
        }

        /// <summary>레이어 이름으로 레이어 검색</summary>
        /// <param name="layerName">레이어 이름</param>
        /// <param name="ignoreCase">대소문자 구분여부</param>
        /// <returns></returns>
        public Layer NameOf(string layerName, bool ignoreCase = true)
        {
            lock (this.SyncRoot)
            {
                foreach (Layer layer in (ObservableList<Layer>)this)
                {
                    if (string.Compare(layer.Name, layerName, ignoreCase) == 0)
                        return layer;
                }
            }
            return (Layer)null;
        }

        /// <summary>엔티티 이름으로 모든 레이어 검색</summary>
        /// <param name="entityName">엔티티 이름</param>
        /// <param name="parentLayer">발견된 엔티티가 포함된 레이어</param>
        /// <param name="ignoreCase">대소문자 구분여부</param>
        /// <returns></returns>
        public IEntity NameOf(string entityName, out Layer parentLayer, bool ignoreCase = true)
        {
            parentLayer = (Layer)null;
            lock (this.SyncRoot)
            {
                foreach (Layer layer in (ObservableList<Layer>)this)
                {
                    foreach (IEntity entity in (ObservableList<IEntity>)layer)
                    {
                        if (string.Compare(entity.Name, entityName, ignoreCase) == 0)
                        {
                            parentLayer = layer;
                            return entity;
                        }
                    }
                }
            }
            return (IEntity)null;
        }

        /// <summary>레이어들을 모두 복제</summary>
        /// <returns></returns>
        public object Clone()
        {
            Layers layers = new Layers();
            lock (this.SyncRoot)
            {
                foreach (Layer layer in this.list)
                    layers.Add((Layer)layer.Clone());
                if (this.active != null)
                    layers.active = layers.NameOf(this.active.Name);
            }
            return (object)layers;
        }
    }
}
