using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    /// <summary>편집기에서 새로운 레이어를 생성할때 발생하는 이벤트</summary>
    /// <param name="sender"></param>
    public delegate void SiriusDocumentLayerNew(object sender);
    /// <summary>문서 새로 만들기</summary>
    /// <param name="sender"></param>
    public delegate void SiriusDocumentNew(object sender);
    /// <summary>문서 열기</summary>
    /// <param name="sender"></param>
    public delegate void SiriusDocumentOpen(object sender);
    /// <summary>편집기에서 새로운 펜을 생성할때 발생하는 이벤트</summary>
    /// <param name="sender"></param>
    public delegate void SiriusDocumentPenNew(object sender);
    /// <summary>문서 저장</summary>
    /// <param name="sender"></param>
    public delegate void SiriusDocumentSave(object sender);
    /// <summary>문서 다름이름으로 저장</summary>
    /// <param name="sender"></param>
    public delegate void SiriusDocumentSaveAs(object sender);
    /// <summary>뷰어가 렌더링할 대상 소스 문서가 변경되었을때를 처리하는 델리게이트</summary>
    /// <param name="sender"></param>
    /// <param name="doc"></param>
    public delegate void SiriusDocumentSourceChanged(object sender, IDocument doc);
}
