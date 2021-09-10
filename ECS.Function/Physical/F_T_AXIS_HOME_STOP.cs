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
    public class F_T_AXIS_HOME_STOP : AbstractFunction
    {
        private const string IO_T_HOME_STOP = "oPMAC.iAxisT.HomeStop";
        private const string IO_T_IS_HOMMING = "iPMAC.iAxisT.IsHomming";
        private const string ALARM_T_AXIS_HOMESTOP_TIMEOUT = "E2011";
        private const string ALARM_T_AXIS_HOMESTOP_FAIL = "E2012";

        public override bool CanExecute()
        {
            Abort = false;
            IsProcessing = false;

            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;

            if (DataManager.Instance.SET_INT_DATA(IO_T_HOME_STOP, 1))
            {
                Thread.Sleep(1000);
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
                        AlarmManager.Instance.SetAlarm(ALARM_T_AXIS_HOMESTOP_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (DataManager.Instance.GET_INT_DATA(IO_T_IS_HOMMING, out result) == 0)
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
                AlarmManager.Instance.SetAlarm(ALARM_T_AXIS_HOMESTOP_FAIL);              
                return this.F_RESULT_FAIL;
            }
        }

        public override void ExecuteWhenSimulate()
        {
            DataManager.Instance.SET_INT_DATA(IO_T_IS_HOMMING, 0);
        }

        public override void PostExecute()
        {
            Abort = false;
            IsProcessing = false;
            if (EquipmentSimulation == OperationMode.SIMULATION.ToString())
            {
                DataManager.Instance.SET_INT_DATA(IO_T_HOME_STOP, 0);
            }
        }
    }
}
