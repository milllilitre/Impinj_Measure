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

namespace rfid
{
    public partial class Form2 : Form
    {
        public static List<ImpinjReader> readers = new List<ImpinjReader>();
        //public static ImpinjReader reader = new ImpinjReader();
        public static string ip1=null;//地址
        public static string ip2 = null;
        public static string ip3 = null;
        public static string ip4 = null;
        public static bool apply_ornot = false;
        private string[] ip = new string[4];
        private int[] population = new int[4];
        private bool[][] freqList = new bool[4][]{new bool[16], new bool[16], new bool[16], new bool[16]};
        private bool[] selected = new bool[4];
        private bool[][] antennaSelected = new bool[4][]{new bool[4], new bool[4], new bool[4], new bool[4]};
        private double[][] txPower = new double[4][]{new double[4], new double[4], new double [4], new double[4]};
        private double[][] txSensitivity = new double[4][] { new double[4], new double[4], new double[4], new double[4] };
        public Form2()
        {
            InitializeComponent();
            this.MaximizeBox = false;
            loadSettings();
            syncSettings();
        }

        private void saveSettings()
        {
            string localFilePath = "config.ini"; //获得文件路径 
            StreamWriter fin;
            fin = new StreamWriter(localFilePath, false);
            for (int i = 0; i < 4; ++i)
            {
                fin.WriteLine(ip[i]);
                fin.WriteLine(population[i]);
                fin.WriteLine(selected[i]);
                int j = 0;
                for (j = 0; j < 16; ++j)
                {
                    fin.WriteLine(freqList[i][j]);
                }
                for (j = 0; j < 4; ++j)
                {
                    fin.WriteLine(antennaSelected[i][j]);
                }
                for (j = 0; j < 4; ++j)
                {
                    fin.WriteLine(txPower[i][j]);
                }
                for (j = 0; j < 4; ++j)
                {
                    fin.WriteLine(txSensitivity[i][j]);
                }
            }
            fin.Close();
        }

        private void loadSettings()
        {
            string localFilePath = "config.ini"; //获得文件路径 
            StreamReader fin;
            fin = new StreamReader(localFilePath, true);
            for (int i = 0; i < 4; ++i)
            {
                ip[i] = fin.ReadLine();
                population[i] = int.Parse(fin.ReadLine());
                selected[i] = bool.Parse(fin.ReadLine());
                int j = 0;
                for (j = 0; j < 16; ++j)
                {
                    freqList[i][j] = bool.Parse(fin.ReadLine());
                }
                for (j = 0; j < 4; ++j)
                {
                    antennaSelected[i][j] = bool.Parse(fin.ReadLine());
                }
                for (j = 0; j < 4; ++j)
                {
                    txPower[i][j] = double.Parse(fin.ReadLine());
                }
                for (j = 0; j < 4; ++j)
                {
                    txSensitivity[i][j] = double.Parse(fin.ReadLine());
                }
            }
            fin.Close();
        }

