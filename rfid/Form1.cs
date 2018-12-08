using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Impinj.OctaneSdk;
using System.Diagnostics;
using System.Threading;
using System.Timers;


namespace rfid
{
    public partial class Form1 : Form
    {
        static public DataTable dt = new DataTable();//显示表格
        static public bool dtLock = false;
        //BindingSource bs = new BindingSource();
        
        bool start_or_stop =true;//start 按钮切换
        //public bool detail_ornot = false;
        public static Stopwatch stw = new Stopwatch();// 需要using system.diagnostics。用来计时。
       // DataView dv =new DataView(); //去除dt空行，在timer_tick中有剩下两行代码
       int maxMin = 0;  //定时器的最大读取分钟数
       int maxSec = 0;  //定时器的最大读取秒数
       static int c1 = 0; static int c2 = 0; static int c3 = 0; static int c4 = 0;//四个读写器分别的读取数目


        public Form1()
        {
            InitializeComponent();
            maxMin = int.Parse(textBox1.Text) / 60;
            maxSec = int.Parse(textBox1.Text) % 60;
        }
        
        private void Form1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
            DataColumn dc1 = new DataColumn("EPC", Type.GetType("System.String"));
            dt.Columns.Add(dc1);            
            DataColumn dc2 = new DataColumn("RSS", Type.GetType("System.String"));
            dt.Columns.Add(dc2);
            DataColumn dc3 = new DataColumn("Frequency", Type.GetType("System.String"));
            dt.Columns.Add(dc3);
            DataColumn dc4 = new DataColumn("Phase", Type.GetType("System.String"));
            dt.Columns.Add(dc4);
            DataColumn dc5 = new DataColumn("Reader", Type.GetType("System.String"));
            dt.Columns.Add(dc5);
            //DataColumn dc6 = new DataColumn("Address", Type.GetType("System.String"));
            //dt.Columns.Add(dc6);
            DataColumn dc6 = new DataColumn("Antenna", Type.GetType("System.String"));
            dt.Columns.Add(dc6);
            DataColumn dc7 = new DataColumn("SeenTime", Type.GetType("System.String"));
            dt.Columns.Add(dc7);
            dataGridView1.ReadOnly = true;
           
            dataGridView1.DataSource = dt;
                              
        }
       

        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void applysettings_Click(object sender, EventArgs e)
        {

            Form form = new Form2();
            form.ShowDialog();
            if (Form2.apply_ornot == true)
            {
                start.Enabled = true;  
            }
           
            
            
        }

        private static void showdialog()
        {
            throw new NotImplementedException();
        }
/****************************************************************  
点击开始按钮，开始盘存
*****************************************************************/
        private void start_Click(object sender, EventArgs e)
        {
            try
            {
                if (start_or_stop == true)
                {
                    Thread.Sleep(int.Parse(textBox2.Text) * 1000);
                    c1 = 0; c2 = 0; c3 = 0; c4 = 0;
                    //Thread.Sleep(10000);
                    Console.Beep();
                    Console.Beep();
                    dt.Rows.Clear();                    
                    stw.Reset();
                    stw.Start();
                    timer1.Start();
                    dtLock = false;
                    dataGridView1.DataSource = dt;
                    foreach (ImpinjReader reader in Form2.readers)
                    {
                        reader.Start();
                    }
                    start_or_stop = false;
                    start.Text = "stop";
                    detail.Enabled = false;// 这个按钮是detail result
                }
                else
                {
                    stw.Stop();
                    foreach (ImpinjReader reader in Form2.readers)
                    {
                        reader.Stop();;
                    }
                    timer1.Stop();
                    Thread.Sleep(2000);
                    start.Text = "restart";
                    detail.Enabled = true;// 这个按钮是detail result

                    //去除dt空行
                    while (dtLock == true) ;
                    dtLock = true;
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "Reader<>'' or Reader is not null";//
                    dt = dv.ToTable();
                    dtLock = false;
//                    dataGridView1.DataSource = dt;

                    start_or_stop = true;
                    //dataGridView1.Update();
                    dataGridView1.Refresh();
                    
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + ee.StackTrace);
            }
       
        }
           
            
           
