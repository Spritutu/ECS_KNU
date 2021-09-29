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

namespace ECS.Function.Physical
{
    public class F_Z_AXIS_MOVE_TO_SETDIS : AbstractFunction
    {
        private const string IO_Z_MOVE_TO_SETDIS = "oPMAC.iAxisZ.MoveToSetDis";
        private const string IO_Z_IS_MOVING = "iPMAC.iAxisZ.IsMoving";

        private const string IO_GET_Z_POSITION = "iPMAC.dAxisZ.Position";
        private const string VIO_DBL_Z_INPOS_RANGE = "vSet.dAxisZ.InPosRange";

        private const string IO_DBL_Z_SET_DISTANCE = "oPMAC.dAxisZ.SetDistance";
        private const string IO_DBL_Z_SET_VELOCITZ = "oPMAC.dAxisZ.SetVelocity";

        private const string VIO_DBL_Z_REL_DISTANCE = "vSet.dAxisZ.RelDistance";
        private const string VIO_DBL_Z_REL_VELOCITZ = "vSet.dAxisZ.RelVelocity";


        private const string ALARM_Z_AXIS_MOVE_TIMEOUT = "E2028";
        private const string ALARM_Z_AXIS_MOVE_FAIL = "E2029";

        private double _TargetPosition;

        public override bool CanExecute()
        {
            IsAbort = false;
            IsProcessing = false;
            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;

            double startPostion = DataManager.Instance.GET_DOUBLE_DATA(IO_GET_Z_POSITION, out bool _);
            double setDistance = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_Z_REL_DISTANCE, out bool _);
            double setVelocity = DataManager.Instance.GET_DOUBLE_DATA(VIO_DBL_Z_REL_VELOCITZ, out bool _);

            _TargetPosition = startPostion + setDistance;

            DataManager.Instance.SET_DOUBLE_DATA(IO_DBL_Z_SET_DISTANCE, setDistance);
            DataManager.Instance.SET_DOUBLE_DATA(IO_DBL_Z_SET_VELOCITZ, setVelocity);

            Thread.SpinWait(500);

            if (DataManager.Instance.SET_INT_DATA(IO_Z_MOVE_TO_SETDIS, 1))
            {
                //Thread.Sleep(1000);
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
                        AlarmManager.Instance.SetAlarm(ALARM_Z_AXIS_MOVE_TIMEOUT);
                        return this.F_RESULT_TIMEOUT;
                    }
                    else if (InPosition(_TargetPosition))
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
            DataManager.Instance.SET_DOUBLE_DATA(IO_GET_Z_POSITION, _TargetPosition);
        }
        public override void PostExecute()
        {
            IsAbort = false;
            IsProcessing = false;
            DataManager.Instance.SET_INT_DATA(IO_Z_MOVE_TO_SETDIS, 0);
        }
    }
}
