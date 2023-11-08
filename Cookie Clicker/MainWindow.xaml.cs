using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
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
        double clickerCost = 15;
        double clickCount = 500;


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




            new BitmapImage(new Uri("..Cookie Clicker/cookie.png", UriKind.RelativeOrAbsolute));
            CookieImage.RenderTransformOrigin = new Point(0.5, 0.5);
            RotateTransform rotateTransform = new RotateTransform();
            CookieImage.RenderTransform = rotateTransform;

            DoubleAnimation rotationAnimation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                Duration = TimeSpan.FromSeconds(40),
                RepeatBehavior = RepeatBehavior.Forever
                
            };
            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //when left click on cookie image add cookie clickCount to cookies
            cookies = cookies + clickCount;
            LblCookie.Content = (int)cookies + " Cookies";
            DrawCookies();
        }
        private void gameTimer_tick(object sender, EventArgs e)
        {
            //update labels every 50ms
            DrawCookies();
            LblCookiePerSecond.Content = cookiesPerSecond + "s";
            LblCostClicker.Content = "Cost: " + clickerCost;

  
            ClickerVerify();

        }
        private void DrawCookies()
        {
            //update cookielabel to concatinate to smaller digits
            int cookiesCount = (int)cookies;
            string cookiesLabel = cookiesCount.ToString();
            if (cookiesCount > 9999 && cookiesCount < 100000)
            {
                cookiesLabel = cookiesCount.ToString().Substring(0, cookiesLabel.Length - 3) + "K";
            }
            else if (cookiesCount > 100000 && cookiesCount < 1000000)
            {
                cookiesLabel = cookiesCount.ToString().Substring(0, cookiesLabel.Length - 3) + "K";
            }


            LblCookie.Content = cookiesLabel + " Cookies";

        }
        private void CookieTimer_Tick(object sender, EventArgs e)
        {
            //add cookies every second
            cookies = cookies + cookiesPerSecond;
        }
        private void ClickerVerify()
        {

            //verifies if u have atleast 15 cookies
            if (cookies < clickerCost)
            {
                ClickerP.IsEnabled = false;
                ClickerP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else
            {
                ClickerP.IsEnabled = true;
                ClickerP.Background = new SolidColorBrush(Colors.AliceBlue);

            }
        }
        private void ClickerP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //verify if cookies is high enough to purchase
            ClickerVerify();
            cookies = cookies - clickerCost;
            clickerCost = clickerCost * 1.10;
            clickerCost = Math.Round(clickerCost);
            cookiesPerSecond = cookiesPerSecond + 0.5;
        }




    }
}
