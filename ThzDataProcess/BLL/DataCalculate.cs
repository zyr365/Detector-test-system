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
        public void adDataCaculate_zyr(List<byte[]> Zhendata, Action<int, int, string> ShowEvent_zyr1, Action<int,Double> ShowEvent_zyr3)
        {
            byte[] byteData = new byte[35250];//1410*25
            int count1 = 0, count2 = 0,sampleCount=0;
            StringBuilder sNeed = new StringBuilder();
            //MainForm mf = new MainForm();

            sampleCount = Zhendata[0][28] * 256 + Zhendata[0][29];
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
            double variance = 0.0, average = 0.0;
            for (int i = 0; i < 36; i++)
            {
                for (int j = 0; j < sampleCount; j++)
                {
                    byte bigByte = Convert.ToByte(byteData[j * 74 + i * 2].ToString("X"), 16);
                    if ((bigByte & 0xf0) >> 4 == 15)
                        channel[j] = -(~byteData[j * 74 + i * 2 + 30] + 1 + 256.0 * ~byteData[j * 74 + i * 2 + 31]) / 8192.0*10.0;//2^12 =4096
                    else
                        channel[j] = (byteData[j * 74 + i * 2 + 30] + 256.0 * ~byteData[j * 74 + i * 2 + 31]) / 8192.0*10.0;
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
                 ShowEvent_zyr3(i,average);
                //dp.callBack_zyr1(zhenRows, 2 * i, average.ToString());
                sNeed.Append(average);
                strWrite_zyr(sNeed.ToString(), Environment.CurrentDirectory + "\\bin", "channelData.txt");
                sNeed.Clear();
            }
            zhenRows++;
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
