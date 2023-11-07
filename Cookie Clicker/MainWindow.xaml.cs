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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Cookie_Clicker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double cookies = 0;
        private double cookiesPerSecond = 0;
        

        DispatcherTimer cookieTimer = new DispatcherTimer();
        DispatcherTimer gameTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            cookieTimer.Interval = TimeSpan.FromSeconds(1);
            cookieTimer.Tick += CookieTimer_Tick;
            cookieTimer.Start();

            gameTimer.Interval = TimeSpan.FromMilliseconds(50);
            gameTimer.Tick += gameTimer_tick;
            gameTimer.Start();

        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            cookies++;
            LblCookie.Content = cookies + " Cookies";
        }
        private void gameTimer_tick(object sender, EventArgs e)
        {
            LblCookie.Content = cookies + " Cookies";
            LblCookiePerSecond.Content = cookiesPerSecond + "s";
            ClickerVerify();


        }
        private void CookieTimer_Tick(object sender, EventArgs e)
        {
            cookies = cookies + cookiesPerSecond;
            

            

        }
        private void ClickerVerify()
        {
            if (cookies < 15)
            {
                ClickerP.IsEnabled = false;
            }
            else
            {
                ClickerP.IsEnabled = true;
                ClickerP.Background = new SolidColorBrush(Colors.AliceBlue);

            }
        }
        private void ClickerP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClickerVerify();
            cookiesPerSecond = cookiesPerSecond + 0.2;
            cookies = cookies - 15;
        }
    }
}
