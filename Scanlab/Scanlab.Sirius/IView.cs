
using SharpGL;
using System;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    /// <summary>뷰 인터페이스 (UI 측의 마우스및 윈도우의 이벤트 연결용)</summary>
    public interface IView
    {
        /// <summary>부모 문서</summary>
        IDocument Owner { get; set; }

        /// <summary>렌더러</summary>
        OpenGL Renderer { get; }

        /// <summary>clipping client 폭 (픽셀)</summary>
        int Width { get; }

        /// <summary>clipping client 높이 (픽셀)</summary>
        int Height { get; }

        /// <summary>렌더 중심 X 위치 (사용자 좌표계)</summary>
        float CameraX { get; set; }

        /// <summary>렌더 중심 Y 위치 (사용자 좌표계)</summary>
        float CameraY { get; set; }

        /// <summary>폭 확대 비율 (확대 축소에 사용)</summary>
        float ScaleWidth { get; set; }

        /// <summary>높이 확대 비율 (확대 축소에 사용)</summary>
        float ScaleHeight { get; set; }

        /// <summary>스케일 비율</summary>
        float Scale { get; set; }

        bool IsPerspective { get; set; }

        /// <summary>선택된 개체(entities)들이 이루는 외곽 영역 정보</summary>
        BoundRect SelectedBoundRect { get; }

        /// <summary>
        /// 화면 렌더링(그리기)에 걸린 시간
        /// msec
        /// </summary>
        long RenderTime { get; }

        /// <summary>사용자 정의 데이타</summary>
        object Tag { get; set; }

        /// <summary>화면 다시 그리기 - 재 렌더링 명령</summary>
        void Render();

        /// <summary>
        /// 다시 그리기 이벤트
        /// <returns>렌더링에 걸린 시간 (msec) </returns>
        /// </summary>
        long OnDraw();

        /// <summary>초기화 이벤트</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnInitialized(object sender, EventArgs e);

        /// <summary>클라이언트 창 크기 변경 이벤트</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnResized(object sender, EventArgs e);

        /// <summary>마우스 다운 이벤트</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseDown(object sender, MouseEventArgs e);

        /// <summary>마우스 업 이벤트</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseUp(object sender, MouseEventArgs e);

        /// <summary>마우스 이동 이벤트</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseMove(object sender, MouseEventArgs e);

        /// <summary>마우스 휠 이벤트</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMouseWheel(object sender, MouseEventArgs e);

        /// <summary>특정 위치를 중심으로 줌 인 이벤트</summary>
        /// <param name="p"></param>
        void OnZoomIn(System.Drawing.Point p);

        /// <summary>특정 위치를 중심으로 줌 아웃 이벤트</summary>
        /// <param name="p"></param>
        void OnZoomOut(System.Drawing.Point p);

        /// <summary>지정된 영역이 클라이언트 렌더 영역을 가득(약 95%) 채우도록 줌 FIT</summary>
        /// <param name="br"></param>
        void OnZoomFit(BoundRect br = null);

        /// <summary>
        /// Pan 기능 사용 유무
        /// 마우스 조작이 어려운 환경에서, 마우스 왼쪽 버튼만으로 카메라 위치를 이동하기 위한 기능 on/off
        /// </summary>
        /// <param name="onOff"></param>
        void OnPan(bool onOff);

        /// <summary>클라이언트 영역 중심을 사용자가 원하는 중심으로 재 설정 (자동으로 새로 그려짐)</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        void OnCameraMove(float x, float y);

        /// <summary>
        /// 물리 좌표계(pixel) 값을 사용자 좌표계(mm)로 변환
        /// pixel -&gt; mm
        /// </summary>
        /// <param name="p"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        void Dp2Lp(System.Drawing.Point p, out float x, out float y);

        /// <summary>사용자 좌표(mm)를 물리 좌표(pixel)로 변환</summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="p"></param>
        void Lp2Dp(float x, float y, out System.Drawing.Point p);

        /// <summary>
        /// 해당 물리 좌표값(pixel) 크기를 사용자 좌표 크기(mm)로 변환
        /// pixel -&gt; mm
        /// </summary>
        /// <param name="pixel"></param>
        /// <returns></returns>
        float Dp2Lp(int pixel);

        /// <summary>사용자 좌표(mm) 크기만큼을 물리 좌표(pixel) 크기로 변환</summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int Lp2Dp(float value);
    }
}
