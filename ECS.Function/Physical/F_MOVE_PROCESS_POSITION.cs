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
            IsAbort = false;
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
            double x_offset = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_X_OFFSET, out _);
            double y_offset = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Y_OFFSET, out _);
            double z_offset = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Z_OFFSET, out _);
            double r_offset = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_R_OFFSET, out _);
            double t_offset = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_T_OFFSET, out _);

            double current_pos_x = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_X_POSITION, out _);
            double current_pos_y = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_Y_POSITION, out _);
            double current_pos_z = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_Z_POSITION, out _);
            double current_pos_r = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_R_POSITION, out _);
            double current_pos_t = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_T_POSITION, out _);

            double process_x = current_pos_x + x_offset;
            double process_y = current_pos_y + y_offset;
            double process_z = current_pos_z + z_offset;
            double process_r = current_pos_r + r_offset;
            double process_t = current_pos_t + t_offset;

            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_X_ABS_POSITION, process_x);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Y_ABS_POSITION, process_y);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Z_ABS_POSITION, process_z);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_R_ABS_POSITION, process_r);
            DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_T_ABS_POSITION, process_t);

            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.X_AXIS_MOVE_TO_SETPOS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.Y_AXIS_MOVE_TO_SETPOS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.R_AXIS_MOVE_TO_SETPOS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.T_AXIS_MOVE_TO_SETPOS);

            Stopwatch stopwatch = Stopwatch.StartNew();

            while (true)
            {
                Thread.Sleep(100);

                if (IsAbort)
                {
                    FunctionManager.Instance.ABORT_FUNCTION(FuncNameHelper.X_AXIS_MOVE_TO_SETPOS);
                    FunctionManager.Instance.ABORT_FUNCTION(FuncNameHelper.Y_AXIS_MOVE_TO_SETPOS);
                    FunctionManager.Instance.ABORT_FUNCTION(FuncNameHelper.R_AXIS_MOVE_TO_SETPOS);
                    FunctionManager.Instance.ABORT_FUNCTION(FuncNameHelper.T_AXIS_MOVE_TO_SETPOS);
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
