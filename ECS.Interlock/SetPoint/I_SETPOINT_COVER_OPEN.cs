using INNO6.Core.Interlock.Interface;
using INNO6.Core.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.Interlock.SetPoint
{
    public class I_SETPOINT_COVER_OPEN : IExecuteInterlock
    {

        public bool Execute(object setValue)
        {
            //GAS DETECTOR ALARM 발생시 할일 정의
            AlarmManager.Instance.SetAlarm("E1005");

            return true;
        }
    }
}