        //}
        private void stop_Click(object sender, EventArgs e)
        {
        //这个stop按钮没用了
        }
/****************************************************************  
处理标签报告
*****************************************************************/
        public static void OnTagsReported(ImpinjReader sender, TagReport report)
        {
            try
            {
                foreach (Tag tag in report)
                {
                    if(tag.Epc != null)
                    {
                        while (dtLock == true) ;
                        dtLock = true;
                        DataRow dr1 = dt.NewRow();
                        dr1["EPC"] = tag.Epc;
                        dr1["RSS"] = tag.PeakRssiInDbm;
                        dr1["Frequency"] = tag.ChannelInMhz;
                        dr1["Phase"] = tag.PhaseAngleInRadians;
                        dr1["Reader"] = sender.Name;
                        //dr1["Address"] = sender.Address;
                        dr1["Antenna"] = tag.AntennaPortNumber;
                        dr1["SeenTime"] = tag.LastSeenTime;
                        dt.Rows.Add(dr1);
                        dtLock = false;
                        switch (sender.Name)
                        {
                            case "Reader #1":
                            case "1":
                                ++c1;
                                break;
                            case "Reader #2":
                            case "2":
                                ++c2;
                                break;
                            case "Reader #3":
                            case "3":
                                ++c3;
                                break;
                            case "Reader #4":
                            case "4":
                                ++c4;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message + ee.StackTrace);
            }
        }
        /**********************************************
         * 
         **********************************************/

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.DataSource =dt;            
        }


        private void exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       

        private void clear_Click(object sender, EventArgs e)
        {
            dt.Rows.Clear();
            dataGridView1.DataSource = dt;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show(
            "窗口关闭后，数据即将丢失！是否现在关闭窗口",
            "提示",
            MessageBoxButtons.OKCancel,
            MessageBoxIcon.Question) != DialogResult.OK)
            {
                e.Cancel = true;
                try
                {
                    foreach (ImpinjReader reader in Form2.readers)
                    {
                        reader.Stop();

                        reader.Disconnect();
                    }
                }
                catch (Exception ee)
                {
                    MessageBox.Show(ee.StackTrace);
                    Console.Beep();
                }

                return;
            }
        }


/****************************************************************  
计时器1计时函数
*****************************************************************/
        private void timer1_Tick(object sender, EventArgs e)
        {
            //dataGridView1.DataSource = dt;
            TimeSpan ts = stw.Elapsed;
            if (checkBox1.Checked == true)
            {
                if (ts.Hours == 0 && ts.Minutes == maxMin && ts.Seconds == maxSec)
                {
                    
                    foreach (ImpinjReader reader in Form2.readers)
                    {
                        reader.Stop();
                        Console.Beep();
                        Console.Beep();
                        Console.Beep();
                    }
                    stw.Stop();
                    timer1.Stop();

                    start.Text = "restart";
                    detail.Enabled = true;// 这个按钮是detail result

                    //去除dt空行
                    while (dtLock == true) ;
                    dtLock = true;
                    DataView dv = dt.DefaultView;
                    dv.RowFilter = "Reader<>'' or Reader is not null";//
                    dt = dv.ToTable();
                    dtLock = false;
//                    dataGridView1.DataSource = dt;

                    dataGridView1.Refresh();
                    start_or_stop = true;
                }
            }
            
            string runtime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}", ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            label1.Text = runtime;
            label4.Text = Convert.ToString(dt.Rows.Count);
            ////给dt去除空行
            //DataView dv = dt.DefaultView;
            //dv.RowFilter = "Reader<>null";//
            //DataTable dt2 = dv.ToTable();
            //dt.DefaultView.RowFilter = "Reader<>null";
            //
            if ((ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds + ts.Milliseconds / 1000 )!= 0)
            {
                label21.Text = Convert.ToString(dt.Rows.Count / (ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds + ts.Milliseconds/1000));
                label9.Text = Convert.ToString(c1);
                label20.Text = Convert.ToString(c1 / (ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds + ts.Milliseconds / 1000));
                label10.Text = Convert.ToString(c2);
                label19.Text = Convert.ToString(c2 / (ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds + ts.Milliseconds / 1000));
                label11.Text = Convert.ToString(c3);
                label18.Text = Convert.ToString(c3 / (ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds + ts.Milliseconds / 1000));
                label12.Text = Convert.ToString(c4);
                label17.Text = Convert.ToString(c4 / (ts.Hours * 3600 + ts.Minutes * 60 + ts.Seconds + ts.Milliseconds / 1000));
            }
        }
