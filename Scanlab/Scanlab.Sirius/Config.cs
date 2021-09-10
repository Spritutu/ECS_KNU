
using System.Drawing;
using System.Reflection;

namespace Scanlab.Sirius
{
    /// <summary>라이브러리 내부 전역 환경 변수들</summary>
    internal class Config
    {
        /// <summary>문서 배경 색상</summary>
        public static float[] DocumentClearColor = new float[4]
        {
      0.05f,
      0.05f,
      0.05f,
      1f
        };
        /// <summary>XY 좌표 색상</summary>
        public static float[] DocumentAxesColor = new float[4]
        {
      0.4f,
      0.4f,
      0.4f,
      0.8f
        };
        /// <summary>선택 사각 박스(Rubber band) 색상</summary>
        public static float[] DocumentRubberBandRectColor = new float[4]
        {
      1f,
      1f,
      1f,
      0.7f
        };
        /// <summary>선택된 외곽 사각형의 색상</summary>
        public static float[] DocumentSelectedBoundRectColor = new float[4]
        {
      0.0f,
      1f,
      1f,
      0.4f
        };
        /// <summary>선택된 외곽 사각형의 중심 색상</summary>
        public static float[] DocumentSelectedBoundRectCenterColor = new float[4]
        {
      1f,
      1f,
      0.0f,
      0.6f
        };
        /// <summary>회전 중심 색상</summary>
        public static float[] DocumentRotateOriginColor = new float[4]
        {
      0.9f,
      0.0f,
      0.0f,
      0.8f
        };
        /// <summary>기준점 색상</summary>
        public static float[] DocumentOriginColor = new float[4]
        {
      0.9f,
      0.0f,
      0.0f,
      0.8f
        };
        /// <summary>외곽 사각형 색상</summary>
        public static float[] DocumentBoundRectColor = new float[4]
        {
      1f,
      0.0f,
      0.0f,
      0.6f
        };
        /// <summary>엔티티 선택 생상</summary>
        public static float[] EntitySelectedColor = new float[4]
        {
      0.0f,
      1f,
      0.0f,
      0.5f
        };
        /// <summary>엔티티 중심 색상</summary>
        public static float[] EntityCenterColor = new float[4]
        {
      1f,
      1f,
      0.0f,
      0.5f
        };
        /// <summary>그룹 엔티티 오프셋 색상</summary>
        public static float[] EntityGroupOffsetNormalColor = new float[4]
        {
      0.6f,
      0.6f,
      0.6f,
      0.7f
        };
        /// <summary>그룹 엔티티 오프셋 선택시 색상</summary>
        public static float[] EntityGroupOffsetSelectedColor = new float[4]
        {
      0.0f,
      0.6f,
      0.0f,
      0.6f
        };
        /// <summary>엔티티 화살표 색상</summary>
        public static float[] EntityArrowColor = new float[4]
        {
      1f,
      0.0f,
      0.0f,
      0.8f
        };
        /// <summary>개체 Divide 시도시 미리보기용 사각형들의 색상</summary>
        public static float[] DivideRectColor = new float[4]
        {
      1f,
      1f,
      0.0f,
      0.9f
        };
        /// <summary>레이저 가공 경로 시뮬레이션시 사용하는 색상1</summary>
        public static float[] LaserSpotCrossColor1 = new float[4]
        {
      1f,
      1f,
      0.0f,
      0.6f
        };
        public static float[] LaserSpotCrossColor2 = new float[4]
        {
      1f,
      1f,
      0.0f,
      0.8f
        };
        public static float[] LaserSpotCircleColor1 = new float[4]
        {
      1f,
      0.0f,
      0.0f,
      0.7f
        };
        public static float[] LaserSpotCircleColor2 = new float[4]
        {
      1f,
      0.0f,
      0.0f,
      0.3f
        };
        /// <summary>호(Arc) 렌더링시 그리는 각도의 최소 단위 (속도 향상)</summary>
        public static int AngleFactor = 10;
        /// <summary>폐곡선 판단 거리 상수값</summary>
        public static float ClosedPathDistance = 0.005f;
        /// <summary>FreeType 폰트 분석시 베지어 곡선 구간을 미분할 최소 거리값 (mm)</summary>
        public static float BezierSplineMicroStepDistance = 0.1f;
        /// <summary>언두(Undo) 스택 최대 개수 제한 (메모리 절약을 위해)</summary>
        public static int UndoStackSize = 100;
        public static string NodeFont = "Arial";
        public static int NodeFontSize = 10;
        public static Color DefaultColor = Color.White;
        public static Color[] PensColor = new Color[10]
        {
      Color.White,
      Color.Orange,
      Color.Navy,
      Color.Maroon,
      Color.Lime,
      Color.Magenta,
      Color.SkyBlue,
      Color.Violet,
      Color.Pink,
      Color.Gold
        };

        /// <summary>버전</summary>
        public static string DocumentVersion => Assembly.LoadFrom("Scanlab.sirius.dll").GetName().Version.ToString();
    }
}
