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
    /// PaySuccess.xaml 的交互逻辑
    /// </summary>
    public partial class PaySuccess : Window
    {
        int Fee = 0;
        string CarNum = "";
        string Id = "";
        string InTime = "";
        int overtime = 0;

        DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();

        public PaySuccess(int fee,string carnum,string recordId,string intime)
        {
            InitializeComponent();

            //set background
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/background.png"));
            this.Background = imageBrush;

            this.Fee = fee;
            this.CarNum = carnum;
            this.Id = recordId;
            this.InTime = intime;
            string host = ConfigurationManager.AppSettings["localurl"];
            string json = "";
            string paymentStatus="1", paymentMethod="1";
            HTTP.JsonClass2 jsClass;

            dTimer.Tick += new EventHandler(dTimer_Tick);
            dTimer.Interval = new TimeSpan(0, 0, 1);
            dTimer.IsEnabled = false;

            try
            {
                json = HTTP.HTTP_GET_POST.HttpGetMath(host + "/ipc2.0/api/savePaymentReocrd.action", "recordId=" + recordId + "&fee=" + fee + "&paymentMethod=" + paymentMethod + "&paymentStatus=" + paymentStatus);

                jsClass = HTTP.JSON_Deserialize.JsonDeserialize<HTTP.JsonClass2>(json);
                new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---PaySuccess-->retInfo:" + jsClass.retInfo);

                if (jsClass.ret == "0")
                {
                    dTimer.IsEnabled = true;
                    overtime = 30;
                    label.Content = "支付成功，请在15分钟内离场\n 系统将在" + overtime + "秒后返回主页。";
                }
                    
                else
                    label.Content = ConfigurationManager.AppSettings["warn"];
            }
            catch {
                label.Content = ConfigurationManager.AppSettings["warn"];
            }


        }

        private void dTimer_Tick(object sender, EventArgs e)
        {
          
            
            overtime--;
            if (overtime == 0)
            {
                dTimer.IsEnabled = false;
                new MainWindow().Show();
                this.Close();
            }
            label.Content = "支付成功，请在15分钟内离场\n   系统将在" + overtime + "秒后返回主页";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            new Print.PrintSmallTicket(Id, CarNum, Fee/100, InTime);
            button.IsEnabled = false;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}
