using ECS.Common.Helper;
using INNO6.Core.Interlock.Interface;
using INNO6.Core.Manager;
using INNO6.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Interlock.SetPoint
{
    /// <summary>
    /// INTERLOCK
    /// SETPOINT
    /// EMO STOP
    /// </summary>
    public class I_SETPOINT_EMO_STOP : IExecuteInterlock
    {
        public void Execute()
        {
            // emergency stop alarm activative
            

            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.LASER_STOP);
            FunctionManager.Instance.ABORT_FUNCTION_ALL();
            AlarmManager.Instance.SetAlarm("E9002");
            DataManager.Instance.SET_INT_DATA(IoNameHelper.V_INT_SYS_EQP_INTERLOCK, 1);            
        }
    }
}
