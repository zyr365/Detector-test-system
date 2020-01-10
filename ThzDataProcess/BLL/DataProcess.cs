using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThzData;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Drawing;

using ThzDataProcess;
using System.Windows.Forms;


namespace BLL
{
    public class Command
    {
        public static void CommandUp(int frame)
        {
            //string sendString = null;//要发送的字符串 
            byte[] Data = new byte[8];
            switch (frame)
            {
                case 0:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 00, 16 };//停止
                    break;
                case 6:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 00, 32 };//6帧/s 启动
                    break;
                case 8:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 00, 48 };//仅上传AD值
                    //Data = new byte[] { 22, 144, 87, 235, 00, 00, 00, 33 };//8帧/s 启动
                    break;
                case 10:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 00, 34 };//10帧/s 启动
                    break;
                case 12:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 00, 35 };//12帧/s 启动
                    break;
                case -1:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 00, 48 };//仅上传AD值
                    break;
                default:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 00, 33 };//8帧/s 启动
                    break;
            }
            UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 9001));
            //广播发送指令
            IPAddress remoteIP = IPAddress.Parse("255.255.255.255");
            int remotePort = 8010;
            //实例化广播
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);

            //sendString = Console.ReadLine();
            //sendData = Encoding.Default.GetBytes(sendString);

            //client = new UdpClient();
            //将数据发送到远程端点 
            client.Send(Data, Data.Length, remotePoint);
            //关闭连接
            client.Close();

        }

        public static void CommandUp_v1(int frame, byte tair = 00)                     //看不懂
        {
            //string sendString = null;//要发送的字符串 
            byte[] Data = new byte[8];
            switch (frame)
            {
                case 0:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 16, tair };//停止
                    break;
                case 6:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 32, tair };//6帧/s 启动
                    break;
                case 8:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 48, tair };//仅上传AD值
                    //Data = new byte[] { 22, 144, 87, 235, 00, 00, 33, tair };//8帧/s 启动
                    break;
                case 10:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 34, tair };//10帧/s 启动
                    break;
                case 12:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 35, tair };//12帧/s 启动
                    break;
                case 1:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 97, tair };//自动获取编码器值范围
                    break;
                case 60:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 80, tair };//6帧/s 校准参数
                    break;
                case 80:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 81, tair };//8帧/s 校准参数
                    break;
                case 100:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 82, tair };//10帧/s 校准参数
                    break;
                case 120:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 83, tair };//12帧/s 校准参数
                    break;
                case -1:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 48, tair };//仅上传AD值
                    break;
                case 128:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 128, tair };//仅上传AD值
                    break;
                default:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 33, tair };//8帧/s 启动
                    break;
            }

            UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 9001));
           // IPAddress remoteIP = IPAddress.Parse("255.255.255.255");  //以广播方式发送
            IPAddress remoteIP = IPAddress.Parse("192.168.1.255");
            int remotePort = 8008;
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);



            //sendString = Console.ReadLine();
            //sendData = Encoding.Default.GetBytes(sendString);

            //client = new UdpClient();
            //将数据发送到远程端点 
            client.Send(Data, Data.Length, remotePoint);
            //关闭连接
            client.Close();
            

        }

        public static void CommandUp_v2(int frame)
        {
            //string sendString = null;//要发送的字符串 
            byte[] Data = new byte[8];

            switch (frame)
            {
                case 41:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 65, 00 };//停止
                    break;
                case 42:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 66, 00 };//6帧/s 启动
                    break;
                case 43:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 67, 00 };//8帧/s 启动
                    break;
                default:
                    Data = new byte[] { 22, 144, 87, 235, 00, 00, 66, 00 };//8帧/s 启动
                    break;
            }

            UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Any, 9001));
            //广播发送指令
            IPAddress remoteIP = IPAddress.Parse("255.255.255.255");
            int remotePort = 8008;
            //实例化广播
            IPEndPoint remotePoint = new IPEndPoint(remoteIP, remotePort);

            //sendString = Console.ReadLine();
            //sendData = Encoding.Default.GetBytes(sendString);

            //client = new UdpClient();
            //将数据发送到远程端点 
            client.Send(Data, Data.Length, remotePoint);
            //关闭连接
            client.Close();

        }
    }

    public class DataProcess
    {
        string deviceIp = "";                                       //太赫兹设备IP
        const int devicePort = 8008;                                //太赫兹设备发送端口
        int localPort = new int();                                  //本机接收端口

        Queue<List<Byte[]>> DataQueue = new Queue<List<Byte[]>>();  //数据缓存队列
        Semaphore TaskSemaphore = new Semaphore(0, 2560);           //数据缓存队列缓存区
        object ThreadLock = new object();                           //线程锁

        public Action<Bitmap, string[], double[], bool, bool> ShowEvent;
        public Action<string, string, string, int> ShowEvent_zyr1;
        public Action<double, int> ShowEvent_zyr2;
       // public Action<int,Double> ShowEvent_zyr3;
        //MainForm mf = new MainForm();


        DataCalculate dc = new DataCalculate();


        #region 线程开启
        /// <summary>
        /// 所有线程开启函数
        /// </summary>
        public void Start()
        {
            StartAllThread();
        }

        /// <summary>
        /// 开启子线程
        /// </summary>
        private void StartAllThread()
        {
            StartDataRevThread();
            StartDataProcessThread();
            StartShowThread();
        }

        private void StartShowThread()
        {
            Thread t = new Thread(new ThreadStart(ShowAllVariables));                  //开启DataRevThread
            t.Name = "ShowAllVariables";                                            //线程名字
            t.Start();
            t.IsBackground = true;
        }

        /// <summary>
        /// 开启数据接收线程
        /// </summary>
        private void StartDataRevThread()
        {

            Thread t = new Thread(new ThreadStart(DataRevThread));                  //开启DataRevThread
            t.Name = "DataRevThread";                                            //线程名字
            t.Start();
            t.IsBackground = true;                                                  //后台运行
        }

        /// <summary>
        /// 数据处理线程开启
        /// </summary>
        private void StartDataProcessThread()
        {
            Thread t = new Thread(new ThreadStart(DataDecodeImageProcessThread));  //开启DataDecode_ImageProcessThread
            t.Name = "DataDecode_ImageProcessThread";                                            //线程名字
            t.Start();
            t.IsBackground = true;                                                  //后台运行
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataProcess(string deviceip, int localport)
        {
            deviceIp = deviceip;//192.168.1.120
            localPort = localport;//8008
        }


        /*Thread t = new Thread(Start);
           t.Priority = ThreadPriority.Highest;
           t.Start();*/

        #region 数据接收线程
        /// <summary>
        /// 数据接收线程
        /// </summary>
        private void DataRevThread()
        {
            
            try
            {
                //IPAddress remoteIP = IPAddress.Parse("255.255.255.255");                                      //广播


                UdpClient client = new UdpClient(new IPEndPoint(IPAddress.Parse("192.168.1.119"), localPort));   //本机端口 一般UdpClient client = new UdpClient();
                IPAddress remoteIP = IPAddress.Parse(deviceIp);                                                   //远程IP
                int remotePort = devicePort;                                                                   //远程端口
                IPEndPoint endpoint = new IPEndPoint(remoteIP, remotePort);                                 //远程IP和端口
                client.Client.ReceiveBufferSize = 40960;//40960 默认值是8192
                //ARP触发
                client.Send(new byte[] { 00, 11 }, 2, endpoint);   //发送 00 ,11有何作用？？



                while (true)
                    {
                      // MainForm f1 = new MainForm();
                      //f1.ShowThread("123");
                      List<Byte[]> Taskbuff = new List<Byte[]>();
                          for (int i = 0; i < 25; i++)                             //为什么是21？
                        {

                            Byte[] recv = client.Receive(ref endpoint);
                            // MainForm f1 = new MainForm();
                            // f1.ShowThread();
                            Taskbuff.Add(recv);


                       
                        DataShow_v1(recv, Environment.CurrentDirectory + "\\bin", "mydata.bin");



                        // writerFile(recv, Environment.CurrentDirectory + "\\bin\\mydata.bin");

                    }
                    // 任务队列为临界资源，需要锁 
                    lock (ThreadLock)
                        {
                            DataQueue.Enqueue(Taskbuff); //Queue<List<Byte[]>> DataQueue = new Queue<List<Byte[]>>();  在队列的末端添加元素

                        }
                        // 每添加一个任务，信号量加1
                        TaskSemaphore.Release(1);

                        //ChangeProducerText(String.Format("Consumer 1 take Task {0}\r\n", a));


                    }
                

                //client.Close();
            }
            catch (Exception ex)
            {
                string fileName = "Log\\debug" + localPort + "_DataDecode.txt";
                string content = DateTime.Now.ToLocalTime() + ex.Message + "\n" + ex.StackTrace + "\r\n";
                Logger(fileName, content);
            }

        }
        #endregion


        #region 数据显示
        private void DataShow_v1(Byte[] recv,string filePath,string fileName)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (!File.Exists(filePath+ "\\"+fileName))
                File.Create(filePath + "\\"+fileName).Close(); //.Close 很关键，不然会有问题
            FileStream fs = new FileStream(filePath + "\\"+fileName, FileMode.Append, FileAccess.Write);
            fs.Write(recv, 0, recv.Length);
            fs.Close();
            fs.Dispose();
        }
            private void DataShow(Byte[] recv)
        {
            //MainForm f1 = new MainForm();
            // ShowMessage(f1.richTextBox1, "开始显示数据");
            //  MainForm f1 = new MainForm();
            //f1.ShowThread("123");
            /*  int nLength = recv.Length;//字节数组长度
              string sByte = "";
              if (nLength > 0)
              {
                  sByte = Convert.ToString(recv[0], 16);//转换成16进制
                  if (nLength > 1)
                  {
                      for (int k = 1; k < recv.Length; k++)
                      {
                          sByte += ",";//用逗号隔开
                          sByte += Convert.ToString(recv[k], 16);//转换成16进制

                      }
                  }
              }
              if (!Directory.Exists(Environment.CurrentDirectory + "\\bin"))
                  Directory.CreateDirectory(Environment.CurrentDirectory + "\\bin");

              //Environment.CurrentDirectory + "\\" + DateTime.Now.ToString("HHmmssfff") + "bin.txt"
              // writerFile(strToToHexByte(sNeed.ToString().Substring(0)), "ZHEN\\Port" + localPort + "_Frame" + (CountNum) % 10000 + ".bin");
              using (StreamWriter file = new StreamWriter(Environment.CurrentDirectory + "\\bin\\" + DateTime.Now.ToString("HHmmss") + ".txt", true))
              {
                  file.WriteLine(sByte);
                  file.WriteLine("\r\n");

              }*/

            //if (!Directory.Exists(Environment.CurrentDirectory + "\\bin"))
            //    Directory.CreateDirectory(Environment.CurrentDirectory + "\\bin");

            //Stream flstr = new FileStream(Path.GetDirectoryName(Application.ExecutablePath))
            // BinaryWriter sw = new  BinaryWriter(new FileStream(Environment.CurrentDirectory + "\\bin\\mydata.bin", FileMode.Append, FileAccess.ReadWrite)); //创建文件  bin目录下
            //  sw.Write(recv);
            //  sw.Close();

            //writerFile(recv, Environment.CurrentDirectory + "\\bin\\mydata.bin");
            int nLength = recv.Length;//字节数组长度
             string sByte = "";
             if (nLength > 0)
             {
                 sByte = Convert.ToString(recv[0], 16);//转换成16进制
                 if (nLength > 1)
                 {
                     for (int i = 1; i < recv.Length; i++)
                     {
                         sByte += ",";//用逗号隔开
                         sByte += Convert.ToString(recv[i], 16);//转换成16进制

                     }
                 }
             }
            if (!File.Exists(Environment.CurrentDirectory +"\\bin.txt"))
                File.Create(Environment.CurrentDirectory + "\\bin.txt");
            //Environment.CurrentDirectory + "\\" + DateTime.Now.ToString("HHmmssfff") + "bin.txt"
            using (StreamWriter file = new StreamWriter(Environment.CurrentDirectory + "\\bin.txt", true))
            {
                file.WriteLine(DateTime.Now.ToString("HHmmssfff")+sByte);

            }

            //File.WriteAllBytes(Environment.CurrentDirectory+"bin.txt", recv);
            //ShowMessage(f1.richTextBox1, sByte);

        }
        // 向RichTextBox中添加文本
        delegate void ShowMessageDelegate(RichTextBox txtbox, string message);
        private void ShowMessage(RichTextBox txtbox, string message)
        {
            if (txtbox.InvokeRequired)
            {
                ShowMessageDelegate showMessageDelegate = ShowMessage;
                txtbox.Invoke(showMessageDelegate, new object[] { txtbox, message });
            }
            else
            {
                txtbox.Text += message + "\r\n";
            }
        }
        #endregion

        #region 数据解码图像处理线程
        private int CountNum = 0;
        //private int ErrorNum = 0;
        public bool getMeanMatFlag;
        //private string WenduC = "";
        //private string str2 = "";
        Stopwatch elapsetime = new Stopwatch();


        /// <summary>
        /// 数据解码图像处理
        /// </summary>
        private void DataDecodeImageProcessThread()
        {
            try
            {
                List<Byte[]> GetTask = new List<Byte[]>();  //数据缓存队列
                List<Byte[]> listBuff = new List<Byte[]>(); //多余数据缓存区
                THZData thzdata = new THZData();               //Thz数据

                while (true)
                {
                    //接收数据
                    TaskSemaphore.WaitOne();                //等待接收队列


                    lock (ThreadLock)                       //锁线程  
                    {
                        GetTask = DataQueue.Dequeue();   //Queue<List<Byte[]>> DataQueue = new Queue<List<Byte[]>>();
                    }

                    string[] duration = new string[3] { "", "", "" };
                    elapsetime.Restart();//计时开始
                    //数据解析解码
                    //DataDecode(GetTask, ref listBuff, ref thzdata);
                    

                    DataDecode_v1(GetTask, ref listBuff, ref thzdata);
                    //Console.WriteLine("123");


                    elapsetime.Stop();//计时结束
                    duration[0] = elapsetime.ElapsedMilliseconds.ToString("000");
                    //背景校准
                    //if (getMeanMatFlag)
                    //{
                    //    if (frame.UserData != null)
                    //        MeanMatList.Add(frame);

                    //    if (MeanMatList.Count == 10)
                    //    {
                    //        MeanMat = getMeanMat(MeanMatList);
                    //        MeanMatList.Clear();
                    //        getMeanMatFlag = false;
                    //    }
                    //    ImageClass.MeanMat = MeanMat;
                    //}
                    elapsetime.Restart();//计时开始


                    thzdata.StartImageProcess();
                    if (thzdata.FilterImage != null)   ///FilterImage滤波后图像
                    {
                        if (!Directory.Exists("ZHEN"))
                            Directory.CreateDirectory("ZHEN");

                        thzdata.FilterImage.Mat.Bitmap.Save("ZHEN\\" + DateTime.Now.ToString("HHmmssfff") + ".bmp");
                    }

                    elapsetime.Stop();//计时结束
                    duration[1] = elapsetime.ElapsedMilliseconds.ToString("0000");
                    duration[2] = (Convert.ToInt32(duration[0]) + Convert.ToInt32(duration[1])).ToString("0000");
                    //温度显示
                    double[] temparture = thzdata.GetTemparture();
                    if (thzdata.FinalImage != null)
                    {
                        ShowData SD = new ShowData(thzdata.FinalImage.Bitmap, temparture, duration, thzdata.isPeople, thzdata.isHidden);//thzdata.isPeople

                        // 任务队列为临界资源，需要锁 
                        lock (ThreadLock1)
                        {
                            ShowQueue.Enqueue(SD);

                        }
                        // 每添加一个任务，信号量加1
                        TaskSemaphore1.Release(1);
                    }
                    //if (ShowEvent != null && thzdata.FinalImage != null)
                    //    ShowEvent(thzdata.FinalImage.Bitmap, duration, Temparture, thzdata.isPeople);
                    //ShowEvent(PB1Image, ImageClass.lImage, ImageClass.isPeople, elapsetime.ElapsedMilliseconds.ToString(), ImageClass.HideGoods, "错帧数：" + ErrorNum);
                    //ShowTextEvent.Invoke(ImageClass.OutArray.Bitmap);


                }
            }
            catch (Exception ex)
            {
                string fileName = "Log\\debug" + localPort + "_DataDecode.txt";
                string content = DateTime.Now.ToLocalTime() + ex.Message + "\n" + ex.StackTrace + "\r\n";
                Logger(fileName, content);
            }

        }

        Queue<ShowData> ShowQueue = new Queue<ShowData>();  //数据缓存队列
        Semaphore TaskSemaphore1 = new Semaphore(0, 2560);  //数据缓存队列缓存区 
                                                           //第一个就是信号量的内部整数初始值，也就是初始请求数，第二个参数就是最大请求数。

        object ThreadLock1 = new object();
        private void ShowAllVariables()
        {
            ShowData GetTask = new ShowData();  //数据缓存队列
            while (true)
            {
                //接收数据
                TaskSemaphore1.WaitOne();                //等待接收队列
                lock (ThreadLock1)                       //锁线程  
                {
                    GetTask = ShowQueue.Dequeue();
                }

                if (GetTask.ThzImage != null)
                {
                    if (!Directory.Exists("ZHEN"))
                        Directory.CreateDirectory("ZHEN");


                }

                if (ShowEvent != null && GetTask.ThzImage != null)
                    ShowEvent(GetTask.ThzImage, GetTask.Duration, GetTask.Temparture, GetTask.IsPeople, GetTask.IsHidden);

            }
        }

        public void Correct()
        {
            THZData.Recorrect = true;
        }

        //public void SetMirror(bool mirrorFlag)
        //{
        //    thzdata.MirrirFlag = mirrorFlag;
        //}
        #endregion

        /// <summary>
        /// 数据解码
        /// </summary>
        /// <param name="GetTask">List</param>
        /// <param name="listBuff">List</param>
        /// <param name="frame">输出PartData</param>
        /// <param name="FrameLength">FrameLength</param>
        /// <param name="PackageNum">PackageNum</param>
        void DataDecode(List<Byte[]> GetTask, ref List<Byte[]> listBuff, ref THZData frame)
        {
            int PackageNum = 25;
            //if (listBuff.Count >= 100)
            //{
            //    listBuff.Clear();
            //    return;
            //}
            //排除异常项
            listBuff.RemoveAll(s => s.Count() < 1000);

            StringBuilder sNeed = new StringBuilder();

            GetTask.AddRange(listBuff);
            listBuff.Clear();

            foreach (var item in GetTask.OrderBy(s => 256 * s[6] + s[7]).GroupBy(s => 256 * 256 * 256 * s[2] + 256 * 256 * s[3] + 256 * s[4] + s[5]))
            {
                //PackageNum = 256 * item.ToList()[0][0] + item.ToList()[0][1];
                if (item.Count() == PackageNum)
                {

                    var Zhendata = item.OrderBy(s => s[6] * 256 + s[7]).ToList();

                    if (Zhendata[0].Count() != 1420)
                        continue;

                    foreach (Byte[] match in Zhendata)
                        sNeed.Append(BitConverter.ToString(match).Replace("-", "").Substring(20).ToUpper());
                    //if (!Directory.Exists("ZHEN"))
                    //    Directory.CreateDirectory("ZHEN");
                    //writerFile(strToToHexByte(sNeed.ToString().Substring(0)), "ZHEN\\Port" + localPort + "_Frame" + (CountNum) % 10000 + ".bin");
                    CountNum++;
                    frame.Init(sNeed.ToString());//？？？
                    //DecodeFrame(sNeed, ref frame, indexHead, indexTair); 

                    sNeed.Clear();
                }
                else
                {
                    listBuff.AddRange(item.ToList());
                }
            }

        }
        /// <summary>
        /// 数据解码
        /// </summary>
        /// <param name="GetTask">List</param>
        /// <param name="listBuff">List</param>
        /// <param name="frame">输出PartData</param>
        /// <param name="FrameLength">FrameLength</param>
        /// <param name="PackageNum">PackageNum</param>
        void DataDecode_v1(List<Byte[]> GetTask, ref List<Byte[]> listBuff, ref THZData frame)   //ref作用?
        {
            int PackageNum = 25;
            //if (listBuff.Count >= 100)
            //{
            //    listBuff.Clear();
            //    return;
            //}
            //排除异常项
            listBuff.RemoveAll(s => s.Count() < 1000);

            StringBuilder sNeed = new StringBuilder();

            GetTask.AddRange(listBuff);
            listBuff.Clear();
           

            foreach (var item in GetTask.OrderBy(s => 256 * s[6] + s[7]).GroupBy(s => 256 * 256 * 256 * s[2] + 256 * 256 * s[3] + 256 * s[4] + s[5])) //按照包排序、按照帧分组，每次取出一帧
            {

                PackageNum = 256 * item.ToList()[0][0] + item.ToList()[0][1];//获取一帧前两个字节的值，即单帧包数
                if (item.Count() == PackageNum)//如果一帧的包数量和PackageNum相等则是一个完整的包
                {

                    var Zhendata = item.OrderBy(s => s[6] * 256 + s[7]).ToList();//一帧数据按照包排序后存放到Zhendata

                    if (Zhendata[0].Count() != 1420)//第一包不是1420则退出
                        continue;

                    //dc.tempartureData_zyr(Zhendata, ShowEvent_zyr2);
                    dc.adDataCaculate_zyr(Zhendata, ShowEvent_zyr1,ShowEvent_zyr2);

                    foreach (Byte[] match in Zhendata)  //每次取出一个包也就是1420字节，取一帧的数据
                        sNeed.Append(BitConverter.ToString(match).Replace("-", "").Substring(20).ToUpper());
                    //if (!Directory.Exists("ZHEN"))
                    //    Directory.CreateDirectory("ZHEN");
                    writerFile(strToToHexByte(sNeed.ToString().Substring(0)), "ZHEN\\Port" + localPort + "_Frame" + (CountNum) % 10000 + ".bin");
                    CountNum++;
                    frame.Init(sNeed.ToString());
                    //DecodeFrame(sNeed, ref frame, indexHead, indexTair); 

                    sNeed.Clear();
                }
                else
                {
                    listBuff.AddRange(item.ToList());
                }
            }

        }
        void DataDecode_v2(List<Byte[]> GetTask, ref List<Byte[]> listBuff, ref THZData frame)
        {
            int PackageNum = 25;
            //int ChannelCount = 36;
            //int SampleCount = 473;
     
            listBuff.RemoveAll(s => s.Count() < 1000);

            StringBuilder sNeed = new StringBuilder();

            GetTask.AddRange(listBuff);
            listBuff.Clear();

            foreach (var item in GetTask.OrderBy(s => 256 * s[6] + s[7]).GroupBy(s => 256 * 256 * 256 * s[2] + 256 * 256 * s[3] + 256 * s[4] + s[5])) //按照包排序、按照帧分组
            {
                PackageNum = 256 * item.ToList()[0][0] + item.ToList()[0][1];
                if (item.Count() == PackageNum)
                {

                    var Zhendata = item.OrderBy(s => s[6] * 256 + s[7]).ToList();

                    if (Zhendata[0].Count() != 1420)
                        continue;

                    /*mycode*/
                    /* List<Byte> frameByteData = new List<Byte>();
                     double[] ChannelData = new double[438] ;
                     double avg = 0, Variance;
                     foreach (Byte[] Package in Zhendata)
                         foreach (Byte byt in Package)
                             frameByteData.Add(byt);
                     MessageBox.Show(frameByteData.Count().ToString());
                     ChannelCount = 256 * item.ToList()[0][26] + item.ToList()[0][27];
                     SampleCount = 256 * item.ToList()[0][28] + item.ToList()[0][29];
                     for (int i = 0; i < ChannelCount; i++)    // 36
                     {
                         for (int j = 0; j < SampleCount; j++)  // 438
                             ChannelData[j]= frameByteData[j * 74 + 51+i] * 256 + frameByteData[j * 74 + 52+i];
                         avg = Var(ChannelData);
                         Variance=ChannelData.Average();
                         strWrite(ChannelData+"--"+avg+"--"+Variance, Environment.CurrentDirectory + "\\bin","ad.txt");

                     }*/

                    


                    foreach (Byte[] match in Zhendata)
                        sNeed.Append(BitConverter.ToString(match).Replace("-", "").Substring(20).ToUpper());

                    strToToHexByte(sNeed.ToString().Substring(0));


                    //if (!Directory.Exists("ZHEN"))
                    //    Directory.CreateDirectory("ZHEN");
                    writerFile(strToToHexByte(sNeed.ToString().Substring(0)), "ZHEN\\Port" + localPort + "_Frame" + (CountNum) % 10000 + ".bin");
                    CountNum++;
                    frame.Init(sNeed.ToString());
                    //DecodeFrame(sNeed, ref frame, indexHead, indexTair); 

                    sNeed.Clear();
                }
                else
                {
                    listBuff.AddRange(item.ToList());
                }
            }

        }


        /* public void callBack_zyr1(int row, int column, string str)
         {

            ShowEvent_zyr1(row, column, str);
          }
     public void callBack_zyr2(string str)
     {

         ShowEvent_zyr2(str);
     }*/
        #region mycode_zyr
        // DataProcess dp = null;
        //public void tempartureData_zyr(List<byte[]> Zhendata)
        //{
        //    StringBuilder sNeed = new StringBuilder();

        //    foreach (Byte[] match in Zhendata)
        //        sNeed.Append(BitConverter.ToString(match).Replace("-", "").Substring(20).ToUpper());
        //    double[] temparture = GetTemparture_zyr(sNeed.ToString());
        //    // mf.ShowlbDevTem(string.Format("设备温度：\nT1:{0:N}°\nT2:{1:N}°\nT3:{2:N}°\nT4:{3:N}°", temparture[0], temparture[1], temparture[2], temparture[3]));
        //    ShowEvent_zyr2(string.Format("设备温度：\nT1:{0:N}°\nT2:{1:N}°\nT3:{2:N}°\nT4:{3:N}°", temparture[0], temparture[1], temparture[2], temparture[3]));
        //   // dp.callBack_zyr2(string.Format("设备温度：\nT1:{0:N}°\nT2:{1:N}°\nT3:{2:N}°\nT4:{3:N}°", temparture[0], temparture[1], temparture[2], temparture[3]));
        //    sNeed.Clear();
        //    foreach (var tem in temparture)
        //        sNeed.Append(tem + "***");
        //    strWrite_zyr(sNeed.ToString(), Environment.CurrentDirectory + "\\bin", "tempartureData.txt");
        //    sNeed.Clear();


        //}

        public double[] GetTemparture_zyr(string str)
        {
            string tepStr = str.Substring(20 * 2, 8 * 2);
            double[] Temparture = new double[4];
            if (tepStr == null)
                return Temparture;
            byte[] Wen = DataProcess.strToToHexByte(tepStr);
            if ((Wen[0] & 0xf0) >> 4 == 15)
                Temparture[0] = -(~Wen[1] + 1 + 256.0 * ~Wen[0]) / 16;
            else
                Temparture[0] = (Wen[1] + 256.0 * Wen[0]) / 16;

            if ((Wen[2] & 0xf0) >> 4 == 15)
                Temparture[1] = -(~Wen[3] + 1 + 256.0 * ~Wen[2]) / 16;
            else
                Temparture[1] = (Wen[3] + 256.0 * Wen[2]) / 16;

            if ((Wen[4] & 0xf0) >> 4 == 15)
                Temparture[2] = -(~Wen[5] + 1 + 256.0 * ~Wen[4]) / 16;
            else
                Temparture[2] = (Wen[5] + 256.0 * Wen[4]) / 16;

            if ((Wen[6] & 0xf0) >> 4 == 15)
                Temparture[3] = -(~Wen[7] + 1 + 256.0 * ~Wen[6]) / 16;
            else
                Temparture[3] = (Wen[7] + 256.0 * Wen[6]) / 16;
            return Temparture;
        }
        int zhenRows = 0;
       /* public void adDataCaculate_zyr(List<byte[]> Zhendata)
        {
            byte[] byteData = new byte[35250];//1410*25
            int count1 = 0, count2 = 0;
            StringBuilder sNeed = new StringBuilder();
            MainForm mf = new MainForm();

            foreach (Byte[] Package in Zhendata)
            {
                count1 = 0;
                foreach (byte byt in Package)
                {
                    if (count1 >= 20)   //跳过第一个数
                    {

                        byteData[count2] = byt;
                        count2++;

                    }
                    count1++;
                }
            }
            // Console.ReadKey();
            double[] channel = new double[473];
            double variance = 0.0, average = 0.0;
            for (int i = 0; i < 36; i++)
            {
                for (int j = 0; j < 473; j++)
                {
                    byte bigByte = Convert.ToByte(byteData[j * 74 + i * 2].ToString("X"), 16);
                    if ((bigByte & 0xf0) >> 4 == 15)
                        channel[j] = -(~byteData[j * 74 + i * 2 + 30] + 1 + 256.0 * ~byteData[j * 74 + i * 2 + 31]) / 8192.0 * 10.0;//2^12 =4096
                    else
                        channel[j] = (byteData[j * 74 + i * 2 + 30] + 256.0 * ~byteData[j * 74 + i * 2 + 31]) / 8192.0 * 10.0;
                    sNeed.Append(channel[j] + ",");
                }
                variance = Var_zyr(channel);
                //mf.dataShow(zhenRows, 2 * i + 1, variance.ToString());
                ShowEvent_zyr1(zhenRows, 2 * i + 1, variance.ToString());
                //dp.callBack_zyr1(zhenRows, 2 * i + 1, variance.ToString());
                sNeed.Append("***variance:" + variance + "***average:");
                average = channel.Average();
                // mf.dataShow(zhenRows, 2 * i , average.ToString());
                 ShowEvent_zyr1(zhenRows, 2 * i, average.ToString());
                //dp.callBack_zyr1(zhenRows, 2 * i, average.ToString());
                sNeed.Append(average);
                strWrite_zyr(sNeed.ToString(), Environment.CurrentDirectory + "\\bin", "channelData.txt");
                sNeed.Clear();
            }
            zhenRows++;
        }*/

        public double Var_zyr(double[] v)
        {
            double sum1 = 0;
            for (int i = 0; i < v.Length; i++)
            {
                double temp = v[i] * v[i];
                sum1 = sum1 + temp;
            }

            double sum = 0;
            foreach (double d in v)
            {
                sum = sum + d;
            }
            double var = sum1 / v.Length - (sum / v.Length) * (sum / v.Length);
            return var;
        }
        private int rowCount = 0;
        private void strWrite_zyr(string str, string filePath, string fileName)
        {

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (!File.Exists(filePath + "\\" + fileName))
                File.Create(filePath + "\\" + fileName).Close(); //.Close 很关键，不然会有问题
            if (rowCount < 3600)
            {
                StreamWriter sw = new StreamWriter(filePath + "\\" + fileName, true);//true 追加数据
                sw.WriteLine(str);
                sw.Close();
                rowCount++;
            }
            else
            {
                StreamWriter sw = new StreamWriter(filePath + "\\" + fileName, false);
                sw.WriteLine(str);
                sw.Close();
                rowCount = 0;
            }
        }
        #endregion 
        private void strWrite(string str, string filePath, string fileName)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            if (!File.Exists(filePath + "\\" + fileName))
                File.Create(filePath + "\\" + fileName).Close(); //.Close 很关键，不然会有问题

            //方法一
            StreamWriter sw = new StreamWriter(filePath + "\\" + fileName, true);
            sw.WriteLine(str);
            sw.Close();

            //方法2
           /* string path = "D\1.txt";//文件的路径，保证文件存在。
            FileStream fs = new FileStream(path, FileMode.Append);
            SteamWriter sw = new StreamWriter(fs);
            sw.WriteLine(要追加的内容);
            sw.Close();
            fs.Close();*/
        }

       
        /// <summary>
        /// 字符串转16进制Byte字节
        /// </summary>
        /// <param name="hexString">输入字符串</param>
        /// <returns>转化的Byte字节</returns>
        public static byte[] strToToHexByte(string hexString)
        {
            hexString = hexString.Replace("-", "");
            if ((hexString.Length % 2) != 0)
                hexString += "20";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        /// <summary>
        /// 帧数据读取，存放
        /// </summary>
        /// <param name="array">帧数据数组</param>
        /// <param name="strPath">存放文件</param>
        private void writerFile(byte[] array, string strPath)
        {
            //string content = this.txtContent.Text.ToString();

            if (string.IsNullOrEmpty(strPath))
            {
                return;
            }

            //将string转为byte数组
            //byte[] array = Encoding.UTF8.GetBytes(content);

            //string path = Server.MapPath("/test.txt");
            //创建一个文件流
            FileStream fs = new FileStream(strPath, FileMode.Create);

            //将byte数组写入文件中
            fs.Write(array, 0, array.Length);
            //所有流类型都要关闭流，否则会出现内存泄露问题
            fs.Close();
            //Response.Write("保存文件成功");
        }

        private void Logger(string fileName, string content)
        {
            //StreamWriter sw = new StreamWriter(fileName, true);
            //sw.Write(content);
           // sw.Close(); sw.Dispose();

        }
    }

    class ShowData
    {
        public Bitmap ThzImage;
        public string[] Duration;
        public double[] Temparture;
        public bool IsPeople;
        public bool IsHidden;
        public ShowData() { }
        public ShowData(Bitmap bitmap, double[] temparture, string[] duration, bool peopleFlag, bool hidFlag)
        {
            ThzImage = bitmap;
            IsPeople = peopleFlag;
            IsHidden = hidFlag;
            Duration = duration;
            Temparture = temparture;
        }
    }
}
