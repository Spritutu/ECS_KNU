using System.Windows;
using System.Threading;
using System.Windows.Input;
using ECS.UI.View;
using ECS.UI.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using INNO6.Core;
using INNO6.IO;
using Prism.Commands;
using Microsoft.Win32;

namespace ECS.UI.ViewModel
{
    public class SettingParameterViewModel : ViewModelBase
    {
        private int _LaserProgramNumber;
        private double _LaserPowerPercent;
        private double _LaserOnDelay;
        private double _LaserOffDelay;
        private double _MarkDelay;
        private double _JumpDelay;
        private double _PolygonDelay;
        private double _MarkSpeed;
        private double _JumpSpeed;
        private string _ScanFilePath;

        private double _VisionPositionX;
        private double _VisionPositionY;
        private double _VisionPositionZ;
        private double _VisionPositionT;
        private double _VisionPositionR;
        private double _ProcessPositionX;
        private double _ProcessPositionY;
        private double _ProcessPositionZ;
        private double _ProcessPositionT;
        private double _ProcessPositionR;

        private double _XJogVelHigh;
        private double _XJogVelLow;
        private double _YJogVelHigh;
        private double _YJogVelLow;
        private double _ZJogVelHigh;
        private double _ZJogVelLow;
        private double _TJogVelHigh;
        private double _TJogVelLow;
        private double _RJogVelHigh;
        private double _RJogVelLow;

        private ICommand _LaserProgramNumberSetButtonCommand;
        private ICommand _LaserPowerSetButtonCommand;
        private ICommand _LaserOnDelaySetButtonCommand;
        private ICommand _LaserOffDelaySetButtonCommand;
        private ICommand _MarkDelaySetButtonCommand;
        private ICommand _JumpDelaySetButtonCommand;
        private ICommand _PolygonDelaySetButtonCommand;
        private ICommand _MarkSpeedSetButtonCommand;
        private ICommand _JumpSpeedSetButtonCommand;

        private ICommand _VisionPosXSetButtonCommand;
        private ICommand _VisionPosYSetButtonCommand;
        private ICommand _VisionPosZSetButtonCommand;
        private ICommand _VisionPosTSetButtonCommand;
        private ICommand _VisionPosRSetButtonCommand;
        private ICommand _ProcPosXSetButtonCommand;
        private ICommand _ProcPosYSetButtonCommand;
        private ICommand _ProcPosZSetButtonCommand;
        private ICommand _ProcPosTSetButtonCommand;
        private ICommand _ProcPosRSetButtonCommand;

        private ICommand _XJogVelHighButtonCommand;
        private ICommand _YJogVelHighButtonCommand;
        private ICommand _ZJogVelHighButtonCommand;
        private ICommand _TJogVelHighButtonCommand;
        private ICommand _RJogVelHighButtonCommand;

        private ICommand _XJogVelLowButtonCommand;
        private ICommand _YJogVelLowButtonCommand;
        private ICommand _ZJogVelLowButtonCommand;
        private ICommand _TJogVelLowButtonCommand;
        private ICommand _RJogVelLowButtonCommand;
        private ICommand _ScanFilePathSetButtonCommand;
        private ICommand _FileOpenDlgButtonCommand;

        public int LaserProgramNumber { get { return _LaserProgramNumber; } set { _LaserProgramNumber = value; RaisePropertyChanged("LaserProgramNumber"); } }
        public double LaserPowerPercent { get { return _LaserPowerPercent; } set { _LaserPowerPercent = value; RaisePropertyChanged("LaserPowerPercent"); } }
        public double LaserOnDelay { get { return _LaserOnDelay; } set { _LaserOnDelay = value; RaisePropertyChanged("LaserOnDelay"); } }
        public double LaserOffDelay { get { return _LaserOffDelay; } set { _LaserOffDelay = value; RaisePropertyChanged("LaserOffDelay"); } }
        public double MarkDelay { get { return _MarkDelay; } set { _MarkDelay = value; RaisePropertyChanged("MarkDelay"); } }
        public double JumpDelay { get { return _JumpDelay; } set { _JumpDelay = value; RaisePropertyChanged("JumpDelay"); } }
        public double PolygonDelay { get { return _PolygonDelay; } set { _PolygonDelay = value; RaisePropertyChanged("PolygonDelay"); } }
        public double MarkSpeed { get { return _MarkSpeed; } set { _MarkSpeed = value; RaisePropertyChanged("MarkSpeed"); } }
        public double JumpSpeed { get { return _JumpSpeed; } set { _JumpSpeed = value; RaisePropertyChanged("JumpSpeed"); } }



