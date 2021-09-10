
using System.ComponentModel;
using System.Reflection;

namespace Scanlab.Sirius
{
    /// <summary>UI 유틸리티 기능들</summary>
    internal class UiHelper
    {
        internal static void PropertyBrowsable(System.Type type, string name, bool isShow)
        {
            BrowsableAttribute attribute = (BrowsableAttribute)TypeDescriptor.GetProperties(type)[name].Attributes[typeof(BrowsableAttribute)];
            attribute.GetType().GetField("browsable", BindingFlags.Instance | BindingFlags.NonPublic).SetValue((object)attribute, (object)isShow);
        }
    }
}
