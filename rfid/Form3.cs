using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace rfid
{
    public partial class Form3 : Form
    {
        private static DataTable dttag = new DataTable();
        public Form3()
        {
            InitializeComponent();
            if (dttag.Columns.Count == 0)
            {
                DataColumn dc1 = new DataColumn("EPC", Type.GetType("System.String"));
                dttag.Columns.Add(dc1);
                DataColumn dc2 = new DataColumn("R1 count", Type.GetType("System.String"));
                dttag.Columns.Add(dc2);
                DataColumn dc3 = new DataColumn("R1 rate", Type.GetType("System.String"));
                dttag.Columns.Add(dc3);
                DataColumn dc4 = new DataColumn("R2 count", Type.GetType("System.String"));
                dttag.Columns.Add(dc4);
                DataColumn dc5 = new DataColumn("R2 rate", Type.GetType("System.String"));
                dttag.Columns.Add(dc5);
                DataColumn dc6 = new DataColumn("R3 count", Type.GetType("System.String"));
                dttag.Columns.Add(dc6);
                DataColumn dc7 = new DataColumn("R3 rate", Type.GetType("System.String"));
                dttag.Columns.Add(dc7);
                DataColumn dc8 = new DataColumn("R4 count", Type.GetType("System.String"));
                dttag.Columns.Add(dc8);
                DataColumn dc9 = new DataColumn("R4 rate", Type.GetType("System.String"));
                dttag.Columns.Add(dc9);
            }
            
            
            for (int i = 0; i < Form1.dt.Rows.Count; ++i)
            {
                string tt = Convert.ToString(Form1.dt.Rows[i][0]);
                if (dttag.Select("EPC='"+tt+"'").Length == 0)
                {
                    
                    
                    DataRow drt = dttag.NewRow();
                    drt["EPC"] = Form1.dt.Rows[i][0];
                    switch (Convert.ToString(Form1.dt.Rows[i][4]))
                    {
                        case "Reader #1":
                            drt["R1 count"] = 1;
                            drt["R2 count"] = 0;
                            drt["R3 count"] = 0;
                            drt["R4 count"] = 0;
                            break;
                        case "Reader #2":
                            drt["R1 count"] = 0;
                            drt["R2 count"] = 1;
                            drt["R3 count"] = 0;
                            drt["R4 count"] = 0;
                            break;
                        case "Reader #3":
                            drt["R1 count"] = 0;
                            drt["R2 count"] = 0;
                            drt["R3 count"] = 1;
                            drt["R4 count"] = 0;
                            break;
                        case "Reader #4":
                            drt["R1 count"] = 0;
                            drt["R2 count"] = 0;
                            drt["R3 count"] = 0;
                            drt["R4 count"] = 1;
                            break;
                        default:
                            break;
                    }

                    dttag.Rows.Add(drt);
                }
                else
                {
                    DataRow[] dttagr = dttag.Select("EPC='"+tt+"'");
                    switch (Convert.ToString(Form1.dt.Rows[i][4]))
                    {
                        case "Reader #1":
                            dttagr[0][1] = Convert.ToUInt64(dttagr[0][1]) + 1;
                            break;
                        case "Reader #2":
                            dttagr[0][3] = Convert.ToUInt64(dttagr[0][3]) + 1;
                            break;
                        case "Reader #3":
                            dttagr[0][5] = Convert.ToUInt64(dttagr[0][5]) + 1;
                            break;
                        case "Reader #4":
                            dttagr[0][7] = Convert.ToUInt64(dttagr[0][7]) + 1;
                            break;
                        default:
                            break;
                    }
                     
                }
            }
           TimeSpan tst = Form1.stw.Elapsed;
            if (tst.Hours * 3600 + tst.Minutes * 60 + tst.Seconds != 0)
            {
                for (int i = 0; i < dttag.Rows.Count; ++i)
                {
                    long b = Convert.ToInt32(dttag.Rows[i][1]);
                    long c = tst.Hours * 3600 + tst.Minutes * 60 + tst.Seconds;
                    
                    dttag.Rows[i][2] = (Convert.ToDouble(dttag.Rows[i][1])) / (tst.Hours * 3600 + tst.Minutes * 60 + tst.Seconds);//dttag.Rows[i][2] = 
                    dttag.Rows[i][4] = (Convert.ToDouble(dttag.Rows[i][3])) / (tst.Hours * 3600 + tst.Minutes * 60 + tst.Seconds);
                    dttag.Rows[i][6] = (Convert.ToDouble(dttag.Rows[i][5])) / (tst.Hours * 3600 + tst.Minutes * 60 + tst.Seconds);
                    dttag.Rows[i][8] = (Convert.ToDouble(dttag.Rows[i][7])) / (tst.Hours * 3600 + tst.Minutes * 60 + tst.Seconds);
                }
            }
            
            dataGridView1.DataSource = dttag;
        }
        //private bool indtornot(string st)
        //{
            
        //        if (dttag.Rows[i][1] == st)
        //            return true;

            
        //}
        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            dttag.Rows.Clear();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            // sfd.ShowDialog();
            sfd.Filter = "CSV文件|*.CSV";
            sfd.InitialDirectory = "C:\\";
            // sfd.InitialDirectory = "e:\\";
            //sfd.InitialDirectory = "E:\\";
            //sfd.Filter = "txt文件(*.txt)|*.txt";
            if (sfd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                string fileName = sfd.FileName;
                Form1.SaveCSV(dttag, fileName);//SaveCSV(dt, fileName);
                //string localFilePath = sfd.FileName.ToString(); //获得文件路径 
                //StreamWriter fin;
                //fin = new StreamWriter(localFilePath, true);
                //fin.Write(dt);
                //fin.Close();
            }
        }

        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "CSV文件|*.CSV";
            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                dataGridView1.DataSource = null;
                string fileName = ofd.FileName;
                dataGridView1.DataSource =Form1.OpenCSV(fileName);
                MessageBox.Show("成功显示CSV数据！");
            }
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            // sfd.ShowDialog();
            sfd.Filter = "CSV文件|*.CSV";
            sfd.InitialDirectory = "C:\\";
            // sfd.InitialDirectory = "e:\\";
            //sfd.InitialDirectory = "E:\\";
            //sfd.Filter = "txt文件(*.txt)|*.txt";
            if (sfd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            else
            {
                string fileName = sfd.FileName;
                Form1.SaveCSV(dttag, fileName);
            }
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
            dttag.Rows.Clear();
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(dataGridView1.SelectedCells) != "")
                Clipboard.SetDataObject(dataGridView1.GetClipboardContent());
        }

        private void 复制ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(dataGridView1.SelectedCells) != "")
                Clipboard.SetDataObject(dataGridView1.GetClipboardContent());
        }
    }
}
