using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using INNO6.Core;
using INNO6.IO.Interface;
using Scanlab;
using Scanlab.Sirius;

namespace DEV.ScanlabControl
{
    public class SCANLAB : XSequence, IDeviceHandler
    {
        // ID Define
        public const string ID_1_INPUT = "1";
        public const string ID_1_OUTPUT = "2";
        public const string ID_1_BOTH = "3";

        public const string ID_2_OBJECT = "0";
        public const string ID_2_DOUBLE = "1";
        public const string ID_2_INT = "2";
        public const string ID_2_STRING = "3";

        public const string ID_3_DEVICE_N1 = "1";

        public const int TRUE = 1;
        public const int FALSE = 0;

        public const int OPEN = 1;
        public const int CLOSE = 0;

        public const int ON = 1;
        public const int OFF = 0;

        public const string UNKNOWN = "UNKNOWN";



        private IRtc _Rtc;
        private ILaser _Laser;
        private string _DeviceName;
        private SCAN_HEAD_STATUS _SCAN_HEAD_STATUS;
        private eDevMode _DevMode;

        private float _Frequency;
        private float _PulseWidth;
        private float _LaserOnDelay;
        private float _LaserOffDelay;
        private float _ScannerJumpDelay;
        private float _ScannerMarkDelay;
        private float _ScannerPolygonDelay;
        private float _JumpSpeed;
        private float _MarkSpeed;
        private float _LaserMaxPower;
        private float _LaserProcessPower;
        private float _LaserPowerVoltage;
        private float _LaserPowerRatio;
        private float _FThetaLensFOV;
        private string _FieldCorrectionFilePath;
        private int _RecipeCount;
        private int _ApplyRecipeFileNumber;
        private uint _Repeat;
        private SortedList<int, string> _RecipeFileList = new SortedList<int, string>();



        private bool Initialized = false;

        private ConfigManager _Config;

        public bool DeviceAttach(string deviceName, string init_file_path, string arg2, string arg3, string arg4, string arg5, string arg6, string arg7, string arg8, string arg9)
        {
            try
            {
                bool result = false;
                _DevMode = eDevMode.UNKNOWN;

                result = InitialParameters(init_file_path);

                result = Scanlab.Core.Initialize();


                float kfactor = (float)Math.Pow(2, 20) / _FThetaLensFOV; // k factor (bits/mm) = 2^20 / fov
                _FieldCorrectionFilePath = Path.GetFullPath(_FieldCorrectionFilePath);

                _DeviceName = deviceName;
                _Rtc = new Rtc5(0, "output.txt");
                _SCAN_HEAD_STATUS = new SCAN_HEAD_STATUS();

                result = _Rtc.Initialize(kfactor, LaserMode.Yag1, _FieldCorrectionFilePath);


                _Rtc.CtlFrequency(_Frequency * 1000, _PulseWidth); // laser frequency : 50KHz, pulse width : 2usec
                _Rtc.CtlSpeed(_JumpSpeed, _MarkSpeed); // default jump and mark speed : 100mm/s
                _Rtc.CtlDelay(_LaserOnDelay, _LaserOffDelay, _ScannerJumpDelay, _ScannerMarkDelay, _ScannerPolygonDelay); // scanner and laser delays
                
                #region initialize Laser (virtual)
                _Laser = new LaserVirtual(0, "virtual", _LaserMaxPower);
                _Laser.Rtc = _Rtc;
                #endregion
                Start();

                if(result)
                {
                    _DevMode = eDevMode.CONNECT;
                }
                else
                {
                    _DevMode = eDevMode.ERROR;
                }
                
                return result;
            }
            catch (FormatException)
            {
                _DevMode = eDevMode.ERROR;
                return false;
            }
            catch(OverflowException)
            {
                _DevMode = eDevMode.ERROR;
                return false;
            }
            catch(ArgumentNullException)
            {
                _DevMode = eDevMode.ERROR;
                return false;
            }
            finally
            {
      
            }
        }

