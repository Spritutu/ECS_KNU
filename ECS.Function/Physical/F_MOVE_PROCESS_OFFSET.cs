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
    public class F_MOVE_PROCESS_OFFSET : AbstractFunction
    {
        private string ALARM_Z_POSITION_WARNING = "E5002";
        private string VIO_PROCESS_POSITION_X = "vSet.dAxisX.ProcessPosition";
        private string VIO_PROCESS_POSITION_Y = "vSet.dAxisY.ProcessPosition";
        private string VIO_PROCESS_POSITION_Z = "vSet.dAxisZ.ProcessPosition";
        private string VIO_PROCESS_POSITION_T = "vSet.dAxisT.ProcessPosition";
        private string VIO_PROCESS_POSITION_R = "vSet.dAxisR.ProcessPosition";

        private string VIO_VISION_POSITION_X = "vSet.dAxisX.VisionPosition";
        private string VIO_VISION_POSITION_Y = "vSet.dAxisY.VisionPosition";
        private string VIO_VISION_POSITION_Z = "vSet.dAxisZ.VisionPosition";
        private string VIO_VISION_POSITION_T = "vSet.dAxisT.VisionPosition";
        private string VIO_VISION_POSITION_R = "vSet.dAxisR.VisionPosition";

        private const string VIO_DBL_X_REL_DISTANCE = "vSet.dAxisX.RelDistance";
        private const string VIO_DBL_X_REL_VELOCITY = "vSet.dAxisX.RelVelocity";
        private const string VIO_DBL_Y_REL_DISTANCE = "vSet.dAxisY.RelDistance";
        private const string VIO_DBL_Y_REL_VELOCITY = "vSet.dAxisY.RelVelocity";
        private const string VIO_DBL_Z_REL_DISTANCE = "vSet.dAxisZ.RelDistance";
        private const string VIO_DBL_Z_REL_VELOCITY = "vSet.dAxisZ.RelVelocity";
        private const string VIO_DBL_T_REL_DISTANCE = "vSet.dAxisT.RelDistance";
        private const string VIO_DBL_T_REL_VELOCITY = "vSet.dAxisT.RelVelocity";
        private const string VIO_DBL_R_REL_DISTANCE = "vSet.dAxisR.RelDistance";
        private const string VIO_DBL_R_REL_VELOCITY = "vSet.dAxisR.RelVelocity";

        private const string VIO_DBL_Z_ABS_POSITION = "vSet.dAxisZ.AbsPosition";

        private string F_X_AXIS_MOVE_TO_SETDIS = "F_X_AXIS_MOVE_TO_SETDIS";
        private string F_Y_AXIS_MOVE_TO_SETDIS = "F_Y_AXIS_MOVE_TO_SETDIS";
        private string F_Z_AXIS_MOVE_TO_SETDIS = "F_Z_AXIS_MOVE_TO_SETDIS";
        private string F_T_AXIS_MOVE_TO_SETDIS = "F_T_AXIS_MOVE_TO_SETDIS";
        private string F_R_AXIS_MOVE_TO_SETDIS = "F_R_AXIS_MOVE_TO_SETDIS";

        private string F_Z_AXIS_MOVE_TO_SETPOS = "F_Z_AXIS_MOVE_TO_SETPOS";

        private double _VisionPosZ;

        public override bool CanExecute()
        {
            bool result = true;
            IsAbort = false;
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
            double visionPosX = DataManager.Instance.GET_DOUBLE_DATA(VIO_VISION_POSITION_X, out bool _);
            double visionPosY = DataManager.Instance.GET_DOUBLE_DATA(VIO_VISION_POSITION_Y, out bool _);
            double visionPosZ = DataManager.Instance.GET_DOUBLE_DATA(VIO_VISION_POSITION_Z, out bool _);
            double visionPosT = DataManager.Instance.GET_DOUBLE_DATA(VIO_VISION_POSITION_T, out bool _);
            double visionPosR = DataManager.Instance.GET_DOUBLE_DATA(VIO_VISION_POSITION_R, out bool _);

            double processPosX = DataManager.Instance.GET_DOUBLE_DATA(VIO_PROCESS_POSITION_X, out bool _);
            double processPosY = DataManager.Instance.GET_DOUBLE_DATA(VIO_PROCESS_POSITION_Y, out bool _);
            double processPosZ = DataManager.Instance.GET_DOUBLE_DATA(VIO_PROCESS_POSITION_Z, out bool _);
            double processPosT = DataManager.Instance.GET_DOUBLE_DATA(VIO_PROCESS_POSITION_T, out bool _);
            double processPosR = DataManager.Instance.GET_DOUBLE_DATA(VIO_PROCESS_POSITION_R, out bool _);

            double dffsetX = processPosX - visionPosX;
            double offsetY = processPosY - visionPosY;
            double offsetZ = processPosZ - visionPosZ;
            double offsetT = processPosT - visionPosT;
            double offsetR = processPosR - visionPosR;

            DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_X_REL_DISTANCE, dffsetX);
            DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_Y_REL_DISTANCE, offsetY);
            DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_Z_REL_DISTANCE, offsetZ);
            DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_T_REL_DISTANCE, offsetT);
            DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_R_REL_DISTANCE, offsetR);

            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_X_AXIS_MOVE_TO_SETDIS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_Y_AXIS_MOVE_TO_SETDIS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_R_AXIS_MOVE_TO_SETDIS);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_T_AXIS_MOVE_TO_SETDIS);

            Stopwatch stopwatch = Stopwatch.StartNew();

            while (true)
            {
                Thread.Sleep(100);

                if (IsAbort)
                {
                    return F_RESULT_ABORT;
                }
                else if (stopwatch.ElapsedMilliseconds > TimeoutMiliseconds)
                {
                    return this.F_RESULT_TIMEOUT;
                }
                else if (!FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(F_X_AXIS_MOVE_TO_SETDIS) &&
                    !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(F_Y_AXIS_MOVE_TO_SETDIS) &&
                    !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(F_R_AXIS_MOVE_TO_SETDIS) &&
                    !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(F_T_AXIS_MOVE_TO_SETDIS)
                    )
                {
                    DataManager.Instance.SET_DOUBLE_DATA(VIO_DBL_Z_ABS_POSITION, _VisionPosZ);
                    FunctionManager.Instance.EXECUTE_FUNCTION_SYNC(F_Z_AXIS_MOVE_TO_SETPOS);
                    FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_Z_AXIS_MOVE_TO_SETDIS);
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

