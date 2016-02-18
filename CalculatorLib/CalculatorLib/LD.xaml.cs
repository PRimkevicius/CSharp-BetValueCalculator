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
    /// Interaction logic for LD.xaml
    /// </summary>
    public partial class LD : Window
    {
        DefautlVariables DVar;
        MainOperations Main;

        private bool state;
        public bool State // true = lay; false = dutch;
        {
            set
            {
                state = value;
                if (state)
                {
                    LabelType.Content = "LAY";
                    TextBoxComm.Visibility = System.Windows.Visibility.Visible;

                }
                else
                {
                    LabelType.Content = "BACK";
                    TextBoxComm.Visibility = System.Windows.Visibility.Hidden;
                }

            }
            get { return state; }
        }
        private double commisions = 1;
        public LD(DefautlVariables DVar)
        {
            this.DVar = DVar;
            InitializeComponent();

            Main = new MainOperations(this, DVar);
            this.State = DVar.State;

            if (DVar.BGBrush1 == null) DVar.BGBrush1 = new SolidColorBrush(Brushes.LightGray.Color);

            this.Background = DVar.BGBrush1;
            this.Background.Opacity = (double)DVar.Opacity / 100;

            TextBoxStake.Text = DVar.Stake.ToString();

            TextBoxBack.Text = DVar.koef1 == 0 ? "2" :DVar.koef1.ToString();
            TextBoxLay.Text = DVar.koef2 == 0 ? "2" : DVar.koef2.ToString();
            TextBoxComm.Text = commisions.ToString();
            commisions /= 100;

            TextBoxStake.KeyDown += TextBoxStake_KeyDown;
            TextBoxBack.KeyDown += TextBoxStake_KeyDown;
            TextBoxLay.KeyDown += TextBoxStake_KeyDown;
            TextBoxStake.TextChanged += TextBox_TextChanged;
            TextBoxBack.TextChanged += TextBox_TextChanged;
            TextBoxLay.TextChanged += TextBox_TextChanged;
            TextBoxComm.KeyDown += TextBoxStake_KeyDown;
            TextBoxComm.TextChanged += TextBoxComm_TextChanged;

            TextBoxAltStake.KeyDown += TextBoxStake_KeyDown;
            TextBoxAltStake.TextChanged += TextBox_TextChanged;

            RBBack.Checked += RB_Checked;
            RBEquel.Checked += RB_Checked;
            RBLay.Checked += RB_Checked;

            if (state) RadioButtonLay.IsChecked = true;
            else RadioButtonDutch.IsChecked = true;
            Recalc();
            this.Topmost = true;
        }

        public void SetValues(string back, string lay)
        {
            TextBoxLay.Text = lay;
            TextBoxBack.Text = back;
            Recalc();
        }


        private void Recalc()
        {
            string buf = TextBoxStake.Text;
            double stake = 10;
            if (buf == "" || !TryToParse.ParseDouble(buf, out stake) || stake == 0) { ShowErrors(); return; }

            double back = 10;
            buf = TextBoxBack.Text;
            if (buf == "" || !TryToParse.ParseDouble(buf, out back) || back == 0) { ShowErrors(); return; }

            double lay = 10;
            buf = TextBoxLay.Text;
            if (buf == "" || !TryToParse.ParseDouble(buf, out lay) || lay == 0) { ShowErrors(); return; }



            double laystake = stake;

            if (RBEquel.IsChecked.Value)
            {
                laystake = !State ? Math.Round(back * stake / lay, 2) : lay < 2 ? Math.Round(back * stake / ((1 + commisions) * lay - commisions * 2), 2) : Math.Round(back * stake / lay, 2);
            }
            if (RBBack.IsChecked.Value)
            {
                laystake = !State ? Math.Round(stake / (lay - 1), 2) : Math.Round((1 + commisions + commisions / 100) * stake, 2);
            }
            if (RBLay.IsChecked.Value)
            {
                laystake = !State ? Math.Round(back * stake - stake, 2) : lay < 2 ? Math.Round(stake * (back - 1) / ((1 + commisions) * lay - (1 + commisions)), 2) : Math.Round(stake * (back - 1) / (lay - 1 + commisions), 2);
            }


            double comm = lay < 2 ? (lay - 1) * laystake * commisions : laystake * commisions;

            LabelBackPL.Content = State ? Math.Round(stake * (back - 1) - (lay - 1) * laystake - comm, 2).ToString() : Math.Round(stake * (back - 1) - laystake, 2).ToString();
            LabelLayPL.Content = State ? Math.Round(laystake - stake - laystake * commisions, 2).ToString() : Math.Round(laystake * (lay - 1) - stake, 2).ToString();
            LabelLayStake.Content = laystake.ToString();

            if (TextBoxAltStake.Text == "") return;
            double altlaystake = 1;
            buf = TextBoxAltStake.Text;
            if (buf == null || !TryToParse.ParseDouble(buf, out altlaystake) || stake == 0) { return; }

            double altcomm = lay < 2 ? (lay - 1) * altlaystake * commisions : altlaystake * commisions;

            LabelAltBackPL.Content = State ? Math.Round(stake * (back - 1) - (lay - 1) * altlaystake - altcomm, 2).ToString() : Math.Round(stake * (back - 1) - altlaystake, 2).ToString();
            LabelAltLayPL.Content = State ? Math.Round(altlaystake - stake - altlaystake * commisions, 2).ToString() : Math.Round(altlaystake * (lay - 1) - stake, 2).ToString();

        }
        private void ShowErrors()
        {
            LabelBackPL.Content = "-";
            LabelLayPL.Content = "-";
            LabelAltBackPL.Content = "-";
            LabelAltLayPL.Content = "-";
            LabelAltLayPL.Content = "-";
            LabelAltBackPL.Content = "-";
            LabelLayStake.Content = "-";
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TextBoxStake.Text == "" || TextBoxBack.Text == "" || TextBoxLay.Text == "") { ShowErrors(); return; }
            Recalc();
        }

        void TextBoxComm_TextChanged(object sender, TextChangedEventArgs e)
        {

            string str = TextBoxComm.Text;
            double value;
            if (str != "" && TryToParse.ParseDouble(str, out value) && value != 0)
            {
                commisions = value / 100;
                Recalc();
            }
            else TextBoxComm.Text = (commisions * 100).ToString();


        }

        private void TextBoxStake_KeyDown(object sender, KeyEventArgs e)
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


        private void Button_StakeClick(object sender, RoutedEventArgs e)
        {
            Button bt = (Button)e.Source;
            TextBoxStake.Text = bt.Content.ToString();
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


        private void Expande(object sender, RoutedEventArgs e)
        {
            if (this.Height == 195)
            {
                this.Height += 65;

            }
            else this.Height -= 65;
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            State = RadioButtonLay.IsChecked.Value;
            Recalc();
        }

        private void RB_Checked(object sender, RoutedEventArgs e)
        {
            Recalc();
        }



        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)e.OriginalSource;
            tb.Dispatcher.BeginInvoke(new Action(delegate
            {
                tb.SelectAll();
            }), System.Windows.Threading.DispatcherPriority.Input);
        }

    }
}
