using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>
    /// 모터 인터페이스
    /// 스캐너 Z 축을 제어하기 위한 모터 인터페이스
    /// </summary>
    public interface IMotor
    {
        /// <summary>동기화 객체</summary>
        object SyncRoot { get; }

        /// <summary>모터 번호</summary>
        uint No { get; set; }

        /// <summary>모터 이름</summary>
        string Name { get; set; }

        /// <summary>위치 (mm)</summary>
        float Position { get; }

        /// <summary>준비상태 여부</summary>
        bool IsReady { get; }

        /// <summary>구동중 여부</summary>
        bool IsBusy { get; }

        /// <summary>알람 발생 여부</summary>
        bool IsError { get; }

        /// <summary>사용자 정의 데이타</summary>
        object Tag { get; set; }

        /// <summary>홈(원점) 검색 시작</summary>
        /// <returns></returns>
        bool CtlHomeSearch();

        /// <summary>절대 위치 이동</summary>
        /// <param name="position">위치값(mm)</param>
        /// <returns></returns>
        bool CtlMoveAbs(float position);

        /// <summary>상대 위치 이동</summary>
        /// <param name="distance">거리값(mm)</param>
        /// <returns></returns>
        bool CtlMoveRel(float distance);

        /// <summary>정지</summary>
        /// <returns></returns>
        bool CtlMoveStop();

        /// <summary>알람 해제</summary>
        /// <returns></returns>
        bool CtlReset();
    }
}