        public double VisionPositionX { get { return _VisionPositionX; } set { _VisionPositionX = value; RaisePropertyChanged("VisionPositionX"); } }
        public double VisionPositionY { get { return _VisionPositionY; } set { _VisionPositionY = value; RaisePropertyChanged("VisionPositionY"); } }
        public double VisionPositionZ { get { return _VisionPositionZ; } set { _VisionPositionZ = value; RaisePropertyChanged("VisionPositionZ"); } }
        public double VisionPositionT { get { return _VisionPositionT; } set { _VisionPositionT = value; RaisePropertyChanged("VisionPositionT"); } }
        public double VisionPositionR { get { return _VisionPositionR; } set { _VisionPositionR = value; RaisePropertyChanged("VisionPositionR"); } }

        public double ProcessPositionX { get { return _ProcessPositionX; } set { _ProcessPositionX = value; RaisePropertyChanged("ProcessPositionX"); } }
        public double ProcessPositionY { get { return _ProcessPositionY; } set { _ProcessPositionY = value; RaisePropertyChanged("ProcessPositionY"); } }
        public double ProcessPositionZ { get { return _ProcessPositionZ; } set { _ProcessPositionZ = value; RaisePropertyChanged("ProcessPositionZ"); } }
        public double ProcessPositionT { get { return _ProcessPositionT; } set { _ProcessPositionT = value; RaisePropertyChanged("ProcessPositionT"); } }
        public double ProcessPositionR { get { return _ProcessPositionR; } set { _ProcessPositionR = value; RaisePropertyChanged("ProcessPositionR"); } }

        public double XJogVelHigh { get { return _XJogVelHigh; } set { _XJogVelHigh = value; RaisePropertyChanged("XJogVelHigh"); } }
        public double YJogVelHigh { get { return _YJogVelHigh; } set { _YJogVelHigh = value; RaisePropertyChanged("YJogVelHigh"); } }
        public double ZJogVelHigh { get { return _ZJogVelHigh; } set { _ZJogVelHigh = value; RaisePropertyChanged("ZJogVelHigh"); } }
        public double TJogVelHigh { get { return _TJogVelHigh; } set { _TJogVelHigh = value; RaisePropertyChanged("TJogVelHigh"); } }
        public double RJogVelHigh { get { return _RJogVelHigh; } set { _RJogVelHigh = value; RaisePropertyChanged("RJogVelHigh"); } }
        public double XJogVelLow { get { return _XJogVelLow; } set { _XJogVelLow = value; RaisePropertyChanged("XJogVelLow"); } }
        public double YJogVelLow { get { return _YJogVelLow; } set { _YJogVelLow = value; RaisePropertyChanged("YJogVelLow"); } }
        public double ZJogVelLow { get { return _ZJogVelLow; } set { _ZJogVelLow = value; RaisePropertyChanged("ZJogVelLow"); } }
        public double TJogVelLow { get { return _TJogVelLow; } set { _TJogVelLow = value; RaisePropertyChanged("TJogVelLow"); } }
        public double RJogVelLow { get { return _RJogVelLow; } set { _RJogVelLow = value; RaisePropertyChanged("RJogVelLow"); } }
        public string ScanFilePath { get { return _ScanFilePath; } set { _ScanFilePath = value; RaisePropertyChanged("ScanFilePath"); } }

