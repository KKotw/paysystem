using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PaySystem.DLL.Coin
{

    public class SerialPortManager
    {
        private bool _recStaus = true;//接收状态字
        private bool _comPortIsOpen;
        private void SetAfterClose()//成功关闭串口或串口丢失后的设置
        {
            _comPortIsOpen = false;//串口状态设置为关闭状态 
        }
        private void SetComLose()//成功关闭串口或串口丢失后的设置
        {
            SetAfterClose();//成功关闭串口或串口丢失后的设置
        }
        public SerialPort CurrentSerialPort { get; set; } = new SerialPort();
        public byte[] ReceivedDataPacket { get; set; }

        public bool OpenSerialPort(String PortName)
        {
      
           
            if (_comPortIsOpen == false) 
            {
                try //尝试打开串口
                {
                    CurrentSerialPort.ReadTimeout = 8000; //串口读超时8秒
                    CurrentSerialPort.WriteTimeout = 8000; //串口写超时8秒，在1ms自动发送数据时拔掉串口，写超时5秒后，会自动停止发送，如果无超时设定，这时程序假死
                    CurrentSerialPort.ReadBufferSize = 1024; //数据读缓存
                    CurrentSerialPort.WriteBufferSize = 1024; //数据写缓存
                    CurrentSerialPort.DataReceived += ComReceive; //串口接收中断
                    CurrentSerialPort.PortName = PortName;
                    CurrentSerialPort.Open();
                    _comPortIsOpen = true; //串口打开状态字改为true                 
                }
                catch (Exception exception) //如果串口被其他占用，则无法打开
                {
                    _comPortIsOpen = false;
                    ReceiveCompleted = false;
                    throw new Exception("unable open serial port" + exception.Message);
                }
                return true;
            }
            return true;
        }

        public char[] ReceivedDataPacketChar { get; set; }
        public bool ReceiveCompleted { get; set; }

        private void ComReceive(object sender, SerialDataReceivedEventArgs e)
        {
            ReceiveCompleted = false;
            if (_recStaus) //如果已经开启接收
            {
                try
                {
                    Thread.Sleep(50);
                    ReceivedDataPacket = new byte[CurrentSerialPort.BytesToRead];
                    ReceivedDataPacketChar = new char[CurrentSerialPort.BytesToRead];
                    // change to char datas 
                    if (ByteMode)
                    {
                        CurrentSerialPort.Read(ReceivedDataPacket, 0, ReceivedDataPacket.Length);
                        /*
                        string s = "";
                        for (int a = 0; a < ReceivedDataPacket.Length; a++)
                            s += ReceivedDataPacket[a] + " ";
                        new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---serialGetData-->" + s);
                        */
                    }
                    else
                    {
                        CurrentSerialPort.Read(ReceivedDataPacketChar, 0, CurrentSerialPort.BytesToRead);
                    }
                    ReceiveCompleted = true;
                }
                catch (Exception)
                {
                    if (CurrentSerialPort.IsOpen == false) //如果ComPort.IsOpen == false，说明串口已丢失
                    {
                        SetComLose(); //串口丢失后相关设置
                    }
                    else
                    {
                        throw new Exception("unable to receive data");
                    }
                }
            }
            else //暂停接收
            {
                CurrentSerialPort.DiscardInBuffer(); //清接收缓存
            }
        }



        public bool SendDataPacket(string dataPacket)
        {
            char[] dataPacketChar = dataPacket.ToCharArray();
            return SendDataPacket(dataPacketChar);
        }

        public bool SendDataPacket(byte[] dataPackeg)
        {
            try
            {
                CurrentSerialPort.DiscardInBuffer();//清接收缓存
                ByteMode = true;
                CurrentSerialPort.Write(dataPackeg, 0, dataPackeg.Length);

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
            return true;
        }

        public void CleanBuffer()
        {
            CurrentSerialPort.DiscardOutBuffer();//清发送缓存
            CurrentSerialPort.DiscardInBuffer();//清接收缓存
        }

        public bool CloseSerialPort()
        {
            try//尝试关闭串口
            {
                CurrentSerialPort.DiscardOutBuffer();//清发送缓存
                CurrentSerialPort.DiscardInBuffer();//清接收缓存
                //WaitClose = true;//激活正在关闭状态字，用于在串口接收方法的invoke里判断是否正在关闭串口
                CurrentSerialPort.Close();//关闭串口
                                          // WaitClose = false;//关闭正在关闭状态字，用于在串口接收方法的invoke里判断是否正在关闭串口
                SetAfterClose();//成功关闭串口或串口丢失后的设置
                _comPortIsOpen = false;
            }
            catch//如果在未关闭串口前，串口就已丢失，这时关闭串口会出现异常
            {
                if (CurrentSerialPort.IsOpen == false)//判断当前串口状态，如果ComPort.IsOpen==false，说明串口已丢失
                {
                    SetComLose();
                }
                else//未知原因，无法关闭串口
                {
                    throw new Exception("unable close serial port");
                }
            }
            return true;
        }

        public bool ByteMode { get; set; }

        public bool SendDataPacket(char[] senddata)
        {
            try
            {
                ByteMode = false;
                CurrentSerialPort.Write(senddata, 0, senddata.Length);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
                return false;
            }
            return true;
        }
    }
}
