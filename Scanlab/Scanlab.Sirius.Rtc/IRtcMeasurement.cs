using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>RTC Measurement (계측 데이타) 인터페이스</summary>
    public interface IRtcMeasurement
    {
        /// <summary>측정된 데이타 가져오기</summary>
        /// <param name="channel">채널</param>
        /// <param name="data">데이타 배열</param>
        /// <returns></returns>
        bool CtlGetMeasurement(MeasurementChannel channel, out int[] data);

        /// <summary>리스트 명령 - 측정 시작</summary>
        /// <param name="frequency">usec</param>
        /// <param name="channels">대상 채널</param>
        /// <returns></returns>
        bool ListMeasurementBegin(float frequency, MeasurementChannel[] channels);

        /// <summary>리스트 명령 - 측정 끝</summary>
        /// <returns></returns>
        bool ListMeasurementEnd();
    }
}
