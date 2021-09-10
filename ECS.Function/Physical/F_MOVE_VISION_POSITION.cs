using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using INNO6.Core.Manager;
using INNO6.IO;

namespace ECS.Function.Physical
{
    public class F_MOVE_VISION_POSITION : AbstractFunction
    {
        private string ALARM_Z_POSITION_WARNING = "E5002";
        private string VIO_VISION_POSITION_X = "vSet.dAxisX.VisionPosition";
        private string VIO_VISION_POSITION_Y = "vSet.dAxisY.VisionPosition";
        private string VIO_VISION_POSITION_Z = "vSet.dAxisZ.VisionPosition";
        private string VIO_VISION_POSITION_T = "vSet.dAxisT.VisionPosition";
        private string VIO_VISION_POSITION_R = "vSet.dAxisR.VisionPosition";

        private const string VIO_DBL_X_ABS_POSITION = "vSet.dAxisX.AbsPosition";
        private const string VIO_DBL_X_ABS_VELOCITY = "vSet.dAxisX.AbsVelocity";
        private const string VIO_DBL_Y_ABS_POSITION = "vSet.dAxisY.AbsPosition";
        private const string VIO_DBL_Y_ABS_VELOCITY = "vSet.dAxisY.AbsVelocity";
        private const string VIO_DBL_Z_ABS_POSITION = "vSet.dAxisZ.AbsPosition";
        private const string VIO_DBL_Z_ABS_VELOCITY = "vSet.dAxisZ.AbsVelocity";
        private const string VIO_DBL_T_ABS_POSITION = "vSet.dAxisT.AbsPosition";
        private const string VIO_DBL_T_ABS_VELOCITY = "vSet.dAxisT.AbsVelocity";
        private const string VIO_DBL_R_ABS_POSITION = "vSet.dAxisR.AbsPosition";
        private const string VIO_DBL_R_ABS_VELOCITY = "vSet.dAxisR.AbsVelocity";

        private string F_X_AXIS_MOVE_TO_SETPOS = "F_X_AXIS_MOVE_TO_SETPOS";
        private string F_Y_AXIS_MOVE_TO_SETPOS = "F_Y_AXIS_MOVE_TO_SETPOS";
        private string F_Z_AXIS_MOVE_TO_SETPOS = "F_Z_AXIS_MOVE_TO_SETPOS";
        private string F_T_AXIS_MOVE_TO_SETPOS = "F_T_AXIS_MOVE_TO_SETPOS";
        private string F_R_AXIS_MOVE_TO_SETPOS = "F_R_AXIS_MOVE_TO_SETPOS";


        private double _VisionPosZ;

        public override bool CanExecute()
        {
            
            bool result = true;
            Abort = false;
            IsProcessing = false;

            _VisionPosZ = DataManager.Instance.GET_DOUBLE_DATA("iPMAC.dAxisZ.Position", out bool _);

            if (_VisionPosZ > 200)
            {
                DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_Z_ABS_POSITION, 200);

                if (FunctionManager.Instance.EXECUTE_FUNCTION_SYNC(F_Z_AXIS_MOVE_TO_SETPOS) == F_RESULT_SUCCESS)
                {
                    result &= true;
                }
                else
                {
                    result &= false;
                }
            }

            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            bool result = false;

            double visionPosX = DataManager.Instance.GET_DOUBLE_DATA(VIO_VISION_POSITION_X, out bool _);
            double visionPosY = DataManager.Instance.GET_DOUBLE_DATA(VIO_VISION_POSITION_Y, out bool _);
            double visionPosZ = DataManager.Instance.GET_DOUBLE_DATA(VIO_VISION_POSITION_Z, out bool _);
            double visionPosT = DataManager.Instance.GET_DOUBLE_DATA(VIO_VISION_POSITION_T, out bool _);
            double visionPosR = DataManager.Instance.GET_DOUBLE_DATA(VIO_VISION_POSITION_R, out bool _);

            DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_X_ABS_POSITION, visionPosX);
            DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_Y_ABS_POSITION, visionPosY);
            DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_Z_ABS_POSITION, visionPosZ);
            DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_T_ABS_POSITION, visionPosT);
            DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_R_ABS_POSITION, visionPosR);

            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_X_AXIS_MOVE_TO_SETPOS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_Y_AXIS_MOVE_TO_SETPOS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_R_AXIS_MOVE_TO_SETPOS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_T_AXIS_MOVE_TO_SETPOS);

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
                else if (!FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(F_X_AXIS_MOVE_TO_SETPOS) &&
                    !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(F_Y_AXIS_MOVE_TO_SETPOS) &&
                    !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(F_R_AXIS_MOVE_TO_SETPOS) &&
                    !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(F_T_AXIS_MOVE_TO_SETPOS)
                    )
                {
                    FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_Z_AXIS_MOVE_TO_SETPOS);
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

        public override void ExecuteWhenSimulate()
        {
            
        }
    }
}

