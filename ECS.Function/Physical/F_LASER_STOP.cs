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
    public class F_LASER_STOP : AbstractFunction
    {
        private const string ALARM_LASER_STOP_TIMEOUT = "E3009";
        private const string ALARM_LASER_STOP_FAIL = "E3010";

        private const string DO_NAME_EXT_ACTIVATION = "oPMAC.iLaser.ExtActivation";
        private const string DO_NAME_LASER_ON = "oPMAC.iLaser.On";
        private const string DO_NAME_REQUEST_LASER = "oPMAC.iLaser.RequestLaser";
        private const string DO_NAME_PSTART_STATIC = "oPMAC.iLaser.PStartStatical";
        private const string DO_NAME_LASER_STANDBY = "oPMAC.iLaser.Standby";

        private const string DI_NAME_LASER_IS_ON = "iPMAC.iLaser.IsOn";
        private const string DI_NAME_LASER_ASSIGNED = "iPMAC.iLaser.Assigned";
        private const string DI_NAME_LASER_READY = "iPMAC.iLaser.Ready";
        private const string DI_NAME_LASER_PROGACTIVE = "iPMAC.iLaser.ProgActive";

        public override bool CanExecute()
        {
            Abort = false;
            IsProcessing = false;

            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = true;

            result &= DataManager.Instance.SET_INT_DATA(DO_NAME_PSTART_STATIC, 0);
            result &= DataManager.Instance.SET_INT_DATA(DO_NAME_EXT_ACTIVATION, 0);
            result &= DataManager.Instance.SET_INT_DATA(DO_NAME_REQUEST_LASER, 0);
            result &= DataManager.Instance.SET_INT_DATA(DO_NAME_LASER_STANDBY, 0);
            result &= DataManager.Instance.SET_INT_DATA(DO_NAME_LASER_ON, 0);

            if (result)
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

                        AlarmManager.Instance.SetAlarm(ALARM_LASER_STOP_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (((DataManager.Instance.GET_INT_DATA(DI_NAME_LASER_ASSIGNED, out result) == 0) &&
                               (DataManager.Instance.GET_INT_DATA(DI_NAME_LASER_IS_ON, out result) == 0) &&
                              (DataManager.Instance.GET_INT_DATA(DI_NAME_LASER_READY, out result) == 0)))
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
                AlarmManager.Instance.SetAlarm(ALARM_LASER_STOP_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void ExecuteWhenSimulate()
        {
            DataManager.Instance.SET_INT_DATA(DI_NAME_LASER_ASSIGNED, 0);
            DataManager.Instance.SET_INT_DATA(DI_NAME_LASER_IS_ON, 0);
            DataManager.Instance.SET_INT_DATA(DI_NAME_LASER_READY, 0);
            DataManager.Instance.SET_INT_DATA(DI_NAME_LASER_PROGACTIVE, 0);
        }

        public override void PostExecute()
        {
 
        }
    }
}
