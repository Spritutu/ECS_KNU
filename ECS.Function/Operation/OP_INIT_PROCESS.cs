using INNO6.Core.Manager;
using INNO6.IO;
using ECS.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ECS.Function.Operation
{
    public class OP_INIT_PROCESS : AbstractFunction
    {
        public override bool CanExecute()
        {
            ProgressRate = 0;
            ProcessingMessage = "Start Processing...";
            return base.CanExecute();
        }
        public override string Execute()
        {
            string result = F_RESULT_SUCCESS;
            IsProcessing = true;

            //if ((result = FunctionManager.Instance.EXECUTE_FUNCTION_SYNC(FuncNameHelper.LASER_STOP)) != F_RESULT_SUCCESS) return result;
            ProgressUpdate(10, "Laser Init Process Completed");
            // Stage Vision Position Move
            if ((result = FunctionManager.Instance.EXECUTE_FUNCTION_SYNC(FuncNameHelper.MOVE_VISION_POSITION)) != F_RESULT_SUCCESS) return result;
            // Laser Standby
            ProgressUpdate(10, "Vision Position Moving Completed");
            if ((result = FunctionManager.Instance.EXECUTE_FUNCTION_SYNC(FuncNameHelper.LASER_ON)) != F_RESULT_SUCCESS) return result;
            ProgressUpdate(20, "Laser On Process Completed");
            if ((result = FunctionManager.Instance.EXECUTE_FUNCTION_SYNC(FuncNameHelper.LASER_REQUEST)) != F_RESULT_SUCCESS) return result;
            ProgressUpdate(20, "Laser Request Process Completed");
            if ((result = FunctionManager.Instance.EXECUTE_FUNCTION_SYNC(FuncNameHelper.LASER_PROGRAM_START)) != F_RESULT_SUCCESS) return result;
            ProgressUpdate(20, "Laser Program Start Process Completed");
            return result;
        }

        public override void PostExecute()
        {
            if(FunctionResult == F_RESULT_SUCCESS)
            {
                DataManager.Instance.SET_STRING_DATA(IoNameHelper.V_STR_SYS_OPERATION_MODE, "AUTO");
                ProgressUpdate(100, "Init Process Completed");
            }
        }
    }
}
