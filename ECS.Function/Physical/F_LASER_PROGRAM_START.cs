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
    public class F_LASER_PROGRAM_START : AbstractFunction
    {
        private const string ALARM_LASER_PSTART_TIMEOUT = "E3003";
        private const string ALARM_LASER_PSTART_FAIL = "E3004";

        private const string V_NAME_PROGRAM_NO = "vSys.iLaser.ProgramNo";
        private const string DI_NAME_LASER_IS_ON = "iPMAC.iLaser.IsOn";
        private const string DI_NAME_LASER_ASSIGNED = "iPMAC.iLaser.Assigned";
        private const string DI_NAME_LASER_READY = "iPMAC.iLaser.Ready";
        private const string DI_NAME_PROGRAM_ACTIVE = "iPMAC.iLaser.ProgActive";
        private const string DO_NAME_PSTART_STATIC = "oPMAC.iLaser.PStartStatical";
        private const string DO_NAME_PROGRAM_NUMBER = "oPMAC.iLaser.ProgramNo";


        public override bool CanExecute()
        {
            Abort = false;
            IsProcessing = false;
            bool canExecute = this.EquipmentStatusCheck();

            if(DataManager.Instance.GET_INT_DATA(DI_NAME_LASER_ASSIGNED, out _) == 1 &&
                DataManager.Instance.GET_INT_DATA(DI_NAME_LASER_IS_ON, out _) == 1 /*&&
                DataManager.Instance.GET_INT_DATA(DI_NAME_LASER_READY, out _) == 1*/
                )
            {
                canExecute &= true;
            }
            else
            {
                canExecute &= false;
            }

            return canExecute;
        }

        public override string Execute()
        {
            bool result = false;
            int laserProgramNo = DataManager.Instance.GET_INT_DATA(V_NAME_PROGRAM_NO, out _);

            DataManager.Instance.SET_INT_DATA(DO_NAME_PROGRAM_NUMBER, laserProgramNo);

            if (DataManager.Instance.SET_INT_DATA(DO_NAME_PSTART_STATIC, 1))
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

                        AlarmManager.Instance.SetAlarm(ALARM_LASER_PSTART_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (DataManager.Instance.GET_INT_DATA(DI_NAME_PROGRAM_ACTIVE, out result) == 1)
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
                AlarmManager.Instance.SetAlarm(ALARM_LASER_PSTART_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void ExecuteWhenSimulate()
        {
            DataManager.Instance.SET_INT_DATA(DI_NAME_PROGRAM_ACTIVE, 1);
        }

        public override void PostExecute()
        {
            if (EquipmentSimulation == OperationMode.SIMULATION.ToString())
            {
                DataManager.Instance.SET_INT_DATA(DO_NAME_PSTART_STATIC, 0);
            }
        }
    }
}
