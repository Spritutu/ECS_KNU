using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ECS.UI
{
    public class ViewCache : UserControl
    {
        public ViewCache()
        {
            this.Unloaded += this.ViewCache_Unloaded;
        }

        void ViewCache_Unloaded(object sender, RoutedEventArgs e)
        {
            this.Unloaded -= this.ViewCache_Unloaded;
            this.Content = null;
        }

        private Type _contentType;

        public Type ContentType
        {
            get { return this._contentType; }

            set
            {
                if (this._contentType == value)
                {
                    return;
                }

                this._contentType = value;
                this.Content = ViewFactory.GetView(this._contentType);

            }
        }
    }
}
