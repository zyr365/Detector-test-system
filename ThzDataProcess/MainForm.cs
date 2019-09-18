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

namespace ThzDataProcess
{
    public partial class MainForm : DevComponents.DotNetBar.Office2007Form
    {
        private int frameRate = 128;    
        bool isStart = false;
        const string stopIcon = @"icon\stop.png";
        const string startIcon = @"icon\start.png";
        DataProcess DPBLL = null;
        public MainForm()
        {
            this.EnableGlass = false;
            InitializeComponent();
            string DeviceIP = "192.168.1.120";
            int LocalPort = 8009;
            DPBLL = new DataProcess(DeviceIP, LocalPort);


            DPBLL.Start();//启动线程
            /*Thread t = new Thread(new ThreadStart(ShowThread));
            t.Priority = ThreadPriority.Highest;
            t.Start();*/

            // this.WindowState = FormWindowState.Maximized;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }
        public void ShowThread()
        {
           MessageBox.Show("321");

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
    }
}