        public ICommand VisionPosXSetButtonCommand { get { if (_VisionPosXSetButtonCommand == null) { _VisionPosXSetButtonCommand = new DelegateCommand(ExecuteVisionPosXSetButtonCommand); } return _VisionPosXSetButtonCommand; }}
        public ICommand VisionPosYSetButtonCommand { get { if (_VisionPosYSetButtonCommand == null) { _VisionPosYSetButtonCommand = new DelegateCommand(ExecuteVisionPosYSetButtonCommand); } return _VisionPosYSetButtonCommand; }}
        public ICommand VisionPosZSetButtonCommand { get { if (_VisionPosZSetButtonCommand == null) { _VisionPosZSetButtonCommand = new DelegateCommand(ExecuteVisionPosZSetButtonCommand); } return _VisionPosZSetButtonCommand; } }
        public ICommand VisionPosTSetButtonCommand { get { if (_VisionPosTSetButtonCommand == null) { _VisionPosTSetButtonCommand = new DelegateCommand(ExecuteVisionPosTSetButtonCommand); } return _VisionPosTSetButtonCommand; } }
        public ICommand VisionPosRSetButtonCommand { get { if (_VisionPosRSetButtonCommand == null) { _VisionPosRSetButtonCommand = new DelegateCommand(ExecuteVisionPosRSetButtonCommand); } return _VisionPosRSetButtonCommand; } }
        public ICommand ProcPosXSetButtonCommand { get { if (_ProcPosXSetButtonCommand == null) { _ProcPosXSetButtonCommand = new DelegateCommand(ExecuteProcPosXSetButtonCommand); } return _ProcPosXSetButtonCommand; } }
        public ICommand ProcPosYSetButtonCommand { get { if (_ProcPosYSetButtonCommand == null) { _ProcPosYSetButtonCommand = new DelegateCommand(ExecuteProcPosYSetButtonCommand); } return _ProcPosYSetButtonCommand; } }
        public ICommand ProcPosZSetButtonCommand { get { if (_ProcPosZSetButtonCommand == null) { _ProcPosZSetButtonCommand = new DelegateCommand(ExecuteProcPosZSetButtonCommand); } return _ProcPosZSetButtonCommand; } }
        public ICommand ProcPosTSetButtonCommand { get { if (_ProcPosTSetButtonCommand == null) { _ProcPosTSetButtonCommand = new DelegateCommand(ExecuteProcPosTSetButtonCommand); } return _ProcPosTSetButtonCommand; } }
        public ICommand ProcPosRSetButtonCommand { get { if (_ProcPosRSetButtonCommand == null) { _ProcPosRSetButtonCommand = new DelegateCommand(ExecuteProcPosRSetButtonCommand); } return _ProcPosRSetButtonCommand; } }

        public ICommand XJogVelHighButtonCommand { get { if (_XJogVelHighButtonCommand == null) { _XJogVelHighButtonCommand = new DelegateCommand(ExecuteXJogVelHighButtonCommand); } return _XJogVelHighButtonCommand; } }
        public ICommand YJogVelHighButtonCommand { get { if (_YJogVelHighButtonCommand == null) { _YJogVelHighButtonCommand = new DelegateCommand(ExecuteYJogVelHighButtonCommand); } return _YJogVelHighButtonCommand; } }
        public ICommand ZJogVelHighButtonCommand { get { if (_ZJogVelHighButtonCommand == null) { _ZJogVelHighButtonCommand = new DelegateCommand(ExecuteZJogVelHighButtonCommand); } return _ZJogVelHighButtonCommand; } }
        public ICommand TJogVelHighButtonCommand { get { if (_TJogVelHighButtonCommand == null) { _TJogVelHighButtonCommand = new DelegateCommand(ExecuteTJogVelHighButtonCommand); } return _TJogVelHighButtonCommand; } }
        public ICommand RJogVelHighButtonCommand { get { if (_RJogVelHighButtonCommand == null) { _RJogVelHighButtonCommand = new DelegateCommand(ExecuteRJogVelHighButtonCommand); } return _RJogVelHighButtonCommand; } }

        public ICommand XJogVelLowButtonCommand { get { if (_XJogVelLowButtonCommand == null) { _XJogVelLowButtonCommand = new DelegateCommand(ExecuteXJogVelLowButtonCommand); } return _XJogVelLowButtonCommand; } }
        public ICommand YJogVelLowButtonCommand { get { if (_YJogVelLowButtonCommand == null) { _YJogVelLowButtonCommand = new DelegateCommand(ExecuteYJogVelLowButtonCommand); } return _YJogVelLowButtonCommand; } }
        public ICommand ZJogVelLowButtonCommand { get { if (_ZJogVelLowButtonCommand == null) { _ZJogVelLowButtonCommand = new DelegateCommand(ExecuteZJogVelLowButtonCommand); } return _ZJogVelLowButtonCommand; } }
        public ICommand TJogVelLowButtonCommand { get { if (_TJogVelLowButtonCommand == null) { _TJogVelLowButtonCommand = new DelegateCommand(ExecuteTJogVelLowButtonCommand); } return _TJogVelLowButtonCommand; } }
        public ICommand RJogVelLowButtonCommand { get { if (_RJogVelLowButtonCommand == null) { _RJogVelLowButtonCommand = new DelegateCommand(ExecuteRJogVelLowButtonCommand); } return _RJogVelLowButtonCommand; } }

