using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    public partial class SiriusPreviewForm : Form
    {

        private IEntity entity;
        private StringCollection folderCol;

        public IEntity Entity
        {
            get => this.entity;
            private set => this.entity = value;
        }


        public SiriusPreviewForm()
        {
            InitializeComponent();
            this.folderCol = new StringCollection();
            this.CreateHeadersAndFillListView();
            string root = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo");
            this.PaintListView(root);
            this.folderCol.Add(root);
            this.listView1.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            this.txtDistance.Text = string.Format("{0:F3}", (object)Config.ClosedPathDistance);
        }
        private void CreateHeadersAndFillListView()
        {
            this.listView1.Columns.Add(new ColumnHeader()
            {
                Text = "Filename"
            });
            this.listView1.Columns.Add(new ColumnHeader()
            {
                Text = "Size"
            });
            this.siriusViewerForm1.Document = (IDocument)new DocumentDefault();
        }

        private void PaintListView(string root)
        {
            this.lblRoot.Text = root;
            try
            {
                if (string.IsNullOrEmpty(root))
                    return;
                DirectoryInfo directoryInfo = new DirectoryInfo(root);
                directoryInfo.GetDirectories();
                string[] extensions = new string[2]
                {
          ".dxf",
          ".plt"
                };
                FileInfo[] array = ((IEnumerable<FileInfo>)directoryInfo.GetFiles()).Where<FileInfo>((Func<FileInfo, bool>)(f => ((IEnumerable<string>)extensions).Contains<string>(f.Extension.ToLower()))).ToArray<FileInfo>();
                this.listView1.Items.Clear();
                this.listView1.BeginUpdate();
                foreach (FileInfo fileInfo in array)
                    this.listView1.Items.Add(new ListViewItem()
                    {
                        Text = fileInfo.Name,
                        Tag = (object)fileInfo.FullName,
                        SubItems = {
              new ListViewItem.ListViewSubItem()
              {
                Text = fileInfo.Length.ToString()
              }
            }
                    });
                this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
                this.listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                this.listView1.EndUpdate();
            }
            catch (Exception ex)
            {
                int num = (int)MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) => this.listView1_ItemSelectionChanged((object)this.listView1, e);

        private void listView1_ItemSelectionChanged(object sender, EventArgs e)
        {
            ListView listView = (ListView)sender;
            if (1 != listView.SelectedItems.Count)
                return;
            string str = listView.SelectedItems[0].Tag.ToString();
            if (listView.SelectedItems[0].ImageIndex != 0)
            {
                float closedPathDistance = Config.ClosedPathDistance;
                try
                {
                    IDocument document = this.siriusViewerForm1.Document;
                    document.Action.ActNew();
                    string extension = Path.GetExtension(str);
                    Config.ClosedPathDistance = float.Parse(this.txtDistance.Text);
                    if (string.Compare(extension, ".dxf", true) == 0)
                        document.Action.ActImportDxf(str, out this.entity);
                    else if (string.Compare(extension, ".plt", true) == 0)
                        document.Action.ActImportHPGL(str, out this.entity);
                    this.siriusViewerForm1.View.OnZoomFit();
                }
                finally
                {
                    Config.ClosedPathDistance = closedPathDistance;
                }
            }
            else
            {
                this.PaintListView(str);
                this.folderCol.Add(str);
            }
        }

        private void PropertyForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Escape)
                return;
            this.Close();
        }

    }
}
