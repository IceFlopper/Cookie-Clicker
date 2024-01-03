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

        //all variables for the game

        int clicks = 0;
        private double cookies = 0;
        private double cookiesPerSecond = 0;
        double prodMultiplier = 1.05;
        double costMultiplier = 1.25;
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
        int upgradeCursorCount = 0;
        int upgradeCursorCostMultiplier = 5;
        int upgradeCursorLevel = 2;
        double cookieCostUpgradeCursor = 100;
        //Clickerupgrade
        int upgradeClickerCount = 0;
        int upgradeClickerLevel = 5;
        double cookieCostUpgradeClicker = 250;
        //grandmaupgrade
        int upgradeGrandmaCount = 0;
        int upgradeGrandmaLevel = 3;
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
        int upgradeClicker2Level = 5;
        double cookieCostUpgradeClicker2 = 10000;
        //factoryupgrade
        int upgradeFactoryCount = 0;
        int upgradeFactoryLevel = 2;
        double cookieCostUpgradeFactory = 200000;


        //players for the sounds
        MediaPlayer soundClick = new MediaPlayer();
        MediaPlayer soundBuy = new MediaPlayer();
        MediaPlayer soundAchievement = new MediaPlayer();

        //timers for game logic and updating
        DispatcherTimer gameTimer = new DispatcherTimer();
        DispatcherTimer secondsTimer = new DispatcherTimer();

        //height for wrap panels
        int wrapHeight = 50;
        int imgSize = 45;

        public MainWindow()
        {
            InitializeComponent();

            //thread for thread.sleep in CookieLogic
            Thread cookieThread = new Thread(CookieLogic);
            cookieThread.Start();


            soundBuy.Volume = 0.4;
            soundAchievement.Volume = 0.6;


            gameTimer.Interval = TimeSpan.FromMilliseconds(10);
            gameTimer.Tick += gameTimer_tick;
            //gameTimer.Tick += CookieLogic;

            gameTimer.Start();

            secondsTimer.Interval = TimeSpan.FromSeconds(1);
            secondsTimer.Tick += SecondsCounter;
            secondsTimer.Start();

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
        private void Viewbox_Loaded(object sender, RoutedEventArgs e)
        {
            //make inputbo for bakery name label
            string bakeryName = "";

            while (string.IsNullOrEmpty(bakeryName) || bakeryName.Length > 20)
            {
                bakeryName = Interaction.InputBox("What's your bakery name? (below 20 characters)", "Bakery name", "");

                if (string.IsNullOrEmpty(bakeryName))
                {
                    return;
                }
                if (bakeryName.Length > 20)
                {
                    MessageBox.Show("Name must be equal or less than 20 characters or empty!");
                }
            }

            LblBakeryName.Content = bakeryName + "'s Bakery";
        }



        private void DevBtn_Click(object sender, RoutedEventArgs e)
        {
            clickCount += 1000000;
        }

        int seconds = 0;
        private void SecondsCounter(object sender, EventArgs e)
        {
            seconds++;
            //LblSeconds.Content = "Seconds" + seconds.ToString();
        }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //when left click on cookie image add cookie clickCount to cookies
            cookies = cookies + clickCount;
            LblCookie.Content = cookies + " Cookies";
            CookieRotateAndBounce();
            SoundClickOn();
            DrawCookies();
            clicks++;
            //LblClicks.Content = "Clicks" + clicks;
        }

        
        double tick = 0;
        private void CookieLogic()
        {
            //incremental cookies per second
            while (true)
            {
                for (int i = 0; i < 10; i++)
                {
                    double increment = cookiesPerSecond * 0.0025;
                    cookies += increment;
                }

                tick += 1;
                Thread.Sleep(25);
            }
        }

        double currentCookieRotation = 0;

        RotateTransform rotateTransform = new RotateTransform();
        private void CookieRotateAndBounce()
        {
            //cookie rotation and bounce effect
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
                scaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, shrinkAnimation);
                scaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, shrinkAnimation);
            };

            rotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotationAnimation);
        }
        private void SoundClickOn()
        {
            //cookie click sound
                soundClick.Open(new Uri("clickOn.wav", UriKind.RelativeOrAbsolute));
                soundClick.Play();
        }

        private void BuyItemSound()
        {
            //buy item click sound
            soundBuy.Open(new Uri("buy1.wav", UriKind.RelativeOrAbsolute));
            soundBuy.Play();
        }

        private void PlayAchievementSound() 
        {
            //unlock achievement sound
            soundAchievement.Open(new Uri("Achievement.wav", UriKind.RelativeOrAbsolute));
            soundAchievement.Play();
        }

        private void gameTimer_tick(object sender, EventArgs e)
        {
            //update game every 10ms
            //LblTick.Content = "Ticks" + tick;

            DrawCookies();
            ClickerVerify();
            GrandmaVerify();
            FarmVerify();
            MineVerify();
            FactoryVerify();
            BankVerify();
            TempleVerify();
            UIupdate();
            UpgradeUnlock();
            VerifyAchievements();
        }
        private void UIupdate()
        {
            //fix UI sizing dynamically
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
            public static string FormatCookies(double cCount)
            {
                //format cookies to be easier to read
                
                if(cCount > 10 && cCount < 9999)
                {
                    return $"{(cCount):F0}";
                }
                else if (cCount > 9999 && cCount < 999999)
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

        private string DrawLabel(double amount, string prefix = "", string suffix = "")
        {
            //cookie formatter main function
            string label = CookieFormatter.FormatCookies(amount);
            return prefix + label + suffix;
        }
        private void DrawCookies()
        {
            //update all labels with formatting.
            LblCookie.Content = DrawLabel((long)cookies, "", " Cookies"); ;
            LblCostClicker.Content = DrawLabel((long)clickerCost, "Cost: ");
            LblCostGrandma.Content = DrawLabel((long)grandmaCost, "Cost: ");
            LblCostFarm.Content = DrawLabel((long)farmCost, "Cost: ");
            LblCostMine.Content = DrawLabel((long)mineCost, "Cost: ");
            LblCostFactory.Content = DrawLabel((long)factoryCost, "Cost: ");
            LblCostBank.Content = DrawLabel((long)bankCost, "Cost: ");
            LblCostTemple.Content = DrawLabel((long)templeCost, "Cost: ");
            LblCookiePerSecond.Content = DrawLabel(Math.Round(cookiesPerSecond, 2), "", "/s");

            TooltipUpgrade1.Content = DrawLabel(cookieCostUpgradeCursor, "Costs: ");
            TooltipUpgrade2.Content = DrawLabel(cookieCostUpgradeClicker, "Costs: ");
            TooltipUpgrade3.Content = DrawLabel(cookieCostUpgradeGrandma, "Costs: ");
            TooltipUpgrade4.Content = DrawLabel(cookieCostUpgradeFarm, "Costs: ");
            TooltipUpgrade5.Content = DrawLabel(cookieCostUpgradeMine, "Costs: ");
            TooltipUpgrade6.Content = DrawLabel(cookieCostUpgradeClicker2, "Costs: ");
            TooltipUpgrade7.Content = DrawLabel(cookieCostUpgradeFactory, "Costs: ");
        }

        //all functionality for Clicker object
        private bool isMouseOverClicker = false;

        private void ClickerP_MouseEnter(object sender, MouseEventArgs e)
        {
            //upgrade hover effect in
            isMouseOverClicker = true;
            ClickerVerify();
        }

        private void ClickerP_MouseLeave(object sender, MouseEventArgs e)
        {
            //upgrade hover effect out
            isMouseOverClicker = false;
            ClickerVerify();
        }

        private bool clickerPVisibilityChanged = false;
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
            if (cookies >= clickerCost && clickerPVisibilityChanged == false)
            {
                ClickerBorder.Visibility = Visibility.Visible;
                clickerPVisibilityChanged = true;
            }
            else
            {

            }
        }
        private void ClickerP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy clicker
            ClickerVerify(); BuyItemSound();
            //add 1 clicker to clicker count when executed
            clickerCount++;
            LblClicker.Content = "Clicker" + "s: " + clickerCount;
            //increase cost and increase production for scaling
            cookies = cookies - clickerCost;
            clickerCost = clickerCost * costMultiplier;
            cookiesPerSecond = cookiesPerSecond + clickerProduction;
            clickerProduction = clickerProduction * prodMultiplier;
            //round cost of production
            clickerCost = Math.Round(clickerCost);
            double clickerProductionRounded = Math.Round(clickerProduction, 2);
            LblClickerProd.Content = clickerProductionRounded + "/s";

            //show investment in middle
            ClickerWrap();
            ClickerImageSpawn();
            scrollClicker.Visibility = Visibility.Visible;



        }

        private bool wrapPanelClickerCreated = false;
        private bool scrollviewerClickerCreated = false;
        private bool imgClickerCreated = false;
        private ScrollViewer scrollClicker;
        private WrapPanel wrapClicker;

        private void ClickerWrap()
        {
            //make clicker UI elemnt in middle of screen and create children if needed

            if (!scrollviewerClickerCreated)
            {
                scrollClicker = new ScrollViewer();
                scrollClicker.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollClicker.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Insert(0, scrollClicker);
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
        }
        private void ClickerImageSpawn()
        {
            //create image element inside the wrap UI element

            Image ImgClicker = new Image();
            ImgClicker.Source = new BitmapImage(new Uri("Clicker.png", UriKind.RelativeOrAbsolute));
            ImgClicker.Width = imgSize;
            ImgClicker.Height = imgSize;
            ImgClicker.HorizontalAlignment = HorizontalAlignment.Center;
            ImgClicker.VerticalAlignment = VerticalAlignment.Center;


            wrapClicker.Children.Add(ImgClicker);
            imgClickerCreated = true;
        }


        //all functionality for Grandma object
        private bool isMouseOverGrandma = false;

        private void GrandmaP_MouseEnter(object sender, MouseEventArgs e)
        {
            //upgrade hover effect in
            isMouseOverGrandma = true;
            GrandmaVerify();
        }

        private void GrandmaP_MouseLeave(object sender, MouseEventArgs e)
        {
            //upgrade hover effect out
            isMouseOverGrandma = false;
            GrandmaVerify();
        }

        private bool grandmaPVisibilityChanged = false;
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
            if (cookies >= grandmaCost && grandmaPVisibilityChanged == false)
            {
                GrandmaBorder.Visibility = Visibility.Visible;
                grandmaPVisibilityChanged = true;
            }
            else
            {

            }
        }

        private void GrandmaP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy grandma
            GrandmaVerify(); BuyItemSound();
            //add 1 grandma to grandma count when executed
            grandmaCount++;
            LblGrandma.Content = "Grandma" + "s: " + grandmaCount;
            //increase cost and increase production for scaling
            cookies = cookies - grandmaCost;
            grandmaCost = grandmaCost * costMultiplier;
            cookiesPerSecond = cookiesPerSecond + grandmaProduction;
            grandmaProduction = grandmaProduction * prodMultiplier;
            //round cost of production
            grandmaCost = Math.Round(grandmaCost);
            double grandmaProudctionRounded = Math.Round(grandmaProduction, 2);
            LblGrandmaProd.Content = grandmaProudctionRounded + "/s";

            //show investment in middle
            ClickerWrap();
            GrandmaWrap();
            GrandmaImageSpawn();
            scrollGrandma.Visibility = Visibility.Visible;

        }
        private bool wrapPanelGrandmaCreated = false;
        private bool scrollviewerGrandmaCreated = false;
        private bool imgGrandmaCreated = false;
        private ScrollViewer scrollGrandma;
        private WrapPanel wrapGrandma;

        private void GrandmaWrap()
        {
            //make grandma UI elemnt in middle of screen and create children if needed

            if (!scrollviewerGrandmaCreated)
            {
                scrollGrandma = new ScrollViewer();
                scrollGrandma.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollGrandma.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Insert(1, scrollGrandma);
                scrollviewerGrandmaCreated = true;
            }

            if (!wrapPanelGrandmaCreated)
            {
                wrapGrandma = new WrapPanel();
                wrapGrandma.Height = wrapHeight;
                wrapGrandma.Background = new SolidColorBrush(Colors.DarkRed);

                if (scrollGrandma != null)
                {
                    if (imgClickerCreated == false)
                    {scrollClicker.Visibility = Visibility.Collapsed;}
                    scrollGrandma.Content = wrapGrandma;
                    wrapPanelGrandmaCreated = true;
                }
            }
        }
        private void GrandmaImageSpawn()
        {
            //create image element inside the wrap UI element

            Image imgGrandma = new Image();
            imgGrandma.Source = new BitmapImage(new Uri("Grandma.png", UriKind.RelativeOrAbsolute));
            imgGrandma.Width = imgSize;
            imgGrandma.Height = imgSize;
            imgGrandma.HorizontalAlignment = HorizontalAlignment.Center;
            imgGrandma.VerticalAlignment = VerticalAlignment.Center;

            wrapGrandma.Children.Add(imgGrandma);
            imgGrandmaCreated = true;
        }


        //all functionality for Farm object
        bool isMouseOverFarm = false;

        private void FarmP_MouseEnter(object sender, MouseEventArgs e)
        {
            //upgrade hover effect in
            isMouseOverFarm = true;
            FarmVerify();

        }

        private void FarmP_MouseLeave(object sender, MouseEventArgs e)
        {
            //upgrade hover effect out
            isMouseOverFarm = false;
            FarmVerify();
        }

        private bool farmPVisibilityChanged = false;
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
            if (cookies >= farmCost && farmPVisibilityChanged == false)
            {
                FarmBorder.Visibility = Visibility.Visible;
                farmPVisibilityChanged = true;
            }
            else
            {

            }
        }

        private void FarmP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy farm
            FarmVerify(); BuyItemSound();
            //add 1 farm to grandma count when executed
            farmCount++;
            LblFarm.Content = "Farm" + "s: " + farmCount;
            //increase cost and increase production for scaling
            cookies = cookies - farmCost;
            farmCost = farmCost * costMultiplier;
            cookiesPerSecond = cookiesPerSecond + farmProduction;
            farmProduction = farmProduction * prodMultiplier;
            //round cost of production
            farmCost = Math.Round(farmCost);
            double farmProudctionRounded = Math.Round(farmProduction, 2);
            LblFarmProd.Content = farmProudctionRounded + "/s";

            //show investment in middle
            ClickerWrap();
            GrandmaWrap();
            FarmWrap();
            FarmImageSpawn();

            scrollFarm.Visibility = Visibility.Visible;

        }

        private bool wrapPanelFarmCreated = false;
        private bool scrollviewerFarmCreated = false;
        private bool imgFarmCreated = false;
        private ScrollViewer scrollFarm;
        private WrapPanel wrapFarm;

        private void FarmWrap()
        {
            //make farm UI elemnt in middle of screen and create children if needed
            if (!scrollviewerFarmCreated)
            {
                scrollFarm = new ScrollViewer();
                scrollFarm.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollFarm.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Insert(2, scrollFarm);
                scrollviewerFarmCreated = true;
            }

            if (!wrapPanelFarmCreated)
            {
                wrapFarm = new WrapPanel();
                wrapFarm.Height = wrapHeight;
                wrapFarm.Background = new SolidColorBrush(Colors.DarkOliveGreen);

                if (scrollFarm != null)
                {
                    if (imgClickerCreated == false)
                    {scrollClicker.Visibility = Visibility.Collapsed;}
                    if (imgGrandmaCreated == false)
                    {scrollGrandma.Visibility = Visibility.Collapsed;}

                    scrollFarm.Content = wrapFarm;
                    wrapPanelFarmCreated = true;
                }
            }
        }
        private void FarmImageSpawn()
        {
            //create image element inside the wrap UI element

            Image imgFarm = new Image();
            imgFarm.Source = new BitmapImage(new Uri("farm.png", UriKind.RelativeOrAbsolute));
            imgFarm.Width = imgSize;
            imgFarm.Height = imgSize;
            imgFarm.HorizontalAlignment = HorizontalAlignment.Center;
            imgFarm.VerticalAlignment = VerticalAlignment.Center;

            wrapFarm.Children.Add(imgFarm);
            imgFarmCreated = true;
        }


        //all functionality for Mine object
        bool isMouseOverMine = false;
        private void MineP_MouseEnter(object sender, MouseEventArgs e)
        {
            //upgrade hover effect in
            isMouseOverMine = true;
            MineVerify();
        }

        private void MineP_MouseLeave(object sender, MouseEventArgs e)
        {
            //upgrade hover effect out
            isMouseOverMine = false;
            MineVerify();
        }

        private bool minePVisibilityChanged = false;
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
            if (cookies >= mineCost && minePVisibilityChanged == false)
            {
                MineBorder.Visibility = Visibility.Visible;
                minePVisibilityChanged = true;
            }
            else
            {

            }
        }
        private void MineP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //buy mine
            MineVerify(); BuyItemSound();
            //add 1 mine to mine count when executed
            mineCount++;
            LblMine.Content = "Mine" + "s: " + mineCount;
            //increase cost and increase production for scaling
            cookies = cookies - mineCost;
            mineCost = mineCost * costMultiplier;
            cookiesPerSecond = cookiesPerSecond + mineProduction;
            mineProduction = mineProduction * prodMultiplier;
            //round cost of production
            mineCost = Math.Round(mineCost);
            double mineProductionRounded = Math.Round(mineProduction, 2);
            LblMineProd.Content = mineProductionRounded + "/s";

            //show investment in middle
            ClickerWrap();
            GrandmaWrap();
            FarmWrap();
            MineWrap();
            MineImageSpawn();

            scrollMine.Visibility = Visibility.Visible;
        }
        private bool wrapPanelMineCreated = false;
        private bool scrollviewerMineCreated = false;
        private bool imgMineCreated = false;
        private ScrollViewer scrollMine;
        private WrapPanel wrapMine;

        private void MineWrap()
        {
            //make mine UI elemnt in middle of screen and create children if needed

            if (!scrollviewerMineCreated)
            {
                scrollMine = new ScrollViewer();
                scrollMine.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollMine.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Insert(3, scrollMine);
                scrollviewerMineCreated = true;
            }

            if (!wrapPanelMineCreated)
            {
                wrapMine = new WrapPanel();
                wrapMine.Height = wrapHeight;
                wrapMine.Background = new SolidColorBrush(Colors.Gray);

                if (scrollMine != null)
                {
                    if (imgClickerCreated == false)
                    { scrollClicker.Visibility = Visibility.Collapsed;}
                    if (imgGrandmaCreated == false)
                    { scrollGrandma.Visibility = Visibility.Collapsed;}
                    if (imgFarmCreated == false)
                    {  scrollFarm.Visibility = Visibility.Collapsed;}
                    scrollMine.Content = wrapMine;
                    wrapPanelMineCreated = true;
                }
            }
        }
        private void MineImageSpawn()
        {
            //create image element inside the wrap UI element

            Image imgMine = new Image();
            imgMine.Source = new BitmapImage(new Uri("mine.png", UriKind.RelativeOrAbsolute));
            imgMine.Width = imgSize;
            imgMine.Height = imgSize;
            imgMine.HorizontalAlignment = HorizontalAlignment.Center;
            imgMine.VerticalAlignment = VerticalAlignment.Center;

            wrapMine.Children.Add(imgMine);
            imgMineCreated = true;
        }


        //all functionality for Factory object
        bool isMouseOverFactory = false;
        private void FactoryP_MouseEnter(object sender, MouseEventArgs e)
        {
            //upgrade hover effect in
            isMouseOverFactory = true;
            FactoryVerify();
        }

        private void FactoryP_MouseLeave(object sender, MouseEventArgs e)
        {
            //upgrade hover effect out
            isMouseOverFactory = false;
            FactoryVerify();

        }

        private bool factoryPVisibilityChanged = false;
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
            if (cookies >= factoryCost && factoryPVisibilityChanged == false)
            {
                FactoryBorder.Visibility = Visibility.Visible;
                factoryPVisibilityChanged = true;
            }
            else
            {

            }

        }
        private void FactoryP_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // buy factory
            FactoryVerify(); BuyItemSound();
            // add 1 factory to factory count when executed
            factoryCount++;
            LblFactory.Content = "Factory" + "s: " + factoryCount;
            //increase cost and increase production for scaling
            cookies = cookies - factoryCost;
            factoryCost = factoryCost * costMultiplier;
            cookiesPerSecond = cookiesPerSecond + factoryProduction;
            factoryProduction = factoryProduction * prodMultiplier;
            //round cost of production
            factoryCost = Math.Round(factoryCost);
            double factoryProductionRounded = Math.Round(factoryProduction, 2);
            LblFactoryProd.Content = factoryProductionRounded + "/s";
            //show investment in middle
            ClickerWrap();
            GrandmaWrap();
            FarmWrap();
            MineWrap();
            FactoryWrap();
            FactoryImageSpawn();

            scrollFactory.Visibility = Visibility.Visible;
        }
        private bool wrapPanelFactoryCreated = false;
        private bool scrollviewerFactoryCreated = false;
        private bool imgFactoryCreated = false;
        private ScrollViewer scrollFactory;
        private WrapPanel wrapFactory;

        private void FactoryWrap()
        {
            //make factory UI elemnt in middle of screen and create children if needed

            if (!scrollviewerFactoryCreated)
            {
                scrollFactory = new ScrollViewer();
                scrollFactory.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollFactory.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Insert(4, scrollFactory);
                scrollviewerFactoryCreated = true;
            }

            if (!wrapPanelFactoryCreated)
            {
                wrapFactory = new WrapPanel();
                wrapFactory.Height = wrapHeight;
                wrapFactory.Background = new SolidColorBrush(Colors.OrangeRed);

                if (scrollFactory != null)
                {
                    if (imgClickerCreated == false) 
                    { scrollClicker.Visibility = Visibility.Collapsed; }
                    if (imgGrandmaCreated == false) 
                    { scrollGrandma.Visibility = Visibility.Collapsed; }
                    if (imgFarmCreated == false)
                    { scrollFarm.Visibility = Visibility.Collapsed; }
                    if (imgMineCreated == false)
                    { scrollMine.Visibility = Visibility.Collapsed; }
                    
                    scrollFactory.Content = wrapFactory;
                    wrapPanelFactoryCreated = true;
                }
            }
        }
        private void FactoryImageSpawn()
        {
            //create image element inside the wrap UI element

            Image imgFactory = new Image();
            imgFactory.Source = new BitmapImage(new Uri("factory.png", UriKind.RelativeOrAbsolute));
            imgFactory.Width = imgSize;
            imgFactory.Height = imgSize;
            imgFactory.HorizontalAlignment = HorizontalAlignment.Center;
            imgFactory.VerticalAlignment = VerticalAlignment.Center;

            wrapFactory.Children.Add(imgFactory);
            imgFactoryCreated = true;
        }


        //all functionality for Bank object
        bool isMouseOverBank = false;

        private void BankP_MouseEnter(object sender, MouseEventArgs e)
        {
            //upgrade hover effect in
            isMouseOverBank = true;
            BankVerify();
        }

        private void BankP_MouseLeave(object sender, MouseEventArgs e)
        {
            //upgrade hover effect out
            isMouseOverBank = false;
            BankVerify();
        }

        private bool bankPVisibilityChanged = false;

        private void BankVerify()
        {
            //verify if cookie count is high enough to purchase Bank


            BankP.Visibility = Visibility.Visible;
            bankPVisibilityChanged = true;
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
            if (cookies >= bankCost && bankPVisibilityChanged == false)
            {
                BankBorder.Visibility = Visibility.Visible;
                bankPVisibilityChanged = true;
            }
            else
            {

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
            //increase cost and increase production for scaling
            cookies = cookies - bankCost;
            bankCost = bankCost * costMultiplier;
            cookiesPerSecond = cookiesPerSecond + bankProduction;
            bankProduction = bankProduction * prodMultiplier;
            //round cost of production
            bankCost = Math.Round(bankCost);
            double bankProductionRounded = Math.Round(bankProduction, 2);
            LblBankProd.Content = bankProductionRounded + "/s";
            //show investment in middle
            ClickerWrap();
            GrandmaWrap();
            FarmWrap();
            MineWrap();
            FactoryWrap();
            BankWrap();
            BankImageSpawn();

            scrollBank.Visibility = Visibility.Visible;
        }
        private bool wrapPanelBankCreated = false;
        private bool scrollviewerBankCreated = false;
        private bool imgBankCreated = false;
        private ScrollViewer scrollBank;
        private WrapPanel wrapBank;

        private void BankWrap()
        {
            //make bank UI elemnt in middle of screen and create children if needed

            if (!scrollviewerBankCreated)
            {
                scrollBank = new ScrollViewer();
                scrollBank.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollBank.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Insert(5, scrollBank);
                scrollviewerBankCreated = true;
            }

            if (!wrapPanelBankCreated)
            {
                wrapBank = new WrapPanel();
                wrapBank.Height = wrapHeight;
                wrapBank.Background = new SolidColorBrush(Colors.YellowGreen);

                if (scrollBank != null)
                {
                    if (imgClickerCreated == false)
                    { scrollClicker.Visibility = Visibility.Collapsed; }
                    if (imgGrandmaCreated == false)
                    { scrollGrandma.Visibility = Visibility.Collapsed; }
                    if (imgFarmCreated == false)
                    { scrollFarm.Visibility = Visibility.Collapsed; }
                    if (imgMineCreated == false)
                    { scrollMine.Visibility = Visibility.Collapsed; }
                    if (imgFactoryCreated == false)
                    { scrollFactory.Visibility = Visibility.Collapsed; }
                    scrollBank.Content = wrapBank;
                    wrapPanelBankCreated = true;
                }
            }
        }
        private void BankImageSpawn()
        {
            //create image element inside the wrap UI element

            Image imgBank = new Image();
            imgBank.Source = new BitmapImage(new Uri("bank.png", UriKind.RelativeOrAbsolute));
            imgBank.Width = imgSize;
            imgBank.Height = imgSize;
            imgBank.HorizontalAlignment = HorizontalAlignment.Center;
            imgBank.VerticalAlignment = VerticalAlignment.Center;

            wrapBank.Children.Add(imgBank);
            imgBankCreated = true;
        }

        //all functionality for Temple object
        bool isMouseOverTemple = false;

        private void TempleP_MouseEnter(object sender, MouseEventArgs e)
        {
            //upgrade hover effect in
            isMouseOverTemple = true;
            TempleVerify();
        }

        private void TempleP_MouseLeave(object sender, MouseEventArgs e)
        {
            //upgrade hover effect out
            isMouseOverTemple = false;
            TempleVerify();
        }

        private bool templePVisibilityChanged = false; 
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

            if (cookies >= templeCost && templePVisibilityChanged == false)
            {
                TempleBorder.Visibility = Visibility.Visible;
                templePVisibilityChanged = true;
            }
            else
            {

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
            //increase cost and increase production for scaling
            cookies = cookies - templeCost;
            templeCost = templeCost * costMultiplier;
            cookiesPerSecond = cookiesPerSecond + templeProduction;
            templeProduction = templeProduction * prodMultiplier;
            //round cost of production
            templeCost = Math.Round(templeCost);
            double templeProductionRounded = Math.Round(templeProduction, 2);
            LblTempleProd.Content = templeProductionRounded + "/s";
            //show investment in middle
            ClickerWrap();
            GrandmaWrap();
            FarmWrap();
            MineWrap();
            FactoryWrap();
            BankWrap();
            TempleWrap();
            TempleImageSpawn();
            scrollTemple.Visibility = Visibility.Visible;
        }
        private bool wrapPanelTempleCreated = false;
        private bool scrollviewerTempleCreated = false;
        private bool imgTempleCreated = false;
        private ScrollViewer scrollTemple;
        private WrapPanel wrapTemple;


        private void TempleWrap()
        {
            //make temple UI elemnt in middle of screen and create children if needed

            if (!scrollviewerTempleCreated)
            {
                scrollTemple = new ScrollViewer();
                scrollTemple.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                scrollTemple.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
                StackMain.Children.Insert(6, scrollTemple);
                scrollviewerTempleCreated = true;
            }

            if (!wrapPanelTempleCreated)
            {
                wrapTemple = new WrapPanel();
                wrapTemple.Height = wrapHeight;
                wrapTemple.Background = new SolidColorBrush(Colors.GreenYellow);

                if (scrollTemple != null)
                {
                    if (imgClickerCreated == false)
                    { scrollClicker.Visibility = Visibility.Collapsed; }
                    if (imgGrandmaCreated == false)
                    { scrollGrandma.Visibility = Visibility.Collapsed; }
                    if (imgFarmCreated == false)
                    { scrollFarm.Visibility = Visibility.Collapsed; }
                    if (imgMineCreated == false)
                    { scrollMine.Visibility = Visibility.Collapsed; }
                    if (imgFactoryCreated == false)
                    { scrollFactory.Visibility = Visibility.Collapsed; }
                    if (imgBankCreated == false)
                    { scrollBank.Visibility = Visibility.Collapsed; }
                    scrollTemple.Content = wrapTemple;
                    wrapPanelTempleCreated = true;
                }
            }
        }
        private void TempleImageSpawn()
        {
            //create image element inside the wrap UI element

            Image imgTemple = new Image();
            imgTemple.Source = new BitmapImage(new Uri("temple.png", UriKind.RelativeOrAbsolute));
            imgTemple.Width = imgSize;
            imgTemple.Height = imgSize;
            imgTemple.HorizontalAlignment = HorizontalAlignment.Center;
            imgTemple.VerticalAlignment = VerticalAlignment.Center;

            wrapTemple.Children.Add(imgTemple);
            imgTempleCreated = true;
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

        private bool upgrade1isVisable = false;
        private bool upgrade2isVisable = false;
        private bool upgrade3isVisable = false;
        private bool upgrade4isVisable = false;
        private bool upgrade5isVisable = false;
        private bool upgrade6isVisable = false;
        private bool upgrade7isVisable = false;

        private void UpgradeUnlock()
        {
            //verifies if u have enough cookies to purchase upgrade.

            //upgrade 1 Cursor
            if (cookies > cookieCostUpgradeCursor && upgrade1isVisable == false)
            {
                BorderUpgradeCursor.Visibility = Visibility.Visible;
                upgrade1isVisable = true;
               
            }
            else
            {

            }
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

            if (cookies > cookieCostUpgradeClicker && upgrade2isVisable == false)
            {
                BorderUpgradeClicker1.Visibility = Visibility.Visible;
                upgrade2isVisable = true;

            }
            else
            {

            }
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

            if (cookies > cookieCostUpgradeGrandma && upgrade3isVisable == false)
            {
                BorderUpgradeGrandma.Visibility = Visibility.Visible;
                upgrade3isVisable = true;

            }
            else
            {

            }

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
            if (cookies > cookieCostUpgradeFarm && upgrade4isVisable == false)
            {
                BorderUpgradeFarm.Visibility = Visibility.Visible;
                upgrade4isVisable = true;

            }
            else
            {

            }
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
            if (cookies > cookieCostUpgradeMine && upgrade5isVisable == false)
            {
                BorderUpgradeMine.Visibility = Visibility.Visible;
                upgrade5isVisable = true;

            }
            else
            {

            }
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
            else
            {
                if (cookies < cookieCostUpgradeMine)
                {
                    Upgrade5.IsEnabled = false;
                    Upgrade5.Background = new SolidColorBrush(Colors.SaddleBrown);
                    LblUpgrade5.Foreground = new SolidColorBrush(Colors.Wheat);
                }
            }



            //upgrade 6 Clicker2
            if (cookies > cookieCostUpgradeClicker2 && upgrade6isVisable == false)
            {
                BorderUpgradeClicker2.Visibility = Visibility.Visible;
                upgrade6isVisable = true;

            }
            else
            {

            }
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
            if (cookies > cookieCostUpgradeFactory && upgrade7isVisable == false)
            {
                BorderUpgradeFactory.Visibility = Visibility.Visible;
                upgrade7isVisable = true;

            }
            else
            {

            }
            if (cookies > cookieCostUpgradeFactory && upgradeFactoryCount != 1)
            {
                BorderUpgradeFactory.Visibility = Visibility.Visible;

                if (cookies < cookieCostUpgradeFactory)
                {
                    Upgrade7.IsEnabled = false;
                    Upgrade7.Background = new SolidColorBrush(Colors.SaddleBrown);
                    LblUpgrade7.Foreground = new SolidColorBrush(Colors.Wheat);
                }
                if (isMouseOverUpgrade7)
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
            else
            {
                if (cookies < cookieCostUpgradeFactory)
                {
                    Upgrade7.IsEnabled = false;
                    Upgrade7.Background = new SolidColorBrush(Colors.SaddleBrown);
                    LblUpgrade7.Foreground = new SolidColorBrush(Colors.Wheat);
                }
            }


        }

        //Cursor upgrade
        private void Upgrade1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            upgradeCursorCount++;

            clickCount = clickCount * upgradeCursorLevel;
            cookies = cookies - cookieCostUpgradeCursor;
            cookieCostUpgradeCursor = cookieCostUpgradeCursor * upgradeCursorCostMultiplier;
            if (upgradeCursorCount >= 5)
            {
                upgradeCursorLevel = 3;
            }
            LblUpgrade1.Content = upgradeCursorLevel + "x" + " Cursor";

        }

        //Clicker upgrade
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

        //Grandma upgrade
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

        //Farm upgrade

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

        //Mine upgrade

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

        //Clicker2 upgrade

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

        //Factory upgrade

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

        private void LblBakeryName_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string newName = Microsoft.VisualBasic.Interaction.InputBox("Enter the new name for the bakery:", "Change Bakery Name", LblBakeryName.Content.ToString());

            // Check if the user clicked 'Cancel'
            if (!string.IsNullOrEmpty(newName))
            {
                LblBakeryName.Content = newName;
            }
        }
        private bool foundGoldenCookie = false;
        double achievementScore = 0;
        private bool achievement1Achieved = false;
        private bool achievement2Achieved = false;
        private bool achievement3Achieved = false;
        private bool achievement4Achieved = false;
        private bool achievement5Achieved = false;
        private bool achievement6Achieved = false;
        private bool achievement7Achieved = false;
        private bool achievement8Achieved = false;
        private bool achievement9Achieved = false;
        private bool achievement10Achieved = false;
        private bool achievement11Achieved = false;
        private bool achievement12Achieved = false;
        private bool achievement13Achieved = false;
        private bool achievement14Achieved = false;
        private bool achievement15Achieved = false;
        private bool achievement16Achieved = false;
        private bool achievement17Achieved = false;
        private bool achievement18Achieved = false;
        private bool achievement19Achieved = false;
        private bool achievement20Achieved = false;

        private bool achievementsOpened = false;
        private void AchievemntsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (achievementsOpened == false)
            {
                AchievementsScrollviewer.Visibility = Visibility.Visible;
                AchievementsP.Visibility = Visibility.Visible;
                PlayAchievementSound();
                LblScore.Content = "Score: " + achievementScore.ToString();
                achievementsOpened = true;

            }
            else if (achievementsOpened == true)
            {
                AchievementsScrollviewer.Visibility = Visibility.Collapsed;
                AchievementsP.Visibility = Visibility.Collapsed;
                achievementsOpened = false;
            }
        }

        private void VerifyAchievements()
        {
            if (clicks >= 1 && achievement1Achieved == false)
            {
                achievement1Achieved = true;

                if (achievement1Achieved)
                {
                    achievementScore += 50;
                    Achievement1.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: You made a cookie! (clicked for the first time - 50)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (clicks >= 100 && achievement2Achieved == false)
            {
                achievement2Achieved = true;

                if (achievement2Achieved)
                {
                    achievementScore += 250;
                    Achievement2.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: You clicked 100 times! (100 clicks - 250)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (clicks >= 1000 && achievement3Achieved == false)
            {
                achievement3Achieved = true;

                if (achievement3Achieved)
                {
                    achievementScore += 2500;
                    Achievement3.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Thats 1000 times.. Are you okay? (1000 clicks - 2500)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (cookiesPerSecond >= 1 && achievement4Achieved == false)
            {
                achievement4Achieved = true;

                if (achievement4Achieved)
                {
                    achievementScore += 100;
                    Achievement4.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Production expands! (1 cookies per second - 100)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (cookiesPerSecond >= 10 && achievement5Achieved == false)
            {
                achievement5Achieved = true;

                if (achievement5Achieved)
                {
                    achievementScore += 250;
                    Achievement5.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: More than 100 per second! (10 cookies per second - 250)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (cookiesPerSecond >= 100 && achievement6Achieved == false)
            {
                achievement6Achieved = true;

                if (achievement6Achieved)
                {
                    achievementScore += 500;
                    Achievement6.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Thats a lot of cookies! (100 cookies per second - 500)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (cookiesPerSecond >= 1000 && achievement7Achieved == false)
            {
                achievement7Achieved = true;

                if (achievement7Achieved)
                {
                    achievementScore += 1000;
                    Achievement7.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Industrial Revolution! (1000 cookies per second - 1000)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (cookiesPerSecond >= 10000 && achievement8Achieved == false)
            {
                achievement8Achieved = true;

                if (achievement8Achieved)
                {
                    achievementScore += 5000;
                    Achievement8.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Industrial Revolution! (10000 cookies per second - 5000)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (imgClickerCreated == true && achievement9Achieved == false)
            {
                achievement9Achieved = true;

                if (achievement9Achieved)
                {
                    achievementScore += 100;
                    Achievement9.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: You bought your first investment! (Bought first investment - 100)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (imgGrandmaCreated == true && achievement10Achieved == false)
            {
                achievement10Achieved = true;
                if (achievement10Achieved)
                {
                    achievementScore += 150;
                    Achievement10.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Use grandma's as slaves! (Bought a grandma - 150)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (imgFarmCreated == true && achievement11Achieved == false)
            {
                achievement11Achieved = true;
                if (achievement11Achieved)
                {
                    achievementScore += 200;
                    Achievement11.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Agricultural Revolution! (Bought a farm - 200)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (imgMineCreated == true && achievement12Achieved == false)
            {
                achievement12Achieved = true;
                if (achievement12Achieved)
                {
                    achievementScore += 250;
                    Achievement12.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Cookies discovered underground! (Bought a mine - 250)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (imgFactoryCreated == true && achievement13Achieved == false)
            {
                achievement13Achieved = true;
                if (achievement13Achieved)
                {
                    achievementScore += 300;
                    Achievement13.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Churning out cookies! (Bought a factory - 300)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (imgBankCreated == true && achievement14Achieved == false)
            {
                achievement14Achieved = true;
                if (achievement14Achieved)
                {
                    achievementScore += 350;
                    Achievement14.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Cookies being traded in stocks! (Bought a bank - 350)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (imgTempleCreated == true && achievement15Achieved == false)
            {
                achievement15Achieved = true;
                if (achievement15Achieved)
                {
                    achievementScore += 400;
                    Achievement15.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Cookies being worshipped by millions! (Bought a temple - 400)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if ((upgradeCursorCount >= 1 || upgradeClicker2Count >= 1 || upgradeGrandmaCount >= 1 || upgradeFarmCount >= 1 || upgradeMineCount >= 1 || upgradeFactoryCount >= 1 || upgradeClickerCount >= 1) && achievement16Achieved == false)
            {
                achievement16Achieved = true;
                if (achievement16Achieved)
                {
                    achievementScore += 100;
                    Achievement16.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: You discovered upgrades! (Bought an upgrade - 200)");
                    LblScore.Content = "Score: " + achievementScore.ToString();
                }
            }

            if (seconds >= 600 && achievement17Achieved == false)
            {
                achievement17Achieved = true;
                if (achievement17Achieved)
                {
                    achievementScore += 100;
                    Achievement17.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Having fun? (Played for 10 minutes - 100)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (seconds >= 3600 && achievement18Achieved == false)
            {
                achievement18Achieved = true;
                if (achievement18Achieved)
                {
                    achievementScore += 500;
                    Achievement18.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: REALLY having fun?! (Played for 60 minutes - 500)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (achievementsOpened == true && achievement19Achieved == false)
            {
                achievement19Achieved = true;
                if (achievement19Achieved)
                {
                    achievementScore += 50;
                    Achievement19.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Here we are! (Opened achievements - 50)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }
            if (foundGoldenCookie == true && achievement20Achieved == false)
            {
                achievement20Achieved = true;
                if (achievement20Achieved)
                {
                    achievementScore += 1000;
                    Achievement20.Visibility = Visibility.Visible;
                    PlayAchievementSound();
                    MessageBox.Show("Achievement: Enjoy!! (Found first golden cookie - 1000)");
                    LblScore.Content = "Score: " + achievementScore.ToString();

                }
            }


        }

    }

}