        public ICommand LaserProgramNumberSetButtonCommand { get { if (_LaserProgramNumberSetButtonCommand == null) { _LaserProgramNumberSetButtonCommand = new DelegateCommand(ExecuteLaserProgramNumberSetButtonCommand); } return _LaserProgramNumberSetButtonCommand; } }

        public ICommand LaserPowerSetButtonCommand { get { if (_LaserPowerSetButtonCommand == null) { _LaserPowerSetButtonCommand = new DelegateCommand(ExecuteLaserPowerSetButtonCommand); } return _LaserPowerSetButtonCommand; } }
        public ICommand LaserOnDelaySetButtonCommand { get { if (_LaserOnDelaySetButtonCommand == null) { _LaserOnDelaySetButtonCommand = new DelegateCommand(ExecuteLaserOnDelaySetButtonCommand); } return _LaserOnDelaySetButtonCommand; } }

        public ICommand LaserOffDelaySetButtonCommand { get { if (_LaserOffDelaySetButtonCommand == null) { _LaserOffDelaySetButtonCommand = new DelegateCommand(ExecuteLaserOffDelaySetButtonCommand); } return _LaserOffDelaySetButtonCommand; } }

        public ICommand MarkDelaySetButtonCommand { get { if (_MarkDelaySetButtonCommand == null) { _MarkDelaySetButtonCommand = new DelegateCommand(ExecuteMarkDelaySetButtonCommand); } return _MarkDelaySetButtonCommand; } }
        public ICommand JumpDelaySetButtonCommand { get { if (_JumpDelaySetButtonCommand == null) { _JumpDelaySetButtonCommand = new DelegateCommand(ExecuteJumpDelaySetButtonCommand); } return _JumpDelaySetButtonCommand; } }
        public ICommand PolygonDelaySetButtonCommand { get { if (_PolygonDelaySetButtonCommand == null) { _PolygonDelaySetButtonCommand = new DelegateCommand(ExecutePolygonDelaySetButtonCommand); } return _PolygonDelaySetButtonCommand; } }
        public ICommand MarkSpeedSetButtonCommand { get { if (_MarkSpeedSetButtonCommand == null) { _MarkSpeedSetButtonCommand = new DelegateCommand(ExecuteMarkSpeedSetButtonCommand); } return _MarkSpeedSetButtonCommand; } }
        public ICommand JumpSpeedSetButtonCommand { get { if (_JumpSpeedSetButtonCommand == null) { _JumpSpeedSetButtonCommand = new DelegateCommand(ExecuteJumpSpeedSetButtonCommand); } return _JumpSpeedSetButtonCommand; } }
        public ICommand ScanFilePathSetButtonCommand { get { if (_ScanFilePathSetButtonCommand == null) { _ScanFilePathSetButtonCommand = new DelegateCommand(ExecuteScanFilePathSetButtonCommand); } return _ScanFilePathSetButtonCommand; } }
        public ICommand FileOpenDlgButtonCommand { get { if (_FileOpenDlgButtonCommand == null) { _FileOpenDlgButtonCommand = new DelegateCommand(ExecuteFileOpenDlgButtonCommand); } return _FileOpenDlgButtonCommand; } }

