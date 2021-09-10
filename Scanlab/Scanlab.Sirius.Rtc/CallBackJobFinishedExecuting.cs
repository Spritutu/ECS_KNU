using System;

namespace Scanlab.Sirius
{
    internal class CallBackJobFinishedExecuting : ExecTimeCallback
    {
        private Rtc6SyncAxis rtc;

        public CallBackJobFinishedExecuting(Rtc6SyncAxis rtc) => this.rtc = rtc;

        public override void Run(uint jobID, ulong Progress, double ExecTime)
        {
            if (this.rtc.handle == 0U)
                return;
            this.rtc.jobStatus.execStatus = ExecutionStatus.Finished;
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, execution finished.", (object)this.rtc.Index, (object)jobID), Array.Empty<object>());
        }
    }
}
