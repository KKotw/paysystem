using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PaySystem
{
    class MainApp
    {

        [STAThread]
        public static void Main(string[] args)
        {
            if (args.Length > 1 && args[0] == "DongleUserId=19521" && args[1] == "Key=1587")
            {

                App app = new App();
                app.InitializeComponent();
                app.Run();
            }
            else if (args.Length > 1 && args[0] == "while(true)" && args[1] == "AdminManager;")
            {
                App app = new App();
                app.InitializeComponent();
                app.StartupUri = new Uri("VIEW/AdminManager.xaml", UriKind.RelativeOrAbsolute);
                app.Run();
            }
            else
            {
                
                App app = new App();
                app.InitializeComponent();
                //app.StartupUri = new Uri("VIEW/AdminManager.xaml", UriKind.RelativeOrAbsolute);
                app.Run();
                

               // MessageBox.Show("没有检测到加密狗，请插好加密狗并重新运行。");
               // return;
            }
        }

    }
}
