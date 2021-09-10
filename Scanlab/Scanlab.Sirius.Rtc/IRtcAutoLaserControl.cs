using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>RTC 의 ALC(Automatic Laser Control 기능의 인터페이스</summary>
    public interface IRtcAutoLaserControl
    {
        /// <summary>
        /// ALC(Automatic Laser Control 중 위치 의존적 방법으로 Scale 값이 외부 파일에서 제공됨
        /// Null 지정후 CtlAutoLaserControl 호출하면 비활성화됨
        /// 포맷
        /// [PositionCtrlTable No]
        /// PositionNo = Value
        /// ScaleNo = Value
        /// ...
        ///  N 1-50
        /// Position : 스캐너 중심으로 부터 떨어진 거리 (반지름) 의 퍼센트값: 100 % =  2^19 bits (RTC5 경우)
        /// Scale : 0- 4
        /// </summary>
        string AutoLaserControlByPositionFileName { get; set; }

        /// <summary>
        /// ALC(Automatic Laser Control 중 위치 의존적 방법으로 어떤 테이블을 사용할지 지정
        /// </summary>
        uint AutoLaserControlByPositionTableNo { get; set; }

        /// <summary>ALC(Automatic Laser Control) 기능 설정</summary>
        /// <typeparam name="T">AutoLaserControlSignal 열거형중 ExtDO 는 uint, 그외는 float</typeparam>
        /// <param name="ctrl">AutoLaserControlSignal 열거형</param>
        /// <param name="mode">AutoLaserControlMode 열거형</param>
        /// <param name="percentage100">100% 일때의 출력값</param>
        /// <param name="min">최소 출력값</param>
        /// <param name="max">최대 출력값</param>
        /// <param name="compensator">출력값 보정기 사용시 지정</param>
        /// <returns></returns>
        bool CtlAutoLaserControl<T>(
          AutoLaserControlSignal ctrl,
          AutoLaserControlMode mode,
          T percentage100,
          T min,
          T max,
          ICompensator<T> compensator = null);

        /// <summary>
        /// ALC(Automatic Laser Control) 기능중 Vector Dependent 기능을 활성화
        /// </summary>
        /// <typeparam name="T">AutoLaserControlSignal 열거형중 ExtDO 는 uint, 그외는 float</typeparam>
        /// <param name="ctrl">AutoLaserControlSignal 열거형</param>
        /// <param name="startingValue">시작 출력값</param>
        /// <param name="compensator">출력값 보정기 사용시 지정</param>
        /// <returns></returns>
        bool ListAlcByVectorBegin<T>(
          AutoLaserControlSignal ctrl,
          T startingValue,
          ICompensator<T> compensator = null);

        /// <summary>
        /// ALC(Automatic Laser Control) 기능중 Vector Dependent 기능을 비활성화
        /// </summary>
        /// <returns></returns>
        bool ListAlcByVectorEnd();
    }
}
