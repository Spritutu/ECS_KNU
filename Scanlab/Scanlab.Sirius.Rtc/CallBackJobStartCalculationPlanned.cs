using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    internal class CallBackJobStartCalculationPlanned : JobCallback
    {
        private Rtc6SyncAxis rtc;

        public CallBackJobStartCalculationPlanned(Rtc6SyncAxis rtc) => this.rtc = rtc;

        public override void Run(uint jobID)
        {
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, started trajectory planning. so calculating ....", (object)this.rtc.Index, (object)jobID), Array.Empty<object>());
            this.rtc.jobStatus.calcStatus = CalculationStatus.Start;
        }
    }
}
