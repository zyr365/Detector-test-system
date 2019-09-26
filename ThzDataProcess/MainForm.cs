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
using System.Xml;
using BLL;

using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms.DataVisualization.Charting;

namespace ThzDataProcess
{
    public partial class MainForm : DevComponents.DotNetBar.Office2007Form
    {
        private int frameRate = 8,channel=1;    
        bool isStart = false, isStart1 = false;
        const string stopIcon = @"icon\stop.png";
        const string startIcon = @"icon\start.png";
        DataProcess DPBLL = null;
        object ThreadLock = new object();

        private Queue<double> dataQueue = new Queue<double>(100);
       

        public MainForm()
        {
            this.EnableGlass = false;
            InitializeComponent();
           // Control.CheckForIllegalCrossThreadCalls = false;
            //CheckForIllegalCrossThreadCalls = false;
            dataGridViewInit();
            InitChart();

            string DeviceIP = "192.168.1.110";
            int LocalPort = 8008;
            DPBLL = new DataProcess(DeviceIP, LocalPort);

            // richTextBox1.Text = "111";
            //listBoxAdv1.Items.Add(6);
            //listBoxAdv1.Items.Add(8);
            comboBoxEx1.SelectedIndex = 0;
            comboBoxEx2.SelectedIndex = 0;
            frameRate = Int32.Parse(comboBoxEx1.SelectedItem.ToString().Replace("帧", ""));
            channel = Int32.Parse(comboBoxEx2.SelectedItem.ToString().Replace("通道", ""));

            //DataCalculate dc = new DataCalculate();
            DPBLL.ShowEvent_zyr1 = dataShow;
            DPBLL.ShowEvent_zyr2 = ShowlbDevTem;
            DPBLL.ShowEvent_zyr3 = chartShow;
            DPBLL.Start();//启动线程

            /*Thread t = new Thread(Start);
            t.Priority = ThreadPriority.Highest;
            t.Start();*/

            // this.WindowState = FormWindowState.Maximized;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        public void ShowThread(string name)
        {
           MessageBox.Show(name.ToString());
           // richTextBox1.Text = "123456";

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isStart)
            {
                Command.CommandUp_v1(frameRate);
                btnStart.Text = @"终止";
                btnStart.DisabledImage = btnStart.Image;
                btnStart.Image = (Image)btnStart.PressedImage.Clone();
                isStart = !isStart;

            }
            else
            {
                Command.CommandUp_v1(0);
                btnStart.Text = @"启动";
                btnStart.Image = btnStart.DisabledImage;
                isStart = !isStart;
            }
        }

        private void comboBoxEx1_SelectedIndexChanged(object sender, EventArgs e)
        {
            frameRate = Int32.Parse(comboBoxEx1.SelectedItem.ToString().Replace("帧", ""));
            //MessageBox.Show(frameRate.ToString());
        }
        private void comboBoxEx2_SelectedIndexChanged(object sender, EventArgs e)
        {
            channel = Int32.Parse(comboBoxEx2.SelectedItem.ToString().Replace("通道", ""));
            dataQueue.Clear();
        }

        private void dataGridViewInit()
        {
            DataTable dt1 = new DataTable();
            for (int i = 1; i < 37; i++)
            { 
               dt1.Columns.Add("CH" + i + "_AVG"); dt1.Columns.Add("CH" + i + "_VAR");
             }
            for (int i=0;i< 30;i++)
               dt1.Rows.Add("");
            dataGridViewX1.DataSource = dt1;
            

        }
        
        public void dataShow(int row,int column,string str)
        {
            row = row % 30;
            ShowMessage(dataGridViewX1, str,row, column);
           
        }

        
        delegate void ShowMessageDelegate(DataGridView dg, string message, int row, int column);
        private void ShowMessage(DataGridView dg, string message, int row, int column)
        {
            if (dg.InvokeRequired)
            {
                ShowMessageDelegate showMessageDelegate = ShowMessage;
                dg.Invoke(showMessageDelegate, new object[] { dg, message ,row ,column});
            }
            else
            {
                
                dg.Rows[row].Cells[column].Value = message;
            }
        }