        public bool DeviceDettach()
        {
            bool result = false;

            result = _Rtc.CtlLaserOff();
            result = _Rtc.CtlAbort();
            _Rtc.Dispose();
            _DevMode = eDevMode.DISCONNECT;
            return result;
        }

        public bool DeviceInit()
        {
            return true;
        }

        public bool DeviceReset()
        {
            bool result = true;

            result &= _Rtc.CtlAbort();

            result &= _Rtc.CtlBusyWait();

            //reset rtc's status
            result &= _Rtc.CtlReset();
            return result;
        }


        protected override void Sequence()
        {
            ScanHeadStatusCheck();
        }

        public object GET_DATA_IN(string id_1, string id_2, string id_3, string id_4, ref bool result)
        {
            throw new NotImplementedException();
        }

        public double GET_DOUBLE_IN(string id_1, string id_2, string id_3, string id_4, ref bool result)
        {
            result = false;

            if(id_1 == ID_1_INPUT && id_1 == ID_2_DOUBLE)
            {
                if(id_3 == "1")
                {
                    if(id_4 == "1")
                    {
                        result = true;
                        return _LaserPowerRatio;
                    }
                }
            }

            return 0.0;
        }

        /// <summary>
        /// SCAN HEAD STATUS (HEAD#1/BUSY STATUS : id1 = '1', id2 = '2', id3 = '1', id4 = '1'
        /// SCAN HEAD STATUS (HEAD#1/POWER STATUS : id1 = '1', id2 = '2', id3 = '1', id4 = '2'
        /// SCAN HEAD STATUS (HEAD#1/POSTIONACK STATUS : id1 = '1', id2 = '2', id3 = '1', id4 = '3'
        /// SCAN HEAD STATUS (HEAD#1/ERROR STATUS : id1 = '1', id2 = '2', id3 = '1', id4 = '4'
        /// SCAN HEAD STATUS (HEAD#1/TEMPERATURE STATUS : id1 = '1', id2 = '2', id3 = '1', id4 = '5'
        /// Scan Process Repeat :             id1 = '1', id2 = '2', id3 = '1', id4 = '7'  
        /// </summary>
        /// <param name="id_1">IO DIRECTION</param>
        /// <param name="id_2">IO DATA TYPE</param>
        /// <param name="id_3">Parameter Type</param>
        /// <param name="id_4"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public int GET_INT_IN(string id_1, string id_2, string id_3, string id_4, ref bool result)
        {
            if (id_1 == ID_1_INPUT && id_2 == ID_2_INT && id_3 == ID_3_DEVICE_N1)
            {
                // BUSY STATUS
                if (id_4 == "1")
                {
                    result = true;
                    return _SCAN_HEAD_STATUS.BusyStatus;
                }
                else if (id_4 == "2")
                {
                    result = true;
                    return _SCAN_HEAD_STATUS.PowerOk;
                }
                else if (id_4 == "3")
                {
                    result = true;
                    return _SCAN_HEAD_STATUS.PositionAckOk;
                }
                else if (id_4 == "4")
                {
                    result = true;
                    return _SCAN_HEAD_STATUS.ErrorStatus;
                }
                else if (id_4 == "5")
                {
                    result = true;
                    return _SCAN_HEAD_STATUS.TempOk;
                }
                else if (id_4 == "7")
                {
                    result = true;
                    return (int)_Repeat;
                }
            }
            result = false;
            return -1;
        }


