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
    public class F_T_AXIS_MOVE_TO_SETPOS : AbstractFunction
    {
        private const string IO_T_MOVE_TO_SETPOS = "oPMAC.iAxisT.MoveToSetPos";
        private const string IO_T_IS_MOVING = "iPMAC.iAxisT.IsMoving";

        private const string IO_DBL_T_SET_POSITION = "oPMAC.dAxisT.SetPosition";
        private const string IO_DBL_T_SET_VELOCITY = "oPMAC.dAxisT.SetVelocity";

        private const string VIO_DBL_T_ABS_POSITION = "vSet.dAxisT.AbsPosition";
        private const string VIO_DBL_T_ABS_VELOCITY = "vSet.dAxisT.AbsVelocity";

        private const string ALARM_T_AXIS_MOVE_TIMEOUT = "E2018";
        private const string ALARM_T_AXIS_MOVE_FAIL = "E2019";

        private const string IO_GET_T_POSITION = "iPMAC.dAxisT.Position";
        private const string VIO_DBL_T_INPOS_RANGE = "vSet.dAxisT.InPosRange";


        private double setPosition;
        private double setVelocity;

        public override bool CanExecute()
        {
            IsAbort = false;
            IsProcessing = false;
            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;

            setPosition = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_T_ABS_POSITION, out bool _);
            setVelocity = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_T_ABS_VELOCITY, out bool _);

            DataManager.Instance.SET_DOUBLE_DATA(IO_DBL_T_SET_POSITION, setPosition);
            DataManager.Instance.SET_DOUBLE_DATA(IO_DBL_T_SET_VELOCITY, setVelocity);

            Thread.SpinWait(500);

            if (DataManager.Instance.SET_INT_DATA(IO_T_MOVE_TO_SETPOS, 1))
            {
                Thread.Sleep(500);
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
                        IsProcessing = false;
                        DataManager.Instance.SET_INT_DATA(IO_T_MOVE_TO_SETPOS, 0);
                        AlarmManager.Instance.SetAlarm(ALARM_T_AXIS_MOVE_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (InPosition(setPosition))
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
                AlarmManager.Instance.SetAlarm(ALARM_T_AXIS_MOVE_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void ExecuteWhenSimulate()
        {
            double setPosition = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_T_ABS_POSITION, out bool _);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_T_POSITION, setPosition);
        }

        private bool InPosition(double targetPos)
        {
            double curPos = DataManager.Instance.GET_DOUBLE_DATA(IO_GET_T_POSITION, out bool _);
            double inPosRange = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_T_INPOS_RANGE, out bool _);

            double highLimit = targetPos + inPosRange;
            double lowLimit = targetPos - inPosRange;

            if (highLimit >= curPos && lowLimit <= curPos) return true;
            else return false;
        }

        public override void PostExecute()
        {
            IsAbort = false;
            IsProcessing = false;

            if (EquipmentSimulation == OperationMode.SIMULATION.ToString())
            {
                DataManager.Instance.SET_INT_DATA(IO_T_MOVE_TO_SETPOS, 0);
            }
        }
    }
}
