using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INNO6.Core.Manager;
using INNO6.IO;
using ECS.Common.Helper;
using ECS.Recipe;
using ECS.Recipe.Model;
using ECS.Recipe.Comparer;
using System.Threading;

namespace ECS.Function.Operation
{
    public class OP_AUTO_PROCESS : AbstractFunction
    {
        public override bool CanExecute()
        {
            bool result = true;
            ProgressRate = 0;
            ProcessingMessage = "Start Processing...";
            result &= base.CanExecute();
            string mode = DataManager.Instance.GET_STRING_DATA(IoNameHelper.V_STR_SYS_OPERATION_MODE, out _);

            if (mode == "AUTO") result = true;

            return result;
        }

        public override string Execute()
        {
            bool result = true;
            int progress = 0;
            string processMessage = "";

            double process_x = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_X_PROCESS_POSITION, out _);
            double process_y = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Y_PROCESS_POSITION, out _);
            double process_z = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Z_PROCESS_POSITION, out _);
            double process_r = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_R_PROCESS_POSITION, out _);
            double process_t = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_T_PROCESS_POSITION, out _);

            List<RECIPE_STEP> stepList = RecipeManager.Instance.GET_RECIPE_STEP_LIST();

            stepList.Sort(new RecipeStepIdComparer());

            progress = 100 / stepList.Count;

            foreach (RECIPE_STEP step in stepList)
            {

                processMessage = string.Format("Processing... STEP ID={0}", step.STEP_ID);

                double xPosition = process_x + step.X_POS;
                double yPosition = process_y + step.Y_POS;
                double zPosition = process_z + step.Z_POS;
                double rPosition = process_r + step.R_POS;
                double tPosition = process_t + step.T_POS;

                result &= DataManager.Instance.SET_INT_DATA(IoNameHelper.V_INT_GUI_CURRENT_STEPID, step.STEP_ID);

                result &= DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.OUT_DBL_RTC_LASER_PROCESS_POWER_PERCENT, step.POWER_PERCENT);
                result &= DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_X_ABS_POSITION, xPosition);
                result &= DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Y_ABS_POSITION, yPosition);
                result &= DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_Z_ABS_POSITION, zPosition);
                result &= DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_R_ABS_POSITION, rPosition);
                result &= DataManager.Instance.SET_DOUBLE_DATA(IoNameHelper.V_DBL_SET_T_ABS_POSITION, tPosition);

                result &= DataManager.Instance.SET_STRING_DATA(IoNameHelper.V_STR_SET_SCAN_DOCUMENT_FILEPATH, step.SCAN_FILE);
                result &= DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_RTC_SCAN_PROCESS_REPEAT, step.REPEAT);

                if (result)
                {
                    StopWatch.Restart();

                    FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.X_AXIS_MOVE_TO_SETPOS);
                    FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.Y_AXIS_MOVE_TO_SETPOS);
                    FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.Z_AXIS_MOVE_TO_SETPOS);
                    FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.R_AXIS_MOVE_TO_SETPOS);
                    FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.T_AXIS_MOVE_TO_SETPOS);
                    {
                        while(true)
                        {
                            Thread.Sleep(100);

                            if (Abort)
                            {
                                FunctionManager.Instance.ABORT_FUNCTION(FuncNameHelper.MOVE_PROCESS_POSITION);
                                return F_RESULT_ABORT;
                            }
                            else if(!FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(FuncNameHelper.X_AXIS_MOVE_TO_SETPOS)
                                    && !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(FuncNameHelper.Y_AXIS_MOVE_TO_SETPOS)
                                    && !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(FuncNameHelper.Z_AXIS_MOVE_TO_SETPOS)
                                    && !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(FuncNameHelper.R_AXIS_MOVE_TO_SETPOS)
                                    && !FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(FuncNameHelper.T_AXIS_MOVE_TO_SETPOS))
                            {
                                ProgressUpdate(progress/2, processMessage);
                                break;
                            }
                        }
                    }

                    FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.SCAN_PROCDOC_START);
                    {
                        while (true)
                        {
                            Thread.Sleep(100);

                            if (Abort)
                            {
                                FunctionManager.Instance.ABORT_FUNCTION(FuncNameHelper.SCAN_PROCDOC_START);
                                return F_RESULT_ABORT;
                            }
                            else if (!FunctionManager.Instance.CHECK_EXECUTING_FUNCTION_EXSIST(FuncNameHelper.SCAN_PROCDOC_START))
                            {
                                ProgressUpdate(progress/2 , processMessage);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    break;
                }
            }

            if(result)
            {
                return F_RESULT_SUCCESS;
            }
            else
            {
                return F_RESULT_FAIL;
            }
        }

        public override void PostExecute()
        {
            this.ProgressRate = 100;
        }
    }
}