        private void syncSettings()
        {
            checkBox17.Checked = selected[0];
            checkBox5.Checked = selected[1];
            checkBox10.Checked = selected[2];
            checkBox15.Checked = selected[3];
            textBox1.Text = ip[0];
            textBox11.Text = ip[1];
            textBox21.Text = ip[2];
            textBox31.Text = ip[3];
            textBox10.Text = population[0].ToString();
            textBox12.Text = population[1].ToString();
            textBox22.Text = population[2].ToString();
            textBox32.Text = population[3].ToString();
            for (int i = 0; i < 16; ++i)
            {
                checkedListBox1.SetItemChecked(i, freqList[0][i]);
            }
            for (int i = 0; i < 16; ++i)
            {
                checkedListBox2.SetItemChecked(i, freqList[1][i]);
            }
            for (int i = 0; i < 16; ++i)
            {
                checkedListBox3.SetItemChecked(i, freqList[2][i]);
            }
            for (int i = 0; i < 16; ++i)
            {
                checkedListBox4.SetItemChecked(i, freqList[3][i]);
            }
            checkBox1.Checked = antennaSelected[0][0];
            checkBox2.Checked = antennaSelected[0][1];
            checkBox3.Checked = antennaSelected[0][2];
            checkBox4.Checked = antennaSelected[0][3];
            checkBox9.Checked = antennaSelected[1][0];
            checkBox8.Checked = antennaSelected[1][1];
            checkBox7.Checked = antennaSelected[1][2];
            checkBox6.Checked = antennaSelected[1][3];
            checkBox14.Checked = antennaSelected[2][0];
            checkBox13.Checked = antennaSelected[2][1];
            checkBox12.Checked = antennaSelected[2][2];
            checkBox11.Checked = antennaSelected[2][3];
            checkBox20.Checked = antennaSelected[3][0];
            checkBox19.Checked = antennaSelected[3][1];
            checkBox18.Checked = antennaSelected[3][2];
            checkBox16.Checked = antennaSelected[3][3];
            textBox2.Text = txPower[0][0].ToString();
            textBox5.Text = txPower[0][1].ToString();
            textBox7.Text = txPower[0][2].ToString();
            textBox9.Text = txPower[0][3].ToString();
            textBox20.Text = txPower[1][0].ToString();
            textBox18.Text = txPower[1][1].ToString();
            textBox16.Text = txPower[1][2].ToString();
            textBox14.Text = txPower[1][3].ToString();
            textBox30.Text = txPower[2][0].ToString();
            textBox28.Text = txPower[2][1].ToString();
            textBox26.Text = txPower[2][2].ToString();
            textBox24.Text = txPower[2][3].ToString();
            textBox40.Text = txPower[3][0].ToString();
            textBox38.Text = txPower[3][1].ToString();
            textBox36.Text = txPower[3][2].ToString();
            textBox34.Text = txPower[3][3].ToString();
            textBox3.Text = txSensitivity[0][0].ToString();
            textBox4.Text = txSensitivity[0][1].ToString();
            textBox6.Text = txSensitivity[0][2].ToString();
            textBox8.Text = txSensitivity[0][3].ToString();
            textBox19.Text = txSensitivity[1][0].ToString();
            textBox17.Text = txSensitivity[1][1].ToString();
            textBox15.Text = txSensitivity[1][2].ToString();
            textBox13.Text = txSensitivity[1][3].ToString();
            textBox29.Text = txSensitivity[2][0].ToString();
            textBox27.Text = txSensitivity[2][1].ToString();
            textBox25.Text = txSensitivity[2][2].ToString();
            textBox23.Text = txSensitivity[2][3].ToString();
            textBox39.Text = txSensitivity[3][0].ToString();
            textBox37.Text = txSensitivity[3][1].ToString();
            textBox35.Text = txSensitivity[3][2].ToString();
            textBox33.Text = txSensitivity[3][3].ToString();
        }

