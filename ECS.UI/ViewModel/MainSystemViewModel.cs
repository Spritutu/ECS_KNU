using Basler.Pylon;
using ECS.UI.Model;
using GalaSoft.MvvmLight;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using INNO6.IO;
using INNO6.IO.Service;
using System.Threading;
using INNO6.Core.Manager;
using GalaSoft.MvvmLight.CommandWpf;
using System.Text.RegularExpressions;

namespace ECS.UI.ViewModel
{
    public class MainSystemViewModel : ViewModelBase
    {
        private static readonly Regex _regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text

        #region IO DEFINE
        private const string IO_NAME_X_POSITION_INPUT = "oPMAC.dAxisX.SetPostion";
        private const string IO_NAME_Y_POSITION_INPUT = "oPMAC.dAxisY.SetPostion";

        private const string IO_NAME_X_VELOCITY_INPUT = "oPMAC.dAxisX.SetVelocity";
        private const string IO_NAME_Y_VELOCITY_INPUT = "oPMAC.dAxisY.SetVelocity";

        private const string IO_NAME_CH1_LED_OUTPUT = "oLed.iDataSet.Ch1";
        private const string IO_NAME_CH1_LED_OUTPUT_STATUS = "iLed.iData.Ch1";

        private const string IO_NAME_CH1_LED_ONOFF_STATUS = "iLed.iOnOff.Ch1";
        #endregion


        private RelayCommand<TextCompositionEventArgs> previewTextInputCommand;
        private RelayCommand<object> xJogPlusPreviewMouseLeftButtonDownCommand;
        private RelayCommand<object> xJogPlusPreviewMouseLeftButtonUpCommand;
        private RelayCommand<object> xJogMinusPreviewMouseLeftButtonDownCommand;
        private RelayCommand<object> xJogMinusPreviewMouseLeftButtonUpCommand;

        private RelayCommand<object> yJogPlusPreviewMouseLeftButtonDownCommand;
        private RelayCommand<object> yJogPlusPreviewMouseLeftButtonUpCommand;
        private RelayCommand<object> yJogMinusPreviewMouseLeftButtonDownCommand;
        private RelayCommand<object> yJogMinusPreviewMouseLeftButtonUpCommand;

        private RelayCommand<object> ch1LedOutputValueChangedCommand;
        private RelayCommand<RoutedEventArgs> ch1_LedOnOff_Command;

        private ICommand xAxisPositionMoveCommand;
        private ICommand xAxisPositionMoveStopCommand;
        private ICommand yAxisPositionMoveCommand;
        private ICommand yAxisPositionMoveStopCommand;
        private ICommand ch1_LedOn_Command;
        private ICommand ch1_LedOff_Command;

        private bool _RadioButtonXAxisChecked;
        private bool _RadioButtonYAxisChecked;
        private bool _RadioButtonZAxisChecked;
        private bool _RadioButtonTAxisChecked;
        private bool _RadioButtonRAxisChecked;

        private bool ch1_LedOn_Button_Enable;
        private bool ch1_LedOff_Button_Enable;

        private double ch1LedOutputValue;

        private bool xAxisPositionMoveEnable;
        private bool yAxisPositionMoveEnable;
        private bool xAxisPositionMoveStopEnable;
        private bool yAxisPositionMoveStopEnable;

        private string xAxisPositionInput;
        private string yAxisPositionInput;

        private string xAxisVelocityInput;
        private string yAxisVelocityInput;


        private ICommand buttonXAxisHommingClicked;
        private string buttonXAxisHommingContent;
        private bool buttonXAxisHommingEnable;

        private ICommand buttonYAxisHommingClicked;
        private string buttonYAxisHommingContent;
        private bool buttonYAxisHommingEnable;

        private ICommand buttonXAxisHomeStopClicked;
        private string buttonXAxisHomeStopContent;
        private bool buttonXAxisHomeStopEnable;

        private ICommand buttonYAxisHomeStopClicked;
        private string buttonYAxisHomeStopContent;
        private bool buttonYAxisHomeStopEnable;

        private string buttonXAxisJogPlusContent;
        private bool buttonXAxisJogPlusEnable;

