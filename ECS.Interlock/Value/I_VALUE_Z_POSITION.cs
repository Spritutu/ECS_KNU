using INNO6.Core.Interlock.Interface;
using INNO6.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INNO6.Core;
using INNO6.IO;
using ECS.Common.Helper;

namespace ECS.Interlock.Value
{
    public class I_VALUE_Z_POSITION : IExecuteInterlock
    {

        public bool Execute(object setValue)
        {
            FunctionManager.Instance.ABORT_FUNCTION_ALL();
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(FuncNameHelper.Z_AXIS_SERVO_STOP);
            DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_ALL_MOTION_ABORT, 1);
            DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_ALL_SERVOKILL, 1);
            AlarmManager.Instance.SetAlarm(AlarmCodeHelper.STAGE_Z_AXIS_POSITION_LIMIT);

            return true;
        }
    }
}
