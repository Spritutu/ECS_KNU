
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Scanlab.Sirius
{
    /// <summary>그룹 편집기 (group)</summary>
    internal class GroupEditor : UITypeEditor
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
            Group group1 = (Group)null;
            if (context.Instance is Group)
            {
                group1 = context.Instance as Group;
                views = group1.Views.Keys.ToList<IView>();
            }
            if (service != null && group1 != null)
            {
                Group group2 = (Group)group1.Clone();
                //using (GroupForm groupForm = new GroupForm(group1, views))
                //    value = service.ShowDialog((Form)groupForm) != DialogResult.OK ? (object)group2.Items : (object)groupForm.Entities.ToArray<IEntity>();
            }
            return value;
        }
    }
}
