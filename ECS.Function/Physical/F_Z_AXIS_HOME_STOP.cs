using ECS.Common.Helper;
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
    public class F_Z_AXIS_HOME_STOP : AbstractFunction
    {
        private const string IO_Z_HOME_STOP = "oPMAC.iAxisZ.HomeStop";
        private const string IO_Z_IS_HOMMING = "iPMAC.iAxisZ.IsHomming";

        private const string ALARM_Z_AXIS_HOMESTOP_TIMEOUT = "E2041";
        private const string ALARM_Z_AXIS_HOMESTOP_FAIL = "E2042";

        public override bool CanExecute()
        {
            IsAbort = false;
            IsProcessing = false;

            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;
            FunctionManager.Instance.ABORT_FUNCTION(FuncNameHelper.Z_AXIS_HOMMING);

            if (DataManager.Instance.SET_INT_DATA(IO_Z_HOME_STOP, 1))
            {
                Thread.Sleep(100);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                while (true)
                {
                    Thread.Sleep(100);

                    if (IsAbort)
                    {
                        return F_RESULT_ABORT;
                    }
                    else if (stopwatch.ElapsedMilliseconds > TimeoutMiliseconds)
                    {
                        AlarmManager.Instance.SetAlarm(ALARM_Z_AXIS_HOMESTOP_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (DataManager.Instance.GET_INT_DATA(IO_Z_IS_HOMMING, out result) == 0 ||
                        EquipmentSimulation == OperationMode.SIMULATION.ToString())
                    {
                        return this.F_RESULT_SUCCESS;
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
                AlarmManager.Instance.SetAlarm(ALARM_Z_AXIS_HOMESTOP_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void PostExecute()
        {
            IsAbort = false;
            IsProcessing = false;
            DataManager.Instance.SET_INT_DATA(IO_Z_HOME_STOP, 0);          
        }
    }
}
