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
    class DataCalculate
    {

       
        DataProcess dp = null;
        public  void tempartureData_zyr(List<byte[]> Zhendata, Action<string> ShowEvent_zyr2)
        {
            StringBuilder sNeed = new StringBuilder();
           
            foreach (Byte[] match in Zhendata)
                sNeed.Append(BitConverter.ToString(match).Replace("-", "").Substring(20).ToUpper());
            double[] temparture = GetTemparture_zyr(sNeed.ToString());
            // mf.ShowlbDevTem(string.Format("设备温度：\nT1:{0:N}°\nT2:{1:N}°\nT3:{2:N}°\nT4:{3:N}°", temparture[0], temparture[1], temparture[2], temparture[3]));
            ShowEvent_zyr2(string.Format("设备温度：\nT1:{0:N}°\nT2:{1:N}°\nT3:{2:N}°\nT4:{3:N}°", temparture[0], temparture[1], temparture[2], temparture[3]));
           // dp.callBack_zyr2(string.Format("设备温度：\nT1:{0:N}°\nT2:{1:N}°\nT3:{2:N}°\nT4:{3:N}°", temparture[0], temparture[1], temparture[2], temparture[3]));
            sNeed.Clear();
            foreach (var tem in temparture)
                sNeed.Append(tem + "***");
            strWrite_zyr(sNeed.ToString(), Environment.CurrentDirectory + "\\bin", "tempartureData.txt");
            sNeed.Clear();


        }

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
        public void adDataCaculate_zyr(List<byte[]> Zhendata, Action<string, string, string,int> ShowEvent_zyr1, Action<double,int> ShowEvent_zyr2)
        {
            //byte[] byteData = new byte[35250];//1410*25
           
            int count1 = 0, count2 = 0,sampleCount=0;
            StringBuilder sNeed = new StringBuilder();
            StringBuilder sNeed1 = new StringBuilder();
            //MainForm mf = new MainForm();

            sampleCount = Zhendata[0][28] * 256 + Zhendata[0][29];
            byte[] byteData = new byte[Zhendata.Count()* 1410];//1410*44
            foreach (Byte[] Package in Zhendata)
            {
                count1 = 0;
                foreach (byte byt in Package)
                {
                    if (count1 >= 10)   //跳过第一个数
                    {

                        byteData[count2] = byt;
                        count2++;

                    }
                    count1++;
                }
            }
            // Console.ReadKey();
            double[] channel = new double[sampleCount];//473、465
            double[] channel1 = new double[sampleCount];
            double variance = 0.0, average_original = 0.0 , average_converted = 0.0,stdDev = 0.0,maxNum= 0.0,minNum = 0.0;
            for (int i = 0; i < 36; i++)
            {
                for (int j = 0; j < sampleCount; j++)
                {
                    byte bigByte = Convert.ToByte(byteData[j * 74 + i * 2 + 40].ToString("X"), 16);
                    if ((bigByte & 0xf0) >> 4 == 15)
                    {
                        channel[j] = -(~byteData[j * 74 + i * 2 + 41] + 1 + 256.0 * ~byteData[j * 74 + i * 2 + 40]) / 8192.0 * 5;//2^12 =4096
                        channel1[j] = -(~byteData[j * 74 + i * 2 + 41] + 1 + 256.0 * ~byteData[j * 74 + i * 2 + 40]);
                    }
                    else
                    {
                        channel[j] = (byteData[j * 74 + i * 2 + 41] + 256.0 * byteData[j * 74 + i * 2 + 40]) / 8192.0 * 5;
                        channel1[j] = (byteData[j * 74 + i * 2 + 41] + 256.0 * byteData[j * 74 + i * 2 + 40]) ;
                    }
                    sNeed.Append(channel[j] + ",");
                    sNeed1.Append(channel1[j] + ",");
                }

                Stopwatch elapsetime = new Stopwatch();
                elapsetime.Start();


                stdDev = CalculateStdDev(channel)*1000;//标准差
                average_converted = channel.Average()*1000;
                maxNum = channel.Max()*1000;
                minNum = channel.Min()*1000;
                average_original = channel.Average();

                ShowEvent_zyr1(average_converted.ToString("0.00"), stdDev.ToString("0.00") ,maxNum.ToString("0.00")+"/"+minNum.ToString("0.00"),i+1);
                ShowEvent_zyr2(average_original, i+1);

                elapsetime.Stop();
                Console.WriteLine(elapsetime.ElapsedMilliseconds.ToString("000"));

                //variance = Var_zyr(channel);//方差
                // ShowEvent_zyr1(zhenRows, 2 * i + 1, variance.ToString());
                //sNeed.Append("***variance:" + variance + "***average:");

                //ShowEvent_zyr1(zhenRows, 2 * i, average.ToString());
                // ShowEvent_zyr3(i+1,average);
                //sNeed.Append(average);
                //strWrite_zyr(sNeed.ToString(), Environment.CurrentDirectory + "\\bin", "channelData.txt");
                //sNeed.Clear();
            }
            zhenRows++;
        }

       // private static double CalculateStdDev(IEnumerable<double> values)
             private static double CalculateStdDev(double[] values)
        {
            double ret = 0;
            if (values.Count() > 0)
            {
                //  计算平均数   
                double avg = values.Average();
                //  计算各数值与平均数的差值的平方，然后求和 
                double sum = values.Sum(d => Math.Pow(d - avg, 2));
                //  除以数量，然后开方
                ret = Math.Sqrt(sum / values.Count());
            }
            return ret;
        }

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
    }
}
