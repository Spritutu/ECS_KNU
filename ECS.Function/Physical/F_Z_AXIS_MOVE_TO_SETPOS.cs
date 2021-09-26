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
    public class F_Z_AXIS_MOVE_TO_SETPOS : AbstractFunction
    {
        private const string IO_Z_MOVE_TO_SETPOS = "oPMAC.iAxisZ.MoveToSetPos";
        private const string IO_Z_IS_MOVING = "iPMAC.iAxisZ.IsMoving";

        private const string IO_DBL_Z_SET_POSITION = "oPMAC.dAxisZ.SetPosition";
        private const string IO_DBL_Z_SET_VELOCITY = "oPMAC.dAxisZ.SetVelocity";

        private const string VIO_DBL_Z_ABS_POSITION = "vSet.dAxisZ.AbsPosition";
        private const string VIO_DBL_Z_ABS_VELOCITY = "vSet.dAxisZ.AbsVelocity";

        private const string ALARM_Z_AXIS_MOVE_TIMEOUT = "E2048";
        private const string ALARM_Z_AXIS_MOVE_FAIL = "E2049";


        private const string IO_GET_Z_POSITION = "iPMAC.dAxisZ.Position";
        private const string VIO_DBL_Z_INPOS_RANGE = "vSet.dAxisZ.InPosRange";

        public override bool CanExecute()
        {
            Abort = false;
            IsProcessing = false;
            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;

            double setPosition = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_Z_ABS_POSITION, out bool _);
            double setVelocity = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_Z_ABS_VELOCITY, out bool _);

            DataManager.Instance.SET_DOUBLE_DATA(IO_DBL_Z_SET_POSITION, setPosition);
            DataManager.Instance.SET_DOUBLE_DATA(IO_DBL_Z_SET_VELOCITY, setVelocity);

            Thread.SpinWait(500);

            if (DataManager.Instance.SET_INT_DATA(IO_Z_MOVE_TO_SETPOS, 1))
            {
                Thread.Sleep(100);
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                IsProcessing = true;

                while (true)
                {
                    Thread.Sleep(100);

                    if (Abort)
                    {
                        DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_Z_JOGSTOP, 1);
                        return F_RESULT_ABORT;
                    }
                    else if (stopwatch.ElapsedMilliseconds > TimeoutMiliseconds)
                    {
                        DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_Z_JOGSTOP, 1);
                        AlarmManager.Instance.SetAlarm(ALARM_Z_AXIS_MOVE_TIMEOUT);
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
                AlarmManager.Instance.SetAlarm(ALARM_Z_AXIS_MOVE_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        private bool InPosition(double targetPos)
        {
            double curPos = DataManager.Instance.GET_DOUBLE_DATA(IO_GET_Z_POSITION, out bool _);
            double inPosRange = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_Z_INPOS_RANGE, out bool _);

            double highLimit = targetPos + inPosRange;
            double lowLimit = targetPos - inPosRange;

            if (highLimit >= curPos && lowLimit <= curPos) return true;
            else return false;
        }

        public override void ExecuteWhenSimulate()
        {
            double setPosition = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Z_ABS_POSITION, out bool _);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_Z_POSITION, setPosition);
        }

        public override void PostExecute()
        {
            Abort = false;
            IsProcessing = false;
            DataManager.Instance.SET_INT_DATA(IO_Z_MOVE_TO_SETPOS, 0);
        }
    }
}
