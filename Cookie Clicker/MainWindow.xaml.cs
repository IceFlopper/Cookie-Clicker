﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
        double clickerProduction = 0.1;
        //grandma
        double grandmaCost = 100;
        int grandmaCount = 0;
        double grandmaProduction = 1;
        //farm
        double farmCost = 1100;
        int farmCount = 0;
        double farmProduction = 8;
        //mine
        double mineCost = 9000;
        int mineCount = 0;
        double mineProduction = 30;

        //upgrades
        int Upgrade1Level = 2;
        int cookieCostUpgrade1 = 100;
        int Upgrade2Level = 2;
        int cookieCostUpgrade2 = 150;


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
            LblMineProd.Content = mineProduction + "/s";
            LblUpgrade1.Content = "x" + Upgrade1Level + "Cursor";
            LblUpgrade2.Content = "x" + Upgrade2Level + "Clicker";




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
            LblCostMine.Content = "Cost: " + mineCost;





            ClickerVerify();
            GrandmaVerify();
            FarmVerify();
            MineVerify();
            UpgradeUnlock();


        }
        private void DrawCookies()
        {
            //update cookielabel to concatinate to smaller digits
            double cookiesCount = (int)cookies;
            string cookiesLabel = cookiesCount.ToString();
            if (cookiesCount > 9999 && cookiesCount < 999999)
            {
                cookiesLabel = $"{(cookiesCount / 1000.0):F1}K";
            }
            else if (cookiesCount > 999999 && cookiesCount < 999999999)
            {
                cookiesLabel = $"{(cookiesCount / 1000000.0):F1}M";
            }
            else if (cookiesCount > 999999999 && cookiesCount < 999999999999)
            {
                cookiesLabel = $"{(cookiesCount / 1000000000.0):F1}B";
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

            //verify if cookie count is high enough to purchase Farm
            if (cookies < farmCost)
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
        private void MineVerify()
        {

            //verify if cookie count is high enough to purchase Farm
            if (cookies < mineCost)
            {
                MineP.IsEnabled = false;
                MineP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else
            {
                MineP.IsEnabled = true;
                MineP.Background = new SolidColorBrush(Colors.AliceBlue);

            }
        }


        private void ClickerP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy clicker
            ClickerVerify();
            //add 1 clicker to clicker count when executed
            clickerCount++;
            LblClicker.Content = "Clicker" + "s: " + clickerCount;

            cookies = cookies - clickerCost;
            clickerCost = clickerCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + clickerProduction;

            clickerProduction = clickerProduction * 1.10;
            clickerCost = Math.Round(clickerCost);
            double clickerProductionRounded = Math.Round(clickerProduction, 2);
            LblClickerProd.Content = clickerProductionRounded + "/s";
        }

        private void GrandmaP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy grandma
            GrandmaVerify();
            //add 1 grandma to grandma count when executed
            grandmaCount++;
            LblGrandma.Content = "Grandma" + "s: " + grandmaCount;

            cookies = cookies - grandmaCost;
            grandmaCost = grandmaCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + grandmaProduction;

            grandmaProduction = grandmaProduction * 1.10;
            grandmaCost = Math.Round(grandmaCost);
            double grandmaProudctionRounded = Math.Round(grandmaProduction, 2);
            LblGrandmaProd.Content = grandmaProudctionRounded + "/s";
        }
        private void FarmP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy farm
            FarmVerify();
            //add 1 farm to grandma count when executed
            farmCount++;
            LblFarm.Content = "Farm" + "s: " + farmCount;

            cookies = cookies - farmCost;
            farmCost = farmCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + farmProduction;

            farmProduction = farmProduction * 1.10;
            farmCost = Math.Round(farmCost);
            double farmProudctionRounded = Math.Round(farmProduction, 2);
            LblFarmProd.Content = farmProudctionRounded + "/s";
        }
        private void MineP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy mine
            MineVerify();
            //add 1 mine to mine count when executed
            mineCount++;
            LblMine.Content = "Mine" + "s: " + mineCount;

            cookies = cookies - mineCost;
            mineCost = mineCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + mineProduction;

            mineProduction = mineProduction * 1.10;
            mineCost = Math.Round(mineCost);
            double mineProductionRounded = Math.Round(mineProduction, 2);
            LblMineProd.Content = mineProductionRounded + "/s";
        }
        private void UpgradeUnlock()
        {
            if (cookies >= cookieCostUpgrade1)
            {
                Upgrade1.IsEnabled = true;
                Upgrade1.Background = new SolidColorBrush(Colors.SandyBrown);
            }
            else
            {
                Upgrade1.IsEnabled = false;
                Upgrade1.Background = new SolidColorBrush(Colors.SaddleBrown);

            }

            if (cookies >= cookieCostUpgrade2)
            {
                Upgrade2.IsEnabled = true;
                Upgrade2.Background = new SolidColorBrush(Colors.SandyBrown);
            }
            else
            {
                Upgrade2.IsEnabled = false;
                Upgrade2.Background = new SolidColorBrush(Colors.SaddleBrown);

            }

        }
        private void Upgrade1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickCount = clickCount * Upgrade1Level;
            cookies = cookies - cookieCostUpgrade1;
            cookieCostUpgrade1 = cookieCostUpgrade1 * 3;
        }

        private void Upgrade2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickerProduction = clickerProduction * Upgrade2Level;
            cookies = cookies - cookieCostUpgrade2;
            LblClickerProd.Content = clickerProduction + "/s";
            Upgrade2.Visibility = Visibility.Collapsed;
            double clickerProductionRounded = Math.Round(clickerProduction, 2);
            LblClickerProd.Content = clickerProductionRounded + "/s";
        }

        private void Upgrade1_MouseEnter(object sender, MouseEventArgs e)
        {


            
        }

        private void Upgrade2_MouseEnter(object sender, MouseEventArgs e)
        {


        }

        private void Upgrade1_MouseLeave(object sender, MouseEventArgs e)
        {

        }

        private void Upgrade2_MouseLeave(object sender, MouseEventArgs e)
        {

        }
    }
}
