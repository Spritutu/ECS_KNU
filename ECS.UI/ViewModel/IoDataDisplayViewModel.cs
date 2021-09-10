using ECS.UI.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECS.UI.ViewModel
{
    public class IoDataDisplayViewModel : INotifyPropertyChanged
    {
        private IoDataDisplay _IoDataDisplay;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public IoDataDisplayViewModel(IoDataDisplay ioDataDisplay)
        {
            _IoDataDisplay = ioDataDisplay;
        }

        public string Value
        {
            get { return _IoDataDisplay.Value; }
            set
            {
                _IoDataDisplay.Value = value;
                OnPropertyChanged("Value");
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
