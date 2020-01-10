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

        private Queue<double>[] dataQueue = new Queue<double>[8];//把Queue<double>看成一个类型 int[] a=new int [8]
        public MainForm()
        {
            this.DoubleBuffered = true;//设置本窗体
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲


            this.EnableGlass = false;
            InitializeComponent();

            InitChart();
            string DeviceIP = "192.168.1.120";
            int LocalPort = 8009;
            DPBLL = new DataProcess(DeviceIP, LocalPort);

            DPBLL.ShowEvent_zyr1 = dataShow;
            DPBLL.ShowEvent_zyr2 = chartShow;
            DPBLL.Start();//启动线程

            radioButton1.Checked = true;
            radioButton2.Checked = false;

            dataQueue[0] = new Queue<double>(100);
            dataQueue[1] = new Queue<double>(100);
            dataQueue[2] = new Queue<double>(100);
            dataQueue[3] = new Queue<double>(100);
            dataQueue[4] = new Queue<double>(100);
            dataQueue[5] = new Queue<double>(100);
            dataQueue[6] = new Queue<double>(100);
            dataQueue[7] = new Queue<double>(100);


        }
        // 防止闪屏        
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }



        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.Top = 0;
            this.Left = 0;
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void InitChart()
        {
            Chart[] ch = new Chart[8] { chart1, chart2 , chart3, chart4, chart5, chart6, chart7, chart8 };
            for (int i = 0; i < 8; i++)
            {
                ch[i].ChartAreas.Clear();
                ChartArea chartArea1 = new ChartArea("C1");
                ch[i].ChartAreas.Add(chartArea1);
                //定义存储和显示点的容器
                ch[i].Series.Clear();
                Series series1 = new Series("S1");
                series1.ChartArea = "C1";
                ch[i].Series.Add(series1);

                ch[i].ChartAreas[0].AxisY.IsStartedFromZero = false;
                ch[i].Legends[0].Enabled = false;

                ch[i].ChartAreas[0].AxisX.Interval = 5;
                ch[i].ChartAreas[0].AxisX.MajorGrid.LineColor = System.Drawing.Color.Silver;
                ch[i].ChartAreas[0].AxisY.MajorGrid.LineColor = System.Drawing.Color.Silver;
                //设置标题
                ch[i].Titles.Clear();
                ch[i].Titles.Add("S01");
                ch[i].Titles[0].Text = "通道" + (i + 1) + " AD折线图显示";
                ch[i].Titles[0].ForeColor = Color.RoyalBlue;
                ch[i].Titles[0].Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
                //设置图表显示样式
                ch[i].Series[0].Color = Color.Red;
                //this.chart1.Titles[0].Text = string.Format("{0}折线图显示", );
                ch[i].Series[0].ChartType = SeriesChartType.Line;
                ch[i].Series[0].Points.Clear();
            }
        }
        public void chartShow( Double y,int ch)
        {

            Chart[] chNum = new Chart[8] { chart1, chart2, chart3, chart4, chart5, chart6, chart7, chart8 };
            if(ch <= 8)
               chartDisplay(chNum[ch-1], ch, y);

        }
        delegate void ChartDelegate(Chart chart, int ch, Double y);
        private void chartDisplay(Chart chart, int ch, Double y)
        {

            if (chart.InvokeRequired)
            {
                ChartDelegate chartDelegate = chartDisplay;
                chart.Invoke(chartDelegate, new object[] { chart, ch, y });
            }
            else
            {
                if ( isStart == true )
                    UpdateQueueValue(ch,y);
                chart.Series[0].Points.Clear();
                // for (int j = 0; j < 100; j++)
                //     chart1.Series[0].Points.AddXY(j, y);
                for (int i = 0; i < dataQueue[ch-1].Count; i++)
                    chart.Series[0].Points.AddXY((i + 1), dataQueue[ch-1].ElementAt(i));
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (!isStart)
            {
                Command.CommandUp_v1(frameRate);
                btnStart.Text = @"停止采集";
                btnStart.DisabledImage = btnStart.Image;
                btnStart.Image = (Image)btnStart.PressedImage.Clone();
                isStart = !isStart;

            }
            else
            {
                Command.CommandUp_v1(0);
                btnStart.Text = @"开始采集";
                btnStart.Image = btnStart.DisabledImage;
                isStart = !isStart;
            }
        }

        private void UpdateQueueValue(int ch,Double y)
        {

            if (dataQueue[ch-1].Count > 100)
                //先出列
                dataQueue[ch-1].Dequeue();
            dataQueue[ch-1].Enqueue(y);


        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Command.CommandUp_v1(0);
        }

        public void dataShow(string avg, string stdDev, string maxMin, int ch)
        {
            double temperatureSensitivity = 0.0;
            double dValue = 0.0,temp1=0.0, temp2 = 0.0, tempV1 = 0.0, tempV2 = 0.0;

            //ShowMessage(dataGridViewX1, str, row, column);
            Label[] lb = new Label[48] { label_1,label_2,label_3,label_4,label_5, label_6, label_7, label_8, label_9, label_10 , label_11, label_12, label_13, label_14, label_15,label_16,
                                         label_17,label_18,label_19,label_20,label_21, label_22, label_23, label_24, label_25, label_26 , label_27, label_28, label_29, label_30, label_31,label_32,
                                         label_33,label_34,label_35,label_36,label_37, label_38, label_39, label_40, label_41, label_42 , label_43, label_44, label_45, label_46, label_47,label_48 };

            if (ch <= 8 && isStart == true)
            {
                if (radioButton1.Checked == true)
                {

                    ShowMessage(lb[(ch - 1) * 6], "V1 ： " + avg);
                    ShowMessage(lb[(ch - 1) * 6 + 1], "σ1 ： " + stdDev);
                    ShowMessage(lb[(ch - 1) * 6 + 2], "Max/Min ：" + maxMin);
                
                }
                else
                {
                    ShowMessage(lb[(ch - 1) * 6 + 3], "V2 ： " + avg);
                    ShowMessage(lb[(ch - 1) * 6 + 4], "σ2 ： " + stdDev);
                    ShowMessage(lb[(ch - 1) * 6 + 2], "Max/Min ：" + maxMin);
                }

                if (textBox1.Text != "" && textBox2.Text != "")
                {
                    dValue = Math.Abs(Convert.ToDouble(textBox1.Text) - Convert.ToDouble(textBox2.Text));
                    temp1 = Convert.ToDouble(lb[(ch - 1) * 6 + 1].Text.Substring(5, lb[(ch - 1) * 6 + 1].Text.Length - 5));
                    temp2 = Convert.ToDouble(lb[(ch - 1) * 6 + 4].Text.Substring(5, lb[(ch - 1) * 6 + 4].Text.Length - 5));
                    tempV1 = Convert.ToDouble(lb[(ch - 1) * 6].Text.Substring(5, lb[(ch - 1) * 6].Text.Length - 5));
                    tempV2 = Convert.ToDouble(lb[(ch - 1) * 6 + 3].Text.Substring(5, lb[(ch - 1) * 6 + 3].Text.Length - 5));
                    if (tempV1 - tempV2 != 0)
                        temperatureSensitivity = (temp1 + temp2) * dValue / Math.Abs(tempV1 - tempV2) / 2.0;
                    ShowMessage(lb[(ch - 1) * 6 + 5], "ΔT ：" + temperatureSensitivity.ToString("0.00"));
                }

            }
            else
            {
                ;
            }
        }

       

        delegate void ShowMessageDelegate(Label lbl, string message);
        private void ShowMessage(Label lbl, string message)
        {
            if (lbl.InvokeRequired)
            {
                ShowMessageDelegate showMessageDelegate = ShowMessage;
                lbl.Invoke(showMessageDelegate, new object[] { lbl, message});
            }
            else
            {

                lbl.Text = message;
            }
        }

       
    }
}
