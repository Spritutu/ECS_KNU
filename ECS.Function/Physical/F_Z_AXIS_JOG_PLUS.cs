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
    public class F_Z_AXIS_JOG_PLUS : AbstractFunction
    {
        private const string IO_Z_JOG_PLUS = "oPMAC.iAxisZ.JogFwd";

        private const string ALARM_Z_AXIS_JOG_PLUS_FAIL = "E2036";

        private const string VIO_Z_JOG_SPEED_MODE = "vSet.sAxisZ.JogVelMode";
        private const string VIO_Z_JOG_SPEED_HIGH = "vSet.dAxisZ.JogVelHigh";
        private const string VIO_Z_JOG_SPEED_LOW = "vSet.dAxisZ.JogVelLow";

        private const string IO_Z_JOG_VELOCITY_SET = "oPMAC.dAxisZ.JogVel";

        public override bool CanExecute()
        {
            return true;
        }

        public override string Execute()
        {
            if (DataManager.Instance.GET_STRING_DATA(VIO_Z_JOG_SPEED_MODE, out bool _) == "HIGH")
            {
                double velocity = DataManager.Instance.GET_DOUBLE_DATA(VIO_Z_JOG_SPEED_HIGH, out bool _);
                DataManager.Instance.SET_DOUBLE_DATA(IO_Z_JOG_VELOCITY_SET, velocity);
            }
            else
            {
                double velocity = DataManager.Instance.GET_DOUBLE_DATA(VIO_Z_JOG_SPEED_LOW, out bool _);
                DataManager.Instance.SET_DOUBLE_DATA(IO_Z_JOG_VELOCITY_SET, velocity);
            }

            if (DataManager.Instance.SET_INT_DATA(IO_Z_JOG_PLUS, 1))
            {
                
                return this.F_RESULT_SUCCESS;
            }
            else
            {
                AlarmManager.Instance.SetAlarm(ALARM_Z_AXIS_JOG_PLUS_FAIL);
                return this.F_RESULT_FAIL;
            }
        }

        public override void PostExecute()
        {
            DataManager.Instance.SET_INT_DATA(IO_Z_JOG_PLUS, 0);
        }
    }
}