        private string buttonXAxisJogMinusContent;
        private bool buttonXAxisJogMinusEnable;

        private string buttonYAxisJogPlusContent;
        private bool buttonYAxisJogPlusEnable;

        private string buttonYAxisJogMinusContent;
        private bool buttonYAxisJogMinusEnable;

        private string buttonXAxisJogStopContent;
        private bool buttonXAxisJogStopEnable;

        public MainSystemViewModel()
        {

            buttonXAxisHommingContent = "X-HOME";
            buttonXAxisHommingEnable = true;

            buttonYAxisHommingContent = "Y-HOME";
            buttonYAxisHommingEnable = true;

            buttonXAxisHomeStopContent = "X-STOP";
            buttonXAxisHomeStopEnable = true;

            buttonYAxisHomeStopContent = "Y-STOP";
            buttonYAxisHomeStopEnable = true;

            buttonXAxisJogPlusContent = "X-JOG+";
            buttonXAxisJogPlusEnable = true;

            buttonXAxisJogMinusContent = "X-JOG-";
            buttonXAxisJogMinusEnable = true;

            buttonYAxisJogPlusContent = "Y-JOG+";
            buttonYAxisJogPlusEnable = true;

            buttonYAxisJogMinusContent = "Y-JOG-";
            buttonYAxisJogMinusEnable = true;

            buttonXAxisJogStopContent = "X-JOG Stop";
            buttonXAxisJogStopEnable = true;

            xAxisPositionInput = "0";
            yAxisPositionInput = "0";

            xAxisVelocityInput = "0";
            yAxisVelocityInput = "0";

            xAxisPositionMoveEnable = true;
            yAxisPositionMoveEnable = true;

            xAxisPositionMoveStopEnable = false;
            yAxisPositionMoveStopEnable = false;

            Ch1LedOutputValue = DataManager.Instance.GET_INT_DATA(IO_NAME_CH1_LED_OUTPUT_STATUS, out bool _);

            DataManager.Instance.DataAccess.DataChangedEvent += DataAccess_DataChanged;

            UpdateCh1LedButtonEnable();
        }

        private void DataAccess_DataChanged(object sender, DataChangedEventHandlerArgs args)
        {
            Data data = args.Data;

            if(data.Name.Equals(IO_NAME_CH1_LED_ONOFF_STATUS))
            {
                UpdateCh1LedButtonEnable();
            }   
        }

        public ICommand Ch1_LedOn_Command
        {
            get
            {
                if (ch1_LedOn_Command == null)
                {
                    ch1_LedOn_Command = new DelegateCommand(Execute_Ch1_LedOn);
                }

                return ch1_LedOn_Command;
            }
        }


        public ICommand Ch1_LedOff_Command
        {
            get
            {
                if (ch1_LedOff_Command == null)
                {
                    ch1_LedOff_Command = new DelegateCommand(Execute_Ch1_LedOff);
                }

                return ch1_LedOff_Command;
            }
        }

        public RelayCommand<RoutedEventArgs> Ch1_LedOnOff_Command
        {
            get
            {
                if (ch1_LedOnOff_Command == null)
                {
                    ch1_LedOnOff_Command = new RelayCommand<RoutedEventArgs>(Execute_Ch1_LedOnOff);
                }

                return ch1_LedOnOff_Command;
            }
        }

        private void Execute_Ch1_LedOnOff(RoutedEventArgs args)
        {
            
        }

        private void UpdateCh1LedButtonEnable()
        {
            if (DataManager.Instance.GET_INT_DATA(IO_NAME_CH1_LED_ONOFF_STATUS, out bool _) == 0)
            {
                Ch1_LedOff_Button_Enable = false;
                Ch1_LedOn_Button_Enable = true;
            }
            else
            {
                Ch1_LedOff_Button_Enable = true;
                Ch1_LedOn_Button_Enable = false;
            }
        }

        private void Execute_Ch1_LedOn()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_LED_CH1_ON");
        }

