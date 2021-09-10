using System;

namespace Scanlab.Sirius
{
    internal class CallBackJobIsExecuting : ExecTimeCallback
    {
        private Rtc6SyncAxis rtc;

        public CallBackJobIsExecuting(Rtc6SyncAxis rtc) => this.rtc = rtc;

        public override void Run(uint jobID, ulong Progress, double ExecTime)
        {
            if (this.rtc.handle == 0U || ExecutionStatus.Executing == this.rtc.jobStatus.execStatus)
                return;
            this.rtc.jobStatus.execStatus = ExecutionStatus.Executing;
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, is executing ...", (object)this.rtc.Index, (object)jobID), Array.Empty<object>());
        }
    }
}