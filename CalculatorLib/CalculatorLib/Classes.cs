using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;


namespace CalculatorLib
{
    public struct DefautlVariables
    {
        public double Stake;
        public double Opacity;
        public double ComBook;
        public double ComBf;
        public SolidColorBrush BGBrush1;
        public SolidColorBrush BGBrush2;
        public bool State;
        public double koef1;
        public double koef2;
    }
    public class ColorPicker
    {
        public List<SolidColorBrush> BrushList = new List<SolidColorBrush>();
        public ColorPicker()
        {
            BrushList.Add(new SolidColorBrush(Brushes.LightBlue.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightCoral.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightCyan.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightGoldenrodYellow.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightGray.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightGreen.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightPink.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightSalmon.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightSeaGreen.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightSkyBlue.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightSlateGray.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightSteelBlue.Color));
            BrushList.Add(new SolidColorBrush(Brushes.LightYellow.Color));

        }
    }
    class MainOperations
    {

        Window MainW;
        DefautlVariables DVar;
      public  MainOperations(Window MainWindow, DefautlVariables DVar)
        {
            this.DVar = DVar;
            MainW = MainWindow;

          CreateContextMenuItems();
        }
        public void CreateContextMenuItems()
        {
            MenuItem I1 = new MenuItem() { Header="Close" };
            I1.Click += MenuItem_CloseClick;


            MenuItem I2 = new MenuItem() { Header = "New Lay" };
            I2.Click += MenuItem_ClickNew;

            MenuItem I3 = new MenuItem() { Header = "New Dutch" };
            I3.Click += MenuItem_ClickNew;

            MenuItem I4 = new MenuItem() { Header = "New 3Way" };
            I4.Click += MenuItem_ClickNew;

            MenuItem I5 = new MenuItem() { Header = "Free Bet" };
            I5.Click += MenuItem_ClickNew;

            MenuItem I6 = new MenuItem() { Header = "BG Color" };

            ColorPicker CP = new ColorPicker();
            int nr = 0;
            foreach(SolidColorBrush item in CP.BrushList)
            {
                MenuItem it = new MenuItem();
                it.Background = item;
                it.Click+=it_ClickMenuItemColor;
                it.Tag = nr;

                I6.Items.Add(it);
                nr++;
            }

            ContextMenu menu = new ContextMenu();
            menu.Items.Add(I1);
            menu.Items.Add(new Separator());
            menu.Items.Add(I2);
            menu.Items.Add(I3);
            menu.Items.Add(I4);
            menu.Items.Add(I5);
            menu.Items.Add(I6);

            MainW.ContextMenu = menu;
        }

        private void it_ClickMenuItemColor(object sender, RoutedEventArgs e)
        {
            MenuItem item = sender as MenuItem;
            MainW.Background = item.Background;
            MainW.Background.Opacity = (double)DVar.Opacity / 100;
          
        }



        private void MenuItem_ClickNew(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)e.Source;
            Window win = null;
            if (item.Header == "New Lay") { DVar.State = true; win = new LD(DVar); }
            if (item.Header == "New Dutch") { DVar.State = true; win = new LD(DVar); }
            if (item.Header == "New 3Way") win = new _3Way(DVar);
            if (item.Header == "Free Bet") { DVar.State = true; win = new FreeBet(DVar); }
            win.Show();
        }

        private void MenuItem_CloseClick(object sender, RoutedEventArgs e) { MainW.Close(); }



    }


    class TryToParse
    {
        public static bool ParseDouble(string value, out double number)
        {
            number = 0;
            if (!double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out number) &&
                !double.TryParse(value, NumberStyles.Float, CultureInfo.GetCultureInfo("en-US"), out number) &&
                !double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out number))
            { return false; }

            return true;
        }
    }
}
