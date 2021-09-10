using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab
{
    /// <summary>로그 메시지 콜백 델리게이트</summary>
    /// <param name="type">로그 타입</param>
    /// <param name="message">로그 메시지</param>
    public delegate void LoggerMessage(Logger.Type type, string message);
}
