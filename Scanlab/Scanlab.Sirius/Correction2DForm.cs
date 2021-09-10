using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scanlab.Sirius
{
    public partial class Correction2DForm : Form
    {
        public Correction2DForm(RtcCorrection2D rtcCorrection)
        {
            InitializeComponent();

            this.RtcCorrection = rtcCorrection;
            this.RtcCorrection.OnResult += new ResultEventHandler(this.Correction_OnResult);
            this.RefreshData();
        }

        private void Correction2DForm_Shown(object sender, EventArgs e) => this.RefreshData();

        private void Correction2DForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            EventHandler onClose = this.OnClose;
            if (onClose == null)
                return;
            onClose((object)this, EventArgs.Empty);
        }

        /// <summary>RtcCorrection2D 객체</summary>
        public RtcCorrection2D RtcCorrection { get; private set; }

        /// <summary>Apply 버튼 클릭시 이벤트 핸들러</summary>
        public event EventHandler OnApply;

        /// <summary>창(폼) 닫힐시 이벤트 핸들러</summary>
        public event EventHandler OnClose;

        public void RefreshData()
        {
            this.numRows.Value = (Decimal)this.RtcCorrection.Rows;
            this.numCols.Value = (Decimal)this.RtcCorrection.Cols;
            this.txtInterval.Text = string.Format("{0:F5}", (object)this.RtcCorrection.Interval);
            this.txtSource.Text = this.RtcCorrection.SourceCorrectionFile;
            this.txtTarget.Text = this.RtcCorrection.TargetCorrectionFile;
            this.dataGridView1.SuspendLayout();
            this.dataGridView1.Rows.Clear();
            this.dataGridView1.Columns.Clear();
            int length1 = this.RtcCorrection.Data.GetLength(0);
            int length2 = this.RtcCorrection.Data.GetLength(1);
            this.dataGridView1.ColumnCount = length2;
            for (int index1 = 0; index1 < length1; ++index1)
            {
                DataGridViewRow dataGridViewRow = new DataGridViewRow();
                dataGridViewRow.Height = 40;
                dataGridViewRow.CreateCells(this.dataGridView1);
                for (int index2 = 0; index2 < length2; ++index2)
                {
                    Vector2 vector2 = this.RtcCorrection.Data[index1, index2].Reference - this.RtcCorrection.Data[index1, index2].Measured;
                    dataGridViewRow.Cells[index2].Value = (object)string.Format("{0:F3} {1}{2:F3} ", (object)vector2.X, (object)Environment.NewLine, (object)vector2.Y);
                }
                this.dataGridView1.Rows.Add(dataGridViewRow);
            }
            this.dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            for (int index = 0; index < length2; ++index)
            {
                this.dataGridView1.Columns[index].HeaderText = (index + 1).ToString();
                this.dataGridView1.Columns[index].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            for (int index = 0; index < length1; ++index)
            {
                this.dataGridView1.Rows[index].HeaderCell.Value = (object)(index + 1).ToString();
                this.dataGridView1.Rows[index].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            }
            this.dataGridView1.ResumeLayout();
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtTarget.Text))
            {
                int num1 = (int)MessageBox.Show("Please select target correction file !", "Scanner Field Correction 2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                if (MessageBox.Show("Do you really want to convert correction file ?", "Scanner Field Correction 2D", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                this.txtResult.Text = string.Empty;
                if (this.RtcCorrection.Convert())
                    return;
                int num2 = (int)MessageBox.Show("Fail to convert correction file !", "Scanner Field Correction 2D", MessageBoxButtons.OK, MessageBoxIcon.Hand);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            EventHandler onApply = this.OnApply;
            if (onApply == null)
                return;
            onApply((object)this, EventArgs.Empty);
        }

        private void Correction_OnResult(object sender, bool success, string result)
        {
            this.tabControl1.SelectedIndex = 1;
            this.txtResult.Text = result;
            if (!success)
                return;
            int num = (int)MessageBox.Show("Success to convert correction file !", "Scanner Field Correction 2D", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void btnSourceFile_Click(object sender, EventArgs e)
        {
            this.openFileDialog1.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction");
            this.openFileDialog1.Filter = "ct5 correction files(*.ct5)|*.ct5|All files (*.*)|*.*";
            if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            this.txtSource.Text = this.openFileDialog1.FileName;
            this.RtcCorrection.SourceCorrectionFile = this.txtSource.Text;
        }

        private void btnTargetFile_Click(object sender, EventArgs e)
        {
            this.saveFileDialog1.InitialDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "correction");
            this.saveFileDialog1.Filter = "ct5 correction files(*.ct5)|*.ct5|All files (*.*)|*.*";
            if (this.saveFileDialog1.ShowDialog() != DialogResult.OK)
                return;
            this.txtTarget.Text = this.saveFileDialog1.FileName;
            this.RtcCorrection.TargetCorrectionFile = this.txtTarget.Text;
        }

        private void numRows_ValueChanged(object sender, EventArgs e)
        {
        }

        private void importFromToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Import Correction Data ...";
            openFileDialog.Filter = "txt files(*.txt)|*.txt|dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;
            int num1 = 0;
            using (StreamReader streamReader = new StreamReader(openFileDialog.FileName))
            {
                while (!streamReader.EndOfStream)
                {
                    string source = streamReader.ReadLine();
                    if (!string.IsNullOrEmpty(source) && source.ElementAt<char>(0) != ';')
                    {
                        if (source.ElementAt<char>(0) == '#')
                        {
                            string[] strArray = source.Split(',');
                            int length1 = int.Parse(strArray[1]);
                            int length2 = int.Parse(strArray[2]);
                            this.RtcCorrection.Data = new CorrectionData2D[length1, length2];
                            this.RtcCorrection.Rows = length1;
                            this.RtcCorrection.Cols = length2;
                        }
                        else
                        {
                            string[] strArray = source.Split(',', ':');
                            int index1 = int.Parse(strArray[0]);
                            int index2 = int.Parse(strArray[1]);
                            this.RtcCorrection.Data[index1, index2].Reference = new Vector2(float.Parse(strArray[2]), float.Parse(strArray[3]));
                            this.RtcCorrection.Data[index1, index2].Measured = new Vector2(float.Parse(strArray[4]), float.Parse(strArray[5]));
                            ++num1;
                        }
                    }
                }
                this.RefreshData();
                int num2 = (int)MessageBox.Show(string.Format("Correction data : {0} has read from {1}", (object)num1, (object)openFileDialog.FileName), "Import", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void exportToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Export Correction Data ...";
            saveFileDialog.Filter = "txt files(*.txt)|*.txt|dat files (*.dat)|*.dat|All files (*.*)|*.*";
            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;
            int num1 = 0;
            using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
            {
                streamWriter.WriteLine("; 2020 copyright to spirallab.sirius");
                streamWriter.WriteLine("; data format : row, col, reference, measured");
                streamWriter.WriteLine(string.Format("# correction data counts, {0}, {1}", (object)this.RtcCorrection.Rows, (object)this.RtcCorrection.Cols));
                for (int index1 = 0; index1 < this.RtcCorrection.Rows; ++index1)
                {
                    for (int index2 = 0; index2 < this.RtcCorrection.Cols; ++index2)
                    {
                        streamWriter.WriteLine(string.Format("{0}, {1} : {2},  {3}", (object)index1, (object)index2, (object)this.RtcCorrection.Data[index1, index2].ReferenceToString(), (object)this.RtcCorrection.Data[index1, index2].MeasuredToString()));
                        ++num1;
                    }
                }
                int num2 = (int)MessageBox.Show(string.Format("Correction data : {0} has written to {1}", (object)num1, (object)saveFileDialog.FileName), "Export", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            if (e.RowIndex < 0)
                ;
        }

        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            if (rowIndex < 0 || columnIndex < 0)
                return;
            string[] strArray = ((string)this.dataGridView1.Rows[rowIndex].Cells[columnIndex].Value).Split('\r', ',');
            float x = float.Parse(strArray[0]);
            float y = float.Parse(strArray[1]);
            CorrectionData2D correctionData2D = this.RtcCorrection.Data[rowIndex, columnIndex];
            correctionData2D.Measured = correctionData2D.Reference - new Vector2(x, y);
            this.RtcCorrection.Data[rowIndex, columnIndex] = correctionData2D;
        }

        private void DataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            int columnIndex = e.ColumnIndex;
            int rowIndex = e.RowIndex;
            if (rowIndex < 0 || columnIndex < 0)
                return;
            this.dataGridView1.SuspendLayout();
            DataGridViewRow row = this.dataGridView1.Rows[rowIndex];
            string[] strArray = ((string)row.Cells[columnIndex].Value).Split('\r', ',', ';', '\t');
            float num1 = new Vector2(float.Parse(strArray[0]), float.Parse(strArray[1])).Length();
            if ((double)num1 > 0.0500000007450581)
                num1 = 0.05f;
            int num2 = (int)((double)byte.MaxValue - (double)byte.MaxValue * (double)num1 / 0.0500000007450581);
            Color color1 = Color.FromArgb(num2, num2, num2);
            Color color2 = Color.FromArgb((int)byte.MaxValue - num2, (int)byte.MaxValue - num2, (int)byte.MaxValue - num2);
            row.Cells[columnIndex].Style.BackColor = color1;
            row.Cells[columnIndex].Style.ForeColor = color2;
            this.dataGridView1.ResumeLayout();
        }
    }
}
