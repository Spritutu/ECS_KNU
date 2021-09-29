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
    public class F_LASER_REQUEST : AbstractFunction
    {
        private const string ALARM_LASER_REQUEST_TIMEOUT = "E3007";
        private const string ALARM_LASER_REQUEST_FAIL = "E3008";

        private const string DO_NAME_REQUEST_LASER = "oPMAC.iLaser.RequestLaser";
        private const string DO_NAME_LASER_STANDBY = "oPMAC.iLaser.Standby";

        private const string DI_NAME_LASER_ASSIGNED = "iPMAC.iLaser.Assigned";
        private const string DI_NAME_LASER_READY = "iPMAC.iLaser.Ready";
        public override bool CanExecute()
        {
            IsAbort = false;
            IsProcessing = false;

            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;

            if (DataManager.Instance.SET_INT_DATA(DO_NAME_REQUEST_LASER, 1) && DataManager.Instance.SET_INT_DATA(DO_NAME_LASER_STANDBY, 1))
            {
                Thread.Sleep(1000);
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

                        AlarmManager.Instance.SetAlarm(ALARM_LASER_REQUEST_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if ((DataManager.Instance.GET_INT_DATA(DI_NAME_LASER_ASSIGNED, out result) == 1) && 
                              (DataManager.Instance.GET_INT_DATA(DI_NAME_LASER_READY, out result) == 1))
                    {
                        return this.F_RESULT_SUCCESS;
                    }
                    else if(EquipmentSimulation == OperationMode.SIMULATION.ToString())
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
                AlarmManager.Instance.SetAlarm(ALARM_LASER_REQUEST_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void ExecuteWhenSimulate()
        {
            DataManager.Instance.SET_INT_DATA(DI_NAME_LASER_ASSIGNED, 1);
            DataManager.Instance.SET_INT_DATA(DI_NAME_LASER_READY, 1);
        }

        public override void PostExecute()
        {
            if (EquipmentSimulation == OperationMode.SIMULATION.ToString())
            {
                DataManager.Instance.SET_INT_DATA(DO_NAME_REQUEST_LASER, 0);
                DataManager.Instance.SET_INT_DATA(DO_NAME_LASER_STANDBY, 0);
            }
        }
    }
}