        public SettingParameterViewModel()
        {
            VisionPositionX = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisX.VisionPosition", out bool _);
            VisionPositionY = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisY.VisionPosition", out bool _);
            VisionPositionZ = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisZ.VisionPosition", out bool _);
            VisionPositionT = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisT.VisionPosition", out bool _);
            VisionPositionR = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisR.VisionPosition", out bool _);

            ProcessPositionX = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisX.ProcessPosition", out bool _);
            ProcessPositionY = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisY.ProcessPosition", out bool _);
            ProcessPositionZ = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisZ.ProcessPosition", out bool _);
            ProcessPositionT = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisT.ProcessPosition", out bool _);
            ProcessPositionR = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisR.ProcessPosition", out bool _);


            XJogVelHigh = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisX.JogVelHigh", out bool _);
            YJogVelHigh = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisY.JogVelHigh", out bool _);
            ZJogVelHigh = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisZ.JogVelHigh", out bool _);
            TJogVelHigh = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisT.JogVelHigh", out bool _);
            RJogVelHigh = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisR.JogVelHigh", out bool _);

            XJogVelLow = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisX.JogVelLow", out bool _);
            YJogVelLow = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisY.JogVelLow", out bool _);
            ZJogVelLow = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisZ.JogVelLow", out bool _);
            TJogVelLow = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisT.JogVelLow", out bool _);
            RJogVelLow = DataManager.Instance.GET_DOUBLE_DATA("vSet.dAxisR.JogVelLow", out bool _);

            LaserOnDelay = DataManager.Instance.GET_DOUBLE_DATA("oRTC.dLaser.OnDelay", out bool _);
            LaserOffDelay = DataManager.Instance.GET_DOUBLE_DATA("oRTC.dLaser.OffDelay", out bool _);

            MarkDelay = DataManager.Instance.GET_DOUBLE_DATA("oRTC.dScan.MarkDelay", out bool _);
            JumpDelay = DataManager.Instance.GET_DOUBLE_DATA("oRTC.dScan.JumpDelay", out bool _);
            PolygonDelay = DataManager.Instance.GET_DOUBLE_DATA("oRTC.dScan.PolygonDelay", out bool _);

            MarkSpeed = DataManager.Instance.GET_DOUBLE_DATA("oRTC.dScan.MarkSpeed", out bool _);
            JumpSpeed = DataManager.Instance.GET_DOUBLE_DATA("oRTC.dScan.JumpSpeed", out bool _);

            ScanFilePath = DataManager.Instance.GET_STRING_DATA("vSet.sScan.DocFilePath", out bool _);

            LaserPowerPercent = DataManager.Instance.GET_DOUBLE_DATA("oRTC.dLaser.ProcessPowerPercent", out bool _);
            LaserProgramNumber = DataManager.Instance.GET_INT_DATA("vSet.iLaser.ProgramNo", out bool _);
        }

