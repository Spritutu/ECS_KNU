﻿using ECS.Common.Helper;
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
        public bool Execute(object setValue)
        {
            // emergency stop alarm activative

            FunctionManager.Instance.ABORT_FUNCTION_ALL();

            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.LASER_STOP);
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.SCAN_PROCESS_ABORT);
            
            DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_ALL_MOTION_ABORT, 1);
            DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_ALL_SERVOKILL, 1);
            AlarmManager.Instance.SetAlarm("E9002");
            DataManager.Instance.SET_INT_DATA(IoNameHelper.V_INT_SYS_EQP_INTERLOCK, 1);

            return true;
        }
    }
}
