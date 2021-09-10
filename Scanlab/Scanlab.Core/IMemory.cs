using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab
{
    /// <summary>
    /// 메모리 인터페이스
    /// 데이타의 변경을 화면/혹은 DB에 통지해야 할 경우 PropertyChanged 이벤트 핸들러를 등록하여 사용한다
    /// 메모리 정보를 DB 에 저장하는 경우 속성 정보가 변경될때마다 DB와 동기화 된다.
    /// 만약 DB 저장이 필요하지 않은 속성의 경우에는 [JsonIgnore] Attribute 를 지정해 준다
    /// 다중 쓰레드 환경등에서의 동기화가 필요할 경우에는 반드시 SyncRoot 를 통해 공유 자원을 보호해야 한다
    /// </summary>
    public interface IMemory : INotifyPropertyChanged, IDisposable
    {
        /// <summary>이름</summary>
        string Name { get; }

        /// <summary>동기화 객체</summary>
        object SyncRoot { get; }
    }
}
