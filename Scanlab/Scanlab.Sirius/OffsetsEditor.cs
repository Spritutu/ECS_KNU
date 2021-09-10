using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scanlab.Sirius
{
    /// <summary>offsets 배열 변환기 (group)</summary>
    internal class OffsetsEditor : UITypeEditor
    {
        public override UITypeEditorEditStyle GetEditStyle(
          ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        public override object EditValue(
          ITypeDescriptorContext context,
          System.IServiceProvider provider,
          object value)
        {
            IWindowsFormsEditorService service = provider.GetService(typeof(IWindowsFormsEditorService)) as IWindowsFormsEditorService;
            List<IView> views = (List<IView>)null;
            Group group = (Group)null;
            if (context.Instance is Group)
            {
                group = context.Instance as Group;
                views = group.Views.Keys.ToList<IView>();
            }
            Offset[] array = value as Offset[];
            if (service != null && array != null)
            {
                //using (OffsetsForm offsetsForm = new OffsetsForm(group, array, views))
                //{
                //    if (service.ShowDialog((Form)offsetsForm) == DialogResult.OK)
                //        value = (object)offsetsForm.Offsets.ToArray<Offset>();
                //}
            }
            return value;
        }
    }
}
