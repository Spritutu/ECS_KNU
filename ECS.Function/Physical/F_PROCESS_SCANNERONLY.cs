using INNO6.Core.Manager;
using INNO6.IO;
using Scanlab.Sirius;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ECS.Function.Physical
{
    public class F_PROCESS_SCANNERONLY : AbstractFunction
    {
        private const int NOTBUSY = 0;
        private const int BUSY = 1;
        private const int ERROR = 1;
        private const int NOERROR = 0;
        private const int OK = 1;
        private const int NOTOK = 0;

        private const string ALARM_SCAN_PROCDOC_START_TIMEOUT = "E3015";
        private const string ALARM_SCAN_PROCDOC_START_FAIL = "E3016";

        private const string DO_NAME_SCAN_PROCDOC_START = "oRTC.oScan.Document";
        private const string DO_NAME_SCAN_LAYER_START = "oRTC.oScan.LayerObject";
        private const string VIO_NAME_SCAN_TEMP_OBJECT = "vSet.oScan.TempObject";

        private const string DI_NAME_SCAN_BUSY = "iRTC.iScan.BusyStatus";
        private const string DI_NAME_SCAN_ERROR = "iRTC.iScan.ErrorStatus";
        private const string DI_NAME_SCAN_POSITIONACK = "iRTC.iScan.PosAckStatus";
        private const string DI_NAME_SCAN_POWER = "iRTC.iScan.PowerStatus";
        private const string DI_NAME_SCAN_TEMP = "iRTC.iScan.TempStatus";

        private IDocument _ScanDocument = null;

        public F_PROCESS_SCANNERONLY()
        {
        }

        public override bool CanExecute()
        {
            bool check = true;
            Abort = false;
            IsProcessing = false;

            _ScanDocument = (IDocument)DataManager.Instance.GET_OBJECT_DATA(VIO_NAME_SCAN_TEMP_OBJECT, out bool _);

            if (_ScanDocument == null) check &= false;

            check &= this.EquipmentStatusCheck();

            return check;
        }

        public override string Execute()
        {
            bool result = false;

            if (DataManager.Instance.SET_OBJECT_DATA(DO_NAME_SCAN_PROCDOC_START, _ScanDocument))
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (true)
                {
                    Thread.Sleep(10);

                    if (Abort)
                    {
                        return F_RESULT_ABORT;
                    }
                    else if (stopwatch.ElapsedMilliseconds > TimeoutMiliseconds)
                    {

                        AlarmManager.Instance.SetAlarm(ALARM_SCAN_PROCDOC_START_TIMEOUT);
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
                AlarmManager.Instance.SetAlarm(ALARM_SCAN_PROCDOC_START_FAIL);
                return this.F_RESULT_FAIL;
            }
        }
    
        public override void PostExecute()
        {
            //throw new NotImplementedException();
        }
    }
}
