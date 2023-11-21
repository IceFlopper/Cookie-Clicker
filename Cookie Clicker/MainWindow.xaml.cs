using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Media;
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
using Microsoft.VisualBasic;

namespace Cookie_Clicker
{

    public partial class MainWindow : Window
    {

        private double cookies = 0;
        private double cookiesPerSecond = 0;
        //amount gained per click
        double clickCount = 10000;

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
        //factory
        double factoryCost = 110000;
        int factoryCount = 0;
        double factoryProduction = 200;
        //bank
        double bankCost = 1400000;
        int bankCount = 0;
        double bankProduction = 1000;
        //temple
        double templeCost = 20000000;
        int templeCount = 0;
        double templeProduction = 5000;

        //upgrades
        //cursorupgrade
        int upgradeCursorLevel = 2;
        double cookieCostUpgradeCursor = 100;
        //Clickerupgrade
        int upgradeClickerCount = 0;
        int upgradeClickerLevel = 3;
        double cookieCostUpgradeClicker = 250;
        //grandmaupgrade
        int upgradeGrandmaCount = 0;
        int upgradeGrandmaLevel = 2;
        double cookieCostUpgradeGrandma = 750;
        //farmUpgrade
        int upgradeFarmCount = 0;
        int upgradeFarmLevel = 2;
        double cookieCostUpgradeFarm = 5000;
        //mineupgrade
        int upgradeMineCount = 0;
        int upgradeMineLevel = 2;
        double cookieCostUpgradeMine = 30000;
        //clicker2upgrade
        int upgradeClicker2Count = 0;
        int upgradeClicker2Level = 3;
        double cookieCostUpgradeClicker2 = 1250;
        //factoryupgrade
        int upgradeFactoryCount = 0;
        int upgradeFactoryLevel = 2;
        double cookieCostUpgradeFactory = 200000;

        MediaPlayer soundClick = new MediaPlayer();
        MediaPlayer soundBuy = new MediaPlayer();


        DispatcherTimer gameTimer = new DispatcherTimer();

        //height for wrap panels
        int wrapHeight = 50;
        public MainWindow()
        {
            InitializeComponent();
            

            Thread cookieThread = new Thread(CookieLogic);
            cookieThread.Start();


            soundBuy.Volume = 0.4;


            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Tick += gameTimer_tick;
            gameTimer.Start();

            //content for clickerproduction
            LblClickerProd.Content = clickerProduction + "/s";
            LblGrandmaProd.Content = grandmaProduction + "/s";
            LblFarmProd.Content = farmProduction + "/s";
            LblMineProd.Content = mineProduction + "/s";
            LblFactoryProd.Content = factoryProduction + "/s";
            LblBankProd.Content = bankProduction + "/s";
            LblTempleProd.Content = templeProduction + "/s";

            //content for upgrade 
            LblUpgrade1.Content = upgradeCursorLevel + "x" + " Cursor";
            LblUpgrade2.Content = upgradeClickerLevel + "x" + " Clicker";
            LblUpgrade3.Content = upgradeGrandmaLevel + "x" + " Grandma";
            LblUpgrade4.Content = upgradeFarmLevel + "x" + " Farm";
            LblUpgrade5.Content = upgradeMineLevel + "x" + " Mine";
            LblUpgrade6.Content = upgradeClicker2Level + "x" + " Clicker";


            CookieRotateAndBounce();

        }



        
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //when left click on cookie image add cookie clickCount to cookies
            cookies = cookies + clickCount;
            LblCookie.Content = cookies + " Cookies";
            CookieRotateAndBounce();
            SoundClickOn();
            DrawCookies();
        }

        
        private void CookieLogic()
        {
            //incremental cookies per second
            while (true)
            {
                for (int i = 0; i < 10; i++)
                {
                    double increment = cookiesPerSecond * 0.001;
                    cookies += increment;
                }


                Thread.Sleep(10);
            }

        }

        double currentCookieRotation = 0;

