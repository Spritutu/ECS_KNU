using ECS.UI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace ECS.UI.View
{
    /// <summary>
    /// _3DModelView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Visual3DMainView : UserControl
    {
        public Visual3DMainView()
        {
            InitializeComponent();
            this.DataContext = new Visual3DMainViewModel(new FileDialogService(), view1);
        }
    }
}
