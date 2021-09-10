using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using INNO6.IO;
using Prism.Commands;
using ECS.Common.Helper;
using Microsoft.Win32;
using ECS.UI.Windows;
using ECS.Recipe.Model;
using ECS.Recipe;

namespace ECS.UI.ViewModel
{

    public class RecipeManagerViewModel : ViewModelBase
    {

        #region Define Private Variable
        private ObservableCollection<RECIPE_STEP> _RecipeStepList;

        private string _SelectedEnergyMode;
        private List<string> _EnergyModeList;

        private int _SelectedRecipeStepIndex = -1;
        private ICommand _RecipeStepSelectionChanged;
        private ICommand _EnergyModeSelectionChanged;

        private ICommand _NewButtonCommand;
        private ICommand _EditButtonCommand;
        private ICommand _DeleteButtonCommand;

        private ICommand _RecipeNewButtonCommand;
        private ICommand _RecipeOpenButtonCommand;
        private ICommand _RecipeSaveButtonCommand;

        private ICommand _ScanFileOpenButtonCommand;
        private ICommand _ConvertButtonCommand;

        private bool _NewButtonEnabled;
        private bool _EditButtonEnabled;
        private bool _DeleteButtonEnabled;

        private int _STEPID;
        private double _XPOSITION;
        private double _YPOSITION;
        private double _ZPOSITION;
        private double _TPOSITION;
        private double _RPOSITION;

        private double _LASER_POWER_PERCENT;
        private int _REPEAT_COUNT;
        private string _SCAN_FILEPATH;


        private string _CurrentRecipeName;
        private List<string> _RecipeFileList;
        private int _SelectedRecipeIndex;

        #endregion

        #region Define Public Properties


        public bool NewButtonEnabled { get { return _NewButtonEnabled; } set { _NewButtonEnabled = value; RaisePropertyChanged("NewButtonEnabled"); } }
        public bool EditButtonEnabled { get { return _EditButtonEnabled; } set { _EditButtonEnabled = value; RaisePropertyChanged("EditButtonEnabled"); } }
        public bool DeleteButtonEnabled { get { return _DeleteButtonEnabled; } set { _DeleteButtonEnabled = value; RaisePropertyChanged("DeleteButtonEnabled"); } }
        public int SelectedRecipeStepIndex { get { return _SelectedRecipeStepIndex; } set { _SelectedRecipeStepIndex = value; RaisePropertyChanged("SelectedRecipeStepIndex"); } }
        public string SelectedEnergyMode { get { return _SelectedEnergyMode; } set { _SelectedEnergyMode = value; RaisePropertyChanged("SelectedEnergyMode"); } }
        public List<string> EnergyModeList { get { return new List<string>() { "EGY NGR", "EGYBURST NGR", "HV NGR" }; } set { _EnergyModeList = value; } }
        public ICommand RecipeStepSelectionChanged { get { if (_RecipeStepSelectionChanged == null) { _RecipeStepSelectionChanged = new DelegateCommand<SelectionChangedEventArgs>(Execute_RecipeStepSelectionChanged); } return _RecipeStepSelectionChanged; } }
        public ICommand EnergyModeSelectionChanged { get { if (_EnergyModeSelectionChanged == null) { _EnergyModeSelectionChanged = new DelegateCommand(ExecuteEnergyModeSelectionChanged); } return _EnergyModeSelectionChanged; } }
        public ICommand NewButtonCommand { get { if (_NewButtonCommand == null) { _NewButtonCommand = new DelegateCommand(ExecuteNewButtonCommand); } return _NewButtonCommand; } }
        public ICommand EditButtonCommand { get { if (_EditButtonCommand == null) { _EditButtonCommand = new DelegateCommand(ExecuteEditButtonCommand); } return _EditButtonCommand; } }
        public ICommand DeleteButtonCommand { get { if (_DeleteButtonCommand == null) { _DeleteButtonCommand = new DelegateCommand(ExecuteDeleteButtonCommand); } return _DeleteButtonCommand; } }

        public ICommand RecipeNewButtonCommand { get { if (_RecipeNewButtonCommand == null) { _RecipeNewButtonCommand = new DelegateCommand(ExecuteRecipeNewButtonCommand); } return _RecipeNewButtonCommand; } }
        public ICommand RecipeOpenButtonCommand { get { if (_RecipeOpenButtonCommand == null) { _RecipeOpenButtonCommand = new DelegateCommand(ExecuteRecipeOpenButtonCommand); } return _RecipeOpenButtonCommand; } }
        public ICommand RecipeSaveButtonCommand { get { if (_RecipeSaveButtonCommand == null) { _RecipeSaveButtonCommand = new DelegateCommand(ExecuteRecipeSaveButtonCommand); } return _RecipeSaveButtonCommand; } }

