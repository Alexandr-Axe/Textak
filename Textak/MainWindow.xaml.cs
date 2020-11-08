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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace Textak
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            panelOkno.Height = Height;
            panelOkno.Width = Width;
            TBHlavni.Width = Width - 15;
            TBHlavni.Height = Height - 55;
        }
        public bool Ulozeno = true;
        //public static TextBox TextMainWindow;
        OpenFileDialog ofd = new OpenFileDialog() { Filter = "Textový dokument Glora (*.gte)|*.gte|Poznámkový blok (*.txt)|*.txt|Microsoft Word (*.docx)|*.docx|Všechny soubory (*.*)|*.*" };
        SaveFileDialog sfd = new SaveFileDialog() { Filter = "Textový dokument Glora (*.gte)|*.gte|Poznámkový blok (*.txt)|*.txt|Microsoft Word (*.docx)|*.docx|Všechny soubory (*.*)|*.*" };
        string AdresaSouboru = string.Empty;
        string nazevSouboruTitle = "Nový textový soubor";
        public string TextPoNacteni = "";
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            panelOkno.Height = Height;
            panelOkno.Width = Width;
            TBHlavni.Width = Width - 15;
            TBHlavni.Height = Height - 55;
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

        char lomitko = Convert.ToChar(92);
        private void MenuItem_Soubor_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Pracujici = (MenuItem)sender;
            string[] JmenoSouboru;
            if (Pracujici.Header.ToString() == "Ukončit") Close();
            else if (Pracujici.Header.ToString() == "Nový")
            {
                if (Ulozeno)
                {
                    TBHlavni.Text = "";
                    Title = "Nový textový soubor";
                }
                else
                {
                    MessageBoxResult mbr = MessageBox.Show("Tento soubor nebyl uložen! Přejete si ho uložit?", "Upozornění", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    if (mbr == MessageBoxResult.Yes)
                    {
                        sfd.ShowDialog();
                        try
                        {
                            File.WriteAllText(sfd.FileName, TBHlavni.Text);
                            Ulozeno = true;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        TBHlavni.Text = "";
                        Title = "Nový textový soubor";
                        Ulozeno = true;
                    }
                }
            }
            else if (Pracujici.Header.ToString() == "Otevřít...")
            {
                ofd.ShowDialog();
                AdresaSouboru = $"{ofd.InitialDirectory}{ofd.FileName}";
                if (TBHlavni.Text != TextPoNacteni)
                {
                   MessageBoxResult mbr = MessageBox.Show("Chcete uložit tento soubor?", "Soubor není uložen", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    switch (mbr)
                    {
                        case MessageBoxResult.Yes:
                            sfd.ShowDialog();
                            try
                            {
                                File.WriteAllText(sfd.FileName, TBHlavni.Text);
                                Title = sfd.FileName.Split(lomitko)[sfd.FileName.Split(lomitko).Length - 1];
                            }
                            catch (Exception)
                            {
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
                if (File.Exists(AdresaSouboru))
                {
                    TBHlavni.Text = File.ReadAllText(AdresaSouboru, Encoding.UTF8);
                    JmenoSouboru = ofd.FileName.Split(lomitko);
                    Title = JmenoSouboru[JmenoSouboru.Length - 1];
                    TBHlavni.Focus();
                    TBHlavni.SelectionStart = TBHlavni.Text.Length;
                    TBHlavni.SelectionLength = 0;
                    TextPoNacteni = TBHlavni.Text;
                }

            }
            else if (Pracujici.Header.ToString() == "Uložit")
            {
                if (Title != "Nový textový soubor") File.WriteAllText(AdresaSouboru, TBHlavni.Text);
                else
                {
                    sfd.ShowDialog();
                    try
                    {
                        File.WriteAllText(sfd.FileName, TBHlavni.Text);
                        Title = sfd.FileName.Split(lomitko)[sfd.FileName.Split(lomitko).Length - 1];
                    }
                    catch (Exception)
                    {
                    }
                }
            }
            else if (Pracujici.Header.ToString() == "Tisk...")
            {
                PrintDialog PD = new PrintDialog();
                if ((bool)PD.ShowDialog().GetValueOrDefault())
                {
                    FlowDocument FD = new FlowDocument();
                    foreach (string Odstavec in TBHlavni.Text.Split('\n'))
                    {
                        Paragraph Paragraf = new Paragraph();
                        Paragraf.Margin = new Thickness(0);
                        Paragraf.Inlines.Add(new Run(Odstavec));
                        FD.Blocks.Add(Paragraf);
                    }
                    DocumentPaginator DP = ((IDocumentPaginatorSource)FD).DocumentPaginator;
                    PD.PrintDocument(DP, Title);
                }
            }
        }
        int pomocnyIndex = 0;
        private void MenuItem_Upravy_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Pracujici = (MenuItem)sender;
            if (Pracujici.Header.ToString() == "Formátovat text...") 
            {
                EditaceTextu ET = new EditaceTextu(TBHlavni);
                ET.Show();
            }
            else if (Pracujici.Header.ToString() == "Najít...")
            {
                NajdiVTextuOkno NVTO = new NajdiVTextuOkno(TBHlavni, proslo, pozice);
                NVTO.Show();
            }
            else if (Pracujici.Header.ToString() == "Nahradit...")
            {
                VymenTextOkno VTO = new VymenTextOkno(TBHlavni);
                VTO.Show();
            }
            else if (Pracujici.Header.ToString() == "Vložit obrázek")
            {
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.Filter = "Obrázek JPEG(*.jpg)|*.jpg|Obrázek PNG(*.png)|*.png";
                if (OFD.ShowDialog() == true)
                {
                    Obrazek.Source = new BitmapImage(new Uri(OFD.FileName));
                    Obrazek.Width = 300;
                    Obrazek.Height = 300;
                    Obrazek.Name = $"img{pomocnyIndex++}";
                }
            }
        }
        void Image_MouseDown(object sender, MouseButtonEventArgs e) 
        {
            /*Image img = (Image)sender;
            img.Margin = new Thickness(e.GetPosition(HlavniOkno).X - (img.Width / 2), e.GetPosition(HlavniOkno).Y - (img.Height / 2), e.GetPosition(HlavniOkno).X + (img.Width / 2), e.GetPosition(HlavniOkno).Y + (img.Height / 2));*/
        }
        private void MenuItem_Zobrazeni_Click(object sender, RoutedEventArgs e)
        {
            MenuItem Pracujici = (MenuItem)sender;
            if (Pracujici.Header.ToString() == "+")
            {
                if (TBHlavni.FontSize < 50) TBHlavni.FontSize += 5;
            }
            else if (Pracujici.Header.ToString() == "-")
            {
                if (TBHlavni.FontSize > 10) TBHlavni.FontSize -= 5;
            }
            else if (Pracujici.Header.ToString() == "Obnovit výchozí") 
            {
                TBHlavni.FontSize = 13;
                TBHlavni.FontFamily = new FontFamily("Segoe UI");
                TBHlavni.FontWeight = FontWeights.Normal;
                TBHlavni.FontStyle = FontStyles.Normal;
            }
        }
        private void MenuItem_Cas_Click(object sender, RoutedEventArgs e)
        {
            switch (((MenuItem)sender).Header.ToString())
            {
                case "Zapsat čas":
                    TBHlavni.Text += $"{DateTime.Now.Hour}:{DateTime.Now.Minute}";
                    break;
                case "Zapsat datum":
                    TBHlavni.Text += $"{DateTime.Now.Day}.{DateTime.Now.Month}. {DateTime.Now.Year}";
                    break;
            }
            TBHlavni.Focus();
            TBHlavni.SelectionStart = TBHlavni.Text.Length;
            TBHlavni.SelectionLength = 0;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (File.Exists(AdresaSouboru))
            {
                string text = File.ReadAllText(AdresaSouboru, Encoding.UTF8);
                if (text != TBHlavni.Text)
                {
                    MessageBoxResult Vysledek = MessageBox.Show("Soubor byl změněn! Přejete si ho uložit?", "Neuložený soubor", MessageBoxButton.YesNo);
                    switch (Vysledek)
                    {
                        case MessageBoxResult.Yes:
                            try
                            {
                                File.WriteAllText(AdresaSouboru, TBHlavni.Text);
                            }
                            catch (Exception)
                            {
                            }
                            break;
                        case MessageBoxResult.No:
                            break;
                    }
                }
            }
            else
            {
                if (TBHlavni.Text != "")
                {
                    MessageBoxResult Vysledek = MessageBox.Show("Přejete si soubor uložit?", "Neuložený soubor", MessageBoxButton.YesNo);
                    switch (Vysledek)
                    {
                        case MessageBoxResult.Yes:
                            sfd.ShowDialog();
                            try
                            {
                                File.WriteAllText(sfd.FileName, TBHlavni.Text);
                            }
                            catch (Exception)
                            {
                            }
                            break;
                        case MessageBoxResult.No:
                            Application.Current.Shutdown();
                            break;
                    }
                }
            }
            Application.Current.Shutdown();
        }
        public bool panelVyhledat = false;
        private void TBHlavni_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TBHlavni.Text != TextPoNacteni) { Title = $"{nazevSouboruTitle} - změněno"; Ulozeno = false; }
            else { Title = nazevSouboruTitle; Ulozeno = true; }

            if (panelVyhledat)//TOHLE JE POTŘEBA PŘEVÉST NA NOVÝ VYHLEDÁVACÍ OKNO
            {
                pozice = 0;
                proslo = false;
            }
        }
        public bool proslo = false;
        public int pozice = 0;

        private void Window_StateChanged(object sender, EventArgs e)
        {
            panelOkno.Height = SystemParameters.PrimaryScreenHeight;
            panelOkno.Width = SystemParameters.PrimaryScreenWidth;
            TBHlavni.Width = panelOkno.Width - 15;
            TBHlavni.Height = panelOkno.Height - 55;
        }//NEFUNKČNÍ
        private void TBHlavni_KeyDown(object sender, KeyEventArgs e)
        {
            
                if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.F))
                {
                    NajdiVTextuOkno NVTO = new NajdiVTextuOkno(TBHlavni, proslo, pozice);
                    NVTO.Show();
                }
                else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.S))
                {
                    if (Title != "Nový textový soubor") File.WriteAllText(AdresaSouboru, TBHlavni.Text);
                    else
                    {
                        sfd.ShowDialog();
                        try
                        {
                            File.WriteAllText(sfd.FileName, TBHlavni.Text);
                            Title = sfd.FileName.Split(lomitko)[sfd.FileName.Split(lomitko).Length - 1];
                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.G))
                {
                    VymenTextOkno VTO = new VymenTextOkno(TBHlavni);
                    VTO.Show();
                }
                else if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.N))
                {
                    if (Ulozeno)
                    {
                        TBHlavni.Text = "";
                        Title = "Nový textový soubor";
                    }
                    else
                    {
                        MessageBoxResult mbr = MessageBox.Show("Tento soubor nebyl uložen! Přejete si ho uložit?", "Upozornění", MessageBoxButton.YesNo, MessageBoxImage.Information);
                        if (mbr == MessageBoxResult.Yes)
                        {
                            sfd.ShowDialog();
                            try
                            {
                                File.WriteAllText(sfd.FileName, TBHlavni.Text);
                                Ulozeno = true;
                            }
                            catch (Exception)
                            {
                            }
                        }
                        else
                        {
                            TBHlavni.Text = "";
                            Title = "Nový textový soubor";
                            Ulozeno = true;
                        }
                    }
                }
        }
    }
}
