using ECS.Common.Helper;
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
    public class F_MOVE_PROCESS_POSITION : AbstractFunction
    {
        private double _VisionPosZ;
        public override bool CanExecute()
        {
            bool result = true;
            Abort = false;
            IsProcessing = false;

            //_VisionPosZ = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_Z_POSITION, out bool _);

            //if (_VisionPosZ > 200)
            //{
            //    DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.OUT_DBL_PMAC_Z_SETPOSITION, 200);

            //    if (FunctionManager.Instance.EXECUTE_FUNCTION_SYNC(FuncNameHelper.Z_AXIS_MOVE_TO_SETPOS) == F_RESULT_SUCCESS)
            //    {
            //        result &= true;
            //    }
            //    else
            //    {
            //        result &= false;
            //    }
            //}

            return this.EquipmentStatusCheck();
        }
        public override string Execute()
        {
            double processPosX = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_X_PROCESS_POSITION, out _);
            double processPosY = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Y_PROCESS_POSITION, out _);
            double processPosZ = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Z_PROCESS_POSITION, out _);
            double processPosT = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_T_PROCESS_POSITION, out _);
            double processPosR = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_R_PROCESS_POSITION, out _);

            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_X_ABS_POSITION, processPosX);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Y_ABS_POSITION, processPosY);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Z_ABS_POSITION, processPosZ);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_R_ABS_POSITION, processPosR);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_T_ABS_POSITION, processPosT);

            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.X_AXIS_MOVE_TO_SETPOS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.Y_AXIS_MOVE_TO_SETPOS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.R_AXIS_MOVE_TO_SETPOS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.T_AXIS_MOVE_TO_SETPOS);

            Stopwatch stopwatch = Stopwatch.StartNew();

            while (true)
            {
                Thread.Sleep(100);

                if (Abort)
                {
                    return F_RESULT_ABORT;
                }
                else if (stopwatch.ElapsedMilliseconds > TimeoutMiliseconds)
                {
                    return this.F_RESULT_TIMEOUT;
                }
                else if (!FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(FuncNameHelper.X_AXIS_MOVE_TO_SETPOS)
                         && !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(FuncNameHelper.Y_AXIS_MOVE_TO_SETPOS)
                         && !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(FuncNameHelper.R_AXIS_MOVE_TO_SETPOS)
                         && !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(FuncNameHelper.T_AXIS_MOVE_TO_SETPOS)
                    )
                {
                    FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.Z_AXIS_MOVE_TO_SETPOS);

                    return this.F_RESULT_SUCCESS;
                }
                else if (EquipmentSimulation == OperationMode.SIMULATION.ToString())
                {

                }
                else
                {
                    IsProcessing = true;
                    continue;
                }
            }
        }

        public override void PostExecute()
        {

        }
    }
}