        public ICommand ScanFileOpenButtonCommand { get { if (_ScanFileOpenButtonCommand == null) { _ScanFileOpenButtonCommand = new DelegateCommand(ExecuteScanFileOpenButtonCommand); } return _ScanFileOpenButtonCommand; } }
        
        public ICommand ConvertButtonCommand { get { if (_ConvertButtonCommand == null) { _ConvertButtonCommand = new DelegateCommand(ExecuteConvertButtonCommand); } return _ConvertButtonCommand; } }


        public ObservableCollection<RECIPE_STEP> RecipeStepList { get { return _RecipeStepList; } set { _RecipeStepList = value; RaisePropertyChanged("RecipeStepList"); } }

        public int STEPID { get { return _STEPID; } set { _STEPID = value; RaisePropertyChanged("STEPID"); } }
        public double XPOSITION { get { return _XPOSITION; } set { _XPOSITION = value; RaisePropertyChanged("XPOSITION"); } }
        public double YPOSITION { get { return _YPOSITION; } set { _YPOSITION = value; RaisePropertyChanged("YPOSITION"); } }
        public double ZPOSITION { get { return _ZPOSITION; } set { _ZPOSITION = value; RaisePropertyChanged("ZPOSITION"); } }
        public double TPOSITION { get { return _TPOSITION; } set { _TPOSITION = value; RaisePropertyChanged("TPOSITION"); } }
        public double RPOSITION { get { return _RPOSITION; } set { _RPOSITION = value; RaisePropertyChanged("RPOSITION"); } }

        public double LASER_POWER_PERCENT { get { return _LASER_POWER_PERCENT; } set { _LASER_POWER_PERCENT = value; RaisePropertyChanged("LASER_POWER_PERCENT"); } }
        public int REPEAT_COUNT { get { return _REPEAT_COUNT; } set { _REPEAT_COUNT = value; RaisePropertyChanged("REPEAT_COUNT"); } }
        public string SCAN_FILEPATH { get { return _SCAN_FILEPATH; } set { _SCAN_FILEPATH = value; RaisePropertyChanged("SCAN_FILEPATH"); } }

        public string CurrentRecipeName { get { return _CurrentRecipeName; } set { _CurrentRecipeName = value; RaisePropertyChanged("CurrentRecipeName"); } }
        public int SelectedRecipeIndex { get { return _SelectedRecipeIndex; } set { _SelectedRecipeIndex = value; RaisePropertyChanged("SelectedRecipeIndex"); } }
        public List<string> RecipeFileList { get { return _RecipeFileList; } set { _RecipeFileList = value; RaisePropertyChanged("RecipeFileList"); } }
        #endregion

        #region Define Constructor
        public RecipeManagerViewModel()
        {
            Initialize();

            NewButtonEnabled = true;
            EditButtonEnabled = true;
            DeleteButtonEnabled = true;

            CurrentRecipeName = DataManager.Instance.GET_STRING_DATA(IoNameHelper.V_STR_SYS_CURRENT_RECIPE, out _);
        }

        #endregion

        #region Define Private Method

        private void Initialize()
        {
            RecipeStepList = new ObservableCollection<RECIPE_STEP>(RecipeManager.Instance.GET_RECIPE_STEP_LIST());
        }

        private void UpdateRecipeStepList()
        {
            RecipeStepList = new ObservableCollection<RECIPE_STEP>(RecipeManager.Instance.GET_RECIPE_STEP_LIST());
        }


        private void Execute_RecipeStepSelectionChanged(SelectionChangedEventArgs args)
        {
            if (SelectedRecipeStepIndex < 0)
            {
                //MessageBoxManager.ShowMessageBox(string.Format("RECIPE STEP 열이 선택되지 않았습니다. : Selected Row = {0}", SelectedRecipeStepIndex), true);
                return;
            }

            RECIPE_STEP step = RecipeStepList[SelectedRecipeStepIndex];

            STEPID = step.STEP_ID;
            XPOSITION = step.X_POS;
            YPOSITION = step.Y_POS;
            ZPOSITION = step.Z_POS;
            TPOSITION = step.T_POS;
            RPOSITION = step.R_POS;
            LASER_POWER_PERCENT = step.POWER_PERCENT;
            REPEAT_COUNT = step.REPEAT;
            SCAN_FILEPATH = step.SCAN_FILE;
        }

        private void ExecuteEnergyModeSelectionChanged()
        {

        }

