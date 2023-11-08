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
        //amount gained per click
        double clickCount = 1;

        //clicker
        double clickerCost = 15;
        int clickerCount = 0;
        double clickerProduction = 0.5;
        //grandma
        double grandmaCost = 150;
        int grandmaCount = 0;
        double grandmaProduction = 5;
        //farm
        double farmCost = 3000;
        int farmCount = 0;
        double farmProduction = 25;


        DispatcherTimer cookieTimer = new DispatcherTimer();
        DispatcherTimer gameTimer = new DispatcherTimer();
        public MainWindow()
        {
            InitializeComponent();

            cookieTimer.Interval = TimeSpan.FromSeconds(1);
            cookieTimer.Tick += CookieTimer_Tick;
            cookieTimer.Start();

            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Tick += gameTimer_tick;
            gameTimer.Start();

            //content for clickerproduction
            LblClickerProd.Content = clickerProduction + "/s";
            LblGrandmaProd.Content = grandmaProduction + "/s";
            LblFarmProd.Content = farmProduction + "/s";




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
            //update labels every 10ms
            DrawCookies();
            cookiesPerSecond = Math.Round(cookiesPerSecond, 1);
            LblCookiePerSecond.Content = cookiesPerSecond+ "/s";
            LblCostClicker.Content = "Cost: " + clickerCost;
            LblCostGrandma.Content = "Cost: " + grandmaCost;
            LblCostFarm.Content = "Cost: " + farmCost;




            ClickerVerify();
            GrandmaVerify();
            FarmVerify();


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

            //verify if cookie count is high enough to purchase Clicker
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
        private void GrandmaVerify()
        {

            //verify if cookie count is high enough to purchase Grandma
            if (cookies < grandmaCost)
            {
                GrandmaP.IsEnabled = false;
                GrandmaP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else
            {
                GrandmaP.IsEnabled = true;
                GrandmaP.Background = new SolidColorBrush(Colors.AliceBlue);

            }
        }
        private void FarmVerify()
        {

            //verify if cookie count is high enough to purchase Grandma
            if (cookies < grandmaCost)
            {
                FarmP.IsEnabled = false;
                FarmP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else
            {
                FarmP.IsEnabled = true;
                FarmP.Background = new SolidColorBrush(Colors.AliceBlue);

            }
        }
        private void ClickerP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy clicker
            ClickerVerify();
            clickerCount++;
            LblClicker.Content = "Clicker" + "s: " + clickerCount;
            cookies = cookies - clickerCost;
            clickerCost = clickerCost * 1.25;
            clickerProduction = clickerProduction * 1.10;
            clickerCost = Math.Round(clickerCost);
            cookiesPerSecond = cookiesPerSecond + clickerProduction;
            double clickerProductionRounded = Math.Round(clickerProduction, 2);
            LblClickerProd.Content = clickerProductionRounded + "/s";
        }

        private void GrandmaP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy grandma
            GrandmaVerify();
            grandmaCount++;
            LblGrandma.Content = "Grandma" + "s: " + grandmaCount;
            cookies = cookies - grandmaCost;
            grandmaCost = grandmaCost * 1.25;
            grandmaProduction = grandmaProduction * 1.10;
            grandmaCost = Math.Round(grandmaCost);
            cookiesPerSecond = cookiesPerSecond + grandmaProduction;
            double grandmaProudctionRounded = Math.Round(grandmaProduction, 2);
            LblGrandmaProd.Content = grandmaProudctionRounded + "/s";
        }
        private void FarmP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy farm
            FarmVerify();
            farmCount++;
            LblFarm.Content = "Farms: " + farmCount;
            cookies = cookies - farmCost;
            farmCost = farmCost * 1.25;
            farmProduction = farmProduction * 1.10;
            farmCost = Math.Round(farmCost);
            cookiesPerSecond = cookiesPerSecond + farmProduction;
            double farmProductionRounded = Math.Round(farmProduction, 2);
            LblFarmProd.Content = farmProductionRounded + "/s";
        }
    }
}