        public void chartShow(int ch,Double y)
        {
            

            chartDisplay(chart1, ch,y);

        }
        delegate void ChartDelegate(Chart chart,int ch,  Double y);
        private void chartDisplay(Chart chart, int ch, Double y)
        {
            if (chart.InvokeRequired)
            {
                ChartDelegate chartDelegate = chartDisplay;
                chart.Invoke(chartDelegate, new object[] { chart, ch, y });
            }
            else
            {
                if (ch == channel&& isStart1==true)
                    UpdateQueueValue(y);
                this.chart1.Series[0].Points.Clear();
                // for (int j = 0; j < 100; j++)
                //     chart1.Series[0].Points.AddXY(j, y);
                for (int i = 0; i < dataQueue.Count; i++)
                    this.chart1.Series[0].Points.AddXY((i + 1), dataQueue.ElementAt(i));
            }
        }

        private void UpdateQueueValue(Double y)
        {

           if (dataQueue.Count > 100)
                //先出列
                    dataQueue.Dequeue();
          dataQueue.Enqueue(y);


        }

        public delegate void SWTDelegate(string AddStr);
        //public delegate void ComsumerTextDelegate(int Index, string AddStr);
        public void ShowlbDevTem(string AddStr)
        {
            if (lbDevTem.InvokeRequired)
            {
                SWTDelegate pd = new SWTDelegate(ShowlbDevTem);
                lbDevTem.Invoke(pd, new object[] { AddStr });
            }
            else
            {
                lbDevTem.Text = AddStr;
                //richTextBox1.AppendText(AddStr);
            }
        }


        private void dataGridViewX1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            for (int i = 0; i < dataGridViewX1.Rows.Count; i++)
                this.dataGridViewX1.Rows[i].HeaderCell.Value = (i + 1).ToString();
        }

       
        private void InitChart()
        {
            //定义图表区域
            this.chart1.ChartAreas.Clear();
            ChartArea chartArea1 = new ChartArea("C1");
            this.chart1.ChartAreas.Add(chartArea1);
            //定义存储和显示点的容器
            this.chart1.Series.Clear();
            Series series1 = new Series("S1");
            series1.ChartArea = "C1";
            this.chart1.Series.Add(series1);
            //设置图表显示样式
            this.chart1.ChartAreas[0].AxisY.Minimum = 0;
            this.chart1.ChartAreas[0].AxisY.Maximum = 100;
            this.chart1.ChartAreas[0].AxisX.Interval = 5;
            this.chart1.ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
            this.chart1.ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
            //设置标题
            this.chart1.Titles.Clear();
            this.chart1.Titles.Add("S01");
            this.chart1.Titles[0].Text = "AD折线图显示";
            this.chart1.Titles[0].ForeColor = Color.RoyalBlue;
            this.chart1.Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            //设置图表显示样式
            this.chart1.Series[0].Color = Color.Red;
          
            //this.chart1.Titles[0].Text = string.Format("{0}折线图显示", );
            this.chart1.Series[0].ChartType = SeriesChartType.Line;
            
           
            this.chart1.Series[0].Points.Clear();
        }

        private void btnStart1_Click(object sender, EventArgs e)
        {
            if (!isStart1)
            {
                this.timer1.Start();
                btnStart1.Text = @"终止";
                btnStart1.DisabledImage = btnStart1.Image;
                btnStart1.Image = (Image)btnStart1.PressedImage.Clone();
                isStart1 = !isStart1;

            }
            else
            {
                this.timer1.Stop();
                btnStart1.Text = @"启动";
                btnStart1.Image = btnStart1.DisabledImage;
                isStart1 = !isStart1;
            }
        }
    }
}