        RotateTransform rotateTransform = new RotateTransform();
        private void CookieRotateAndBounce()
        {
            ScaleTransform scaleTransform = new ScaleTransform();
            CookieImage.RenderTransformOrigin = new Point(0.5, 0.5);

            currentCookieRotation = rotateTransform.Angle;

            CookieImage.RenderTransform = new TransformGroup()
            {
                Children = new TransformCollection()
        {
            scaleTransform,
            rotateTransform
        }
            };

            DoubleAnimation growAnimation = new DoubleAnimation
            {
                To = 0.95,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            DoubleAnimation shrinkAnimation = new DoubleAnimation
            {
                To = 1.05,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            DoubleAnimation growAnimation2 = new DoubleAnimation
            {
                To = 1,
                Duration = TimeSpan.FromMilliseconds(100)
            };

            DoubleAnimation rotationAnimation = new DoubleAnimation
            {
                From = currentCookieRotation,
                To = currentCookieRotation + 360,
                Duration = TimeSpan.FromSeconds(40),
                RepeatBehavior = RepeatBehavior.Forever
            };

            scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, growAnimation);
            scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, growAnimation);


            growAnimation.Completed += (s, e) =>
            {
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, growAnimation2);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, growAnimation2);
            };

            growAnimation2.Completed += (s, e) =>
            {
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, shrinkAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, shrinkAnimation);
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        }
        private void SoundClickOn()
        {
            try
            {
                soundClick.Open(new Uri("clickOn.wav", UriKind.RelativeOrAbsolute));
                soundClick.Play();
            }
            catch (Exception)
            { throw; }
        }

        private void BuyItemSound()
        {
            try
            {
                soundBuy.Open(new Uri("buy1.wav", UriKind.RelativeOrAbsolute));
                soundBuy.Play();
            }
            catch (Exception)
            { }

        }

        private void gameTimer_tick(object sender, EventArgs e)
        {
            //update game every 10ms
            cookiesPerSecond = Math.Round(cookiesPerSecond, 1);

            DrawCookies();
            DrawCookiesPerSecond();
            DrawCookiesClickerCost();
            DrawCookiesGrandmaCost();
            DrawCookiesFarmCost();
            DrawCookiesMineCost();
            DrawCookiesFactoryCost();
            DrawCookiesBankCost();
            DrawCookiesTempleCost();    
            ClickerVerify();
            GrandmaVerify();
            FarmVerify();
            MineVerify();
            FactoryVerify();
            BankVerify();
            TempleVerify();
            UIupdate();
            UpgradeUnlock();
        }
        private void UIupdate()
        {
            if (WrapUpgrades.ActualHeight > 50)
            {
                ScrollItems.MaxHeight = Math.Max(175, 175);
            }
            else
            {
                ScrollItems.MaxHeight = 221;
            }
        }
        public class CookieFormatter
        {
            public static string FormatCookies(long cCount)
            {

                if (cCount > 9999 && cCount < 999999)
                {
                    return $"{(cCount / 1000.0):F3}";
                }
                else if (cCount > 999999 && cCount < 999999999)
                {
                    return $"{(cCount / 1000000.0):F3}M";
                }
                else if (cCount > 999999999 && cCount < 999999999999)
                {
                    return $"{(cCount / 1000000000.0):F3}B";
                }
                else if (cCount > 99999999999 && cCount < 999999999999999)
                {
                    return $"{(cCount / 1000000000000.0):F3}T";
                }
                else if (cCount > 9999999999999 && cCount < 999999999999999999)
                {
                    return $"{(cCount / 1000000000000000.0):F3}q";
                }
                else
                {
                    return cCount.ToString();
                }
            }
        }
        private void DrawCookies()
        {
            string cookiesLabel = CookieFormatter.FormatCookies((long)cookies);
            LblCookie.Content = cookiesLabel + " Cookies";

        }
        private void DrawCookiesClickerCost()
        {
            string cookiesLabel = CookieFormatter.FormatCookies((long)clickerCost);
            LblCostClicker.Content = "Cost: " + cookiesLabel;
        }
        private void DrawCookiesGrandmaCost()
        {
            string cookiesLabel = CookieFormatter.FormatCookies((long)grandmaCost);
            LblCostGrandma.Content = "Cost: " + cookiesLabel;
        }
        private void DrawCookiesFarmCost()
        {
            string cookiesLabel = CookieFormatter.FormatCookies((long)farmCost);
            LblCostFarm.Content = "Cost: " + cookiesLabel;
        }
        private void DrawCookiesMineCost()
        {
            string cookiesLabel = CookieFormatter.FormatCookies((long)mineCost);
            LblCostMine.Content = "Cost: " + cookiesLabel;
        }
        private void DrawCookiesFactoryCost()
        {
            string cookiesLabel = CookieFormatter.FormatCookies((long)factoryCost);
            LblCostFactory.Content = "Cost: " + cookiesLabel;
        }
        private void DrawCookiesBankCost()
        {
            string cookiesLabel = CookieFormatter.FormatCookies((long)bankCost);
            LblCostBank.Content = "Cost: " + cookiesLabel;
        }
        private void DrawCookiesTempleCost()
        {
            string cookiesLabel =  CookieFormatter.FormatCookies((long)templeCost);
            LblCostTemple.Content = "Cost: " + cookiesLabel;
        }
        private void DrawCookiesPerSecond()
        {

            string cookiesLabel = $"{cookiesPerSecond:F1}";
            LblCookiePerSecond.Content = cookiesLabel + "/s";
        }
        //function Clicker
        private bool isMouseOverClicker = false;

        private void ClickerP_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverClicker = true;
            ClickerVerify();
        }

        private void ClickerP_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverClicker = false;
            ClickerVerify();
        }

        private void ClickerVerify()
        {
            //verify if cookie count is high enough to purchase clicker
            if (cookies < clickerCost)
            {
                ClickerP.IsEnabled = false;
                ClickerP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else if (isMouseOverClicker)
            {
                ClickerP.IsEnabled = true;
                ClickerP.Background = new SolidColorBrush(Colors.DeepSkyBlue);
            }
            else
            {
                ClickerP.IsEnabled = true;
                ClickerP.Background = new SolidColorBrush(Colors.AliceBlue);
            }
        }
        private void ClickerP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy clicker
            ClickerVerify(); BuyItemSound();
            //add 1 clicker to clicker count when executed
            clickerCount++;
            LblClicker.Content = "Clicker" + "s: " + clickerCount;

            cookies = cookies - clickerCost;
            clickerCost = clickerCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + clickerProduction;

            clickerCost = Math.Round(clickerCost);
            double clickerProductionRounded = Math.Round(clickerProduction, 2);
            LblClickerProd.Content = clickerProductionRounded + "/s";

            //add thing in middle
            ClickerMain();


        }

        private bool wrapPanelClickerCreated = false;
        private bool scrollviewerClickerCreated = false;
        private ScrollViewer scrollClicker;
        private WrapPanel wrapClicker;
        private void ClickerMain()
        {
            if (!wrapPanelClickerCreated)
            {
                scrollClicker = new ScrollViewer();
                scrollClicker.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollClicker.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Add(scrollClicker);
                scrollviewerClickerCreated = true; 
            }

            if (!wrapPanelClickerCreated)
            {
                wrapClicker = new WrapPanel();
                wrapClicker.Height = wrapHeight;
                wrapClicker.Background = new SolidColorBrush(Colors.AliceBlue);

                if (scrollClicker != null)
                {
                    scrollClicker.Content = wrapClicker; 
                    wrapPanelClickerCreated = true;
                }
            }

                Image ImgClicker = new Image();
            ImgClicker.Source = new BitmapImage(new Uri("Clicker.png", UriKind.RelativeOrAbsolute));
            ImgClicker.Width = 40;
            ImgClicker.Height = 40;
            ImgClicker.HorizontalAlignment = HorizontalAlignment.Center;
            ImgClicker.VerticalAlignment = VerticalAlignment.Center;


            wrapClicker.Children.Add(ImgClicker);
        }


        //function Grandma
        private bool isMouseOverGrandma = false;

        private void GrandmaP_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverGrandma = true;
            GrandmaVerify();
        }

        private void GrandmaP_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverGrandma = false;
            GrandmaVerify();
        }

        private void GrandmaVerify()
        {
            //verify if cookie count is high enough to purchase grandma
            if (cookies < grandmaCost)
            {
                GrandmaP.IsEnabled = false;
                GrandmaP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else if (isMouseOverGrandma)
            {
                GrandmaP.IsEnabled = true;
                GrandmaP.Background = new SolidColorBrush(Colors.DeepSkyBlue);
            }
            else
            {
                GrandmaP.IsEnabled = true;
                GrandmaP.Background = new SolidColorBrush(Colors.AliceBlue);
            }
        }

        private void GrandmaP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy grandma
            GrandmaVerify(); BuyItemSound();
            //add 1 grandma to grandma count when executed
            grandmaCount++;
            LblGrandma.Content = "Grandma" + "s: " + grandmaCount;

            cookies = cookies - grandmaCost;
            grandmaCost = grandmaCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + grandmaProduction;

            grandmaCost = Math.Round(grandmaCost);
            double grandmaProudctionRounded = Math.Round(grandmaProduction, 2);
            LblGrandmaProd.Content = grandmaProudctionRounded + "/s";

            //add thing in middle
            GrandmaMain();
        }
        private bool wrapPanelGrandmaCreated = false;
        private bool scrollviewerGrandmaCreated = false;
        private ScrollViewer scrollGrandma;
        private WrapPanel wrapGrandma;

        private void GrandmaMain()
        {
            if (!scrollviewerGrandmaCreated)
            {
                scrollGrandma = new ScrollViewer();
                scrollGrandma.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollGrandma.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Add(scrollGrandma);
                scrollviewerGrandmaCreated = true;
            }

            if (!wrapPanelGrandmaCreated)
            {
                wrapGrandma = new WrapPanel();
                wrapGrandma.Height = wrapHeight;
                wrapGrandma.Background = new SolidColorBrush(Colors.AliceBlue);

                if (scrollGrandma != null)
                {
                    scrollGrandma.Content = wrapGrandma;
                    wrapPanelGrandmaCreated = true;
                }
            }

            Image imgGrandma = new Image();
            imgGrandma.Source = new BitmapImage(new Uri("Grandma.png", UriKind.RelativeOrAbsolute));
            imgGrandma.Width = 40;
            imgGrandma.Height = 40;
            imgGrandma.HorizontalAlignment = HorizontalAlignment.Center;
            imgGrandma.VerticalAlignment = VerticalAlignment.Center;

            wrapGrandma.Children.Add(imgGrandma);
        }


        //function Farm
        bool isMouseOverFarm = false;

        private void FarmP_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverFarm = true;
            FarmVerify();

        }

        private void FarmP_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverFarm = false;
            FarmVerify();
        }

        private void FarmVerify()
        {

            //verify if cookie count is high enough to purchase Farm
            if (cookies < farmCost)
            {
                FarmP.IsEnabled = false;
                FarmP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else if (isMouseOverFarm)
            {
                FarmP.IsEnabled = true;
                FarmP.Background = new SolidColorBrush(Colors.DeepSkyBlue);
            }
            else
            {
                FarmP.IsEnabled = true;
                FarmP.Background = new SolidColorBrush(Colors.AliceBlue);
            }
        }

        private void FarmP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy farm
            FarmVerify(); BuyItemSound();
            //add 1 farm to grandma count when executed
            farmCount++;
            LblFarm.Content = "Farm" + "s: " + farmCount;

            cookies = cookies - farmCost;
            farmCost = farmCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + farmProduction;

            farmCost = Math.Round(farmCost);
            double farmProudctionRounded = Math.Round(farmProduction, 2);
            LblFarmProd.Content = farmProudctionRounded + "/s";

            FarmMain();
        }

        private bool wrapPanelFarmCreated = false;
        private bool scrollviewerFarmCreated = false;
        private ScrollViewer scrollFarm;
        private WrapPanel wrapFarm;

        private void FarmMain()
        {
            if (!scrollviewerFarmCreated)
            {
                scrollFarm = new ScrollViewer();
                scrollFarm.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollFarm.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Add(scrollFarm);
                scrollviewerFarmCreated = true;
            }

            if (!wrapPanelFarmCreated)
            {
                wrapFarm = new WrapPanel();
                wrapFarm.Height = wrapHeight;
                wrapFarm.Background = new SolidColorBrush(Colors.AliceBlue);

                if (scrollFarm != null)
                {
                    scrollFarm.Content = wrapFarm;
                    wrapPanelFarmCreated = true;
                }
            }

            Image imgFarm = new Image();
            imgFarm.Source = new BitmapImage(new Uri("farm.png", UriKind.RelativeOrAbsolute));
            imgFarm.Width = 40;
            imgFarm.Height = 40;
            imgFarm.HorizontalAlignment = HorizontalAlignment.Center;
            imgFarm.VerticalAlignment = VerticalAlignment.Center;

            wrapFarm.Children.Add(imgFarm);
        }


        //function Mine
        bool isMouseOverMine = false;
        private void MineP_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverMine = true;
            MineVerify();
        }

        private void MineP_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverMine = false;
            MineVerify();
        }
        private void MineVerify()
        {
            //verify if cookie count is high enough to purchase Farm
            if (cookies < mineCost)
            {
                MineP.IsEnabled = false;
                MineP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else if (isMouseOverMine) 
            {
                MineP.IsEnabled = true;
                MineP.Background = new SolidColorBrush(Colors.DeepSkyBlue);

            }
            else
            {
                MineP.IsEnabled = true;
                MineP.Background = new SolidColorBrush(Colors.AliceBlue);
            }
        }
        private void MineP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy mine
            MineVerify(); BuyItemSound();
            //add 1 mine to mine count when executed
            mineCount++;
            LblMine.Content = "Mine" + "s: " + mineCount;

            cookies = cookies - mineCost;
            mineCost = mineCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + mineProduction;

            mineCost = Math.Round(mineCost);
            double mineProductionRounded = Math.Round(mineProduction, 2);
            LblMineProd.Content = mineProductionRounded + "/s";

            MineMain();
        }
        private bool wrapPanelMineCreated = false;
        private bool scrollviewerMineCreated = false;
        private ScrollViewer scrollMine;
        private WrapPanel wrapMine;

        private void MineMain()
        {
            if (!scrollviewerMineCreated)
            {
                scrollMine = new ScrollViewer();
                scrollMine.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollMine.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Add(scrollMine);
                scrollviewerMineCreated = true;
            }

            if (!wrapPanelMineCreated)
            {
                wrapMine = new WrapPanel();
                wrapMine.Height = wrapHeight;
                wrapMine.Background = new SolidColorBrush(Colors.AliceBlue);

                if (scrollMine != null)
                {
                    scrollMine.Content = wrapMine;
                    wrapPanelMineCreated = true;
                }
            }

            Image imgMine = new Image();
            imgMine.Source = new BitmapImage(new Uri("mine.png", UriKind.RelativeOrAbsolute));
            imgMine.Width = 40;
            imgMine.Height = 40;
            imgMine.HorizontalAlignment = HorizontalAlignment.Center;
            imgMine.VerticalAlignment = VerticalAlignment.Center;

            wrapMine.Children.Add(imgMine);
        }


        //function Factory
        bool isMouseOverFactory = false;
        private void FactoryP_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverFactory = true;
            FactoryVerify();
        }

        private void FactoryP_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverFactory = false;
            FactoryVerify();

        }
        private void FactoryVerify()
        {

            //verify if cookie count is high enough to purchase Farm
            if (cookies < factoryCost)
            {
                FactoryP.IsEnabled = false;
                FactoryP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else if (isMouseOverFactory)
            {
                FactoryP.IsEnabled = true;
                FactoryP.Background = new SolidColorBrush(Colors.DeepSkyBlue);
            }
            else
            {
                FactoryP.IsEnabled = true;
                FactoryP.Background = new SolidColorBrush(Colors.AliceBlue);
            }
        }
        private void FactoryP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // buy factory
            FactoryVerify(); BuyItemSound();
            // add 1 factory to factory count when executed
            factoryCount++;
            LblFactory.Content = "Factory" + "s: " + factoryCount;

            cookies = cookies - factoryCost;
            factoryCost = factoryCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + factoryProduction;

            factoryCost = Math.Round(factoryCost);
            double factoryProductionRounded = Math.Round(factoryProduction, 2);
            LblFactoryProd.Content = factoryProductionRounded + "/s";

            FactoryMain();
        }
        private bool wrapPanelFactoryCreated = false;
        private bool scrollviewerFactoryCreated = false;
        private ScrollViewer scrollFactory;
        private WrapPanel wrapFactory;

        private void FactoryMain()
        {
            if (!scrollviewerFactoryCreated)
            {
                scrollFactory = new ScrollViewer();
                scrollFactory.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollFactory.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Add(scrollFactory);
                scrollviewerFactoryCreated = true;
            }

            if (!wrapPanelFactoryCreated)
            {
                wrapFactory = new WrapPanel();
                wrapFactory.Height = wrapHeight;
                wrapFactory.Background = new SolidColorBrush(Colors.AliceBlue);

                if (scrollFactory != null)
                {
                    scrollFactory.Content = wrapFactory;
                    wrapPanelFactoryCreated = true;
                }
            }

            Image imgFactory = new Image();
            imgFactory.Source = new BitmapImage(new Uri("factory.png", UriKind.RelativeOrAbsolute));
            imgFactory.Width = 40;
            imgFactory.Height = 40;
            imgFactory.HorizontalAlignment = HorizontalAlignment.Center;
            imgFactory.VerticalAlignment = VerticalAlignment.Center;

            wrapFactory.Children.Add(imgFactory);
        }


        //function Bank
        bool isMouseOverBank = false;

        private void BankP_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverBank = true;
            BankVerify();
        }

        private void BankP_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverBank = false;
            BankVerify();
        }

        private void BankVerify()
        {
            //verify if cookie count is high enough to purchase Bank
            if (cookies < bankCost)
            {
                BankP.IsEnabled = false;
                BankP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else if (isMouseOverBank)
            {
                BankP.IsEnabled = true;
                BankP.Background = new SolidColorBrush(Colors.DeepSkyBlue);
            }
            else
            {
                BankP.IsEnabled = true;
                BankP.Background = new SolidColorBrush(Colors.AliceBlue);
            }
        }

        private void BankP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // buy bank
            BankVerify();
            BuyItemSound();

            // add 1 bank to bank count when executed
            bankCount++;
            LblBank.Content = "Bank" + "s: " + bankCount;

            cookies = cookies - bankCost;
            bankCost = bankCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + bankProduction;

            bankCost = Math.Round(bankCost);
            double bankProductionRounded = Math.Round(bankProduction, 2);
            LblBankProd.Content = bankProductionRounded + "/s";

            BankMain();
        }
        private bool wrapPanelBankCreated = false;
        private bool scrollviewerBankCreated = false;
        private ScrollViewer scrollBank;
        private WrapPanel wrapBank;

        private void BankMain()
        {
            if (!scrollviewerBankCreated)
            {
                scrollBank = new ScrollViewer();
                scrollBank.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollBank.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Add(scrollBank);
                scrollviewerBankCreated = true;
            }

            if (!wrapPanelBankCreated)
            {
                wrapBank = new WrapPanel();
                wrapBank.Height = wrapHeight;
                wrapBank.Background = new SolidColorBrush(Colors.AliceBlue);

                if (scrollBank != null)
                {
                    scrollBank.Content = wrapBank;
                    wrapPanelBankCreated = true;
                }
            }

            Image imgBank = new Image();
            imgBank.Source = new BitmapImage(new Uri("bank.png", UriKind.RelativeOrAbsolute));
            imgBank.Width = 40;
            imgBank.Height = 40;
            imgBank.HorizontalAlignment = HorizontalAlignment.Center;
            imgBank.VerticalAlignment = VerticalAlignment.Center;

            wrapBank.Children.Add(imgBank);
        }

        //function Temple
        bool isMouseOverTemple = false;

        private void TempleP_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverTemple = true;
            TempleVerify();
        }

        private void TempleP_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverTemple = false;
            TempleVerify();
        }

        private void TempleVerify()
        {
            // Verify if cookie count is high enough to purchase Temple
            if (cookies < templeCost)
            {
                TempleP.IsEnabled = false;
                TempleP.Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            else if (isMouseOverTemple)
            {
                TempleP.IsEnabled = true;
                TempleP.Background = new SolidColorBrush(Colors.DeepSkyBlue);
            }
            else
            {
                TempleP.IsEnabled = true;
                TempleP.Background = new SolidColorBrush(Colors.AliceBlue);
            }
        }

        private void TempleP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Buy Temple
            TempleVerify();
            BuyItemSound();

            // Add 1 Temple to Temple count when executed
            templeCount++;
            LblTemple.Content = "Temple" + "s: " + templeCount;

            cookies = cookies - templeCost;
            templeCost = templeCost * 1.25;
            cookiesPerSecond = cookiesPerSecond + templeProduction;

            templeCost = Math.Round(templeCost);
            double templeProductionRounded = Math.Round(templeProduction, 2);
            LblTempleProd.Content = templeProductionRounded + "/s";

            TempleMain();
        }
        private bool wrapPanelTempleCreated = false;
        private bool scrollviewerTempleCreated = false;
        private ScrollViewer scrollTemple;
        private WrapPanel wrapTemple;

        private void TempleMain()
        {
            if (!scrollviewerTempleCreated)
            {
                scrollTemple = new ScrollViewer();
                scrollTemple.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollTemple.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Add(scrollTemple);
                scrollviewerTempleCreated = true;
            }

            if (!wrapPanelTempleCreated)
            {
                wrapTemple = new WrapPanel();
                wrapTemple.Height = wrapHeight;
                wrapTemple.Background = new SolidColorBrush(Colors.AliceBlue);

                if (scrollTemple != null)
                {
                    scrollTemple.Content = wrapTemple;
                    wrapPanelTempleCreated = true;
                }
            }

            Image imgTemple = new Image();
            imgTemple.Source = new BitmapImage(new Uri("temple.png", UriKind.RelativeOrAbsolute));
            imgTemple.Width = 40;
            imgTemple.Height = 40;
            imgTemple.HorizontalAlignment = HorizontalAlignment.Center;
            imgTemple.VerticalAlignment = VerticalAlignment.Center;

            wrapTemple.Children.Add(imgTemple);
        }
        //hover function Upgrade1 Cursor

        private bool isMouseOverUpgrade1 = false;
        private void Upgrade1_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade1 = true;
            UpgradeUnlock();
        }

        private void Upgrade1_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade1 = false;
            UpgradeUnlock();
        }

        //hover function Upgrade2 Clicker

        private bool isMouseOverUpgrade2 = false;
        private void Upgrade2_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade2 = true;
            UpgradeUnlock();
        }

        private void Upgrade2_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade2 = false;
            UpgradeUnlock();
        }

        //hover function Upgrade3 Grandma

        private bool isMouseOverUpgrade3 = false;

        private void Upgrade3_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade3 = true;
            UpgradeUnlock();
        }
        private void Upgrade3_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade3 = false;
            UpgradeUnlock();

        }
        private bool isMouseOverUpgrade4 = false;

        //hover function Upgrade4 Farm

        private void Upgrade4_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade4 = true;
            UpgradeUnlock();
        }
        private void Upgrade4_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade4 = false;
            UpgradeUnlock();

        }
        private bool isMouseOverUpgrade5 = false;

        //hover function Upgrade5 Mine

        private void Upgrade5_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade5 = true;
            UpgradeUnlock();
        }

        private void Upgrade5_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade5 = false;
            UpgradeUnlock();

        }
        //hover function Upgrade6 Clicker2
        private bool isMouseOverUpgrade6 = false;


        private void Upgrade6_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade6 = true;
            UpgradeUnlock();
        }

        private void Upgrade6_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade6 = false;
            UpgradeUnlock();

        }
        //hover function Upgrade7 Factory
        private bool isMouseOverUpgrade7 = false;

        private void Upgrade7_MouseEnter(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade7 = true;
            UpgradeUnlock();
        }

        private void Upgrade7_MouseLeave(object sender, MouseEventArgs e)
        {
            isMouseOverUpgrade7 = false;
            UpgradeUnlock();
        }

        private void UpgradeUnlock()
        {
            //verifies if u have enough cookies to purchase upgrade.

            //upgrade 1 Cursor
            if (cookies < cookieCostUpgradeCursor)
            {
                Upgrade1.IsEnabled = false;
                Upgrade1.Background = new SolidColorBrush(Colors.SaddleBrown);
                LblUpgrade1.Foreground = new SolidColorBrush(Colors.Wheat);
            }
            else if (isMouseOverUpgrade1)
            {
                Upgrade1.IsEnabled = true;
                Upgrade1.Background = new SolidColorBrush(Colors.RosyBrown);
                LblUpgrade1.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Upgrade1.IsEnabled = true;

                Upgrade1.Background = new SolidColorBrush(Colors.SandyBrown);
                LblUpgrade1.Foreground = new SolidColorBrush(Colors.Black);
            }

            //upgrade 2 Clicker1

            if (cookies < cookieCostUpgradeClicker)
            {
                Upgrade2.IsEnabled = false;
                Upgrade2.Background = new SolidColorBrush(Colors.SaddleBrown);
                LblUpgrade2.Foreground = new SolidColorBrush(Colors.Wheat);
            }
            else if (isMouseOverUpgrade2)
            {
                Upgrade2.IsEnabled = true;
                Upgrade2.Background = new SolidColorBrush(Colors.RosyBrown);
                LblUpgrade2.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Upgrade2.IsEnabled = true;
                Upgrade2.Background = new SolidColorBrush(Colors.SandyBrown);
                LblUpgrade2.Foreground = new SolidColorBrush(Colors.Black);
            }

            //upgrade 3 Grandm2x


            if (cookies < cookieCostUpgradeGrandma)
            {
                Upgrade3.IsEnabled = false;
                Upgrade3.Background = new SolidColorBrush(Colors.SaddleBrown);
                LblUpgrade3.Foreground = new SolidColorBrush(Colors.Wheat);
            }
            else if (isMouseOverUpgrade3)
            {
                Upgrade3.IsEnabled = true;
                Upgrade3.Background = new SolidColorBrush(Colors.RosyBrown);
                LblUpgrade3.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Upgrade3.IsEnabled = true;
                Upgrade3.Background = new SolidColorBrush(Colors.SandyBrown);
                LblUpgrade3.Foreground = new SolidColorBrush(Colors.Black);
            }
            // Upgrade 4 Farm2x
            if (cookies < cookieCostUpgradeFarm)
            {
                Upgrade4.IsEnabled = false;
                Upgrade4.Background = new SolidColorBrush(Colors.SaddleBrown);
                LblUpgrade4.Foreground = new SolidColorBrush(Colors.Wheat);
            }
            else if (isMouseOverUpgrade4)
            {
                Upgrade4.IsEnabled = true;
                Upgrade4.Background = new SolidColorBrush(Colors.RosyBrown);
                LblUpgrade4.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                Upgrade4.IsEnabled = true;
                Upgrade4.Background = new SolidColorBrush(Colors.SandyBrown);
                LblUpgrade4.Foreground = new SolidColorBrush(Colors.Black);
            }

            //upgrade 5 Mine2x
            if (cookies > cookieCostUpgradeMine && upgradeMineCount != 1)
            {
                BorderUpgradeMine.Visibility = Visibility.Visible;

                if (cookies < cookieCostUpgradeMine)
                {
                    Upgrade5.IsEnabled = false;
                    Upgrade5.Background = new SolidColorBrush(Colors.SaddleBrown);
                    LblUpgrade5.Foreground = new SolidColorBrush(Colors.Wheat);
                }
                else if (isMouseOverUpgrade5)
                {
                    Upgrade5.IsEnabled = true;
                    Upgrade5.Background = new SolidColorBrush(Colors.RosyBrown);
                    LblUpgrade5.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    Upgrade5.IsEnabled = true;
                    Upgrade5.Background = new SolidColorBrush(Colors.SandyBrown);
                    LblUpgrade5.Foreground = new SolidColorBrush(Colors.Black);
                }

            }
            


            //upgrade 6 Clicker2

            if (upgradeClicker2Count != 1 && upgradeClickerCount > 0)
            {
                BorderUpgradeClicker2.Visibility = Visibility.Visible;

                if (cookies < cookieCostUpgradeClicker2)
                {
                    Upgrade6.IsEnabled = false;
                    Upgrade6.Background = new SolidColorBrush(Colors.SaddleBrown);
                    LblUpgrade6.Foreground = new SolidColorBrush(Colors.Wheat);
                }
                else if (isMouseOverUpgrade6)
                {
                    Upgrade6.IsEnabled = true;
                    Upgrade6.Background = new SolidColorBrush(Colors.RosyBrown);
                    LblUpgrade6.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    Upgrade6.IsEnabled = true;
                    Upgrade6.Background = new SolidColorBrush(Colors.SandyBrown);
                    LblUpgrade6.Foreground = new SolidColorBrush(Colors.Black);
                }
            }
            //upgrade 7 Factory

            if (cookies > cookieCostUpgradeFactory && upgradeFactoryCount != 1)
            {
                BorderUpgradeFactory.Visibility = Visibility.Visible;

                if (cookies < cookieCostUpgradeFactory)
                {
                    Upgrade7.IsEnabled = false;
                    Upgrade7.Background = new SolidColorBrush(Colors.SaddleBrown);
                    LblUpgrade7.Foreground = new SolidColorBrush(Colors.Wheat);
                }
                else if (isMouseOverUpgrade7)
                {
                    Upgrade7.IsEnabled = true;
                    Upgrade7.Background = new SolidColorBrush(Colors.RosyBrown);
                    LblUpgrade7.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    Upgrade7.IsEnabled = true;
                    Upgrade7.Background = new SolidColorBrush(Colors.SandyBrown);
                    LblUpgrade7.Foreground = new SolidColorBrush(Colors.Black);
                }
            }


        }

        private void Upgrade1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clickCount = clickCount * upgradeCursorLevel;
            cookies = cookies - cookieCostUpgradeCursor;
            cookieCostUpgradeCursor = cookieCostUpgradeCursor * 3.5;
        }

        private void Upgrade2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            upgradeClickerCount++;

            cookiesPerSecond = cookiesPerSecond - (clickerProduction * clickerCount);
            clickerProduction = clickerProduction * upgradeClickerLevel;
            cookiesPerSecond = cookiesPerSecond + (clickerProduction * clickerCount);
            cookies = cookies - cookieCostUpgradeClicker;
            LblClickerProd.Content = clickerProduction + "/s";

            BorderUpgradeClicker1.Visibility = Visibility.Collapsed;
            double clickerProductionRounded = Math.Round(clickerProduction, 2);
            LblClickerProd.Content = clickerProductionRounded + "/s";
        }

        private void Upgrade3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            cookiesPerSecond = cookiesPerSecond - (grandmaProduction * grandmaCount);
            grandmaProduction = grandmaProduction * upgradeGrandmaLevel;
            cookiesPerSecond = cookiesPerSecond + (grandmaProduction * grandmaCount);
            cookies = cookies - cookieCostUpgradeGrandma;
            LblGrandmaProd.Content = grandmaProduction + "/s";

            BorderUpgradeGrandma.Visibility = Visibility.Collapsed;
            double grandmaProductionRounded = Math.Round(grandmaProduction, 2);
            LblGrandmaProd.Content = grandmaProductionRounded + "/s";
        }
        private void Upgrade4_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            upgradeFarmCount++;
            cookiesPerSecond = cookiesPerSecond - (farmProduction * farmCount);
            farmProduction = farmProduction * upgradeFarmLevel;
            cookiesPerSecond = cookiesPerSecond + (farmProduction * farmCount);
            cookies = cookies - cookieCostUpgradeFarm;
            LblFarmProd.Content = farmProduction + "/s";

            BorderUpgradeFarm.Visibility = Visibility.Collapsed;
            double farmProductionRounded = Math.Round(farmProduction, 2);
            LblFarmProd.Content = farmProductionRounded + "/s";
        }
        private void Upgrade5_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            upgradeMineCount++;
            cookiesPerSecond = cookiesPerSecond - (mineProduction * mineCount);
            mineProduction = mineProduction * upgradeMineLevel;
            cookiesPerSecond = cookiesPerSecond + (mineProduction * mineCount);
            cookies = cookies - cookieCostUpgradeMine;
            LblMineProd.Content = mineProduction + "/s";

            BorderUpgradeMine.Visibility = Visibility.Collapsed;
            double mineProductionRounded = Math.Round(mineProduction, 2);
            LblMineProd.Content = mineProductionRounded + "/s";
        }
        private void Upgrade6_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            upgradeClicker2Count++;

            cookiesPerSecond = cookiesPerSecond - (clickerProduction * clickerCount);
            clickerProduction = clickerProduction * upgradeClicker2Level;
            cookiesPerSecond = cookiesPerSecond + (clickerProduction * clickerCount);
            cookies = cookies - cookieCostUpgradeClicker2;
            LblClickerProd.Content = clickerProduction + "/s";

            BorderUpgradeClicker2.Visibility = Visibility.Collapsed;
            double clickerProductionRounded = Math.Round(clickerProduction, 2);
            LblClickerProd.Content = clickerProductionRounded + "/s";
        }

        private void Upgrade7_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            upgradeFactoryCount++;
            cookiesPerSecond = cookiesPerSecond - (factoryProduction * factoryCount);
            factoryProduction = factoryProduction * upgradeFactoryLevel;
            cookiesPerSecond = cookiesPerSecond + (factoryProduction * factoryCount);
            cookies = cookies - cookieCostUpgradeFactory;
            LblFactoryProd.Content = factoryProduction + "/s";

            BorderUpgradeFactory.Visibility = Visibility.Collapsed;
            double factoryProductionRounded = Math.Round(factoryProduction, 2);
            LblFactoryProd.Content = factoryProductionRounded + "/s";
        }



    }
}
