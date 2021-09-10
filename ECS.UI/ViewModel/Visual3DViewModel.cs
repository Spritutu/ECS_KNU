using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;

using HelixToolkit.Wpf;

namespace ECS.UI.ViewModel
{
    public class Visual3DViewModel : ViewModelBase
    {
        public IEnumerable<Visual3DViewModel> Children
        {
            get
            {
                var mv = this.element as ModelVisual3D;
                if (mv != null)
                {
                    if (mv.Content != null)
                    {
                        yield return new Visual3DViewModel(mv.Content);
                    }

                    foreach (var mc in mv.Children)
                    {
                        yield return new Visual3DViewModel(mc);
                    }
                }

                var mg = this.element as Model3DGroup;
                if (mg != null)
                {
                    foreach (var mc in mg.Children)
                    {
                        yield return new Visual3DViewModel(mc);
                    }
                }

                var gm = this.element as GeometryModel3D;
                if (gm != null)
                {
                    yield return new Visual3DViewModel(gm.Geometry);
                }

                //int n = VisualTreeHelper.GetChildrenCount(element);
                //for (int i = 0; i < n; i++)
                //    yield return new VisualViewModel(VisualTreeHelper.GetChild(element, i));
                foreach (DependencyObject c in LogicalTreeHelper.GetChildren(this.element))
                {
                    yield return new Visual3DViewModel(c);
                }
            }
        }

        public string Name
        {
            get
            {
                return this.element.GetName();
            }
        }

        public string TypeName
        {
            get
            {
                return this.element.GetType().Name;
            }
        }

        public Brush Brush
        {
            get
            {
                if (this.element.GetType() == typeof(ModelVisual3D))
                    return Brushes.Yellow;
                if (this.element.GetType() == typeof(GeometryModel3D))
                    return Brushes.Green;
                if (this.element.GetType() == typeof(Model3DGroup))
                    return Brushes.Blue;
                if (this.element.GetType() == typeof(Visual3D))
                    return Brushes.Gray;
                if (this.element.GetType() == typeof(Model3D))
                    return Brushes.Black;
                return null;
            }
        }

        public override string ToString()
        {
            return this.element.GetType().ToString();
        }

        private DependencyObject element;

        public Visual3DViewModel(DependencyObject e)
        {
            this.element = e;
        }
    }
}
