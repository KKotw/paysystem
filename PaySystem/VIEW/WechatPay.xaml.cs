using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using ZXing;
using BarcodeWriter = ZXing.Presentation.BarcodeWriter;

namespace PaySystem.VIEW
{
    /// <summary>
    /// WeChatPay.xaml 的交互逻辑
    /// </summary>
    public partial class WeChatPay : Window
    {
        int ParkPrice = 0;
        //string CarNum = "";
        string buildId = "";
        string recordIds = "";
        string paymentRecordId = "";
        bool PaySuccess = false;

        int overtime = 0;
        DispatcherTimer dTimer = new System.Windows.Threading.DispatcherTimer();

        public WeChatPay(int Price, string CarNumber,string buildingId,string recordId)
        {
            InitializeComponent();

            //set background
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/background.png"));
            this.Background = imageBrush;

            
            dTimer.Tick += new EventHandler(dTimer_Tick);
            dTimer.Interval =  new TimeSpan(0, 0, 1);
            dTimer.IsEnabled = false;

            ParkPrice = Price;
            buildId = buildingId;
            recordIds = recordId;
            Task.Factory.StartNew(getQrcode);
         

           //ParkPrice = 1;  //强制测试使用
        }



        private void dTimer_Tick(object sender, EventArgs e)
        {
            if (PaySuccess)
            {
                overtime--;
                if (overtime == 0)
                {
                    dTimer.IsEnabled = false;
                    new MainWindow().Show();
                    this.Close();
                }
                label.Content = "支付成功,请在15分钟内离场。\n    系统将在" + overtime + "秒后返回主页";
               

            }
            else
            {
                overtime--;
                if (overtime == 0)
                {
                    dTimer.IsEnabled = false;
                    new MainWindow().Show();
                    this.Close();
                }
                label1.Content = "     "+overtime + "\n支付超时时间" ;



            //查询支付订单
            try
            {
                String json = HTTP.HTTP_GET_POST.HttpPostMath("http://www.lightcar.cn/ipms/third/queryPayOrder.action", "id=" + paymentRecordId);
                HTTP.jsonClass3 jsClass3 = HTTP.JSON_Deserialize.JsonDeserialize<HTTP.jsonClass3>(json);
                if (jsClass3.data[0].paymentStatus == "1")
                {

                    this.image.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/wechatpaysuccess.png"));
                    label.Content = "支付成功,请在15分钟内离场。";
                    label1.Content = "";
                    PaySuccess = true;
                    overtime = 30;
                    // dTimer.IsEnabled = false;
                }
               
            }
            catch {
                label.Content = "网络连接异常";
            }

            }
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }


        /// <summary>
        /// PayUrl  to  QR code 400*400
        /// </summary>
        /// <param name="paylink"></param>
        void StrToQRCode(string paylink)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Height = 400,
                    Width = 400,
                    Margin = 0
                }
            };
            var image = writer.Write(paylink);

            this.image.Source = image;
            this.label.Content = "扫一扫 微信支付";
        }



        void getQrcode()
        {


            string host = ConfigurationManager.AppSettings["localurl"];
            string host2 = ConfigurationManager.AppSettings["serverurl"];

            string json = "";          
            string payurl = " Error !";
            HTTP.JsonClass2 jsClass;

            

            try
            {
                json = HTTP.HTTP_GET_POST.HttpGetMath(host + "/ipc2.0/api/savePaymentReocrd.action", "recordId=" + recordIds + "&fee=" + ParkPrice + "&paymentMethod=3&paymentStatus=0");
                jsClass = HTTP.JSON_Deserialize.JsonDeserialize<HTTP.JsonClass2>(json);
                paymentRecordId = jsClass.retInfo;

                if (paymentRecordId != "")
                {
                    json = HTTP.HTTP_GET_POST.HttpGetMath(host2 + "/ipms/api/weixinPayT.action", "parkingRecordId=" + recordIds + "&paymentRecordId=" + paymentRecordId + "&channelId=" + buildId);         
                    jsClass = HTTP.JSON_Deserialize.JsonDeserialize<HTTP.JsonClass2>(json);

                    if(jsClass.ret ==  "0" )
                    { 
                    payurl = jsClass.retInfo;
                    dTimer.IsEnabled = true;
                    overtime = 120;
                    }
                }

            }
            catch
            {
                
                MessageBox.Show("Error！");
            }


            this.Dispatcher.BeginInvoke((Action)delegate ()
            {
                StrToQRCode(payurl);
            });

           
        }

        private void Grid_Initialized(object sender, EventArgs e)
        {

        }
    }
}
