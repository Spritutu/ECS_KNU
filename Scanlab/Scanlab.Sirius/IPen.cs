
using Newtonsoft.Json;
using System;
using System.Drawing;

namespace Scanlab.Sirius
{
    /// <summary>
    /// Pen 인터페이스 (레이저 가공 파라메터를 가지는 펜 인터페이스)
    /// 장비별로 프로젝트 별로 레이저 가공 기법이 다르므로, 펜(IPen)을 상속받아 Pen 객체를 구현
    /// </summary>
    [JsonObject]
    public interface IPen : IEntity, IMarkerable, ICloneable
    {
        /// <summary>색상 식별자</summary>
        Color Color { get; set; }

        /// <summary>파워 (W)</summary>
        float Power { get; }

        /// <summary>주파수 (Hz)</summary>
        float Frequency { get; }

        /// 펄스폭 (usec)
        float PulseWidth { get; }

        /// <summary>점프 속도 (mm/s)</summary>
        float JumpSpeed { get; }

        /// <summary>마크 속도 (mm/s)</summary>
        float MarkSpeed { get; }
    }
}
