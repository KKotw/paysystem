using System;

using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PaySystem
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int outNum = 0;
        public MainWindow()
        {
            InitializeComponent();
            outNum = 0;
            new LOG.LogSerial().exchangeLogSerialNum();

            //set background
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource  = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/background.png"));
            this.Background = imageBrush;

            //广告展示
            this.image.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/01.png"));
        }


  

        private void button_Click(object sender, RoutedEventArgs e)
        {
            outNum += 1;
            closesoft();

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            
             new VIEW.KeyBoard().Show();

            this.Close();

        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            outNum += 2;
            closesoft();

        }

        private void button_Copy2_Click(object sender, RoutedEventArgs e)
        {
            outNum += 4;
            closesoft();

        }

        private void button_Copy1_Click(object sender, RoutedEventArgs e)
        {
            outNum += 8;
            closesoft();
        }

        void closesoft()
        {
            if (outNum == 15)
            {
                new LOG.LogClass().WriteLogFile("程序关闭:" + DateTime.Now.Date);
                this.Close();
            }
                if (outNum > 15) outNum = 0;
        }
    }
}