        private void ExecuteNewButtonCommand()
        {

            if (RecipeManager.Instance.IS_EXIST_STEPID(STEPID))
            {
                MessageBoxManager.ShowMessageBox(string.Format("Same STEPID Aready Exist. : STEPID = {0}", STEPID), true);
                return;
            }

            RECIPE_STEP step = new RECIPE_STEP()
            {
                STEP_ID = STEPID,
                X_POS = XPOSITION,
                Y_POS = YPOSITION,
                Z_POS = ZPOSITION,
                T_POS = TPOSITION,
                R_POS = RPOSITION,
                POWER_PERCENT = LASER_POWER_PERCENT,
                REPEAT = REPEAT_COUNT,
                SCAN_FILE = SCAN_FILEPATH
            };

            RecipeManager.Instance.INSERT_STEP(step);
            UpdateRecipeStepList();
        }

        private void ExecuteEditButtonCommand()
        {
            if (SelectedRecipeStepIndex < 0)
            {
                MessageBoxManager.ShowMessageBox(string.Format("RECIPE STEP 열이 선택되지 않았습니다. : Selected Row = {0}", SelectedRecipeStepIndex), true);
                return;
            }

            RECIPE_STEP step = RecipeStepList[SelectedRecipeStepIndex];

            step.X_POS = XPOSITION;
            step.Y_POS = YPOSITION;
            step.Z_POS = ZPOSITION;
            step.T_POS = TPOSITION;
            step.R_POS = RPOSITION;
            step.POWER_PERCENT = LASER_POWER_PERCENT;
            step.REPEAT = REPEAT_COUNT;
            step.SCAN_FILE = SCAN_FILEPATH;

            RecipeManager.Instance.EDIT_STEP(step);
            UpdateRecipeStepList();
        }

        private void ExecuteDeleteButtonCommand()
        {
            if (SelectedRecipeStepIndex < 0)
            {
                MessageBoxManager.ShowMessageBox(string.Format("RECIPE STEP 열이 선택되지 않았습니다. : Selected Row = {0}", SelectedRecipeStepIndex), true);
                return;
            }

            RecipeManager.Instance.DELETE_STEP(STEPID);
            UpdateRecipeStepList();
        }

        private void ExecuteRecipeNewButtonCommand()
        {
            if (MessageBoxManager.ShowNewRecipeWindow("New Recipe", out string newRecipeName) == MSGBOX_RESULT.OK)
            {
                CurrentRecipeName = newRecipeName;

                DataManager.Instance.SET_STRING_DATA(IoNameHelper.V_STR_SYS_CURRENT_RECIPE, newRecipeName);
                DataManager.Instance.CHANGE_DEFAULT_DATA(IoNameHelper.V_STR_SYS_CURRENT_RECIPE, newRecipeName);

                RecipeManager.Instance.SAVE_RECIPE_FILE(newRecipeName, Common.GetInstance().UserAccount);

                UpdateRecipeStepList();
            }
        }

        private void ExecuteRecipeOpenButtonCommand()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = ".rcp";
            dlg.Filter = "Recipe Files (*.rcp)|*.rcp";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                RecipeManager.Instance.OPEN_RECIPE_FILE(dlg.FileName, out string recipeName);
                CurrentRecipeName = recipeName;
                DataManager.Instance.SET_STRING_DATA(IoNameHelper.V_STR_SYS_CURRENT_RECIPE, recipeName);
                DataManager.Instance.CHANGE_DEFAULT_DATA(IoNameHelper.V_STR_SYS_CURRENT_RECIPE, recipeName);
                UpdateRecipeStepList();
            }
        }

        private void ExecuteRecipeSaveButtonCommand()
        {
            RecipeManager.Instance.SAVE_RECIPE_FILE(CurrentRecipeName, Common.GetInstance().UserAccount);
        }

        private void ExecuteScanFileOpenButtonCommand()
        {
            OpenFileDialog dlg = new OpenFileDialog();

            dlg.DefaultExt = ".dxf";
            dlg.Filter = "DXF Files (*.dxf)|*.dxf|All Files (*.*)|*.*";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                SCAN_FILEPATH = dlg.FileName;
            }
        }

        private void ExecuteConvertButtonCommand()
        {
            string convertFile = string.Empty;
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.DefaultExt = ".sirius";
            dlg.Filter = "SIRIUS Files (*.sirius)|*.sirius";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                convertFile = dlg.FileName;

                //RecipeManager.Instance.CONVERT_RECIPE(convertFile, convertFile.TrimEnd(new char[] { '.' }));
            }
        }
        #endregion


    }
}