        /// <summary>
        /// SCAN HEAD STATUS (HEAD#1/BUSY STATUS : id1 = '1', id2 = '3', id3 = '1', id4 = '1'
        /// SCAN HEAD STATUS (HEAD#1/POWER STATUS : id1 = '1', id2 = '3', id3 = '1', id4 = '2'
        /// SCAN HEAD STATUS (HEAD#1/POSTIONACK STATUS : id1 = '1', id2 = '3', id3 = '1', id4 = '3'
        /// SCAN HEAD STATUS (HEAD#1/ERROR STATUS : id1 = '1', id2 = '3', id3 = '1', id4 = '4'
        /// SCAN HEAD STATUS (HEAD#1/TEMPERATURE STATUS : id1 = '1', id2 = '3', id3 = '1', id4 = '5'
        /// </summary>
        /// <param name="id_1">IO DIRECTION</param>
        /// <param name="id_2">IO DATA TYPE</param>
        /// <param name="id_3">Parameter Type</param>
        /// <param name="id_4"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public string GET_STRING_IN(string id_1, string id_2, string id_3, string id_4, ref bool result)
        {
            throw new NotImplementedException();
        }

        public eDevMode IsDevMode()
        {
            return _DevMode;
        }

        public void SET_DATA_OUT(string id_1, string id_2, string id_3, string id_4, object value, ref bool result)
        {
            result = false;

            if (id_1.Equals(ID_1_OUTPUT) && id_2.Equals(ID_2_OBJECT))
            {
                if(id_3 == "1" && id_4 == "1")
                {
                    if(value is IDocument)
                    {
                        var doc = value as IDocument;
                        result = ExecuteScanDocument(doc);
                    }        
                }
                else if(id_3=="1" && id_4 == "2")
                {
                    if (value is IDocument)
                    {
                        var doc = value as IDocument;
                        result = ExecuteScanDocument(doc);
                    }
                }
            }
        }

