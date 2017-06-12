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

namespace PaySystem.VIEW
{
    /// <summary>
    /// CheckCar3.xaml 的交互逻辑
    /// </summary>
    public partial class CheckCar3 : Window
    {
        private readonly int CashPay = 1;
        private readonly int WeChatPay = 2;

        HTTP.JsonClass jsClass;
        string CarNumber = "";
        //int page = 0;
        int count = 0;
        static string host = ConfigurationManager.AppSettings["localurl"];

        public CheckCar3(string CarNum)
        {
            InitializeComponent();

            this.CarNumber = CarNum;
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/background.png"));
            this.Background = imageBrush;

            image0.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/nocarimg.png"));

            image1.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/nocarimg.png"));
            image2.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/nocarimg.png"));
            image3.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/nocarimg.png"));
            image4.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/nocarimg.png"));
            image5.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/nocarimg.png"));
            image6.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/nocarimg.png"));

            // right.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/right.png"));
            // left.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/left.png"));
            GetDataFromIpc();

        }

        private void GetDataFromIpc()
        {
          
            try
            {
                string json = "";
                
                json = HTTP.HTTP_GET_POST.HttpGetMath(host + "/ipc2.0/ubrecord_getListByNum", "carNum=" + CarNumber + "&status=0");      
                jsClass = HTTP.JSON_Deserialize.JsonDeserialize<HTTP.JsonClass>(json);

               

                if (jsClass.total > 0)
                {
                    
                    if (jsClass.total == 1)
                    {
                        
                        //处理只有一辆车的
                        ShowData(0,image0,label0,label01,label02,label03);
                        count = 1;
                    }
                    else if (jsClass.total > 1)
                    {
                        //显示6个的
                        this.SixView.Visibility = Visibility.Visible;
                        this.OneView.Visibility = Visibility.Hidden;

                        if (jsClass.total > 6)
                        {
                            //any page !?
                           // this.right.Visibility = Visibility.Visible;
                           // this.left.Visibility = Visibility.Visible;
                            count = 6;
                        }
                        else
                            count = jsClass.total;
                        //处理多辆车的
                        for (int i = 0; i < count; i++)
                        {                          
                            ShowData(i, (Image)this.FindName("image" + (i+1) ), (Label)this.FindName("label" + (i+1) +"1"), (Label)this.FindName("label" + (i + 1) + "2"), (Label)this.FindName("label" + (i + 1) + "3"), (Label)this.FindName("label" + (i + 1) + "4"));                    
                        }                       
                    }


                }
                else
                {
                    label0.Content = "检索不到车辆，请检测车牌!";
                    label01.Content = "";
                    label02.Content = "";
                    label03.Content = "";

                    new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "--- err,检索不到车辆，请检测车牌!");
                }

            }
            catch
            {
                label0.Content = "网络异常!!";
                label01.Content = "";
                label02.Content = "";
                label03.Content = "";
                new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---err ,网络连接异常或者返回json格式有误!");
            }

        }




        private void ShowData(int slectindex,Image showCarImage,Label showCarNum,Label showCarfee,Label showCarCost,Label showCarInTime)
        {

            new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---success,查询到匹配车牌 :" + jsClass.rows[slectindex].carNum + "。相应建筑ID：" + jsClass.rows[slectindex].buildingId + "。应付价格：" + jsClass.rows[slectindex].deptFee + "。已付金额：" + jsClass.rows[slectindex].payedFee + "。进场时间：" + jsClass.rows[slectindex].inTimeStr);
            try
            {
                showCarNum.Content = "车牌: " + jsClass.rows[slectindex].carNum;
                showCarfee.Content = "应付金额：                   元";
                showCarCost.Content = (Convert.ToDouble(jsClass.rows[slectindex].deptFee) / 100).ToString("0.00");
                showCarInTime.Content = "入场时间: " + ConvertStringToDateTime(jsClass.rows[slectindex].inTimeStr).ToString();

            }
            catch
            {
                new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---err ,获取车辆信息出错!");
                showCarNum.Content = "获取车辆信息出错!!";
                showCarfee.Content = "";
                showCarCost.Content = "";
                showCarInTime.Content = "";
            }


            try
            {
                string imgurl = jsClass.rows[slectindex].enterSnapPath;
                if(imgurl.Length > 10)
                imgurl = host + imgurl;
                //显示网络图片
                showCarImage.Source = new BitmapImage(new Uri(imgurl));
            }
            catch
            {
                new LOG.LogClass().WriteLogFile(LOG.LogSerial.logSerialnum + "---err ,无图片地址或者格式有误!");
                //本地图片载入方式
                showCarImage.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/nocarimg.png"));

            }


        }

        private DateTime ConvertStringToDateTime(string timeStamp)
        {
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dtStart.Add(toNow);
        }

        private int GetSlectIndex()
        {
            if (count > 1)
            {
                if (SixView.SelectedIndex < count)
                    return SixView.SelectedIndex;
                else
                    return -1;
            }
            else if (count == 1)
                return 0;
            else
                return -1;

        }

        private void SendDataToCashPayOrWeChetPay(int PayManner)
        {

            int slectindex = GetSlectIndex();
            if (slectindex < 0)
            {
                MessageBox.Show("所选车辆无信息，请重新选择！");
                return;
            }
                //get data
                string buildingId = jsClass.rows[slectindex].buildingId.ToString();
                string recordId = jsClass.rows[slectindex].id;
                string CarNum = jsClass.rows[slectindex].carNum;
                string Intime = ConvertStringToDateTime(jsClass.rows[slectindex].inTimeStr).ToString();
                int deptFee = Convert.ToInt32(jsClass.rows[slectindex].deptFee);
         


            if (PayManner.Equals(CashPay))
            {
   
                    new VIEW.CashPay1(deptFee, CarNum, recordId, Intime).Show(); //传价格与车牌
                    this.Close();
               

            }
            else if (PayManner.Equals(WeChatPay))
            {
                new VIEW.WeChatPay(deptFee, CarNum, buildingId, recordId).Show(); //传价格与车牌
                this.Close();
            }
            else
            {
                //未知支付方式？
            }
        }
  

        private void button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            SendDataToCashPayOrWeChetPay(CashPay);
        }

        private void right_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //page++; 
        }

        private void left_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //page--;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            SendDataToCashPayOrWeChetPay(WeChatPay);
        }
    }
}
