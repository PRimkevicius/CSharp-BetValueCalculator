using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CalculatorLib
{
    /// <summary>
    /// Interaction logic for _3Way.xaml
    /// </summary>

    public partial class _3Way : Window
    {
        DefautlVariables DVar;
        MainOperations Main;
        public _3Way(DefautlVariables DVar)
        {

            this.DVar = DVar;
            InitializeComponent();
            Main = new MainOperations(this, DVar);

            if (DVar.BGBrush1 == null) DVar.BGBrush1 = new SolidColorBrush(Brushes.LightGray.Color);
            this.Background = DVar.BGBrush1;
            this.Background.Opacity = (double)DVar.Opacity / 100;

            TextBoxStake.Text = DVar.Stake.ToString();
            TextBoxBack1.Text = "2";
            TextBoxBack2.Text = "2";
            TextBoxBack3.Text = "2";

            TextBoxStake.KeyDown += TextBox_KeyDown;
            TextBoxBack1.KeyDown += TextBox_KeyDown;
            TextBoxBack2.KeyDown += TextBox_KeyDown;
            TextBoxBack3.KeyDown += TextBox_KeyDown;
            TextBoxAltStake2.KeyDown += TextBox_KeyDown;
            TextBoxAltStake3.KeyDown += TextBox_KeyDown;


            TextBoxStake.TextChanged += TextBox_TextChanged;
            TextBoxBack1.TextChanged += TextBox_TextChanged;
            TextBoxBack2.TextChanged += TextBox_TextChanged;
            TextBoxBack3.TextChanged += TextBox_TextChanged;
            TextBoxAltStake2.TextChanged += TextBox_TextChanged;
            TextBoxAltStake3.TextChanged += TextBox_TextChanged;

            RBEquel.Checked += RB_Checked;
            RBBack1.Checked += RB_Checked;
            RBBack2.Checked += RB_Checked;
            RBBack3.Checked += RB_Checked;

            Recalc();
            this.Topmost = true;
        }

        private void Recalc()
        {
            string buf = TextBoxStake.Text;
            double stake = 1;
            if (buf == "" || buf == null || !TryToParse.ParseDouble(buf, out stake) || stake == 0) { ShowErrors(); return; }

            double back1 = 1;
            buf = TextBoxBack1.Text;
            if (buf == "" || buf == null || !TryToParse.ParseDouble(buf, out back1) || back1 == 0) { ShowErrors(); return; }

            double back2 = 1;
            buf = TextBoxBack2.Text;
            if (buf == "" || buf == null || !TryToParse.ParseDouble(buf, out back2) || back2 == 0) { ShowErrors(); return; }

            double back3 = 1;
            buf = TextBoxBack3.Text;
            if (buf == "" || buf == null || !TryToParse.ParseDouble(buf, out back3) || back3 == 0) { ShowErrors(); return; }

            double backstake2 = stake;
            double backstake3 = stake;

            if (RBEquel.IsChecked.Value)
            {
                backstake2 = Math.Round(back1 * stake / back2, 2);
                backstake3 = Math.Round(back1 * stake / back3, 2);
            }
            if (RBBack1.IsChecked.Value)
            {
                double value = stake * back3 * (back1 - 1) / (back3 + back2);
                backstake2 = Math.Round(value, 2);
                backstake3 = Math.Round(value * back2 / back3, 2);
            }
            if (RBBack2.IsChecked.Value)
            {
                double value = stake * back1 / back3;
                backstake2 = Math.Round((stake + value) / (back2 - 1), 2);
                backstake3 = Math.Round(value, 2);
            }
            if (RBBack3.IsChecked.Value)
            {
                double value = stake * back1 / back2;
                backstake2 = Math.Round(value, 2);
                backstake3 = Math.Round((stake + value) / (back3 - 1), 2);
            }


            LabelBack1PL.Content = Math.Round(stake * (back1 - 1) - backstake2 - backstake3, 2).ToString();
            LabelBack2PL.Content = Math.Round(backstake2 * (back2 - 1) - stake - backstake3, 2).ToString();
            LabelBack3PL.Content = Math.Round(backstake3 * (back3 - 1) - stake - backstake2, 2).ToString();

            LabelBackStake1.Content = stake.ToString();
            LabelBackStake2.Content = backstake2.ToString();
            LabelBackStake3.Content = backstake3.ToString();

            if (TextBoxAltStake2.Text == "" && TextBoxAltStake3.Text == "") return;

            double altstake2 = 1;
            double altstake3 = 1;
            buf = TextBoxAltStake2.Text;
            if (buf == "" || buf == null || !TryToParse.ParseDouble(buf, out altstake2) || altstake2 == 0) { return; }

            buf = TextBoxAltStake3.Text;
            if (buf == "" || buf == null || !TryToParse.ParseDouble(buf, out altstake3) || altstake3 == 0) { return; }

            LabelAlt1PL.Content = Math.Round(stake * (back1 - 1) - altstake2 - altstake3, 2);
            LabelAlt2PL.Content = Math.Round(altstake2 * (back2 - 1) - stake - altstake3, 2);
            LabelAlt3PL.Content = Math.Round(altstake3 * (back3 - 1) - stake - altstake2, 2);

        }
        private void ShowErrors()
        {
            LabelBack1PL.Content = "-";
            LabelBack2PL.Content = "-";
            LabelBack3PL.Content = "-";
            LabelBackStake1.Content = "-";
            LabelBackStake2.Content = "-";
            LabelBackStake3.Content = "-";
            LabelAlt1PL.Content = "-";
            LabelAlt2PL.Content = "-";
            LabelAlt3PL.Content = "-";
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            string key = e.Key.ToString();
            if (key.Any(char.IsDigit) || key.Contains("Decimal") || key.Contains("Period") || key.Equals("Tab") || key.Contains("Comma"))
            {

                if (key.Contains("Decimal") || key.Contains("Period") || key.Contains("Comma"))
                {
                    TextBox tb = (TextBox)e.Source;
                    string str = tb.Text;
                    str = str.Replace(',', '.');
                    if (str.Count(c => c == '.') > 0) { e.Handled = true; return; }
                }
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxStake.Text == "" || TextBoxBack1.Text == "" || TextBoxBack2.Text == "" || TextBoxBack3.Text == "") { ShowErrors(); return; }
            Recalc();
        }



        private void Button_Increase(object sender, RoutedEventArgs e)
        {
            RepeatButton b = (RepeatButton)e.Source;
            TextBox tb = (TextBox)this.FindName(b.Tag.ToString());
            double value = 1;

            if (tb.Text != "" && TryToParse.ParseDouble(tb.Text, out value) && value != 0)
            {
                value += (double)1 / 100;
            }
            tb.Text = value.ToString();
        }

        private void Button_Decrease(object sender, RoutedEventArgs e)
        {
            RepeatButton b = (RepeatButton)e.Source;
            TextBox tb = (TextBox)this.FindName(b.Tag.ToString());
            double value = 1;
            if (tb.Text != "" && TryToParse.ParseDouble(tb.Text, out value) && value != 0)
            {
                value -= (double)1 / 100;
                if (value <= 1) value = 1;
            }
            tb.Text = value.ToString();
        }
        private void SelectAllTextInTextBox(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.SelectAll();
        }
        private void Button_Click_TopMost(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)sender;
            if (this.Topmost) { this.Topmost = false; bt.Background = Brushes.Gray; }
            else { this.Topmost = true; bt.Background = Brushes.Red; }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)e.OriginalSource;
            tb.Dispatcher.BeginInvoke(new Action(delegate
            {
                tb.SelectAll();
            }), System.Windows.Threading.DispatcherPriority.Input);
        }

        private void RB_Checked(object sender, RoutedEventArgs e)
        {
            Recalc();
        }
        private void Expande(object sender, RoutedEventArgs e)
        {
            if (this.Height == 175)
            {
                this.Height += 65;
            }
            else this.Height -= 65;
        }
    }
}
