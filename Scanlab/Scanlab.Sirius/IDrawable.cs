// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.IDrawable
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using System;
using System.Numerics;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 화면에 출력(렌더)이 가능한 엔티티(Entity)에 대한 인터페이스
    /// 주요기능 : 그리기(렌더링) 및 변환(이동, 회전등), 선택
    /// </summary>
    public interface IDrawable
    {
        /// <summary>마우스 over 등에 의한 시각 효과 (현재 미지원)</summary>
        bool IsHighlighted { get; set; }

        /// <summary>가공 경로를 나타내는 사각정보 표시여부 (엔티티 선택시 보여짐)</summary>
        bool IsDrawPath { get; set; }

        /// <summary>
        /// 화면에 표시 여부
        /// visible or not
        /// </summary>
        bool IsVisible { get; set; }

        /// <summary>
        /// Deprecated ! Use Color2 instead
        /// 엔티티 색상
        /// entity ACI(Autocad Color Index) color
        /// </summary>
        [Obsolete("AciColor 클래스는 이제 더이상 지원되지 않습니다. Color2로 교체하세요.")]
        AciColor Color { get; set; }

        /// <summary>엔티티 색상</summary>
        System.Drawing.Color Color2 { get; set; }

        /// <summary>회전량</summary>
        float Angle { get; set; }

        /// <summary>엔티티 렌더링 (by OpenGL)</summary>
        /// <param name="view">대상 뷰</param>
        bool Draw(IView view);

        /// <summary>엔티티 이동</summary>
        /// <param name="delta">이동량 벡터</param>
        void Transit(Vector2 delta);

        /// <summary>자신의 기준점을 중심으로 회전</summary>
        /// <param name="angle">회전 각도</param>
        void Rotate(float angle);

        /// <summary>지점된 회전 중심을 기준으로 회전</summary>
        /// <param name="angle">회전 각도</param>
        /// <param name="rotateCenter">회전 중심 벡터</param>
        void Rotate(float angle, Vector2 rotateCenter);

        /// <summary>
        /// 원점을 중심으로 크기 변환
        /// (내부적으로 사용되므로, 외부사용자는 호출 금지)
        /// </summary>
        /// <param name="scale">스케일 (1.0)</param>
        void Scale(Vector2 scale);

        /// <summary>지정된 위치를 중심으로 크기변환</summary>
        /// <param name="scale">스케일 (1.0)</param>
        /// <param name="scaleCenter">스케일 변환 벡터</param>
        void Scale(Vector2 scale, Vector2 scaleCenter);

        /// <summary>엔티티 선택 (점으로)</summary>
        /// <param name="x">x 위치 (mm)</param>
        /// <param name="y">y 위치 (mm)</param>
        /// <param name="threshold">문턱값 (mm)</param>
        /// <returns></returns>
        bool HitTest(float x, float y, float threshold = 0.02f);

        /// <summary>엔티티 선택 (사각 영역으로)</summary>
        /// <param name="left">좌 (mm)</param>
        /// <param name="top">상 (mm)</param>
        /// <param name="right">우 (mm)</param>
        /// <param name="bottom">하 (mm)</param>
        /// <param name="threshold">문턱값 (mm)</param>
        /// <returns></returns>
        bool HitTest(float left, float top, float right, float bottom, float threshold = 0.02f);

        /// <summary>엔티티 선택 (사각 영역으로)</summary>
        /// <param name="rect">사각 영역</param>
        /// <param name="threshold">문턱값 (mm)</param>
        /// <returns></returns>
        bool HitTest(BoundRect rect, float threshold = 0.02f);
    }
}
