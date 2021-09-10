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
    public class F_LASER_ON : AbstractFunction
    {
        private const string ALARM_LASER_INIT_TIMEOUT = "E3001";
        private const string ALARM_LASER_INIT_FAIL = "E3002";

        private const string DO_NAME_EXT_ACTIVATION = "oPMAC.iLaser.ExtActivation";
        private const string DO_NAME_LASER_ON = "oPMAC.iLaser.On";
        private const string DO_NAME_REQUEST_LASER = "oPMAC.iLaser.RequestLaser";

        private const string DI_NAME_LASER_IS_ON = "iPMAC.iLaser.IsOn";
        private const string DI_NAME_LASER_ASSIGNED = "iPMAC.iLaser.Assigned";
        private const string DI_NAME_LASER_READY = "iPMAC.iLaser.Ready";



        public override bool CanExecute()
        {
            Abort = false;
            IsProcessing = false;

            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;

            // TRUMP LASER TruMicro EXT_ACTIVATION signal ON and LASER ON signal ON
            if (DataManager.Instance.SET_INT_DATA(DO_NAME_EXT_ACTIVATION, 1) &&
                DataManager.Instance.SET_INT_DATA(DO_NAME_LASER_ON, 1))
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

                        AlarmManager.Instance.SetAlarm(ALARM_LASER_INIT_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (DataManager.Instance.GET_INT_DATA(DI_NAME_LASER_IS_ON, out result) == 1 )
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
                AlarmManager.Instance.SetAlarm(ALARM_LASER_INIT_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void ExecuteWhenSimulate()
        {
            // EXT_ACTIVATION, LASER_ON Signal에 대한 응답
            // iPMAC.iLaser.IsOn, iPMAC.iLaser.Ready

            DataManager.Instance.SET_INT_DATA(DI_NAME_LASER_IS_ON, 1);
            DataManager.Instance.SET_INT_DATA(DI_NAME_LASER_READY, 1);
        }

        public override void PostExecute()
        {
            //throw new NotImplementedException();
        }
    }
}
