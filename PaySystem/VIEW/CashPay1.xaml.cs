using PaySystem.DLL.Classes;
using PaySystem.DLL.Coin;
using PaySystem.DLL.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PaySystem.VIEW
{
    /// <summary>
    /// CashPay1.xaml 的交互逻辑
    /// </summary>
    public partial class CashPay1 : Window
    {
        DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
        CPayout Payout;
        TextBox textBox1 = new TextBox();
        CoinManager Cm;

        int GetCash = 0;
        int[] payoutcash = new int[] { 0,0,0,0,0,0,0 }; // 1 ; 0 ; 5 ;10 ;20 ;50 ;100
        int[] UserInCash = new int[] { 0, 0, 0, 0, 0, 0, 0 ,0 }; // 1 ; 0 ; 5 ;10 ;20 ;50 ;100 ; coin
        int[] UserChange = new int[] { 0, 0, 0, 0, 0, 0, 0, 0 }; // 1 ; 0 ; 5 ;10 ;20 ;50 ;100 ; coin


        bool PayoutOrGetin = false; //true  找钱；false 收钱
        int ClickStatus = 0; // 1 退款  2 找零 0 其他状态

        int ParkPrice = 0;
        bool GiveBack = false;
        bool PayOutChange = false;
        string CarNum = "";
        string recordId = "";
        string InTime = "";
        bool IsInitSuccesed = false;

        bool IsCashEnable = true;


        public CashPay1(int Price, string CarNumber, string recordsId, string inTime)
        {
            InitializeComponent();

            this.ParkPrice = Price/100;
            this.CarNum = CarNumber;
            this.recordId = recordsId;  //付款使用的id
            this.InTime = inTime;

            //ParkPrice = 3;  //强制测试使用

            //set background
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/background.png"));
            this.Background = imageBrush;


            string CashEnable = ConfigurationManager.AppSettings["CASHENABLE"];
            if (CashEnable == "F")
            {
                IsCashEnable = false;
            }


            label.Content = "车牌：" + CarNumber;
            labe2.Content = "应付金额：               元";          
            labe3.Content = ((double)ParkPrice).ToString("0.00");

             prompt.Text =   ConfigurationManager.AppSettings["warn"];

            IsInitSuccesed = CashPayInit();
            if (IsInitSuccesed)
            {
                dTimer.IsEnabled = true;
            }
            else
            {
                dTimer.IsEnabled = false;
                Payout.Reset(textBox1);
                prompt.Text = "设备初始化错误，请返回主页重新操作";
            }
            
        }

        private void button_Click(object sender, RoutedEventArgs e) //返回
        {
            dTimer.IsEnabled = false;
            Payout.Reset(textBox1); 
            Payout.SSPComms.CloseComPort();
            Cm.CloseGetCoinChannelALL();
            Cm.PayoutCoinReset();
            Cm.CloseSerialPort();

            new MainWindow().Show();
            this.Close();
        }


        private void button1_Click(object sender, RoutedEventArgs e) //退钱
        {

            if (IsInitSuccesed == false)
                return;

            int temp = 0;
            for (int a = 0; a < 8; a++)
                temp+=UserInCash[a];

            if (temp > 0)
            {
                // PayoutOrGetin = true;
                ClickStatus = 1;
                GiveBack = true;
                CashGiveBack();
                // button2.IsEnabled = false;
                prompt.Text = " 正在执行退款操作，请勿离开 ";
            }
            else
            {
                prompt.Text = " 尚未投币，请重新选择 ";
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e) //确定支付
        {

            if (IsInitSuccesed == false)
                return;



            if (GetCash < ParkPrice)
            {
                prompt.Text = "金额不足，请继续投币。如果零钱不足请选择退款。";
            }
            else if (GetCash == ParkPrice)
            {
                GetCash = 0;
                dTimer.IsEnabled = false;
                Payout.Reset(textBox1);
                Payout.SSPComms.CloseComPort();
                Cm.CloseGetCoinChannelALL();
                Cm.PayoutCoinReset();
                Cm.CloseSerialPort();


                Thread.Sleep(1000);
                new VIEW.PaySuccess(ParkPrice * 100, CarNum, recordId, InTime).Show();
                this.Close();
            }
            else if (GetCash > ParkPrice && IsCashEnable == true)
            {
                prompt.Text = " 正在找零 ";
                ClickStatus = 2;
                calculate(GetCash - ParkPrice);
                GetCash = 0;
                //检查钱箱是否有钱
                if (CheckCashBox())
                {
                    //PayoutOrGetin = true;
                    PayOutChange = true;
                    ChangePayout();
                    //button1.IsEnabled = false;
                }
                else
                {
                    ClickStatus = 0;
                    prompt.Text = "钱箱零钱不足，请选择其他支付方式并退款";
                }
            }
            else
            {
                if (IsCashEnable == false)
                {
                    prompt.Text = "钱箱零钱不足，请选择其他支付方式并退款";
                    return;
                }
            }
    
        }

        bool CheckCashBox()
        {
            for (int a = 0; a < 7; a++)
            {
                if (payoutcash[a] < UserChange[a])
                    return false;
            }

            //check coin cashbox
           // if (Cm.CheckPayoutCoinEnable() == 0)
             //   return false;

            return true;
        }

        void calculate(int price) //找零策略
        {
            UserChange[3] = price / 10;
            UserChange[2] = (price % 10)/5;
            UserChange[7] = (price % 10) % 5;

        }

        private void dTimer_Tick(object sender, EventArgs e) //定时任务函数
        {
            CPayout.price = "";
            if (Payout.DoPoll(textBox1) == false)
            {
               
                while (true)
                {
                    Payout.SSPComms.CloseComPort();            
                    if (ConnectToValidator(5) == true)
                        break; 
                    Payout.SSPComms.CloseComPort(); 
                    return;
                }
                
            }
            else
            {
                //处理正常收取纸币函数
                HandleSmartPayout();
            }


            //硬币收找处理
            if (PayoutOrGetin)  
            {

                string str = Cm.PayoutRevice();
                
                if (str != "" && IsCashEnable == true)
                {
                    string[] s = str.Split(',');
                    if (s[0] == "0" && s[1] !="0" )
                    {
                       
                        if (ClickStatus == 2)
                        {
                            new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CashPayout-->付款完毕处理");

                            ClickStatus = 0;
                            prompt.Text = " 处理完毕，1秒后自动跳转 ";
 
                            dTimer.IsEnabled = false;
                            Payout.Reset(textBox1);
                            Payout.SSPComms.CloseComPort();
                            Cm.CloseGetCoinChannelALL();
                            Cm.PayoutCoinReset();
                            Cm.CloseSerialPort();

                            Thread.Sleep(1000);
                            new VIEW.PaySuccess(ParkPrice * 100, CarNum, recordId, InTime).Show();
                            this.Close();
                        }
                        else if (ClickStatus == 1)
                        {
                            new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CashPayout-->退款完毕处理");

                            ClickStatus = 0;
                            prompt.Text = " 退款完毕 ";

                            dTimer.IsEnabled = false;
                            Payout.Reset(textBox1);
                            Payout.SSPComms.CloseComPort();
                            Cm.CloseGetCoinChannelALL();
                            Cm.PayoutCoinReset();
                            Cm.CloseSerialPort();

                            Thread.Sleep(1000);
                            new MainWindow().Show();
                            this.Close();
                        }
                        PayoutOrGetin = false;
                    }
                    else //硬币没了 出纸币（标记没钱找了） 通知ipc
                    {

                        /*
                        IsCashEnable = false;
                        new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CashPayout-->硬币找零不足，开启通过纸币找零"  );
                        UserChange[0] = int.Parse(s[0]);
                        PayoutOrGetin = true;
                        PayOutChange = true;
                        ChangePayout();
                       // button1.IsEnabled = false;
                       */

                    }
                }
            }
            else
            {
              
                int temp = Cm.GetInCoin();   
                if(temp>0)
                { 
                UserInCash[7] += temp;
                GetCash += temp;
                CheckCashEnoughPay(true);
                }
            }

        }

        void CashGiveBack()  //退钱
        {
            CPayout.flag = "";
            for (int a = 0; a < 7; a++)
            {
                if (UserInCash[a] > 0)
                {
                    
                    switch (a)
                    {
                        case 0: CalculatePayout("1.00".ToString(), ("CNY").ToCharArray()); UserInCash[a] -= 1; break;
                        case 2: CalculatePayout("5.00".ToString(), ("CNY").ToCharArray()); UserInCash[a] -= 1; break;
                        case 3: CalculatePayout("10.00".ToString(), ("CNY").ToCharArray()); UserInCash[a] -= 1; break;
                        case 4: CalculatePayout("20.00".ToString(), ("CNY").ToCharArray()); UserInCash[a] -= 1; break;
                        case 5: CalculatePayout("50.00".ToString(), ("CNY").ToCharArray()); UserInCash[a] -= 1; break;
                        case 6: CalculatePayout("100.00".ToString(), ("CNY").ToCharArray()); UserInCash[a] -= 1; break;
                       // case 7: Cm.PayoutCoin((byte)UserInCash[a]); UserInCash[a] = 0; PayoutOrGetin = true; break;
                    }
                    new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CashPayout-->纸币退款:代号" + a);
                    return;

                }
            }


            if (UserInCash[7] > 0)
            {
               
                PayoutOrGetin = true;
                if(Cm.PayoutCoin((byte)UserInCash[7]))
                {
                    new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CashPayout-->硬币退款:" + UserInCash[7]);
                    UserInCash[7] = 0;
                }            
                  

                return;
            }
 
            GiveBack = false;
            //button2.IsEnabled = true;
            GetCash = 0;

        }

        void ChangePayout()//找钱
        {
            CPayout.flag = "";
            for (int a = 0; a < 7; a++)
            {
                if (UserChange[a] > 0)
                {

                    switch (a)
                    {
                        case 0: CalculatePayout("1.00".ToString(), ("CNY").ToCharArray()); UserChange[a] -= 1; break;
                        case 2: CalculatePayout("5.00".ToString(), ("CNY").ToCharArray()); UserChange[a] -= 1; break;
                        case 3: CalculatePayout("10.00".ToString(), ("CNY").ToCharArray()); UserChange[a] -= 1; break;
                        case 4: CalculatePayout("20.00".ToString(), ("CNY").ToCharArray()); UserChange[a] -= 1; break;
                        case 5: CalculatePayout("50.00".ToString(), ("CNY").ToCharArray()); UserChange[a] -= 1; break;
                        case 6: CalculatePayout("100.00".ToString(), ("CNY").ToCharArray()); UserChange[a] -= 1; break;
                        //case 7: Cm.PayoutCoin((byte)UserChange[a]); UserChange[a] = 0; PayoutOrGetin = true; break;
                    }

                    /*
                    if (a == 0 && UserChange[0]==0)
                    {
                        UserChange[7] = 0;
                        
                        Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        cfa.AppSettings.Settings["CASHENABLE"].Value = "F";
                        cfa.Save();
                    }
                    */
                    new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CashPayout-->纸币找零:代号" + a);
                    return;

                }


            }

            if (UserChange[7] > 0)
            {
                
                PayoutOrGetin = true;
                if (Cm.PayoutCoin((byte)UserChange[7]))
                {

                    new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CashPayout-->硬币找零:" + UserChange[7]);
                    UserChange[7] = 0;
                }
              

                return;
            }
            else
            {
                
                prompt.Text = " 处理完毕，1秒后自动跳转 ";
               
                dTimer.IsEnabled = false;
                Payout.Reset(textBox1);
                Payout.SSPComms.CloseComPort();
                Cm.CloseGetCoinChannelALL();
                Cm.PayoutCoinReset();
                Cm.CloseSerialPort();

                Thread.Sleep(1000);
                new VIEW.PaySuccess(ParkPrice * 100, CarNum, recordId, InTime).Show();
                this.Close();
            }


            PayOutChange = false;
           // button1.IsEnabled = true;

        }


        void CheckCashEnoughPay(bool isCoin)
        {
            string str = "";
            
            if (GetCash >= ParkPrice)
                str = ",已足够支付";
            if(isCoin)
                prompt.Text = "收到：1.00元硬币，当前总金额：" + GetCash.ToString("0.00") +"元" + str;
            else
                prompt.Text = "收到：" + CPayout.price + "元纸币，当前总金额：" + GetCash.ToString("0.00") + "元" + str;

        }

        void HandleSmartPayout()
        {
                  
            if (CPayout.price != "")
            {
                string[] s = CPayout.price.Split('.');
                
                int cash = Int32.Parse(s[0]);
                switch (cash)
                {
                    case 1: UserInCash[0] += 1; break;
                    case 5: UserInCash[2] += 1; break;
                    case 10: UserInCash[3] += 1; break;
                    case 20: UserInCash[4] += 1; break;
                    case 50: UserInCash[5] += 1; break;
                    case 100: UserInCash[6] += 1; break;
                }
                GetCash += cash;

                CheckCashEnoughPay(false);
            }


            if (CPayout.flag == "Dispensed")
            {
                //退款 （检测是否还有需要退）
                if (GiveBack)
                {
                    CashGiveBack();
                }
                // 找零 （检测是否还有需要找零）
                if (PayOutChange)
                {
                    ChangePayout();
                }
            }
            
        }

        private bool GetInitData(string SmartPayoutCom, string HopperCom)     //初始化各种状态，获取必要数据
        {
            bool SmartPayoutStatu = false;
            bool HopperStatu = false;

            int reconnectionAttempts = 5;
            Payout.CommandStructure.ComPort = SmartPayoutCom;
            Payout.CommandStructure.SSPAddress = 0;
            Payout.CommandStructure.Timeout = 3000;


            // connect to validator
            if (ConnectToValidator(reconnectionAttempts))
            {
                SmartPayoutStatu = true;
                
            }
            Payout.SetChannelToPayout(textBox1);
            SetupFormLayout();

            //connect to hopper
            if (Cm.CheckCoinInStatus())
            {
                HopperStatu = true;
            }

            string isTest = ConfigurationManager.AppSettings["test"];            
            if(isTest.Equals("XinXing")) { 
                //检查硬币钱箱是否有足够钱找
                if (Cm.CheckPayoutCoinEnable() == 1) //一半钱
                {
                    IsCashEnable = false;
                }
                else if (Cm.CheckPayoutCoinEnable() == 0) //"快没钱";
                {
                    IsCashEnable = false;
                    Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                    cfa.AppSettings.Settings["CASHENABLE"].Value = "F";
                    cfa.Save();
                }
                else if (Cm.CheckPayoutCoinEnable() == -1) //"检测有误 重新检查"
                {
                    IsCashEnable = false;
                }
            }

            new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CashPayout-->纸币机初始化状态：" + SmartPayoutStatu + "。硬币机初始化状态：" + HopperStatu);
            if (SmartPayoutStatu && HopperStatu)  //均连接成功并且获取数据执行定时器任务
            {
                
                return true;
            }

            return false;
        }



        private bool CashPayInit() //初始化设备
        {

            string SmartPayoutCom = ConfigurationManager.AppSettings["SmartPayoutCom"];
            string HopperCom = ConfigurationManager.AppSettings["HopperCom"];

            Payout = new CPayout();
                Cm  = new CoinManager(HopperCom);
                dTimer.Tick += new EventHandler(dTimer_Tick);
                dTimer.Interval = new TimeSpan(200);
                dTimer.IsEnabled = false;


           

            if (GetInitData(SmartPayoutCom, HopperCom))
                return true;
            else
                return false;
              
      
        }

       

        private void CalculatePayout(string amount, char[] currency)
        {
            string[] s = amount.Split('.');
 
            int n = 0;
            try
            {
                n = Int32.Parse(s[0]) * 100; // Multiply by 100 for penny value
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "EXCEPTION");
                return;
            }
            // Make payout
            Payout.PayoutAmount(n, currency, textBox1);
        }

        private void SetupFormLayout()
        {
    
            if (Payout == null)
            {
                //MessageBox.Show("Validator class is null.", "ERROR");
                return;
            }
            for (int i = 1; i <= Payout.NumberOfChannels; i++)
            {
                ChannelData d = new ChannelData();
                Payout.GetDataByChannel(i, ref d);
                payoutcash[i - 1] = d.Level;
                // 1 ; 0 ; 5 ;10 ;20 ;50 ;100
            }
            new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---CashPayout-->钱箱存钱数量，1：" + payoutcash[0] + "张。5：" + payoutcash[2] + "张。10：" + payoutcash[3] + "张。");

        }


        private bool ConnectToValidator(int attempts)
        {

            for (int i = 0; i < attempts; i++)
            {

                Payout.SSPComms.CloseComPort();
                Payout.CommandStructure.EncryptionStatus = false;
                if (Payout.OpenComPort(textBox1) && Payout.NegotiateKeys(textBox1))
                {
                    Payout.CommandStructure.EncryptionStatus = true; // now encrypting
                    byte maxPVersion = FindMaxProtocolVersion();
                    if (maxPVersion >= 6)
                    {
                        Payout.SetProtocolVersion(maxPVersion, textBox1);
                    }
                    else
                    {
                        //MessageBox.Show("This program does not support slaves under protocol 6!", "ERROR");
                        return false;
                    }
                    Payout.SetupRequest(textBox1);
                    if ((Payout.UnitType) != (char)0x06)
                    {
                       // MessageBox.Show("Unsupported type shown by SMART Payout, this SDK supports the SMART Payout only");

                        return false;
                    }
                    Payout.SetInhibits(textBox1);
                    Payout.EnableValidator(textBox1);
                    Payout.EnablePayout(textBox1);
                    return true;
                }

            }
            return false;
        }


        private byte FindMaxProtocolVersion()
        {
            byte b = 0x06;
            while (true)
            {
                Payout.SetProtocolVersion(b);
                if (Payout.CommandStructure.ResponseData[0] == CCommands.SSP_RESPONSE_CMD_FAIL)
                    return --b;
                b++;
                if (b > 12)
                    return 0x06; 
            }
        }

 


        

    }
}
