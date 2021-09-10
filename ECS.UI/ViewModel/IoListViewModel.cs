using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ECS.UI.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using INNO6.IO;
using INNO6.Core;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Threading;

namespace ECS.UI.ViewModel
{
    public class IoListViewModel : ViewModelBase
    {
        private Dispatcher _Dispatcher = Dispatcher.CurrentDispatcher;
        private bool _IsIoListUpdate = true;
        private ObservableCollection<IoDataDisplay> filteredIoDataList;
        private ObservableCollection<IoDataDisplay> alIoDataList;


        private void ItemName_TextChanged()
        {
            LogHelper.Instance.UILog.DebugFormat("[{0}] Search_Click", this.GetType().Name);
            try
            {
                if (!string.IsNullOrEmpty(ItemName))
                {
                    filteredIoDataList = new ObservableCollection<IoDataDisplay>(IoDataList.Where(x => x.Name.Contains(ItemName)));
                    //_FilteredSVData = (from m in SVData.AsEnumerable()
                    //                   where m.Field<string>("NAME").Contains(ItemName)
                    //                   select m).CopyToDataTable<DataRow>();

                    LogHelper.Instance.UILog.DebugFormat("[{0}] Search_Click Done. ({1})", this.GetType().Name, ItemName);
                }
                else
                {
                    filteredIoDataList = alIoDataList;
                }

                IoDataList = filteredIoDataList;               
            }
            catch (Exception ex)
            {
                LogHelper.Instance.ErrorLog.DebugFormat("{0} -> {1}", this.GetType().Name, ex.Message);
            }

        }

        public CollectionViewSource ViewSource { get; set; }

        private ObservableCollection<IoDataDisplay> _ioDataList;
        public ObservableCollection<IoDataDisplay> IoDataList
        {
            get { return _ioDataList; }
            set
            {
                if (_ioDataList != value)
                {
                    _ioDataList = value;
                    RaisePropertyChanged("IoDataList");
                }
            }
        }      

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                if (_ItemName != value)
                {
                    _ItemName = value;
                    RaisePropertyChanged("ItemName");
                }
            }
        }

        public RelayCommand ItemNameChanged { get; private set; }

        public IoListViewModel()
        {
            Initialize();
            ItemNameChanged = new RelayCommand(() => ItemName_TextChanged());           
        }

        private void CallbackIoValueRead()
        {
            ObservableCollection<IoDataDisplay> ioList = IoDataList;

            foreach (var io in ioList)
            {
                Data data = DataManager.Instance.DataAccess.RemoteObject.DataList.Find((o) => (o.Name == io.Name));

                if(data != null)
                {
                    io.Value = data.Value.ToString();
                }
                else
                {
                    continue;
                }
            }

            if(!_Dispatcher.CheckAccess())
            {
                _Dispatcher.Invoke(new Action(() =>
                {
                    ViewSource.Source = ioList;
                    ViewSource.View.Refresh();
                }));
            }

        }

        private void Initialize()
        {
            IoDataList = new ObservableCollection<IoDataDisplay>();
            var _sortedIoData = from ioData in DataManager.Instance.DataAccess.RemoteObject.DataList
                                orderby ioData.Name ascending
                                select ioData;

            foreach (var item in _sortedIoData)
            {
                if (item == null) continue;

                IoDataList.Add(new IoDataDisplay()
                {
                    Name = item.Name,
                    DeviceModule = item.Module,
                    DriverName = item.DriverName,
                    Description = item.Description,
                    Type = item.Type.ToString(),
                    Direction = item.Direction.ToString(),
                    DefaultValue = item.DefaultValue,
                    DataResetTimeout = item.DataResetTimeout,
                    PollingTime = item.PollingTime,
                    Value = item.Value.ToString(),
                    Use = item.Use ? "YES" : "NO"
                });
            }

            ViewSource = new CollectionViewSource();
            alIoDataList = IoDataList;
            ViewSource.Source = IoDataList;
            ViewSource.View.Refresh();

            Start();
        }

        public void Start()
        {
            _IsIoListUpdate = true;

            Task.Run(new Action(() => {
                while (_IsIoListUpdate)
                {
                    CallbackIoValueRead();
                    Thread.Sleep(1000);
                }
            }));
            //_timer = new Timer(CallbackIoValueRead, IoDataList, 0, 1000);
        }

        public void Stop()
        {
            _IsIoListUpdate = false;
        }
    }
}