        private void Execute_Ch1_LedOff()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_LED_CH1_OFF");
        }


        private void LaserInitCommand()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_LASER_INIT");
        }

        public ICommand XAxisPositionMoveCommand
        {
            get
            {
                if (xAxisPositionMoveCommand == null)
                {
                    xAxisPositionMoveCommand = new DelegateCommand(XAxisMoveToPosition);
                }

                return xAxisPositionMoveCommand;
            }
        }

        public ICommand XAxisPositionMoveStopCommand
        {
            get
            {
                if (xAxisPositionMoveStopCommand == null)
                {
                    xAxisPositionMoveStopCommand = new DelegateCommand(XAxisStopPositionMove);
                }

                return xAxisPositionMoveStopCommand;
            }
        }

        public ICommand YAxisPositionMoveCommand
        {
            get
            {
                if (yAxisPositionMoveCommand == null)
                {
                    yAxisPositionMoveCommand = new DelegateCommand(YAxisMoveToPosition);
                }

                return yAxisPositionMoveCommand;
            }
        }

        public ICommand YAxisPositionMoveStopCommand
        {
            get
            {
                if (yAxisPositionMoveStopCommand == null)
                {
                    yAxisPositionMoveStopCommand = new DelegateCommand(YAxisStopPositionMove);
                }

                return yAxisPositionMoveStopCommand;
            }
        }

        private void XAxisStopPositionMove()
        {

        }

        private void YAxisStopPositionMove()
        {

        }

        private void YAxisMoveToPosition()
        {
            double yPos = double.Parse(YAxisPositionInput);
            double yVel = double.Parse(YAxisVelocityInput);

            if(DataManager.Instance.SET_DOUBLE_DATA(IO_NAME_Y_POSITION_INPUT, yPos) && 
                DataManager.Instance.SET_DOUBLE_DATA(IO_NAME_Y_VELOCITY_INPUT, yVel))
            {
                FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_Y_AXIS_MOVE_TO_SETPOS");
            }
        }

        private void XAxisMoveToPosition()
        {
            double xPos = double.Parse(XAxisPositionInput);
            double xVel = double.Parse(XAxisVelocityInput);

            if (DataManager.Instance.SET_DOUBLE_DATA(IO_NAME_X_POSITION_INPUT, xPos) &&
                DataManager.Instance.SET_DOUBLE_DATA(IO_NAME_X_VELOCITY_INPUT, xVel))
            {
                FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_X_AXIS_MOVE_TO_SETPOS");
            }
        }

        
        public ICommand Ch1LedOutputValueChangedCommand
        {
            get
            {
                return this.ch1LedOutputValueChangedCommand ?? (this.ch1LedOutputValueChangedCommand = new RelayCommand<object>(ExecuteCh1LedOutputValueChangedCommand));
            }
        }

        private void ExecuteCh1LedOutputValueChangedCommand(object obj)
        {
            if (obj is RoutedPropertyChangedEventArgs<double>)
            {
                RoutedPropertyChangedEventArgs<double> eventArgs = obj as RoutedPropertyChangedEventArgs<double>;
                int value = Convert.ToInt32(eventArgs.NewValue);
                DataManager.Instance.SET_INT_DATA(IO_NAME_CH1_LED_OUTPUT, value);
            }         
        }

        public ICommand PreviewTextInputCommand
        {
            get
            {
                return this.previewTextInputCommand ?? (this.previewTextInputCommand = new RelayCommand<TextCompositionEventArgs>(ExecutePreviewTextInputCommand));
            }
        }

        public ICommand XJogPlusPreviewMouseLeftButtonDownCommand
        {
            get
            {
                return this.xJogPlusPreviewMouseLeftButtonDownCommand ?? (this.xJogPlusPreviewMouseLeftButtonDownCommand = new RelayCommand<object>(XJogPlusExecuteMouseLeftButtonDownCommand));
            }
        }

        public ICommand XJogPlusPreviewMouseLeftButtonUpCommand
        {
            get
            {
                return this.xJogPlusPreviewMouseLeftButtonUpCommand ?? (this.xJogPlusPreviewMouseLeftButtonUpCommand = new RelayCommand<object>(XJogPlusExecuteMouseLeftButtonUpCommand));
            }
        }

        public ICommand XJogMinusPreviewMouseLeftButtonDownCommand
        {
            get
            {
                return this.xJogMinusPreviewMouseLeftButtonDownCommand ?? (this.xJogMinusPreviewMouseLeftButtonDownCommand = new RelayCommand<object>(XJogMinusExecuteMouseLeftButtonDownCommand));
            }
        }


        public ICommand XJogMinusPreviewMouseLeftButtonUpCommand
        {
            get
            {
                return this.xJogMinusPreviewMouseLeftButtonUpCommand ?? (this.xJogMinusPreviewMouseLeftButtonUpCommand = new RelayCommand<object>(XJogMinusExecuteMouseLeftButtonUpCommand));
            }
        }

        public ICommand YJogPlusPreviewMouseLeftButtonDownCommand
        {
            get
            {
                return this.yJogPlusPreviewMouseLeftButtonDownCommand ?? (this.yJogPlusPreviewMouseLeftButtonDownCommand = new RelayCommand<object>(YJogPlusExecuteMouseLeftButtonDownCommand));
            }
        }



        public ICommand YJogPlusPreviewMouseLeftButtonUpCommand
        {
            get
            {
                return this.yJogPlusPreviewMouseLeftButtonUpCommand ?? (this.yJogPlusPreviewMouseLeftButtonUpCommand = new RelayCommand<object>(YJogPlusExecuteMouseLeftButtonUpCommand));
            }
        }

        public ICommand YJogMinusPreviewMouseLeftButtonDownCommand
        {
            get
            {
                return this.yJogMinusPreviewMouseLeftButtonDownCommand ?? (this.yJogMinusPreviewMouseLeftButtonDownCommand = new RelayCommand<object>(YJogMinusExecuteMouseLeftButtonDownCommand));
            }
        }

        public ICommand YJogMinusPreviewMouseLeftButtonUpCommand
        {
            get
            {
                return this.yJogMinusPreviewMouseLeftButtonUpCommand ?? (this.yJogMinusPreviewMouseLeftButtonUpCommand = new RelayCommand<object>(YJogMinusExecuteMouseLeftButtonUpCommand));
            }
        }


        private void XJogMinusExecuteMouseLeftButtonDownCommand(object obj)
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_X_AXIS_JOG_MINUS");
        }

        private void XJogMinusExecuteMouseLeftButtonUpCommand(object obj)
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_X_AXIS_JOG_STOP");
        }

        private void XJogPlusExecuteMouseLeftButtonDownCommand(object obj)
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_X_AXIS_JOG_PLUS");
        }

        private void XJogPlusExecuteMouseLeftButtonUpCommand(object obj)
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_X_AXIS_JOG_STOP");
        }

        private void YJogPlusExecuteMouseLeftButtonDownCommand(object obj)
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_Y_AXIS_JOG_PLUS");
        }

        private void YJogPlusExecuteMouseLeftButtonUpCommand(object obj)
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_Y_AXIS_JOG_STOP");
        }
        private void YJogMinusExecuteMouseLeftButtonDownCommand(object obj)
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_Y_AXIS_JOG_MINUS");
        }

        private void YJogMinusExecuteMouseLeftButtonUpCommand(object obj)
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_Y_AXIS_JOG_STOP");
        }

        private void ExecutePreviewTextInputCommand(TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public ICommand ButtonXAxisHommingClicked
        {
            get
            {
                if (buttonXAxisHommingClicked == null)
                {
                    buttonXAxisHommingClicked = new DelegateCommand(XAxisHomming);
                }

                return buttonXAxisHommingClicked;
            }
        }

        public ICommand ButtonYAxisHommingClicked
        {
            get
            {
                if (buttonYAxisHommingClicked == null)
                {
                    buttonYAxisHommingClicked = new DelegateCommand(YAxisHomming);
                }

                return buttonYAxisHommingClicked;
            }
        }

        public ICommand ButtonXAxisHomeStopClicked
        {
            get
            {
                if (buttonXAxisHomeStopClicked == null)
                {
                    buttonXAxisHomeStopClicked = new DelegateCommand(XAxisHomeStop);
                }

                return buttonXAxisHomeStopClicked;
            }
        }

        public ICommand ButtonYAxisHomeStopClicked
        {
            get
            {
                if (buttonYAxisHomeStopClicked == null)
                {
                    buttonYAxisHomeStopClicked = new DelegateCommand(YAxisHomeStop);
                }

                return buttonYAxisHomeStopClicked;
            }
        }


        private void XAxisHomming()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_X_AXIS_HOMMING");
        }

        private void YAxisHomming()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_Y_AXIS_HOMMING");
        }

        private void XAxisHomeStop()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_X_AXIS_HOME_STOP");
        }

        private void YAxisHomeStop()
        {
            FunctionManager.Instance.EXECUTE_FUNCTION_ASYNC("F_Y_AXIS_HOME_STOP");
        }



        public string XAxisPositionInput
        {
            get { return xAxisPositionInput; }
            set
            {
                xAxisPositionInput = value;
                RaisePropertyChanged("XAxisPositionInput");
            }
        }

        public string YAxisPositionInput
        {
            get { return yAxisPositionInput; }
            set
            {
                yAxisPositionInput = value;
                RaisePropertyChanged("YAxisPositionInput");
            }
        }

        public double Ch1LedOutputValue
        {
            get { return ch1LedOutputValue; }
            set
            {
                ch1LedOutputValue = value;
                RaisePropertyChanged("Ch1LedOutputValue");
            }
        }

        public string XAxisVelocityInput
        {
            get { return xAxisVelocityInput; }
            set
            {
                xAxisVelocityInput = value;
                RaisePropertyChanged("XAxisVelocityInput");
            }
        }

        public string YAxisVelocityInput
        {
            get { return yAxisVelocityInput; }
            set
            {
                yAxisVelocityInput = value;
                RaisePropertyChanged("YAxisVelocityInput");
            }
        }

        public string ButtonXAxisHommingContent
        {
            get { return buttonXAxisHommingContent; }
            set
            {
                buttonXAxisHommingContent = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonXAxisHommingContent");
            }
        }

        public bool ButtonXAxisHommingEnable
        {
            get { return buttonXAxisHommingEnable; }
            set
            {
                buttonXAxisHommingEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonXAxisHommingEnable");
            }
        }

        public string ButtonXAxisHomeStopContent
        {
            get { return buttonXAxisHomeStopContent; }
            set
            {
                buttonXAxisHomeStopContent = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonXAxisHomeStopContent");
            }
        }

        public bool ButtonXAxisHomeStopEnable
        {
            get { return buttonXAxisHomeStopEnable; }
            set
            {
                buttonXAxisHomeStopEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonXAxisHomeStopEnable");
            }
        }


        public string ButtonYAxisHomeStopContent
        {
            get { return buttonYAxisHomeStopContent; }
            set
            {
                buttonYAxisHomeStopContent = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonYAxisHomeStopContent");
            }
        }

        public bool ButtonYAxisHomeStopEnable
        {
            get { return buttonYAxisHomeStopEnable; }
            set
            {
                buttonYAxisHomeStopEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonYAxisHomeStopEnable");
            }
        }

        public string ButtonYAxisHommingContent
        {
            get { return buttonYAxisHommingContent; }
            set
            {
                buttonYAxisHommingContent = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonYAxisHommingContent");
            }
        }

        public bool ButtonYAxisHommingEnable
        {
            get { return buttonYAxisHommingEnable; }
            set
            {
                buttonYAxisHommingEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonYAxisHommingEnable");
            }
        }

        public string ButtonXAxisJogPlusContent
        {
            get { return buttonXAxisJogPlusContent; }
            set
            {
                buttonXAxisJogPlusContent = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonXAxisJogPlusContent");
            }
        }

        public bool ButtonXAxisJogPlusEnable
        {
            get { return buttonXAxisJogPlusEnable; }
            set
            {
                buttonXAxisJogPlusEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonXAxisJogPlusEnable");
            }
        }

        public string ButtonXAxisJogMinusContent
        {
            get { return buttonXAxisJogMinusContent; }
            set
            {
                buttonXAxisJogMinusContent = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonXAxisJogMinusContent");
            }
        }

        public bool ButtonXAxisJogMinusEnable
        {
            get { return buttonXAxisJogMinusEnable; }
            set
            {
                buttonXAxisJogMinusEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonXAxisJogMinusEnable");
            }
        }

        public string ButtonYAxisJogPlusContent
        {
            get { return buttonYAxisJogPlusContent; }
            set
            {
                buttonYAxisJogPlusContent = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonYAxisJogPlusContent");
            }
        }

        public bool ButtonYAxisJogPlusEnable
        {
            get { return buttonYAxisJogPlusEnable; }
            set
            {
                buttonYAxisJogPlusEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonYAxisJogPlusEnable");
            }
        }

        public string ButtonYAxisJogMinusContent
        {
            get { return buttonYAxisJogMinusContent; }
            set
            {
                buttonYAxisJogMinusContent = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonYAxisJogMinusContent");
            }
        }

        public bool ButtonYAxisJogMinusEnable
        {
            get { return buttonYAxisJogMinusEnable; }
            set
            {
                buttonYAxisJogMinusEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonYAxisJogMinusEnable");
            }
        }

        public string ButtonXAxisJogStopContent
        {
            get { return buttonXAxisJogStopContent; }
            set
            {
                buttonXAxisJogStopContent = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonXAxisJogStopContent");
            }
        }

        public bool ButtonXAxisJogStopEnable
        {
            get { return buttonXAxisJogStopEnable; }
            set
            {
                buttonXAxisJogStopEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("ButtonXAxisJogStopEnable");
            }
        }

        public bool XAxisPositionMoveEnable
        {
            get { return xAxisPositionMoveEnable; }
            set
            {
                xAxisPositionMoveEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("XAxisPositionMoveEnable");
            }
        }

        public bool YAxisPositionMoveEnable
        {
            get { return yAxisPositionMoveEnable; }
            set
            {
                yAxisPositionMoveEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("YAxisPositionMoveEnable");
            }
        }

        public bool XAxisPositionMoveStopEnable
        {
            get { return xAxisPositionMoveStopEnable; }
            set
            {
                xAxisPositionMoveStopEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("XAxisPositionMoveStopEnable");
            }
        }

        public bool YAxisPositionMoveStopEnable
        {
            get { return yAxisPositionMoveStopEnable; }
            set
            {
                yAxisPositionMoveStopEnable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("YAxisPositionMoveStopEnable");
            }
        }

        public bool Ch1_LedOff_Button_Enable
        {
            get { return ch1_LedOff_Button_Enable; }
            set
            {
                ch1_LedOff_Button_Enable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("Ch1_LedOff_Button_Enable");
            }
        }

        public bool Ch1_LedOn_Button_Enable
        {
            get { return ch1_LedOn_Button_Enable; }
            set
            {
                ch1_LedOn_Button_Enable = value;
                // RaisePropertyChanged MUST fire the same case-sensitive name of property
                RaisePropertyChanged("Ch1_LedOn_Button_Enable");
            }
        }

        public bool RadioButtonXAxisChecked{ get { return _RadioButtonXAxisChecked; } set { _RadioButtonXAxisChecked = value; RaisePropertyChanged("RadioButtonXAxisChecked"); } }
        public bool RadioButtonYAxisChecked { get { return _RadioButtonYAxisChecked; } set { _RadioButtonYAxisChecked = value; RaisePropertyChanged("RadioButtonYAxisChecked"); } }
        public bool RadioButtonZAxisChecked { get { return _RadioButtonZAxisChecked; } set { _RadioButtonZAxisChecked = value; RaisePropertyChanged("RadioButtonZAxisChecked"); } }
        public bool RadioButtonTAxisChecked { get { return _RadioButtonTAxisChecked; } set { _RadioButtonTAxisChecked = value; RaisePropertyChanged("RadioButtonTAxisChecked"); } }
        public bool RadioButtonRAxisChecked { get { return _RadioButtonRAxisChecked; } set { _RadioButtonRAxisChecked = value; RaisePropertyChanged("RadioButtonRAxisChecked"); } }


        private static bool IsTextAllowed(string text)
        {
            return !_regex.IsMatch(text);
        }
    }
}