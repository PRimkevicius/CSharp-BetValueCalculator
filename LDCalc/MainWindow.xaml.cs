using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows;
//using System.Windows.Forms;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Drawing;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using CalculatorLib;

namespace BetCalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            if (BetCalc.Properties.Settings.Default.Transparency == 1)
            {
                BetCalc.Properties.Settings.Default.Transparency = 70;
                BetCalc.Properties.Settings.Default.Save();
            }
            if (BetCalc.Properties.Settings.Default.Color1.ToString() == "")
            {
                BetCalc.Properties.Settings.Default.Color1 = System.Drawing.Color.LightGray;
                BetCalc.Properties.Settings.Default.Save();
            }
            if (BetCalc.Properties.Settings.Default.Color2.ToString() == "")
            {
                BetCalc.Properties.Settings.Default.Color2 = System.Drawing.Color.LightBlue;
                BetCalc.Properties.Settings.Default.Save();
            }

            InitializeComponent();

            //   System.Windows.Media.Brush imageColor = new SolidColorBrush(BetCalc.Properties.Settings.Default.Color2)
            //Color1.Fill = new SolidColorBrush(BetCalc.Properties.Settings.Default.Color1);
            //Color2.Fill = new SolidColorBrush(BetCalc.Properties.Settings.Default.Color2);

            Color1.Fill = new SolidColorBrush(Colors.LightGray);
            Color2.Fill = new SolidColorBrush(Colors.LightBlue);

            System.Windows.Forms.NotifyIcon ni = new System.Windows.Forms.NotifyIcon();

            System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();



            contextMenu.MenuItems.Add("E&xit", CloseApplication);
            contextMenu.MenuItems.Add("&Show", ShowApplication);
            contextMenu.MenuItems.Add("-");
            contextMenu.MenuItems.Add("F&reeBet", OpenFreeBetCalc);
            contextMenu.MenuItems.Add("3&Way", Open3WayCalc);
            contextMenu.MenuItems.Add("&Dutch", OpenDutchCalc);
            contextMenu.MenuItems.Add("&Lay", OpenLayCalc);

            ni.ContextMenu = contextMenu;


            ni.Icon = new System.Drawing.Icon("icon.ico");
            ni.Visible = true;
            ni.DoubleClick +=
                delegate(object sender, EventArgs args)
                {
                    this.Show();
                    this.WindowState = WindowState.Normal;
                };


            if (BetCalc.Properties.Settings.Default.Minimized)
            {
                this.Hide();
            }

        }
        private void OpenCalc(bool value)
        {
            DefautlVariables DVar = new DefautlVariables();
            DVar.State = value;
            DVar.Stake = BetCalc.Properties.Settings.Default.DefaultStake;
            DVar.Opacity = BetCalc.Properties.Settings.Default.Transparency;
            DVar.ComBf = BetCalc.Properties.Settings.Default.CommBf;
            DVar.ComBook = BetCalc.Properties.Settings.Default.CommBook;
            System.Drawing.Color color = BetCalc.Properties.Settings.Default.Color1;
            DVar.BGBrush1 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R,color.G,color.B));
            color = BetCalc.Properties.Settings.Default.Color2;
            DVar.BGBrush2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
            LD win = new LD(DVar);
            win.Show();
        }
        private void Open3WayCalc()
        {
            DefautlVariables DVar = new DefautlVariables();
            DVar.Stake = BetCalc.Properties.Settings.Default.DefaultStake;
            DVar.Opacity = BetCalc.Properties.Settings.Default.Transparency;
            DVar.ComBf = BetCalc.Properties.Settings.Default.CommBf;
            DVar.ComBook = BetCalc.Properties.Settings.Default.CommBook;
            System.Drawing.Color color = BetCalc.Properties.Settings.Default.Color1;
            DVar.BGBrush1 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
            color = BetCalc.Properties.Settings.Default.Color2;
            DVar.BGBrush2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
            _3Way win = new _3Way(DVar);
            win.Show();
        }
        private void OpenFreeBetCalc()
        {
            DefautlVariables DVar = new DefautlVariables();
            DVar.State = true;
            DVar.Stake = BetCalc.Properties.Settings.Default.DefaultStake;
            DVar.Opacity = BetCalc.Properties.Settings.Default.Transparency;
            DVar.ComBf = BetCalc.Properties.Settings.Default.CommBf;
            DVar.ComBook = BetCalc.Properties.Settings.Default.CommBook;
            System.Drawing.Color color = BetCalc.Properties.Settings.Default.Color1;
            DVar.BGBrush1 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));
            color = BetCalc.Properties.Settings.Default.Color2;
            DVar.BGBrush2 = new SolidColorBrush(System.Windows.Media.Color.FromRgb(color.R, color.G, color.B));

            FreeBet win = new FreeBet(DVar);
            win.Show();
        }

        private void OpenDutchCalc(object sender, EventArgs e) { OpenCalc(false); }
        private void OpenLayCalc(object sender, EventArgs e) { OpenCalc(true); }
        private void Open3WayCalc(object sender, EventArgs e) { Open3WayCalc(); }
        private void OpenFreeBetCalc(object sender, EventArgs e) { OpenFreeBetCalc(); }



        private void ButtonLay_Click(object sender, RoutedEventArgs e) { OpenCalc(true); }
        private void ButtonDutch_Click(object sender, RoutedEventArgs e) { OpenCalc(false); }
        private void Button3Way_Click(object sender, RoutedEventArgs e) { Open3WayCalc(); }
        private void ButtonFreeBet_Click(object sender, RoutedEventArgs e) { OpenFreeBetCalc(); }


        private void CloseApplication(object sender, EventArgs e)
        {
            this.Close();
        }
        private void ShowApplication(object sender, EventArgs e)
        {
            //if (this.WindowState == WindowState.Minimized) { this.WindowState = WindowState.Normal; this.Show(); }
            //if (this.Visibility != Visibility.Visible) { this.WindowState = WindowState.Normal;  this.Show();  }

            if (this.Visibility == Visibility.Hidden)
            {
                this.Visibility = Visibility.Visible;
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                    new Action(delegate()
                    {
                        this.WindowState = WindowState.Normal;
                        this.Activate();
                    })
                );
            }
            //this.WindowState = WindowState.Normal; this.Show();
            //this.Activate();
        }

        private void tb_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string key = e.Key.ToString();

            if (key.Any(char.IsDigit) || key.Contains("Decimal") || key.Contains("Period"))
            {
                if (key.Contains("Decimal") || key.Contains("Period"))
                {
                    System.Windows.Controls.TextBox tb = (System.Windows.Controls.TextBox)e.Source;
                    string str = tb.Text;
                    if (str.Count(c => c == '.') > 0) { e.Handled = true; return; }
                }
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }



        private void Window_Closing(object sender, CancelEventArgs e)
        {
            BetCalc.Properties.Settings.Default.Save();
            Environment.Exit(0);
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (this.WindowState == WindowState.Minimized) { this.Hide(); return; }
        }



        private void Button_ClickColor(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)e.Source;
            ContextMenu menu = new ContextMenu();
            ColorPicker CP = new ColorPicker();
            int nr = 0;
            foreach (SolidColorBrush item in CP.BrushList)
            {
                MenuItem it = new MenuItem();
                it.Background = item;
                it.Click += it_ClickContextMenuItem;
                it.Tag = bt.Tag;
                menu.Items.Add(it);
                nr++;
            }
            bt.ContextMenu = menu;
            bt.ContextMenu.IsOpen = true;
        }

        private void it_ClickContextMenuItem(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)e.Source;

            if (item.Tag.ToString() == "1")
            {
                Color1.Fill = item.Background;
                SolidColorBrush brush = (SolidColorBrush)item.Background;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
                BetCalc.Properties.Settings.Default.Color1 = color;
            }
            if (item.Tag.ToString() == "2")
            {
                Color2.Fill = item.Background;
                SolidColorBrush brush = (SolidColorBrush)item.Background;
                System.Drawing.Color color = System.Drawing.Color.FromArgb(brush.Color.A, brush.Color.R, brush.Color.G, brush.Color.B);
                BetCalc.Properties.Settings.Default.Color2 = color;
            }
            BetCalc.Properties.Settings.Default.Save();
        }

    }


   

}
