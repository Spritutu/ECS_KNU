using INNO6.Core.Manager;
using INNO6.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECS.Function.Physical
{
    public class F_SCAN_PROCESS_ABORT : AbstractFunction
    {
        
        private const int NOTBUSY = 0;
        private const int BUSY = 1;
        private const int ERROR = 1;
        private const int NOERROR = 0;
        private const int OK = 1;
        private const int NOTOK = 0;

        private const string ALARM_SCAN_PROCESS_ABORT_TIMEOUT = "E3013";
        private const string ALARM_SCAN_PROCESS_ABORT_FAIL = "E3014";


        private const string DO_NAME_SCAN_PROCESS_ABORT = "oRTC.iScan.ProcessAbort";

        private const string DI_NAME_SCAN_BUSY = "iRTC.iScan.BusyStatus";
        private const string DI_NAME_SCAN_ERROR = "iRTC.iScan.ErrorStatus";
        private const string DI_NAME_SCAN_POSITIONACK = "iRTC.iScan.PositionAckStatus";
        private const string DI_NAME_SCAN_POWER = "iRTC.iScan.PowerStatus";
        private const string DI_NAME_SCAN_TEMP = "iRTC.iScan.TempStatus";

        public override bool CanExecute()
        {
            bool check = true;
            Abort = false;
            IsProcessing = false;

            check &= this.EquipmentStatusCheck();
            return check;
        }

        public override string Execute()
        {
            bool result = false;

            if (DataManager.Instance.SET_INT_DATA(DO_NAME_SCAN_PROCESS_ABORT, 1))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (true)
                {
                    Thread.Sleep(100);

                    if (Abort)
                    {
                        return F_RESULT_ABORT;
                    }
                    else if (stopwatch.ElapsedMilliseconds > TimeoutMiliseconds)
                    {

                        AlarmManager.Instance.SetAlarm(ALARM_SCAN_PROCESS_ABORT_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (DataManager.Instance.GET_INT_DATA(DI_NAME_SCAN_BUSY, out result) == NOTBUSY)
                    {
                        return this.F_RESULT_SUCCESS;
                    }
                    else if (EquipmentSimulation == OperationMode.SIMULATION.ToString())
                    {
                        ExecuteWhenSimulate();
                    }
                    else
                    {
                        IsProcessing = true;
                        continue;
                    }
                }
            }
            else
            {
                AlarmManager.Instance.SetAlarm(ALARM_SCAN_PROCESS_ABORT_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void ExecuteWhenSimulate()
        {
            DataManager.Instance.SET_INT_DATA(DI_NAME_SCAN_BUSY, NOTBUSY);
        }

        public override void PostExecute()
        {
            DataManager.Instance.SET_INT_DATA(DO_NAME_SCAN_PROCESS_ABORT, 0);
        }
        
    }
}
