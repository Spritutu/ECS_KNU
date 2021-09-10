using INNO6.Core;
using INNO6.Core.Communication;
using INNO6.IO.Interface;
using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using INNO6.Core.Communication.Socket;

namespace DEV.LEDControl
{
    public class ALD4CH64W : XSequence, IDeviceHandler
    {
        #region Define
        public const string COMM_TIMEOUT = "TIMEOUT";
        public const string COMM_SUCCESS = "SUCCESS";
        public const string COMM_DISCONNECT = "DISCONNECT";
        public const string COMM_ERROR = "ERROR";
        public const string COMM_UNKNOWN = "UNKNOWN";

        public const int ERROR_DATA_OUTPUT = -1;


        // ID Define
        public const string ID_1_VIRTUAL = "0";
        public const string ID_1_INPUT = "1";
        public const string ID_1_OUTPUT = "2";

        public const string ID_2_OBJECT = "0";
        public const string ID_2_DOUBLE = "1";
        public const string ID_2_INT = "2";
        public const string ID_2_STRING = "3";

        public const string ID_3_UNKNOWN = "0";
        public const string ID_3_DATA = "1";
        public const string ID_3_STATUS = "2";
        public const string ID_3_BOTH = "3";

        public const string ID_4_CHALL = "0";
        public const string ID_4_CH1 = "1";
        public const string ID_4_CH2 = "2";
        public const string ID_4_CH3 = "3";
        public const string ID_4_CH4 = "4";
        #endregion

        private XSerialComm xSerial;
        private TcpClient tcpClient;
        private string _deviceName;
        private eDevMode _deviceMode;
        private long _milisecondResponseTimeout;
        private bool isDeviceLogging = true;
        private static object _key = new object();

        #region Properties
        private List<Channel> Channels = new List<Channel>();
        private int ledOnOffStatus;
        #endregion

        public ALD4CH64W()
        {
            _deviceName = "";
            _deviceMode = eDevMode.UNKNOWN;
            _milisecondResponseTimeout = 500;
            ledOnOffStatus = 0x00;
            InitializeChannels();         
        }

        private void InitializeChannels()
        {
            lock (_key)
            {
                Channels.Clear();

                Channels.Add(new Channel(1, 0, 0, false));
                Channels.Add(new Channel(2, 0, 0, false));
                Channels.Add(new Channel(3, 0, 0, false));
                Channels.Add(new Channel(4, 0, 0, false));
            }
        }

        public bool DeviceAttach(string deviceName, string portName, string baudRate, string parity, string dataBits, string stopBits, string scanTime, string responsTimeout, string logging = "Y", string mode = "NORMAL")
        {
            this._deviceName = deviceName;
            _milisecondResponseTimeout = int.Parse(responsTimeout);           

            ScanTime = int.Parse(scanTime);
            
            if (!string.IsNullOrEmpty(logging) && logging.Substring(0,1).Equals("Y"))
            {
                isDeviceLogging = true;
            }
            else
            {
                isDeviceLogging = false;
            }

            if (!string.IsNullOrEmpty(mode) && mode.Substring(0,1).Equals("S"))
            {
                _deviceMode = eDevMode.SIMULATE;
                tcpClient = new TcpClient("127.0.0.1", 3001);
                tcpClient.OnConnectedEvent += Emulator_OnConnected;
                tcpClient.OnDisconnectedEvent += Emulator_OnDisconnected;
            }
            else
            {
                xSerial = new XSerialComm(portName, int.Parse(baudRate), (Parity)int.Parse(parity), int.Parse(dataBits), (StopBits)int.Parse(stopBits));
            }


            if (xSerial == null && tcpClient == null)
            {
                LogHelper.Instance.DeviceLog.DebugFormat("[ERROR] DeviceAttach() : DeviceName = {0}, DeviceMode = {1}, Cause = {2}" , _deviceName, _deviceMode.ToString(), portName + " Can not create SerialPort object!");
                _deviceMode = eDevMode.ERROR;
                return false;
            }
            else
            {
                if (_deviceMode == eDevMode.SIMULATE)
                {
                    tcpClient.Connect();

               
                    return true;
                }
                else
                {
                    xSerial.Open();

                    if (xSerial.IsOpen)
                    {
                        ThreadStart();
                        _deviceMode = eDevMode.CONNECT;
                        return true;
                    }
                    else
                    {
                        LogHelper.Instance.DeviceLog.DebugFormat("[ERROR] DeviceAttach() : DeviceName = {0}, DeviceMode = {1}, Cause = {2}", _deviceName, _deviceMode.ToString(), portName + " Can Not Open !");
                        _deviceMode = eDevMode.ERROR;
                        return false;
                    }
                }
            }
        }

