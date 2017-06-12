using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PaySystem.VIEW
{
    /// <summary>
    /// loading.xaml 的交互逻辑
    /// 用户控件：旋转位图
    /// </summary>
    public partial class loading : UserControl
    {
        private Storyboard story;
        public loading()
        {
            InitializeComponent();
            this.story = (base.Resources["waiting"] as Storyboard);
            try
            {
                this.image.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/loading.png"));
            }
            catch { }
        }

        private void Image_Loaded_1(object sender, RoutedEventArgs e)
        {
            this.story.Begin(this.image, true);
        }
        public void Stop()
        {
            base.Dispatcher.BeginInvoke(new Action(() => {
                this.story.Pause(this.image);
                base.Visibility = System.Windows.Visibility.Collapsed;
            }));
        }

    }
}
