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
using ECS.Common.Helper;

namespace ECS.Function.Physical
{
    public class F_ALL_AXIS_MOVE_STOP : AbstractFunction
    {
        public override bool CanExecute()
        {
            return true;
        }

        public override string Execute()
        {
            bool result = true;
            result &= DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_X_JOGSTOP, 1);
            result &= DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_Y_JOGSTOP, 1);
            result &= DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_Z_JOGSTOP, 1);
            result &= DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_R_JOGSTOP, 1);
            result &= DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_T_JOGSTOP, 1);

            if (result) return F_RESULT_SUCCESS;
            else return F_RESULT_FAIL;
        }

        public override void PostExecute()
        {
            DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_X_JOGSTOP, 0);
            DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_Y_JOGSTOP, 0);
            DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_Z_JOGSTOP, 0);
            DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_R_JOGSTOP, 0);
            DataManager.Instance.SET_INT_DATA(IoNameHelper.OUT_INT_PMAC_T_JOGSTOP, 0);
        }
    }
}
