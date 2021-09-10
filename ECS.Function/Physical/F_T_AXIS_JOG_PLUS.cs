using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INNO6.Core.Function;
using INNO6.IO;
using ECS.Function;
using System.Diagnostics;
using System.Threading;
using INNO6.Core.Manager;

namespace ECS.Function.Physical
{
    
    public class F_T_AXIS_JOG_PLUS : AbstractFunction
    {
        private const string IO_T_JOG_PLUS = "oPMAC.iAxisT.JogFwd";

        private const string ALARM_T_AXIS_JOG_PLUS_FAIL = "E2016";

        private const string VIO_T_JOG_SPEED_MODE = "vSet.sAxisT.JogVelMode";
        private const string VIO_T_JOG_SPEED_HIGH = "vSet.dAxisT.JogVelHigh";
        private const string VIO_T_JOG_SPEED_LOW = "vSet.dAxisT.JogVelLow";

        private const string IO_T_JOG_VELOCITY_SET = "oPMAC.dAxisT.JogVel";

        public override bool CanExecute()
        {
            return this.EquipmentStatusCheck();
        }

        public override string Execute()
        {
            if (DataManager.Instance.GET_STRING_DATA(VIO_T_JOG_SPEED_MODE, out bool _) == "HIGH")
            {
                double velocity = DataManager.Instance.GET_DOUBLE_DATA(VIO_T_JOG_SPEED_HIGH, out bool _);
                DataManager.Instance.SET_DOUBLE_DATA(IO_T_JOG_VELOCITY_SET, velocity);
            }
            else
            {
                double velocity = DataManager.Instance.GET_DOUBLE_DATA(VIO_T_JOG_SPEED_LOW, out bool _);
                DataManager.Instance.SET_DOUBLE_DATA(IO_T_JOG_VELOCITY_SET, velocity);
            }

            if (DataManager.Instance.SET_INT_DATA(IO_T_JOG_PLUS, 1))
            {
                return this.F_RESULT_SUCCESS;
            }
            else
            {             
                AlarmManager.Instance.SetAlarm(ALARM_T_AXIS_JOG_PLUS_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void PostExecute()
        {
            DataManager.Instance.SET_INT_DATA(IO_T_JOG_PLUS, 0);    
        }
    }
}