        private void getSettingsFromView()
        {
            selected[0] = checkBox17.Checked;
            selected[1] = checkBox5.Checked;
            selected[2] = checkBox10.Checked;
            selected[3] = checkBox15.Checked;
            ip[0] = textBox1.Text;
            ip[1] = textBox11.Text;
            ip[2] = textBox21.Text;
            ip[3] = textBox31.Text;
            population[0] = int.Parse(textBox10.Text);
            population[1] = int.Parse(textBox12.Text);
            population[2] = int.Parse(textBox22.Text);
            population[3] = int.Parse(textBox32.Text);
            for (int i = 0; i < 16; ++i)//一共有16个频率可选
            {
                freqList[0][i] = false;
                if (checkedListBox1.GetItemChecked(i))
                {
                    freqList[0][i] = true;
                }
            }
            for (int i = 0; i < 16; ++i)//一共有16个频率可选
            {
                freqList[1][i] = false;
                if (checkedListBox2.GetItemChecked(i))
                {
                    freqList[1][i] = true;
                }
            }
            for (int i = 0; i < 16; ++i)//一共有16个频率可选
            {
                freqList[2][i] = false;
                if (checkedListBox3.GetItemChecked(i))
                {
                    freqList[2][i] = true;
                }
            }
            for (int i = 0; i < 16; ++i)//一共有16个频率可选
            {
                freqList[3][i] = false;
                if (checkedListBox4.GetItemChecked(i))
                {
                    freqList[3][i] = true;
                }
            }
            antennaSelected[0][0] = checkBox1.Checked;
            antennaSelected[0][1] = checkBox2.Checked;
            antennaSelected[0][2] = checkBox3.Checked;
            antennaSelected[0][3] = checkBox4.Checked;
            antennaSelected[1][0] = checkBox9.Checked;
            antennaSelected[1][1] = checkBox8.Checked;
            antennaSelected[1][2] = checkBox7.Checked;
            antennaSelected[1][3] = checkBox6.Checked;
            antennaSelected[2][0] = checkBox14.Checked;
            antennaSelected[2][1] = checkBox13.Checked;
            antennaSelected[2][2] = checkBox12.Checked;
            antennaSelected[2][3] = checkBox11.Checked;
            antennaSelected[3][0] = checkBox20.Checked;
            antennaSelected[3][1] = checkBox19.Checked;
            antennaSelected[3][2] = checkBox18.Checked;
            antennaSelected[3][3] = checkBox16.Checked;
            txPower[0][0] = double.Parse(textBox2.Text);
            txPower[0][1] = double.Parse(textBox5.Text);
            txPower[0][2] = double.Parse(textBox7.Text);
            txPower[0][3] = double.Parse(textBox9.Text);
            txPower[1][0] = double.Parse(textBox20.Text);
            txPower[1][1] = double.Parse(textBox18.Text);
            txPower[1][2] = double.Parse(textBox16.Text);
            txPower[1][3] = double.Parse(textBox14.Text);
            txPower[2][0] = double.Parse(textBox30.Text);
            txPower[2][1] = double.Parse(textBox28.Text);
            txPower[2][2] = double.Parse(textBox26.Text);
            txPower[2][3] = double.Parse(textBox24.Text);
            txPower[3][0] = double.Parse(textBox40.Text);
            txPower[3][1] = double.Parse(textBox38.Text);
            txPower[3][2] = double.Parse(textBox36.Text);
            txPower[3][3] = double.Parse(textBox34.Text);
            txSensitivity[0][0] = double.Parse(textBox3.Text);
            txSensitivity[0][1] = double.Parse(textBox4.Text);
            txSensitivity[0][2] = double.Parse(textBox6.Text);
            txSensitivity[0][3] = double.Parse(textBox8.Text);
            txSensitivity[1][0] = double.Parse(textBox19.Text);
            txSensitivity[1][1] = double.Parse(textBox17.Text);
            txSensitivity[1][2] = double.Parse(textBox15.Text);
            txSensitivity[1][3] = double.Parse(textBox13.Text);
            txSensitivity[2][0] = double.Parse(textBox29.Text);
            txSensitivity[2][1] = double.Parse(textBox27.Text);
            txSensitivity[2][2] = double.Parse(textBox25.Text);
            txSensitivity[2][3] = double.Parse(textBox23.Text);
            txSensitivity[3][0] = double.Parse(textBox39.Text);
            txSensitivity[3][1] = double.Parse(textBox37.Text);
            txSensitivity[3][2] = double.Parse(textBox35.Text);
            txSensitivity[3][3] = double.Parse(textBox33.Text);
        }




