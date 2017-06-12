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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PaySystem.VIEW
{
    /// <summary>
    /// KeyBoard.xaml 的交互逻辑
    /// </summary>
    public partial class KeyBoard : Window
    {
        bool TheFirstClick = true;
        public KeyBoard()
        {
            InitializeComponent();

            //set background
            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "img/background.png"));
            this.Background = imageBrush;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button1_Copy11_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("B");
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("1");
        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("2");
        }

        private void button1_Copy1_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("3");
        }

        private void button1_Copy2_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("4");
        }

        private void button1_Copy3_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("5");
        }

        private void button1_Copy4_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("6");
        }

        private void button1_Copy5_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("7");
        }

        private void button1_Copy6_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("8");
        }

        private void button1_Copy7_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("9");
        }

        private void button1_Copy8_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("0");
        }

        private void button1_Copy10_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("A");
        }

        private void button1_Copy12_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("C");
        }

        private void button1_Copy13_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("D");
        }

        private void button1_Copy14_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("E");
        }

        private void button1_Copy15_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("F");
        }

        private void button1_Copy16_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("G");
        }

        private void button1_Copy17_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("H");
        }

        private void button1_Copy18_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("I");
        }

        private void button1_Copy19_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("J");
        }

        private void button1_Copy20_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("K");
        }

        private void button1_Copy21_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("L");
        }

        private void button1_Copy22_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("M");
        }

        private void button1_Copy23_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("N");
        }

        private void button1_Copy24_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("O");
        }

        private void button1_Copy25_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("P");
        }

        private void button1_Copy26_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("Q");
        }

        private void button1_Copy27_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("R");
        }

        private void button1_Copy28_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("S");
        }

        private void button1_Copy29_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("T");
        }

        private void button1_Copy30_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("U");
        }

        private void button1_Copy31_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("V");
        }

        private void button1_Copy32_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("W");
        }

        private void button1_Copy33_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("X");
        }

        private void button1_Copy34_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("Y");
        }

        private void button1_Copy35_Click(object sender, RoutedEventArgs e)
        {
            ButtonClick("Z");
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void button1_Copy9_Click(object sender, RoutedEventArgs e)
        {
            if(textBox.Text !="" && textBox.Text.Length <6 )
            textBox.Text = textBox.Text.Substring(0, textBox.Text.Length - 1);
        }

        private void button1_Copy36_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text != "" && TheFirstClick==false)
            {
              
                new VIEW.CheckCar3(this.textBox.Text).Show();
                
                this.Close();
            }
            else
                MessageBox.Show("请输入车牌！");

        }


        void ButtonClick(string Inchar)
        {
            if (TheFirstClick)
            {
                this.textBox.Text = "";
                TheFirstClick = false;
            }

            if (textBox.Text.Length < 5)
                this.textBox.Text += Inchar;
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            this.Close();
        }
    }
}

