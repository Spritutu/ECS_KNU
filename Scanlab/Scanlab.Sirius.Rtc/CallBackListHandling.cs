using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    internal class CallBackListHandling : ListHandlingCallback
    {
        private Rtc6SyncAxis rtc;

        public CallBackListHandling(Rtc6SyncAxis rtc) => this.rtc = rtc;

        public override bool Run(uint arg0)
        {
            if (this.rtc.handle == 0U)
                return false;
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: list handling : {1}", (object)this.rtc.Index, (object)arg0), Array.Empty<object>());
            return true;
        }
    }
}
