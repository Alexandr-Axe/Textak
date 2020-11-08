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
using System.Windows.Shapes;

namespace Textak
{
    /// <summary>
    /// Interakční logika pro VymenTextOkno.xaml
    /// </summary>
    public partial class VymenTextOkno : Window
    {
        TextBox TBHlavni;
        public VymenTextOkno(TextBox hlavni)
        {
            InitializeComponent();
            TBSearch.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_GotKeyboardFocus);
            TBSearch.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_LostKeyboardFocus);
            TBChange.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_GotKeyboardFocus);
            TBChange.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_LostKeyboardFocus);
            TBHlavni = hlavni;
        }
        MainWindow MW = new MainWindow();
        private void txt_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Foreground == Brushes.Gray)
                {
                    ((TextBox)sender).Text = "";
                    ((TextBox)sender).Foreground = Brushes.Black;
                }
            }
        }
        private void txt_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (sender is TextBox)
            {
                if (((TextBox)sender).Text == "")
                {
                    ((TextBox)sender).Foreground = Brushes.Gray;
                    if (((TextBox)sender).Name == "Search") ((TextBox)sender).Text = "Vyhledat";
                    else if (((TextBox)sender).Name == "Change") ((TextBox)sender).Text = "Změnit";
                }
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TBHlavni.Text.Contains(TBSearch.Text))
            {
                switch (((Button)sender).Name)
                {
                    case "btnSwap":
                        TBHlavni.Text = TBHlavni.Text.Replace(TBSearch.Text, TBChange.Text);
                        TBHlavni.Focus();
                        TBHlavni.SelectionStart = TBHlavni.Text.Length;
                        TBHlavni.SelectionLength = 0;
                        break;
                    default:
                        break;
                }
                MW.TBHlavni = TBHlavni;
                Close();
            }
            else MessageBox.Show("V textu se nenachází hledané slovo!", "WOOOOW");
        }
    }
}
