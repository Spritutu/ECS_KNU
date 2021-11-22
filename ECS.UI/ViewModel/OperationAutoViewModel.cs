using ECS.Common.Helper;
using ECS.Recipe;
using ECS.UI.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using INNO6.Core.Manager;
using INNO6.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ECS.UI.ViewModel
{
    public class OperationAutoViewModel : ViewModelBase
    {
        private Timer _Timer;
        private bool _IsEnableProcessButton;
        private bool _IsEnableInitButton;
        private double _XAxisPosition;
        private double _YAxisPosition;
        private double _ZAxisPosition;
        private double _RAxisPosition;
        private double _TAxisPosition;

        private int _StepId;

        private double _LaserPowerPercent;
        private int _ProcessRepeat;
        private ICommand _ProcessButtonCommand;
        private ICommand _InitButtonCommand;
        private string _ScanFilePath;
        private string _RecipeFilePath;

        public void Start()
        {
            _Timer = new Timer(TimerCallbackFunction, this, 0, 500);
        }

        public void Stop()
        {
            if(_Timer != null)
                _Timer.Dispose();
        }

        public string ScanFilePath { get { return _ScanFilePath; } set { _ScanFilePath = value; RaisePropertyChanged("ScanFilePath"); } }

        public double XAxisPosition { get { return _XAxisPosition; } set { _XAxisPosition = value; RaisePropertyChanged("XAxisPosition"); } }
        public double YAxisPosition { get { return _YAxisPosition; } set { _YAxisPosition = value; RaisePropertyChanged("YAxisPosition"); } }
        public double ZAxisPosition { get { return _ZAxisPosition; } set { _ZAxisPosition = value; RaisePropertyChanged("ZAxisPosition"); } }
        public double RAxisPosition { get { return _RAxisPosition; } set { _RAxisPosition = value; RaisePropertyChanged("RAxisPosition"); } }
        public double TAxisPosition { get { return _TAxisPosition; } set { _TAxisPosition = value; RaisePropertyChanged("TAxisPosition"); } }
        public int StepId { get { return _StepId; } set { _StepId = value; RaisePropertyChanged("StepId"); } }
        public int ProcessRepeat { get { return _ProcessRepeat; } set { _ProcessRepeat = value; RaisePropertyChanged("ProcessRepeat"); } }
        public string RecipeFilePath { get { return _RecipeFilePath; } set { _RecipeFilePath = value; RaisePropertyChanged("RecipeFilePath"); } }

        public double LaserPowerPercent { get { return _LaserPowerPercent; } set { _LaserPowerPercent = value; RaisePropertyChanged("LaserPowerPercent"); } }

        public bool IsEnableProcessButton { get { return _IsEnableProcessButton; } set { _IsEnableProcessButton = value; RaisePropertyChanged("IsEnableProcessButton"); } }
        public bool IsEnableInitButton { get { return _IsEnableInitButton; } set { _IsEnableInitButton = value; RaisePropertyChanged("IsEnableInitButton"); } }

        public ICommand ProcessButtonCommand { get { return this._ProcessButtonCommand ?? (this._ProcessButtonCommand = new RelayCommand(ExecuteProcessButtonCommand)); } }
        public ICommand InitButtonCommand { get { return this._InitButtonCommand ?? (this._InitButtonCommand = new RelayCommand(ExecuteInitButtonCommand)); } }


        private void TimerCallbackFunction(object state)
        {
            RecipeFilePath = RecipeManager.Instance.CurrentRecipeName;
            StepId = DataManager.Instance.GET_INT_DATA(IoNameHelper.V_INT_GUI_CURRENT_STEPID, out _);
            XAxisPosition = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_X_POSITION, out _);
            YAxisPosition = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_Y_POSITION, out _);
            ZAxisPosition = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_Z_POSITION, out _);
            RAxisPosition = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_R_POSITION, out _);
            TAxisPosition = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_PMAC_T_POSITION, out _);
            LaserPowerPercent = DataManager.Instance.GET_DOUBLE_DATA(IoNameHelper.IN_DBL_RTC_LASER_POWER_PERCENT, out _);
            ProcessRepeat = DataManager.Instance.GET_INT_DATA(IoNameHelper.IN_INT_RTC_SCAN_PROCESS_REPEAT, out _);
            ScanFilePath = DataManager.Instance.GET_STRING_DATA(IoNameHelper.V_STR_SET_SCAN_DOCUMENT_FILEPATH, out _);
        }

        private void ExecuteProcessButtonCommand()
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("Do you really want to start processing?");

            if (MessageBoxManager.ShowYesNoBox(message.ToString(), "PROCESS START") == MSGBOX_RESULT.OK)
            {
                PROCESS_RESULT result = MessageBoxManager.ShowProgressWindow("AUTO PROCESSING", "Wait for a moments...", FuncNameHelper.AUTO_PROCESS);

                if (result == PROCESS_RESULT.SUCCESS)
                {
                    IsEnableInitButton = false;
                    IsEnableProcessButton = false;
                }
                else
                {
                    IsEnableInitButton = true;
                    IsEnableProcessButton = true;
                }
            }
        }

        private void ExecuteInitButtonCommand()
        {
            StringBuilder message = new StringBuilder();

            message.AppendLine("Do you really want to initialize equipment?");

            if (MessageBoxManager.ShowYesNoBox(message.ToString(), "INIT START") == MSGBOX_RESULT.OK)
            {
                PROCESS_RESULT result = MessageBoxManager.ShowProgressWindow("INIT PROCESSING", "Wait for a moments...", FuncNameHelper.INIT_PROCESS);

                if (result == PROCESS_RESULT.SUCCESS)
                {
                    IsEnableInitButton = false;
                    IsEnableProcessButton = true;
                }
            }
            
        }

        public OperationAutoViewModel()
        {
            _IsEnableInitButton = true;
            _IsEnableProcessButton = true;
        }

    }
}
