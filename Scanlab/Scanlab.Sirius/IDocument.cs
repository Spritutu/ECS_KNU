// Decompiled with JetBrains decompiler
// Type: SpiralLab.Sirius.IDocument
// Assembly: spirallab.sirius, Version=1.0.7.3, Culture=neutral, PublicKeyToken=null
// MVID: 45F5C82C-CD8C-4777-BF82-85C50A80042A
// Assembly location: C:\Users\sean0\Downloads\sirius-master\sirius-master\bin\spirallab.sirius.dll

using System;
using System.Collections.Generic;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Document 인터페이스
    /// 엔티티, 레이어, 블럭 , 문서정보 등의 통합 데이타를 가지는 도큐먼트 데이타 (레이저 가공 레시피)
    /// 주의) Document 를 상속 구현하는 모든 문서 객체들은 SpiralLab.Sirius 네임스페이스 유지해야 함
    /// </summary>
    public interface IDocument : ICloneable
    {
        /// <summary>문서 버전 정보</summary>
        string Version { get; }

        /// <summary>이름</summary>
        string Name { get; set; }

        /// <summary>설명</summary>
        string Description { get; set; }

        /// <summary>파일이름</summary>
        string FileName { get; set; }

        /// <summary>사용자 정의 확장 데이타</summary>
        string ExtensionData { get; set; }

        /// <summary>사용자 정의 외부 확장용 파일의 경로</summary>
        string ExtensionFilePath { get; set; }

        /// <summary>
        /// 사용자 명령 및 이벤트 액션 객체
        /// (저장되지 않는 임시 객체)
        /// </summary>
        Action Action { get; }

        /// <summary>문서의 중심 위치, 가로및 세로 크기 정보</summary>
        BoundRect Dimension { get; set; }

        /// <summary>X, Y 오프셋(mm) 및 회전각도</summary>
        Offset RotateOffset { get; set; }

        /// <summary>
        /// 문서(Doc) 에 연결된 뷰 목록 (다중 뷰어를 제공하기 위해 내부적으로 사용됨)
        /// (저장되지 않는 임시 객체)
        /// </summary>
        List<IView> Views { get; set; }

        /// <summary>
        /// 블럭 컨테이너  (내부에 복수개의 Block 가 있고, Block 안에 복수개의 Entity 가 있음)
        /// </summary>
        Blocks Blocks { get; }

        /// <summary>
        /// 레이어 컨테이너 (내부에 복수개의 Layer 가 있고, Layer 안에 복수개의 Entity 가 있음)
        /// </summary>
        Layers Layers { get; }

        /// <summary>
        /// 사용자 정의 데이타
        /// (저장되지 않는 임시 객체)
        /// </summary>
        object Tag { get; set; }

        /// <summary>새로운 문서의 초기 상태로 변경</summary>
        void New();
    }
}
