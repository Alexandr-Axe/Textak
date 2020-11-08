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
    /// Interakční logika pro NajdiVTextuOkno.xaml
    /// </summary>
    public partial class NajdiVTextuOkno : Window
    {
        TextBox TBHlavni;
        bool proslo;
        int pozice;
        public NajdiVTextuOkno(TextBox hlavni, bool projde, int position)
        {
            InitializeComponent();
            TBSearch.GotKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_GotKeyboardFocus);
            TBSearch.LostKeyboardFocus += new KeyboardFocusChangedEventHandler(txt_LostKeyboardFocus);
            TBHlavni = hlavni;
            proslo = projde;
            pozice = position;
            MW.panelVyhledat = true;
        }
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

        MainWindow MW = new MainWindow();
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TBHlavni.Text.Contains(TBSearch.Text))
            {
                switch (((Button)sender).Name)
                {
                    case "btnSearch":
                        TBHlavni.Focus();
                        if (!proslo)
                        {
                            int loc = TBHlavni.Text.IndexOf(TBSearch.Text, StringComparison.OrdinalIgnoreCase); //Stejné jako ToLower(), jen rychlejší
                            TBHlavni.SelectionStart = loc;
                            TBHlavni.SelectionLength = TBSearch.Text.Length;
                            if (TBHlavni.Text.IndexOf(TBSearch.Text, loc + 1) == -1) loc = 0;
                            else loc = TBHlavni.Text.IndexOf(TBSearch.Text, loc + 1);
                            pozice = loc;
                            proslo = true;
                        }
                        else
                        {
                            TBHlavni.SelectionStart = pozice;
                            TBHlavni.SelectionLength = TBSearch.Text.Length;
                            if (TBHlavni.Text.IndexOf(TBSearch.Text, pozice + 1) == -1) { pozice = 0; proslo = false; }
                            else pozice = TBHlavni.Text.IndexOf(TBSearch.Text, pozice + 1);
                        }
                        
                        break;
                    case "btnSwap":
                            TBHlavni.Text = TBHlavni.Text.Replace(TBSearch.Text, "parta");
                            TBHlavni.Focus();
                            TBHlavni.SelectionStart = TBHlavni.Text.Length;
                            TBHlavni.SelectionLength = 0;
                        break;
                    default:
                        break;
                }
                MW.TBHlavni = TBHlavni;
            }
            else MessageBox.Show("V textu se nenachází hledané slovo!", "WOOOOW");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            MW.panelVyhledat = false;
        }
    }
}
