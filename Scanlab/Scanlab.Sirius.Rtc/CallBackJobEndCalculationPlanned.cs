
using System;

namespace Scanlab.Sirius
{
    internal class CallBackJobEndCalculationPlanned : ExecTimeCallback
    {
        private Rtc6SyncAxis rtc;

        public CallBackJobEndCalculationPlanned(Rtc6SyncAxis rtc) => this.rtc = rtc;

        public override void Run(uint jobID, ulong Progress, double ExecTime)
        {
            this.rtc.jobStatus.calcStatus = CalculationStatus.Finished;
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, trajectory planning ended and job characteristics as below ...", (object)this.rtc.Index, (object)jobID), Array.Empty<object>());
            double[] numArray1 = new double[2];
            double[] numArray2 = new double[2];
            double[] numArray3 = new double[2];
            double[] numArray4 = new double[2];
            double[] numArray5 = new double[2];
            double[] numArray6 = new double[2];
            double[] numArray7 = new double[2];
            int jobCharacteristic1 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_ScannerPosX, out numArray1[0]);
            int jobCharacteristic2 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_ScannerPosY, out numArray1[1]);
            int jobCharacteristic3 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_StagePosX, out numArray2[0]);
            int jobCharacteristic4 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_StagePosY, out numArray2[1]);
            int jobCharacteristic5 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_ScannerVelX, out numArray3[0]);
            int jobCharacteristic6 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_ScannerVelY, out numArray3[1]);
            int jobCharacteristic7 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_StageVelX, out numArray4[0]);
            int jobCharacteristic8 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_StageVelY, out numArray4[1]);
            int jobCharacteristic9 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_ScannerAccX, out numArray5[0]);
            int jobCharacteristic10 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_ScannerAccY, out numArray5[1]);
            int jobCharacteristic11 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_StageAccX, out numArray6[0]);
            int jobCharacteristic12 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_StageAccY, out numArray6[1]);
            int jobCharacteristic13 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_StageJerkX, out numArray7[0]);
            int jobCharacteristic14 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_StageJerkY, out numArray7[1]);
            double num1;
            int jobCharacteristic15 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_MotionMicroSteps, out num1);
            double num2;
            int jobCharacteristic16 = (int)syncAXIS.slsc_ctrl_get_job_characteristic(this.rtc.handle, jobID, slsc_JobCharacteristic.slsc_JobCharacteristic_InsertedSkywritings, out num2);
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, characteristic max scanner position x, y= {2:F3}, {3:F3} mm", (object)this.rtc.Index, (object)jobID, (object)numArray1[0], (object)numArray1[1]), Array.Empty<object>());
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, characteristic max stage   position x, y= {2:F3}, {3:F3} mm", (object)this.rtc.Index, (object)jobID, (object)numArray2[0], (object)numArray2[1]), Array.Empty<object>());
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, characteristic max scanner velocity x, y= {2:F3}, {3:F3} mm/s", (object)this.rtc.Index, (object)jobID, (object)numArray3[0], (object)numArray3[1]), Array.Empty<object>());
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, characteristic max stage   velocity x, y= {2:F3}, {3:F3} mm/s", (object)this.rtc.Index, (object)jobID, (object)numArray4[0], (object)numArray4[1]), Array.Empty<object>());
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, characteristic max scanner accel    x, y= {2:F3}, {3:F3} mm/s\u00B2", (object)this.rtc.Index, (object)jobID, (object)numArray5[0], (object)numArray5[1]), Array.Empty<object>());
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, characteristic max stage   accel    x, y= {2:F3}, {3:F3} mm/s\u00B2", (object)this.rtc.Index, (object)jobID, (object)numArray6[0], (object)numArray6[1]), Array.Empty<object>());
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, characteristic max stage   jerk     x, y= {2:F3}, {3:F3} mm/s\u00B3", (object)this.rtc.Index, (object)jobID, (object)numArray7[0], (object)numArray7[1]), Array.Empty<object>());
            Logger.Log(Logger.Type.Info, string.Format("syncaxis [{0}]: job id : {1}, characteristic estimated time = {2:F1}/{3:F1}s, micro vectors= {4}, sky writings= {5}", (object)this.rtc.Index, (object)jobID, (object)(ExecTime / 1000.0), (object)(num1 / 100000.0), (object)num1, (object)num2), Array.Empty<object>());
        }
    }
}