        private void ExecuteVisionPosXSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisX.VisionPosition", VisionPositionX);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisX.VisionPosition", VisionPositionX);
        }

        private void ExecuteVisionPosYSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisY.VisionPosition", VisionPositionY);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisY.VisionPosition", VisionPositionY);

        }

        private void ExecuteVisionPosZSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisZ.VisionPosition", VisionPositionZ);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisZ.VisionPosition", VisionPositionZ);
        }
        private void ExecuteVisionPosTSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisT.VisionPosition", VisionPositionT);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisT.VisionPosition", VisionPositionT);
        }

        private void ExecuteVisionPosRSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisR.VisionPosition", VisionPositionR);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisR.VisionPosition", VisionPositionR);
        }

        private void ExecuteProcPosXSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisX.ProcessPosition", ProcessPositionX);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisX.ProcessPosition", ProcessPositionX);

        }

        private void ExecuteProcPosYSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisY.ProcessPosition", ProcessPositionY);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisY.ProcessPosition", ProcessPositionY);

        }

        private void ExecuteProcPosZSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisZ.ProcessPosition", ProcessPositionZ);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisZ.ProcessPosition", ProcessPositionZ);
        }
        private void ExecuteProcPosTSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisT.ProcessPosition", ProcessPositionT);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisT.ProcessPosition", ProcessPositionT);
        }

        private void ExecuteProcPosRSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisR.ProcessPosition", ProcessPositionR);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisR.ProcessPosition", ProcessPositionR);
        }


        private void ExecuteXJogVelHighButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisX.JogVelHigh", XJogVelHigh);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisX.JogVelHigh", XJogVelHigh);
        }

        private void ExecuteYJogVelHighButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisY.JogVelHigh", XJogVelHigh);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisY.JogVelHigh", XJogVelHigh);
        }

        private void ExecuteZJogVelHighButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisZ.JogVelHigh", XJogVelHigh);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisZ.JogVelHigh", XJogVelHigh);
        }

        private void ExecuteTJogVelHighButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisT.JogVelHigh", XJogVelHigh);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisT.JogVelHigh", XJogVelHigh);
        }

        private void ExecuteRJogVelHighButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisR.JogVelHigh", XJogVelHigh);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisR.JogVelHigh", XJogVelHigh);
        }

        private void ExecuteXJogVelLowButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisX.JogVelLow", XJogVelHigh);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisX.JogVelLow", XJogVelHigh);
        }

        private void ExecuteYJogVelLowButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisY.JogVelLow", XJogVelHigh);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisY.JogVelLow", XJogVelHigh);
        }

        private void ExecuteZJogVelLowButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisZ.JogVelLow", XJogVelHigh);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisZ.JogVelLow", XJogVelHigh);
        }

        private void ExecuteTJogVelLowButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisT.JogVelLow", XJogVelHigh);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisT.JogVelLow", XJogVelHigh);
        }

        private void ExecuteRJogVelLowButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("vSet.dAxisR.JogVelLow", XJogVelHigh);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.dAxisR.JogVelLow", XJogVelHigh);
        }

        private void ExecuteLaserProgramNumberSetButtonCommand()
        {
            DataManager.Instance.SET_INT_DATA("vSet.iLaser.ProgramNo", LaserProgramNumber);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.iLaser.ProgramNo", LaserProgramNumber);
        }

        private void ExecuteLaserPowerSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("oRTC.dLaser.ProcessPowerPercent", LaserPowerPercent);
            DataManager.Instance.CHANGE_DEFAULT_DATA("oRTC.dLaser.ProcessPowerPercent", LaserPowerPercent);
        }

        private void ExecuteLaserOnDelaySetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("oRTC.dLaser.OnDelay", LaserOnDelay);
            DataManager.Instance.CHANGE_DEFAULT_DATA("oRTC.dLaser.OnDelay", LaserOnDelay);
        }

        private void ExecuteLaserOffDelaySetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("oRTC.dLaser.OffDelay", LaserOffDelay);
            DataManager.Instance.CHANGE_DEFAULT_DATA("oRTC.dLaser.OffDelay", LaserOffDelay);
        }

        private void ExecuteMarkDelaySetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("oRTC.dScan.MarkDelay", MarkDelay);
            DataManager.Instance.CHANGE_DEFAULT_DATA("oRTC.dScan.MarkDelay", MarkDelay);
        }

        private void ExecuteJumpDelaySetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("oRTC.dScan.JumpDelay", JumpDelay);
            DataManager.Instance.CHANGE_DEFAULT_DATA("oRTC.dScan.JumpDelay", JumpDelay);
        }

        private void ExecutePolygonDelaySetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("oRTC.dScan.PolygonDelay", PolygonDelay);
            DataManager.Instance.CHANGE_DEFAULT_DATA("oRTC.dScan.PolygonDelay", PolygonDelay);
        }
        
        private void ExecuteMarkSpeedSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("oRTC.dScan.MarkSpeed", MarkSpeed);
            DataManager.Instance.CHANGE_DEFAULT_DATA("oRTC.dScan.MarkSpeed", MarkSpeed);
        }

        private void ExecuteJumpSpeedSetButtonCommand()
        {
            DataManager.Instance.SET_DOUBLE_DATA("oRTC.dScan.JumpSpeed", JumpSpeed);
            DataManager.Instance.CHANGE_DEFAULT_DATA("oRTC.dScan.JumpSpeed", JumpSpeed);
        }

        private void ExecuteScanFilePathSetButtonCommand()
        {
            DataManager.Instance.SET_STRING_DATA("vSet.sScan.DocFilePath", ScanFilePath);
            DataManager.Instance.CHANGE_DEFAULT_DATA("vSet.sScan.DocFilePath", ScanFilePath);
        }

        private void ExecuteFileOpenDlgButtonCommand()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = ".dxf";
            dlg.Filter = "DXF Files (*.dxf)|*.dxf|All Files (*.*)|*.*";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                ScanFilePath = dlg.FileName;
            }
        }
    }
}
