using INNO6.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEV.MotionControl
{
    public partial class PMAC
    {
        private bool GetDigitalValue(string address, ref int onOff)
        {
            StringBuilder strRequest = new StringBuilder();
            string strResponse = "";
            int result = 0;

            strRequest.AppendFormat("{0}", address);

            CommandOrQuery(strRequest.ToString(), out strResponse);

            if (int.TryParse(strResponse, out onOff))
            {
                if (_isWriteLog) LogHelper.Instance.DeviceLog.DebugFormat("[SUCCESS] GetDigitalValue() : SendMessage={0}, ResponseMessage={1}", strRequest, strResponse);
                return true;
            }
            else
            {
                LogHelper.Instance.DeviceLog.DebugFormat("[ERROR] GetDigitalValue() : SendMessage={0}, ResponseMessage={1}", strRequest, strResponse);
                return false;
            }
        }

        private bool SetDigitalValue(string address, int setOnOff)
        {
            StringBuilder strRequest = new StringBuilder();
            string strResponse = "";


            strRequest.AppendFormat("{0}={1}", address, setOnOff);

            CommandOrQuery(strRequest.ToString(), out strResponse);

            if (string.IsNullOrEmpty(strResponse))
            {
                if (_isWriteLog) LogHelper.Instance.DeviceLog.DebugFormat("[SUCCESS] SetDigitalValue() : SendMessage={0}, ResponseMessage={1}", strRequest, strResponse);
                return true;
            }
            else
            {
                LogHelper.Instance.DeviceLog.DebugFormat("[ERROR] SetDigitalValue() : SendMessage={0}, ResponseMessage={1}", strRequest, strResponse);
                return false;
            }
        }
    }
}
