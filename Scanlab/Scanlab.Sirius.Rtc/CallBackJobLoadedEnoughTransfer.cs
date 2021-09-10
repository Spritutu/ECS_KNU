

using System;

namespace Scanlab.Sirius
{
    internal class CallBackJobLoadedEnoughTransfer : JobCallback
    {
        private Rtc6SyncAxis rtc;

        public CallBackJobLoadedEnoughTransfer(Rtc6SyncAxis rtc) => this.rtc = rtc;

        public override void Run(uint jobID)
        {
            this.rtc.jobStatus.transStatus = TransferStatus.LoadedEnough;
            if (this.rtc.handle == 0U)
                return;
            this.rtc.jobStatus.transStatus = TransferStatus.LoadedEnough;
            uint errorCode = syncAXIS.slsc_ctrl_start_execution(this.rtc.handle);
            if (errorCode == 0U)
                Logger.Log(Logger.Type.Warn, string.Format("syncaxis [{0}]: job id : {1}, loaded enough. so starting automatically", (object)this.rtc.Index, (object)jobID), Array.Empty<object>());
            else
                Logger.Log(Logger.Type.Error, string.Format("syncaxis [{0}]: job id : {1}, fail to slsc_ctrl_start_execution. error: {2}", (object)this.rtc.Index, (object)jobID, (object)this.rtc.CtlGetErrMsg(errorCode)), Array.Empty<object>());
        }
    }
}