        /// <summary>
        /// Laser Frequency :            id1 = '2', id2 = '1', id3 = '1', id4 = '1'
        /// Laser PulseWidth :           id1 = '2', id2 = '1', id3 = '1', id4 = '2'
        /// Laser On Delay :             id1 = '2', id2 = '1', id3 = '1', id4 = '3'
        /// Laser Off Delay :            id1 = '2', id2 = '1', id3 = '1', id4 = '4'
        /// Scanner Jump Delay :         id1 = '2', id2 = '1', id3 = '1', id4 = '5'
        /// Scanner Mark Delay :         id1 = '2', id2 = '1', id3 = '1', id4 = '6'
        /// Scanner PolygonDelay Delay   id1 = '2', id2 = '1', id3 = '1', id4 = '7'
        /// Scanner Jump Speed :         id1 = '2', id2 = '1', id3 = '1', id4 = '8'
        /// Scanner Mark Speed :         id1 = '2', id2 = '1', id3 = '1', id4 = '9'
        /// Laser Maximum Power (watt) :         id1 = '2', id2 = '1', id3 = '1', id4 = '10'
        /// Laser Process Power (watt) :         id1 = '2', id2 = '1', id3 = '1', id4 = '11'
        /// Laser Process Power Ratio (%) :         id1 = '2', id2 = '1', id3 = '1', id4 = '12'
        /// </summary>
        /// <param name="id_1">IO DIRECTION</param>
        /// <param name="id_2">IO DATA TYPE</param>
        /// <param name="id_3">DEVICE NUMBER</param>
        /// <param name="id_4">PARAMETER ID</param>
        /// <param name="result"></param>
        public void SET_DOUBLE_OUT(string id_1, string id_2, string id_3, string id_4, double value, ref bool result)
        {
            if (id_1 == ID_1_OUTPUT && id_2 == ID_2_DOUBLE)
            {
               if(id_3 == ID_3_DEVICE_N1)
                {
                    switch(id_4)
                    {
                        case "1":
                            this._Frequency = Convert.ToSingle(value);                           
                            break;
                        case "2":
                            this._PulseWidth = Convert.ToSingle(value);
                            break;
                        case "3":
                            this._LaserOnDelay = Convert.ToSingle(value);
                            break;
                        case "4":
                            this._LaserOffDelay = Convert.ToSingle(value);
                            break;
                        case "5":
                            this._ScannerJumpDelay = Convert.ToSingle(value);
                            break;
                        case "6":
                            this._ScannerMarkDelay = Convert.ToSingle(value);
                            break;
                        case "7":
                            this._ScannerPolygonDelay = Convert.ToSingle(value);
                            break;
                        case "8":
                            this._JumpSpeed = Convert.ToSingle(value);
                            break;
                        case "9":
                            this._MarkSpeed = Convert.ToSingle(value);
                            break;
                        case "10":
                            this._LaserMaxPower = Convert.ToSingle(value);
                            break;
                        case "11":
                            this._LaserProcessPower = Convert.ToSingle(value);
                            break;
                        case "12":
                            this._LaserPowerRatio = Convert.ToSingle(value);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Laser Control On/Off :            id1 = '2', id2 = '2', id3 = '1', id4 = '1'
        /// Correction File Reload :          id1 = '2', id2 = '2', id3 = '1', id4 = '2' 
        /// Scan Parameter Reset :            id1 = '2', id2 = '2', id3 = '1', id4 = '3' 
        /// Apply Recipe File Number :        id1 = '2', id2 = '2', id3 = '1', id4 = '4' 
        /// Execute Scan Process  :           id1 = '2', id2 = '2', id3 = '1', id4 = '5'  
        /// Abort Scan Process :              id1 = '2', id2 = '2', id3 = '1', id4 = '6'  
        /// Scan Process Repeat :             id1 = '2', id2 = '2', id3 = '1', id4 = '7'  
        /// </summary>
        /// <param name="id_1">IO DIRECTION</param>
        /// <param name="id_2">IO DATA TYPE</param>
        /// <param name="id_3">DEVICE NUMBER</param>
        /// <param name="id_4">PARAMETER ID</param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        public void SET_INT_OUT(string id_1, string id_2, string id_3, string id_4, int value, ref bool result)
        {
            result = false;

            if(id_1 == "2" && id_2 == "2")
            {
                if(id_3 == "1")
                {
                    switch(id_4)
                    {
                        case "1":
                            {
                                if (value == ON) result = LaserOn(_LaserPowerRatio);
                                else result = LaserOff();
                            }
                            break;
                        case "2":
                            {
                                if (value == TRUE) result = CorrectionFileReLoad();
                            }
                            break;
                        case "3":
                            {
                                if (value == TRUE) result = ScanParameterReLoad();
                            }
                            break;
                        case "4":
                            {
                                if (!_RecipeFileList.ContainsKey(value))
                                {
                                    result = false;
                                }
                                else
                                {
                                    _ApplyRecipeFileNumber = value;
                                    result = true;
                                }
                            }
                            break;
                        case "5":
                            {
                                if (value == ON)
                                {
                                    result = ExecuteScanDocument();
                                }
                            }
                            break;
                        case "6":
                            {
                                if (value == ON)
                                {
                                    StopMarkAndReset(_Laser, _Rtc);
                                    result = true;
                                }
                            }
                            break;
                        case "7":
                            {
                                _Repeat = Convert.ToUInt32(value);
                                result = true;
                            }
                            break;
                        default:
                            result = false;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Field Correction File Path : id1 = '2', id2 = '1', id3 = '1', id4 = '1'
        /// </summary>
        /// <param name="id_1">IO DIRECTION</param>
        /// <param name="id_2">IO DATA TYPE</param>
        /// <param name="id_3">DEVICE NUMBER</param>
        /// <param name="id_4">PARAMETER ID</param>
        /// <param name="value"></param>
        /// <param name="result"></param>
        public void SET_STRING_OUT(string id_1, string id_2, string id_3, string id_4, string value, ref bool result)
        {
            if (id_1 == ID_1_OUTPUT && id_2 == ID_2_STRING)
            {
                if (id_3 == ID_3_DEVICE_N1)
                {
                    switch(id_4)
                    {
                        case "1":
                            _FieldCorrectionFilePath = value;
                            break;
                    }
                }
            }
        }


        #region private method



        private void ScanHeadStatusCheck()
        {
            if(_Rtc.CtlGetStatus(RtcStatus.Busy)){ _SCAN_HEAD_STATUS.BusyStatus = SCAN_HEAD_STATUS.BUSY; }
            else { _SCAN_HEAD_STATUS.BusyStatus = SCAN_HEAD_STATUS.NOTBUSY; }

            if (_Rtc.CtlGetStatus(RtcStatus.PowerOK)) { _SCAN_HEAD_STATUS.PowerOk = SCAN_HEAD_STATUS.OK; }
            else { _SCAN_HEAD_STATUS.PowerOk = SCAN_HEAD_STATUS.NOTOK; }

            if (_Rtc.CtlGetStatus(RtcStatus.PositionAckOK)) { _SCAN_HEAD_STATUS.PositionAckOk = SCAN_HEAD_STATUS.OK; }
            else { _SCAN_HEAD_STATUS.PositionAckOk = SCAN_HEAD_STATUS.NOTOK; }

            if (_Rtc.CtlGetStatus(RtcStatus.NoError)) { _SCAN_HEAD_STATUS.ErrorStatus = SCAN_HEAD_STATUS.NO_ERROR; }
            else { _SCAN_HEAD_STATUS.ErrorStatus = SCAN_HEAD_STATUS.ERROR; }

            if (_Rtc.CtlGetStatus(RtcStatus.TempOK)) { _SCAN_HEAD_STATUS.TempOk = SCAN_HEAD_STATUS.OK; }
            else { _SCAN_HEAD_STATUS.TempOk = SCAN_HEAD_STATUS.NOTOK; }

        }


        private bool LaserOn(float powerRatio)
        {
            lock (_Laser.SyncRoot)
            {
                if (null == _Rtc)
                {
                    return false;
                }
                bool success = true;
                // output power를 analog voltage로 환산 (0 ~ 10V)
                float analogVoltage = powerRatio/100 * 10;

                success &= _Rtc.CtlWriteData<float>(ExtensionChannel.ExtAO1, analogVoltage);
                success &= _Rtc.CtlLaserOn();
                return success;
            }
        }

        private bool LaserOff()
        {
            lock (_Laser.SyncRoot)
            {
                bool success = true;
                if (success &= _Rtc.CtlGetStatus(RtcStatus.NotBusy))
                {
                    LogHelper.Instance.DeviceLog.ErrorFormat("Current status is Laser off !");
                    return success;
                }

                success &= _Rtc.CtlWriteData<float>(ExtensionChannel.ExtAO1, 0);
                success &= _Rtc.CtlLaserOff();

                return success;
            }
        }

        private bool CorrectionFileReLoad()
        {
            bool success = true;
            if (success &= _Rtc.CtlGetStatus(RtcStatus.Busy))
            {
                LogHelper.Instance.DeviceLog.ErrorFormat("Processing are working already !");
                return success;
            }

            success &= _Rtc.CtlLoadCorrectionFile(CorrectionTableIndex.Table1, _FieldCorrectionFilePath);

            if (success)
                success &= _Rtc.CtlSelectCorrection(CorrectionTableIndex.Table1);

            return success;
        }

        private bool ScanParameterReLoad()
        {
            bool success = true;
            if (success &= _Rtc.CtlGetStatus(RtcStatus.Busy))
            {
                LogHelper.Instance.DeviceLog.ErrorFormat("Processing are working already !");
                return success;
            }

            success &= _Rtc.CtlFrequency(_Frequency, _PulseWidth);
            success &= _Rtc.CtlDelay(_LaserOnDelay, _LaserOffDelay, _ScannerJumpDelay, _ScannerMarkDelay, _ScannerPolygonDelay);
            success &= _Rtc.CtlSpeed(_JumpSpeed, _MarkSpeed);

            return success;
        }

        /// <summary>
        /// 행렬을 이용해 직선을 그릴때 1도마다 직선을 회전시켜 그리기
        /// </summary>
        /// <param name="rtc"></param>
        /// <param name="angleStart"></param>
        /// <param name="angleEnd"></param>
        private static bool DrawLinesWithRotate(ILaser laser, IRtc rtc, double angleStart, double angleEnd, float line_start_x, float line_start_y, float line_end_x, float line_end_y)
        {
            bool success = true;
            success &= rtc.ListBegin(laser);
            rtc.MatrixStack.Push(2, 4); // dx =2, dy=4 만큼 이동
            for (double angle = angleStart; angle <= angleEnd; angle += 1)
            {
                rtc.MatrixStack.Push(angle);
                success &= rtc.ListJump(new Vector2(line_start_x, line_start_y));
                success &= rtc.ListMark(new Vector2(line_end_x, line_end_y));
                rtc.MatrixStack.Pop();
            }
            rtc.MatrixStack.Pop();
            if (success)
            {
                success &= rtc.ListEnd();
                success &= rtc.ListExecute(true);
            }
            return success;
        }

        /// <summary>
        /// draw cicle with dots
        /// </summary>
        /// <param name="laser"></param>
        /// <param name="rtc"></param>
        /// <param name="radius"></param>
        /// <param name="durationMsec"></param>
        private static bool DrawCircleWithDots(ILaser laser, IRtc rtc, float radius, float durationMsec)
        {
            if (rtc.CtlGetStatus(RtcStatus.Busy))
                return false;
            LogHelper.Instance.DeviceLog.DebugFormat("WARNING !!! LASER IS BUSY ... DrawCircleWithDots");
            Stopwatch timer = Stopwatch.StartNew();
            bool success = true;
            success &= rtc.ListBegin(laser);
            for (float angle = 0; angle < 360; angle += 1)
            {
                double x = radius * Math.Sin(angle * Math.PI / 180.0);
                double y = radius * Math.Cos(angle * Math.PI / 180.0);
                success &= rtc.ListJump(new Vector2((float)x, (float)y));
                //지정된 짧은 시간동안 레이저 출사
                success &= rtc.ListLaserOn(durationMsec);
                if (!success)
                    break;
            }
            success &= rtc.ListEnd();
            if (success)
                success &= rtc.ListExecute(true);
            LogHelper.Instance.DeviceLog.DebugFormat($"Processing time = {timer.ElapsedMilliseconds / 1000.0:F3}s");
            return success;
        }

        private static bool DrawSquareAreaWithPixels(ILaser laser, IRtc rtc, float size, float gap, float shot_usec)
        {

            if (rtc.CtlGetStatus(RtcStatus.Busy))
                return false;
            LogHelper.Instance.DeviceLog.DebugFormat("WARNING !!! LASER IS BUSY ... DrawSquareAreaWithPixels");
            Stopwatch timer = Stopwatch.StartNew();
            // pixel operation 은 IRtcExtension 인터페이스에서 제공
            var rtcExt = rtc as IRtcExtension;
            if (null == rtcExt)
                return false;
            int counts = (int)(size / gap);
            bool success = true;
            success &= rtc.ListBegin(laser);
            for (int i = 0; i < counts; i++)
            {
                //줄의 시작위치로 점프
                success &= rtc.ListJump(new Vector2(0, i * gap));
                // pixel의 최대 주기시간 (200us), 출력 채널(analog 1), 가로세로 간격 (gap), 총 pixel 개수
                success &= rtcExt.ListPixelLine(200, new Vector2(gap, 0), (uint)counts, ExtensionChannel.ExtAO1);
                for (int j = 0; j < counts; j++)
                    success &= rtcExt.ListPixel(shot_usec, 0.5f); // 20usec, 5V
                if (!success)
                    break;
            }
            success &= rtc.ListEnd();
            if (success)
                success &= rtc.ListExecute(true);
            LogHelper.Instance.DeviceLog.DebugFormat($"Processing time = {timer.ElapsedMilliseconds / 1000.0:F3}s");
            return success;
        }

        private static void StopMarkAndReset(ILaser laser, IRtc rtc)
        {
            LogHelper.Instance.DeviceLog.DebugFormat("Trying to abort ...");

            //abort to mark
            rtc.CtlAbort();
            //wait for rtc busy off
            rtc.CtlBusyWait();
            //reset rtc's status
            rtc.CtlReset();
        }

        private bool InitialParameters(string iniFilePath)
        {
            bool success = true;
            _Repeat = 1;
            string fileFullPath = System.IO.Path.GetFullPath(iniFilePath);
            _Config = new ConfigManager(fileFullPath);

            success &= float.TryParse(_Config.GetIniValue("LENS", "FOV"), out _FThetaLensFOV);
            success &= float.TryParse(_Config.GetIniValue("DELAY", "LASER_ON"), out _LaserOnDelay);
            success &= float.TryParse(_Config.GetIniValue("DELAY", "LASER_OFF"), out _LaserOffDelay);
            success &= float.TryParse(_Config.GetIniValue("DELAY", "JUMP"), out _ScannerJumpDelay);
            success &= float.TryParse(_Config.GetIniValue("DELAY", "MARK"), out _ScannerMarkDelay);
            success &= float.TryParse(_Config.GetIniValue("DELAY", "POLYGON"), out _ScannerPolygonDelay);
            success &= float.TryParse(_Config.GetIniValue("SPEED", "JUMP"), out _JumpSpeed);
            success &= float.TryParse(_Config.GetIniValue("SPEED", "MARK"), out _MarkSpeed);
            success &= float.TryParse(_Config.GetIniValue("LASER", "FREQUENCY"), out _Frequency);
            success &= float.TryParse(_Config.GetIniValue("LASER", "PULSEWIDTH"), out _PulseWidth);
            success &= float.TryParse(_Config.GetIniValue("LASER", "MAXIMUM_POWER"), out _LaserMaxPower);
            success &= float.TryParse(_Config.GetIniValue("LASER", "PROCESS_POWER"), out _LaserPowerRatio);
            _FieldCorrectionFilePath = _Config.GetIniValue("FIELD_CORRECTION", "FILEPATH");
            success &= int.TryParse(_Config.GetIniValue("RECIPE", "COUNT"), out _RecipeCount);
            success &= int.TryParse(_Config.GetIniValue("RECIPE", "SET"), out _ApplyRecipeFileNumber);

            for(int i = 0; i < _RecipeCount; i++)
            {
                string file_num = string.Format("{0}", i + 1);
                _RecipeFileList.Add(i+1, _Config.GetIniValue("RECIPE_FILE", file_num));
            }

            return success;
        }

        private bool ExecuteScanDocument()
        {
            bool success = true;

            if (!_RecipeFileList.ContainsKey(_ApplyRecipeFileNumber)) 
                return false;

            string recipe = Path.GetFullPath(_RecipeFileList[_ApplyRecipeFileNumber]);

            IDocument doc = DocumentSerializer.OpenSirius(recipe);

            success = ScannerProcessing(_Laser, _Rtc, doc);

            return success;
        }

        private bool ExecuteScanDocument(IDocument doc)
        {
            bool success = true;

            success &= ScannerProcessing(_Laser, _Rtc, doc);

            return success;
        }

        private bool ExecuteScanLayer(Layer layer)
        {
            bool success = true;

            success &= ScannerProcessing(_Laser, _Rtc, layer);

            return success;
        }

        private bool ScannerProcessing(ILaser laser, IRtc rtc, Layer layer)
        {
            var timer = Stopwatch.StartNew();
            bool success = true;
            success &= rtc.ListBegin(laser);

            rtc.ListDelay(_LaserOnDelay, _LaserOffDelay, _ScannerJumpDelay, _ScannerMarkDelay, _ScannerPolygonDelay);
            rtc.ListSpeed(_JumpSpeed, _MarkSpeed);
            _LaserPowerVoltage = 10 * (_LaserPowerRatio / 100);
            rtc.ListWriteData<float>(ExtensionChannel.ExtAO1, _LaserPowerVoltage);

            var markerArg = new MarkerArgDefault()
            {
                Document = null,
                Rtc = rtc,
                Laser = laser,
            };

            foreach (var item in layer.Items)
            {
                if(item is IMarkerable markerable)
                {
                    markerable.Mark(markerArg);
                }
            }

            return success;
        }

        private bool ScannerProcessing(ILaser laser, IRtc rtc, IDocument doc)
        {
            var timer = Stopwatch.StartNew();


            bool success = true;

            success &= rtc.ListBegin(laser);

            //success &= rtc.ListWriteData<float>(ExtensionChannel.ExtAO1, 1f);

            //rtc.ListDelay(_LaserOnDelay, _LaserOffDelay, _ScannerJumpDelay, _ScannerMarkDelay, _ScannerPolygonDelay);
            //rtc.ListSpeed(_JumpSpeed, _MarkSpeed);

            //for(int i = -1; i<1; i++)
            //{
            //    Vector2 start = new Vector2(20.0f, Convert.ToSingle(i)*10.0f);
            //    Vector2 end = new Vector2(-20.0f, Convert.ToSingle(i)*10.0f);

            //    success &= rtc.ListJump(start, 1);
            //    success &= rtc.ListMark(end, 1);
            //}

            //success &= rtc.ListEnd();
            //success &= rtc.ListExecute(true);

            //Vector2 start = new Vector2(20.0f, 0f);

            //Vector2 end = new Vector2(-20.0f, 0f);

            //Vector2 start1 = new Vector2(20.0f, 20f);

            //Vector2 end1 = new Vector2(-20.0f, 20f);

            //Vector2 start2 = new Vector2(0f, 20.0f);

            //Vector2 end2 = new Vector2(0f, -20.0f);

            //success &= rtc.ListWriteData<float>(ExtensionChannel.ExtAO1, 2f);



            //success = success & rtc.ListJump(start, 1) & rtc.ListMark(end, 1);

            //success = success & rtc.ListJump(start1, 1) & rtc.ListMark(end1, 1);

            //success = success & rtc.ListJump(start2, 1) & rtc.ListMark(end2, 1);

            //success &= rtc.ListEnd();
            //success &= rtc.ListExecute(true);
            rtc.ListDelay(_LaserOnDelay, _LaserOffDelay, _ScannerJumpDelay, _ScannerMarkDelay, _ScannerPolygonDelay);
            rtc.ListSpeed(_JumpSpeed, _MarkSpeed);
            _LaserPowerVoltage = 10 * (_LaserPowerRatio / 100);
            rtc.ListWriteData<float>(ExtensionChannel.ExtAO1, _LaserPowerVoltage);
            var markerArg = new MarkerArgDefault()
            {
                Document = doc,
                Rtc = rtc,
                Laser = laser,
            };

            foreach (var layer in doc.Layers)
            {
                layer.Regen();
                layer.Repeat = _Repeat;
                success &= layer.Mark(markerArg);

                // or 
                //레이어 내의 개체(Entity)들을 순회
                //foreach (var entity in layer)
                //{
                //    var markerable = entity as IMarkerable;
                //    //레이저 가공이 가능한 개체(markerable)인지를 판단
                //    if (null != markerable)
                //        success &= markerable.Mark(markerArg);    // 해당 개체(Entity) 가공 
                //    if (!success)
                //        break;
                //}

                if (!success)
                    break;
            }

            if (success)
            {
                //Vector2 zero = new Vector2(0.0f, 0.0f);
                //success &= rtc.ListJump(zero);
                success &= rtc.ListEnd();
                success &= rtc.ListExecute(true);
            }

            LogHelper.Instance.DeviceLog.InfoFormat($"Processing time = {timer.ElapsedMilliseconds / 1000.0:F3}s");

            return success;
        }

        #endregion
    }
}
