using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Correction (스캐너 보정) 인터페이스
    /// 실행파일\correction\correXionPro.exe (for 2D)
    /// 실행파일\correction\stretchcorreXion5.exe (for 3D)
    /// 가 경로상에 존재해야 한다
    /// </summary>
    public interface ICorrection
    {
        /// <summary>결과 통보용 이벤트 핸들러</summary>
        event ResultEventHandler OnResult;

        /// <summary>입력 데이타의 행 개수</summary>
        int Rows { get; set; }

        /// <summary>입력 데이타의 열 개수</summary>
        int Cols { get; set; }

        /// <summary>보정 간격 (mm)</summary>
        float Interval { get; }

        /// <summary>K 값 (bits/mm)</summary>
        float KFactor { get; }

        /// <summary>입력 보정 파일 (correction 폴더에서의 상대적 경로)</summary>
        string SourceCorrectionFile { get; }

        /// <summary>출력 보정 파일 (correction 폴더에서의 상대적 경로)</summary>
        string TargetCorrectionFile { get; }

        /// <summary>입력 데이타 모두 제거</summary>
        void Clear();

        /// <summary>변환 시작</summary>
        /// <returns>성공 여부</returns>
        bool Convert();
    }
}
