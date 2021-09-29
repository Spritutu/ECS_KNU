using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INNO6.Core.Function;
using INNO6.IO;
using ECS.Function;
using System.Diagnostics;
using System.Threading;
using INNO6.Core.Manager;
using ECS.Common.Helper;

namespace ECS.Function.Physical
{
    public class F_R_AXIS_MOVE_TO_SETPOS : AbstractFunction
    {
        private const string IO_R_MOVE_TO_SETPOS = "oPMAC.iAxisR.MoveToSetPos";
        private const string IO_R_IS_MOVING = "iPMAC.iAxisR.IsMoving";

        private const string IO_DBL_R_SET_POSITION = "oPMAC.dAxisR.SetPosition";
        private const string IO_DBL_R_SET_VELOCITY = "oPMAC.dAxisR.SetVelocity";

        private const string VIO_DBL_R_ABS_POSITION = "vSet.dAxisR.AbsPosition";
        private const string VIO_DBL_R_ABS_VELOCITY = "vSet.dAxisR.AbsVelocity";

        private const string ALARM_R_AXIS_MOVE_TIMEOUT = "E2008";
        private const string ALARM_R_AXIS_MOVE_FAIL = "E2009";

        private const string IO_GET_R_POSITION = "iPMAC.dAxisR.Position";
        private const string VIO_DBL_R_INPOS_RANGE = "vSet.dAxisR.InPosRange";

        double setPosition;
        double setVelocity;

        public override bool CanExecute()
        {
            IsAbort = false;
            IsProcessing = false;
            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;

            setPosition = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_R_ABS_POSITION, out bool _);
            setVelocity = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_R_ABS_VELOCITY, out bool _);

            DataManager.Instance.SET_DOUBLE_DATA(IO_DBL_R_SET_POSITION, setPosition);
            DataManager.Instance.SET_DOUBLE_DATA(IO_DBL_R_SET_VELOCITY, setVelocity);

            Thread.SpinWait(500);

            if (DataManager.Instance.SET_INT_DATA(IO_R_MOVE_TO_SETPOS, 1))
            {
                Thread.Sleep(1000);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                IsProcessing = true;

                while (true)
                {
                    Thread.Sleep(100);

                    if (IsAbort)
                    {
                        return F_RESULT_ABORT;
                    }
                    else if (stopwatch.ElapsedMilliseconds > TimeoutMiliseconds)
                    {
                        AlarmManager.Instance.SetAlarm(ALARM_R_AXIS_MOVE_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (InPosition(setPosition))
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
                AlarmManager.Instance.SetAlarm(ALARM_R_AXIS_MOVE_FAIL);

                return this.F_RESULT_FAIL;
            }
        }

        private bool InPosition(double targetPos)
        {
            double curPos = DataManager.Instance.GET_DOUBLE_DATA(IO_GET_R_POSITION, out bool _);
            double inPosRange = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_R_INPOS_RANGE, out bool _);

            double highLimit = targetPos + inPosRange;
            double lowLimit = targetPos - inPosRange;

            if (highLimit >= curPos && lowLimit <= curPos) return true;
            else return false;
        }
        public override void ExecuteWhenSimulate()
        {
            double setPosition = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_R_ABS_POSITION, out bool _);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_R_POSITION, setPosition);
        }

        public override void PostExecute()
        {
            IsAbort = false;
            IsProcessing = false;

            if (EquipmentSimulation == OperationMode.SIMULATION.ToString())
            {
                DataManager.Instance.SET_INT_DATA(IO_R_MOVE_TO_SETPOS, 0);
            }
        }
    }
}
