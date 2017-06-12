using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaySystem.DLL.Coin
{
    class CoinManager
    {

        SerialPortManager spm = new SerialPortManager();
        

        byte EventCounter = 0; //纪录硬币事件个数

        public CoinManager(string COM)
        {
            spm.OpenSerialPort(COM); //new 这个对象时就启动串口
        }


    //////////////////////////////开关串口////////////////////////////
        public void OpenSerialPort(string COM)
        {
            spm.OpenSerialPort(COM);  
        }

        public void CloseSerialPort()
        {
            spm.CloseSerialPort();
        }

  

        //////////////////////////////硬币找零////////////////////////////
        public string PayoutRevice()
        {

            byte[] temp;
            string str = "";
            if (spm.ReceiveCompleted)
            {
                temp = spm.ReceivedDataPacket;

                if (temp.Length > 11)
                {
                    byte b = 0x56;
                    int position = Array.IndexOf(temp, b);

                    if(temp.Length - position > 7) {
                        if (temp[position + 1] == 1 && temp[position + 2] == 4 && temp[position + 3] == 3 && temp[position + 4] == 0)
                            str = temp[position + 6].ToString() + "," + temp[position + 7].ToString();

                        new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CoinRevice-->剩余：" + temp[position + 6]+ "出币个数： " + temp[position + 7]);
                    }
                }
            }
            spm.SendDataPacket(HopperCmd.RequestPayoutCoinStatus_Send);

            return str;
        }

        public void PayoutCoinReset()
        {
            spm.SendDataPacket(HopperCmd.PayoutCoinReset_Send);
        }

        public bool PayoutCoin(byte CoinCount)
        {
            byte[] temp = new byte[] { 0x03, 0x09, 0x01, 0xA7, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };

            spm.CleanBuffer(); 

            
                    spm.SendDataPacket(HopperCmd.PayoutCoinRequestCipher_Send);
                    Thread.Sleep(157);
                    if (spm.ReceiveCompleted)
                    {
                         byte[] a = spm.ReceivedDataPacket;
                        new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CoinPayout-->获取密码成功 " );

                        temp[12] = CoinCount;
                        temp[13] = CheckSum(temp);
                        spm.SendDataPacket(temp);

                        Thread.Sleep(57);
                        if (spm.ReceiveCompleted)
                        {
                            new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CoinPayout-->出币成功 " );
                            return true;
                        }
                    }
                
            
            new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CoinPayout-->出币失败");
            return false;
           
        }

        byte CheckSum(byte[] indata) //计算校验值
        {
            byte temp = 0;

            for (int i = 0; i < indata.Length; i++)
                temp += indata[i];

            temp = (byte)((0xff - temp) + 1);
            return temp;
        }

        //判断钱箱是否有钱
       public int CheckPayoutCoinEnable()
        {

            int i = 2;
            spm.SendDataPacket(HopperCmd.PayoutCoinCheck_Send);
            while (true)
            {
                Thread.Sleep(57);
                if (spm.ReceiveCompleted)
                {
                    byte[] temp = spm.ReceivedDataPacket;
                    if (temp.Length > 10)
                    {
                        if (temp[9] == 0x32)
                        {
                            //  "老多钱了";
                            return 2;
                        }
                        else if (temp[9] == 0x30)
                        {
                            //提示 快去补钱了
                            // "差不多一半吧。";
                            return 1;
                        }
                        else if (temp[9] == 0x31)
                        {
                            // "没钱了。";
                            return 0;
                        }
                    }
                    else
                    {
                        //textBox.Text = "硬币数量检测错误";
                        return -1;
                    }
                }

                if (i == 0)
                {
                    return -1;
                }
                i--;
            }

        }


        //////////////////////////////硬币收币////////////////////////////

        public void CloseGetCoinChannelALL()
        {
            spm.SendDataPacket(HopperCmd.GetCoinCloseChannelALL_Send);
            
        }


        //返回硬币事件处理个数
        public int GetInCoin()
        {
            byte[] temp;
            int CoinOneCount = 0;
            if (spm.ReceiveCompleted)
            {
                temp = spm.ReceivedDataPacket;

                if (temp.Length > 17)
                {

                    CoinOneCount = reviceCoin(temp);
                    
                }
            }

            spm.SendDataPacket(HopperCmd.GetCoinReciveCoin_Send);

            return CoinOneCount;
        }



        int reviceCoin(byte[] getdata)
        {
            byte b = 0x0b;
            int CoinOneCount = 0;
            int position = Array.IndexOf(getdata, b);

            if (position > 0 && getdata[position - 1] == 0x01 && getdata[position + 1] == 0x02 && getdata[position + 2] == 0 && getdata[position + 3] != EventCounter)
            {
                if (getdata[position + 3] - EventCounter == 1)//处理一个
                {
                    EventCounter = getdata[position + 3];

                    if (getdata[position + 4] == 1 && getdata[position + 5] == 1)
                        CoinOneCount++;

                }
                else if (getdata[position + 3] - EventCounter == 2)//处理连续两个硬币
                {
                    EventCounter = getdata[position + 3];

                    if (getdata[position + 4] == 1 && getdata[position + 5] == 1)
                        CoinOneCount++;

                    if (getdata[position + 6] == 1 && getdata[position + 7] == 1)
                        CoinOneCount++;
                }

                return CoinOneCount;
            }
            else
            {
                return 0;
            }

        }


        public bool CheckCoinInStatus()
        {
            int i = 2;
            //检测在线
            
            spm.SendDataPacket(HopperCmd.GetCoinOnLine_Send);
            while (true)
            {
                Thread.Sleep(57);
                if (spm.ReceiveCompleted)
                {
                    if (HopperCmd.CheckEquals(spm.ReceivedDataPacket, HopperCmd.GetCoinOnLine_Get))
                        break;
                }
                
                if(i==0)
                {
                    return false;
                }
                i--;
            }
            

            //设备自检
            i = 2;
            spm.SendDataPacket(HopperCmd.GetCoinSelfCheck_Send);
            while (true)
            {
                Thread.Sleep(57);
                if (spm.ReceiveCompleted)
                {
                    if (HopperCmd.CheckEquals(spm.ReceivedDataPacket, HopperCmd.GetCoinSelfCheck_Get))
                        break;
                }

                if (i == 0)
                {
                    return false;
                }
                i--;
            }
            //设备复位
            i = 2;
            spm.SendDataPacket(HopperCmd.GetCoinReset_Send);
            while (true)
            {
                Thread.Sleep(57);
                if (spm.ReceiveCompleted)
                {
                    if (HopperCmd.CheckEquals(spm.ReceivedDataPacket, HopperCmd.GetCoinReset_Get))
                        break;
                }

                if (i == 0)
                {
                    return false;
                }
                i--;
            }
            //打开一元通道
            i = 2;
            spm.SendDataPacket(HopperCmd.GetCoinOpenChannelOne_Send);
            while (true)
            {
                Thread.Sleep(57);
                if (spm.ReceiveCompleted)
                {
                    if (HopperCmd.CheckEquals(spm.ReceivedDataPacket, HopperCmd.GetCoinOpenChannelOne_Get))
                        break;
                }

                if (i == 0)
                {
                    return false;
                }
                i--;
            }

            //检查出币设备是够在线
            i = 2;
            spm.SendDataPacket(HopperCmd.PayoutCoinOnLine_Send);
            while (true)
            {
                Thread.Sleep(57);
                if (spm.ReceiveCompleted)
                {
                    if (HopperCmd.CheckEquals(spm.ReceivedDataPacket, HopperCmd.PayoutCoinOnLine_Get))
                        break;
                }

                if (i == 0)
                {
                    return false;
                }
                i--;
            }

            //出币设备复位
            i = 2;
            spm.SendDataPacket(HopperCmd.PayoutCoinReset_Send);
            while (true)
            {
                Thread.Sleep(57);
                if (spm.ReceiveCompleted)
                {
                    if (HopperCmd.CheckEquals(spm.ReceivedDataPacket, HopperCmd.PayoutCoinReset_Get))
                        break;
                }

                if (i == 0)
                {
                    return false;
                }
                i--;
            }
            //出币设备使能
            i = 2;
            spm.SendDataPacket(HopperCmd.PayoutCoinEnable_Send);
            while (true)
            {
                Thread.Sleep(57);
                if (spm.ReceiveCompleted)
                {
                    if (HopperCmd.CheckEquals(spm.ReceivedDataPacket, HopperCmd.PayoutCoinEnable_Get))
                        break;
                }

                if (i == 0)
                {
                    return false;
                }
                i--;
            }

            spm.SendDataPacket(HopperCmd.PayoutCoinRequestCipher_Send);

            EventCounter = 0;
            //spm.CleanBuffer();
            //初始化完成
            return true;
        }



    }
}
