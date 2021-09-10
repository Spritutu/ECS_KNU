using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanlab.Sirius
{
    internal class CallBackJobProgressCalculationPlanned : ExecTimeCallback
    {
        private Rtc6SyncAxis rtc;

        public CallBackJobProgressCalculationPlanned(Rtc6SyncAxis rtc) => this.rtc = rtc;

        public override void Run(uint jobID, ulong Progress, double ExecTime) => this.rtc.jobStatus.calcStatus = CalculationStatus.InProgress;
    }
}
