using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DEV.LEDControl
{
    enum CHANNEL_STATUS
    {
        OFF = 0,
        ON = 1,
    }

    public class Channel
    {
        public int Channel_Number { get; set; }
        public int Data { get; set; }
        public int OnOff_Status { get; set; }
        public bool IsError { get; set; }

        public Channel(int channel_number, int data, int onoff_status, bool isError)
        {
            Channel_Number = channel_number;
            Data = data;
            OnOff_Status = onoff_status;
            IsError = isError;
        }
    }
}