        private void Emulator_OnDisconnected(object sender, EventArgs e)
        {
            while (!tcpClient.IsOpen)
            {
                // tcp connection 될 때까지 connection 시도 
                tcpClient.Connect();

                Thread.Sleep(3000);
            }
        }

        private void Emulator_OnConnected(object sender, EventArgs e)
        {
            if (tcpClient.IsOpen)
            {
                ThreadStart();
            }
        }

        public bool DeviceDettach()
        {
            if (xSerial != null && xSerial.IsOpen)
            {
                xSerial.Close();
                _deviceMode = eDevMode.DISCONNECT;
                return true;
            }
            else if (_deviceMode == eDevMode.SIMULATE)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool DeviceInit()
        {
            InitializeChannels();

            return true;
            
        }

        public bool DeviceReset()
        {
            throw new NotImplementedException();
        }




        public object GET_DATA_IN(string id_1, string id_2, string id_3, string id_4, ref bool result)
        {
            throw new NotImplementedException();
        }

        public double GET_DOUBLE_IN(string id_1, string id_2, string id_3, string id_4, ref bool result)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// CHANNEL 1 OUTPUT DATA : id1 = '1', id2 = '2', id3 = '1', id4 = '1'
        /// CHANNEL 2 OUTPUT DATA : id1 = '1', id2 = '2', id3 = '1', id4 = '2'
        /// CHANNEL 3 OUTPUT DATA : id1 = '1', id2 = '2', id3 = '1', id4 = '3'
        /// CHANNEL 4 OUTPUT DATA : id1 = '1', id2 = '2', id3 = '1', id4 = '4'
        /// 
        /// CHANNEL 1 ON/OFF STATUS : id1 = '1', id2 = '2', id3 = '2', id4 = '1'
        /// CHANNEL 2 ON/OFF STATUS : id1 = '1', id2 = '2', id3 = '2', id4 = '2'
        /// CHANNEL 3 ON/OFF STATUS : id1 = '1', id2 = '2', id3 = '2', id4 = '3'
        /// CHANNEL 4 ON/OFF STATUS : id1 = '1', id2 = '2', id3 = '2', id4 = '4'
        /// 
        /// </summary>
        /// <param name="id_1"></param>
        /// <param name="id_2"></param>
        /// <param name="id_3"></param>
        /// <param name="id_4"></param>
        /// <param name="result"></param>
        /// <returns></returns>

        public int GET_INT_IN(string id_1, string id_2, string id_3, string id_4, ref bool result)
        {
            if (id_1.Equals(ID_1_INPUT) && id_2.Equals(ID_2_INT) && id_3.Equals(ID_3_DATA))
            {
                if (id_4.Equals(ID_4_CH1))
                {
                    result = true;
                    return Channels[0].Data;
                }
                else if (id_4.Equals(ID_4_CH2))
                {
                    result = true;
                    return Channels[1].Data;
                }
                else if (id_4.Equals(ID_4_CH3))
                {
                    result = true;
                    return Channels[2].Data;
                }
                else if (id_4.Equals(ID_4_CH4))
                {
                    result = true;
                    return Channels[3].Data;
                }
                else
                {
                    result = false;
                    return -1;
                }
            }
            else if (id_1.Equals(ID_1_INPUT) && id_2.Equals(ID_2_INT) && id_3.Equals(ID_3_STATUS))
            {
                if (id_4.Equals(ID_4_CH1))
                {
                    result = true;
                    return Channels[0].OnOff_Status;
                }
                else if (id_4.Equals(ID_4_CH2))
                {
                    result = true;
                    return Channels[1].OnOff_Status;
                }
                else if (id_4.Equals(ID_4_CH3))
                {
                    result = true;
                    return Channels[2].OnOff_Status;
                }
                else if (id_4.Equals(ID_4_CH4))
                {
                    result = true;
                    return Channels[3].OnOff_Status;
                }
                else
                {
                    result = false;
                    return -1;
                }
            }
            else
            {
                if (isDeviceLogging) LogHelper.Instance.DeviceLog.DebugFormat("[ERROR] GET_INT_IN - Unknown Parameter Id.");
                result = false;
                return -1;
            }
        }

        public string GET_STRING_IN(string id_1, string id_2, string id_3, string id_4, ref bool result)
        {
            throw new NotImplementedException();
        }

        public eDevMode IsDevMode()
        {
            return _deviceMode;
        }

        public void SET_DATA_OUT(string id_1, string id_2, string id_3, string id_4, object value, ref bool result)
        {
            throw new NotImplementedException();
        }

        public void SET_DOUBLE_OUT(string id_1, string id_2, string id_3, string id_4, double value, ref bool result)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// CHANNEL 1 OUTPUT DATA SET : id1 = '2', id2 = '2', id3 = '1', id4 = '1'
        /// CHANNEL 2 OUTPUT DATA SET : id1 = '2', id2 = '2', id3 = '1', id4 = '2'
        /// CHANNEL 3 OUTPUT DATA SET : id1 = '2', id2 = '2', id3 = '1', id4 = '3'
        /// CHANNEL 4 OUTPUT DATA SET : id1 = '2', id2 = '2', id3 = '1', id4 = '4'
        /// 
        /// CHANNEL 1 ON/OFF SET : id1 = '2', id2 = '2', id3 = '2', id4 = '1'
        /// CHANNEL 2 ON/OFF SET : id1 = '2', id2 = '2', id3 = '2', id4 = '2'
        /// CHANNEL 3 ON/OFF SET : id1 = '2', id2 = '2', id3 = '2', id4 = '3'
        /// CHANNEL 4 ON/OFF SET : id1 = '2', id2 = '2', id3 = '2', id4 = '4'
        /// 
        /// 
        /// </summary>
        /// <param name="id_1"></param>
        /// <param name="id_2"></param>
        /// <param name="id_3"></param>
        /// <param name="id_4"></param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        public void SET_INT_OUT(string id_1, string id_2, string id_3, string id_4, int value, ref bool result)
        {
            if (id_1.Equals(ID_1_OUTPUT) && id_2.Equals(ID_2_INT) && id_3.Equals(ID_3_DATA))
            {
                if (id_4.Equals(ID_4_CHALL))
                {
                    OutputDataChange(Convert.ToChar(ID_4_CHALL), BitConverter.GetBytes(value)[0]);
                    result = true;
                }
                else if (id_4.Equals(ID_4_CH1))
                {
                    OutputDataChange(Convert.ToChar(ID_4_CH1), BitConverter.GetBytes(value)[0]);
                    result = true;
                }
                else if (id_4.Equals(ID_4_CH2))
                {
                    OutputDataChange(Convert.ToChar(ID_4_CH2), BitConverter.GetBytes(value)[0]);
                    result = true;
                }
                else if (id_4.Equals(ID_4_CH3))
                {
                    OutputDataChange(Convert.ToChar(ID_4_CH3), BitConverter.GetBytes(value)[0]);
                    result = true;
                }
                else if (id_4.Equals(ID_4_CH4))
                {
                    OutputDataChange(Convert.ToChar(ID_4_CH4), BitConverter.GetBytes(value)[0]);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else if (id_1.Equals(ID_1_OUTPUT) && id_2.Equals(ID_2_INT) && id_3.Equals(ID_3_STATUS))
            {
                if (id_4.Equals(ID_4_CHALL))
                {
                    if (value == 1) LedTurnOnByChannelNumber(0);
                    else LedTurnOffByChannelNumber(0);
                    result = true;
                }
                else if (id_4.Equals(ID_4_CH1))
                {
                    if (value == 1) LedTurnOnByChannelNumber(1);
                    else LedTurnOffByChannelNumber(1);
                    result = true;
                }
                else if (id_4.Equals(ID_4_CH2))
                {
                    if (value == 1) LedTurnOnByChannelNumber(2);
                    else LedTurnOffByChannelNumber(2);
                    result = true;
                }
                else if (id_4.Equals(ID_4_CH3))
                {
                    if (value == 1) LedTurnOnByChannelNumber(3);
                    else LedTurnOffByChannelNumber(3);
                    result = true;
                }
                else if (id_4.Equals(ID_4_CH4))
                {
                    if (value == 1) LedTurnOnByChannelNumber(4);
                    else LedTurnOffByChannelNumber(4);
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if (isDeviceLogging) LogHelper.Instance.DeviceLog.DebugFormat("[ERROR] SET_INT_OUT - Unknown Parameter Id.");
                result = false;
            }
        }

        public void SET_STRING_OUT(string id_1, string id_2, string id_3, string id_4, string value, ref bool result)
        {
            throw new NotImplementedException();
        }

        private void ThreadStart()
        {
            if (IsStarted() != true)
            {
                Start();
            }
        } 


        private string RequestOutputData(char channel, out int outputData)
        {
            outputData = ERROR_DATA_OUTPUT;

            if (_deviceMode != eDevMode.CONNECT && _deviceMode != eDevMode.SIMULATE)               
            { 
                return COMM_DISCONNECT;
            }

            List<byte> requestCommand = new List<byte>();

            requestCommand.Add(Convert.ToByte('R'));
            requestCommand.Add(Convert.ToByte(channel));
            requestCommand.Add(Convert.ToByte('D')); // Request control channel data

            string result = GetCommandEx(requestCommand.ToArray(), out byte[] response);

            if (result != COMM_TIMEOUT && result != COMM_ERROR)
            {            
                // request channel is Normal or Error
                if (response[0] == 'R')
                {
                    if (channel == response[1])
                    {
                        outputData = response[2];
                        return COMM_SUCCESS;
                    }
                    else
                    {
                        LogHelper.Instance.DeviceLog.DebugFormat("[ERROR] Wrong recieved data packet from the ALD4CH64W : Channel = {0}", channel);
                        return COMM_ERROR;
                    }
                }
                else
                {
                    return COMM_ERROR;
                }
            }       

            return COMM_ERROR;
        }

        private string RequestTurnOnStatusAll(out int status)
        {
            status = 0;

            List<byte> requestCommand = new List<byte>();

            requestCommand.Add(Convert.ToByte('R'));
            requestCommand.Add(Convert.ToByte('N'));
            requestCommand.Add(Convert.ToByte('F')); // Request control channel data

            string result = GetCommandEx(requestCommand.ToArray(), out byte[] response);

            if (result != COMM_TIMEOUT && result != COMM_ERROR)
            {
                if (response[0] == 'O' && response[1] == 'N')
                {
                    status = Convert.ToInt16(response[2]);
                    return COMM_SUCCESS;
                }
            }

            return result;
        }

        private void LedTurnOffByChannelNumber(int channel)
        {
            StringBuilder data = new StringBuilder();

            data.Append('O');
            data.Append('N');

            int selectedChannels = ledOnOffStatus;

            switch (channel)
            {
                case 0:
                    {
                        selectedChannels &= (~0x0F);
                    }
                    break;
                case 1:
                    {
                        selectedChannels &= (~0x01);
                    }
                    break;
                case 2:
                    {
                        selectedChannels &= (~0x02);
                    }
                    break;
                case 3:
                    {
                        selectedChannels &= (~0x04);
                    }
                    break;
                case 4:
                    {
                        selectedChannels &= (~0x08);
                    }
                    break;
                default:
                    {
                        selectedChannels |= 0x00;
                    }
                    break;
            }

            data.Append(selectedChannels);

            SetCommandEx(data.ToString());
        }

        private void LedTurnOnByChannelNumber(int channel)
        {
            StringBuilder data = new StringBuilder();

            data.Append('O');
            data.Append('N');

            int selectedChannels = ledOnOffStatus;

            switch (channel)
            {
                case 0:
                    {
                        selectedChannels |= 0x0F;                       
                    }
                    break;
                case 1:
                    {
                        selectedChannels |= 0x01;
                    }
                    break;
                case 2:
                    {
                        selectedChannels |= 0x02;
                    }
                    break;
                case 3:
                    {
                        selectedChannels |= 0x04;
                    }
                    break;
                case 4:
                    {
                        selectedChannels |= 0x08;
                    }
                    break;
                default:
                    {
                        selectedChannels |= 0x00;
                    }
                    break;
            }       

            data.Append(selectedChannels);

            SetCommandEx(data.ToString());
        }

        private void LedTurnOnAfterOutputDataChange(char channel, byte output_current_data)
        {
            StringBuilder data = new StringBuilder();

            data.Append('C');
            data.Append(channel);
            data.Append(output_current_data);

            SetCommandEx(data.ToString());
        }

        private void OutputDataChange(char channel, byte output_current_data)
        {
            List<byte> data = new List<byte>();

            data.Add(Convert.ToByte('R'));
            data.Add(Convert.ToByte(channel));
            data.Add(output_current_data);

            SetCommandEx(data.ToArray());
        }

        private void SetCommandEx(byte[] command)
        {
            if (_deviceMode == eDevMode.CONNECT)
            {
                SetCommand(command);
            }
            else if (_deviceMode == eDevMode.SIMULATE)
            {
                SetCommandEmulator(command);
            }
            else
            {
                return;
            }
        }

        private void SetCommandEx(string command)
        {
            if(_deviceMode == eDevMode.CONNECT)
            {
                SetCommand(command);
            }
            else if (_deviceMode == eDevMode.SIMULATE)
            {
                SetCommandEmulator(command);
            }
            else
            {
                return;
            }
        }

        private void SetCommandEmulator(string command)
        {
            lock (_key)
            {
                tcpClient.SendMessage(command);
            }
        }

        private void SetCommand(string command)
        {
            lock(_key)
            {
                xSerial.SendMessage(command);
            }
        }

        private void SetCommandEmulator(byte[] command)
        {
            lock (_key)
            {
                tcpClient.SendMessage(command);
            }
        }

        private void SetCommand(byte[] command)
        {
            lock (_key)
            {
                xSerial.SendMessage(command);
            }
        }

        private string GetCommandEx(byte[] request, out byte[] response)
        {
            if (_deviceMode == eDevMode.CONNECT)
            {
                return GetCommand(request, out response);
            }
            else if (_deviceMode == eDevMode.SIMULATE)
            {
                return GetCommandEmulator(request, out response);
            }
            else
            {
                response = null;
                return COMM_ERROR;
            }
        }

        private string GetCommand(byte[] request, out byte[] returnValue)
        {
            lock (_key)
            {
                xSerial.SendMessage(request);

                Stopwatch sw = new Stopwatch();
                sw.Start();

                while (true)
                {
                    if (sw.ElapsedMilliseconds >= _milisecondResponseTimeout)
                    {
                        returnValue = null;
                        return COMM_TIMEOUT;
                    }
                    else if (xSerial.ReadBuffer(out string response))
                    {
                        returnValue = Encoding.ASCII.GetBytes(response);
                        return COMM_SUCCESS;
                    }
                    else
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                }
            }
        }

        private string GetCommand(string request)
        {
            lock(_key)
            {       
                xSerial.SendMessage(request);

                Stopwatch sw = new Stopwatch();
                sw.Start();

                while (true)
                {
                    if (sw.ElapsedMilliseconds >= _milisecondResponseTimeout)
                    {
                        return COMM_TIMEOUT;
                    }
                    else if (xSerial.ReadBuffer(out string response))
                    {          
                        return response;
                    }
                    else
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                }
            }
        }

        private string GetCommandEmulator(byte[] request, out byte[] returnValue)
        {
            lock (_key)
            {
                tcpClient.SendMessage(request);

                Stopwatch sw = new Stopwatch();
                sw.Start();

                while (true)
                {
                    if (sw.ElapsedMilliseconds >= _milisecondResponseTimeout)
                    {
                        returnValue = null;
                        return COMM_TIMEOUT;
                    }
                    else if (tcpClient.ReadBuffer(out byte[] response))
                    {
                        returnValue = response;
                        return COMM_SUCCESS;
                    }
                    else
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                }
            }
        }



        protected override void Sequence()
        {
            lock (_key)
            {
                int outData;

                if (_deviceMode != eDevMode.CONNECT && _deviceMode != eDevMode.SIMULATE)
                {
                    return;
                }
                else if (_deviceMode == eDevMode.SIMULATE && !tcpClient.IsOpen)
                {
                    return;
                }
                else if (_deviceMode == eDevMode.CONNECT && !xSerial.IsOpen)
                {
                    return;
                }

                if (RequestOutputData('1', out outData) == COMM_SUCCESS)
                {
                    Channels[0].Data = outData;
                }

                
                if (RequestOutputData('2', out outData) == COMM_SUCCESS)
                {
                    Channels[1].Data = outData;
                }

                if (RequestOutputData('3', out outData) == COMM_SUCCESS)
                {
                    Channels[2].Data = outData;
                }

                if (RequestOutputData('4', out outData) == COMM_SUCCESS)
                {
                    Channels[3].Data = outData;
                }


                if (RequestTurnOnStatusAll(out outData) == COMM_SUCCESS)
                {
                    ledOnOffStatus = outData;
                    string bitString = Convert.ToString(outData, 2).PadLeft(8, '0'); //0000 0000
                    char[] bitArr = bitString.ToCharArray();

                    for (int i = 1; i <= bitArr.Length; i++)
                    {
                        Channel channel = Channels.Find((ch) => ch.Channel_Number == i);

                        if (channel == null)
                        {
                            continue;
                        }

                        channel.OnOff_Status = bitArr[bitArr.Length - i] == '1' ? (int)CHANNEL_STATUS.ON : (int)CHANNEL_STATUS.OFF;
                    }
                }
                
            }
        }
    }
}
