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
using System.IO;
using System.Text.RegularExpressions;

namespace Textak
{
    /// <summary>
    /// Interakční logika pro EditaceTextu.xaml
    /// </summary>
    public partial class EditaceTextu : Window
    {
        TextBox TBHlavni;
        public EditaceTextu(TextBox Varta)
        {
            InitializeComponent();
            TBHlavni = Varta;
        }
        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
        }
        bool font = false;
        bool height = false;
        bool decoration = false;
        MainWindow MW = new MainWindow();
        private void btnAccept_Click(object sender, RoutedEventArgs e)
        {
            if (LBFonts.SelectedItem != null) font = true;
            if (LBDecorations.SelectedItem != null) decoration = true;
            if (TBFontsize.Text != string.Empty) height = true;
            TBHlavni.FontFamily = new FontFamily($"{(font == true ? LBIFont.Content : TBHlavni.FontFamily)}");
            if (decoration)
            {
                switch (LBIDec.Name)
                {
                    case "Neupravený":
                        TBHlavni.FontStyle = FontStyles.Normal;
                        break;
                    case "Kurzíva":
                        TBHlavni.FontStyle = FontStyles.Italic;
                        break;
                    case "Tučný":
                        TBHlavni.FontWeight = FontWeights.Bold;
                        break;
                    case "TučnýKurzívou":
                        TBHlavni.FontStyle = FontStyles.Italic;
                        TBHlavni.FontWeight = FontWeights.Bold;
                        break;
                }
            }
            if (height) TBHlavni.FontSize = Convert.ToDouble(TBFontsize.Text);
            MW.TBHlavni = TBHlavni;
            Close();
        }
        ListBoxItem LBIDec;
        ListBoxItem LBIFont;
        ListBoxItem LBIHeight;
        private void LBChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListBox)sender).Name)
            {
                case "LBFonts":
                    LBIFont = (ListBoxItem)LBFonts.SelectedItem;
                    TBFonts.Text = LBIFont.Content.ToString();
                    break;
                case "LBDecorations":
                    LBIDec = (ListBoxItem)LBDecorations.SelectedItem;
                    TBDecorations.Text = LBIDec.Content.ToString();
                    break;
                case "LBFontsize":
                    LBIHeight = (ListBoxItem)LBFontsize.SelectedItem;
                    TBFontsize.Text = LBIHeight.Content.ToString();
                    break;
            }
        }
    }
}
