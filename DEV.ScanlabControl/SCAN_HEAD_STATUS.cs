using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEV.ScanlabControl
{
    public class SCAN_HEAD_STATUS
    {
        public static int OK = 1;
        public static int NOTOK = 0;
        public static int BUSY = 1;
        public static int NOTBUSY = 0;
        public static int ERROR = 1;
        public static int NO_ERROR = 0;


        public int BusyStatus { get; set; }
        public int PowerOk { get; set; }
        public int PositionAckOk { get; set; }
        public int ErrorStatus { get; set; }
        public int TempOk { get; set; }

        public SCAN_HEAD_STATUS()
        {
            BusyStatus = NOTBUSY;
            PowerOk = OK;
            PositionAckOk = OK;
            ErrorStatus = NO_ERROR;
            TempOk = OK;
        }
    }
}