        private void apply_button_Click(object sender, EventArgs e)
        {
            getSettingsFromView();
            foreach (ImpinjReader reader in Form2.readers)
            {
                reader.Stop();

                reader.Disconnect();
            }
            readers.Clear();
            if (checkBox17.Checked == true)//Treader #1 setting 
            {
                try
                {
                    //readers.Add(new ImpinjReader(textBox1.Text, "Reader #1"));
                    readers.Add(new ImpinjReader(textBox1.Text, "1"));
                    int index = readers.Count - 1;
                    ip1 = textBox1.Text;
                    // ImpinjReader reader = readers[0];
                    readers[index].Connect(ip1);
                    //Program.reader.Connect(ip1);
                    Settings settings = readers[index].QueryDefaultSettings();
                    settings.Report.IncludeAntennaPortNumber = true;
                    settings.Report.IncludePhaseAngle = true;
                    settings.Report.IncludeChannel = true;
                    settings.Report.IncludeDopplerFrequency = true;
                    settings.Report.IncludeFastId = true;   //TID的另一种表现形式。tx freqency
                    settings.Report.IncludeFirstSeenTime = true;
                    settings.Report.IncludeLastSeenTime = true;
                    settings.Report.IncludePeakRssi = true;
                    settings.Report.IncludeSeenCount = true;
                    settings.Report.IncludePcBits = true;

                    settings.ReaderMode = ReaderMode.MaxThroughput;//.AutoSetDenseReader;
                    settings.SearchMode = SearchMode.DualTarget;//.DualTarget;
                    settings.Session = 1;
                    settings.TagPopulationEstimate = Convert.ToUInt16(textBox10.Text);

                    //checkedListBox1.SetItemChecked(0, true);//默认选取920.625
                    //checkedListBox1.SetItemChecked(4, true);
                    //checkedListBox1.SetItemChecked(8, true);
                    //checkedListBox1.SetItemChecked(12, true);


                    for (int i = 0; i < 16; ++i)//一共有16个频率可选
                    {
                        if (checkedListBox1.GetItemChecked(i))
                        {
                            settings.TxFrequenciesInMhz.Add(920.625+i*0.25);
                        }
                    }
                   
                    settings.Antennas.DisableAll();
                    
                    if (checkBox1.Checked)
                    {
                        settings.Antennas.GetAntenna(1).IsEnabled = true;
                        settings.Antennas.GetAntenna(1).TxPowerInDbm = Convert.ToDouble(textBox2.Text);
                        settings.Antennas.GetAntenna(1).RxSensitivityInDbm = Convert.ToDouble(textBox3.Text);
                    }
                    if (checkBox2.Checked)
                    {
                        settings.Antennas.GetAntenna(2).IsEnabled = true;
                        settings.Antennas.GetAntenna(2).TxPowerInDbm = Convert.ToDouble(textBox5.Text);
                        settings.Antennas.GetAntenna(2).RxSensitivityInDbm = Convert.ToDouble(textBox4.Text);
                    }
                    if (checkBox3.Checked)
                    {
                        settings.Antennas.GetAntenna(3).IsEnabled = true;
                        settings.Antennas.GetAntenna(3).TxPowerInDbm = Convert.ToDouble(textBox7.Text);
                        settings.Antennas.GetAntenna(3).RxSensitivityInDbm = Convert.ToDouble(textBox6.Text);
                    }
                    if (checkBox4.Checked)
                    {
                        settings.Antennas.GetAntenna(4).IsEnabled = true;
                        settings.Antennas.GetAntenna(4).TxPowerInDbm = Convert.ToDouble(textBox9.Text);
                        settings.Antennas.GetAntenna(4).RxSensitivityInDbm = Convert.ToDouble(textBox8.Text);
                    }
                    settings.Report.Mode = ReportMode.Individual;
                    readers[index].ApplySettings(settings);
                    readers[index].TagsReported += Form1.OnTagsReported;
                }
                catch (OctaneSdkException ee)
                {
                    MessageBox.Show("Octane SDK exception: Reader #1"+ee.Message,"error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                }
                catch (Exception ee)
                {
                    // Handle other .NET errors.
                    MessageBox.Show("Exception : Reader #1"+ee.Message,"error",MessageBoxButtons.OK,MessageBoxIcon.Warning);
                    MessageBox.Show(ee.StackTrace);
                }
            }
            if (checkBox5.Checked == true)//reader2 setting
            {
                try
                {

                    readers.Add(new ImpinjReader(textBox11.Text, "Reader #2"));
                    ip2 = textBox11.Text;
                    int index = readers.Count-1;
                    // ImpinjReader reader = readers[0];
                    readers[index].Connect(ip2);
                    //Program.reader.Connect(ip1);
                    Settings settings = readers[index].QueryDefaultSettings();
                    settings.Report.IncludeAntennaPortNumber = true;
                    settings.Report.IncludePhaseAngle = true;
                    settings.Report.IncludeChannel = true;
                    settings.Report.IncludeDopplerFrequency = true;
                    settings.Report.IncludeFastId = true;   //TID的另一种表现形式。tx freqency
                    settings.Report.IncludeFirstSeenTime = true;
                    settings.Report.IncludeLastSeenTime = true;
                    settings.Report.IncludePeakRssi = true;
                    settings.Report.IncludeSeenCount = true;
                    settings.Report.IncludePcBits = true;

                    settings.ReaderMode = ReaderMode.MaxThroughput;//.AutoSetDenseReader;
                    settings.SearchMode = SearchMode.DualTarget;//.DualTarget;
                    settings.Session = 1;
                    settings.TagPopulationEstimate = Convert.ToUInt16(textBox10.Text);

                    //checkedListBox1.SetItemChecked(0, true);//默认选取920.625

                    for (int i = 0; i < 16; ++i)//一共有16个频率可选
                    {
                        if (checkedListBox2.GetItemChecked(i))
                        {
                            settings.TxFrequenciesInMhz.Add(920.625 + i * 0.25);
                        }
                    }
                    settings.Antennas.DisableAll();

                    if (checkBox1.Checked)
                    {
                        settings.Antennas.GetAntenna(1).IsEnabled = true;
                        settings.Antennas.GetAntenna(1).TxPowerInDbm = Convert.ToDouble(textBox2.Text);
                        settings.Antennas.GetAntenna(1).RxSensitivityInDbm = Convert.ToDouble(textBox3.Text);
                    }
                    if (checkBox2.Checked)
                    {
                        settings.Antennas.GetAntenna(2).IsEnabled = true;
                        settings.Antennas.GetAntenna(2).TxPowerInDbm = Convert.ToDouble(textBox5.Text);
                        settings.Antennas.GetAntenna(2).RxSensitivityInDbm = Convert.ToDouble(textBox4.Text);
                    }
                    if (checkBox3.Checked)
                    {
                        settings.Antennas.GetAntenna(3).IsEnabled = true;
                        settings.Antennas.GetAntenna(3).TxPowerInDbm = Convert.ToDouble(textBox7.Text);
                        settings.Antennas.GetAntenna(3).RxSensitivityInDbm = Convert.ToDouble(textBox6.Text);
                    }
                    if (checkBox4.Checked)
                    {
                        settings.Antennas.GetAntenna(4).IsEnabled = true;
                        settings.Antennas.GetAntenna(4).TxPowerInDbm = Convert.ToDouble(textBox9.Text);
                        settings.Antennas.GetAntenna(4).RxSensitivityInDbm = Convert.ToDouble(textBox8.Text);
                    }
                    settings.Report.Mode = ReportMode.Individual;

                    readers[index].ApplySettings(settings);
                    readers[index].TagsReported += Form1.OnTagsReported;
                }
                catch (OctaneSdkException ee)
                {
                    MessageBox.Show("Octane SDK exception: Reader #2:" + ee.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ee)
                {
                    // Handle other .NET errors.
                    MessageBox.Show("Exception : Reader #2:" + ee.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            //reader 3
            if (checkBox10.Checked == true)
            {
                try
                {

                    readers.Add(new ImpinjReader(textBox11.Text, "Reader #3"));
                    ip3 = textBox21.Text;
                    int index = readers.Count - 1;
                    // ImpinjReader reader = readers[0];
                    readers[index].Connect(ip3);
                    //Program.reader.Connect(ip1);
                    Settings settings = readers[index].QueryDefaultSettings();
                    settings.Report.IncludeAntennaPortNumber = true;
                    settings.Report.IncludePhaseAngle = true;
                    settings.Report.IncludeChannel = true;
                    settings.Report.IncludeDopplerFrequency = true;
                    settings.Report.IncludeFastId = true;   //TID的另一种表现形式。tx freqency
                    settings.Report.IncludeFirstSeenTime = true;
                    settings.Report.IncludeLastSeenTime = true;
                    settings.Report.IncludePeakRssi = true;
                    settings.Report.IncludeSeenCount = true;
                    settings.Report.IncludePcBits = true;

                    settings.ReaderMode = ReaderMode.MaxThroughput;//.AutoSetDenseReader;
                    settings.SearchMode = SearchMode.DualTarget;//.DualTarget;
                    settings.Session = 1;
                    settings.TagPopulationEstimate = Convert.ToUInt16(textBox10.Text);

                    //checkedListBox1.SetItemChecked(0, true);//默认选取920.625

                    for (int i = 0; i < 16; ++i)//一共有16个频率可选
                    {
                        if (checkedListBox2.GetItemChecked(i))
                        {
                            settings.TxFrequenciesInMhz.Add(920.625 + i * 0.25);
                        }
                    }
                    settings.Antennas.DisableAll();

                    if (checkBox1.Checked)
                    {
                        settings.Antennas.GetAntenna(1).IsEnabled = true;
                        settings.Antennas.GetAntenna(1).TxPowerInDbm = Convert.ToDouble(textBox2.Text);
                        settings.Antennas.GetAntenna(1).RxSensitivityInDbm = Convert.ToDouble(textBox3.Text);
                    }
                    if (checkBox2.Checked)
                    {
                        settings.Antennas.GetAntenna(2).IsEnabled = true;
                        settings.Antennas.GetAntenna(2).TxPowerInDbm = Convert.ToDouble(textBox5.Text);
                        settings.Antennas.GetAntenna(2).RxSensitivityInDbm = Convert.ToDouble(textBox4.Text);
                    }
                    if (checkBox3.Checked)
                    {
                        settings.Antennas.GetAntenna(3).IsEnabled = true;
                        settings.Antennas.GetAntenna(3).TxPowerInDbm = Convert.ToDouble(textBox7.Text);
                        settings.Antennas.GetAntenna(3).RxSensitivityInDbm = Convert.ToDouble(textBox6.Text);
                    }
                    if (checkBox4.Checked)
                    {
                        settings.Antennas.GetAntenna(4).IsEnabled = true;
                        settings.Antennas.GetAntenna(4).TxPowerInDbm = Convert.ToDouble(textBox9.Text);
                        settings.Antennas.GetAntenna(4).RxSensitivityInDbm = Convert.ToDouble(textBox8.Text);
                    }
                    settings.Report.Mode = ReportMode.Individual;

                    readers[index].ApplySettings(settings);
                    readers[index].TagsReported += Form1.OnTagsReported;
                }
                catch (OctaneSdkException ee)
                {
                    MessageBox.Show("Octane SDK exception: Reader #2:" + ee.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ee)
                {
                    // Handle other .NET errors.
                    MessageBox.Show("Exception : Reader #2:" + ee.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            //reader 4
         
            if (checkBox15.Checked == true)
            {
                try
                {

                    readers.Add(new ImpinjReader(textBox11.Text, "Reader #4"));
                    ip4 = textBox31.Text;
                    int index = readers.Count - 1;
                    // ImpinjReader reader = readers[0];
                    readers[index].Connect(ip4);
                    //Program.reader.Connect(ip1);
                    Settings settings = readers[index].QueryDefaultSettings();
                    settings.Report.IncludeAntennaPortNumber = true;
                    settings.Report.IncludePhaseAngle = true;
                    settings.Report.IncludeChannel = true;
                    settings.Report.IncludeDopplerFrequency = true;
                    settings.Report.IncludeFastId = true;   //TID的另一种表现形式。tx freqency
                    settings.Report.IncludeFirstSeenTime = true;
                    settings.Report.IncludeLastSeenTime = true;
                    settings.Report.IncludePeakRssi = true;
                    settings.Report.IncludeSeenCount = true;
                    settings.Report.IncludePcBits = true;

                    settings.ReaderMode = ReaderMode.MaxThroughput;//.AutoSetDenseReader;
                    settings.SearchMode = SearchMode.DualTarget;//.DualTarget;
                    settings.Session = 1;
                    settings.TagPopulationEstimate = Convert.ToUInt16(textBox10.Text);

                    //checkedListBox1.SetItemChecked(0, true);//默认选取920.625

                    for (int i = 0; i < 16; ++i)//一共有16个频率可选
                    {
                        if (checkedListBox2.GetItemChecked(i))
                        {
                            settings.TxFrequenciesInMhz.Add(920.625 + i * 0.25);
                        }
                    }
                    settings.Antennas.DisableAll();

                    if (checkBox1.Checked)
                    {
                        settings.Antennas.GetAntenna(1).IsEnabled = true;
                        settings.Antennas.GetAntenna(1).TxPowerInDbm = Convert.ToDouble(textBox2.Text);
                        settings.Antennas.GetAntenna(1).RxSensitivityInDbm = Convert.ToDouble(textBox3.Text);
                    }
                    if (checkBox2.Checked)
                    {
                        settings.Antennas.GetAntenna(2).IsEnabled = true;
                        settings.Antennas.GetAntenna(2).TxPowerInDbm = Convert.ToDouble(textBox5.Text);
                        settings.Antennas.GetAntenna(2).RxSensitivityInDbm = Convert.ToDouble(textBox4.Text);
                    }
                    if (checkBox3.Checked)
                    {
                        settings.Antennas.GetAntenna(3).IsEnabled = true;
                        settings.Antennas.GetAntenna(3).TxPowerInDbm = Convert.ToDouble(textBox7.Text);
                        settings.Antennas.GetAntenna(3).RxSensitivityInDbm = Convert.ToDouble(textBox6.Text);
                    }
                    if (checkBox4.Checked)
                    {
                        settings.Antennas.GetAntenna(4).IsEnabled = true;
                        settings.Antennas.GetAntenna(4).TxPowerInDbm = Convert.ToDouble(textBox9.Text);
                        settings.Antennas.GetAntenna(4).RxSensitivityInDbm = Convert.ToDouble(textBox8.Text);
                    }
                    settings.Report.Mode = ReportMode.Individual;

                    readers[index].ApplySettings(settings);
                    readers[index].TagsReported += Form1.OnTagsReported;
                }
                catch (OctaneSdkException ee)
                {
                    MessageBox.Show("Octane SDK exception: Reader #2:" + ee.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                catch (Exception ee)
                {
                    // Handle other .NET errors.
                    MessageBox.Show("Exception : Reader #2:" + ee.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            apply_ornot = true;
            saveSettings();
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
