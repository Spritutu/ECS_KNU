using System.Reflection;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    internal static class ListViewExtensions
    {
        public static void SetDoubleBuffered(this ListView listView, bool value) => listView.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object)listView, (object)value);
    }
}
