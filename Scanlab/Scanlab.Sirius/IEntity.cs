// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.IEntity
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 엔티티 인터페이스
    /// 
    ///                          IEntity
    ///                             |
    ///   IDrawable  _____________  |
    ///                          |  |
    ///   IExplodable _________  |  |
    ///                       |  |  |
    ///   IHatchable  ______  |  |  |
    ///                    |  |  |  |
    ///   IMarkerable ___  |  |  |  |
    ///                 |  |  |  |  |
    ///                 |  |  |  |  |
    ///                [ Your Entitiy ]
    /// 
    /// </summary>
    public interface IEntity
    {
        /// <summary>부모 객체</summary>
        IEntity Owner { get; set; }

        /// <summary>엔티티의 타입</summary>
        EType EntityType { get; }

        /// <summary>
        /// 엔티티 이름
        /// entity name
        /// </summary>
        string Name { get; }

        /// <summary>엔티티 설명</summary>
        string Description { get; set; }

        /// <summary>객체의 외각 사각형 영역</summary>
        BoundRect BoundRect { get; }

        /// <summary>
        /// 엔티티 선택 여부
        /// entity is selected or not
        /// </summary>
        bool IsSelected { get; set; }

        /// <summary>
        /// 편집 가능 여부
        /// locked or not
        /// </summary>
        bool IsLocked { get; set; }

        /// <summary>
        /// 트리뷰 노드
        /// 모든 엔티티는 TreeView 에 출력이 가능해야 하기에 Node 를 하나씩 가진다
        /// </summary>
        TreeNode Node { get; set; }

        /// <summary>트리뷰 노드 인덱스 번호 = list 에서의 인덱스 번호 (내부적으로 사용됨)</summary>
        int Index { get; set; }

        /// <summary>사용자 데이타</summary>
        object Tag { get; set; }

        /// <summary>
        /// 데이타 변경시 혹은 내부 벡터및 외각(boundrect)영역 재 계산이 필요할 경우 등
        /// 내부데이타 refresh 발생시 호출되어야 하는 함수 (regenerate)
        /// </summary>
        void Regen();
    }
}
