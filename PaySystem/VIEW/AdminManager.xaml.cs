using PaySystem.DLL.Classes;
using PaySystem.DLL.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
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
    /// AdminManager.xaml 的交互逻辑
    /// </summary>
    public partial class AdminManager : Window
    {
        int[] payoutcash = new int[] { 0, 0, 0, 0, 0, 0, 0 }; // 1 ; 0 ; 5 ;10 ;20 ;50 ;100

        DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();
        CPayout Payout;
        TextBox LOGS = new TextBox();

        public AdminManager()
        {
            InitializeComponent();

            //禁用取钱功能
            button2.IsEnabled = false;

            string SmartPayoutCom = ConfigurationManager.AppSettings["SmartPayoutCom"];

            Payout = new CPayout();
            dTimer.Tick += new EventHandler(dTimer_Tick);
            dTimer.Interval = new TimeSpan(200);
            dTimer.IsEnabled = false;


            //init
            int reconnectionAttempts = 5;
            Payout.CommandStructure.ComPort = SmartPayoutCom;
            Payout.CommandStructure.SSPAddress = 0;
            Payout.CommandStructure.Timeout = 3000;


            // connect to validator
            if (ConnectToValidator(reconnectionAttempts))
            {
                dTimer.IsEnabled =  true;
            }

            Payout.AdminSetChannelToPayout(LOGS);
            CheckPayoutCash();
            ShowCashBoxCount();
        }

        private void dTimer_Tick(object sender, EventArgs e)
        {
            CPayout.price = "";
            if (Payout.DoPoll(LOGS) == false)
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
                if (CPayout.price != "")
                {
                    string[] s = CPayout.price.Split('.');
                    int cash = Int32.Parse(s[0]);
                    switch (cash)
                    {
                        case 1: payoutcash[0] += 1; break;
                        case 5: payoutcash[2] += 1; break;
                        case 10: payoutcash[3] += 1; break;
                        case 20: payoutcash[4] += 1; break;
                        case 50: payoutcash[5] += 1; break;
                        case 100: payoutcash[6] += 1; break;
                    }
                    if (payoutcash[0] > 5 || payoutcash[2] > 30 || payoutcash[3] > 30)
                        Payout.AdminSetChannelToPayout(LOGS);
                    ShowCashBoxCount();

                }
            }
            
        }

 


        private void CheckPayoutCash()
        {
           
            if (Payout == null)
            {
                return;
            }
            for (int i = 1; i <= Payout.NumberOfChannels; i++)
            {
                ChannelData d = new ChannelData();
                Payout.GetDataByChannel(i, ref d);
                payoutcash[i - 1] = d.Level;

            }
       
        }

        private  void ShowCashBoxCount()
        {
            textBox.Text = "1.00 元拥有 " + payoutcash[0] + " 张(如投币将进入不可找钱箱)\n" +
                           "5.00 元拥有 " + payoutcash[2] + " 张\n" +
                           "10.00 元拥有 " + payoutcash[3] + " 张\n" +
                           "20.00 元拥有 " + payoutcash[4] + " 张 (如投币将进入不可找钱箱) \n" +
                           "50.00 元拥有 " + payoutcash[5] + " 张 (如投币将进入不可找钱箱) \n" +
                           "100.00 元拥有 " + payoutcash[6] + " 张 (如投币将进入不可找钱箱) ";
                
        }
        private bool ConnectToValidator(int attempts)
        {

            for (int i = 0; i < attempts; i++)
            {

                Payout.SSPComms.CloseComPort();
                Payout.CommandStructure.EncryptionStatus = false;
                if (Payout.OpenComPort(LOGS) && Payout.NegotiateKeys(LOGS))
                {
                    Payout.CommandStructure.EncryptionStatus = true; // now encrypting
                    byte maxPVersion = FindMaxProtocolVersion();
                    if (maxPVersion >= 6)
                    {
                        Payout.SetProtocolVersion(maxPVersion, LOGS);
                    }
                    else
                    {
                        //MessageBox.Show("This program does not support slaves under protocol 6!", "ERROR");
                        return false;
                    }
                    Payout.SetupRequest(LOGS);
                    if ((Payout.UnitType) != (char)0x06)
                    {
                        // MessageBox.Show("Unsupported type shown by SMART Payout, this SDK supports the SMART Payout only");

                        return false;
                    }
                    Payout.SetInhibits(LOGS);
                    Payout.EnableValidator(LOGS);
                    Payout.EnablePayout(LOGS);
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
            Payout.PayoutAmount(n, currency, LOGS);
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //payout to cashbox
            Payout.EmptyPayoutDevice(LOGS);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
 

           
            if (radioButton0.IsChecked == true)
                CalculatePayout("5.00".ToString(), ("CNY").ToCharArray());

            if (radioButton1.IsChecked == true)
                CalculatePayout("10.00".ToString(), ("CNY").ToCharArray());

            if (radioButton2.IsChecked == true)
                CalculatePayout("20.00".ToString(), ("CNY").ToCharArray());

            if (radioButton3.IsChecked == true)
                CalculatePayout("50.00".ToString(), ("CNY").ToCharArray());

            if (radioButton4.IsChecked == true)
                CalculatePayout("100.00".ToString(), ("CNY").ToCharArray());


        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            Configuration cfa = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            cfa.AppSettings.Settings["CASHENABLE"].Value = "T";
            cfa.Save();


            dTimer.IsEnabled = false;
            Payout.Reset(LOGS);
            Payout.SSPComms.CloseComPort();

            this.Close();
        }
    }
}
