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
    public class F_LASER_RESET : AbstractFunction
    {
        private const string ALARM_LASER_RESET_TIMEOUT = "E3005";
        private const string ALARM_LASER_RESET_FAIL = "E3006";

        private const string DO_NAME_LASER_RESET = "oPMAC.iLaser.Reset";

        private const string DI_NAME_LASER_FAULT = "oPMAC.iLaser.Fault";

        public override bool CanExecute()
        {
            IsAbort = false;
            IsProcessing = false;

            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;

            if (DataManager.Instance.SET_INT_DATA(DO_NAME_LASER_RESET, 1))
            {
                Stopwatch stopwatch = Stopwatch.StartNew();

                while (true)
                {
                    Thread.Sleep(10);

                    if (IsAbort)
                    {
                        return F_RESULT_ABORT;
                    }
                    else if (stopwatch.ElapsedMilliseconds > TimeoutMiliseconds)
                    {
                        AlarmManager.Instance.SetAlarm(ALARM_LASER_RESET_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (DataManager.Instance.GET_INT_DATA(DI_NAME_LASER_FAULT, out result) == 0)
                    {
                        IsAbort = false;
                        IsProcessing = false;
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
                AlarmManager.Instance.SetAlarm(ALARM_LASER_RESET_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void ExecuteWhenSimulate()
        {
            DataManager.Instance.SET_INT_DATA(DI_NAME_LASER_FAULT, 0);
        }

        public override void PostExecute()
        {
            if (EquipmentSimulation == OperationMode.SIMULATION.ToString())
            {
                DataManager.Instance.SET_INT_DATA(DO_NAME_LASER_RESET, 0);
            }
        }
    }
}