//------------------------------------------------------------------------------------------------------------
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form form3 = new Form3();
            form3.ShowDialog();
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 打开ToolStripMenuItem_Click(object sender, EventArgs e)
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
                SaveCSV(dt, fileName);
                //string localFilePath = sfd.FileName.ToString(); //获得文件路径 
                //StreamWriter fin;
                //fin = new StreamWriter(localFilePath, true);
                //fin.Write(dt);
                //fin.Close();
            }
        }
        public static void SaveCSV(DataTable dt, string fileName)
        {
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
            string data = "";

            //写出列名称
            //for (int i = 0; i < dt.Columns.Count; i++)
            //{
            //    data += dt.Columns[i].ColumnName.ToString();
            //    if (i < dt.Columns.Count - 1)
            //    {
            //        data += ",";
            //    }
            //}
            //sw.WriteLine(data);

            //写出各行数据
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                data = "";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    data += dt.Rows[i][j].ToString();
                    if (j < dt.Columns.Count - 1)
                    {
                        data += ",";
                    }
                }
                sw.WriteLine(data);
            }

            sw.Close();
            fs.Close();
            MessageBox.Show("CSV文件保存成功！");
        }

        private void 打开ToolStripMenuItem1_Click(object sender, EventArgs e)
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
                dataGridView1.DataSource = OpenCSV(fileName);
                MessageBox.Show("成功显示CSV数据！");
            }
        }
        public static DataTable OpenCSV(string fileName)
        {
            DataTable dt = new DataTable();
            FileStream fs = new FileStream(fileName, System.IO.FileMode.Open, System.IO.FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            bool IsFirst = true;

            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');
                if (IsFirst == true)
                {
                    IsFirst = false;
                    columnCount = aryLine.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(aryLine[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < columnCount; j++)
                    {
                        dr[j] = aryLine[j];
                    }
                    dt.Rows.Add(dr);
                }
            }

            sr.Close();
            fs.Close();
            return dt;
        }

        private void 复制ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(dataGridView1.SelectedCells) != "")
                Clipboard.SetDataObject(dataGridView1.GetClipboardContent());
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (e.RowIndex >= 0)
                {
                    //若行已是选中状态就不再进行设置
                    if (dataGridView1.Rows[e.RowIndex].Selected == false)
                    {
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[e.RowIndex].Selected = true;
                    }
                    //只选中一行时设置活动单元格
                    if (dataGridView1.SelectedRows.Count == 1)
                    {
                        dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    }
                    //弹出操作菜单
                    contextMenuStrip1.Show(MousePosition.X, MousePosition.Y);
                }
            }
        }

        private void 复制ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Convert.ToString(dataGridView1.SelectedCells) != "")
                Clipboard.SetDataObject(dataGridView1.GetClipboardContent());
        }

        private void 关于ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Shandong University");
        }

        private void 联系我们ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("邮箱：liumengup@126.com");
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            // sfd.ShowDialog();
            sfd.Filter = "CSV文件|*.CSV";
//            sfd.InitialDirectory = "C:\\";
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
                Form1.SaveCSV(dt, fileName);
                //string localFilePath = sfd.FileName.ToString(); //获得文件路径 
                //StreamWriter fin;
                //fin = new StreamWriter(localFilePath, true);
                //fin.Write(dt);
                //fin.Close();
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            maxMin = int.Parse(textBox1.Text) / 60;
            maxSec = int.Parse(textBox1.Text) % 60;
        }
    }
}

