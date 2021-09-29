using INNO6.IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using INNO6.Core.Manager;

namespace ECS.Function.Physical
{
    public class F_Z_AXIS_HOMMING : AbstractFunction
    {
        private const string IO_Z_HOMMING_START = "oPMAC.iAxisZ.Homming";
        private const string IO_Z_HOMMING_COMPLETE = "iPMAC.iAxisZ.IsHome";

        private const string ALARM_X_AXIS_HOMMING_TIMEOUT = "E2043";
        private const string ALARM_X_AXIS_HOMMING_FAIL = "E2044";

        public override bool CanExecute()
        {
            IsAbort = false;
            IsProcessing = false;
            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;

            if (DataManager.Instance.SET_INT_DATA(IO_Z_HOMMING_START, 1))
            {
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
                        AlarmManager.Instance.SetAlarm(ALARM_X_AXIS_HOMMING_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (DataManager.Instance.GET_INT_DATA(IO_Z_HOMMING_COMPLETE, out result) == 1)
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
                AlarmManager.Instance.SetAlarm(ALARM_X_AXIS_HOMMING_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void ExecuteWhenSimulate()
        {
            DataManager.Instance.SET_INT_DATA(IO_Z_HOMMING_COMPLETE, 1);
        }

        public override void PostExecute()
        {
            IsAbort = false;
            IsProcessing = false;
            DataManager.Instance.SET_INT_DATA(IO_Z_HOMMING_START, 0);
            DataManager.Instance.SET_INT_DATA(IO_Z_HOMMING_COMPLETE, 0);
        }
    }
}
