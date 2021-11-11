using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using INNO6.Core.Manager;
using INNO6.IO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ECS.UI.ViewModel
{
    public class LaserControlViewModel : ViewModelBase
    {
        #region Define const. variable (IO NAME, FUNCTION NAME)
        private const string VIRTUAL_LASER_PROGRAM_NO = "vSet.iLaser.ProgramNo";
        private const string IO_LASER_PROGRAM_NO = "oPMAC.iLaser.PStartStatical";
        private const string IO_LASER_PROCESS_POWER_PERCENT = "oRTC.dLaser.ProcessPowerPercent";
        private const string IO_LASER_PROCESS_POWER = "oRTC.dLaser.ProcessPower";

        private const string IO_LASER_IS_ON = "iPMAC.iLaser.IsOn";
        private const string IO_LASER_READY = "iPMAC.iLaser.Ready";
        private const string IO_LASER_ASSIGNED = "iPMAC.iLaser.Assigned";
        private const string IO_LASER_PROGRAM_ACIVE = "iPMAC.iLaser.ProgActive";
        private const string IO_LASER_PROGRAM_COMPLETED = "iPMAC.iLaser.ProgCompleted";
        private const string IO_LASER_FAULT = "iPMAC.iLaser.Fault";
        private const string IO_LASER_EXT_ACTIVATION = "oPMAC.iLaser.ExtActivation";
        private const string IO_LASER_ON = "oPMAC.iLaser.On";
        private const string IO_LASER_STANDBY = "oPMAC.iLaser.Standby";
        private const string IO_LASER_WARNING = "vSys.iEqp.LaserWarningOnOff";


        private const string IO_SCAN_IS_BUSY = "iRTC.iScan.BusyStatus";
        private const string IO_SCAN_IS_ERROR = "iRTC.iScan.ErrorStatus";
        private const string IO_SCAN_IS_POSITIONACK = "iRTC.iScan.PosAckStatus";
        private const string IO_SCAN_IS_TEMP = "iRTC.iScan.TempStatus";
        private const string IO_SCAN_IS_POWER = "iRTC.iScan.PowerStatus";
        private const string IO_SCAN_PROCESS_FILEPATH = "vSet.sScan.DocFilePath";
        private const string IO_SCAN_PROCESS_REPEAT = "oRTC.iScan.ProcessRepeat";

        private const string F_LASER_ON = "F_LASER_ON";
        private const string F_LASER_STOP = "F_LASER_STOP";
        private const string F_LASER_REQUEST = "F_LASER_REQUEST";
        private const string F_LASER_RESET = "F_LASER_RESET";
        private const string F_LASER_PROGRAM_START = "F_LASER_PROGRAM_START";
        private const string F_SCAN_PROCESS_START = "F_SCAN_PROCESS_START";
        private const string F_SCAN_PROCESS_ABORT = "F_SCAN_PROCESS_ABORT";
        private const string F_SCAN_PROCDOC_START = "F_SCAN_PROCDOC_START";
        #endregion

        private Timer _Timer;

        private int _LaserProgramNo;
        private int _ScanProgramNo;
        private int _ScanBusyStatus;
        private int _ScanTempStatus;
        private int _ScanErrorStatus;
        private int _ScanPositionAckStatus;
        private int _ScanPowerStatus;
        private int _ProcessRepeat;
        private double _LaserPowerPercent;
        private bool _LaserOnButtonEnable;
        private bool _LaserStopButtonEnable;
        private bool _LaserRequestButtonEnable;
        private bool _LaserProgramSetButtonEnable;
        private bool _LaserResetButtonEnable;
        private bool _LaserShutterButtonEnable;
        private bool _ScanProcessButtonEnable;

        private bool _IsLaserOn;
        private bool _IsLaserAssigned;
        private bool _IsLaserReady;
        private bool _IsLaserProgramActive;
        private bool _IsLaserProgramCompleted;
        private bool _IsLaserFault;

        private string _LaserStopButtonContent;
        private string _LaserOnButtonContent;
        private string _LaserRequestButtonContent;
        private string _LaserResetButtonContent;
        private string _LaserShutterButtonContent;
        private string _ScanProcessButtonContent;
        private string _LaserProgramSetButtonContent;
        private string _ScanProcessFilePath;

        private ICommand _LaserOnButtonCommand;
        private ICommand _LaserStopButtonCommand;
        private ICommand _LaserRequestButtonCommand;
        private ICommand _LaserProgramSetButtonCommand;
        private ICommand _LaserProgramNoValueChanged;
        private ICommand _LaserResetButtonCommand;
        private ICommand _LaserShutterButtonCommand;
        private ICommand _LaserPowerPercentValueChanged;
        private ICommand _ScanProgramNoValueChanged;
        private ICommand _ScanProcessButtonCommand;
        private ICommand _FileOpenButtonCommand;
        private ICommand _ProcessRepeatValueChanged;

        public LaserControlViewModel()
        {
            _LaserOnButtonContent = "STANDBY";
            _LaserOnButtonEnable = true;
            _LaserProgramSetButtonEnable = true;

            _LaserResetButtonContent = "RESET";
            _LaserResetButtonEnable = false;

            _LaserRequestButtonContent = "REQUEST";
            _LaserRequestButtonEnable = false;

            _LaserStopButtonContent = "STOP";

            _LaserProgramSetButtonContent = "P-START";
            _LaserProgramSetButtonEnable = false;

            _LaserShutterButtonContent = "BEAM.OPEN";
            _LaserShutterButtonEnable = false;

            _ScanProcessButtonContent = "SCAN.START";
            _ScanProcessButtonEnable = false;

            DataManager.Instance.DataAccess.DataChangedEvent += DataAccess_DataChanged;

            ScanBusyStatus = DataManager.Instance.GET_INT_DATA(IO_SCAN_IS_BUSY, out bool _);
            ScanErrorStatus = DataManager.Instance.GET_INT_DATA(IO_SCAN_IS_ERROR, out bool _);
            ScanPositionAckStatus = DataManager.Instance.GET_INT_DATA(IO_SCAN_IS_POSITIONACK, out bool _);
            ScanPowerStatus = DataManager.Instance.GET_INT_DATA(IO_SCAN_IS_POWER, out bool _);
            ScanTempStatus = DataManager.Instance.GET_INT_DATA(IO_SCAN_IS_TEMP, out bool _);

            _Timer = new Timer(LaserControlViewSchedulingTimmer, this, 0, 1000);
        }

        private void DataAccess_DataChanged(object sender, DataChangedEventHandlerArgs args)
        {
            Data data = args.Data;

            switch(data.Name)
            {
                case IO_LASER_IS_ON:
                    IsLaserOn = (data.Type == eDataType.Int)? (int)data.Value == 1 : false;

                    if(IsLaserOn)
                    {
                        LaserRequestButtonEnable = true;
                        LaserStopButtonEnable = true;
                        LaserOnButtonEnable = false;
                        DataManager.Instance.SET_INT_DATA(IO_LASER_WARNING, 1);
                    }
                    else
                    {
                        LaserRequestButtonEnable = false;
                        LaserStopButtonEnable = false;
                        LaserOnButtonEnable = true;
                        LaserShutterButtonEnable = false;
                        LaserShutterButtonContent = "BEAM.OPEN";
                        ScanProcessButtonEnable = false;
                        ScanProcessButtonContent = "SCAN.START";
                        DataManager.Instance.SET_INT_DATA(IO_LASER_WARNING, 0);
                    }

                    break;
                case IO_LASER_READY:
                    IsLaserReady = (data.Type == eDataType.Int) ? (int)data.Value == 1 : false;

                    if(IsLaserReady)
                    {
                        LaserRequestButtonEnable = false;
                        LaserProgramSetButtonEnable = true;
                    }
                    break;
                case IO_LASER_ASSIGNED:
                    IsLaserAssigned = (data.Type == eDataType.Int) ? (int)data.Value == 1 : false;

                    if (IsLaserAssigned)
                    {
                        LaserProgramSetButtonContent = "P-START";
                        LaserProgramSetButtonEnable = false;
                        LaserRequestButtonEnable = false;
                    }
                    else
                    {
                        LaserProgramSetButtonEnable = false;
                        LaserRequestButtonEnable = true;
                    }
                        
                    break;
                case IO_LASER_PROGRAM_ACIVE:
                    IsLaserProgramActive = (data.Type == eDataType.Int) ? (int)data.Value == 1 : false;

                    if(IsLaserProgramActive)
                    {
                        LaserProgramSetButtonEnable = false;

                        LaserShutterButtonEnable = true;
                        LaserShutterButtonContent = "BEAM.OPEN";

                        ScanProcessButtonContent = "SCAN.START";
                        LaserProgramSetButtonEnable = false;

                        ScanProcessButtonEnable = true;
                    }
                    else
                    {
                        ScanProcessButtonEnable = false;
                        LaserShutterButtonEnable = false;
                    }

                    break;
                case IO_LASER_PROGRAM_COMPLETED:
                    IsLaserProgramCompleted = (data.Type == eDataType.Int) ? (int)data.Value == 1 : false;
                    break;
                case IO_LASER_FAULT:
                    IsLaserFault = (data.Type == eDataType.Int) ? (int)data.Value == 1 : false;

                    if (IsLaserFault) LaserResetButtonEnable = true;
                    else LaserResetButtonEnable = false;

                    break;
                case IO_SCAN_IS_BUSY:
                    ScanBusyStatus = (int)data.Value;

                    if(ScanBusyStatus == 1)
                    {
                        LaserShutterButtonEnable = true;
                        LaserShutterButtonContent = "BEAM.CLOSE";
                        ScanProcessButtonEnable = true;
                        ScanProcessButtonContent = "SCAN.STOP";
                    }
                    else
                    {
                        LaserShutterButtonEnable = true;
                        LaserShutterButtonContent = "BEAM.OPEN";
                        ScanProcessButtonEnable = true;
                        ScanProcessButtonContent = "SCAN.START";
                    }                 
                    break;
                case IO_SCAN_IS_ERROR:
                    ScanErrorStatus = (int)data.Value;
                    break;
                case IO_SCAN_IS_POSITIONACK:
                    ScanPositionAckStatus = (int)data.Value;
                    break;
                case IO_SCAN_IS_POWER:
                    ScanPowerStatus = (int)data.Value;
                    break;
                case IO_SCAN_IS_TEMP:
                    ScanTempStatus = (int)data.Value;
                    break;
            }

        }

        private void LaserControlViewSchedulingTimmer(object state)
        {
            IsLaserFault = DataManager.Instance.GET_INT_DATA(IO_LASER_FAULT, out bool _) == 1 ? true : false;

            if (IsLaserFault) LaserResetButtonEnable = true;
            else LaserResetButtonEnable = false;

        }


        public int LaserProgramNo { get { return _LaserProgramNo; } set { _LaserProgramNo = value; RaisePropertyChanged("LaserProgramNo"); } }
        public int ScanProgramNo { get { return _ScanProgramNo; } set { _ScanProgramNo = value; RaisePropertyChanged("ScanProgramNo"); } }
        public int ScanBusyStatus { get { return _ScanBusyStatus; } set { _ScanBusyStatus = value; RaisePropertyChanged("ScanBusyStatus"); } }
        public int ScanErrorStatus { get { return _ScanErrorStatus; } set { _ScanErrorStatus = value; RaisePropertyChanged("ScanErrorStatus"); } }
        public int ScanPositionAckStatus { get { return _ScanPositionAckStatus; } set { _ScanPositionAckStatus = value; RaisePropertyChanged("ScanPositionAckStatus"); } }
        public int ScanTempStatus { get { return _ScanTempStatus; } set { _ScanTempStatus = value; RaisePropertyChanged("ScanTempStatus"); } }
        public int ScanPowerStatus { get { return _ScanPowerStatus; } set { _ScanPowerStatus = value; RaisePropertyChanged("ScanPowerStatus"); } }
        public int ProcessRepeat { get { return _ProcessRepeat; } set { _ProcessRepeat = value; RaisePropertyChanged("ProcessRepeat"); } }
        
        public double LaserPowerPercent { get { return _LaserPowerPercent; } set { _LaserPowerPercent = value; RaisePropertyChanged("LaserPowerPercent"); } }
 
        public bool LaserOnButtonEnable { get { return _LaserOnButtonEnable; } set { _LaserOnButtonEnable = value; RaisePropertyChanged("LaserOnButtonEnable"); } }
        public bool LaserStopButtonEnable { get { return _LaserStopButtonEnable; } set { _LaserStopButtonEnable = value; RaisePropertyChanged("LaserStopButtonEnable"); } }
        public bool LaserRequestButtonEnable { get { return _LaserRequestButtonEnable; } set { _LaserRequestButtonEnable = value; RaisePropertyChanged("LaserRequestButtonEnable"); } }
        public bool LaserResetButtonEnable { get { return _LaserResetButtonEnable; } set { _LaserResetButtonEnable = value; RaisePropertyChanged("LaserResetButtonEnable"); } }
        public bool LaserProgramSetButtonEnable { get { return _LaserProgramSetButtonEnable; } set { _LaserProgramSetButtonEnable = value; RaisePropertyChanged("LaserProgramSetButtonEnable"); } }
        public bool LaserShutterButtonEnable { get { return _LaserShutterButtonEnable; } set { _LaserShutterButtonEnable = value; RaisePropertyChanged("LaserShutterButtonEnable"); } }
        public bool ScanProcessButtonEnable { get { return _ScanProcessButtonEnable; } set { _ScanProcessButtonEnable = value; RaisePropertyChanged("ScanProcessButtonEnable"); } }

        public bool IsLaserOn { get { return _IsLaserOn; } set { _IsLaserOn = value; RaisePropertyChanged("IsLaserOn"); } }
        public bool IsLaserAssigned { get { return _IsLaserAssigned; } set { _IsLaserAssigned = value; RaisePropertyChanged("IsLaserAssigned"); } }
        public bool IsLaserReady { get { return _IsLaserReady; } set { _IsLaserReady = value; RaisePropertyChanged("IsLaserReady"); } }
        public bool IsLaserProgramActive { get { return _IsLaserProgramActive; } set { _IsLaserProgramActive = value; RaisePropertyChanged("IsLaserProgramActive"); } }
        public bool IsLaserProgramCompleted { get { return _IsLaserProgramCompleted; } set { _IsLaserProgramCompleted = value; RaisePropertyChanged("IsLaserProgramCompleted"); } }
        public bool IsLaserFault { get { return _IsLaserFault; } set { _IsLaserFault = value; RaisePropertyChanged("IsLaserFault"); } }


        public string LaserOnButtonContent { get { return _LaserOnButtonContent; } set { _LaserOnButtonContent = value; RaisePropertyChanged("LaserOnButtonContent"); } }
        public string LaserStopButtonContent { get { return _LaserStopButtonContent; } set { _LaserStopButtonContent = value; RaisePropertyChanged("LaserStopButtonContent"); } }
        public string LaserRequestButtonContent { get { return _LaserRequestButtonContent; } set { _LaserRequestButtonContent = value; RaisePropertyChanged("LaserRequestButtonContent"); } }
        public string LaserResetButtonContent { get { return _LaserResetButtonContent; } set { _LaserResetButtonContent = value; RaisePropertyChanged("LaserResetButtonContent"); } }
        public string LaserProgramSetButtonContent { get { return _LaserProgramSetButtonContent; } set { _LaserProgramSetButtonContent = value; RaisePropertyChanged("LaserProgramSetButtonContent"); } }

        
        public string LaserShutterButtonContent { get { return _LaserShutterButtonContent; } set { _LaserShutterButtonContent = value; RaisePropertyChanged("LaserShutterButtonContent"); } }
        public string ScanProcessButtonContent { get { return _ScanProcessButtonContent; } set { _ScanProcessButtonContent = value; RaisePropertyChanged("ScanProcessButtonContent"); } }
        public string ScanProcessFilePath { get { return _ScanProcessFilePath; } set { _ScanProcessFilePath = value; RaisePropertyChanged("ScanProcessFilePath"); } }

        public ICommand LaserPowerPercentValueChanged { get { return this._LaserPowerPercentValueChanged ?? (this._LaserPowerPercentValueChanged = new RelayCommand(ExecuteLaserPowerPercentValueChanged)); } }
        public ICommand LaserOnButtonCommand { get { return this._LaserOnButtonCommand ?? (this._LaserOnButtonCommand = new RelayCommand(ExecuteLaserOnButtonCommand)); } }
        public ICommand LaserStopButtonCommand { get { return this._LaserStopButtonCommand ?? (this._LaserStopButtonCommand = new RelayCommand(ExecuteLaserStopButtonCommand)); } }

        public ICommand LaserRequestButtonCommand { get { return this._LaserRequestButtonCommand ?? (this._LaserRequestButtonCommand = new RelayCommand(ExecuteLaserRequestButtonCommand)); } }

        public ICommand LaserResetButtonCommand { get { return this._LaserResetButtonCommand ?? (this._LaserResetButtonCommand = new RelayCommand(ExecuteLaserResetButtonCommand)); } }
        public ICommand LaserShutterButtonCommand { get { return this._LaserShutterButtonCommand ?? (this._LaserShutterButtonCommand = new RelayCommand(ExecuteLaserShutterButtonCommand)); } }

        public ICommand LaserProgramSetButtonCommand { get { return this._LaserProgramSetButtonCommand ?? (this._LaserProgramSetButtonCommand = new RelayCommand(ExecuteLaserProgramSetButtonCommand)); } }
        public ICommand LaserProgramNoValueChanged { get { return this._LaserProgramNoValueChanged ?? (this._LaserProgramNoValueChanged = new RelayCommand(ExecuteLaserProgramNoValueChanged)); } }
        public ICommand ScanProgramNoValueChanged { get { return this._ScanProgramNoValueChanged ?? (this._ScanProgramNoValueChanged = new RelayCommand(ExecuteScanProgramNoValueChanged));}}             
        public ICommand ScanProcessButtonCommand { get { return this._ScanProcessButtonCommand ?? (this._ScanProcessButtonCommand = new RelayCommand(ExecuteScanProcessButtonCommand)); } }
        public ICommand FileOpenButtonCommand { get { return this._FileOpenButtonCommand ?? (this._FileOpenButtonCommand = new RelayCommand(ExecuteFileOpenButtonCommand)); } }
        public ICommand ProcessRepeatValueChanged { get { return this._ProcessRepeatValueChanged ?? (this._ProcessRepeatValueChanged = new RelayCommand(ExecuteProcessRepeatValueChanged)); } }




        
        private void ExecuteFileOpenButtonCommand()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = ".dxf";
            dlg.Filter = "DXF Files (*.dxf)|*.dxf|All Files (*.*)|*.*";

            bool? result = dlg.ShowDialog();

            if(result == true)
            {
                ScanProcessButtonEnable = true;
                ScanProcessFilePath = dlg.FileName;
                DataManager.Instance.SET_STRING_DATA(IO_SCAN_PROCESS_FILEPATH, ScanProcessFilePath);
            }
        }

        private void ExecuteLaserShutterButtonCommand()
        {
            if(LaserShutterButtonContent.Equals("BEAM OPEN"))
            {
                LaserShutterButtonContent = "BEAM CLOSE";
                DataManager.Instance.SET_INT_DATA("oRTC.iScan.LaserOn", 1);
            }
            else
            {
                LaserShutterButtonContent = "BEAM OPEN";
                DataManager.Instance.SET_INT_DATA("oRTC.iScan.LaserOn", 0);
            }   
        }

        private void ExecuteLaserProgramNoValueChanged()
        {
            DataManager.Instance.SET_INT_DATA(VIRTUAL_LASER_PROGRAM_NO, LaserProgramNo);
        }

        private void ExecuteProcessRepeatValueChanged()
        {
            DataManager.Instance.SET_INT_DATA(IO_SCAN_PROCESS_REPEAT, ProcessRepeat);
        }

        private void ExecuteLaserProgramSetButtonCommand()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_LASER_PROGRAM_START);
        }

        private void ExecuteLaserOnButtonCommand()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_LASER_ON);
        }

        private void ExecuteLaserStopButtonCommand()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_LASER_STOP);
        }


        private void ExecuteLaserRequestButtonCommand()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_LASER_REQUEST);
        }


        private void ExecuteLaserResetButtonCommand()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_LASER_RESET);
        }

        private void ExecuteLaserPowerPercentValueChanged()
        {
            DataManager.Instance.SET_DOUBLE_DATA(IO_LASER_PROCESS_POWER_PERCENT, LaserPowerPercent);
        }

        private void ExecuteScanProcessButtonCommand()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_SCAN_PROCDOC_START);
            //FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_SCAN_PROCESS_START);
        }

        private void ExecuteScanProgramNoValueChanged()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC(F_SCAN_PROCESS_ABORT);
        }
    }
}